using Ejemplo.Live_Tiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.StartScreen;
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
    public sealed partial class Ejercicio6b : Page
    {
        public Ejercicio6b()
        {
            this.InitializeComponent();
        }

        #region Ejercicio 6b-1

        private string _tileId;
        private async void ButtonPin_Click(object sender, RoutedEventArgs e)
        {
            ButtonPin.IsEnabled = false;

            try
            {
                _tileId = DateTime.Now.Ticks.ToString();
                SecondaryTile tile = new SecondaryTile(_tileId);
                tile.Arguments = "args";

                tile.DisplayName = TextBoxDisplayName.Text;

                if (CheckBoxSquare150x150Logo.IsChecked.Value)
                    tile.VisualElements.Square150x150Logo = new Uri("ms-appx:///Assets/Square150x150Logo.png");

                if (CheckBoxSquare71x71Logo.IsChecked.Value)
                    tile.VisualElements.Square71x71Logo = new Uri("ms-appx:///Assets/Logo284.scale-100.png");

                if (CheckBoxWide310x150Logo.IsChecked.Value)
                    tile.VisualElements.Wide310x150Logo = new Uri("ms-appx:///Assets/Wide310x150Logo.png");

                if (CheckBoxSquare310x310Logo.IsChecked.Value)
                    tile.VisualElements.Square310x310Logo = new Uri("ms-appx:///Assets/Wide310x310Logo.png");

                tile.VisualElements.ShowNameOnSquare150x150Logo = CheckBoxShowNameOnSquare150x150Logo.IsChecked.Value;
                tile.VisualElements.ShowNameOnSquare310x310Logo = CheckBoxShowNameOnSquare310x310Logo.IsChecked.Value;
                tile.VisualElements.ShowNameOnWide310x150Logo = CheckBoxShowNameOnWide310x150Logo.IsChecked.Value;

                await tile.RequestCreateAsync();
            }

            catch (Exception ex)
            {
                await new Windows.UI.Popups.MessageDialog(ex.ToString(), "Error").ShowAsync();
            }

            ButtonPin.IsEnabled = true;
        }
        #endregion

        #region Ejercicio 6b-2
        private void EnviarFechaATilePrimarioButton_Click(object sender, RoutedEventArgs e)
        {
            string xml = $@"
                <tile version='3'>
                    <visual branding='nameAndLogo'>

                        <binding template='TileMedium'>
                            <text hint-wrap='true'>Taller UWP notificacion 1</text>
                            <text hint-wrap='true' hint-style='captionSubtle'/>
                        </binding>

                        <binding template='TileWide'>
                            <text hint-wrap='true'>Taller UWP notificacion 2</text>
                            <text hint-wrap='true' hint-style='captionSubtle'/>
                        </binding>

                        <binding template='TileLarge'>
                            <text hint-wrap='true'>Taller UWP notificacion 3</text>
                            <text hint-wrap='true' hint-style='captionSubtle'/>
                        </binding>

                </visual>
            </tile>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            string nowTimeString = DateTime.Now.ToString();

            // Assign date/time values through XmlDocument to avoid any xml escaping issues
            foreach (XmlElement textEl in doc.SelectNodes("//text").OfType<XmlElement>())
                if (textEl.InnerText.Length == 0)
                    textEl.InnerText = nowTimeString;

            TileNotification notification = new TileNotification(doc);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
        }
        #endregion

        #region Ejercicio 6b-3
        private async void EnviarNotificacionConCaducidadButton_Click(object sender, RoutedEventArgs e)
        {
            base.IsEnabled = false;

            try
            {
                if (_tileId == null)
                {
                    await new MessageDialog("Ejecuta primero el Ejercicio 6b-1", "Error").ShowAsync();
                    return;
                }

                SecondaryTile tile = (await SecondaryTile.FindAllAsync()).FirstOrDefault(i => i.TileId.Equals(_tileId));
                if (tile == null)
                {
                    await new MessageDialog("Parece que has eliminado el Tile de inicio. Vuelve a crearlo!", "Error").ShowAsync();
                    return;
                }

                // Decide expiration time
                Windows.Globalization.Calendar cal = new Windows.Globalization.Calendar();
                cal.SetToNow();
                cal.AddSeconds(20);

                // Get expiration time and date
                var longTime = new Windows.Globalization.DateTimeFormatting.DateTimeFormatter("longtime");
                DateTimeOffset expiryTime = cal.GetDateTime();
                string expiryTimeString = longTime.Format(expiryTime);

                // Create the custom tile that will expire
                string tileXmlString =
                "<tile>"
                + "<visual>"
                + "<binding template='TileMedium'>"
                + "<text hint-wrap='true'>Esta notificación expirará a las " + expiryTimeString + "</text>"
                + "</binding>"
                + "<binding template='TileWide'>"
                + "<text hint-wrap='true'>Esta notificación expirará a las " + expiryTimeString + "</text>"
                + "</binding>"
                + "<binding template='TileLarge'>"
                + "<text hint-wrap='true'>Esta notificación expirará a las " + expiryTimeString + "</text>"
                + "</binding>"
                + "</visual>"
                + "</tile>";

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(tileXmlString);

                // Create the notification
                TileNotification notifyTile = new TileNotification(xmlDoc);

                // Set expiration time for the notification
                notifyTile.ExpirationTime = expiryTime;

                // And send the notification to the tile
                TileUpdateManager.CreateTileUpdaterForSecondaryTile(tile.TileId).Update(notifyTile);
            }

            catch (Exception ex)
            {
                await new MessageDialog(ex.ToString(), "Error al actualizar el Tile").ShowAsync();
            }

            finally
            {
                base.IsEnabled = true;
            }
           
        }
        #endregion

        #region Ejercicio 6b-4
        private async void CargarLiveTileAdaptativoButton_Click(object sender, RoutedEventArgs e)
        {
            await TilesHelper.UpdateTiles($@"
                <tile>
                    <visual>

                        <binding template='TileMedium' hint-presentation='people'>
                            <image src='Assets/Photos/1.jpg'/>
                            <image src='Assets/Photos/2.jpg'/>
                            <image src='Assets/Photos/3.jpg'/>
                            <image src='Assets/Photos/4.jpg'/>
                            <image src='Assets/Photos/5.jpg'/>
                            <image src='Assets/Photos/6.jpg'/>
                            <image src='Assets/Photos/7.jpg'/>
                            <image src='Assets/Photos/8.jpg'/>
                            <image src='Assets/Photos/9.jpg'/>
                            <image src='Assets/Photos/10.jpg'/>
                        </binding>

                        <binding template='TileWide' hint-presentation='people'>
                            <image src='Assets/Photos/1.jpg'/>
                            <image src='Assets/Photos/2.jpg'/>
                            <image src='Assets/Photos/3.jpg'/>
                            <image src='Assets/Photos/4.jpg'/>
                            <image src='Assets/Photos/5.jpg'/>
                            <image src='Assets/Photos/6.jpg'/>
                            <image src='Assets/Photos/7.jpg'/>
                            <image src='Assets/Photos/8.jpg'/>
                            <image src='Assets/Photos/9.jpg'/>
                            <image src='Assets/Photos/10.jpg'/>
                        </binding>

                        <binding template='TileLarge' hint-presentation='people'>
                            <image src='Assets/Photos/1.jpg'/>
                            <image src='Assets/Photos/2.jpg'/>
                            <image src='Assets/Photos/3.jpg'/>
                            <image src='Assets/Photos/4.jpg'/>
                            <image src='Assets/Photos/5.jpg'/>
                            <image src='Assets/Photos/6.jpg'/>
                            <image src='Assets/Photos/7.jpg'/>
                            <image src='Assets/Photos/8.jpg'/>
                            <image src='Assets/Photos/9.jpg'/>
                            <image src='Assets/Photos/10.jpg'/>
                        </binding>

                    </visual>
                </tile>");
        }

       
        #endregion

        #region Ejercicio 6b-5
        private async void DesanclarLiveTilesSecundariosButton_Click(object sender, RoutedEventArgs e)
        {
            DesanclarLiveTilesSecundariosButton.IsEnabled = false;

            // Loop through every secondary tile
            foreach (SecondaryTile tile in await SecondaryTile.FindAllAsync())
            {
                // Unpin each secondary tile
                await tile.RequestDeleteAsync();
            }

            DesanclarLiveTilesSecundariosButton.IsEnabled = true;
        }
        #endregion
    }
}
