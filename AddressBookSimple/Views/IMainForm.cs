using AddressBookSimple.Models;
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
        event EventHandler<PersonInfoEventArgs> EditPerson;
        event EventHandler<PersonInfoEventArgs> DeletePerson;
        event EventHandler<PersonInfoEventArgs> ShowPersonInfo;

        event EventHandler NewFile;
        event EventHandler OpenFile;
        event EventHandler SaveFile;
        event EventHandler SaveFileAs;
        event EventHandler ExitApplication;
        event EventHandler SortByName;
        event EventHandler SortByZip;

        event EventHandler SetupTests;
    }

    public class PersonInfoEventArgs : EventArgs
    {
        public PersonInfoEventArgs(string personName)
        {
            PersonName = personName;
        }
        public string PersonName { get; }
    }

}
