using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AddressBookSimple.Models;
using AddressBookSimple.Presenters;
using AddressBookSimple.Views;

namespace AddressBookSimple
{
    public partial class ManagePerson : Form, IManagePerson
    {

        private bool personBeingEditedFlag = false;
        private Person personBeingEdited = null;

        //Constructor used when adding a person
        public ManagePerson(AddressBook model)
        {
            InitializeComponent();
            var managePersonPresenter = new ManagePersonPresenter(this, model);
            personBeingEditedFlag = false;
        }

        //Constructor used when editing a person, which populates the text boxes with person info
        public ManagePerson(AddressBook model, Person person)
        {
            InitializeComponent();
            var managePersonPresenter = new ManagePersonPresenter(this, model);
            personBeingEditedFlag = true;
            personBeingEdited = person;

            textBoxFirstName.Text = person.FirstName;
            textBoxLastName.Text = person.LastName;
            textBoxAddress.Text = person.Address;
            textBoxCity.Text = person.City;
            textBoxState.Text = person.State;
            textBoxZip.Text = person.Zip;
            textBoxPhone.Text = person.PhoneNumber;
        }

        public string InputFirstName
        {
            get => textBoxFirstName.Text;
            set => textBoxFirstName.Text = value;
        }

        public string InputLastName
        {
            get => textBoxLastName.Text;
            set => textBoxLastName.Text = value;
        }

        public string InputPhoneNumber
        {
            get => textBoxPhone.Text;
            set => textBoxPhone.Text = value;
        }

        public string InputAddress
        {
            get => textBoxAddress.Text;
            set => textBoxAddress.Text = value;
        }

        public string InputCity
        {
            get => textBoxCity.Text;
            set => textBoxCity.Text = value;
        }

        public string InputState
        {
            get => textBoxState.Text;
            set => textBoxState.Text = value;
        }

        public string InputZip
        {
            get => textBoxZip.Text;
            set => textBoxZip.Text = value;
        }

        //Public events
        public event EventHandler<PersonEditedEventArgs> SavingPerson;

        private void saveButton_Click(object sender, EventArgs e)
        {
            SavingPerson?.Invoke(this, new PersonEditedEventArgs(personBeingEditedFlag, personBeingEdited));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Method used to close this form and can be called from the presenter
        public void CloseView()
        {
            this.Close();
        }
    }
}
