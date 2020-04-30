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
        string InputFirstName { get; set; }
        string InputLastName { get; set; }
        string InputPhoneNumber { get; set; }
        List<string> ListPersons { set; }

        event EventHandler<EventArgs> AddPerson;
    }
}
