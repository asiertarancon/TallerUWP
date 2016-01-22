using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en http://go.microsoft.com/fwlink/?LinkId=234238

namespace Ejemplo
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class Ejercicio1 : Page
    {
        public Ejercicio1()
        {
            this.InitializeComponent();
            Loaded += Ejemplo1_Loaded;
        }

        private void Ejemplo1_Loaded(object sender, RoutedEventArgs e)
        {
            FamilyTextBlock.Text = AnalyticsInfo.VersionInfo.DeviceFamily;

            if (App.ActivatedEventArgs == null)
            {
                //Error("OnActivated was not triggered.");
                return;
            }

            if (App.ActivatedEventArgs.Kind != ActivationKind.ToastNotification)
            {
                //Error("Activated args kind was not ActivationKind.ToastNotification");
                return;
            }

            var toastArgs = App.ActivatedEventArgs as ToastNotificationActivatedEventArgs;
            if (toastArgs == null)
            {
                //Error("Activated args was not of type ToastNotificationActivatedEventArgs.");
                return;
            }

            string arguments = toastArgs.UserInput["message"] as string;
           
            HelloTextBlock.Text = "HELLO " + arguments;


            
        }
    }
}
