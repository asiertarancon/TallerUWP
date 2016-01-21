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
        public IList<Contact> contacts;
        public Ejercicio5ViewModel()
        {
            SelectContactCommand = new DelegateCommand(() => SelectContactExecute());
            ListSelectContactCommand = new DelegateCommand(() => ListSelectContactExecute());
            SendMailCommand = new DelegateCommand(() => SendMail("Mail de prueba"));
            GoToMapCommand = new DelegateCommand(() => GoToMap());
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

        private async void ListSelectContactExecute()
        {
            var contactPicker = new Windows.ApplicationModel.Contacts.ContactPicker();
            ListContactSelected = new ObservableCollection<Contact>(await contactPicker.PickContactsAsync());
        }

        public async void SelectContactExecute() {
            var contactPicker = new Windows.ApplicationModel.Contacts.ContactPicker();
            contactPicker.SelectionMode = Windows.ApplicationModel.Contacts.ContactSelectionMode.Fields;
            contactPicker.DesiredFieldsWithContactFieldType.Add(Windows.ApplicationModel.Contacts.ContactFieldType.Email);
            ContactSelected = await contactPicker.PickContactAsync();

            
        }

        public async void SendMail(string messageBody)
        {
            if (ContactSelected == null)
            {
                var dialog = new MessageDialog("Your message here");
                await dialog.ShowAsync();
                return;
            }
            var emailMessage = new Windows.ApplicationModel.Email.EmailMessage();
            emailMessage.Body = messageBody;
            var email = ContactSelected.Emails.FirstOrDefault<Windows.ApplicationModel.Contacts.ContactEmail>();
            if (email != null)
            {
                var emailRecipient = new Windows.ApplicationModel.Email.EmailRecipient(email.Address);
                emailMessage.To.Add(emailRecipient);
            }

            await Windows.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(emailMessage);
        }

        public DelegateCommand SelectContactCommand
        {
            get; set;
        }

        public DelegateCommand ListSelectContactCommand
        {
            get; set;
        }

        public DelegateCommand SendMailCommand
        {
            get; set;
        }

        public DelegateCommand GoToMapCommand
        {
            get; set;
        }

        private Contact _contact;
        public Contact ContactSelected
        {
            get { return _contact; }
            set { SetProperty(ref _contact, value); }
        }

        private ObservableCollection<Contact> _listContact;
        public ObservableCollection<Contact> ListContactSelected
        {
            get { return _listContact; }
            set { SetProperty(ref _listContact, value); }
        }
    }
}
