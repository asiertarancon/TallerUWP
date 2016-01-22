#CODIGO PARA LA SESIÓN 2. Taller UWP

##Ejercicio 5
### Ejercicio 5a
#### XAML
     <Button Content="Selecciona un contacto" Command="{Binding SelectContactCommand}"/>
            <TextBlock Text="{Binding ContactSelected.DisplayName}"/>
            
#### Ejercicio5ViewModel
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

### Ejercicio 5b
#### XAML
    <Button Content="Seleccione varios contactos" Command="{Binding ListSelectContactCommand}"/>
            <ListBox ItemsSource="{Binding ListContactSelected}">
                <ListBox.ItemTemplate>
                    <DataTemplate>
                        <StackPanel Orientation="Horizontal">
                            <Image Source="{Binding SmallDisplayPicture}"/>
                            <TextBlock Margin="10" Text="{Binding DisplayName}"/>
                        </StackPanel>
                    </DataTemplate>
                </ListBox.ItemTemplate>
            </ListBox>
#### ViewModel
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

### Ejercicio 5c
#### XAML
    <Button Content="Enviar un mail" Margin="0,10,0,0" Command="{Binding SendMailCommand}" CommandParameter="{Binding ElementName=AsuntoTextBox, Path=Text}"/>
            <TextBox x:Name="AsuntoTextBox"  Header="Asunto del mail" />
            <TextBox Header="Cuerpo del mail" Text="{Binding BodyMail, Mode=TwoWay}"/>
#### ViewModel
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
### Ejercicio 5d
#### XAML
    <Button Content="Ir al Mapa" Command="{Binding GoToMapCommand}" Margin="0,10,0,0"/>
#### ViewModel
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

## Ejercicio 6
### Ejercicio 6a
#### XAML
    <StackPanel Orientation="Horizontal" Margin="0,20,0,0">
                <TextBlock Text="Número a enviar: "/>
                <TextBox x:Name="NumeroAEnviarTextBox" Margin="5,0,0,0"/>
                <Button x:Name="EnviaBadgeATilePrincipalButton" Content="Actualiza el Badge del Tile Principal" Click="EnviaBadgeATilePrincipalButton_Click"/>
            </StackPanel>
#### XAML.CS
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
### Ejercicio 6a - 2
#### XAML
    <Button x:Name="BorraBadgeButton" Content="Borra el Badge del Tile principal" Click="BorraBadgeButton_Click" Margin="0,20,0,0"/>
#### XAML.CS
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

### Ejercicio 6a - 3
#### XAML             
     <Button x:Name="CreaLiveTileSecundarioButton" Content="Crea un Live Tile Secundario" Click="CreaLiveTileSecundarioButton_Click" Margin="0,20,0,0"/>
#### XAML.CS
        #region Ejercicio 6a - 3
        private void BorraBadgeButton_Click(object sender, RoutedEventArgs e)
        {
            BadgeUpdateManager.CreateBadgeUpdaterForApplication().Clear();
        }
        #endregion            

### Ejercicio 6b - 1
#### XAML
              <TextBox
                x:Name="TextBoxDisplayName"
                Header="Nombre a mostrar"
                Text="Taller UWP"/>

                <CheckBox
                x:Name="CheckBoxSquare71x71Logo"
                Content="Usa Square71x71Logo"/>

                <Border Style="{StaticResource BorderTileStyle}">
                    <Image
                    Style="{StaticResource ImageTileStyle}"
                    Source="ms-appx:///Assets/DefaultSecondaryTileAssests/Small.png"/>
                </Border>

                <CheckBox
                x:Name="CheckBoxSquare150x150Logo"
                Content="Usa Square150x150Logo"
                IsChecked="True"/>

                <Border Style="{StaticResource BorderTileStyle}">
                    <Image
                    Style="{StaticResource ImageTileStyle}"
                    Source="ms-appx:///Assets/DefaultSecondaryTileAssests/Medium.png"/>
                </Border>

                <CheckBox
                x:Name="CheckBoxWide310x150Logo"
                Content="Usa Wide310x150Logo"/>

                <Border Style="{StaticResource BorderTileWideStyle}">
                    <Image
                    Style="{StaticResource ImageTileStyle}"
                    Source="ms-appx:///Assets/DefaultSecondaryTileAssests/Wide.png"/>
                </Border>

                <CheckBox
                x:Name="CheckBoxSquare310x310Logo"
                Content="Usa Square310x310Logo"/>

                <Border Style="{StaticResource BorderTileStyle}">
                    <Image
                    Style="{StaticResource ImageTileStyle}"
                    Source="ms-appx:///Assets/DefaultSecondaryTileAssests/Large.png"/>
                </Border>

                <CheckBox
                    x:Name="CheckBoxShowNameOnSquare150x150Logo"
                    Content="Muestra el nombre en Square150x150Logo"/>

                <CheckBox
                    x:Name="CheckBoxShowNameOnWide310x150Logo"
                    Content="Muestra el nombre en Wide310x150Logo"/>

                <CheckBox
                    x:Name="CheckBoxShowNameOnSquare310x310Logo"
                    Content="Muestra el nombre en Square310x310Logo"/>


                <Button
                    x:Name="ButtonPin"
                    Content="6.b.1 Click para anclar Tile secundario"
                    HorizontalAlignment="Stretch"
                    Click="ButtonPin_Click"
                    Margin="0,12,0,0"/>
