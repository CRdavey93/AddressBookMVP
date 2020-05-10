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
        string personName = "";

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
        public event EventHandler<EditingPersonEventArgs> DeletePerson;
        public event EventHandler NewFile;
        public event EventHandler OpenFile;
        public event EventHandler SaveFile;
        public event EventHandler SaveFileAs;
        public event EventHandler ExitApplication;
        public event EventHandler SortByName;
        public event EventHandler SortByZip;

        //fires the save person event
        private void AddButton_Click(object sender, EventArgs e)
        {
            AddPerson?.Invoke(this, EventArgs.Empty);
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            personName = personsList.GetItemText(personsList.SelectedItem);
            EditPerson?.Invoke(this, new EditingPersonEventArgs(personName));

            int index = personsList.FindStringExact(personName);

            if (index != -1)
                personsList.SetSelected(index, true);
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            personName = personsList.GetItemText(personsList.SelectedItem);
            DeletePerson?.Invoke(this, new EditingPersonEventArgs(personName));
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewFile?.Invoke(this, EventArgs.Empty);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile?.Invoke(this, EventArgs.Empty);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile?.Invoke(this, EventArgs.Empty);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileAs?.Invoke(this, EventArgs.Empty);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExitApplication?.Invoke(this, EventArgs.Empty);
        }

        private void nameSortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SortByName?.Invoke(this, EventArgs.Empty);
        }

        private void zipSortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SortByZip?.Invoke(this, EventArgs.Empty);
        }
    }
}
