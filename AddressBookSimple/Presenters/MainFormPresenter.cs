using AddressBookSimple.Models;
using AddressBookSimple.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

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
            _view.DeletePerson += DeletePerson;
            _view.SaveFileAs += SaveFileAs;
        }

        public void AddPerson(object sender, EventArgs e)
        {
            newManageWindow = new ManagePerson(_addressBook);

            newManageWindow.ShowDialog();


            RefreshTable();
        }

        public void EditPerson(object sender, EditingPersonEventArgs e)
        {
            (string firstName, string lastName) = cleanUpName(e.PersonName);

            Person person = _addressBook.getPerson(firstName, lastName);

            newManageWindow = new ManagePerson(_addressBook, person);

            newManageWindow.ShowDialog();

            RefreshTable();
        }

        public void DeletePerson(object sender, EditingPersonEventArgs e)
        {
            (string firstName, string lastName) = cleanUpName(e.PersonName);
            Person person = _addressBook.getPerson(firstName, lastName);

            _addressBook.AddressBookList.Remove(person);

            RefreshTable();
        }

        public void SaveFileAs(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();

            saveFile.Filter = "xml files |*.xml";
            saveFile.FilterIndex = 2;

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                saveAddressBook(saveFile.FileName, _addressBook);
            }
        }

        public void saveAddressBook(string fileName, AddressBook addressBook)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AddressBook));
            using(TextWriter textWriter = new StreamWriter(fileName))
            {
                serializer.Serialize(textWriter, addressBook);
                textWriter.Close();
            }
        }

        //Helper method for taking the string passed from the main view and ordering it correctly while removing white space to return a usable first and last name.
        public (string firstName, string lastName) cleanUpName(string personName)
        {
            string[] nameArray = personName.Split(',');
            string firstName = nameArray[1].Trim();
            string lastName = nameArray[0].Trim();

            return (firstName, lastName);
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
