using AddressBookSimple.Models;
using AddressBookSimple.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            _view.EditPerson += EditPerson;
        }

        public void AddPerson(object sender, EventArgs e)
        {
            newManageWindow = new ManagePerson(_addressBook);

            newManageWindow.ShowDialog();


            RefreshTable();
        }

        public void EditPerson(object sender, EditingPersonEventArgs e)
        {
            //Ordering the name selected for editing properly for grabbing the person information later on
            string unorderedName = e.PersonName;
            string[] nameArray = unorderedName.Split(',');
            string firstName = nameArray[1].Trim();
            string lastName = nameArray[0].Trim();

            Person person = _addressBook.getPerson(firstName, lastName);

            newManageWindow = new ManagePerson(_addressBook, person);

            newManageWindow.ShowDialog();

            RefreshTable();
        }

        public void RefreshTable()
        {
            
            List<string> personNames = new List<string>();
            string fullName = "";

            foreach (Person person in _addressBook.AddressBookList)
            {
                fullName = person.getFullName(person);
                personNames.Add(fullName);
            }

            _view.ListPersons = personNames;
        }
    }
}
