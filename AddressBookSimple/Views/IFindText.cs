using AddressBookSimple.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookSimple.Views
{
    public interface IFindText
    {
        string InputFindText { get; set; }

        bool ViewClosed { get; set; }

        event EventHandler<SelectedIndexEventArgs> FindingText;

        void CloseView();
    }

}
