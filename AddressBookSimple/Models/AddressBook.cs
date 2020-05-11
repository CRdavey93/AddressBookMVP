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
        private static bool sortedByName = false;
        private static bool sortedByZip = false;
        private static string fileName;

        public AddressBook()
        {
            addressBookList = new List<Person>();
            changesToBeSaved = false;
            sortedByName = true;
            fileName = "";
        }

        public void UpdateAddressBook(Person person)
        {
            addressBookList.Add(person);

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

        public void sortAddressBook()
        {
            if (sortedByName)
                sortByName();
            else if (sortedByZip)
                sortByZip();
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

        public void sortByName()
        {
            addressBookList.Sort(delegate (Person x, Person y)
            {
                if (x.LastName == y.LastName)
                    return x.FirstName.CompareTo(y.FirstName);

                return x.LastName.CompareTo(y.LastName);
            });

        }

        public void sortByZip()
        {
            addressBookList.Sort((x, y) => string.Compare(x.Zip, y.Zip));

        }

        public void addPerson(string firstName, string lastName, string address, string city, string state, string zip, string phone)
        {
            addressBookList.Add(new Person(firstName, lastName, address, city, state, zip, phone));
            changesToBeSaved = true;
        }

        public void setupTests()
        {
            addPerson("Anthony", "Aardvark", "10 Skunk Hollow Lane", "Wenham",
                                 "MA", "01984", "927-2300");
            addPerson("Zelda", "Zebra", "5 Zoo Road", "Beverly",
                                 "MA", "01915", "927-0001");
            addPerson("George", "Gopher", "Tunnel 37", "Hamilton",
                                 "MA", "01936", "468-5555");
            addPerson("Clarence", "Cat", "127 Litter Box Ln", "Ipswich",
                                 "MA", "01938", "356-9999");
            addPerson("Charlene", "Cat", "127 Litter Box Ln", "Ipswich",
                                 "MA", "01938", "356-9999");
            addPerson("Boris", "Buffalo", "Town Common", "Hamilton",
                                 "MA", "01936", "468-5555");
            addPerson("Bertha", "Buffalo", "14 Grassy Fields Rd", "Wenham",
                                 "MA", "01984", "927-2300");
            addPerson("Maxwell", "Moose", "12 You Can't Get There From Here Rd",
                      "TAR2", "ME", "None", "None");
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

        public bool SortedByName
        {
            get => sortedByName;
            set => sortedByName = value;
        }

        public bool SortedByZip
        {
            get => sortedByZip;
            set => sortedByZip = value;
        }
    }
}
