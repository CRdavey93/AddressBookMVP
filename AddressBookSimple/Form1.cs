using AddressBookSimple.Models;
using AddressBookSimple.Presenters;
using AddressBookSimple.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AddressBookSimple
{
    public partial class mainForm : Form, IMainForm
    {
        public mainForm()
        {
            InitializeComponent();
            var mainFormPresenter = new MainFormPresenter(this);
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

        public List<string> ListPersons
        {
            set => personsList.DataSource = value;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        //Public events
        public event EventHandler<EventArgs> AddPerson;

        //fires the save person event
        private void saveButton_Click(object sender, EventArgs e)
        {
            AddPerson?.Invoke(this, EventArgs.Empty);
        }
    }
}
