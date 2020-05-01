using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookSimple.Models
{
    public class AddressBook
    {
        private static List<Person> addressBookFoo;

        public AddressBook()
        {
            addressBookFoo = new List<Person>();
        }

        public void UpdateAddressBook(Person person)
        {
            addressBookFoo.Add(person);
        }

        public List<Person> AddressBookFoo
        {
            get { return addressBookFoo; }
        }
    }
}
