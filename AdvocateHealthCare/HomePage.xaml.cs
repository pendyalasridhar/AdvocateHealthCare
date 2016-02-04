//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net.Http;
//using System.Runtime.InteropServices.WindowsRuntime;
//using System.Threading.Tasks;
//using Windows.Foundation;
//using Windows.Foundation.Collections;
//using Windows.Graphics.Display;
//using Windows.UI;
//using Windows.UI.Popups;
//using Windows.UI.Text;
//using Windows.UI.ViewManagement;
//using Windows.UI.Xaml;
//using Windows.UI.Xaml.Controls;
//using Windows.UI.Xaml.Controls.Primitives;
//using Windows.UI.Xaml.Data;
//using Windows.UI.Xaml.Input;
//using Windows.UI.Xaml.Media;
//using Windows.UI.Xaml.Media.Imaging;
//using Windows.UI.Xaml.Navigation;



//// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

//namespace AdvocateHealthCare
//{
//    /// <summary>
//    /// An empty page that can be used on its own or navigated to within a Frame.
//    /// </summary>
//    /// 


//    public sealed partial class HomePage : Page
//    {
//        public UnreadClass objUnreadClass;
//        public DisplayOrientations orientation = DisplayOrientations.Landscape;
//        public HomePage()
//        {
//            this.InitializeComponent();
//            GetNotificationCount();
//            var bounds = Window.Current.Bounds;

//            double height = bounds.Height;

//            double width = bounds.Width;
//            if (height < width)
//            {
//                orientation = DisplayOrientations.Landscape;
//            }
//            else
//            {
//                orientation = DisplayOrientations.Portrait;
//            }
//            /// public DisplayOrientations orientation = DisplayOrientations.Portrait;
//            DisplayProperties.OrientationChanged += Page_OrientationChanged;
//        }

//        public void Page_Loaded(object sender, RoutedEventArgs e)
//        {

//        }

//        public void Page_OrientationChanged(object sender)
//        {
//            //The orientation of the device is ...
//            orientation = DisplayProperties.CurrentOrientation;
//            if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
//            {

//                ConstructTileGrid(lstDeliveryInformation);
//                ConstructTileGridforgeneral(lstDeliveryInformation);
//                ConstructTileGridforPredelivery(lstDeliveryInformation);
//                ConstructTileGridforHdelivery(lstDeliveryInformation);
//                ConstructTileGridforpostdelivery(lstDeliveryInformation);
//            }

//            if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
//            {

//                //VerticallyFlipped();
//                ConstructTileGrid(lstDeliveryInformation);
//                ConstructTileGridforgeneral(lstDeliveryInformation);
//                ConstructTileGridforPredelivery(lstDeliveryInformation);
//                ConstructTileGridforHdelivery(lstDeliveryInformation);
//                ConstructTileGridforpostdelivery(lstDeliveryInformation);
//            }

//        }


//        public class navigationvideos
//        {
//            public string videosource;
//            public string source;
//        }
//        public class UnreadClass
//        {
//            public int Notificationcount { get; set; }
//        }
//        private void stackAdvocatePortal_Tapped(object sender, TappedRoutedEventArgs e)
//        {
//            this.Frame.Navigate(typeof(DietandPregnancy));
//        }

//        private void stackDietandPregnancy_Tapped(object sender, TappedRoutedEventArgs e)
//        {
//            this.Frame.Navigate(typeof(DietandPregnancy));
//        }

//        private void stackTypesofDelivery_Tapped(object sender, TappedRoutedEventArgs e)
//        {

//        }

//        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
//        {

//        }

//        private void stackAdvocatePortal_Tapped_1(object sender, TappedRoutedEventArgs e)
//        {

//        }


//        private void StackNextClicked_Tapped(object sender, TappedRoutedEventArgs e)
//        {
//            navigationvideos navigationvide = new navigationvideos();
//            navigationvide.videosource = "aANj9_oeyUM";
//            navigationvide.source = "Home";
//            this.Frame.Navigate(typeof(PlayVideo), navigationvide);
//        }

//        private async void HospitalGallery_Tapped(object sender, TappedRoutedEventArgs e)
//        {
//            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://healthadvisor.advocatehealth.com/Classes"));

//        }

//        private void mySearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
//        {
//            this.Frame.Navigate(typeof(SearchPage), args.QueryText);
//        }

//        private void notificationsImg_Tapped(object sender, TappedRoutedEventArgs e)
//        {
//            this.Frame.Navigate(typeof(Notifications));
//        }
//        public static int unreadNotificationCount;

//        public object WebBrowserOpener { get; private set; }
//        //gets all the notifications(counts)
//        public async void GetNotificationCount()
//        {
//            try
//            {
//                if (App.IsInternet() == true)
//                {
//                    List<bool> objLiseUnreadStatus = new List<bool>();
//                    // App.BASE_URL+"/api/Notifications/GetNotifications?UserId=3&HospitalId=1";
//                    string getAllNotifications = App.BASE_URL + "/api/Notifications/GetNotifications?UserId=" + App.userId + "&HospitalId=" + App.hospitalId;
//                    var client = new HttpClient();
//                    HttpResponseMessage response = await client.GetAsync(new Uri(getAllNotifications));
//                    string jsonString = await response.Content.ReadAsStringAsync();
//                    JArray jArr = JArray.Parse(jsonString);
//                    for (var count = 0; count < jArr.Count; count++)
//                    {
//                        bool isRead = (bool)jArr[count]["IsRead"];
//                        if (isRead != true)
//                        {
//                            //objUnreadClass = new UnreadClass();
//                            //objUnreadClass.Notificationcount = (int)jArr[count]["IsRead"];
//                            objLiseUnreadStatus.Add(isRead);
//                        }
//                    }
//                    unreadNotificationCount = objLiseUnreadStatus.Count;
//                    //objUnreadClass = new UnreadClass();
//                    txtNotificationCount.Text = objLiseUnreadStatus.Count.ToString();
//                }
//                else
//                {
//                    MessageDialog msgDialog = new MessageDialog("Please check your internet connection and try again", "Internet Connection is not available");
//                    msgDialog.ShowAsync();
//                }
//            }
//            catch (Exception)
//            {
//                throw;
//            }

//        }
//        private void Notificationgridtapped(object sender, TappedRoutedEventArgs e)
//        {
//            this.Frame.Navigate(typeof(Notifications));
//        }
//        string ActiveItemHeaderName;
//        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
//        {
//            lstDeliveryInformation.Clear();
//            PivotItem currentItem = e.AddedItems[0] as PivotItem;
//            ActiveItemHeaderName = currentItem.Header.ToString();
//            switch (ActiveItemHeaderName)
//            {
//                case "All":
//                    DeliveryInfo(null);
//                    break;
//                case "General":
//                    DeliveryInfo("4");
//                    break;
//                case "Pre Delivery":
//                    DeliveryInfo("1");
//                    break;

//                case "Delivery":
//                    DeliveryInfo("2");
//                    break;

//                case "Post Delivery":
//                    DeliveryInfo("3");
//                    break;
//            }

//        }
//        DeliveryInformation objDeliveryInformation = new DeliveryInformation();
//        //gets the content of selected pivot item by passing id
//        List<DeliveryInformation> lstDeliveryInformation = new List<DeliveryInformation>();
//        public async void DeliveryInfo(string id)
//        {
//            rngProgress.Visibility = Visibility.Visible;
//            if (App.IsInternet() == true)
//            {
//                try
//                {

//                    string DeliveryInfoUri = App.BASE_URL + "/api/Tiles/GetTilesById?SUBCATEGORYID=" + id;
//                    var client = new HttpClient();
//                    HttpResponseMessage response = await client.GetAsync(new Uri(DeliveryInfoUri));
//                    string jsonString = await response.Content.ReadAsStringAsync();

//                    if (jsonString != "[]")
//                    {
//                        JArray jArr = JArray.Parse(jsonString);
//                        for (int itemCount = 0; itemCount < jArr.Count; itemCount++)
//                        {
//                            objDeliveryInformation = new DeliveryInformation();
//                            objDeliveryInformation.DeliveryTitle = (string)jArr[itemCount]["TITLE"];
//                            objDeliveryInformation.DeliveryInfo = (string)jArr[itemCount]["CONTENT"];
//                            var x = App.BASE_URL + jArr[itemCount]["TITLEIMAGE"];

//                            Uri uri = new Uri(x);
//                            objDeliveryInformation.DeliveryUrl = uri;
//                            lstDeliveryInformation.Add(objDeliveryInformation);
//                        }
//                        switch (ActiveItemHeaderName)
//                        {
//                            case "All":
//                                ConstructTileGrid(lstDeliveryInformation);
//                                // grdDeliveryDetails.ItemsSource = lstDeliveryInformation;
//                                break;
//                            case "General":
//                                ConstructTileGridforgeneral(lstDeliveryInformation);
//                                break;
//                            case "Pre Delivery":
//                                ConstructTileGridforPredelivery(lstDeliveryInformation);
//                                break;

//                            case "Delivery":
//                                ConstructTileGridforHdelivery(lstDeliveryInformation);
//                                break;

//                            case "Post Delivery":
//                                ConstructTileGridforpostdelivery(lstDeliveryInformation);
//                                break;
//                        }
//                    }
//                    else
//                    {
//                    }
//                }
//                catch (Exception ex)
//                {
//                    rngProgress.Visibility = Visibility.Collapsed;
//                    MessageDialog msgDialog = new MessageDialog("Please check your internet connectivity. If the problem persists, please contact administrator.", "Message");
//                    msgDialog.ShowAsync();
//                }
//                rngProgress.Visibility = Visibility.Collapsed;
//            }
//            else
//            {
//                rngProgress.Visibility = Visibility.Collapsed;
//                MessageDialog msgDialog = new MessageDialog("Please check your internet connection and try again", "Internet Connection is not available");
//                msgDialog.ShowAsync();

//            }
//        }



//        public void ConstructTileGrid(List<DeliveryInformation> deliveryInfoList)
//        {
//            tileGrid.RowDefinitions.Clear();
//            tileGrid.ColumnDefinitions.Clear();
//            tileGrid.Children.Clear();


//            int row = 0;
//            int col = -1;

//            if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
//            {
//                AddColumnsToTileGridLandscape();


//                RowDefinition rd1 = new RowDefinition();
//                GridLength gl1 = new GridLength(1, GridUnitType.Star);//new GridLength(2, GridUnitType.Star);
//                rd1.Height = gl1;
//                tileGrid.RowDefinitions.Add(rd1);

//                col = col + 1;

//                Grid grdstatic1 = new Grid();

//                grdstatic1.Tapped += ChildGrid_Tapped;


//                Image imgStatic = new Image();
//                imgStatic.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Humera.png", UriKind.Absolute));
//                imgStatic.Margin = new Thickness(10, 10, 10, 10);
//                imgStatic.Stretch = Stretch.Fill;
//                grdstatic1.Children.Add(imgStatic);

//                //Hiden TextBlock
//                TextBlock hidenTB1 = new TextBlock();
//                hidenTB1.Name = "TileName";
//                hidenTB1.Text = "aANj9_oeyUM";
//                hidenTB1.Visibility = Visibility.Collapsed;

//                Grid.SetRow(hidenTB1, row);
//                Grid.SetColumn(hidenTB1, col);
//                grdstatic1.Children.Add(hidenTB1);

//                Grid.SetRow(grdstatic1, row);
//                Grid.SetColumn(grdstatic1, col);
//                tileGrid.Children.Add(grdstatic1);


//                col = col + 1;

//                Grid grdstatic2 = new Grid();

//                grdstatic2.Tapped += ChildGrid_Tapped;

//                Image imgStatic1 = new Image();
//                imgStatic1.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Register.png", UriKind.Absolute));
//                imgStatic1.Margin = new Thickness(10, 10, 10, 10);
//                imgStatic1.Stretch = Stretch.Fill;
//                grdstatic2.Children.Add(imgStatic1);
//                Grid.SetRow(grdstatic2, row);
//                Grid.SetColumn(grdstatic2, col);
//                tileGrid.Children.Add(grdstatic2);


//                //Hiden TextBlock
//                TextBlock hidenTB2 = new TextBlock();
//                hidenTB2.Name = "TileName";
//                hidenTB2.Text = "Register";
//                hidenTB2.Visibility = Visibility.Collapsed;

//                Grid.SetRow(hidenTB2, row);
//                Grid.SetColumn(hidenTB2, col);
//                grdstatic2.Children.Add(hidenTB2);

//                //row = row + 1;


//            }
//            else if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
//            {
//                AddColumnsToTileGridPortrait();

//                RowDefinition rd1 = new RowDefinition();
//                GridLength gl1 = new GridLength(1, GridUnitType.Star);//new GridLength(2, GridUnitType.Star);
//                rd1.Height = gl1;
//                tileGrid.RowDefinitions.Add(rd1);

//                RowDefinition rd2 = new RowDefinition();
//                GridLength gl2 = new GridLength(1, GridUnitType.Star);//new GridLength(2, GridUnitType.Star);
//                rd2.Height = gl2;
//                tileGrid.RowDefinitions.Add(rd2);

//                col = col + 1;

//                Grid grdstatic1 = new Grid();

//                Image imgStatic = new Image();
//                imgStatic.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Humera.png", UriKind.Absolute));
//                imgStatic.Margin = new Thickness(10, 10, 10, 10);
//                grdstatic1.Children.Add(imgStatic);
//                Grid.SetRow(grdstatic1, row);
//                Grid.SetColumn(grdstatic1, col);
//                tileGrid.Children.Add(grdstatic1);

//                row = row + 1;

//                Grid grdstatic2 = new Grid();

//                Image imgStatic1 = new Image();
//                imgStatic1.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Register.png", UriKind.Absolute));
//                imgStatic1.Margin = new Thickness(10, 10, 10, 10);
//                grdstatic2.Children.Add(imgStatic1);
//                Grid.SetRow(grdstatic2, row);
//                Grid.SetColumn(grdstatic2, col);
//                tileGrid.Children.Add(grdstatic2);

//                // row = row + 1;


//            }


//            for (int i = 0; i < deliveryInfoList.Count; i++)
//            {
//                //ScrollViewer scrollViewer = new ScrollViewer();

//                Grid childGrid = null;
//                childGrid = new Grid();

//                //event
//                childGrid.Background = new SolidColorBrush(Colors.White);
//                childGrid.BorderThickness = new Thickness(1);
//                childGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));


//                childGrid.Tapped += ChildGrid_Tapped;

//                childGrid.Margin = new Thickness(10, 10, 10, 10);

//                //row1
//                RowDefinition childGridRow1 = new RowDefinition();
//                GridLength cgl1 = new GridLength(1, GridUnitType.Star);
//                childGridRow1.Height = cgl1;
//                childGrid.RowDefinitions.Add(childGridRow1);
//                //row2
//                RowDefinition childGridRow2 = new RowDefinition();
//                GridLength cgl2 = new GridLength(0.2, GridUnitType.Star);
//                childGridRow2.Height = cgl2;

//                childGrid.RowDefinitions.Add(childGridRow2);
//                //row3
//                RowDefinition childGridRow3 = new RowDefinition();
//                GridLength cgl3 = new GridLength(0.2, GridUnitType.Star);
//                childGridRow3.Height = cgl3;
//                childGrid.RowDefinitions.Add(childGridRow3);

//                //StackPanel deliveryInfoStackTile = new StackPanel();
//                //deliveryInfoStackTile.Orientation = Orientation.Vertical;

//                // < Image  Grid.Row = "0" Source = "{Binding DeliveryUrl}" Stretch = "Fill" ></ Image >
//                Image img = new Image();
//                BitmapImage bitmapImage = new BitmapImage();
//                bitmapImage.UriSource = deliveryInfoList[i].DeliveryUrl;
//                img.Source = bitmapImage;
//                img.Stretch = Stretch.Fill;

