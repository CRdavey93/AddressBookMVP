using AddressBookSimple.Models;
using AddressBookSimple.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookSimple.Presenters
{
    public class MainFormPresenter
    {
        private readonly IMainForm _view;
        private readonly List<Person> _persons;
        private ManagePerson newManageWindow;
        private AddressBook _addressBook;
        public MainFormPresenter(IMainForm view, AddressBook addressBook)
        {
            _view = view;
            _addressBook = addressBook;
            _persons = new List<Person>();
            _view.AddPerson += AddPerson;
        }

        public void AddPerson(object sender, EventArgs e)
        {
            newManageWindow = new ManagePerson();

            newManageWindow.ShowDialog();

            //_persons.Add(person);

            RefreshTable();
        }

        public void RefreshTable()
        {
            
            List<string> personNames = new List<string>();

            foreach (Person person in _addressBook.AddressBookFoo)
            {
                personNames.Add(getFullName(person));
            }

            _view.ListPersons = personNames;
        }

        public string getFullName(Person person)
        {
            return person.LastName + ", " + person.FirstName;
        }
    }
}
