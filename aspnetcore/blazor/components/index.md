---
title: Create and use ASP.NET Core Razor components
author: guardrex
description: Learn how to create and use Razor components, including how to bind to data, handle events, and manage component life cycles.
monikerRange: '>= aspnetcore-3.1'
ms.author: riande
ms.custom: mvc
ms.date: 11/25/2020
no-loc: [appsettings.json, "ASP.NET Core Identity", cookie, Cookie, Blazor, "Blazor Server", "Blazor WebAssembly", "Identity", "Let's Encrypt", Razor, SignalR]
uid: blazor/components/index
---
# Create and use ASP.NET Core Razor components

[View or download sample code](https://github.com/dotnet/AspNetCore.Docs/tree/main/aspnetcore/blazor/common/samples/) ([how to download](xref:index#how-to-download-a-sample))

Blazor apps are built using *components*. A component is a self-contained chunk of user interface (UI), such as a page, dialog, or form. A component includes HTML markup and the processing logic required to inject data or respond to UI events. Components are flexible and lightweight. They can be nested, reused, and shared among projects.

## Component classes

Components are implemented in [Razor](xref:mvc/views/razor) component files (`.razor`) using a combination of C# and HTML markup. A component in Blazor is formally referred to as a *Razor component*.

### Razor syntax

Razor components in Blazor apps extensively use Razor syntax. If you aren't familiar with the Razor markup language, we recommend reading <xref:mvc/views/razor> before proceeding.

When accessing the content on Razor syntax, pay special attention to the following sections:

* [Directives](xref:mvc/views/razor#directives): `@`-prefixed reserved keywords that typically change the way component markup is parsed or function.
* [Directive attributes](xref:mvc/views/razor#directive-attributes): `@`-prefixed reserved keywords that typically change the way component elements are parsed or function.

### Names

A component's name must start with an uppercase character. For example, `MyCoolComponent.razor` is valid, and `myCoolComponent.razor` is invalid.

### Routing

Routing in Blazor is achieved by providing a route template to each accessible component in the app. When a Razor file with an [`@page`][9] directive is compiled, the generated class is given a <xref:Microsoft.AspNetCore.Mvc.RouteAttribute> specifying the route template. At runtime, the router looks for component classes with a <xref:Microsoft.AspNetCore.Mvc.RouteAttribute> and renders whichever component has a route template that matches the requested URL. For more information, see <xref:blazor/fundamentals/routing>.

```razor
@page "/ParentComponent"

...
```

### Markup

The UI for a component is defined using HTML. Dynamic rendering logic (for example, loops, conditionals, expressions) is added using an embedded C# syntax called *Razor*. When an app is compiled, the HTML markup and C# rendering logic are converted into a component class. The name of the generated class matches the name of the file.

Members of the component class are defined in an [`@code`][1] block. In the [`@code`][1] block, component state (properties, fields) is specified with methods for event handling or for defining other component logic. More than one [`@code`][1] block is permissible.

Component members can be used as part of the component's rendering logic using C# expressions that start with `@`. For example, a C# field is rendered by prefixing `@` to the field name. The following example evaluates and renders:

* `headingFontStyle` to the CSS property value for `font-style`.
* `headingText` to the content of the `<h1>` element.

```razor
<h1 style="font-style:@headingFontStyle">@headingText</h1>

@code {
    private string headingFontStyle = "italic";
    private string headingText = "Put on your new Blazor!";
}
```

After the component is initially rendered, the component regenerates its render tree in response to events. Blazor then compares the new render tree against the previous one and applies any modifications to the browser's Document Object Model (DOM). Additional detail is provided in <xref:blazor/components/rendering>.

Components are ordinary C# classes and can be placed anywhere within a project. Components that produce webpages usually reside in the `Pages` folder. Non-page components are frequently placed in the `Shared` folder or a custom folder added to the project.

### Namespaces

Typically, a component's namespace is derived from the app's root namespace and the component's location (folder) within the app. If the app's root namespace is `BlazorSample` and the `Counter` component resides in the `Pages` folder:

* The `Counter` component's namespace is `BlazorSample.Pages`.
* The fully qualified type name of the component is `BlazorSample.Pages.Counter`.

For custom folders that hold components, add a [`@using`][2] directive to the parent component or to the app's `_Imports.razor` file. The following example makes components in the `Components` folder available:

```razor
@using BlazorSample.Components
```

Components can also be referenced using their fully qualified names, which doesn't require the [`@using`][2] directive:

```razor
<BlazorSample.Components.MyComponent />
```

The namespace of a component authored with Razor is based on (in priority order):

* [`@namespace`][8] designation in Razor file (`.razor`) markup (`@namespace BlazorSample.MyNamespace`).
* The project's `RootNamespace` in the project file (`<RootNamespace>BlazorSample</RootNamespace>`).
* The project name, taken from the project file's file name (`.csproj`), and the path from the project root to the component. For example, the framework resolves `{PROJECT ROOT}/Pages/Index.razor` (`BlazorSample.csproj`) to the namespace `BlazorSample.Pages`. Components follow C# name binding rules. For the `Index` component in this example, the components in scope are all of the components:
  * In the same folder, `Pages`.
  * The components in the project's root that don't explicitly specify a different namespace.

> [!NOTE]
> The `global::` qualification isn't supported.
>
> Importing components with aliased [`using`](/dotnet/csharp/language-reference/keywords/using-statement) statements (for example, `@using Foo = Bar`) isn't supported.
>
> Partially qualified names aren't supported. For example, adding `@using BlazorSample` and referencing the `NavMenu` component (`NavMenu.razor`) with `<Shared.NavMenu></Shared.NavMenu>` isn't supported.

### Partial class support

Razor components are generated as partial classes. Razor components are authored using either of the following approaches:

* C# code is defined in an [`@code`][1] block with HTML markup and Razor code in a single file. Blazor templates define their Razor components using this approach.
* C# code is placed in a code-behind file defined as a partial class.

The following example shows the default `Counter` component with an [`@code`][1] block in an app generated from a Blazor template. HTML markup, Razor code, and C# code are in the same file:

`Pages/Counter.razor`:

```razor
@page "/counter"

<h1>Counter</h1>

<p>Current count: @currentCount</p>

<button class="btn btn-primary" @onclick="IncrementCount">Click me</button>

@code {
    private int currentCount = 0;

    void IncrementCount()
    {
        currentCount++;
    }
}
```

The `Counter` component can also be created using a code-behind file with a partial class:

`Pages/Counter.razor`:

```razor
@page "/counter"

<h1>Counter</h1>

<p>Current count: @currentCount</p>

<button class="btn btn-primary" @onclick="IncrementCount">Click me</button>
```

`Counter.razor.cs`:

```csharp
namespace BlazorSample.Pages
{
    public partial class Counter
    {
        private int currentCount = 0;

        void IncrementCount()
        {
            currentCount++;
        }
    }
}
```

Add any required namespaces to the partial class file as needed. Typical namespaces used by Razor components include:

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
```

> [!IMPORTANT]
> [`@using`][2] directives in the `_Imports.razor` file are only applied to Razor files (`.razor`), not C# files (`.cs`).

### Specify a base class

The [`@inherits`][6] directive can be used to specify a base class for a component. The following example shows how a component can inherit a base class, `BlazorRocksBase`, to provide the component's properties and methods. The base class should derive from <xref:Microsoft.AspNetCore.Components.ComponentBase>.

`Pages/BlazorRocks.razor`:

```razor
@page "/BlazorRocks"
@inherits BlazorRocksBase

<h1>@BlazorRocksText</h1>
```

`BlazorRocksBase.cs`:

```csharp
using Microsoft.AspNetCore.Components;

namespace BlazorSample
{
    public class BlazorRocksBase : ComponentBase
    {
        public string BlazorRocksText { get; set; } = 
            "Blazor rocks the browser!";
    }
}
```

### Use components

Components can include other components by declaring them using HTML element syntax. The markup for using a component looks like an HTML tag where the name of the tag is the component type.

The following markup in `Pages/Index.razor` renders a `HeadingComponent` instance:

```razor
<HeadingComponent />
```

`Shared/HeadingComponent.razor`:

::: moniker range=">= aspnetcore-5.0"

[!code-razor[](~/blazor/common/samples/5.x/BlazorSample_WebAssembly/Pages/components-index/HeadingComponent.razor)]

::: moniker-end

::: moniker range="< aspnetcore-5.0"

[!code-razor[](~/blazor/common/samples/3.x/BlazorSample_WebAssembly/Pages/components-index/HeadingComponent.razor)]

::: moniker-end

If a component contains an HTML element with an uppercase first letter that doesn't match a component name, a warning is emitted indicating that the element has an unexpected name. Adding an [`@using`][2] directive for the component's namespace makes the component available, which resolves the warning.

## Parameters

### Component parameters

Components can have *component parameters*, which are defined using public simple or complex properties on the component class with the [`[Parameter]` attribute](xref:Microsoft.AspNetCore.Components.ParameterAttribute). Use attributes to specify arguments for a component in markup.

`Shared/ChildComponent.razor`:

```razor
<div class="panel panel-default">
    <div class="panel-heading">@Title</div>
    <div class="panel-body">@ChildContent</div>

    <button class="btn btn-primary" @onclick="OnClickCallback">
        Trigger a Parent component method
    </button>
</div>

@code {
    [Parameter]
    public string Title { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> OnClickCallback { get; set; }
}
```

Component parameters can be assigned a default value:

```csharp
[Parameter]
public string Title { get; set; } = "Panel Title from Child";
```

In the following example from the sample app, the `ParentComponent` sets the value of the `Title` property of the `ChildComponent`.

`Pages/ParentComponent.razor`:

```razor
@page "/ParentComponent"

<h1>Parent-child example</h1>

<ChildComponent Title="Panel Title from Parent"
                OnClickCallback="@ShowMessage">
    Content of the child component is supplied
    by the parent component.
</ChildComponent>
```

Assign C# fields, properties, and methods to component parameters as HTML attribute values using [Razor's reserved `@` symbol](xref:mvc/views/razor#razor-syntax):

* To assign a parent component's field, property, or method to a child component's parameter, prefix the field, property, or method name with the `@` symbol. To assign the result of an [implicit C# expression](xref:mvc/views/razor#implicit-razor-expressions) to a parameter, prefix the implicit expression with an `@` symbol.

  The following parent component displays four instances of the preceding `ChildComponent` component and sets their `Title` parameter values to:

  * The value of the `title` field.
  * The result of the `GetTitle` C# method.
  * The current local date in long format with <xref:System.DateTime.ToLongDateString%2A>.
  * The `person` object's `Name` property.

  `Pages/ParentComponent.razor`:

  ::: moniker range=">= aspnetcore-5.0"

  ```razor
  <ChildComponent Title="@title">
      Title from field.
  </ChildComponent>
  
  <ChildComponent Title="@GetTitle()">
      Title from method.
  </ChildComponent>
  
  <ChildComponent Title="@DateTime.Now.ToLongDateString()">
      Title from implicit Razor expression.
  </ChildComponent>
  
  <ChildComponent Title="@person.Name">
      Title from implicit Razor expression.
  </ChildComponent>
  
  @code {
      private string title = "Panel Title from Parent";
      private Person person = new();
      
      private string GetTitle()
      {
          return "Panel Title from Parent";
      }
      
      private class Person
      {
          public string Name { get; set; } = "Dr. Who";
      }
  }
  ```

  ::: moniker-end

  ::: moniker range="< aspnetcore-5.0"

  ```razor
  <ChildComponent Title="@title">
      Title from field.
  </ChildComponent>
  
  <ChildComponent Title="@GetTitle()">
      Title from method.
  </ChildComponent>
  
  <ChildComponent Title="@DateTime.Now.ToLongDateString()">
      Title from implicit Razor expression.
  </ChildComponent>
  
  <ChildComponent Title="@person.Name">
      Title from implicit Razor expression.
  </ChildComponent>
  
  @code {
      private string title = "Panel Title from Parent";
      private Person person = new Person();
      
      private string GetTitle()
      {
          return "Panel Title from Parent";
      }
      
      private class Person
      {
          public string Name { get; set; } = "Dr. Who";
      }
  }
  ```

  ::: moniker-end
  
  Unlike in Razor pages (`.cshtml`), Blazor can't perform asynchronous work in a Razor expression while rendering a component. This is because Blazor is designed for rendering interactive UIs. In an interactive UI, the screen must always display something, so it doesn't make sense to block the rendering flow. Instead, asynchronous work is performed during one of the [asynchronous lifecycle events](xref:blazor/components/lifecycle). After each asynchronous lifecycle event, the component may render again. The following Razor syntax is **not** supported:
  
  ```razor
  <ChildComponent Title="@await ...">
      ...
  </ChildComponent>
  ```
  
  The code in the preceding example generates a *compiler error* if the app is built:
  
  > The 'await' operator can only be used within an async method. Consider marking this method with the 'async' modifier and changing its return type to 'Task'.

  To obtain a value for the `Title` parameter in the preceding example asychronously, the component can use the [`OnInitializedAsync` lifecycle event](xref:blazor/components/lifecycle#component-initialization-oninitializedasync), as the following example demonstrates:
  
  ```razor
  <ChildComponent Title="@title">
      Title from implicit Razor expression.
  </ChildComponent>
  
  @code {
      private string title;
      
      protected override async Task OnInitializedAsync()
      {
          title = await ...;
      }
  }
  ```
  
* To assign the result of an [explicit C# expression](xref:mvc/views/razor#explicit-razor-expressions) in the parent component to a child component's parameter, surround the expression in parentheses with an `@` symbol prefix.

  The following child component has a <xref:System.DateTime> component parameter, `ShowItemsSinceDate`.
  
  `Shared/ChildComponent.razor`:
  
  ```razor
  <div class="panel panel-default">
      <div class="panel-heading">Explicit DateTime Expression Example</div>
      <div class="panel-body">
          <p>@ChildContent</p>
          <p>One week ago date: @ShowItemsSinceDate</p>
      </div>
  </div>

  @code {
      [Parameter]
      public DateTime ShowItemsSinceDate { get; set; }

      [Parameter]
      public RenderFragment ChildContent { get; set; }
  }
  ```
  
  The following parent component calculates a date with an explicit C# expression that's one week in the past for assignment to the child's `ShowItemsSinceDate` parameter.
  
  `Pages/ParentComponent.razor`:

  ```razor
  <ChildComponent ShowItemsSinceDate="@(DateTime.Now - TimeSpan.FromDays(7))">
      Title from explicit Razor expression.
  </ChildComponent>
  ```

  Use of an explicit expression to concatenate text with an expression result for assignment to a parameter is **not** supported. The following example seeks to concatenate the text "SKU-" with a product stock number (`SKU` property, "Stock Keeping Unit") provided by a parent component's `product` object. Although this syntax is supported in a Razor page (`.cshtml`), it isn't valid for assignment to the child's `Title` parameter.
  
  ```razor
  <ChildComponent Title="SKU-@(product.SKU)">
      Title from composed Razor expression. This doesn't compile.
  </ChildComponent>
  ```
  
  The code in the preceding example generates a *compiler error* if the app is built:
  
  > Component attributes do not support complex content (mixed C# and markup).
  
  To support the assignment of a composed value, use a method, field, or property. The following example performs the concatination of "SKU-" and a product's stock number in the C# method `GetTitle`:

  ::: moniker range=">= aspnetcore-5.0"

  ```razor
  <ChildComponent Title="@GetTitle()">
      Composed title from method.
  </ChildComponent>

  @code {
      private Product product = new();

      private string GetTitle() => $"SKU-{product.SKU}";
      
      private class Product
      {
          public string SKU { get; set; } = "12345";
      }
  }
  ```

  ::: moniker-end

  ::: moniker range="< aspnetcore-5.0"

  ```razor
  <ChildComponent Title="@GetTitle()">
      Composed title from method.
  </ChildComponent>

  @code {
      private Product product = new Product();

      private string GetTitle() => $"SKU-{product.SKU}";
      
      private class Product
      {
          public string SKU { get; set; } = "12345";
      }
  }
  ```

  ::: moniker-end

For more information, see <xref:mvc/views/razor>.

> [!WARNING]
> Don't create components that write to their own *component parameters*, use a private field instead. For more information, see the [Overwritten parameters](#overwritten-parameters) section.

#### Component parameters should be auto-properties

Component parameters should be declared as *auto-properties*, meaning that they shouldn't contain custom logic in their getters or setters. For example, the following `StartData` property is an auto-property:

```csharp
[Parameter]
public DateTime StartData { get; set; }
```

Don't place custom logic in the `get` or `set` accessor because component parameters are purely intended for use as a channel for a parent component to flow information to a child component. If a setter of a child component property contains logic that causes rerendering of the parent component, an infinite rendering loop results.

If you need to transform a received parameter value:

* Leave the parameter property as a pure auto-property to represent the supplied raw data.
* Create some other property or method that supplies the transformed data based on the parameter property.

You can override [`OnParametersSetAsync`](xref:blazor/components/lifecycle#after-parameters-are-set-onparameterssetasync) if you want to transform a received parameter each time new data is received.

### Route parameters

Components can receive route parameters from the route template provided in the [`@page`][9] directive. The router uses route parameters to populate the corresponding component parameters.

::: moniker range=">= aspnetcore-5.0"

Optional parameters are supported. In the following example, the `text` optional parameter assigns the value of the route segment to the component's `Text` property. If the segment isn't present, the value of `Text` is set to `fantastic`.

`Pages/RouteParameter.razor`:

[!code-razor[](~/blazor/common/samples/5.x/BlazorSample_WebAssembly/Pages/components-index/RouteParameter.razor?highlight=1,6-7)]

::: moniker-end

::: moniker range="< aspnetcore-5.0"

`Pages/RouteParameter.razor`:

[!code-razor[](~/blazor/common/samples/3.x/BlazorSample_WebAssembly/Pages/components-index/RouteParameter.razor?highlight=2,7-8)]

Optional parameters aren't supported, so two [`@page`][9] directives are applied in the preceding example. The first permits navigation to the component without a parameter. The second [`@page`][9] directive receives the `{text}` route parameter and assigns the value to the `Text` property.

::: moniker-end

For information on catch-all route parameters (`{*pageRoute}`), which capture paths across multiple folder boundaries, see <xref:blazor/fundamentals/routing#catch-all-route-parameters>.

## Child content

Components can set the content of another component. The assigning component provides the content between the tags that specify the receiving component.

In the following example, the `ChildComponent` has a `ChildContent` property that represents a <xref:Microsoft.AspNetCore.Components.RenderFragment>, which represents a segment of UI to render. The value of `ChildContent` is positioned in the component's markup where the content should be rendered. The value of `ChildContent` is received from the parent component and rendered inside the Bootstrap panel's `panel-body`.

`Shared/ChildComponent.razor`:

```razor
<div class="panel panel-default">
    <div class="panel-heading">@Title</div>
    <div class="panel-body">@ChildContent</div>

    <button class="btn btn-primary" @onclick="OnClickCallback">
        Trigger a Parent component method
    </button>
</div>

@code {
    [Parameter]
    public string Title { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> OnClickCallback { get; set; }
}
```

> [!NOTE]
> The property receiving the <xref:Microsoft.AspNetCore.Components.RenderFragment> content must be named `ChildContent` by convention.

The `ParentComponent` in the sample app can provide content for rendering the `ChildComponent` by placing the content inside the `<ChildComponent>` tags.

`Pages/ParentComponent.razor`:

```razor
@page "/ParentComponent"

<h1>Parent-child example</h1>

<ChildComponent Title="Panel Title from Parent"
                OnClickCallback="@ShowMessage">
    Content of the child component is supplied
    by the parent component.
</ChildComponent>
```

Due to the way that Blazor renders child content, rendering components inside a `for` loop requires a local index variable if the incrementing loop variable is used in the child component's content:
>
> ```razor
> @for (int c = 0; c < 10; c++)
> {
>     var current = c;
>     <ChildComponent Title="@c">
>         Child Content: Count: @current
>     </ChildComponent>
> }
> ```
>
> Alternatively, use a `foreach` loop with <xref:System.Linq.Enumerable.Range%2A?displayProperty=nameWithType>:
>
> ```razor
> @foreach(var c in Enumerable.Range(0,10))
> {
>     <ChildComponent Title="@c">
>         Child Content: Count: @c
>     </ChildComponent>
> }
> ```

For information on how a <xref:Microsoft.AspNetCore.Components.RenderFragment> can be used as a template for Razor component UI, see the following articles:

* <xref:blazor/components/templated-components>
* <xref:blazor/webassembly-performance-best-practices#define-reusable-renderfragments-in-code>

## Attribute splatting and arbitrary parameters

Components can capture and render additional attributes in addition to the component's declared parameters. Additional attributes can be captured in a dictionary and then *splatted* onto an element when the component is rendered using the [`@attributes`][3] Razor directive. This scenario is useful when defining a component that produces a markup element that supports a variety of customizations. For example, it can be tedious to define attributes separately for an `<input>` that supports many parameters.

In the following example, the first `<input>` element (`id="useIndividualParams"`) uses individual component parameters, while the second `<input>` element (`id="useAttributesDict"`) uses attribute splatting:

```razor
<input id="useIndividualParams"
       maxlength="@maxlength"
       placeholder="@placeholder"
       required="@required"
       size="@size" />

<input id="useAttributesDict"
       @attributes="InputAttributes" />

@code {
    public string maxlength = "10";
    public string placeholder = "Input placeholder text";
    public string required = "required";
    public string size = "50";

    public Dictionary<string, object> InputAttributes { get; set; } =
        new Dictionary<string, object>()
        {
            { "maxlength", "10" },
            { "placeholder", "Input placeholder text" },
            { "required", "required" },
            { "size", "50" }
        };
}
```

The type of the parameter must implement `IEnumerable<KeyValuePair<string, object>>` or `IReadOnlyDictionary<string, object>` with string keys.

The rendered `<input>` elements using both approaches is identical:

```html
<input id="useIndividualParams"
       maxlength="10"
       placeholder="Input placeholder text"
       required="required"
       size="50">

<input id="useAttributesDict"
       maxlength="10"
       placeholder="Input placeholder text"
       required="required"
       size="50">
```

To accept arbitrary attributes, define a component parameter using the [`[Parameter]` attribute](xref:Microsoft.AspNetCore.Components.ParameterAttribute) with the <xref:Microsoft.AspNetCore.Components.ParameterAttribute.CaptureUnmatchedValues> property set to `true`:

```razor
@code {
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> InputAttributes { get; set; }
}
```

The <xref:Microsoft.AspNetCore.Components.ParameterAttribute.CaptureUnmatchedValues> property on [`[Parameter]`](xref:Microsoft.AspNetCore.Components.ParameterAttribute) allows the parameter to match all attributes that don't match any other parameter. A component can only define a single parameter with <xref:Microsoft.AspNetCore.Components.ParameterAttribute.CaptureUnmatchedValues>. The property type used with <xref:Microsoft.AspNetCore.Components.ParameterAttribute.CaptureUnmatchedValues> must be assignable from `Dictionary<string, object>` with string keys. `IEnumerable<KeyValuePair<string, object>>` or `IReadOnlyDictionary<string, object>` are also options in this scenario.

The position of [`@attributes`][3] relative to the position of element attributes is important. When [`@attributes`][3] are splatted on the element, the attributes are processed from right to left (last to first). Consider the following example of a component that consumes a `Child` component:

`ParentComponent.razor`:

```razor
<ChildComponent extra="10" />
```

`ChildComponent.razor`:

```razor
<div @attributes="AdditionalAttributes" extra="5" />

[Parameter(CaptureUnmatchedValues = true)]
public IDictionary<string, object> AdditionalAttributes { get; set; }
```

The `Child` component's `extra` attribute is set to the right of [`@attributes`][3]. The `Parent` component's rendered `<div>` contains `extra="5"` when passed through the additional attribute because the attributes are processed right to left (last to first):

```html
<div extra="5" />
```

In the following example, the order of `extra` and [`@attributes`][3] is reversed in the `Child` component's `<div>`:

`ParentComponent.razor`:

```razor
<ChildComponent extra="10" />
```

`ChildComponent.razor`:

```razor
<div extra="5" @attributes="AdditionalAttributes" />

[Parameter(CaptureUnmatchedValues = true)]
public IDictionary<string, object> AdditionalAttributes { get; set; }
```

The rendered `<div>` in the `Parent` component contains `extra="10"` when passed through the additional attribute:

```html
<div extra="10" />
```

## Capture references to components

Component references provide a way to reference a component instance so that you can issue commands to that instance, such as `Show` or `Reset`. To capture a component reference:

* Add an [`@ref`][4] attribute to the child component.
* Define a field with the same type as the child component.

```razor
<CustomLoginDialog @ref="loginDialog" ... />

@code {
    private CustomLoginDialog loginDialog;

    private void OnSomething()
    {
        loginDialog.Show();
    }
}
```

When the component is rendered, the `loginDialog` field is populated with the `CustomLoginDialog` child component instance. You can then invoke .NET methods on the component instance.

> [!IMPORTANT]
> The `loginDialog` variable is only populated after the component is rendered and its output includes the `MyLoginDialog` element. Until the component is rendered, there's nothing to reference.
>
> To manipulate components references after the component has finished rendering, use the [`OnAfterRender` or `OnAfterRenderAsync` methods](xref:blazor/components/lifecycle#after-component-render-onafterrenderasync).
>
> To use a reference variable with an event handler, use a lambda expression or assign the event handler delegate in the [`OnAfterRender` or `OnAfterRenderAsync` methods](xref:blazor/components/lifecycle#after-component-render-onafterrenderasync). This ensures that the reference variable is assigned before the event handler is assigned.
>
> ```razor
> <button type="button" 
>     @onclick="@(() => loginDialog.DoSomething())">Do Something</button>
>
> <MyLoginDialog @ref="loginDialog" ... />
>
> @code {
>     private MyLoginDialog loginDialog;
> }
> ```

To reference components in a loop, see [Capture references to multiple similar child-components (dotnet/aspnetcore #13358)](https://github.com/dotnet/aspnetcore/issues/13358).

While capturing component references use a similar syntax to [capturing element references](xref:blazor/call-javascript-from-dotnet#capture-references-to-elements), it isn't a JavaScript interop feature. Component references aren't passed to JavaScript code. Component references are only used in .NET code.

> [!NOTE]
> Do **not** use component references to mutate the state of child components. Instead, use normal declarative parameters to pass data to child components. Use of normal declarative parameters result in child components that rerender at the correct times automatically.

## Synchronization context

Blazor uses a synchronization context (<xref:System.Threading.SynchronizationContext>) to enforce a single logical thread of execution. A component's [lifecycle methods](xref:blazor/components/lifecycle) and any event callbacks that are raised by Blazor are executed on the synchronization context.

Blazor Server's synchronization context attempts to emulate a single-threaded environment so that it closely matches the WebAssembly model in the browser, which is single threaded. At any given point in time, work is performed on exactly one thread, giving the impression of a single logical thread. No two operations execute concurrently.

### Avoid thread-blocking calls

Generally, don't call the following methods. The following methods block the thread and thus block the app from resuming work until the underlying <xref:System.Threading.Tasks.Task> is complete:

* <xref:System.Threading.Tasks.Task%601.Result%2A>
* <xref:System.Threading.Tasks.Task.Wait%2A>
* <xref:System.Threading.Tasks.Task.WaitAny%2A>
* <xref:System.Threading.Tasks.Task.WaitAll%2A>
* <xref:System.Threading.Thread.Sleep%2A>
* <xref:System.Runtime.CompilerServices.TaskAwaiter.GetResult%2A>

### Invoke component methods externally to update state

In the event a component must be updated based on an external event, such as a timer or other notifications, use the `InvokeAsync` method, which dispatches to Blazor's synchronization context. For example, consider a *notifier service* that can notify any listening component of the updated state:

```csharp
public class NotifierService
{
    // Can be called from anywhere
    public async Task Update(string key, int value)
    {
        if (Notify != null)
        {
            await Notify.Invoke(key, value);
        }
    }

    public event Func<string, int, Task> Notify;
}
```

Register the `NotifierService`:

* In Blazor WebAssembly, register the service as singleton in `Program.Main`:

  ```csharp
  builder.Services.AddSingleton<NotifierService>();
  ```

* In Blazor Server, register the service as scoped in `Startup.ConfigureServices`:

  ```csharp
  services.AddScoped<NotifierService>();
  ```

Use the `NotifierService` to update a component:

```razor
@page "/"
@inject NotifierService Notifier
@implements IDisposable

<p>Last update: @lastNotification.key = @lastNotification.value</p>

@code {
    private (string key, int value) lastNotification;

    protected override void OnInitialized()
    {
        Notifier.Notify += OnNotify;
    }

    public async Task OnNotify(string key, int value)
    {
        await InvokeAsync(() =>
        {
            lastNotification = (key, value);
            StateHasChanged();
        });
    }

    public void Dispose()
    {
        Notifier.Notify -= OnNotify;
    }
}
```

In the preceding example:

* `NotifierService` invokes the component's `OnNotify` method outside of Blazor's synchronization context. `InvokeAsync` is used to switch to the correct context and queue a render. For more information, see <xref:blazor/components/rendering>.
* The component implements <xref:System.IDisposable>, and the `OnNotify` delegate is unsubscribed in the `Dispose` method, which is called by the framework when the component is disposed. For more information, see <xref:blazor/components/lifecycle#component-disposal-with-idisposable>.

## Use \@key to control the preservation of elements and components

When rendering a list of elements or components and the elements or components subsequently change, Blazor's diffing algorithm must decide which of the previous elements or components can be retained and how model objects should map to them. Normally, this process is automatic and can be ignored, but there are cases where you may want to control the process.

Consider the following example:

```csharp
@foreach (var person in People)
{
    <DetailsEditor Details="@person.Details" />
}

@code {
    [Parameter]
    public IEnumerable<Person> People { get; set; }
}
```

The contents of the `People` collection may change with inserted, deleted, or re-ordered entries. When the component rerenders, the `<DetailsEditor>` component may change to receive different `Details` parameter values. This may cause more complex rerendering than expected. In some cases, rerendering can lead to visible behavior differences, such as lost element focus.

The mapping process can be controlled with the [`@key`][5] directive attribute. [`@key`][5] causes the diffing algorithm to guarantee preservation of elements or components based on the key's value:

```csharp
@foreach (var person in People)
{
    <DetailsEditor @key="person" Details="@person.Details" />
}

@code {
    [Parameter]
    public IEnumerable<Person> People { get; set; }
}
```

When the `People` collection changes, the diffing algorithm retains the association between `<DetailsEditor>` instances and `person` instances:

* If a `Person` is deleted from the `People` list, only the corresponding `<DetailsEditor>` instance is removed from the UI. Other instances are left unchanged.
* If a `Person` is inserted at some position in the list, one new `<DetailsEditor>` instance is inserted at that corresponding position. Other instances are left unchanged.
* If `Person` entries are re-ordered, the corresponding `<DetailsEditor>` instances are preserved and re-ordered in the UI.

In some scenarios, use of [`@key`][5] minimizes the complexity of rerendering and avoids potential issues with stateful parts of the DOM changing, such as focus position.

> [!IMPORTANT]
> Keys are local to each container element or component. Keys aren't compared globally across the document.

### When to use \@key

Typically, it makes sense to use [`@key`][5] whenever a list is rendered (for example, in a [foreach](/dotnet/csharp/language-reference/keywords/foreach-in) block) and a suitable value exists to define the [`@key`][5].

You can also use [`@key`][5] to prevent Blazor from preserving an element or component subtree when an object changes:

```razor
<div @key="currentPerson">
    ... content that depends on currentPerson ...
</div>
```

If `@currentPerson` changes, the [`@key`][5] attribute directive forces Blazor to discard the entire `<div>` and its descendants and rebuild the subtree within the UI with new elements and components. This can be useful if you need to guarantee that no UI state is preserved when `@currentPerson` changes.

### When not to use \@key

There's a performance cost when diffing with [`@key`][5]. The performance cost isn't large, but only specify [`@key`][5] if controlling the element or component preservation rules benefit the app.

Even if [`@key`][5] isn't used, Blazor preserves child element and component instances as much as possible. The only advantage to using [`@key`][5] is control over *how* model instances are mapped to the preserved component instances, instead of the diffing algorithm selecting the mapping.

### What values to use for \@key

Generally, it makes sense to supply one of the following kinds of value for [`@key`][5]:

* Model object instances (for example, a `Person` instance as in the earlier example). This ensures preservation based on object reference equality.
* Unique identifiers (for example, primary key values of type `int`, `string`, or `Guid`).

Ensure that values used for [`@key`][5] don't clash. If clashing values are detected within the same parent element, Blazor throws an exception because it can't deterministically map old elements or components to new elements or components. Only use distinct values, such as object instances or primary key values.

## Overwritten parameters

The Blazor framework generally imposes safe parent-to-child parameter assignment:

* Parameters aren't overwritten unexpectedly.
* Side-effects are minimized. For example, additional renders are avoided because they may create infinite rendering loops.

A child component receives new parameter values that possibly overwrite existing values when the parent component rerenders. Accidentally overwriting parameter values in a child component often occurs when developing the component with one or more data-bound parameters and the developer writes directly to a parameter in the child:

* The child component is rendered with one or more parameter values from the parent component.
* The child writes directly to the value of a parameter.
* The parent component rerenders and overwrites the value of the child's parameter.

The potential for overwriting parameter values extends into the child component's property setters, too.

**Our general guidance is not to create components that directly write to their own parameters.**

Consider the following faulty `Expander` component that:

* Renders child content.
* Toggles showing child content with a component parameter (`Expanded`).
* The component writes directly to the `Expanded` parameter, which demonstrates the problem with overwritten parameters and should be avoided.

```razor
<div @onclick="Toggle" class="card bg-light mb-3" style="width:30rem">
    <div class="card-body">
        <h2 class="card-title">Toggle (<code>Expanded</code> = @Expanded)</h2>

        @if (Expanded)
        {
            <p class="card-text">@ChildContent</p>
        }
    </div>
</div>

@code {
    [Parameter]
    public bool Expanded { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    private void Toggle()
    {
        Expanded = !Expanded;
    }
}
```

The `Expander` component is added to a parent component that may call <xref:Microsoft.AspNetCore.Components.ComponentBase.StateHasChanged%2A>:

```razor
@page "/expander"

<Expander Expanded="true">
    Expander 1 content
</Expander>

<Expander Expanded="true" />

<button @onclick="StateHasChanged">
    Call StateHasChanged
</button>
```

Initially, the `Expander` components behave independently when their `Expanded` properties are toggled. The child components maintain their states as expected. When <xref:Microsoft.AspNetCore.Components.ComponentBase.StateHasChanged%2A> is called in the parent, the `Expanded` parameter of the first child component is reset back to its initial value (`true`). The second `Expander` component's `Expanded` value isn't reset because no child content is rendered in the second component.

To maintain state in the preceding scenario, use a *private field* in the `Expander` component to maintain its toggled state.

The following revised `Expander` component:

* Accepts the `Expanded` component parameter value from the parent.
* Assigns the component parameter value to a *private field* (`expanded`) in the [`OnInitialized` event](xref:blazor/components/lifecycle#component-initialization-oninitializedasync).
* Uses the private field to maintain its internal toggle state, which demonstrates how to avoid writing directly to a parameter.

```razor
<div @onclick="Toggle" class="card bg-light mb-3" style="width:30rem">
    <div class="card-body">
        <h2 class="card-title">Toggle (<code>expanded</code> = @expanded)</h2>

        @if (expanded)
        {
            <p class="card-text">@ChildContent</p>
        }
    </div>
</div>

@code {
    private bool expanded;

    [Parameter]
    public bool Expanded { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    protected override void OnInitialized()
    {
        expanded = Expanded;
    }

    private void Toggle()
    {
        expanded = !expanded;
    }
}
```

For additional information, see [Blazor Two Way Binding Error (dotnet/aspnetcore #24599)](https://github.com/dotnet/aspnetcore/issues/24599). 

## Apply an attribute

Attributes can be applied to Razor components with the [`@attribute`][7] directive. The following example applies the [`[Authorize]` attribute](xref:Microsoft.AspNetCore.Authorization.AuthorizeAttribute) to the component class:

```razor
@page "/"
@attribute [Authorize]
```

## Conditional HTML element attributes

HTML element attributes are conditionally rendered based on the .NET value. If the value is `false` or `null`, the attribute isn't rendered. If the value is `true`, the attribute is rendered minimized.

In the following example, `IsCompleted` determines if `checked` is rendered in the element's markup:

```razor
<input type="checkbox" checked="@IsCompleted" />

@code {
    [Parameter]
    public bool IsCompleted { get; set; }
}
```

If `IsCompleted` is `true`, the check box is rendered as:

```html
<input type="checkbox" checked />
```

If `IsCompleted` is `false`, the check box is rendered as:

```html
<input type="checkbox" />
```

For more information, see <xref:mvc/views/razor>.

> [!WARNING]
> Some HTML attributes, such as [`aria-pressed`](https://developer.mozilla.org/docs/Web/Accessibility/ARIA/Roles/button_role#Toggle_buttons), don't function properly when the .NET type is a `bool`. In those cases, use a `string` type instead of a `bool`.

## Raw HTML

Strings are normally rendered using DOM text nodes, which means that any markup they may contain is ignored and treated as literal text. To render raw HTML, wrap the HTML content in a `MarkupString` value. The value is parsed as HTML or SVG and inserted into the DOM.

> [!WARNING]
> Rendering raw HTML constructed from any untrusted source is a **security risk** and should be avoided!

The following example shows using the `MarkupString` type to add a block of static HTML content to the rendered output of a component:

```html
@((MarkupString)myMarkup)

@code {
    private string myMarkup = 
        "<p class='markup'>This is a <em>markup string</em>.</p>";
}
```

## Razor templates

Render fragments can be defined using Razor template syntax. Razor templates are a way to define a UI snippet and assume the following format:

```razor
@<{HTML tag}>...</{HTML tag}>
```

The following example illustrates how to specify <xref:Microsoft.AspNetCore.Components.RenderFragment> and <xref:Microsoft.AspNetCore.Components.RenderFragment%601> values and render templates directly in a component. Render fragments can also be passed as arguments to [templated components](xref:blazor/components/templated-components).

```razor
@timeTemplate

@petTemplate(new Pet { Name = "Rex" })

@code {
    private RenderFragment timeTemplate = @<p>The time is @DateTime.Now.</p>;
    private RenderFragment<Pet> petTemplate = (pet) => @<p>Pet: @pet.Name</p>;

    private class Pet
    {
        public string Name { get; set; }
    }
}
```

Rendered output of the preceding code:

```html
<p>The time is 10/04/2018 01:26:52.</p>

<p>Pet: Rex</p>
```

## Static assets

Blazor follows the convention of ASP.NET Core apps placing static assets under the project's [`web root (wwwroot)` folder](xref:fundamentals/index#web-root).

Use a base-relative path (`/`) to refer to the web root for a static asset. In the following example, `logo.png` is physically located in the `{PROJECT ROOT}/wwwroot/images` folder:

```razor
<img alt="Company logo" src="/images/logo.png" />
```

Razor components do **not** support tilde-slash notation (`~/`).

For information on setting an app's base path, see <xref:blazor/host-and-deploy/index#app-base-path>.

## Tag Helpers aren't supported in components

[`Tag Helpers`](xref:mvc/views/tag-helpers/intro) aren't supported in Razor components (`.razor` files). To provide Tag Helper-like functionality in Blazor, create a component with the same functionality as the Tag Helper and use the component instead.

## Scalable Vector Graphics (SVG) images

Since Blazor renders HTML, browser-supported images, including Scalable Vector Graphics (SVG) images (`.svg`), are supported via the `<img>` tag:

```html
<img alt="Example image" src="some-image.svg" />
```

Similarly, SVG images are supported in the CSS rules of a stylesheet file (`.css`):

```css
.my-element {
    background-image: url("some-image.svg");
}
```

However, inline SVG markup isn't supported in all scenarios. If you place an `<svg>` tag directly into a component file (`.razor`), basic image rendering is supported but many advanced scenarios aren't yet supported. For example, `<use>` tags aren't currently respected, and [`@bind`][10] can't be used with some SVG tags. For more information, see [SVG support in Blazor (dotnet/aspnetcore #18271)](https://github.com/dotnet/aspnetcore/issues/18271).

## Whitespace rendering behavior

::: moniker range=">= aspnetcore-5.0"

Unless the [`@preservewhitespace`](xref:mvc/views/razor#preservewhitespace) directive is used with a value of `true`, extra whitespace is removed by default if:

* Leading or trailing within an element.
* Leading or trailing within a `RenderFragment` parameter. For example, child content passed to another component.
* It precedes or follows a C# code block, such as `@if` or `@foreach`.

Whitespace removal might affect the rendered output when using a CSS rule, such as `white-space: pre`. To disable this performance optimization and preserve the whitespace, take one of the following actions:

* Add the `@preservewhitespace true` directive at the top of the `.razor` file to apply the preference to a specific component.
* Add the `@preservewhitespace true` directive inside an `_Imports.razor` file to apply the preference to an entire subdirectory or the entire project.

In most cases, no action is required, as apps typically continue to behave normally (but faster). If stripping whitespace causes any problem for a particular component, use `@preservewhitespace true` in that component to disable this optimization.

::: moniker-end

::: moniker range="< aspnetcore-5.0"

Whitespace is retained in a component's source code. Whitespace-only text renders in the browser's Document Object Model (DOM) even when there's no visual effect.

Consider the following Razor component code:

```razor
<ul>
    @foreach (var item in Items)
    {
        <li>
            @item.Text
        </li>
    }
</ul>
```

The preceding example renders the following unnecessary whitespace:

* Outside of the `@foreach` code block.
* Around the `<li>` element.
* Around the `@item.Text` output.

A list containing 100 items results in 402 areas of whitespace, and none of the extra whitespace visually affects the rendered output.

When rendering static HTML for components, whitespace inside a tag isn't preserved. For example, view the source of the following component in rendered output:

```razor
<img     alt="My image"   src="img.png"     />
```

Whitespace isn't preserved from the preceding Razor markup:

```razor
<img alt="My image" src="img.png" />
```

::: moniker-end

## Additional resources

* <xref:blazor/security/server/threat-mitigation>: Includes guidance on building Blazor Server apps that must contend with resource exhaustion.

<!--Reference links in article-->
[1]: <xref:mvc/views/razor#code>
[2]: <xref:mvc/views/razor#using>
[3]: <xref:mvc/views/razor#attributes>
[4]: <xref:mvc/views/razor#ref>
[5]: <xref:mvc/views/razor#key>
[6]: <xref:mvc/views/razor#inherits>
[7]: <xref:mvc/views/razor#attribute>
[8]: <xref:mvc/views/razor#namespace>
[9]: <xref:mvc/views/razor#page>
[10]: <xref:mvc/views/razor#bind>