//                Grid.SetRow(img, 0);
//                childGrid.Children.Add(img);

//                //  < TextBlock Grid.Row = "1" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "20"   Foreground = "#DF6C3F"  Text = "{Binding DeliveryTitle}" Margin = "10,0,0,0" ></ TextBlock >

//                TextBlock tb1 = new TextBlock();
//                tb1.Name = "TileName";
//                if (deliveryInfoList[i].DeliveryInfo == null)
//                {

//                }
//                else
//                {
//                    tb1.Text = deliveryInfoList[i].DeliveryTitle;
//                }

//                tb1.TextTrimming = TextTrimming.WordEllipsis;
//                tb1.FontSize = 25;
//                tb1.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
//                tb1.FontWeight = FontWeights.SemiLight;
//                tb1.Margin = new Thickness(10, 0, 0, 0);

//                Grid.SetRow(tb1, 1);
//                childGrid.Children.Add(tb1);

//                //  < TextBlock Grid.Row = "2" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "17"  Foreground = "Black"  Text = "{Binding DeliveryInfo}"   Margin = "10,0,0,0" ></ TextBlock >

//                TextBlock tb2 = new TextBlock();
//                if (deliveryInfoList[i].DeliveryInfo == null)
//                {

//                }
//                else
//                {
//                    tb2.Text = deliveryInfoList[i].DeliveryInfo;
//                }

//                tb2.TextTrimming = TextTrimming.WordEllipsis;
//                tb2.FontSize = 19;
//                tb2.Foreground = new SolidColorBrush(Colors.Black);
//                tb2.FontWeight = FontWeights.SemiLight;
//                tb2.Margin = new Thickness(10, 10, 0, 15);

//                Grid.SetRow(tb2, 2);
//                childGrid.Children.Add(tb2);


//                if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
//                {
//                    if ((i + 3) > 2 * (row + 1))
//                    {
//                        RowDefinition rd = new RowDefinition();
//                        GridLength gl = new GridLength(1, GridUnitType.Star);
//                        rd.Height = gl;
//                        tileGrid.RowDefinitions.Add(rd);

//                        row = row + 1;
//                        col = 0;
//                    }
//                    else
//                    {
//                        col = col + 1;
//                    }
//                }
//                else if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
//                {
//                    if ((i + 3) > 1 * (row + 1))
//                    {
//                        RowDefinition rd = new RowDefinition();
//                        GridLength gl = new GridLength(1, GridUnitType.Star);
//                        rd.Height = gl;
//                        tileGrid.RowDefinitions.Add(rd);

//                        row = row + 1;
//                        col = 0;
//                    }
//                    else
//                    {
//                        col = col + 1;
//                    }
//                }
//                else
//                {
//                    if ((i + 1) > 2 * (row + 1))
//                    {
//                        RowDefinition rd = new RowDefinition();
//                        GridLength gl = new GridLength(1, GridUnitType.Star);
//                        rd.Height = gl;
//                        tileGrid.RowDefinitions.Add(rd);

//                        row = row + 1;
//                        col = 0;
//                    }
//                    else
//                    {
//                        col = col + 1;
//                    }
//                }

//                //Add to Grid
//                Grid.SetRow(childGrid, row);
//                Grid.SetColumn(childGrid, col);
//                tileGrid.Children.Add(childGrid);

//            }

//        }



//        private async void ChildGrid_Tapped(object sender, TappedRoutedEventArgs e)
//        {


//            foreach (Grid cg in tileGrid.Children)
//            {
//                cg.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
//            }

//            ((Windows.UI.Xaml.Controls.Grid)sender).BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));//new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));


//            DisplayOrientations x = orientation;
//            var childrens = ((Windows.UI.Xaml.Controls.Panel)sender).Children;

//            var textblocks = childrens.OfType<TextBlock>();


//            foreach (TextBlock t in textblocks)
//            {
//                if (t.Name == "TileName")
//                {
//                    if (t.Text == "Diet and Pregnancy")
//                    {
//                        await Task.Delay(1000);
//                        this.Frame.Navigate(typeof(DietandPregnancy));

//                    }
//                    else if (t.Text == "My Advocate Portal")
//                    {
//                        await Task.Delay(1000);
//                        this.Frame.Navigate(typeof(MyAdvocatePage));
//                    }
//                    else if (t.Text == "aANj9_oeyUM")
//                    {
//                        //navigationvideos navigationvide = new navigationvideos();
//                        //navigationvide.videosource = "aANj9_oeyUM";
//                        //navigationvide.source = "Home";
//                        //this.Frame.Navigate(typeof(PlayVideo), navigationvide);
//                        navigationvideos navigationvide = new navigationvideos();
//                        navigationvide.videosource = "VideoIDFromHomePage";
//                        navigationvide.source = "Home";
//                        this.Frame.Navigate(typeof(PlayVideo), navigationvide);
//                    }
//                    else if (t.Text == "Register")
//                    {
//                       // string uri = "https://healthadvisor.advocatehealth.com/Classes";
//                        Uri myUri = new Uri("https://healthadvisor.advocatehealth.com/Classes", UriKind.Absolute);
//                        //  await Windows.System.Launcher.LaunchUriAsync(new Uri("https://healthadvisor.advocatehealth.com/Classes"));

//                        var options = new Windows.System.LauncherOptions();
//                        options.DesiredRemainingView = Windows.UI.ViewManagement.ViewSizePreference.UseHalf;
//                        options.DisplayApplicationPicker = true;
//                        bool success = await Windows.System.Launcher.LaunchUriAsync(myUri, options);
//                        if (success)
//                        {
//                            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://healthadvisor.advocatehealth.com/Classes"));
//                        }
//                        else
//                        {

//                        }

//                    }
//                }

//            }


//        }

//        private void GridStaticTiles_Tapped(object sender, TappedRoutedEventArgs e)
//        {
//            // throw new NotImplementedException();
//        }

//        private void ChildGridgeneral_Tapped(object sender, TappedRoutedEventArgs e)
//        {


//            foreach (Grid cg in Generaltilegrid.Children)
//            {
//                cg.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
//            }

//            ((Windows.UI.Xaml.Controls.Grid)sender).BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
//            DisplayOrientations x = orientation;
//            var childrens = ((Windows.UI.Xaml.Controls.Panel)sender).Children;

//            var textblocks = childrens.OfType<TextBlock>();


//            foreach (TextBlock t in textblocks)
//            {
//                if (t.Name == "TileName")
//                {
//                    if (t.Text == "My Advocate Portal")
//                    {

//                        this.Frame.Navigate(typeof(MyAdvocatePage));

//                    }
//                }
//            }
//        }


//        public void AddColumnsToTileGridLandscape()
//        {

//            //1st column
//            ColumnDefinition cd1 = new ColumnDefinition();
//            GridLength gl1 = new GridLength(1, GridUnitType.Star);
//            cd1.Width = gl1;
//            tileGrid.ColumnDefinitions.Add(cd1);

//            //2st column
//            ColumnDefinition cd2 = new ColumnDefinition();
//            GridLength gl2 = new GridLength(1, GridUnitType.Star);
//            cd2.Width = gl2;
//            tileGrid.ColumnDefinitions.Add(cd2);


//        }
//        public void AddColumnsToTileGridPortrait()
//        {

//            //1st column
//            ColumnDefinition cd1 = new ColumnDefinition();
//            GridLength gl1 = new GridLength(1, GridUnitType.Star);
//            cd1.Width = gl1;
//            tileGrid.ColumnDefinitions.Add(cd1);


//        }
//        public void AddColumnsToTileGneralGridLandscape()
//        {

//            //1st column
//            ColumnDefinition cd1 = new ColumnDefinition();
//            GridLength gl1 = new GridLength(1, GridUnitType.Star);
//            cd1.Width = gl1;
//            Generaltilegrid.ColumnDefinitions.Add(cd1);

//            //2st column
//            ColumnDefinition cd2 = new ColumnDefinition();
//            GridLength gl2 = new GridLength(1, GridUnitType.Star);
//            cd2.Width = gl2;
//            Generaltilegrid.ColumnDefinitions.Add(cd2);

//            //3st column
//            //ColumnDefinition cd3 = new ColumnDefinition();
//            //GridLength gl3 = new GridLength(1, GridUnitType.Star);
//            //cd3.Width = gl3;
//            //tileGrid.ColumnDefinitions.Add(cd3);

//            ////4st column
//            //ColumnDefinition cd4 = new ColumnDefinition();
//            //GridLength gl4 = new GridLength(1, GridUnitType.Star);
//            //cd4.Width = gl4;
//            //tileGrid.ColumnDefinitions.Add(cd4);
//        }
//        public void AddColumnsToTilePredeliveryGridLandscape()
//        {

//            //1st column
//            ColumnDefinition cd1 = new ColumnDefinition();
//            GridLength gl1 = new GridLength(1, GridUnitType.Star);
//            cd1.Width = gl1;
//            Predeliverytilegrid.ColumnDefinitions.Add(cd1);

//            //2st column
//            ColumnDefinition cd2 = new ColumnDefinition();
//            GridLength gl2 = new GridLength(1, GridUnitType.Star);
//            cd2.Width = gl2;
//            Predeliverytilegrid.ColumnDefinitions.Add(cd2);

//            //3st column
//            //ColumnDefinition cd3 = new ColumnDefinition();
//            //GridLength gl3 = new GridLength(1, GridUnitType.Star);
//            //cd3.Width = gl3;
//            //tileGrid.ColumnDefinitions.Add(cd3);

//            ////4st column
//            //ColumnDefinition cd4 = new ColumnDefinition();
//            //GridLength gl4 = new GridLength(1, GridUnitType.Star);
//            //cd4.Width = gl4;
//            //tileGrid.ColumnDefinitions.Add(cd4);
//        }
//        public void AddColumnsToTiledeliveryGridLandscape()
//        {

//            //1st column
//            ColumnDefinition cd1 = new ColumnDefinition();
//            GridLength gl1 = new GridLength(1, GridUnitType.Star);
//            cd1.Width = gl1;
//            deliverytilegrid.ColumnDefinitions.Add(cd1);

//            //2st column
//            ColumnDefinition cd2 = new ColumnDefinition();
//            GridLength gl2 = new GridLength(1, GridUnitType.Star);
//            cd2.Width = gl2;
//            deliverytilegrid.ColumnDefinitions.Add(cd2);

//            //3st column
//            //ColumnDefinition cd3 = new ColumnDefinition();
//            //GridLength gl3 = new GridLength(1, GridUnitType.Star);
//            //cd3.Width = gl3;
//            //tileGrid.ColumnDefinitions.Add(cd3);

//            ////4st column
//            //ColumnDefinition cd4 = new ColumnDefinition();
//            //GridLength gl4 = new GridLength(1, GridUnitType.Star);
//            //cd4.Width = gl4;
//            //tileGrid.ColumnDefinitions.Add(cd4);
//        }
//        public void AddColumnsToTilepostdeliveryGridLandscape()
//        {

//            //1st column
//            ColumnDefinition cd1 = new ColumnDefinition();
//            GridLength gl1 = new GridLength(1, GridUnitType.Star);
//            cd1.Width = gl1;
//            postdeliverytilegrid.ColumnDefinitions.Add(cd1);

//            //2st column
//            ColumnDefinition cd2 = new ColumnDefinition();
//            GridLength gl2 = new GridLength(1, GridUnitType.Star);
//            cd2.Width = gl2;
//            postdeliverytilegrid.ColumnDefinitions.Add(cd2);

//            //3st column
//            //ColumnDefinition cd3 = new ColumnDefinition();
//            //GridLength gl3 = new GridLength(1, GridUnitType.Star);
//            //cd3.Width = gl3;
//            //tileGrid.ColumnDefinitions.Add(cd3);

//            ////4st column
//            //ColumnDefinition cd4 = new ColumnDefinition();
//            //GridLength gl4 = new GridLength(1, GridUnitType.Star);
//            //cd4.Width = gl4;
//            //tileGrid.ColumnDefinitions.Add(cd4);
//        }
//        public void VerticallyFlipped()
//        {
//            tileGrid.ColumnDefinitions.Clear();
//            //1st column
//            ColumnDefinition cd1 = new ColumnDefinition();
//            GridLength gl1 = new GridLength(1, GridUnitType.Auto);
//            cd1.Width = gl1;
//            tileGrid.ColumnDefinitions.Add(cd1);
//        }

//        public class DeliveryInformation
//        {
//            public string DeliveryTitle { get; set; }
//            public string DeliveryInfo { get; set; }
//            public Uri DeliveryUrl { get; set; }
//        }

//        private void grdDeliveryDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
//        {
//            try
//            {
//                DeliveryInformation objDelivery = (DeliveryInformation)(sender as GridView).SelectedItem;
//                string gridTitle = objDelivery.DeliveryTitle;
//                if (gridTitle == "Diet and Pregnancy")
//                {
//                    this.Frame.Navigate(typeof(DietandPregnancy));
//                }
//                if (gridTitle == " My Advocate Portal")
//                {
//                    this.Frame.Navigate(typeof(MyAdvocatePage));
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageDialog msgDialog = new MessageDialog("The required resources are not downloaded.Please check your internet connectivity. If the problem persists, please contact advocate healthcare customer care associate.", "Message");
//                msgDialog.ShowAsync();

//            }
//        }




//        public void AddColumnsToTileGeneralGridPortrait()
//        {

//            //1st column
//            ColumnDefinition cd1 = new ColumnDefinition();
//            GridLength gl1 = new GridLength(1, GridUnitType.Star);
//            cd1.Width = gl1;
//            Generaltilegrid.ColumnDefinitions.Add(cd1);

//            ////2st column
//            //ColumnDefinition cd2 = new ColumnDefinition();
//            //GridLength gl2 = new GridLength(1, GridUnitType.Star);
//            //cd2.Width = gl2;
//            //tileGrid.ColumnDefinitions.Add(cd2);

//            //3st column
//            //ColumnDefinition cd3 = new ColumnDefinition();
//            //GridLength gl3 = new GridLength(1, GridUnitType.Star);
//            //cd3.Width = gl3;
//            //tileGrid.ColumnDefinitions.Add(cd3);

//            ////4st column
//            //ColumnDefinition cd4 = new ColumnDefinition();
//            //GridLength gl4 = new GridLength(1, GridUnitType.Star);
//            //cd4.Width = gl4;
//            //tileGrid.ColumnDefinitions.Add(cd4);
//        }
//        public void AddColumnsToTilePredeliveryGridPortrait()
//        {

//            //1st column
//            ColumnDefinition cd1 = new ColumnDefinition();
//            GridLength gl1 = new GridLength(1, GridUnitType.Star);
//            cd1.Width = gl1;
//            Predeliverytilegrid.ColumnDefinitions.Add(cd1);

//            ////2st column
//            //ColumnDefinition cd2 = new ColumnDefinition();
//            //GridLength gl2 = new GridLength(1, GridUnitType.Star);
//            //cd2.Width = gl2;
//            //tileGrid.ColumnDefinitions.Add(cd2);

//            //3st column
//            //ColumnDefinition cd3 = new ColumnDefinition();
//            //GridLength gl3 = new GridLength(1, GridUnitType.Star);
//            //cd3.Width = gl3;
//            //tileGrid.ColumnDefinitions.Add(cd3);

//            ////4st column
//            //ColumnDefinition cd4 = new ColumnDefinition();
//            //GridLength gl4 = new GridLength(1, GridUnitType.Star);
//            //cd4.Width = gl4;
//            //tileGrid.ColumnDefinitions.Add(cd4);
//        }
//        public void AddColumnsToTiledeliveryGridPortrait()
//        {

//            //1st column
//            ColumnDefinition cd1 = new ColumnDefinition();
//            GridLength gl1 = new GridLength(1, GridUnitType.Star);
//            cd1.Width = gl1;
//            deliverytilegrid.ColumnDefinitions.Add(cd1);

