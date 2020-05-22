using AddressBookSimple.Models;
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
        bool Canceled { get; set; }

        void CloseView();

        event EventHandler<PersonEditedEventArgs> SavingPerson;
    }

    public class PersonEditedEventArgs : EventArgs
    {
        public PersonEditedEventArgs(bool personEditedFlag, Person personBeingEdited)
        {
            PersonEditedFlag = personEditedFlag;
            PersonBeingEdited = personBeingEdited;
        }
        public bool PersonEditedFlag { get; }
        public Person PersonBeingEdited { get; }
    }
}
