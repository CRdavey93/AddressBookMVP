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

        public MainFormPresenter(IMainForm view)
        {
            _view = view;
            _persons = new List<Person>();
            _view.AddPerson += AddPerson;
        }

        public void AddPerson(object sender, EventArgs e)
        {
            var person = new Person(_view.InputFirstName, _view.InputLastName, _view.InputPhoneNumber);

            _view.InputFirstName = null;
            _view.InputLastName = null;
            _view.InputPhoneNumber = null;

            _persons.Add(person);

            RefreshTable();
        }

        public void RefreshTable()
        {
            List<string> personNames = new List<string>();

            foreach (var person in _persons)
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
