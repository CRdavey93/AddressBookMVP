﻿using AddressBookSimple.Models;
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
    public partial class FindTextForm : Form, IFindText
    {
        private int startSearchIndex;
        private bool viewClosed;
        public FindTextForm(AddressBook model, int index)
        {
            InitializeComponent();
            var findTextPresenter = new FindTextPresenter(this, model);
            startSearchIndex = index;
            viewClosed = false;
        }

        public string InputFindText
        {
            get => textBoxFindText.Text;
            set => textBoxFindText.Text = value;
        }

        public bool ViewClosed
        {
            get => viewClosed;
            set => viewClosed = value;
        }

        //Public Events
        public event EventHandler<SelectedIndexEventArgs> FindingText;

        private void okButton_Click(object sender, EventArgs e)
        {
            FindingText?.Invoke(this, new SelectedIndexEventArgs(startSearchIndex));
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            viewClosed = true;
            this.Close();
        }

        //Method used to close the form and can be called from the presenter
        public void CloseView()
        {
            this.Close();
        }
    }
}