//            ////2st column
//            //ColumnDefinition cd2 = new ColumnDefinition();
//            //GridLength gl2 = new GridLength(1, GridUnitType.Star);
//            //cd2.Width = gl2;
//            //tileGrid.ColumnDefinitions.Add(cd2);

//            //3st column
//            //ColumnDefinition cd3 = new ColumnDefinition();
//            //GridLength gl3 = new GridLength(1, GridUnitType.Star);
//            //cd3.Width = gl3;
//            //tileGrid.ColumnDefinitions.Add(cd3);

//            ////4st column
//            //ColumnDefinition cd4 = new ColumnDefinition();
//            //GridLength gl4 = new GridLength(1, GridUnitType.Star);
//            //cd4.Width = gl4;
//            //tileGrid.ColumnDefinitions.Add(cd4);
//        }
//        public void AddColumnsToTilepostdeliveryGridPortrait()
//        {

//            //1st column
//            ColumnDefinition cd1 = new ColumnDefinition();
//            GridLength gl1 = new GridLength(1, GridUnitType.Star);
//            cd1.Width = gl1;
//            postdeliverytilegrid.ColumnDefinitions.Add(cd1);

//            ////2st column
//            //ColumnDefinition cd2 = new ColumnDefinition();
//            //GridLength gl2 = new GridLength(1, GridUnitType.Star);
//            //cd2.Width = gl2;
//            //tileGrid.ColumnDefinitions.Add(cd2);

//            //3st column
//            //ColumnDefinition cd3 = new ColumnDefinition();
//            //GridLength gl3 = new GridLength(1, GridUnitType.Star);
//            //cd3.Width = gl3;
//            //tileGrid.ColumnDefinitions.Add(cd3);

//            ////4st column
//            //ColumnDefinition cd4 = new ColumnDefinition();
//            //GridLength gl4 = new GridLength(1, GridUnitType.Star);
//            //cd4.Width = gl4;
//            //tileGrid.ColumnDefinitions.Add(cd4);
//        }

//        public void ConstructTileGridforgeneral(List<DeliveryInformation> deliveryInfoList)
//        {
//            Generaltilegrid.RowDefinitions.Clear();
//            Generaltilegrid.ColumnDefinitions.Clear();
//            Generaltilegrid.Children.Clear();

//            if (orientation == DisplayOrientations.PortraitFlipped || orientation == DisplayOrientations.Portrait)
//            {
//                AddColumnsToTileGeneralGridPortrait();

//            }
//            else if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
//            {
//                AddColumnsToTileGneralGridLandscape();

//            }
//            else
//            {
//                AddColumnsToTileGneralGridLandscape();

//            }

//            int row = 0;
//            int col = -1;
//            //1st row
//            RowDefinition rd1 = new RowDefinition();
//            GridLength gl1 = new GridLength(2, GridUnitType.Star);
//            rd1.Height = gl1;
//            Generaltilegrid.RowDefinitions.Add(rd1);


//            for (int i = 0; i < deliveryInfoList.Count; i++)
//            {
//                //ScrollViewer scrollViewer = new ScrollViewer();

//                Grid childGrid = null;
//                childGrid = new Grid();

//                //event
//                childGrid.Background = new SolidColorBrush(Colors.White);
//                childGrid.BorderThickness = new Thickness(1);
//                childGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));


//                childGrid.Tapped += ChildGridgeneral_Tapped;

//                childGrid.Margin = new Thickness(10, 10, 10, 10);

//                //row1
//                RowDefinition childGridRow1 = new RowDefinition();
//                GridLength cgl1 = new GridLength(1, GridUnitType.Star);
//                childGridRow1.Height = cgl1;
//                childGrid.RowDefinitions.Add(childGridRow1);
//                //row2
//                RowDefinition childGridRow2 = new RowDefinition();
//                GridLength cgl2 = new GridLength(0.2, GridUnitType.Star);
//                childGridRow2.Height = cgl2;

//                childGrid.RowDefinitions.Add(childGridRow2);
//                //row3
//                RowDefinition childGridRow3 = new RowDefinition();
//                GridLength cgl3 = new GridLength(0.2, GridUnitType.Star);
//                childGridRow3.Height = cgl3;
//                childGrid.RowDefinitions.Add(childGridRow3);

//                //StackPanel deliveryInfoStackTile = new StackPanel();
//                //deliveryInfoStackTile.Orientation = Orientation.Vertical;

//                // < Image  Grid.Row = "0" Source = "{Binding DeliveryUrl}" Stretch = "Fill" ></ Image >
//                Image img = new Image();
//                BitmapImage bitmapImage = new BitmapImage();
//                bitmapImage.UriSource = deliveryInfoList[i].DeliveryUrl;
//                img.Source = bitmapImage;
//                img.Stretch = Stretch.Fill;

//                Grid.SetRow(img, 0);
//                childGrid.Children.Add(img);

//                //  < TextBlock Grid.Row = "1" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "20"   Foreground = "#DF6C3F"  Text = "{Binding DeliveryTitle}" Margin = "10,0,0,0" ></ TextBlock >

//                TextBlock tb1 = new TextBlock();
//                tb1.Name = "TileName";
//                if (deliveryInfoList[i].DeliveryInfo == null)
//                {

//                }
//                else
//                {
//                    tb1.Text = deliveryInfoList[i].DeliveryTitle;
//                }

//                tb1.TextTrimming = TextTrimming.WordEllipsis;
//                tb1.FontSize = 25;
//                tb1.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
//                tb1.FontWeight = FontWeights.SemiLight;
//                tb1.Margin = new Thickness(10, 0, 0, 0);

//                Grid.SetRow(tb1, 1);
//                childGrid.Children.Add(tb1);

//                //  < TextBlock Grid.Row = "2" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "17"  Foreground = "Black"  Text = "{Binding DeliveryInfo}"   Margin = "10,0,0,0" ></ TextBlock >

//                TextBlock tb2 = new TextBlock();
//                if (deliveryInfoList[i].DeliveryInfo == null)
//                {

//                }
//                else
//                {
//                    tb2.Text = deliveryInfoList[i].DeliveryInfo;
//                }

//                tb2.TextTrimming = TextTrimming.WordEllipsis;
//                tb2.FontSize = 19;
//                tb2.Foreground = new SolidColorBrush(Colors.Black);
//                tb2.FontWeight = FontWeights.SemiLight;
//                tb2.Margin = new Thickness(10, 10, 0, 15);

//                Grid.SetRow(tb2, 2);
//                childGrid.Children.Add(tb2);


//                if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
//                {
//                    if ((i + 1) > 2 * (row + 1))
//                    {
//                        RowDefinition rd = new RowDefinition();
//                        GridLength gl = new GridLength(1, GridUnitType.Star);
//                        rd.Height = gl;
//                        Generaltilegrid.RowDefinitions.Add(rd);

//                        row = row + 1;
//                        col = 0;
//                    }
//                    else
//                    {
//                        col = col + 1;
//                    }
//                }
//                else if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
//                {
//                    if ((i + 1) > 1 * (row + 1))
//                    {
//                        RowDefinition rd = new RowDefinition();
//                        GridLength gl = new GridLength(1, GridUnitType.Star);
//                        rd.Height = gl;
//                        Generaltilegrid.RowDefinitions.Add(rd);

//                        row = row + 1;
//                        col = 0;
//                    }
//                    else
//                    {
//                        col = col + 1;
//                    }
//                }
//                else
//                {
//                    if ((i + 1) > 2 * (row + 1))
//                    {
//                        RowDefinition rd = new RowDefinition();
//                        GridLength gl = new GridLength(1, GridUnitType.Star);
//                        rd.Height = gl;
//                        Generaltilegrid.RowDefinitions.Add(rd);

//                        row = row + 1;
//                        col = 0;
//                    }
//                    else
//                    {
//                        col = col + 1;
//                    }
//                }

//                //Add to Grid
//                Grid.SetRow(childGrid, row);
//                Grid.SetColumn(childGrid, col);
//                Generaltilegrid.Children.Add(childGrid);

//            }

//        }
//        public void ConstructTileGridforPredelivery(List<DeliveryInformation> deliveryInfoList)
//        {
//            Predeliverytilegrid.RowDefinitions.Clear();
//            Predeliverytilegrid.ColumnDefinitions.Clear();
//            Predeliverytilegrid.Children.Clear();

//            if (orientation == DisplayOrientations.PortraitFlipped || orientation == DisplayOrientations.Portrait)
//            {
//                AddColumnsToTilePredeliveryGridPortrait();

//            }
//            else if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
//            {
//                AddColumnsToTilePredeliveryGridLandscape();

//            }
//            else
//            {
//                AddColumnsToTilePredeliveryGridLandscape();

//            }

//            int row = 0;
//            int col = -1;
//            //1st row
//            RowDefinition rd1 = new RowDefinition();
//            GridLength gl1 = new GridLength(2, GridUnitType.Star);
//            rd1.Height = gl1;
//            Predeliverytilegrid.RowDefinitions.Add(rd1);



//            for (int i = 0; i < deliveryInfoList.Count; i++)
//            {
//                //ScrollViewer scrollViewer = new ScrollViewer();

//                Grid childGrid = null;
//                childGrid = new Grid();

//                //event
//                childGrid.Background = new SolidColorBrush(Colors.White);
//                childGrid.BorderThickness = new Thickness(1);
//                childGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));


//                childGrid.Tapped += ChildGridpredelivery_Tapped;

//                childGrid.Margin = new Thickness(10, 10, 10, 10);

//                //row1
//                RowDefinition childGridRow1 = new RowDefinition();
//                GridLength cgl1 = new GridLength(1, GridUnitType.Star);
//                childGridRow1.Height = cgl1;
//                childGrid.RowDefinitions.Add(childGridRow1);
//                //row2
//                RowDefinition childGridRow2 = new RowDefinition();
//                GridLength cgl2 = new GridLength(0.2, GridUnitType.Star);
//                childGridRow2.Height = cgl2;

//                childGrid.RowDefinitions.Add(childGridRow2);
//                //row3
//                RowDefinition childGridRow3 = new RowDefinition();
//                GridLength cgl3 = new GridLength(0.2, GridUnitType.Star);
//                childGridRow3.Height = cgl3;
//                childGrid.RowDefinitions.Add(childGridRow3);

//                //StackPanel deliveryInfoStackTile = new StackPanel();
//                //deliveryInfoStackTile.Orientation = Orientation.Vertical;

//                // < Image  Grid.Row = "0" Source = "{Binding DeliveryUrl}" Stretch = "Fill" ></ Image >
//                Image img = new Image();
//                BitmapImage bitmapImage = new BitmapImage();
//                bitmapImage.UriSource = deliveryInfoList[i].DeliveryUrl;
//                img.Source = bitmapImage;
//                img.Stretch = Stretch.Fill;

//                Grid.SetRow(img, 0);
//                childGrid.Children.Add(img);

//                //  < TextBlock Grid.Row = "1" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "20"   Foreground = "#DF6C3F"  Text = "{Binding DeliveryTitle}" Margin = "10,0,0,0" ></ TextBlock >

//                TextBlock tb1 = new TextBlock();
//                tb1.Name = "TileName";
//                if (deliveryInfoList[i].DeliveryInfo == null)
//                {

//                }
//                else
//                {
//                    tb1.Text = deliveryInfoList[i].DeliveryTitle;
//                }

//                tb1.TextTrimming = TextTrimming.WordEllipsis;
//                tb1.FontSize = 25;
//                tb1.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
//                tb1.FontWeight = FontWeights.SemiLight;
//                tb1.Margin = new Thickness(10, 0, 0, 0);

//                Grid.SetRow(tb1, 1);
//                childGrid.Children.Add(tb1);

//                //  < TextBlock Grid.Row = "2" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "17"  Foreground = "Black"  Text = "{Binding DeliveryInfo}"   Margin = "10,0,0,0" ></ TextBlock >

//                TextBlock tb2 = new TextBlock();
//                if (deliveryInfoList[i].DeliveryInfo == null)
//                {

//                }
//                else
//                {
//                    tb2.Text = deliveryInfoList[i].DeliveryInfo;
//                }

//                tb2.TextTrimming = TextTrimming.WordEllipsis;
//                tb2.FontSize = 19;
//                tb2.Foreground = new SolidColorBrush(Colors.Black);
//                tb2.FontWeight = FontWeights.SemiLight;
//                tb2.Margin = new Thickness(10, 10, 0, 15);

//                Grid.SetRow(tb2, 2);
//                childGrid.Children.Add(tb2);


//                if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
//                {
//                    if ((i + 1) > 2 * (row + 1))
//                    {
//                        RowDefinition rd = new RowDefinition();
//                        GridLength gl = new GridLength(1, GridUnitType.Star);
//                        rd.Height = gl;
//                        Predeliverytilegrid.RowDefinitions.Add(rd);

//                        row = row + 1;
//                        col = 0;
//                    }
//                    else
//                    {
//                        col = col + 1;
//                    }
//                }
//                else if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
//                {
//                    if ((i + 1) > 1 * (row + 1))
//                    {
//                        RowDefinition rd = new RowDefinition();
//                        GridLength gl = new GridLength(1, GridUnitType.Star);
//                        rd.Height = gl;
//                        Predeliverytilegrid.RowDefinitions.Add(rd);

//                        row = row + 1;
//                        col = 0;
//                    }
//                    else
//                    {
//                        col = col + 1;
//                    }
//                }
//                else
//                {
//                    if ((i + 1) > 2 * (row + 1))
//                    {
//                        RowDefinition rd = new RowDefinition();
//                        GridLength gl = new GridLength(1, GridUnitType.Star);
//                        rd.Height = gl;
//                        Predeliverytilegrid.RowDefinitions.Add(rd);

//                        row = row + 1;
//                        col = 0;
//                    }
//                    else
//                    {
//                        col = col + 1;
//                    }
//                }

//                //Add to Grid
//                Grid.SetRow(childGrid, row);
//                Grid.SetColumn(childGrid, col);
//                Predeliverytilegrid.Children.Add(childGrid);

//            }

//        }
//        private async void ChildGridpredelivery_Tapped(object sender, TappedRoutedEventArgs e)
//        {


//            foreach (Grid cg in Predeliverytilegrid.Children)
//            {
//                cg.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
//            }

//           ((Windows.UI.Xaml.Controls.Grid)sender).BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));

//            var childrens = ((Windows.UI.Xaml.Controls.Panel)sender).Children;

//            var textblocks = childrens.OfType<TextBlock>();


//            foreach (TextBlock t in textblocks)
//            {
//                if (t.Name == "TileName")
//                {
//                    if (t.Text == "Diet and Pregnancy")
//                    {

//                        this.Frame.Navigate(typeof(DietandPregnancy));

//                    }
//                }
//            }
//        }
//        private async void ChildGriddelivery_Tapped(object sender, TappedRoutedEventArgs e)
//        {


//            foreach (Grid cg in deliverytilegrid.Children)
//            {
//                cg.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
//            }

//           ((Windows.UI.Xaml.Controls.Grid)sender).BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
//        }
//        public void ConstructTileGridforHdelivery(List<DeliveryInformation> deliveryInfoList)
//        {
//            deliverytilegrid.RowDefinitions.Clear();
//            deliverytilegrid.ColumnDefinitions.Clear();
//            deliverytilegrid.Children.Clear();

//            if (orientation == DisplayOrientations.PortraitFlipped || orientation == DisplayOrientations.Portrait)
//            {
//                AddColumnsToTiledeliveryGridPortrait();

//            }
//            else if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
//            {
//                AddColumnsToTiledeliveryGridLandscape();

//            }
//            else
//            {
//                AddColumnsToTiledeliveryGridLandscape();

//            }

