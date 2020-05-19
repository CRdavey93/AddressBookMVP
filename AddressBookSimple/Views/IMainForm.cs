using AddressBookSimple.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AddressBookSimple.Views
{
    public interface IMainForm
    {
        List<string> ListPersons { set; }
        ListBox ListPersonsControl { get; }

        void EnableFindAgainButton();
        void DisableFindAgainButton();

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

        event EventHandler<SelectedIndexEventArgs> FindPerson;
        event EventHandler<SelectedIndexEventArgs> FindPersonAgain;

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

    public class SelectedIndexEventArgs : EventArgs
    {
        public SelectedIndexEventArgs(int index)
        {
            Index = index;
        }

        public int Index { get; }
    }

}