#### XAML.CS
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
### Ejercicio 6b - 2
#### XAML
     <Button
                    x:Name="EnviarFechaATilePrimarioButton"
                    Content="6.b.2 Enviar hora actual a Tile"
                    HorizontalAlignment="Stretch"
                    Click="EnviarFechaATilePrimarioButton_Click"
                    Margin="0,12,0,0"/>
#### XAML.CS
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

### Ejercicio 6b - 3
#### XAML
      <Button
                    x:Name="EnviarNotificacionConCaducidadButton"
                    Content="6.b.3 Enviar notificacion con caducidad a Tile"
                    HorizontalAlignment="Stretch"
                    Click="EnviarNotificacionConCaducidadButton_Click"
                    Margin="0,12,0,0"/>
#### XAML.CS
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
### Ejercicio 6b - 4
#### XAML
     <Button
                    x:Name="CarLiveTileAdaptativoButton"
                    Content="6.b.4 Cargar Live Tiles Adaptativos"
                    HorizontalAlignment="Stretch"
                    Click="CargarLiveTileAdaptativoButton_Click"
                    Margin="0,12,0,0"/>
#### XAML.CS
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
### Ejercicio 6b - 5
#### XAML
     <Button
                    x:Name="DesanclarLiveTilesSecundariosButton"
                    Content="6.b.5 Desanclar Live Tiles Secundarios"
                    HorizontalAlignment="Stretch"
                    Click="DesanclarLiveTilesSecundariosButton_Click"
                    Margin="0,12,0,0"/>
#### XAML.CS
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

## Ejercicio 7
### Ejercicio 7a-1 , 7a-2 y 7a-3
#### XAML
    <Button
                    x:Name="ToastGenericButton"
                    Content="7a-1 Toast Genérica"
                    HorizontalAlignment="Stretch"
                    Click="ToastGenericButton_Click"
                    Margin="0,12,0,0"/>
     <Button
                    x:Name="ToastAlarmButton"
                    Content="7a-2 Toast Alarma"
                    HorizontalAlignment="Stretch"
                    Click="ToastAlarmaButton_Click"
                    Margin="0,12,0,0"/>
     <Button
                    x:Name="ToastLlamadaEntranteButton"
                    Content="7a-3 Toast Llamada entrante"
                    HorizontalAlignment="Stretch"
                    Click="ToastLlamadaEntranteButton_Click"
                    Margin="0,12,0,0"/>
#### XAML.CS
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

### Ejercicio 7b
#### XAML
      <Button
                    x:Name="ToastFantasmaButton"
                    Content="7b Toast fantasma"
                    HorizontalAlignment="Stretch"
                    Click="ToastFantasmaButton_Click"
                    Margin="0,12,0,0"/>
#### XAML.CS
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
### Ejercicio 7c
#### XAML
     <Button
                    x:Name="ToastConImagenInternetButton"
                    Content="7c Toast adaptativo con imagen de internet"
                    HorizontalAlignment="Stretch"
                    Click="ToastConImagenInternetButton_Click"
                    Margin="0,12,0,0"/>
#### XAML.CS
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
### Ejercicio 7d
#### XAML
     <Button
                    x:Name="AbreAplicacionButton"
                    Content="7d Abre nuestra aplicación con el mensaje que pongamos"
                    HorizontalAlignment="Stretch"
                    Click="AbreAplicacionButton_Click"
                    Margin="0,24,0,0"/>
#### XAML.CS
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