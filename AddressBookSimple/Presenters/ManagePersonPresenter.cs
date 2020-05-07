using AddressBookSimple.Models;
using AddressBookSimple.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            _view.SavingPerson += SavePerson;
        }

        public void SavePerson(object sender, PersonEditedEventArgs e)
        {
            bool personExists = false;
            bool missingInputs = checkAddPerson();
            bool personBeingEdited = e.PersonEditedFlag;

            if (!missingInputs)
            {
                if (!personBeingEdited)
                {

                    var person = new Person(_view.InputFirstName, _view.InputLastName, _view.InputAddress, _view.InputCity,
                                            _view.InputState, _view.InputZip, _view.InputPhoneNumber);

                    personExists = _addressBook.doesPersonExist(person);

                    if (!personExists)
                    {
                        _addressBook.UpdateAddressBook(person);
                        _view.CloseView();
                    }
                    else
                    {
                        MessageBox.Show("A person with that name already exists.");
                    }
                }
                else
                {
                    foreach (Person person in _addressBook.AddressBookList)
                    {
                        if(person == e.PersonBeingEdited)
                        {
                            updatePersonInfo(person);

                            _view.CloseView();
                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("First and Last Name required.");

            }
        }

        private void updatePersonInfo(Person person)
        {
            person.FirstName = _view.InputFirstName;
            person.LastName = _view.InputLastName;
            person.Address = _view.InputAddress;
            person.City = _view.InputCity;
            person.State = _view.InputState;
            person.Zip = _view.InputZip;
            person.PhoneNumber = _view.InputPhoneNumber;
        }


        //Helper method to check if both required inputs are included
        private bool checkAddPerson()
        {
            bool nullInput = false;

            if (string.IsNullOrEmpty(_view.InputFirstName) || string.IsNullOrEmpty(_view.InputLastName))
            {
                nullInput = true;
            }
            else
            {
                nullInput = false;
            }

            return nullInput;
        }
    }
}
