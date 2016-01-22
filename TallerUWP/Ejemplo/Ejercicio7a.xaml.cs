using Ejemplo.Live_Tiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
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
    public sealed partial class Ejercicio7a : Page
    {
        public Ejercicio7a()
        {
            this.InitializeComponent();
        }

        #region Ejercicio 7a
        private void ToastGenericButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear all existing notifications
            ToastNotificationManager.History.Clear();

            // Pop incoming call notification            
            var payload =
                $@"
                <toast launch='args'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>Default</text>
                            <text>Second Line of Text</text>
                        </binding>
                    </visual>
                    <actions>

                        <action arguments = 'ok'
                                content = 'ok' />

                        <action arguments = 'cancel'
                                content = 'cancel' />

                    </actions>
                </toast>";

            ToastHelper.PopCustomToast(payload);
        }

        private void ToastAlarmaButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear all existing notifications
            ToastNotificationManager.History.Clear();

            // Pop incoming call notification            
            var payload =
                $@"
                <toast launch='args' scenario='reminder'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>Reminder</text>
                            <text>Second Line of Text</text>
                        </binding>
                    </visual>
                    <actions>

                        <action arguments = 'snooze'
                                content = 'snooze' />

                        <action arguments = 'dismiss'
                                content = 'dismiss' />

                    </actions>
                </toast>";

            ToastHelper.PopCustomToast(payload);
        }

        private void ToastLlamadaEntranteButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear all existing notifications
            ToastNotificationManager.History.Clear();

            // Pop incoming call notification            
            var payload =
                 $@"
                <toast launch='args' scenario='incomingCall'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>Incoming Call</text>
                            <text>Second Line of Text</text>
                        </binding>
                    </visual>
                    <actions>

                        <action arguments = 'answer'
                                content = 'answer' />

                        <action arguments = 'ignore'
                                content = 'ignore' />

                    </actions>
                </toast>";

            ToastHelper.PopCustomToast(payload);
        }
        #endregion

        #region Ejercicio 7b
        private void ToastFantasmaButton_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument toastXml = new XmlDocument();
            toastXml.LoadXml($@"
                <toast>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>Toast fantasma</text>
                            <text>Esta notificación sólo aparece en el centro de notificaciones.</text>
                        </binding>
                    </visual>
                </toast>");

            ToastNotification toast = new ToastNotification(toastXml)
            {
                SuppressPopup = true
            };
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
        #endregion

        #region Ejercicio 7c
        private void ToastConImagenInternetButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear all existing notifications
            ToastNotificationManager.History.Clear();

            // Pop notifications
            var payload =
                $@"
                <toast scenario='reminder'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>Image Src - From Internet</text>
                            <text>Make sure an image is displayed below this text.</text>
                            <image src='http://hiking.andrewbares.net/trips/2/images/large/p193jj6k4e58c1c7k1486u8n6vt6.jpg'/>
                        </binding>
                    </visual>
                </toast>";
            ToastHelper.PopCustomToast(payload);
        }
        #endregion

        #region Ejercicio 7d
        private void AbreAplicacionButton_Click(object sender, RoutedEventArgs e)
        {
            
            // Clear all existing notifications
            ToastNotificationManager.History.Clear();

            // Pop notifications
            var payload =
                $@"
                <toast activationType='foreground' launch='args'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>Notificacion que abre la app</text>
                            <text>Cierra la aplicación. Make sure ""Windows 10"" is in the first text box. Press ""submit"".</text>
                        </binding>
                    </visual>
                    <actions>

                        <input
                            id = 'message'
                            type = 'text'
                            title = 'Message'
                            placeHolderContent = 'Enter ""Taller UWP""'
                            defaultInput = 'Taller UWP' />

                        <action activationType = 'foreground'
                                arguments = 'quickReply'
                                content = 'submit' />

                        <action activationType = 'foreground'
                                arguments = 'cancel'
                                content = 'cancel' />

                    </actions>
                </toast>";
            ToastHelper.PopCustomToast(payload);
            
        }
        #endregion
    }
}
