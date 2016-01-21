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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Ejemplo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Ejercicio6 : Page
    {
        public Ejercicio6()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string xml = $@"
                <tile version='3'>
                    <visual branding='nameAndLogo'>

                        <binding template='TileMedium'>
                            <text hint-wrap='true'>New tile notification</text>
                            <text hint-wrap='true' hint-style='captionSubtle'/>
                        </binding>

                        <binding template='TileWide'>
                            <text hint-wrap='true'>New tile notification</text>
                            <text hint-wrap='true' hint-style='captionSubtle'/>
                        </binding>

                        <binding template='TileLarge'>
                            <text hint-wrap='true'>New tile notification</text>
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
    }
}
