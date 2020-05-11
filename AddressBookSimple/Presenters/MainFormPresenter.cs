using AddressBookSimple.Models;
using AddressBookSimple.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
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
        private static bool Continue = true;
        private static bool Dont_Continue = false;

        public MainFormPresenter(IMainForm view, AddressBook addressBook)
        {
            _view = view;
            _addressBook = addressBook;
            _persons = new List<Person>();

            _view.AddPerson += AddPerson;
            _view.EditPerson += EditPerson;
            _view.DeletePerson += DeletePerson;
            _view.ShowPersonInfo += ShowPersonInfo;

            _view.NewFile += NewFile;
            _view.OpenFile += OpenFile;
            _view.SaveFile += SaveFile;
            _view.SaveFileAs += SaveFileAs;
            _view.SortByName += SortByName;
            _view.SortByZip += SortByZip;

            _view.SetupTests += SetupTests;
        }

        public void AddPerson(object sender, EventArgs e)
        {
            newManageWindow = new ManagePerson(_addressBook);

            newManageWindow.ShowDialog();


            RefreshAddressBook();
        }

        public void EditPerson(object sender, PersonInfoEventArgs e)
        {
            if (_addressBook.AddressBookList.Any())
            {
                (string firstName, string lastName) = cleanUpName(e.PersonName);

                Person person = _addressBook.getPerson(firstName, lastName);

                newManageWindow = new ManagePerson(_addressBook, person);

                newManageWindow.ShowDialog();

                RefreshAddressBook();
            }

        }

        public void DeletePerson(object sender, PersonInfoEventArgs e)
        {
            if (_addressBook.AddressBookList.Any())
            {
                (string firstName, string lastName) = cleanUpName(e.PersonName);
                Person person = _addressBook.getPerson(firstName, lastName);

                _addressBook.AddressBookList.Remove(person);

                RefreshAddressBook();
            }
        }

        public void ShowPersonInfo(object sender, PersonInfoEventArgs e)
        {
            (string firstName, string lastName) = cleanUpName(e.PersonName);

            Person person = _addressBook.getPerson(firstName, lastName);

            string personInfo = "";

            foreach (PropertyInfo prop in person.GetType().GetProperties())
            {
                personInfo += prop.GetValue(person) + Environment.NewLine;
            }

            MessageBox.Show("Requested Info: " + Environment.NewLine + personInfo, "Person Information", MessageBoxButtons.OK);
        }

        //Event to create a new instance of AddressBook and then update the GUI List
        public void NewFile(object sender, EventArgs e)
        {
            if (_addressBook.ChangesToBeSaved)
            {
                if (offerToSave() != Continue)
                    return;
            }
            _addressBook = new AddressBook();
            RefreshAddressBook();
        }

        //Event to setup the open file function by starting a new dialog and getting the file to be opened
        public void OpenFile(object sender, EventArgs e)
        {
            if (_addressBook.ChangesToBeSaved)
            {
                if (offerToSave() != Continue)
                    return;
            }
            using (OpenFileDialog openFile = new OpenFileDialog())
            {
                openFile.Filter = "xml files |*.xml";
                openFile.FilterIndex = 2;

                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    openAddressBook(openFile.FileName);
                }
            }
        }

        //Helper method to deserialize the selected file and set the AddressBook instance to that file
        public void openAddressBook(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AddressBook));
            AddressBook tempAddressBook;
            using(TextReader textReader = new StreamReader(fileName))
            {
                tempAddressBook = (AddressBook)serializer.Deserialize(textReader);
            }

            setAddressBook(tempAddressBook);
        }

        //Event fired when Save is selected from the file menu
        public void SaveFile(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_addressBook.FileName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AddressBook));
                using (TextWriter textWriter = new StreamWriter(_addressBook.FileName))
                {
                    serializer.Serialize(textWriter, _addressBook);
                    textWriter.Close();
                }
                _addressBook.ChangesToBeSaved = false;
            }
            else
            {
                SaveFileAs(sender, e);
            }
        }

        //Event triggered when SaveAs is selected from file menu
        public void SaveFileAs(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();

            saveFile.Filter = "xml files |*.xml";
            saveFile.FilterIndex = 2;

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                _addressBook.FileName = saveFile.FileName;
                saveAddressBook(saveFile.FileName, _addressBook);
            }
        }

        //Helper Method for saving addressBook to an xml file
        public void saveAddressBook(string fileName, AddressBook addressBook)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AddressBook));
            using(TextWriter textWriter = new StreamWriter(fileName))
            {
                serializer.Serialize(textWriter, addressBook);
                textWriter.Close();
            }
            _addressBook.ChangesToBeSaved = false;
        }

        public bool offerToSave()
        {
            DialogResult dialogResult = MessageBox.Show("Changes have recently been made, would you like to save?", "Save", MessageBoxButtons.YesNoCancel);

            switch (dialogResult)
            {
                case DialogResult.Cancel:
                    return Dont_Continue;
                case DialogResult.Yes:
                    try
                    {
                        SaveFile(this, EventArgs.Empty);
                        return Continue;
                    }
                    catch (Exception)
                    {
                        throw;

                    }
                case DialogResult.No:
                    return Continue;
                default:
                    return Dont_Continue;
            }
        }

        private void SortByName(object sender, EventArgs e)
        {
            _addressBook.sortByName();
            _addressBook.SortedByName = true;
            _addressBook.SortedByZip = false;

            RefreshAddressBook();
        }

        private void SortByZip(object sender, EventArgs e)
        {
            _addressBook.sortByZip();
            _addressBook.SortedByZip = true;
            _addressBook.SortedByName = false;

            RefreshAddressBook();
        }

        //Helper method for taking the string passed from the main view and ordering it correctly while removing white space to return a usable first and last name.
        public (string firstName, string lastName) cleanUpName(string personName)
        {
            string[] nameArray = personName.Split(',');
            string firstName = nameArray[1].Trim();
            string lastName = nameArray[0].Trim();

            return (firstName, lastName);
        }

        //Helper method to set the instance of addressBook we are using when opening a file
        public void setAddressBook(AddressBook addressBook)
        {
            _addressBook = addressBook;
            RefreshAddressBook();
        }

        //Method to update the list of names shown on the main form
        public void RefreshAddressBook()
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

        public void SetupTests(object sender, EventArgs e)
        {
            _addressBook.setupTests();
            RefreshAddressBook();
        }
    }
}
