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
        //private AddressBook addressBook;
        public ManagePerson(AddressBook model)
        {
            InitializeComponent();
            var managePersonPresenter = new ManagePersonPresenter(this, model);
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Public events
        public event EventHandler<EventArgs> SavePerson;

        private void saveButton_Click(object sender, EventArgs e)
        {
            SavePerson?.Invoke(this, EventArgs.Empty);
            this.Close();
        }
    }
}
