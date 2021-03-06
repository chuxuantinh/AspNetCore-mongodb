using System.Collections.Generic;

namespace CustomFormatterDemo.Models
{
    public interface IContactRepository
    {
        void Add(Contact contact);
        IEnumerable<Contact> GetAll();
        Contact Get(string key);
        Contact Remove(string key);
        void Update(Contact contact);
    }
}