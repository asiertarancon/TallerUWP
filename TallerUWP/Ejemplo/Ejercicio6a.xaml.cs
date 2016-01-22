using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Data.Xml.Dom;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.UI.StartScreen;
using Windows.UI;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Ejemplo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Ejercicio6a : Page
    {
        public Ejercicio6a()
        {
            this.InitializeComponent();
        }
        
        #region Ejercicio 6a - 1
        private async void EnviaBadgeATilePrincipalButton_Click(object sender, RoutedEventArgs e)
        {
            int num;

            if (!int.TryParse(NumeroAEnviarTextBox.Text, out num))
            {
                await new MessageDialog("Mete un número!", "Error").ShowAsync();
                return;
            }

            // Get the blank badge XML payload for a badge number
            XmlDocument badgeXml = BadgeUpdateManager.GetTemplateContent(BadgeTemplateType.BadgeNumber);

            // Set the value of the badge in the XML to our number
            XmlElement badgeElement = badgeXml.SelectSingleNode("/badge") as XmlElement;
            badgeElement.SetAttribute("value", num.ToString());

            // Create the badge notification
            BadgeNotification badge = new BadgeNotification(badgeXml);

            // Create the badge updater for the application
            BadgeUpdater badgeUpdater = BadgeUpdateManager.CreateBadgeUpdaterForApplication();

            // And update the badge
            badgeUpdater.Update(badge);
        }
        #endregion

        #region Ejercicio 6a - 2
        private static readonly string SECONDARY_TILE_ID = "badge";
        private async void CreaLiveTileSecundarioButton_Click(object sender, RoutedEventArgs e)
        {
            // Create and pin new secondary tile for badges
            SecondaryTile tile = new SecondaryTile(SECONDARY_TILE_ID, "Taller UWP", "args", new Uri("ms-appx:///Assets/Logo.png"), TileSize.Default);
            tile.VisualElements.ShowNameOnSquare150x150Logo = true;
            tile.VisualElements.ShowNameOnSquare310x310Logo = true;
            tile.VisualElements.ShowNameOnWide310x150Logo = true;
            tile.VisualElements.BackgroundColor = Colors.Blue;// Color.FromArgb(255, 127,68, 86);
            var res = await tile.RequestCreateAsync();
        }
        #endregion

        #region Ejercicio 6a - 3
        private void BorraBadgeButton_Click(object sender, RoutedEventArgs e)
        {
            BadgeUpdateManager.CreateBadgeUpdaterForApplication().Clear();
        }
        #endregion
    }
}
