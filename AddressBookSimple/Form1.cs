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
        private static string personName = "";

        public mainForm()
        {
            InitializeComponent();
            var mainFormPresenter = new MainFormPresenter(this, new AddressBook());
            findAgainToolStripMenuItem.Enabled = false;
        }

        public List<string> ListPersons
        {
            //get => personsList.Items.Cast<string>().ToList();
            set => personsList.DataSource = value;
        }

        public ListBox ListPersonsControl
        {
            get => personsList;
        }

        //Public events
        public event EventHandler AddPerson;
        public event EventHandler<PersonInfoEventArgs> EditPerson;
        public event EventHandler<PersonInfoEventArgs> DeletePerson;
        public event EventHandler<PersonInfoEventArgs> ShowPersonInfo;

        public event EventHandler NewFile;
        public event EventHandler OpenFile;
        public event EventHandler SaveFile;
        public event EventHandler SaveFileAs;
        public event EventHandler ExitApplication;

        public event EventHandler SortByName;
        public event EventHandler SortByZip;

        public event EventHandler<SelectedIndexEventArgs> FindPerson;
        public event EventHandler<SelectedIndexEventArgs> FindPersonAgain;

        public event EventHandler SetupTests;

        //fires the save person event
        private void AddButton_Click(object sender, EventArgs e)
        {
            AddPerson?.Invoke(this, EventArgs.Empty);
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            personName = personsList.GetItemText(personsList.SelectedItem);
            EditPerson?.Invoke(this, new PersonInfoEventArgs(personName));
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            personName = personsList.GetItemText(personsList.SelectedItem);
            DeletePerson?.Invoke(this, new PersonInfoEventArgs(personName));
        }

        private void personsList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.personsList.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                personName = personsList.GetItemText(personsList.SelectedItem);
                ShowPersonInfo?.Invoke(this, new PersonInfoEventArgs(personName));
            }
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

        private void setupTest_Click(object sender, EventArgs e)
        {
            SetupTests?.Invoke(this, EventArgs.Empty);
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = this.personsList.SelectedIndex;
            FindPerson?.Invoke(this, new SelectedIndexEventArgs(index));
        }

        private void findAgainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = this.personsList.SelectedIndex;
            FindPersonAgain?.Invoke(this, new SelectedIndexEventArgs(index));
        }

        public void EnableFindAgainButton()
        {
            findAgainToolStripMenuItem.Enabled = true;
        }

        public void DisableFindAgainButton()
        {
            findAgainToolStripMenuItem.Enabled = false;
        }
    }
}
