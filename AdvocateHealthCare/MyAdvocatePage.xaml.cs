using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Text;
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
    public sealed partial class MyAdvocatePage : Page
    {
        public DisplayOrientations orientation = DisplayOrientations.Landscape;
        public MyAdvocatePage()
        {
            this.InitializeComponent();
            getback.Visibility = Visibility.Collapsed;
            txtNotificationCount.Text = HomePage.unreadNotificationCount.ToString();//Displays the notification count
            DisplayProperties.OrientationChanged += Page_OrientationChanged;
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
        }
        public void Page_OrientationChanged(object sender)
        {
            //The orientation of the device is ...
            orientation = DisplayProperties.CurrentOrientation;
            if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
            {
                ConstructTileGridforgeneral(lstDeliveryInformation);
                ConstructTileGridforPredelivery(lstDeliveryInformation);
                ConstructTileGridforDelivery(lstDeliveryInformation);
                ConstructTileGridforPostDelivery(lstDeliveryInformation);
            }

            if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
            {
                ConstructTileGridforgeneral(lstDeliveryInformation);
                ConstructTileGridforPredelivery(lstDeliveryInformation);
                ConstructTileGridforDelivery(lstDeliveryInformation);
                ConstructTileGridforPostDelivery(lstDeliveryInformation);
            }

        }
        private void Viewbox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (grdAddNotes.Visibility == Visibility.Visible)
            {
                grdAddNotes.Visibility = Visibility.Collapsed;
                GridDiet.ColumnDefinitions.RemoveAt(1);
                getback.Visibility = Visibility.Visible;
            }
        }

        private void GetbackButton_Click(object sender, TappedRoutedEventArgs e)
        {
            ColumnDefinition column = new ColumnDefinition();
            column.Width = new GridLength(0.30, GridUnitType.Star);
            GridDiet.ColumnDefinitions.Add(column);
            Grid.SetColumn(grdAddNotes, 1);

            grdAddNotes.Visibility = Visibility.Visible;
            getback.Visibility = Visibility.Collapsed;
        }
        string ActiveItemHeaderName;
        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
        List<DeliveryInformation> lstDeliveryInformation = new List<DeliveryInformation>();
        //gets the content of selected pivot item by passing id
        public async void DeliveryInfo(string id)
        {
            lstDeliveryInformation.Clear();
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
                                // grdDeliveryDetails.ItemsSource = lstDeliveryInformation;
                                break;
                            case "General":
                                ConstructTileGridforgeneral(lstDeliveryInformation);
                                break;
                            case "Pre Delivery":
                                ConstructTileGridforPredelivery(lstDeliveryInformation);
                                break;

                            case "Delivery":
                                ConstructTileGridforDelivery(lstDeliveryInformation);
                                break;

                            case "Post Delivery":
                                ConstructTileGridforPostDelivery(lstDeliveryInformation);
                                break;
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                    MessageDialog msgDialog = new MessageDialog("Please check your internet connectivity. If the problem persists, please contact administrator.", "Message");
                    msgDialog.ShowAsync();
                }
            }
            else
            {
                MessageDialog msgDialog = new MessageDialog("Please check your internet connection and try again", "Internet Connection is not available");
                msgDialog.ShowAsync();
            }
        }


        public class DeliveryInformation
        {
            public string DeliveryTitle { get; set; }
            public string DeliveryInfo { get; set; }
            public Uri DeliveryUrl { get; set; }
        }
        public static string qtapped = "1";
        //saves journals/question
        private void saveJorQnotes(object sender, RoutedEventArgs e)
        {
            if (App.IsInternet())
            {


                try
                {
                    if (JorQtext.Text.Trim() != "" || JorQtext1.Text.Trim() != "")
                    {
                        ProfileJournal profilejournal = new ProfileJournal();
                        profilejournal.CreatedDate = Convert.ToString(DateTime.Now);
                        profilejournal.ProfileJournalID = null;

                        profilejournal.ProfileID = App.userId;

                        profilejournal.JournalInfo = JorQtext.Text;
                        profilejournal.JournalAsset = null;
                        if (qtapped == "2")
                        {
                            profilejournal.JournalTypeID = 2;
                            profilejournal.JournalTitle = "New Question Entry";
                            profilejournal.JournalInfo = JorQtext1.Text;
                        }
                        else
                        {
                            profilejournal.JournalTypeID = 1;
                            profilejournal.JournalTitle = "New Journal Entry";
                            profilejournal.JournalInfo = JorQtext.Text;
                        }
                        profilejournal.LoggedInUser = App.userName;


                        var serializedPatchDoc = JsonConvert.SerializeObject(profilejournal);
                        var method = new HttpMethod("POST");
                        var request = new HttpRequestMessage(method,
                        App.BASE_URL + "/api/ProfileJournal/SaveProfileJournal")


                        {
                            Content = new StringContent(serializedPatchDoc,
                            System.Text.Encoding.Unicode, "application/json")
                        };


                        HttpClient client = new HttpClient();
                        var result = client.SendAsync(request).Result;
                        client.Dispose();

                        if (result.IsSuccessStatusCode == true)
                        {


                            MessageDialog msgDialog = new MessageDialog("Successfully saved.", "Success");
                            msgDialog.ShowAsync();
                        }
                        else {
                            MessageDialog msgDialog = new MessageDialog("Unsuccessful.", "Failure");
                            msgDialog.ShowAsync();
                        }
                    }
                    else
                    {
                        if (qtapped == "1")
                        {
                            MessageDialog msgDialog = new MessageDialog("Please add your journal.", "Incomplete data");
                            msgDialog.ShowAsync();
                        }
                        else
                        {
                            MessageDialog msgDialog = new MessageDialog("Please add your questions.", "Incomplete data");
                            msgDialog.ShowAsync();
                        }
                    }


                    //this.Frame.Navigate(typeof(DietandPregnancy));
                }

                catch (Exception ex)
                {
                    string meg = ex.StackTrace;
                    MessageDialog msgDialog = new MessageDialog(ex.Message, "Message");
                    msgDialog.ShowAsync();
                }

                JorQtext1.Text = "";
                JorQtext.Text = "";
            }
            else
            {
                MessageDialog msgDialog = new MessageDialog("Please check your internet connection and try again", "Internet Connection is not available");
                msgDialog.ShowAsync();

            }
        }
        public class ProfileJournal
        {
            public string ProfileJournalID { get; set; }
            public int ProfileID { get; set; }
            public string JournalTitle { get; set; }
            public string JournalInfo { get; set; }
            public string JournalAsset { get; set; }
            public byte JournalTypeID { get; set; }
            public string CreatedDate { get; set; }
            public string CreatedBy { get; set; }
            public string LoggedInUser { get; set; }

        }
        private void Journaltext_Tapped(object sender, TappedRoutedEventArgs e)
        {
            text.Background = new SolidColorBrush(Color.FromArgb(255, 229, 103, 58));
            text2.Background = new SolidColorBrush(Color.FromArgb(224, 224, 224, 224));
            txtjournal.Foreground = new SolidColorBrush(Colors.White);
            txtquestions.Foreground = new SolidColorBrush(Colors.Black);
            JorQtext1.Visibility = Visibility.Collapsed;
            JorQtext.Visibility = Visibility.Visible;
            qtapped = "1";
        }


        private void Questionstext_Tapped(object sender, TappedRoutedEventArgs e)
        {
            text2.Background = new SolidColorBrush(Color.FromArgb(255, 229, 103, 58));
            text.Background = new SolidColorBrush(Color.FromArgb(224, 224, 224, 224));
            txtjournal.Foreground = new SolidColorBrush(Colors.Black);
            txtquestions.Foreground = new SolidColorBrush(Colors.White);
            qtapped = "2";
            JorQtext.Visibility = Visibility.Collapsed;
            JorQtext1.Visibility = Visibility.Visible;
        }


        private void Notificationgridtapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Notifications));
        }
        //finds the results of searched query
        private void mySearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            this.Frame.Navigate(typeof(SearchPage), args.QueryText);
        }

        private void CloseImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (grdAddNotes.Visibility == Visibility.Visible)
            {
                grdAddNotes.Visibility = Visibility.Collapsed;
                GridDiet.ColumnDefinitions.RemoveAt(1);
                getback.Visibility = Visibility.Visible;
            }
        }
        public void ConstructTileGridforgeneral(List<DeliveryInformation> deliveryInfoList)
        {
            AdvocateGeneraltilegrid.RowDefinitions.Clear();
            AdvocateGeneraltilegrid.ColumnDefinitions.Clear();
            AdvocateGeneraltilegrid.Children.Clear();

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
            AdvocateGeneraltilegrid.RowDefinitions.Add(rd1);


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
                tb1.FontSize = 20;
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
                tb2.FontSize = 17;
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
                        AdvocateGeneraltilegrid.RowDefinitions.Add(rd);

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
                        AdvocateGeneraltilegrid.RowDefinitions.Add(rd);

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
                        AdvocateGeneraltilegrid.RowDefinitions.Add(rd);

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
                AdvocateGeneraltilegrid.Children.Add(childGrid);

            }

        }
        public void AddColumnsToTileGeneralGridPortrait()
        {
            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            AdvocateGeneraltilegrid.ColumnDefinitions.Add(cd1);
        }

        public void AddColumnsToTileGneralGridLandscape()
        {

            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            AdvocateGeneraltilegrid.ColumnDefinitions.Add(cd1);

            //2st column
            ColumnDefinition cd2 = new ColumnDefinition();
            GridLength gl2 = new GridLength(1, GridUnitType.Star);
            cd2.Width = gl2;
            AdvocateGeneraltilegrid.ColumnDefinitions.Add(cd2);
        }
        private void ChildGridgeneral_Tapped(object sender, TappedRoutedEventArgs e)
        {


            foreach (Grid cg in AdvocateGeneraltilegrid.Children)
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
        private async void ChildGridpredelivery_Tapped(object sender, TappedRoutedEventArgs e)
        {


            foreach (Grid cg in AdvocatePredeliverytilegrid.Children)
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
        public void ConstructTileGridforPredelivery(List<DeliveryInformation> deliveryInfoList)
        {
            AdvocatePredeliverytilegrid.RowDefinitions.Clear();
            AdvocatePredeliverytilegrid.ColumnDefinitions.Clear();
            AdvocatePredeliverytilegrid.Children.Clear();

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
            AdvocatePredeliverytilegrid.RowDefinitions.Add(rd1);


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
                tb1.FontSize = 20;
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
                tb2.FontSize = 17;
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
                        AdvocatePredeliverytilegrid.RowDefinitions.Add(rd);

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
                        AdvocatePredeliverytilegrid.RowDefinitions.Add(rd);

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
                        AdvocatePredeliverytilegrid.RowDefinitions.Add(rd);

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
                AdvocatePredeliverytilegrid.Children.Add(childGrid);

            }

        }

        public void AddColumnsToTilePredeliveryGridLandscape()
        {
            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            AdvocatePredeliverytilegrid.ColumnDefinitions.Add(cd1);

            //2st column
            ColumnDefinition cd2 = new ColumnDefinition();
            GridLength gl2 = new GridLength(1, GridUnitType.Star);
            cd2.Width = gl2;
            AdvocatePredeliverytilegrid.ColumnDefinitions.Add(cd2);

        }
        public void AddColumnsToTilePredeliveryGridPortrait()
        {

            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            AdvocatePredeliverytilegrid.ColumnDefinitions.Add(cd1);
        }

        public void ConstructTileGridforDelivery(List<DeliveryInformation> deliveryInfoList)
        {
            AdvocateDeliverytilegrid.RowDefinitions.Clear();
            AdvocateDeliverytilegrid.ColumnDefinitions.Clear();
            AdvocateDeliverytilegrid.Children.Clear();

            if (orientation == DisplayOrientations.PortraitFlipped || orientation == DisplayOrientations.Portrait)
            {
                AddColumnsToTileDeliveryGridPortrait();

            }
            else if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
            {
                AddColumnsToTileDeliveryGridLandscape();

            }
            else
            {
                AddColumnsToTileDeliveryGridLandscape();
            }

            int row = 0;
            int col = -1;
            if (lstDeliveryInformation.Count < 3)
            {
                RowDefinition rd1 = new RowDefinition();
                GridLength gl1 = new GridLength(330);
                rd1.Height = gl1;
                AdvocateDeliverytilegrid.RowDefinitions.Add(rd1);
            }
            else
            {
                RowDefinition rd1 = new RowDefinition();
                GridLength gl1 = new GridLength(1, GridUnitType.Star);
                rd1.Height = gl1;
                AdvocateDeliverytilegrid.RowDefinitions.Add(rd1);
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
                tb1.FontSize = 20;
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
                tb2.FontSize = 17;
                tb2.Foreground = new SolidColorBrush(Colors.Black);
                tb2.FontWeight = FontWeights.SemiLight;
                tb2.Margin = new Thickness(10, 0, 0, 0);

                Grid.SetRow(tb2, 2);
                childGrid.Children.Add(tb2);


                if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
                {
                    if ((i + 1) > 2 * (row + 1))
                    {
                        if (deliveryInfoList.Count < 3)
                        {

                            RowDefinition rd = new RowDefinition();
                            GridLength gl = new GridLength(330, GridUnitType.Star);
                            rd.Height = gl;
                            AdvocateDeliverytilegrid.RowDefinitions.Add(rd);

                            row = row + 1;
                            col = 0;
                        }
                        else
                        {
                            RowDefinition rd = new RowDefinition();
                            GridLength gl = new GridLength(1, GridUnitType.Star);
                            rd.Height = gl;
                            AdvocateDeliverytilegrid.RowDefinitions.Add(rd);

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
                            AdvocateDeliverytilegrid.RowDefinitions.Add(rd);

                            row = row + 1;
                            col = 0;
                        }
                        else
                        {
                            RowDefinition rd = new RowDefinition();
                            GridLength gl = new GridLength(1, GridUnitType.Star);
                            rd.Height = gl;
                            AdvocateDeliverytilegrid.RowDefinitions.Add(rd);

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
                        AdvocateDeliverytilegrid.RowDefinitions.Add(rd);

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
                AdvocateDeliverytilegrid.Children.Add(childGrid);

            }

        }
        private async void ChildGriddelivery_Tapped
(object sender, TappedRoutedEventArgs e)
        {


            foreach (Grid cg in AdvocateDeliverytilegrid.Children)
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

        public void AddColumnsToTileDeliveryGridLandscape()
        {
            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            AdvocateDeliverytilegrid.ColumnDefinitions.Add(cd1);

            //2st column
            ColumnDefinition cd2 = new ColumnDefinition();
            GridLength gl2 = new GridLength(1, GridUnitType.Star);
            cd2.Width = gl2;
            AdvocateDeliverytilegrid.ColumnDefinitions.Add(cd2);

        }
        public void AddColumnsToTileDeliveryGridPortrait()
        {

            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            AdvocateDeliverytilegrid.ColumnDefinitions.Add(cd1);
        }

        public void ConstructTileGridforPostDelivery(List<DeliveryInformation> deliveryInfoList)
        {
            AdvocatePostdeliverytilegrid.RowDefinitions.Clear();
            AdvocatePostdeliverytilegrid.ColumnDefinitions.Clear();
            AdvocatePostdeliverytilegrid.Children.Clear();

            if (orientation == DisplayOrientations.PortraitFlipped || orientation == DisplayOrientations.Portrait)
            {
                AddColumnsToTilePreDeliveryGridPortrait();

            }
            else if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
            {
                AddColumnsToTilePreDeliveryGridLandscape();

            }
            else
            {
                AddColumnsToTilePreDeliveryGridLandscape();
            }

            int row = 0;
            int col = -1;
            if (lstDeliveryInformation.Count < 3)
            {
                RowDefinition rd5 = new RowDefinition();
                GridLength gl7 = new GridLength(330);
                rd5.Height = gl7;
                AdvocatePostdeliverytilegrid.RowDefinitions.Add(rd5);
            }
            else {
                //1st row
                RowDefinition rd1 = new RowDefinition();
                GridLength gl1 = new GridLength(2, GridUnitType.Star);
                rd1.Height = gl1;
                AdvocatePostdeliverytilegrid.RowDefinitions.Add(rd1);

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
                tb1.FontSize = 20;
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
                tb2.FontSize = 17;
                tb2.Foreground = new SolidColorBrush(Colors.Black);
                tb2.FontWeight = FontWeights.SemiLight;
                tb2.Margin = new Thickness(10, 10, 0, 15);

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
                            AdvocatePostdeliverytilegrid.RowDefinitions.Add(rd);

                            row = row + 1;
                            col = 0;
                        }
                        else
                        {
                            RowDefinition rd = new RowDefinition();
                            GridLength gl = new GridLength(1, GridUnitType.Star);
                            rd.Height = gl;
                            AdvocatePostdeliverytilegrid.RowDefinitions.Add(rd);

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
                            AdvocatePostdeliverytilegrid.RowDefinitions.Add(rd);

                            row = row + 1;
                            col = 0;
                        }
                        else
                        {
                            RowDefinition rd = new RowDefinition();
                            GridLength gl = new GridLength(1, GridUnitType.Star);
                            rd.Height = gl;
                            AdvocatePostdeliverytilegrid.RowDefinitions.Add(rd);

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
                        AdvocatePostdeliverytilegrid.RowDefinitions.Add(rd);

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
                //        AdvocatePostdeliverytilegrid.RowDefinitions.Add(rd);

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
                //        AdvocatePostdeliverytilegrid.RowDefinitions.Add(rd);

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
                //        AdvocatePostdeliverytilegrid.RowDefinitions.Add(rd);

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
                AdvocatePostdeliverytilegrid.Children.Add(childGrid);

            }

        }
        private async void ChildGridpostdelivery_Tapped
(object sender, TappedRoutedEventArgs e)
        {


            foreach (Grid cg in AdvocatePostdeliverytilegrid.Children)
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

        public void AddColumnsToTilePreDeliveryGridLandscape()
        {
            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            AdvocatePostdeliverytilegrid.ColumnDefinitions.Add(cd1);

            //2st column
            ColumnDefinition cd2 = new ColumnDefinition();
            GridLength gl2 = new GridLength(1, GridUnitType.Star);
            cd2.Width = gl2;
            AdvocatePostdeliverytilegrid.ColumnDefinitions.Add(cd2);

        }
        public void AddColumnsToTilePreDeliveryGridPortrait()
        {

            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            AdvocatePostdeliverytilegrid.ColumnDefinitions.Add(cd1);
        }

    }
}