//            int row = 0;
//            int col = -1;
//            //1st row
//            if (lstDeliveryInformation.Count < 3)
//            {
//                RowDefinition rd1 = new RowDefinition();
//                GridLength gl1 = new GridLength(330);
//                rd1.Height = gl1;
//                deliverytilegrid.RowDefinitions.Add(rd1);
//            }
//            else
//            {
//                RowDefinition rd1 = new RowDefinition();
//                GridLength gl1 = new GridLength(1, GridUnitType.Star);
//                rd1.Height = gl1;
//                deliverytilegrid.RowDefinitions.Add(rd1);
//            }


//            for (int i = 0; i < deliveryInfoList.Count; i++)
//            {
//                //ScrollViewer scrollViewer = new ScrollViewer();

//                Grid childGrid = null;
//                childGrid = new Grid();

//                //event
//                childGrid.Background = new SolidColorBrush(Colors.White);
//                childGrid.BorderThickness = new Thickness(1);
//                childGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));


//                childGrid.Tapped += ChildGriddelivery_Tapped;

//                childGrid.Margin = new Thickness(10, 10, 10, 10);

//                //row1
//                RowDefinition childGridRow1 = new RowDefinition();
//                GridLength cgl1 = new GridLength(1, GridUnitType.Star);
//                childGridRow1.Height = cgl1;
//                childGrid.RowDefinitions.Add(childGridRow1);
//                //row2
//                RowDefinition childGridRow2 = new RowDefinition();
//                GridLength cgl2 = new GridLength(0.2, GridUnitType.Star);
//                childGridRow2.Height = cgl2;

//                childGrid.RowDefinitions.Add(childGridRow2);
//                //row3
//                RowDefinition childGridRow3 = new RowDefinition();
//                GridLength cgl3 = new GridLength(0.2, GridUnitType.Star);
//                childGridRow3.Height = cgl3;
//                childGrid.RowDefinitions.Add(childGridRow3);

//                //StackPanel deliveryInfoStackTile = new StackPanel();
//                //deliveryInfoStackTile.Orientation = Orientation.Vertical;

//                // < Image  Grid.Row = "0" Source = "{Binding DeliveryUrl}" Stretch = "Fill" ></ Image >
//                Image img = new Image();
//                BitmapImage bitmapImage = new BitmapImage();
//                bitmapImage.UriSource = deliveryInfoList[i].DeliveryUrl;
//                img.Source = bitmapImage;
//                img.Stretch = Stretch.Fill;

//                Grid.SetRow(img, 0);
//                childGrid.Children.Add(img);

//                //  < TextBlock Grid.Row = "1" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "20"   Foreground = "#DF6C3F"  Text = "{Binding DeliveryTitle}" Margin = "10,0,0,0" ></ TextBlock >

//                TextBlock tb1 = new TextBlock();
//                tb1.Name = "TileName";
//                if (deliveryInfoList[i].DeliveryInfo == null)
//                {

//                }
//                else
//                {
//                    tb1.Text = deliveryInfoList[i].DeliveryTitle;
//                }

//                tb1.TextTrimming = TextTrimming.WordEllipsis;
//                tb1.FontSize = 25;
//                tb1.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
//                tb1.FontWeight = FontWeights.SemiLight;
//                tb1.Margin = new Thickness(10, 0, 0, 0);

//                Grid.SetRow(tb1, 1);
//                childGrid.Children.Add(tb1);

//                //  < TextBlock Grid.Row = "2" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "17"  Foreground = "Black"  Text = "{Binding DeliveryInfo}"   Margin = "10,0,0,0" ></ TextBlock >

//                TextBlock tb2 = new TextBlock();
//                if (deliveryInfoList[i].DeliveryInfo == null)
//                {

//                }
//                else
//                {
//                    tb2.Text = deliveryInfoList[i].DeliveryInfo;
//                }

//                tb2.TextTrimming = TextTrimming.WordEllipsis;
//                tb2.FontSize = 19;
//                tb2.Foreground = new SolidColorBrush(Colors.Black);
//                tb2.FontWeight = FontWeights.SemiLight;
//                tb2.Margin = new Thickness(10, 8, 0, 8);

//                Grid.SetRow(tb2, 2);
//                childGrid.Children.Add(tb2);
//                if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
//                {
//                    if ((i + 1) > 2 * (row + 1))
//                    {
//                        if (deliveryInfoList.Count < 3)
//                        {

//                            RowDefinition rd = new RowDefinition();
//                            GridLength gl = new GridLength(330);
//                            rd.Height = gl;
//                            deliverytilegrid.RowDefinitions.Add(rd);

//                            row = row + 1;
//                            col = 0;
//                        }
//                        else
//                        {
//                            RowDefinition rd = new RowDefinition();
//                            GridLength gl = new GridLength(1, GridUnitType.Star);
//                            rd.Height = gl;
//                            deliverytilegrid.RowDefinitions.Add(rd);

//                            row = row + 1;
//                            col = 0;

//                        }
//                    }

//                    else
//                    {
//                        col = col + 1;
//                    }
//                }

//                else if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
//                {
//                    if ((i + 1) > 1 * (row + 1))
//                    {
//                        if (deliveryInfoList.Count < 3)
//                        {
//                            RowDefinition rd = new RowDefinition();
//                            GridLength gl = new GridLength(330);
//                            rd.Height = gl;
//                            deliverytilegrid.RowDefinitions.Add(rd);

//                            row = row + 1;
//                            col = 0;
//                        }
//                        else
//                        {
//                            RowDefinition rd = new RowDefinition();
//                            GridLength gl = new GridLength(1, GridUnitType.Star);
//                            rd.Height = gl;
//                            deliverytilegrid.RowDefinitions.Add(rd);

//                            row = row + 1;
//                            col = 0;
//                        }
//                    }
//                    else
//                    {
//                        col = col + 1;
//                    }
//                }
//                else
//                {
//                    if ((i + 1) > 2 * (row + 1))
//                    {
//                        RowDefinition rd = new RowDefinition();
//                        GridLength gl = new GridLength(1, GridUnitType.Star);
//                        rd.Height = gl;
//                        deliverytilegrid.RowDefinitions.Add(rd);

//                        row = row + 1;
//                        col = 0;
//                    }
//                    else
//                    {
//                        col = col + 1;
//                    }
//                }

//                //if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
//                //{
//                //    if ((i + 1) > 2 * (row + 1))
//                //    {
//                //        RowDefinition rd = new RowDefinition();
//                //        GridLength gl = new GridLength(300);
//                //        rd.Height = gl;
//                //        deliverytilegrid.RowDefinitions.Add(rd);

//                //        row = row + 1;
//                //        col = 0;
//                //    }
//                //    else
//                //    {
//                //        col = col + 1;
//                //    }
//                //}
//                //else if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
//                //{
//                //    if ((i + 1) > 1 * (row + 1))
//                //    {
//                //        RowDefinition rd = new RowDefinition();
//                //        GridLength gl = new GridLength(1, GridUnitType.Star);
//                //        rd.Height = gl;
//                //        deliverytilegrid.RowDefinitions.Add(rd);

//                //        row = row + 1;
//                //        col = 0;
//                //    }
//                //    else
//                //    {
//                //        col = col + 1;
//                //    }
//                //}
//                //else
//                //{
//                //    if ((i + 1) > 2 * (row + 1))
//                //    {
//                //        RowDefinition rd = new RowDefinition();
//                //        GridLength gl = new GridLength(300);
//                //        rd.Height = gl;
//                //        deliverytilegrid.RowDefinitions.Add(rd);

//                //        row = row + 1;
//                //        col = 0;
//                //    }
//                //    else
//                //    {
//                //        col = col + 1;
//                //    }
//                //}

//                //Add to Grid
//                Grid.SetRow(childGrid, row);
//                Grid.SetColumn(childGrid, col);
//                deliverytilegrid.Children.Add(childGrid);

//            }

//        }
//        public void ConstructTileGridforpostdelivery(List<DeliveryInformation> deliveryInfoList)
//        {
//            postdeliverytilegrid.RowDefinitions.Clear();
//            postdeliverytilegrid.ColumnDefinitions.Clear();
//            postdeliverytilegrid.Children.Clear();

//            if (orientation == DisplayOrientations.PortraitFlipped || orientation == DisplayOrientations.Portrait)
//            {
//                AddColumnsToTilepostdeliveryGridPortrait();

//            }
//            else if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
//            {
//                AddColumnsToTilepostdeliveryGridLandscape();

//            }
//            else
//            {
//                AddColumnsToTilepostdeliveryGridLandscape();

//            }

//            int row = 0;
//            int col = -1;
//            ////1st row
//            //RowDefinition rd1 = new RowDefinition();
//            //GridLength gl1 = new GridLength(1, GridUnitType.Star);
//            //rd1.Height = gl1;
//            //postdeliverytilegrid.RowDefinitions.Add(rd1);
//            if (lstDeliveryInformation.Count < 3)
//            {
//                RowDefinition rd11 = new RowDefinition();
//                GridLength gl11 = new GridLength(330);
//                rd11.Height = gl11;
//                postdeliverytilegrid.RowDefinitions.Add(rd11);
//            }
//            else
//            {
//                RowDefinition rd12 = new RowDefinition();
//                GridLength gl12 = new GridLength(1, GridUnitType.Star);
//                rd12.Height = gl12;
//                postdeliverytilegrid.RowDefinitions.Add(rd12);
//            }

//            for (int i = 0; i < deliveryInfoList.Count; i++)
//            {
//                //ScrollViewer scrollViewer = new ScrollViewer();

//                Grid childGrid = null;
//                childGrid = new Grid();

//                //event
//                childGrid.Background = new SolidColorBrush(Colors.White);
//                childGrid.BorderThickness = new Thickness(1);
//                childGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));


//                childGrid.Tapped += ChildGridpostdelivery_Tapped;

//                childGrid.Margin = new Thickness(10, 10, 10, 10);

//                //row1
//                RowDefinition childGridRow1 = new RowDefinition();
//                GridLength cgl1 = new GridLength(1, GridUnitType.Star);
//                childGridRow1.Height = cgl1;
//                childGrid.RowDefinitions.Add(childGridRow1);
//                //row2
//                RowDefinition childGridRow2 = new RowDefinition();
//                GridLength cgl2 = new GridLength(0.2, GridUnitType.Star);
//                childGridRow2.Height = cgl2;

//                childGrid.RowDefinitions.Add(childGridRow2);
//                //row3
//                RowDefinition childGridRow3 = new RowDefinition();
//                GridLength cgl3 = new GridLength(0.2, GridUnitType.Star);
//                childGridRow3.Height = cgl3;
//                childGrid.RowDefinitions.Add(childGridRow3);

//                //StackPanel deliveryInfoStackTile = new StackPanel();
//                //deliveryInfoStackTile.Orientation = Orientation.Vertical;

//                // < Image  Grid.Row = "0" Source = "{Binding DeliveryUrl}" Stretch = "Fill" ></ Image >
//                Image img = new Image();
//                BitmapImage bitmapImage = new BitmapImage();
//                bitmapImage.UriSource = deliveryInfoList[i].DeliveryUrl;
//                img.Source = bitmapImage;
//                img.Stretch = Stretch.Fill;

//                Grid.SetRow(img, 0);
//                childGrid.Children.Add(img);

//                //  < TextBlock Grid.Row = "1" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "20"   Foreground = "#DF6C3F"  Text = "{Binding DeliveryTitle}" Margin = "10,0,0,0" ></ TextBlock >

//                TextBlock tb1 = new TextBlock();
//                tb1.Name = "TileName";
//                if (deliveryInfoList[i].DeliveryInfo == null)
//                {

//                }
//                else
//                {
//                    tb1.Text = deliveryInfoList[i].DeliveryTitle;
//                }

//                tb1.TextTrimming = TextTrimming.WordEllipsis;
//                tb1.FontSize = 25;
//                tb1.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
//                tb1.FontWeight = FontWeights.SemiLight;
//                tb1.Margin = new Thickness(10, 0, 0, 0);

//                Grid.SetRow(tb1, 1);
//                childGrid.Children.Add(tb1);

//                //  < TextBlock Grid.Row = "2" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "17"  Foreground = "Black"  Text = "{Binding DeliveryInfo}"   Margin = "10,0,0,0" ></ TextBlock >

//                TextBlock tb2 = new TextBlock();
//                if (deliveryInfoList[i].DeliveryInfo == null)
//                {

//                }
//                else
//                {
//                    tb2.Text = deliveryInfoList[i].DeliveryInfo;
//                }

//                tb2.TextTrimming = TextTrimming.WordEllipsis;
//                tb2.FontSize = 19;
//                tb2.Foreground = new SolidColorBrush(Colors.Black);
//                tb2.FontWeight = FontWeights.SemiLight;
//                tb2.Margin = new Thickness(10, 8, 0, 8);

//                Grid.SetRow(tb2, 2);
//                childGrid.Children.Add(tb2);
//                if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
//                {
//                    if ((i + 1) > 2 * (row + 1))
//                    {
//                        if (lstDeliveryInformation.Count < 3)
//                        {

//                            RowDefinition rd = new RowDefinition();
//                            GridLength gl = new GridLength(330);
//                            rd.Height = gl;
//                            postdeliverytilegrid.RowDefinitions.Add(rd);

//                            row = row + 1;
//                            col = 0;
//                        }
//                        else
//                        {
//                            RowDefinition rd = new RowDefinition();
//                            GridLength gl = new GridLength(1, GridUnitType.Star);
//                            rd.Height = gl;
//                            postdeliverytilegrid.RowDefinitions.Add(rd);

//                            row = row + 1;
//                            col = 0;

//                        }
//                    }

//                    else
//                    {
//                        col = col + 1;
//                    }
//                }

//                else if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
//                {
//                    if ((i + 1) > 1 * (row + 1))
//                    {
//                        if (lstDeliveryInformation.Count < 3)
//                        {
//                            RowDefinition rd = new RowDefinition();
//                            GridLength gl = new GridLength(330);
//                            rd.Height = gl;
//                            postdeliverytilegrid.RowDefinitions.Add(rd);

//                            row = row + 1;
//                            col = 0;
//                        }
//                        else
//                        {
//                            RowDefinition rd = new RowDefinition();
//                            GridLength gl = new GridLength(1, GridUnitType.Star);
//                            rd.Height = gl;
//                            postdeliverytilegrid.RowDefinitions.Add(rd);

//                            row = row + 1;
//                            col = 0;
//                        }
//                    }
//                    else
//                    {
//                        col = col + 1;
//                    }
//                }
//                else
//                {
//                    if ((i + 1) > 2 * (row + 1))
//                    {
//                        RowDefinition rd = new RowDefinition();
//                        GridLength gl = new GridLength(1, GridUnitType.Star);
//                        rd.Height = gl;
//                        postdeliverytilegrid.RowDefinitions.Add(rd);

//                        row = row + 1;
//                        col = 0;
//                    }
//                    else
//                    {
//                        col = col + 1;
//                    }
//                }

//                //if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
//                //{
//                //    if ((i + 1) > 2 * (row + 1))
//                //    {
//                //        RowDefinition rd = new RowDefinition();
//                //        GridLength gl = new GridLength(1, GridUnitType.Star);
//                //        rd.Height = gl;
//                //        postdeliverytilegrid.RowDefinitions.Add(rd);

//                //        row = row + 1;
//                //        col = 0;
//                //    }
//                //    else
//                //    {
//                //        col = col + 1;
//                //    }
//                //}
//                //else if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
//                //{
//                //    if ((i + 1) > 1 * (row + 1))
//                //    {
//                //        RowDefinition rd = new RowDefinition();
//                //        GridLength gl = new GridLength(1, GridUnitType.Star);
//                //        rd.Height = gl;
//                //        postdeliverytilegrid.RowDefinitions.Add(rd);

