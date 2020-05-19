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
        private List<Person> addressBookList;
        private bool changesToBeSaved = false;
        private bool sortedByName = false;
        private bool sortedByZip = false;
        private string fileName;
        private string foundText;
        private Person personToFind = null;

        public AddressBook()
        {
            addressBookList = new List<Person>();
            changesToBeSaved = false;
            sortedByName = true;
            fileName = "";
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

        public string FoundText
        {
            get => foundText;
            set => foundText = value;
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

        public Person PersonToFind
        {
            get => personToFind;
            set => personToFind = value;
        }

        /*Method to update the address book during an add
         *@param person is the person that we want to add to the addressBook 
         */
        public void UpdateAddressBook(Person person)
        {
            addressBookList.Add(person);

        }

        /*Helper method to check if a person already exists in the address book with the given first and last name
         * @param person is the person that we want to check the existence of in the addressBook
         */
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

         //Helper method to call one sort type or the other depending on the current state of flags.
        public void sortAddressBook()
        {
            if (sortedByName)
                sortByName();
            else if (sortedByZip)
                sortByZip();
        }

        /*Helper method to return person object and it's info based on given first and last name.
         * @param firstName is the first name of the person we are getting.
         * @param lastName is the last name of the person we are getting.
         */
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

        /*Search the addressBook for a match to the text string we want to find. Search should start at the next entry after
        *the currently selected item.
        * @param textToFind is the text we have gotten from some user input and are now trying to find in some person property.
        * @param startSearchIndex is the location of the currently selected person. Search begins on the person after.
        */
        public void searchForText(string textToFind, int startSearchIndex)
        {
            string temp;
            bool found = false;

            foreach (Person item in addressBookList.Skip(startSearchIndex + 1))
            {
                foreach (var prop in item.GetType().GetProperties())
                {
                    if (prop.PropertyType == typeof(string))
                    {
                        temp = prop.GetValue(item).ToString();
                        if (temp.Contains(textToFind))
                        {
                            personToFind = item;
                            found = true;
                            foundText = textToFind;
                            break;
                        }
                    }
                }
                if (found)
                {
                    break;
                }
            }
        }

        //Method that sorts the addressList by name - last name then first name.
        public void sortByName()
        {
            addressBookList.Sort(delegate (Person x, Person y)
            {
                if (x.LastName == y.LastName)
                    return x.FirstName.CompareTo(y.FirstName);

                return x.LastName.CompareTo(y.LastName);
            });

        }

        //Method that sorts the addressList by zip then name.
        public void sortByZip()
        {
            addressBookList.Sort(delegate (Person x, Person y)
            {
                if (x.Zip == y.Zip)
                {
                    if (x.LastName == y.LastName)
                        return x.FirstName.CompareTo(y.FirstName);
                    return x.LastName.CompareTo(y.LastName);
                }
                return x.Zip.CompareTo(y.Zip);
            });


            //addressBookList.Sort((x, y) => string.Compare(x.Zip, y.Zip));

        }

        //Helper method for setting up tests
        public void addPerson(string firstName, string lastName, string address, string city, string state, string zip, string phone)
        {
            addressBookList.Add(new Person(firstName, lastName, address, city, state, zip, phone));
            changesToBeSaved = true;
        }

        //Quick test case setup
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
    }
}
