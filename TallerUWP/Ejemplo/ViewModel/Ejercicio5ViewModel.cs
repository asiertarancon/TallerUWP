using Ejemplo.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Contacts;
using Windows.UI.Popups;

namespace Ejemplo.ViewModel
{
    public class Ejercicio5ViewModel : ViewModelBase
    {
        #region Ejercicio5a
        private DelegateCommand _selectContactCommand;
        public DelegateCommand SelectContactCommand
        {
            get { return _selectContactCommand ?? (_selectContactCommand = new DelegateCommand(SelectContactExecute)); }
            set { SetProperty(ref _selectContactCommand, value); }
        }

        public async void SelectContactExecute()
        {
            var contactPicker = new Windows.ApplicationModel.Contacts.ContactPicker();
            contactPicker.SelectionMode = Windows.ApplicationModel.Contacts.ContactSelectionMode.Fields;
            contactPicker.DesiredFieldsWithContactFieldType.Add(Windows.ApplicationModel.Contacts.ContactFieldType.Email);
            ContactSelected = await contactPicker.PickContactAsync();
        }

        private Contact _contact;
        public Contact ContactSelected
        {
            get { return _contact; }
            set { SetProperty(ref _contact, value); }
        }
        #endregion

        #region Ejercicio 5b
        private DelegateCommand _listSelectContactCommand;
        public DelegateCommand ListSelectContactCommand
        {
            get { return _listSelectContactCommand ?? (_listSelectContactCommand = new DelegateCommand(ListSelectContactExecute)); }
            set { SetProperty(ref _listSelectContactCommand, value); }
        }

        private async void ListSelectContactExecute()
        {
            var contactPicker = new Windows.ApplicationModel.Contacts.ContactPicker();
            ListContactSelected = new ObservableCollection<Contact>(await contactPicker.PickContactsAsync());
        }

        private ObservableCollection<Contact> _listContact;
        public ObservableCollection<Contact> ListContactSelected
        {
            get { return _listContact; }
            set { SetProperty(ref _listContact, value); }
        }
        #endregion
        
        #region Ejercicio5c

        private DelegateCommand<string> _sendMailCommand;
        public DelegateCommand<string> SendMailCommand
        {
            get { return _sendMailCommand ?? (_sendMailCommand = new DelegateCommand<string>((subject)=>SendMail(subject))); }
            set { SetProperty(ref _sendMailCommand, value); }
        }

        public async void SendMail(string subject)
        {
            if (ContactSelected == null)
            {
                var dialog = new MessageDialog("Seleccione un contacto primero!");
                await dialog.ShowAsync();
                return;
            }
            var emailMessage = new Windows.ApplicationModel.Email.EmailMessage();
            emailMessage.Subject = subject;
            emailMessage.Body = BodyMail;
            var email = ContactSelected.Emails.FirstOrDefault<Windows.ApplicationModel.Contacts.ContactEmail>();
            if (email != null)
            {
                var emailRecipient = new Windows.ApplicationModel.Email.EmailRecipient(email.Address);
                emailMessage.To.Add(emailRecipient);
            }

            await Windows.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(emailMessage);
        }

        private string _bodyMail;
        public string BodyMail
        {
            get { return _bodyMail; }
            set { SetProperty(ref _bodyMail, value); }
        }

        #endregion

        #region Ejercicio5d

        private DelegateCommand _goToMapCommand;
        public DelegateCommand GoToMapCommand
        {
            get { return _goToMapCommand ?? (_goToMapCommand = new DelegateCommand(GoToMap)); }
            set { SetProperty(ref _goToMapCommand, value); }
        }

        private async void GoToMap()
        {
            // Center on New York City
            var uriNewYork = new Uri(@"bingmaps:?collection=point.36.116584_-115.176753_Caesars%20Palace");

            // Launch the Windows Maps app
            var launcherOptions = new Windows.System.LauncherOptions();
            launcherOptions.TargetApplicationPackageFamilyName = "Microsoft.WindowsMaps_8wekyb3d8bbwe";
            var success = await Windows.System.Launcher.LaunchUriAsync(uriNewYork, launcherOptions);
        }

        #endregion

    }
}