//                //        row = row + 1;
//                //        col = 0;
//                //    }
//                //    else
//                //    {
//                //        col = col + 1;
//                //    }
//                //}
//                //else
//                //{
//                //    if ((i + 1) > 2 * (row + 1))
//                //    {
//                //        RowDefinition rd = new RowDefinition();
//                //        GridLength gl = new GridLength(1, GridUnitType.Star);
//                //        rd.Height = gl;
//                //        postdeliverytilegrid.RowDefinitions.Add(rd);

//                //        row = row + 1;
//                //        col = 0;
//                //    }
//                //    else
//                //    {
//                //        col = col + 1;
//                //    }
//                //}

//                //Add to Grid
//                Grid.SetRow(childGrid, row);
//                Grid.SetColumn(childGrid, col);
//                postdeliverytilegrid.Children.Add(childGrid);

//            }

//        }
//        private async void ChildGridpostdelivery_Tapped(object sender, TappedRoutedEventArgs e)
//        {


//            foreach (Grid cg in postdeliverytilegrid.Children)
//            {
//                cg.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
//            }

//           ((Windows.UI.Xaml.Controls.Grid)sender).BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
//        }
//    }

//}
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Text;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;



// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AdvocateHealthCare
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 


    public sealed partial class HomePage : Page
    {
        public UnreadClass objUnreadClass;
        public DisplayOrientations orientation = DisplayOrientations.Landscape;
        public HomePage()
        {
            this.InitializeComponent();
            GetNotificationCount();
            var bounds = Window.Current.Bounds;

            double height = bounds.Height;

            double width = bounds.Width;
            if (height < width)
            {
                orientation = DisplayOrientations.Landscape;
            }
            else
            {
                orientation = DisplayOrientations.Portrait;
            }
            /// public DisplayOrientations orientation = DisplayOrientations.Portrait;
            DisplayProperties.OrientationChanged += Page_OrientationChanged;
        }

        public void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public void Page_OrientationChanged(object sender)
        {
            //The orientation of the device is ...
            orientation = DisplayProperties.CurrentOrientation;
            if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
            {

                ConstructTileGrid(lstDeliveryInformation);
                ConstructTileGridforgeneral(lstDeliveryInformation);
                ConstructTileGridforPredelivery(lstDeliveryInformation);
                ConstructTileGridforHdelivery(lstDeliveryInformation);
                ConstructTileGridforpostdelivery(lstDeliveryInformation);
            }

            if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
            {

                //VerticallyFlipped();
                ConstructTileGrid(lstDeliveryInformation);
                ConstructTileGridforgeneral(lstDeliveryInformation);
                ConstructTileGridforPredelivery(lstDeliveryInformation);
                ConstructTileGridforHdelivery(lstDeliveryInformation);
                ConstructTileGridforpostdelivery(lstDeliveryInformation);
            }

        }


        public class navigationvideos
        {
            public string videosource;
            public string source;
        }
        public class UnreadClass
        {
            public int Notificationcount { get; set; }
        }
        private void stackAdvocatePortal_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DietandPregnancy));
        }

        private void stackDietandPregnancy_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DietandPregnancy));
        }

        private void stackTypesofDelivery_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void stackAdvocatePortal_Tapped_1(object sender, TappedRoutedEventArgs e)
        {

        }


        private void StackNextClicked_Tapped(object sender, TappedRoutedEventArgs e)
        {
            navigationvideos navigationvide = new navigationvideos();
            navigationvide.videosource = "aANj9_oeyUM";
            navigationvide.source = "Home";
            this.Frame.Navigate(typeof(PlayVideo), navigationvide);
        }

        private async void HospitalGallery_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://healthadvisor.advocatehealth.com/Classes"));

        }

        private void mySearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            this.Frame.Navigate(typeof(SearchPage), args.QueryText);
        }

        private void notificationsImg_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Notifications));
        }
        public static int unreadNotificationCount;

        public object WebBrowserOpener { get; private set; }
        //gets all the notifications(counts)
        public async void GetNotificationCount()
        {
            try
            {
                if (App.IsInternet() == true)
                {
                    List<bool> objLiseUnreadStatus = new List<bool>();
                    // App.BASE_URL+"/api/Notifications/GetNotifications?UserId=3&HospitalId=1";
                    string getAllNotifications = App.BASE_URL + "/api/Notifications/GetNotifications?UserId=" + App.userId + "&HospitalId=" + App.hospitalId;
                    var client = new HttpClient();
                    HttpResponseMessage response = await client.GetAsync(new Uri(getAllNotifications));
                    string jsonString = await response.Content.ReadAsStringAsync();
                    JArray jArr = JArray.Parse(jsonString);
                    for (var count = 0; count < jArr.Count; count++)
                    {
                        bool isRead = (bool)jArr[count]["IsRead"];
                        if (isRead != true)
                        {
                            //objUnreadClass = new UnreadClass();
                            //objUnreadClass.Notificationcount = (int)jArr[count]["IsRead"];
                            objLiseUnreadStatus.Add(isRead);
                        }
                    }
                    unreadNotificationCount = objLiseUnreadStatus.Count;
                    //objUnreadClass = new UnreadClass();
                    txtNotificationCount.Text = objLiseUnreadStatus.Count.ToString();
                }
                else
                {
                    MessageDialog msgDialog = new MessageDialog("Please check your internet connection and try again", "Internet Connection is not available");
                    msgDialog.ShowAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        private void Notificationgridtapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Notifications));
        }
        string ActiveItemHeaderName;
        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lstDeliveryInformation.Clear();
            PivotItem currentItem = e.AddedItems[0] as PivotItem;
            ActiveItemHeaderName = currentItem.Header.ToString();
            switch (ActiveItemHeaderName)
            {
                case "All":
                    DeliveryInfo(null);
                    break;
                case "General":
                    DeliveryInfo("4");
                    break;
                case "Pre Delivery":
                    DeliveryInfo("1");
                    break;

                case "Delivery":
                    DeliveryInfo("2");
                    break;

                case "Post Delivery":
                    DeliveryInfo("3");
                    break;
            }

        }
        DeliveryInformation objDeliveryInformation = new DeliveryInformation();
        //gets the content of selected pivot item by passing id
        List<DeliveryInformation> lstDeliveryInformation = new List<DeliveryInformation>();
        public async void DeliveryInfo(string id)
        {
            rngProgress.Visibility = Visibility.Visible;
            if (App.IsInternet() == true)
            {
                try
                {

                    string DeliveryInfoUri = App.BASE_URL + "/api/Tiles/GetTilesById?SUBCATEGORYID=" + id;
                    var client = new HttpClient();
                    HttpResponseMessage response = await client.GetAsync(new Uri(DeliveryInfoUri));
                    string jsonString = await response.Content.ReadAsStringAsync();

                    if (jsonString != "[]")
                    {
                        JArray jArr = JArray.Parse(jsonString);
                        for (int itemCount = 0; itemCount < jArr.Count; itemCount++)
                        {
                            objDeliveryInformation = new DeliveryInformation();
                            objDeliveryInformation.DeliveryTitle = (string)jArr[itemCount]["TITLE"];
                            objDeliveryInformation.DeliveryInfo = (string)jArr[itemCount]["CONTENT"];
                            var x = App.BASE_URL + jArr[itemCount]["TITLEIMAGE"];

                            Uri uri = new Uri(x);
                            objDeliveryInformation.DeliveryUrl = uri;
                            lstDeliveryInformation.Add(objDeliveryInformation);
                        }
                        switch (ActiveItemHeaderName)
                        {
                            case "All":
                                ConstructTileGrid(lstDeliveryInformation);
                                // grdDeliveryDetails.ItemsSource = lstDeliveryInformation;
                                break;
                            case "General":
                                ConstructTileGridforgeneral(lstDeliveryInformation);
                                break;
                            case "Pre Delivery":
                                ConstructTileGridforPredelivery(lstDeliveryInformation);
                                break;

                            case "Delivery":
                                ConstructTileGridforHdelivery(lstDeliveryInformation);
                                break;

                            case "Post Delivery":
                                ConstructTileGridforpostdelivery(lstDeliveryInformation);
                                break;
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                    rngProgress.Visibility = Visibility.Collapsed;
                    MessageDialog msgDialog = new MessageDialog("Please check your internet connectivity. If the problem persists, please contact administrator.", "Message");
                    msgDialog.ShowAsync();
                }
                rngProgress.Visibility = Visibility.Collapsed;
            }
            else
            {
                rngProgress.Visibility = Visibility.Collapsed;
                MessageDialog msgDialog = new MessageDialog("Please check your internet connection and try again", "Internet Connection is not available");
                msgDialog.ShowAsync();

            }
        }



        public void ConstructTileGrid(List<DeliveryInformation> deliveryInfoList)
        {
            tileGrid.RowDefinitions.Clear();
            tileGrid.ColumnDefinitions.Clear();
            tileGrid.Children.Clear();


            int row = 0;
            int col = -1;

            if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
            {
                AddColumnsToTileGridLandscape();


                RowDefinition rd1 = new RowDefinition();
                GridLength gl1 = new GridLength(1, GridUnitType.Star);//new GridLength(2, GridUnitType.Star);
                rd1.Height = gl1;
                tileGrid.RowDefinitions.Add(rd1);

                col = col + 1;

                Grid grdstatic1 = new Grid();

                grdstatic1.Tapped += ChildGrid_Tapped;


                Image imgStatic = new Image();
                imgStatic.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Humera.png", UriKind.Absolute));
                imgStatic.Margin = new Thickness(10, 10, 10, 10);
                imgStatic.Stretch = Stretch.Fill;
                grdstatic1.Children.Add(imgStatic);

                //Hiden TextBlock
                TextBlock hidenTB1 = new TextBlock();
                hidenTB1.Name = "TileName";
                hidenTB1.Text = "aANj9_oeyUM";
                hidenTB1.Visibility = Visibility.Collapsed;

                Grid.SetRow(hidenTB1, row);
                Grid.SetColumn(hidenTB1, col);
                grdstatic1.Children.Add(hidenTB1);

                Grid.SetRow(grdstatic1, row);
                Grid.SetColumn(grdstatic1, col);
                tileGrid.Children.Add(grdstatic1);


                col = col + 1;

                Grid grdstatic2 = new Grid();

                grdstatic2.Tapped += ChildGrid_Tapped;

                Image imgStatic1 = new Image();
                imgStatic1.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Register.png", UriKind.Absolute));
                imgStatic1.Margin = new Thickness(10, 10, 10, 10);
                imgStatic1.Stretch = Stretch.Fill;
                grdstatic2.Children.Add(imgStatic1);
                Grid.SetRow(grdstatic2, row);
                Grid.SetColumn(grdstatic2, col);
                tileGrid.Children.Add(grdstatic2);


                //Hiden TextBlock
                TextBlock hidenTB2 = new TextBlock();
                hidenTB2.Name = "TileName";
                hidenTB2.Text = "Register";
                hidenTB2.Visibility = Visibility.Collapsed;

                Grid.SetRow(hidenTB2, row);
                Grid.SetColumn(hidenTB2, col);
                grdstatic2.Children.Add(hidenTB2);

                //row = row + 1;


            }
            else if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
            {
                AddColumnsToTileGridPortrait();

                RowDefinition rd1 = new RowDefinition();
                GridLength gl1 = new GridLength(1, GridUnitType.Star);//new GridLength(2, GridUnitType.Star);
                rd1.Height = gl1;
                tileGrid.RowDefinitions.Add(rd1);

                RowDefinition rd2 = new RowDefinition();
                GridLength gl2 = new GridLength(1, GridUnitType.Star);//new GridLength(2, GridUnitType.Star);
                rd2.Height = gl2;
                tileGrid.RowDefinitions.Add(rd2);

                col = col + 1;

                Grid grdstatic1 = new Grid();
                grdstatic1.Tapped += Grdstatic1_Tapped;

                Image imgStatic = new Image();
                imgStatic.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Humera.png", UriKind.Absolute));
                imgStatic.Margin = new Thickness(10, 10, 10, 10);
                grdstatic1.Children.Add(imgStatic);
                Grid.SetRow(grdstatic1, row);
                Grid.SetColumn(grdstatic1, col);
                tileGrid.Children.Add(grdstatic1);


                TextBlock hidenTB1 = new TextBlock();
                hidenTB1.Name = "TileName";
                hidenTB1.Text = "aANj9_oeyUM";
                hidenTB1.Visibility = Visibility.Collapsed;

                Grid.SetRow(hidenTB1, row);
                Grid.SetColumn(hidenTB1, col);
                grdstatic1.Children.Add(hidenTB1);



                /*
                  Grid.SetRow(hidenTB1, row);
                Grid.SetColumn(hidenTB1, col);
                grdstatic1.Children.Add(hidenTB1);

                Grid.SetRow(grdstatic1, row);
                Grid.SetColumn(grdstatic1, col);
                tileGrid.Children.Add(grdstatic1);
                */


                row = row + 1;

                Grid grdstatic2 = new Grid();
                grdstatic2.Tapped += Grdstatic2_Tapped;
                Image imgStatic1 = new Image();
                imgStatic1.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Register.png", UriKind.Absolute));
                imgStatic1.Margin = new Thickness(10, 10, 10, 10);
                grdstatic2.Children.Add(imgStatic1);
                Grid.SetRow(grdstatic2, row);
                Grid.SetColumn(grdstatic2, col);
                tileGrid.Children.Add(grdstatic2);

                TextBlock hidenTB2 = new TextBlock();
                hidenTB2.Name = "TileName";
                hidenTB2.Text = "Register";
                hidenTB2.Visibility = Visibility.Collapsed;

                Grid.SetRow(hidenTB2, row);
                Grid.SetColumn(hidenTB2, col);
                grdstatic2.Children.Add(hidenTB2);
                // row = row + 1;


            }


            for (int i = 0; i < deliveryInfoList.Count; i++)
            {
                //ScrollViewer scrollViewer = new ScrollViewer();

                Grid childGrid = null;
                childGrid = new Grid();

                //event
                childGrid.Background = new SolidColorBrush(Colors.White);
                childGrid.BorderThickness = new Thickness(1);
                childGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));


                childGrid.Tapped += ChildGrid_Tapped;

                childGrid.Margin = new Thickness(10, 10, 10, 10);

                //row1
                RowDefinition childGridRow1 = new RowDefinition();
                GridLength cgl1 = new GridLength(1, GridUnitType.Star);
                childGridRow1.Height = cgl1;
                childGrid.RowDefinitions.Add(childGridRow1);
                //row2
                RowDefinition childGridRow2 = new RowDefinition();
                GridLength cgl2 = new GridLength(0.2, GridUnitType.Star);
                childGridRow2.Height = cgl2;

                childGrid.RowDefinitions.Add(childGridRow2);
                //row3
                RowDefinition childGridRow3 = new RowDefinition();
                GridLength cgl3 = new GridLength(0.2, GridUnitType.Star);
                childGridRow3.Height = cgl3;
                childGrid.RowDefinitions.Add(childGridRow3);

                //StackPanel deliveryInfoStackTile = new StackPanel();
                //deliveryInfoStackTile.Orientation = Orientation.Vertical;

                // < Image  Grid.Row = "0" Source = "{Binding DeliveryUrl}" Stretch = "Fill" ></ Image >
                Image img = new Image();
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.UriSource = deliveryInfoList[i].DeliveryUrl;
                img.Source = bitmapImage;
                img.Stretch = Stretch.Fill;

                Grid.SetRow(img, 0);
                childGrid.Children.Add(img);

                //  < TextBlock Grid.Row = "1" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "20"   Foreground = "#DF6C3F"  Text = "{Binding DeliveryTitle}" Margin = "10,0,0,0" ></ TextBlock >

                TextBlock tb1 = new TextBlock();
                tb1.Name = "TileName";
                if (deliveryInfoList[i].DeliveryInfo == null)
                {

                }
                else
                {
                    tb1.Text = deliveryInfoList[i].DeliveryTitle;
                }

                tb1.TextTrimming = TextTrimming.WordEllipsis;
                tb1.FontSize = 25;
                tb1.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
                tb1.FontWeight = FontWeights.SemiLight;
                tb1.Margin = new Thickness(10, 0, 0, 0);

                Grid.SetRow(tb1, 1);
                childGrid.Children.Add(tb1);

                //  < TextBlock Grid.Row = "2" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "17"  Foreground = "Black"  Text = "{Binding DeliveryInfo}"   Margin = "10,0,0,0" ></ TextBlock >

                TextBlock tb2 = new TextBlock();
                if (deliveryInfoList[i].DeliveryInfo == null)
                {

                }
                else
                {
                    tb2.Text = deliveryInfoList[i].DeliveryInfo;
                }

                tb2.TextTrimming = TextTrimming.WordEllipsis;
                tb2.FontSize = 19;
                tb2.Foreground = new SolidColorBrush(Colors.Black);
                tb2.FontWeight = FontWeights.SemiLight;
                tb2.Margin = new Thickness(10, 10, 0, 15);

                Grid.SetRow(tb2, 2);
                childGrid.Children.Add(tb2);


                if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
                {
                    if ((i + 3) > 2 * (row + 1))
                    {
                        RowDefinition rd = new RowDefinition();
                        GridLength gl = new GridLength(1, GridUnitType.Star);
                        rd.Height = gl;
                        tileGrid.RowDefinitions.Add(rd);

                        row = row + 1;
                        col = 0;
                    }
                    else
                    {
                        col = col + 1;
                    }
                }
                else if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
                {
                    if ((i + 3) > 1 * (row + 1))
                    {
                        RowDefinition rd = new RowDefinition();
                        GridLength gl = new GridLength(1, GridUnitType.Star);
                        rd.Height = gl;
                        tileGrid.RowDefinitions.Add(rd);

                        row = row + 1;
                        col = 0;
                    }
                    else
                    {
                        col = col + 1;
                    }
                }
                else
                {
                    if ((i + 1) > 2 * (row + 1))
                    {
                        RowDefinition rd = new RowDefinition();
                        GridLength gl = new GridLength(1, GridUnitType.Star);
                        rd.Height = gl;
                        tileGrid.RowDefinitions.Add(rd);

                        row = row + 1;
                        col = 0;
                    }
                    else
                    {
                        col = col + 1;
                    }
                }

                //Add to Grid
                Grid.SetRow(childGrid, row);
                Grid.SetColumn(childGrid, col);
                tileGrid.Children.Add(childGrid);

            }

        }

        private async void Grdstatic2_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // throw new NotImplementedException();  
            foreach (Grid cg in tileGrid.Children)
            {
                cg.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
            }

            ((Windows.UI.Xaml.Controls.Grid)sender).BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));//new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));


            DisplayOrientations x = orientation;
            var childrens = ((Windows.UI.Xaml.Controls.Panel)sender).Children;

            var textblocks = childrens.OfType<TextBlock>();


            foreach (TextBlock t in textblocks)
            {
                if (t.Name == "TileName")
                {
                    if (t.Text == "Diet and Pregnancy")
                    {
                        await Task.Delay(1000);
                        this.Frame.Navigate(typeof(DietandPregnancy));

                    }
                    else if (t.Text == "My Advocate Portal")
                    {
                        await Task.Delay(1000);
                        this.Frame.Navigate(typeof(MyAdvocatePage));
                    }
                    else if (t.Text == "aANj9_oeyUM")
                    {
                        //navigationvideos navigationvide = new navigationvideos();
                        //navigationvide.videosource = "aANj9_oeyUM";
                        //navigationvide.source = "Home";
                        //this.Frame.Navigate(typeof(PlayVideo), navigationvide);
                        navigationvideos navigationvide = new navigationvideos();
                        navigationvide.videosource = "VideoIDFromHomePage";
                        navigationvide.source = "Home";
                        this.Frame.Navigate(typeof(PlayVideo), navigationvide);
                    }
                    else if (t.Text == "Register")
                    {
                        // string uri = "https://healthadvisor.advocatehealth.com/Classes";
                        Uri myUri = new Uri("https://healthadvisor.advocatehealth.com/Classes", UriKind.Absolute);
                        //  await Windows.System.Launcher.LaunchUriAsync(new Uri("https://healthadvisor.advocatehealth.com/Classes"));

                        var options = new Windows.System.LauncherOptions();
                        options.DesiredRemainingView = Windows.UI.ViewManagement.ViewSizePreference.UseHalf;
                        options.DisplayApplicationPicker = true;
                        bool success = await Windows.System.Launcher.LaunchUriAsync(myUri, options);
                        if (success)
                        {
                            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://healthadvisor.advocatehealth.com/Classes"));
                        }
                        else
                        {

                        }

                    }
                }

            }

        }

        private async void Grdstatic1_Tapped(object sender, TappedRoutedEventArgs e)
        {

            foreach (Grid cg in tileGrid.Children)
            {
                cg.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
            }

            ((Windows.UI.Xaml.Controls.Grid)sender).BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));//new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));


            DisplayOrientations x = orientation;
            var childrens = ((Windows.UI.Xaml.Controls.Panel)sender).Children;

            var textblocks = childrens.OfType<TextBlock>();


            foreach (TextBlock t in textblocks)
            {
                if (t.Name == "TileName")
                {
                    if (t.Text == "Diet and Pregnancy")
                    {
                        await Task.Delay(1000);
                        this.Frame.Navigate(typeof(DietandPregnancy));

                    }
                    else if (t.Text == "My Advocate Portal")
                    {
                        await Task.Delay(1000);
                        this.Frame.Navigate(typeof(MyAdvocatePage));
                    }
                    else if (t.Text == "aANj9_oeyUM")
                    {
                        //navigationvideos navigationvide = new navigationvideos();
                        //navigationvide.videosource = "aANj9_oeyUM";
                        //navigationvide.source = "Home";
                        //this.Frame.Navigate(typeof(PlayVideo), navigationvide);
                        navigationvideos navigationvide = new navigationvideos();
                        navigationvide.videosource = "VideoIDFromHomePage";
                        navigationvide.source = "Home";
                        this.Frame.Navigate(typeof(PlayVideo), navigationvide);
                    }
                    else if (t.Text == "Register")
                    {
                        // string uri = "https://healthadvisor.advocatehealth.com/Classes";
                        Uri myUri = new Uri("https://healthadvisor.advocatehealth.com/Classes", UriKind.Absolute);
                        //  await Windows.System.Launcher.LaunchUriAsync(new Uri("https://healthadvisor.advocatehealth.com/Classes"));

                        var options = new Windows.System.LauncherOptions();
                        options.DesiredRemainingView = Windows.UI.ViewManagement.ViewSizePreference.UseHalf;
                        options.DisplayApplicationPicker = true;
                        bool success = await Windows.System.Launcher.LaunchUriAsync(myUri, options);
                        if (success)
                        {
                            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://healthadvisor.advocatehealth.com/Classes"));
                        }
                        else
                        {

                        }

                    }
                }

            }

        }

        private async void ChildGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {


            foreach (Grid cg in tileGrid.Children)
            {
                cg.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
            }

            ((Windows.UI.Xaml.Controls.Grid)sender).BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));//new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));


            DisplayOrientations x = orientation;
            var childrens = ((Windows.UI.Xaml.Controls.Panel)sender).Children;

            var textblocks = childrens.OfType<TextBlock>();


            foreach (TextBlock t in textblocks)
            {
                if (t.Name == "TileName")
                {
                    if (t.Text == "Diet and Pregnancy")
                    {
                        await Task.Delay(1000);
                        this.Frame.Navigate(typeof(DietandPregnancy));

                    }
                    else if (t.Text == "My Advocate Portal")
                    {
                        await Task.Delay(1000);
                        this.Frame.Navigate(typeof(MyAdvocatePage));
                    }
                    else if (t.Text == "aANj9_oeyUM")
                    {
                        //navigationvideos navigationvide = new navigationvideos();
                        //navigationvide.videosource = "aANj9_oeyUM";
                        //navigationvide.source = "Home";
                        //this.Frame.Navigate(typeof(PlayVideo), navigationvide);
                        navigationvideos navigationvide = new navigationvideos();
                        navigationvide.videosource = "VideoIDFromHomePage";
                        navigationvide.source = "Home";
                        this.Frame.Navigate(typeof(PlayVideo), navigationvide);
                    }
                    else if (t.Text == "Register")
                    {
                        // string uri = "https://healthadvisor.advocatehealth.com/Classes";
                        Uri myUri = new Uri("https://healthadvisor.advocatehealth.com/Classes", UriKind.Absolute);
                        //  await Windows.System.Launcher.LaunchUriAsync(new Uri("https://healthadvisor.advocatehealth.com/Classes"));

                        var options = new Windows.System.LauncherOptions();
                        options.DesiredRemainingView = Windows.UI.ViewManagement.ViewSizePreference.UseHalf;
                        options.DisplayApplicationPicker = true;
                        bool success = await Windows.System.Launcher.LaunchUriAsync(myUri, options);
                        if (success)
                        {
                            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://healthadvisor.advocatehealth.com/Classes"));
                        }
                        else
                        {

                        }

                    }
                }

            }


        }

        private void GridStaticTiles_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // throw new NotImplementedException();
        }

        private void ChildGridgeneral_Tapped(object sender, TappedRoutedEventArgs e)
        {


            foreach (Grid cg in Generaltilegrid.Children)
            {
                cg.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
            }

            ((Windows.UI.Xaml.Controls.Grid)sender).BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
            DisplayOrientations x = orientation;
            var childrens = ((Windows.UI.Xaml.Controls.Panel)sender).Children;

            var textblocks = childrens.OfType<TextBlock>();


            foreach (TextBlock t in textblocks)
            {
                if (t.Name == "TileName")
                {
                    if (t.Text == "My Advocate Portal")
                    {

                        this.Frame.Navigate(typeof(MyAdvocatePage));

                    }
                }
            }
        }


        public void AddColumnsToTileGridLandscape()
        {

            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            tileGrid.ColumnDefinitions.Add(cd1);

            //2st column
            ColumnDefinition cd2 = new ColumnDefinition();
            GridLength gl2 = new GridLength(1, GridUnitType.Star);
            cd2.Width = gl2;
            tileGrid.ColumnDefinitions.Add(cd2);


        }
        public void AddColumnsToTileGridPortrait()
        {

            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            tileGrid.ColumnDefinitions.Add(cd1);


        }
        public void AddColumnsToTileGneralGridLandscape()
        {

            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            Generaltilegrid.ColumnDefinitions.Add(cd1);

            //2st column
            ColumnDefinition cd2 = new ColumnDefinition();
            GridLength gl2 = new GridLength(1, GridUnitType.Star);
            cd2.Width = gl2;
            Generaltilegrid.ColumnDefinitions.Add(cd2);

            //3st column
            //ColumnDefinition cd3 = new ColumnDefinition();
            //GridLength gl3 = new GridLength(1, GridUnitType.Star);
            //cd3.Width = gl3;
            //tileGrid.ColumnDefinitions.Add(cd3);

            ////4st column
            //ColumnDefinition cd4 = new ColumnDefinition();
            //GridLength gl4 = new GridLength(1, GridUnitType.Star);
            //cd4.Width = gl4;
            //tileGrid.ColumnDefinitions.Add(cd4);
        }
        public void AddColumnsToTilePredeliveryGridLandscape()
        {

            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            Predeliverytilegrid.ColumnDefinitions.Add(cd1);

            //2st column
            ColumnDefinition cd2 = new ColumnDefinition();
            GridLength gl2 = new GridLength(1, GridUnitType.Star);
            cd2.Width = gl2;
            Predeliverytilegrid.ColumnDefinitions.Add(cd2);

            //3st column
            //ColumnDefinition cd3 = new ColumnDefinition();
            //GridLength gl3 = new GridLength(1, GridUnitType.Star);
            //cd3.Width = gl3;
            //tileGrid.ColumnDefinitions.Add(cd3);

            ////4st column
            //ColumnDefinition cd4 = new ColumnDefinition();
            //GridLength gl4 = new GridLength(1, GridUnitType.Star);
            //cd4.Width = gl4;
            //tileGrid.ColumnDefinitions.Add(cd4);
        }
        public void AddColumnsToTiledeliveryGridLandscape()
        {

            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            deliverytilegrid.ColumnDefinitions.Add(cd1);

            //2st column
            ColumnDefinition cd2 = new ColumnDefinition();
            GridLength gl2 = new GridLength(1, GridUnitType.Star);
            cd2.Width = gl2;
            deliverytilegrid.ColumnDefinitions.Add(cd2);

            //3st column
            //ColumnDefinition cd3 = new ColumnDefinition();
            //GridLength gl3 = new GridLength(1, GridUnitType.Star);
            //cd3.Width = gl3;
            //tileGrid.ColumnDefinitions.Add(cd3);

            ////4st column
            //ColumnDefinition cd4 = new ColumnDefinition();
            //GridLength gl4 = new GridLength(1, GridUnitType.Star);
            //cd4.Width = gl4;
            //tileGrid.ColumnDefinitions.Add(cd4);
        }
        public void AddColumnsToTilepostdeliveryGridLandscape()
        {

            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            postdeliverytilegrid.ColumnDefinitions.Add(cd1);

            //2st column
            ColumnDefinition cd2 = new ColumnDefinition();
            GridLength gl2 = new GridLength(1, GridUnitType.Star);
            cd2.Width = gl2;
            postdeliverytilegrid.ColumnDefinitions.Add(cd2);

            //3st column
            //ColumnDefinition cd3 = new ColumnDefinition();
            //GridLength gl3 = new GridLength(1, GridUnitType.Star);
            //cd3.Width = gl3;
            //tileGrid.ColumnDefinitions.Add(cd3);

            ////4st column
            //ColumnDefinition cd4 = new ColumnDefinition();
            //GridLength gl4 = new GridLength(1, GridUnitType.Star);
            //cd4.Width = gl4;
            //tileGrid.ColumnDefinitions.Add(cd4);
        }
        public void VerticallyFlipped()
        {
            tileGrid.ColumnDefinitions.Clear();
            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Auto);
            cd1.Width = gl1;
            tileGrid.ColumnDefinitions.Add(cd1);
        }

        public class DeliveryInformation
        {
            public string DeliveryTitle { get; set; }
            public string DeliveryInfo { get; set; }
            public Uri DeliveryUrl { get; set; }
        }

        private void grdDeliveryDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DeliveryInformation objDelivery = (DeliveryInformation)(sender as GridView).SelectedItem;
                string gridTitle = objDelivery.DeliveryTitle;
                if (gridTitle == "Diet and Pregnancy")
                {
                    this.Frame.Navigate(typeof(DietandPregnancy));
                }
                if (gridTitle == " My Advocate Portal")
                {
                    this.Frame.Navigate(typeof(MyAdvocatePage));
                }
            }
            catch (Exception ex)
            {
                MessageDialog msgDialog = new MessageDialog("The required resources are not downloaded.Please check your internet connectivity. If the problem persists, please contact advocate healthcare customer care associate.", "Message");
                msgDialog.ShowAsync();

            }
        }




        public void AddColumnsToTileGeneralGridPortrait()
        {

            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            Generaltilegrid.ColumnDefinitions.Add(cd1);

            ////2st column
            //ColumnDefinition cd2 = new ColumnDefinition();
            //GridLength gl2 = new GridLength(1, GridUnitType.Star);
            //cd2.Width = gl2;
            //tileGrid.ColumnDefinitions.Add(cd2);

            //3st column
            //ColumnDefinition cd3 = new ColumnDefinition();
            //GridLength gl3 = new GridLength(1, GridUnitType.Star);
            //cd3.Width = gl3;
            //tileGrid.ColumnDefinitions.Add(cd3);

            ////4st column
            //ColumnDefinition cd4 = new ColumnDefinition();
            //GridLength gl4 = new GridLength(1, GridUnitType.Star);
            //cd4.Width = gl4;
            //tileGrid.ColumnDefinitions.Add(cd4);
        }
        public void AddColumnsToTilePredeliveryGridPortrait()
        {

            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            Predeliverytilegrid.ColumnDefinitions.Add(cd1);

            ////2st column
            //ColumnDefinition cd2 = new ColumnDefinition();
            //GridLength gl2 = new GridLength(1, GridUnitType.Star);
            //cd2.Width = gl2;
            //tileGrid.ColumnDefinitions.Add(cd2);

            //3st column
            //ColumnDefinition cd3 = new ColumnDefinition();
            //GridLength gl3 = new GridLength(1, GridUnitType.Star);
            //cd3.Width = gl3;
            //tileGrid.ColumnDefinitions.Add(cd3);

            ////4st column
            //ColumnDefinition cd4 = new ColumnDefinition();
            //GridLength gl4 = new GridLength(1, GridUnitType.Star);
            //cd4.Width = gl4;
            //tileGrid.ColumnDefinitions.Add(cd4);
        }
        public void AddColumnsToTiledeliveryGridPortrait()
        {

            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            deliverytilegrid.ColumnDefinitions.Add(cd1);

            ////2st column
            //ColumnDefinition cd2 = new ColumnDefinition();
            //GridLength gl2 = new GridLength(1, GridUnitType.Star);
            //cd2.Width = gl2;
            //tileGrid.ColumnDefinitions.Add(cd2);

            //3st column
            //ColumnDefinition cd3 = new ColumnDefinition();
            //GridLength gl3 = new GridLength(1, GridUnitType.Star);
            //cd3.Width = gl3;
            //tileGrid.ColumnDefinitions.Add(cd3);

            ////4st column
            //ColumnDefinition cd4 = new ColumnDefinition();
            //GridLength gl4 = new GridLength(1, GridUnitType.Star);
            //cd4.Width = gl4;
            //tileGrid.ColumnDefinitions.Add(cd4);
        }
        public void AddColumnsToTilepostdeliveryGridPortrait()
        {

            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            postdeliverytilegrid.ColumnDefinitions.Add(cd1);

            ////2st column
            //ColumnDefinition cd2 = new ColumnDefinition();
            //GridLength gl2 = new GridLength(1, GridUnitType.Star);
            //cd2.Width = gl2;
            //tileGrid.ColumnDefinitions.Add(cd2);

            //3st column
            //ColumnDefinition cd3 = new ColumnDefinition();
            //GridLength gl3 = new GridLength(1, GridUnitType.Star);
            //cd3.Width = gl3;
            //tileGrid.ColumnDefinitions.Add(cd3);

            ////4st column
            //ColumnDefinition cd4 = new ColumnDefinition();
            //GridLength gl4 = new GridLength(1, GridUnitType.Star);
            //cd4.Width = gl4;
            //tileGrid.ColumnDefinitions.Add(cd4);
        }

        public void ConstructTileGridforgeneral(List<DeliveryInformation> deliveryInfoList)
        {
            Generaltilegrid.RowDefinitions.Clear();
            Generaltilegrid.ColumnDefinitions.Clear();
            Generaltilegrid.Children.Clear();

            if (orientation == DisplayOrientations.PortraitFlipped || orientation == DisplayOrientations.Portrait)
            {
                AddColumnsToTileGeneralGridPortrait();

            }
            else if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
            {
                AddColumnsToTileGneralGridLandscape();

            }
            else
            {
                AddColumnsToTileGneralGridLandscape();

            }

            int row = 0;
            int col = -1;
            //1st row
            RowDefinition rd1 = new RowDefinition();
            GridLength gl1 = new GridLength(2, GridUnitType.Star);
            rd1.Height = gl1;
            Generaltilegrid.RowDefinitions.Add(rd1);


            for (int i = 0; i < deliveryInfoList.Count; i++)
            {
                //ScrollViewer scrollViewer = new ScrollViewer();

                Grid childGrid = null;
                childGrid = new Grid();

                //event
                childGrid.Background = new SolidColorBrush(Colors.White);
                childGrid.BorderThickness = new Thickness(1);
                childGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));


                childGrid.Tapped += ChildGridgeneral_Tapped;

                childGrid.Margin = new Thickness(10, 10, 10, 10);

                //row1
                RowDefinition childGridRow1 = new RowDefinition();
                GridLength cgl1 = new GridLength(1, GridUnitType.Star);
                childGridRow1.Height = cgl1;
                childGrid.RowDefinitions.Add(childGridRow1);
                //row2
                RowDefinition childGridRow2 = new RowDefinition();
                GridLength cgl2 = new GridLength(0.2, GridUnitType.Star);
                childGridRow2.Height = cgl2;

                childGrid.RowDefinitions.Add(childGridRow2);
                //row3
                RowDefinition childGridRow3 = new RowDefinition();
                GridLength cgl3 = new GridLength(0.2, GridUnitType.Star);
                childGridRow3.Height = cgl3;
                childGrid.RowDefinitions.Add(childGridRow3);

                //StackPanel deliveryInfoStackTile = new StackPanel();
                //deliveryInfoStackTile.Orientation = Orientation.Vertical;

                // < Image  Grid.Row = "0" Source = "{Binding DeliveryUrl}" Stretch = "Fill" ></ Image >
                Image img = new Image();
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.UriSource = deliveryInfoList[i].DeliveryUrl;
                img.Source = bitmapImage;
                img.Stretch = Stretch.Fill;

                Grid.SetRow(img, 0);
                childGrid.Children.Add(img);

                //  < TextBlock Grid.Row = "1" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "20"   Foreground = "#DF6C3F"  Text = "{Binding DeliveryTitle}" Margin = "10,0,0,0" ></ TextBlock >

                TextBlock tb1 = new TextBlock();
                tb1.Name = "TileName";
                if (deliveryInfoList[i].DeliveryInfo == null)
                {

                }
                else
                {
                    tb1.Text = deliveryInfoList[i].DeliveryTitle;
                }

                tb1.TextTrimming = TextTrimming.WordEllipsis;
                tb1.FontSize = 25;
                tb1.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
                tb1.FontWeight = FontWeights.SemiLight;
                tb1.Margin = new Thickness(10, 0, 0, 0);

                Grid.SetRow(tb1, 1);
                childGrid.Children.Add(tb1);

                //  < TextBlock Grid.Row = "2" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "17"  Foreground = "Black"  Text = "{Binding DeliveryInfo}"   Margin = "10,0,0,0" ></ TextBlock >

                TextBlock tb2 = new TextBlock();
                if (deliveryInfoList[i].DeliveryInfo == null)
                {

                }
                else
                {
                    tb2.Text = deliveryInfoList[i].DeliveryInfo;
                }

                tb2.TextTrimming = TextTrimming.WordEllipsis;
                tb2.FontSize = 19;
                tb2.Foreground = new SolidColorBrush(Colors.Black);
                tb2.FontWeight = FontWeights.SemiLight;
                tb2.Margin = new Thickness(10, 10, 0, 15);

                Grid.SetRow(tb2, 2);
                childGrid.Children.Add(tb2);


                if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
                {
                    if ((i + 1) > 2 * (row + 1))
                    {
                        RowDefinition rd = new RowDefinition();
                        GridLength gl = new GridLength(1, GridUnitType.Star);
                        rd.Height = gl;
                        Generaltilegrid.RowDefinitions.Add(rd);

                        row = row + 1;
                        col = 0;
                    }
                    else
                    {
                        col = col + 1;
                    }
                }
                else if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
                {
                    if ((i + 1) > 1 * (row + 1))
                    {
                        RowDefinition rd = new RowDefinition();
                        GridLength gl = new GridLength(1, GridUnitType.Star);
                        rd.Height = gl;
                        Generaltilegrid.RowDefinitions.Add(rd);

                        row = row + 1;
                        col = 0;
                    }
                    else
                    {
                        col = col + 1;
                    }
                }
                else
                {
                    if ((i + 1) > 2 * (row + 1))
                    {
                        RowDefinition rd = new RowDefinition();
                        GridLength gl = new GridLength(1, GridUnitType.Star);
                        rd.Height = gl;
                        Generaltilegrid.RowDefinitions.Add(rd);

                        row = row + 1;
                        col = 0;
                    }
                    else
                    {
                        col = col + 1;
                    }
                }

                //Add to Grid
                Grid.SetRow(childGrid, row);
                Grid.SetColumn(childGrid, col);
                Generaltilegrid.Children.Add(childGrid);

            }

        }
        public void ConstructTileGridforPredelivery(List<DeliveryInformation> deliveryInfoList)
        {
            Predeliverytilegrid.RowDefinitions.Clear();
            Predeliverytilegrid.ColumnDefinitions.Clear();
            Predeliverytilegrid.Children.Clear();

            if (orientation == DisplayOrientations.PortraitFlipped || orientation == DisplayOrientations.Portrait)
            {
                AddColumnsToTilePredeliveryGridPortrait();

            }
            else if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
            {
                AddColumnsToTilePredeliveryGridLandscape();

            }
            else
            {
                AddColumnsToTilePredeliveryGridLandscape();

            }

            int row = 0;
            int col = -1;
            //1st row
            RowDefinition rd1 = new RowDefinition();
            GridLength gl1 = new GridLength(2, GridUnitType.Star);
            rd1.Height = gl1;
            Predeliverytilegrid.RowDefinitions.Add(rd1);



            for (int i = 0; i < deliveryInfoList.Count; i++)
            {
                //ScrollViewer scrollViewer = new ScrollViewer();

                Grid childGrid = null;
                childGrid = new Grid();

                //event
                childGrid.Background = new SolidColorBrush(Colors.White);
                childGrid.BorderThickness = new Thickness(1);
                childGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));


                childGrid.Tapped += ChildGridpredelivery_Tapped;

                childGrid.Margin = new Thickness(10, 10, 10, 10);

                //row1
                RowDefinition childGridRow1 = new RowDefinition();
                GridLength cgl1 = new GridLength(1, GridUnitType.Star);
                childGridRow1.Height = cgl1;
                childGrid.RowDefinitions.Add(childGridRow1);
                //row2
                RowDefinition childGridRow2 = new RowDefinition();
                GridLength cgl2 = new GridLength(0.2, GridUnitType.Star);
                childGridRow2.Height = cgl2;

                childGrid.RowDefinitions.Add(childGridRow2);
                //row3
                RowDefinition childGridRow3 = new RowDefinition();
                GridLength cgl3 = new GridLength(0.2, GridUnitType.Star);
                childGridRow3.Height = cgl3;
                childGrid.RowDefinitions.Add(childGridRow3);

                //StackPanel deliveryInfoStackTile = new StackPanel();
                //deliveryInfoStackTile.Orientation = Orientation.Vertical;

                // < Image  Grid.Row = "0" Source = "{Binding DeliveryUrl}" Stretch = "Fill" ></ Image >
                Image img = new Image();
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.UriSource = deliveryInfoList[i].DeliveryUrl;
                img.Source = bitmapImage;
                img.Stretch = Stretch.Fill;

                Grid.SetRow(img, 0);
                childGrid.Children.Add(img);

                //  < TextBlock Grid.Row = "1" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "20"   Foreground = "#DF6C3F"  Text = "{Binding DeliveryTitle}" Margin = "10,0,0,0" ></ TextBlock >

                TextBlock tb1 = new TextBlock();
                tb1.Name = "TileName";
                if (deliveryInfoList[i].DeliveryInfo == null)
                {

                }
                else
                {
                    tb1.Text = deliveryInfoList[i].DeliveryTitle;
                }

                tb1.TextTrimming = TextTrimming.WordEllipsis;
                tb1.FontSize = 25;
                tb1.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
                tb1.FontWeight = FontWeights.SemiLight;
                tb1.Margin = new Thickness(10, 0, 0, 0);

                Grid.SetRow(tb1, 1);
                childGrid.Children.Add(tb1);

                //  < TextBlock Grid.Row = "2" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "17"  Foreground = "Black"  Text = "{Binding DeliveryInfo}"   Margin = "10,0,0,0" ></ TextBlock >

                TextBlock tb2 = new TextBlock();
                if (deliveryInfoList[i].DeliveryInfo == null)
                {

                }
                else
                {
                    tb2.Text = deliveryInfoList[i].DeliveryInfo;
                }

                tb2.TextTrimming = TextTrimming.WordEllipsis;
                tb2.FontSize = 19;
                tb2.Foreground = new SolidColorBrush(Colors.Black);
                tb2.FontWeight = FontWeights.SemiLight;
                tb2.Margin = new Thickness(10, 10, 0, 15);

                Grid.SetRow(tb2, 2);
                childGrid.Children.Add(tb2);


                if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
                {
                    if ((i + 1) > 2 * (row + 1))
                    {
                        RowDefinition rd = new RowDefinition();
                        GridLength gl = new GridLength(1, GridUnitType.Star);
                        rd.Height = gl;
                        Predeliverytilegrid.RowDefinitions.Add(rd);

                        row = row + 1;
                        col = 0;
                    }
                    else
                    {
                        col = col + 1;
                    }
                }
                else if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
                {
                    if ((i + 1) > 1 * (row + 1))
                    {
                        RowDefinition rd = new RowDefinition();
                        GridLength gl = new GridLength(1, GridUnitType.Star);
                        rd.Height = gl;
                        Predeliverytilegrid.RowDefinitions.Add(rd);

                        row = row + 1;
                        col = 0;
                    }
                    else
                    {
                        col = col + 1;
                    }
                }
                else
                {
                    if ((i + 1) > 2 * (row + 1))
                    {
                        RowDefinition rd = new RowDefinition();
                        GridLength gl = new GridLength(1, GridUnitType.Star);
                        rd.Height = gl;
                        Predeliverytilegrid.RowDefinitions.Add(rd);

                        row = row + 1;
                        col = 0;
                    }
                    else
                    {
                        col = col + 1;
                    }
                }

                //Add to Grid
                Grid.SetRow(childGrid, row);
                Grid.SetColumn(childGrid, col);
                Predeliverytilegrid.Children.Add(childGrid);

            }

        }
        private async void ChildGridpredelivery_Tapped(object sender, TappedRoutedEventArgs e)
        {


            foreach (Grid cg in Predeliverytilegrid.Children)
            {
                cg.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
            }

           ((Windows.UI.Xaml.Controls.Grid)sender).BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));

            var childrens = ((Windows.UI.Xaml.Controls.Panel)sender).Children;

            var textblocks = childrens.OfType<TextBlock>();


            foreach (TextBlock t in textblocks)
            {
                if (t.Name == "TileName")
                {
                    if (t.Text == "Diet and Pregnancy")
                    {

                        this.Frame.Navigate(typeof(DietandPregnancy));

                    }
                }
            }
        }
        private async void ChildGriddelivery_Tapped(object sender, TappedRoutedEventArgs e)
        {


            foreach (Grid cg in deliverytilegrid.Children)
            {
                cg.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
            }

           ((Windows.UI.Xaml.Controls.Grid)sender).BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
        }
        public void ConstructTileGridforHdelivery(List<DeliveryInformation> deliveryInfoList)
        {
            deliverytilegrid.RowDefinitions.Clear();
            deliverytilegrid.ColumnDefinitions.Clear();
            deliverytilegrid.Children.Clear();

            if (orientation == DisplayOrientations.PortraitFlipped || orientation == DisplayOrientations.Portrait)
            {
                AddColumnsToTiledeliveryGridPortrait();

            }
            else if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
            {
                AddColumnsToTiledeliveryGridLandscape();

            }
            else
            {
                AddColumnsToTiledeliveryGridLandscape();

            }

            int row = 0;
            int col = -1;
            //1st row
            if (lstDeliveryInformation.Count < 3)
            {
                RowDefinition rd1 = new RowDefinition();
                GridLength gl1 = new GridLength(330);
                rd1.Height = gl1;
                deliverytilegrid.RowDefinitions.Add(rd1);
            }
            else
            {
                RowDefinition rd1 = new RowDefinition();
                GridLength gl1 = new GridLength(1, GridUnitType.Star);
                rd1.Height = gl1;
                deliverytilegrid.RowDefinitions.Add(rd1);
            }


            for (int i = 0; i < deliveryInfoList.Count; i++)
            {
                //ScrollViewer scrollViewer = new ScrollViewer();

                Grid childGrid = null;
                childGrid = new Grid();

                //event
                childGrid.Background = new SolidColorBrush(Colors.White);
                childGrid.BorderThickness = new Thickness(1);
                childGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));


                childGrid.Tapped += ChildGriddelivery_Tapped;

                childGrid.Margin = new Thickness(10, 10, 10, 10);

                //row1
                RowDefinition childGridRow1 = new RowDefinition();
                GridLength cgl1 = new GridLength(1, GridUnitType.Star);
                childGridRow1.Height = cgl1;
                childGrid.RowDefinitions.Add(childGridRow1);
                //row2
                RowDefinition childGridRow2 = new RowDefinition();
                GridLength cgl2 = new GridLength(0.2, GridUnitType.Star);
                childGridRow2.Height = cgl2;

                childGrid.RowDefinitions.Add(childGridRow2);
                //row3
                RowDefinition childGridRow3 = new RowDefinition();
                GridLength cgl3 = new GridLength(0.2, GridUnitType.Star);
                childGridRow3.Height = cgl3;
                childGrid.RowDefinitions.Add(childGridRow3);

                //StackPanel deliveryInfoStackTile = new StackPanel();
                //deliveryInfoStackTile.Orientation = Orientation.Vertical;

                // < Image  Grid.Row = "0" Source = "{Binding DeliveryUrl}" Stretch = "Fill" ></ Image >
                Image img = new Image();
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.UriSource = deliveryInfoList[i].DeliveryUrl;
                img.Source = bitmapImage;
                img.Stretch = Stretch.Fill;

                Grid.SetRow(img, 0);
                childGrid.Children.Add(img);

                //  < TextBlock Grid.Row = "1" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "20"   Foreground = "#DF6C3F"  Text = "{Binding DeliveryTitle}" Margin = "10,0,0,0" ></ TextBlock >

                TextBlock tb1 = new TextBlock();
                tb1.Name = "TileName";
                if (deliveryInfoList[i].DeliveryInfo == null)
                {

                }
                else
                {
                    tb1.Text = deliveryInfoList[i].DeliveryTitle;
                }

                tb1.TextTrimming = TextTrimming.WordEllipsis;
                tb1.FontSize = 25;
                tb1.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
                tb1.FontWeight = FontWeights.SemiLight;
                tb1.Margin = new Thickness(10, 0, 0, 0);

                Grid.SetRow(tb1, 1);
                childGrid.Children.Add(tb1);

                //  < TextBlock Grid.Row = "2" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "17"  Foreground = "Black"  Text = "{Binding DeliveryInfo}"   Margin = "10,0,0,0" ></ TextBlock >

                TextBlock tb2 = new TextBlock();
                if (deliveryInfoList[i].DeliveryInfo == null)
                {

                }
                else
                {
                    tb2.Text = deliveryInfoList[i].DeliveryInfo;
                }

                tb2.TextTrimming = TextTrimming.WordEllipsis;
                tb2.FontSize = 19;
                tb2.Foreground = new SolidColorBrush(Colors.Black);
                tb2.FontWeight = FontWeights.SemiLight;
                tb2.Margin = new Thickness(10, 8, 0, 8);

                Grid.SetRow(tb2, 2);
                childGrid.Children.Add(tb2);
                if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
                {
                    if ((i + 1) > 2 * (row + 1))
                    {
                        if (deliveryInfoList.Count < 3)
                        {

                            RowDefinition rd = new RowDefinition();
                            GridLength gl = new GridLength(330);
                            rd.Height = gl;
                            deliverytilegrid.RowDefinitions.Add(rd);

                            row = row + 1;
                            col = 0;
                        }
                        else
                        {
                            RowDefinition rd = new RowDefinition();
                            GridLength gl = new GridLength(1, GridUnitType.Star);
                            rd.Height = gl;
                            deliverytilegrid.RowDefinitions.Add(rd);

                            row = row + 1;
                            col = 0;

                        }
                    }

                    else
                    {
                        col = col + 1;
                    }
                }

                else if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
                {
                    if ((i + 1) > 1 * (row + 1))
                    {
                        if (deliveryInfoList.Count < 3)
                        {
                            RowDefinition rd = new RowDefinition();
                            GridLength gl = new GridLength(330);
                            rd.Height = gl;
                            deliverytilegrid.RowDefinitions.Add(rd);

                            row = row + 1;
                            col = 0;
                        }
                        else
                        {
                            RowDefinition rd = new RowDefinition();
                            GridLength gl = new GridLength(1, GridUnitType.Star);
                            rd.Height = gl;
                            deliverytilegrid.RowDefinitions.Add(rd);

                            row = row + 1;
                            col = 0;
                        }
                    }
                    else
                    {
                        col = col + 1;
                    }
                }
                else
                {
                    if ((i + 1) > 2 * (row + 1))
                    {
                        RowDefinition rd = new RowDefinition();
                        GridLength gl = new GridLength(1, GridUnitType.Star);
                        rd.Height = gl;
                        deliverytilegrid.RowDefinitions.Add(rd);

                        row = row + 1;
                        col = 0;
                    }
                    else
                    {
                        col = col + 1;
                    }
                }

                //if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
                //{
                //    if ((i + 1) > 2 * (row + 1))
                //    {
                //        RowDefinition rd = new RowDefinition();
                //        GridLength gl = new GridLength(300);
                //        rd.Height = gl;
                //        deliverytilegrid.RowDefinitions.Add(rd);

                //        row = row + 1;
                //        col = 0;
                //    }
                //    else
                //    {
                //        col = col + 1;
                //    }
                //}
                //else if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
                //{
                //    if ((i + 1) > 1 * (row + 1))
                //    {
                //        RowDefinition rd = new RowDefinition();
                //        GridLength gl = new GridLength(1, GridUnitType.Star);
                //        rd.Height = gl;
                //        deliverytilegrid.RowDefinitions.Add(rd);

                //        row = row + 1;
                //        col = 0;
                //    }
                //    else
                //    {
                //        col = col + 1;
                //    }
                //}
                //else
                //{
                //    if ((i + 1) > 2 * (row + 1))
                //    {
                //        RowDefinition rd = new RowDefinition();
                //        GridLength gl = new GridLength(300);
                //        rd.Height = gl;
                //        deliverytilegrid.RowDefinitions.Add(rd);

                //        row = row + 1;
                //        col = 0;
                //    }
                //    else
                //    {
                //        col = col + 1;
                //    }
                //}

                //Add to Grid
                Grid.SetRow(childGrid, row);
                Grid.SetColumn(childGrid, col);
                deliverytilegrid.Children.Add(childGrid);

            }

        }
        public void ConstructTileGridforpostdelivery(List<DeliveryInformation> deliveryInfoList)
        {
            postdeliverytilegrid.RowDefinitions.Clear();
            postdeliverytilegrid.ColumnDefinitions.Clear();
            postdeliverytilegrid.Children.Clear();

            if (orientation == DisplayOrientations.PortraitFlipped || orientation == DisplayOrientations.Portrait)
            {
                AddColumnsToTilepostdeliveryGridPortrait();

            }
            else if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
            {
                AddColumnsToTilepostdeliveryGridLandscape();

            }
            else
            {
                AddColumnsToTilepostdeliveryGridLandscape();

            }

            int row = 0;
            int col = -1;
            ////1st row
            //RowDefinition rd1 = new RowDefinition();
            //GridLength gl1 = new GridLength(1, GridUnitType.Star);
            //rd1.Height = gl1;
            //postdeliverytilegrid.RowDefinitions.Add(rd1);
            if (lstDeliveryInformation.Count < 3)
            {
                RowDefinition rd11 = new RowDefinition();
                GridLength gl11 = new GridLength(330);
                rd11.Height = gl11;
                postdeliverytilegrid.RowDefinitions.Add(rd11);
            }
            else
            {
                RowDefinition rd12 = new RowDefinition();
                GridLength gl12 = new GridLength(1, GridUnitType.Star);
                rd12.Height = gl12;
                postdeliverytilegrid.RowDefinitions.Add(rd12);
            }

            for (int i = 0; i < deliveryInfoList.Count; i++)
            {
                //ScrollViewer scrollViewer = new ScrollViewer();

                Grid childGrid = null;
                childGrid = new Grid();

                //event
                childGrid.Background = new SolidColorBrush(Colors.White);
                childGrid.BorderThickness = new Thickness(1);
                childGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));


                childGrid.Tapped += ChildGridpostdelivery_Tapped;

                childGrid.Margin = new Thickness(10, 10, 10, 10);

                //row1
                RowDefinition childGridRow1 = new RowDefinition();
                GridLength cgl1 = new GridLength(1, GridUnitType.Star);
                childGridRow1.Height = cgl1;
                childGrid.RowDefinitions.Add(childGridRow1);
                //row2
                RowDefinition childGridRow2 = new RowDefinition();
                GridLength cgl2 = new GridLength(0.2, GridUnitType.Star);
                childGridRow2.Height = cgl2;

                childGrid.RowDefinitions.Add(childGridRow2);
                //row3
                RowDefinition childGridRow3 = new RowDefinition();
                GridLength cgl3 = new GridLength(0.2, GridUnitType.Star);
                childGridRow3.Height = cgl3;
                childGrid.RowDefinitions.Add(childGridRow3);

                //StackPanel deliveryInfoStackTile = new StackPanel();
                //deliveryInfoStackTile.Orientation = Orientation.Vertical;

                // < Image  Grid.Row = "0" Source = "{Binding DeliveryUrl}" Stretch = "Fill" ></ Image >
                Image img = new Image();
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.UriSource = deliveryInfoList[i].DeliveryUrl;
                img.Source = bitmapImage;
                img.Stretch = Stretch.Fill;

                Grid.SetRow(img, 0);
                childGrid.Children.Add(img);

                //  < TextBlock Grid.Row = "1" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "20"   Foreground = "#DF6C3F"  Text = "{Binding DeliveryTitle}" Margin = "10,0,0,0" ></ TextBlock >

                TextBlock tb1 = new TextBlock();
                tb1.Name = "TileName";
                if (deliveryInfoList[i].DeliveryInfo == null)
                {

                }
                else
                {
                    tb1.Text = deliveryInfoList[i].DeliveryTitle;
                }

                tb1.TextTrimming = TextTrimming.WordEllipsis;
                tb1.FontSize = 25;
                tb1.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
                tb1.FontWeight = FontWeights.SemiLight;
                tb1.Margin = new Thickness(10, 0, 0, 0);

                Grid.SetRow(tb1, 1);
                childGrid.Children.Add(tb1);

                //  < TextBlock Grid.Row = "2" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "17"  Foreground = "Black"  Text = "{Binding DeliveryInfo}"   Margin = "10,0,0,0" ></ TextBlock >

                TextBlock tb2 = new TextBlock();
                if (deliveryInfoList[i].DeliveryInfo == null)
                {

                }
                else
                {
                    tb2.Text = deliveryInfoList[i].DeliveryInfo;
                }

                tb2.TextTrimming = TextTrimming.WordEllipsis;
                tb2.FontSize = 19;
                tb2.Foreground = new SolidColorBrush(Colors.Black);
                tb2.FontWeight = FontWeights.SemiLight;
                tb2.Margin = new Thickness(10, 8, 0, 8);

                Grid.SetRow(tb2, 2);
                childGrid.Children.Add(tb2);
                if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
                {
                    if ((i + 1) > 2 * (row + 1))
                    {
                        if (lstDeliveryInformation.Count < 3)
                        {

                            RowDefinition rd = new RowDefinition();
                            GridLength gl = new GridLength(330);
                            rd.Height = gl;
                            postdeliverytilegrid.RowDefinitions.Add(rd);

                            row = row + 1;
                            col = 0;
                        }
                        else
                        {
                            RowDefinition rd = new RowDefinition();
                            GridLength gl = new GridLength(1, GridUnitType.Star);
                            rd.Height = gl;
                            postdeliverytilegrid.RowDefinitions.Add(rd);

                            row = row + 1;
                            col = 0;

                        }
                    }

                    else
                    {
                        col = col + 1;
                    }
                }

                else if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
                {
                    if ((i + 1) > 1 * (row + 1))
                    {
                        if (lstDeliveryInformation.Count < 3)
                        {
                            RowDefinition rd = new RowDefinition();
                            GridLength gl = new GridLength(330);
                            rd.Height = gl;
                            postdeliverytilegrid.RowDefinitions.Add(rd);

                            row = row + 1;
                            col = 0;
                        }
                        else
                        {
                            RowDefinition rd = new RowDefinition();
                            GridLength gl = new GridLength(1, GridUnitType.Star);
                            rd.Height = gl;
                            postdeliverytilegrid.RowDefinitions.Add(rd);

                            row = row + 1;
                            col = 0;
                        }
                    }
                    else
                    {
                        col = col + 1;
                    }
                }
                else
                {
                    if ((i + 1) > 2 * (row + 1))
                    {
                        RowDefinition rd = new RowDefinition();
                        GridLength gl = new GridLength(1, GridUnitType.Star);
                        rd.Height = gl;
                        postdeliverytilegrid.RowDefinitions.Add(rd);

                        row = row + 1;
                        col = 0;
                    }
                    else
                    {
                        col = col + 1;
                    }
                }

                //if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
                //{
                //    if ((i + 1) > 2 * (row + 1))
                //    {
                //        RowDefinition rd = new RowDefinition();
                //        GridLength gl = new GridLength(1, GridUnitType.Star);
                //        rd.Height = gl;
                //        postdeliverytilegrid.RowDefinitions.Add(rd);

                //        row = row + 1;
                //        col = 0;
                //    }
                //    else
                //    {
                //        col = col + 1;
                //    }
                //}
                //else if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
                //{
                //    if ((i + 1) > 1 * (row + 1))
                //    {
                //        RowDefinition rd = new RowDefinition();
                //        GridLength gl = new GridLength(1, GridUnitType.Star);
                //        rd.Height = gl;
                //        postdeliverytilegrid.RowDefinitions.Add(rd);

                //        row = row + 1;
                //        col = 0;
                //    }
                //    else
                //    {
                //        col = col + 1;
                //    }
                //}
                //else
                //{
                //    if ((i + 1) > 2 * (row + 1))
                //    {
                //        RowDefinition rd = new RowDefinition();
                //        GridLength gl = new GridLength(1, GridUnitType.Star);
                //        rd.Height = gl;
                //        postdeliverytilegrid.RowDefinitions.Add(rd);

                //        row = row + 1;
                //        col = 0;
                //    }
                //    else
                //    {
                //        col = col + 1;
                //    }
                //}

                //Add to Grid
                Grid.SetRow(childGrid, row);
                Grid.SetColumn(childGrid, col);
                postdeliverytilegrid.Children.Add(childGrid);

            }

        }
        private async void ChildGridpostdelivery_Tapped(object sender, TappedRoutedEventArgs e)
        {


            foreach (Grid cg in postdeliverytilegrid.Children)
            {
                cg.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
            }

           ((Windows.UI.Xaml.Controls.Grid)sender).BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
        }
    }

}