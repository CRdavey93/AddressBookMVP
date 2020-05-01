using AddressBookSimple.Models;
using AddressBookSimple.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookSimple.Presenters
{
    public class ManagePersonPresenter
    {
        private readonly IManagePerson _view;
        private AddressBook _addressBook;

        public ManagePersonPresenter(IManagePerson view, AddressBook addressBook)
        {
            _view = view;
            _addressBook = addressBook;
            _view.SavePerson += SavePerson;
        }

        public void SavePerson(object sender, EventArgs e)
        {
            var person = new Person(_view.InputFirstName, _view.InputLastName, _view.InputAddress, _view.InputCity,
                                    _view.InputState, _view.InputZip, _view.InputPhoneNumber);

            _addressBook.UpdateAddressBook(person);
        }
    }
}
