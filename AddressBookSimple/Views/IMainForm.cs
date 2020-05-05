﻿using AddressBookSimple.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookSimple.Views
{
    public interface IMainForm
    {
        List<string> ListPersons { set; }

        event EventHandler AddPerson;
        event EventHandler<EditingPersonEventArgs> EditPerson;
    }

    public class EditingPersonEventArgs : EventArgs
    {
        public EditingPersonEventArgs(string personName)
        {
            PersonName = personName;
        }
        public string PersonName { get; }
    }
}
