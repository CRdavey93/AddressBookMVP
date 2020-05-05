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
            var mainFormPresenter = new MainFormPresenter(this, new AddressBook());
        }

        public List<string> ListPersons
        {
            set => personsList.DataSource = value;
        }

        //Public events
        public event EventHandler AddPerson;
        public event EventHandler<EditingPersonEventArgs> EditPerson;

        //fires the save person event
        private void AddButton_Click(object sender, EventArgs e)
        {
            AddPerson?.Invoke(this, EventArgs.Empty);
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            string personName = personsList.GetItemText(personsList.SelectedItem);
            EditPerson?.Invoke(this, new EditingPersonEventArgs(personName));
        }
    }
}
