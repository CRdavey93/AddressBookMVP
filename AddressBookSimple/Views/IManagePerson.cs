using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookSimple.Views
{
    public interface IManagePerson
    {
        string InputFirstName { get; set; }  
        string InputLastName { get; set; }
        string InputPhoneNumber { get; set; }
        string InputAddress { get; set; }
        string InputCity { get; set; }
        string InputState { get; set; }
        string InputZip { get; set; }

        event EventHandler<EventArgs> SavePerson;
    }
}
