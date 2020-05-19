﻿using AddressBookSimple.Models;
using AddressBookSimple.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookSimple.Presenters
{
    public class FindTextPresenter
    {
        private readonly IFindText _view;
        private AddressBook _addressBook;
        public FindTextPresenter(IFindText view, AddressBook addressBook)
        {
            _view = view;
            _addressBook = addressBook;

            _view.FindingText += FindText;
        }

        public void FindText(object sender, SelectedIndexEventArgs e)
        {
            int startSearchIndex = e.Index;
            string textToFind = _view.InputFindText;

            _addressBook.searchForText(textToFind, startSearchIndex);
            _view.CloseView();
        }
    }
}
