using AddressBookSimple.Models;
using AddressBookSimple.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Channels;
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
        private const bool Continue = true;
        private const bool Dont_Continue = false;
        private static int personIndex = 0;
        private ManagePerson newManageWindow;
        private FindTextForm newFindTextWindow;
        private AddressBook _addressBook;

        /* Constructor
         *  @param view the form that this presenter subscribes to and updates
         *  @param addressBook the object to use for interacting with the address book
         */
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

            _view.FindPerson += FindPerson;
            _view.FindPersonAgain += FindPersonAgain;

            _view.SetupTests += SetupTests;
        }

        //Event fired when Add is clicked on the Main Form
        public void AddPerson(object sender, EventArgs e)
        {
            newManageWindow = new ManagePerson(_addressBook);

            newManageWindow.ShowDialog();

            //If not canceled and FindAgainButtonEnabled do _view.DisableFindAgainButton();
            RefreshAddressBook();
        }

        //Event fired when Edit is clicked on the Main Form
        public void EditPerson(object sender, PersonInfoEventArgs e)
        {
            if (_addressBook.AddressBookList.Any())
            {
                (string firstName, string lastName) = cleanUpName(e.PersonName);
                Person person = _addressBook.getPerson(firstName, lastName);

                newManageWindow = new ManagePerson(_addressBook, person);
                newManageWindow.ShowDialog();

                //If not canceled and FindAgainButtonEnabled do _view.DisableFindAgainButton();
                RefreshAddressBook();

                personIndex = findFocusIndex(person);
                setListFocus(personIndex);

            }

        }

        //Event fired when delete is selected from the main form
        public void DeletePerson(object sender, PersonInfoEventArgs e)
        {
            if (_addressBook.AddressBookList.Any())
            {
                (string firstName, string lastName) = cleanUpName(e.PersonName);
                DialogResult dialogResult = MessageBox.Show("Are you sure you would like to delete this person?", "Delete", MessageBoxButtons.YesNoCancel);

                switch (dialogResult)
                {
                    case DialogResult.Cancel:
                        break;
                    case DialogResult.Yes:
                        Person person = _addressBook.getPerson(firstName, lastName);
                        personIndex = findDeleteFocusIndex(person);
                        _addressBook.AddressBookList.Remove(person);
                        //If not canceled and FindAgainButtonEnabled do _view.DisableFindAgainButton();
                        RefreshAddressBook();
                        setListFocus(personIndex);
                        break;
                    case DialogResult.No:
                        break;
                    default:
                        break;
                }
            }
        }

        //Event fired when a persons name is double clicked in the main form
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
            RefreshAddressBook();
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

        //Event fired when sort by name is selected from the sort menu
        public void SortByName(object sender, EventArgs e)
        {
            _addressBook.sortByName();
            _addressBook.SortedByName = true;
            _addressBook.SortedByZip = false;

            RefreshAddressBook();
        }

        //Event fired when sort by zip is selected from the sort menu
        public void SortByZip(object sender, EventArgs e)
        {
            _addressBook.sortByZip();
            _addressBook.SortedByZip = true;
            _addressBook.SortedByName = false;

            RefreshAddressBook();
        }

        //Event fired when Find is selected from the search menu
        private void FindPerson(object sender, SelectedIndexEventArgs e)
        {
            newFindTextWindow = new FindTextForm(_addressBook, e.Index);

            newFindTextWindow.ShowDialog();

            if (!newFindTextWindow.ViewClosed)
            {
                if (_addressBook.PersonToFind != null)
                {
                    int index = findFocusIndex(_addressBook.PersonToFind);
                    setListFocus(index);

                    _addressBook.PersonToFind = null;

                    _view.EnableFindAgainButton();
                }
                else
                {
                    MessageBox.Show("No person could be found with information including that text");
                    _view.DisableFindAgainButton();
                }
            }
        }

        //Event fired when Find Again is selected from the search menu
        private void FindPersonAgain(object sender, SelectedIndexEventArgs e)
        {
            int index = e.Index;
            string textToFind = _addressBook.FoundText;

            _addressBook.searchForText(textToFind, index);

            if (_addressBook.PersonToFind != null)
            {
                int newIndex = findFocusIndex(_addressBook.PersonToFind);
                setListFocus(newIndex);

                _addressBook.PersonToFind = null;

                _view.EnableFindAgainButton();
            }
            else
            {
                MessageBox.Show("No person could be found with information including that text");
                _view.DisableFindAgainButton();
            }

        }

        //Method to offer the user the ability to save if unsaved changes have been made and the user is closing the current instance of the address book
        private bool offerToSave()
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

        /*Helper method for taking the string passed from the main view and ordering it correctly while removing white space to return a usable first and last name.
         * @param personName is the string containing the full name of the person passed from the view in the order last name, first name.
         */
        private static (string firstName, string lastName) cleanUpName(string personName)
        {
            string[] nameArray = personName.Split(',');
            string firstName = nameArray[1].Trim();
            string lastName = nameArray[0].Trim();

            return (firstName, lastName);
        }

        /*Helper method to set the focus of the list on the person we want after list update.
         * @param index is the location in the list that we want to focus on after an operation
         */
        private void setListFocus(int index)
        {
            var listBox = _view.ListPersonsControl;

            if (index != -1)
                listBox.SetSelected(index, true);
        }
        
        /*Find and return the index of the person we want
         * @param person is the person object that we want to find the index of in the list
         */
        private int findFocusIndex(Person person)
        {
            var listBox = _view.ListPersonsControl;
            int index = listBox.FindStringExact(person.getFullName());

            return index;
        }

        /*Find and return the index of the person we want. A separate method to deal with the delete event specifically,
         * accounting for the possibility of the person we want to delete being the last person in list making the focus index - 1.
        * @param person is the person object that we want to find the index of in the list
        */
        private int findDeleteFocusIndex(Person person)
        {
            var listBox = _view.ListPersonsControl;
            int index = listBox.FindStringExact(person.getFullName());
            int lastIndex = listBox.Items.Count - 1;

            if (index == lastIndex)
            {
                index = index - 1;
            }

            return index;
        }

        /*Helper method to set the instance of addressBook we are using when opening a file
         * @param addressBook is the instance of the address book that we want to set
         */
        public void setAddressBook(AddressBook addressBook)
        {
            _addressBook = addressBook;
        }

        //Method to update the list of names shown on the main form
        public void RefreshAddressBook()
        {
            var personNames = new List<string>();
            string fullName = "";

            foreach (Person person in _addressBook.AddressBookList)
            {
                fullName = person.getFullName();
                personNames.Add(fullName);
            }

            _view.ListPersons = personNames;
        }

        //Event fired to setup the testing data
        public void SetupTests(object sender, EventArgs e)
        {
            _addressBook.setupTests();
            RefreshAddressBook();
        }
    }
}
