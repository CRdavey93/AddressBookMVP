using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AddressBookSimple.Models
{
    public class AddressBook
    {
        private static List<Person> addressBookList;
        private static bool changesToBeSaved = false;
        private static string fileName;

        public AddressBook()
        {
            addressBookList = new List<Person>();
            changesToBeSaved = false;
            fileName = "";
        }

        public void UpdateAddressBook(Person person)
        {
            addressBookList.Add(person);
            sortByName(addressBookList);
        }

        //Helper method to check if a person already exists in the address book with the given first and last name
        public bool doesPersonExist(Person person)
        {
            bool personExists = false;
            string firstName = person.FirstName;
            string lastName = person.LastName;

            foreach (Person item in addressBookList)
            {
                if (item.FirstName == firstName && item.LastName == lastName)
                {
                    personExists = true;
                }
                else
                {
                    personExists = false;
                }
            }
            return personExists;
        }

        //Helper method to return person object and it's info based on given first and last name
        public Person getPerson(string firstName, string lastName)
        {
            Person personObject = null;

            foreach (Person person in addressBookList)
            {
                if (person.FirstName == firstName && person.LastName == lastName)
                {
                    personObject = person;
                    break;
                }
            }

            return personObject;
        }

        public void sortByName(List<Person> personList)
        {
            personList.Sort(delegate (Person x, Person y)
            {
                if (x.LastName == y.LastName)
                    return x.FirstName.CompareTo(y.FirstName);

                return x.LastName.CompareTo(y.LastName);
            });

        }

        public void sortByZip(List<Person> personList)
        {
            personList.Sort((x, y) => string.Compare(x.Zip, y.Zip));

        }

        public List<Person> AddressBookList
        {
            get { return addressBookList; }
        }

        public bool ChangesToBeSaved
        {
            get => changesToBeSaved;
            set => changesToBeSaved = value;
        }

        public string FileName
        {
            get => fileName;
            set => fileName = value;
        }
    }
}
