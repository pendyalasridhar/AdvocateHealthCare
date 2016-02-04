using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
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
    public sealed partial class JournalPage : Page
    {
        public DisplayOrientations orientation = DisplayOrientations.Landscape;

        public JournalPage()
        {
            this.InitializeComponent();
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

            this.Loaded += JournalPage_Loaded;
            txtNotificationCount.Text = HomePage.unreadNotificationCount.ToString();
            DisplayProperties.OrientationChanged += Page_OrientationChanged;
          
      
    }
        public void Page_OrientationChanged(object sender)
        {
            //The orientation of the device is ...
            orientation = DisplayProperties.CurrentOrientation;
            if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
            {

                ConstructTileGrid(objJournelInfo);
            }

            if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
            {

                //VerticallyFlipped();
                ConstructTileGrid(objJournelInfo);
            }

        }
        //private void Journalpage_sizechanged(object sender, SizeChangedEventArgs e)
        //{
        //    //throw new NotImplementedException();
        //}
        public class Journalinfo
        {
            public string _id { get; set; }
            public string JournalTitle { get; set; }
            public string _JournalInfo { get; set; }
            public string CreatedDate { get; set; }
            //public string ImageServerPath { get; set; }
            public string ImageServerPath { get; set; }
            public string ProfileJournalID { get; set; }
            public string ProfileName { get; set; }


        }
        //public void VerticallyFlipped()
        //{

        //    //1st column
        //    ColumnDefinition cd1 = new ColumnDefinition();
        //    GridLength gl1 = new GridLength(1, GridUnitType.Star);
        //    cd1.Width = gl1;
        //    tileGrid.ColumnDefinitions.Add(cd1);
        //}

        Journalinfo objjournalinfo;
        List<Journalinfo> objJournelInfo = new List<Journalinfo>();
        async void JournalPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.IsInternet() == true)
            {
                try
                {
                  
                    HttpResponseMessage response = null;

                    //string ServiceCall = App.BASE_URL + "/api/Journals/GetJournals?UserId=2&JournalTypeId=1";
                    string ServiceCall = App.BASE_URL + "/api/Journals/GetJournals?UserId=" + App.userId + "&JournalTypeId=" + "1";//JournalTypeId 1 is for journals
                    var client = new HttpClient();
                    response = await client.GetAsync(new Uri(ServiceCall));


                    var jsonString = await response.Content.ReadAsStringAsync();
                    JArray jobject = JArray.Parse(jsonString);

                    foreach (var item in jobject)
                    {
                        if (item["JournalInfo"].ToString() == "" || item["JournalTitle"].ToString() == "")
                        { continue; }

                         objjournalinfo = new Journalinfo();
                        objjournalinfo.ProfileJournalID = (string)item["ProfileJournalID"];
                        objjournalinfo._id = (string)item["$id"];
                        objjournalinfo.JournalTitle = (string)item["JournalTitle"];
                        objjournalinfo._JournalInfo = (string)item["JournalInfo"];
                        objjournalinfo.CreatedDate = (string)item["CreatedDate"];
                        if ((string)item["ProfileName"] == null) // this is for the first item which we are adding in api. So, first tile is always the same with "name's" format.
                            objjournalinfo.ProfileName = Windows.Storage.ApplicationData.Current.LocalSettings.Values["userName"] + "'s Journal";
                        else
                            objjournalinfo.ProfileName = (string)item["ProfileName"];
                        var x = (string)item["JournalAsset"];

                        if (!string.IsNullOrEmpty(x))

                        {
                            objjournalinfo.ImageServerPath = new BitmapImage(new Uri(App.BASE_URL + x, UriKind.Absolute)).ToString();
                          //  imagepath = Convert.ToString(objjournalinfo.ImageServerPath.UriSource);
                        }
                        if (objjournalinfo._id != "1")
                        {
                            objJournelInfo.Add(objjournalinfo);
                        }
                    }
                    ConstructTileGrid(objJournelInfo);

                }
                catch (Exception ex)
                {
                    MessageDialog msgDialog = new MessageDialog("The required resources are not downloaded.Please check your internet connectivity. If the problem persists, please contact advocate healthcare customer care associate.", "Message");
                    msgDialog.ShowAsync();
                }
            }
            else
            {
                MessageDialog msgDialog = new MessageDialog("Please check your internet connection and try again", "Internet Connection is not available");
                msgDialog.ShowAsync();
            }

        }





        private void imgEdit_Tapped(object sender, TappedRoutedEventArgs e)
        {
           
            var JournalEDIT = (((Windows.UI.Xaml.Controls.Panel)((Windows.UI.Xaml.FrameworkElement)((Windows.UI.Xaml.FrameworkElement)e.OriginalSource).Parent).Parent).Children).ToList();
            this.Frame.Navigate(typeof(JournalEntry), JournalEDIT);
        }





        private void mySearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            this.Frame.Navigate(typeof(SearchPage), args.QueryText);
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Notifications));
        }
        string Title;
        string description;
        //shares image with social sharing
        private async void imgShare_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {

                var Journalshare = (((Windows.UI.Xaml.Controls.Panel)((Windows.UI.Xaml.FrameworkElement)((Windows.UI.Xaml.FrameworkElement)e.OriginalSource).Parent).Parent).Children).ToList();
                int i = 0;
                foreach (var data in (dynamic)Journalshare)
                {
                    if (i == 0)
                    {
                         Title = data.Text;
                    }
                    if (i == 3)
                    {
                        description = data.Text;
                    }
                    i++;
                }
                // string messageBody =  // "Hi Arun ..this mail is sent from Health care App";
                var emailMessage = new Windows.ApplicationModel.Email.EmailMessage();
                emailMessage.Body = Windows.Storage.ApplicationData.Current.LocalSettings.Values["userName"] + " wants to share this journal " + description;
                emailMessage.Subject = Title;
                var email = "Enter mail address";//recipient.Emails.FirstOrDefault<Windows.ApplicationModel.Contacts.ContactEmail>();
                if (email != null)
                {
                    var emailRecipient = new Windows.ApplicationModel.Email.EmailRecipient(email);
                    emailMessage.To.Add(emailRecipient);
                }
                await Windows.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(emailMessage);

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

        private void addJournal_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(JournalEntry));
        }
        public void ConstructTileGrid(List<Journalinfo> objJournelInfo)
        {
            JournaltileGrid.RowDefinitions.Clear();
            JournaltileGrid.ColumnDefinitions.Clear();
            JournaltileGrid.Children.Clear();

            if (orientation == DisplayOrientations.PortraitFlipped || orientation == DisplayOrientations.Portrait)
            {
                AddColumnsToTileGridPortrait();

            }
            else if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
            {
                AddColumnsToTileGridLandscape();

            }
            else
            {
                AddColumnsToTileGridLandscape();

            }

            int row = 0;
            int col = -1;
            //1st row

            if (objJournelInfo.Count < 1)
            {
                RowDefinition rd1 = new RowDefinition();
                GridLength gl1 = new GridLength(300);

                rd1.Height = gl1;

                JournaltileGrid.RowDefinitions.Add(rd1);

                Grid staticgrid = new Grid();
                staticgrid.Background = new SolidColorBrush(Colors.White);
                staticgrid.BorderThickness = new Thickness(1);
                staticgrid.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
                staticgrid.Tapped += ChildGrid2_Tapped;

                staticgrid.Margin = new Thickness(10, 10, 10, 10);
                //row1
                RowDefinition childGridRowstat1 = new RowDefinition();
                GridLength cglstat1 = new GridLength(0.5, GridUnitType.Star);
                childGridRowstat1.Height = cglstat1;
                staticgrid.RowDefinitions.Add(childGridRowstat1);
                //row2
                RowDefinition childGridRowstatic = new RowDefinition();
                GridLength cglstat2 = new GridLength(2, GridUnitType.Star);
                childGridRowstatic.Height = cglstat2;

                staticgrid.RowDefinitions.Add(childGridRowstatic);
                //row3
                RowDefinition childGridRowstat3 = new RowDefinition();
                GridLength cglstat3 = new GridLength(0.5, GridUnitType.Star);
                childGridRowstat3.Height = cglstat3;
                staticgrid.RowDefinitions.Add(childGridRowstat3);
                 //  childGrid.Tapped += ChildGrid_Tapped;

                //  childGrid.Tapped += ChildGrid_Tapped;
                TextBlock stattb1 = new TextBlock();
                stattb1.Name = "TileName";
                stattb1.HorizontalAlignment = HorizontalAlignment.Center;
                string name = Windows.Storage.ApplicationData.Current.LocalSettings.Values["userName"] + "'s Journal";
                stattb1.Text = name;
                stattb1.TextTrimming = TextTrimming.WordEllipsis;
                stattb1.FontSize = 24;
                stattb1.Foreground = new SolidColorBrush(Colors.Black);
                stattb1.FontWeight = FontWeights.Normal;
                stattb1.Margin = new Thickness(10, 0, 0, 0);

                Grid.SetRow(stattb1, 0);
                staticgrid.Children.Add(stattb1);

            

                Image img1 = new Image();


                img1.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/add_a_journal.png", UriKind.Absolute));
                img1.Height = 80;
                img1.Margin = new Thickness(0, 8, 0, 0);
                Grid.SetRow(img1, 1);
                staticgrid.Children.Add(img1);



                TextBlock stattb2 = new TextBlock();
                stattb2.Name = "TileName";

                stattb2.Text = "New Journal Entry";
                stattb2.HorizontalAlignment = HorizontalAlignment.Center;
                stattb2.TextTrimming = TextTrimming.WordEllipsis;
                stattb2.FontSize = 18;
                stattb2.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
                stattb2.FontWeight = FontWeights.Normal;
                stattb2.Margin = new Thickness(10, 0, 0, 0);
                stattb2.VerticalAlignment = VerticalAlignment.Top;
                

                Grid.SetRow(stattb2, 2);
                staticgrid.Children.Add(stattb2);

                Grid.SetRow(staticgrid, 0);
                Grid.SetColumn(staticgrid, 0);
                JournaltileGrid.Children.Add(staticgrid);
                col = col + 1;
            }
            else
            {
                RowDefinition rd1 = new RowDefinition();
                GridLength gl1 = new GridLength(300);

                rd1.Height = gl1;

                JournaltileGrid.RowDefinitions.Add(rd1);
                Grid staticgrid = new Grid();
                staticgrid.Background = new SolidColorBrush(Colors.White);
                staticgrid.BorderThickness = new Thickness(1);
                staticgrid.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
                staticgrid.Tapped += ChildGrid2_Tapped;
              
                staticgrid.Margin = new Thickness(10, 10, 10, 10);
                //row1
                RowDefinition childGridRowstat1 = new RowDefinition();
                GridLength cglstat1 = new GridLength(0.5, GridUnitType.Star);
                childGridRowstat1.Height = cglstat1;
                staticgrid.RowDefinitions.Add(childGridRowstat1);
                //row2
                RowDefinition childGridRowstatic = new RowDefinition();
                GridLength cglstat2 = new GridLength(2, GridUnitType.Star);
                childGridRowstatic.Height = cglstat2;

                staticgrid.RowDefinitions.Add(childGridRowstatic);
                //row3
                RowDefinition childGridRowstat3 = new RowDefinition();
                GridLength cglstat3 = new GridLength(0.5, GridUnitType.Star);
                childGridRowstat3.Height = cglstat3;
                staticgrid.RowDefinitions.Add(childGridRowstat3);
                //   childGrid.Tapped += ChildGrid_Tapped;

                //  childGrid.Tapped += ChildGrid_Tapped;
                TextBlock stattb1 = new TextBlock();
                stattb1.Name = "TileName";
                stattb1.HorizontalAlignment = HorizontalAlignment.Center;
                string name = Windows.Storage.ApplicationData.Current.LocalSettings.Values["userName"] + "'s Journal";
                stattb1.Text = name;
                stattb1.TextTrimming = TextTrimming.WordEllipsis;
                stattb1.FontSize = 24;
                stattb1.Foreground = new SolidColorBrush(Colors.Black);
                stattb1.FontWeight = FontWeights.Normal;
                stattb1.Margin = new Thickness(10, 10, 0, 0);

                Grid.SetRow(stattb1, 0);
                staticgrid.Children.Add(stattb1);

                Image img1 = new Image();
                img1.Margin = new Thickness(0, 8, 0, 0);


                img1.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/add_a_journal.png", UriKind.Absolute));
                img1.Height = 80;

                Grid.SetRow(img1, 1);
                staticgrid.Children.Add(img1);



                TextBlock stattb2 = new TextBlock();
                stattb2.Name = "TileName";

                stattb2.Text = "New Journal Entry";
                stattb2.HorizontalAlignment = HorizontalAlignment.Center;
                stattb2.TextTrimming = TextTrimming.WordEllipsis;
                stattb2.FontSize = 18;
                stattb2.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
                stattb2.FontWeight = FontWeights.Normal;
                stattb2.Margin = new Thickness(10, 0, 0, 0);
                stattb2.VerticalAlignment = VerticalAlignment.Top;


                Grid.SetRow(stattb2, 2);
                staticgrid.Children.Add(stattb2);

                Grid.SetRow(staticgrid, 0);
                Grid.SetColumn(staticgrid, 0);
                JournaltileGrid.Children.Add(staticgrid);
                col = col + 1;
            }
            for (int i = 0; i < objJournelInfo.Count; i++)
            {
                //ScrollViewer scrollViewer = new ScrollViewer();



                Grid childGrid = null;
               childGrid = new Grid();
                //event
                //   childGrid.HorizontalAlignment = HorizontalAlignment.Center;
                childGrid.Background = new SolidColorBrush(Colors.White);
                childGrid.BorderThickness = new Thickness(1);
                childGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));

                 childGrid.Tapped += ChildGrid2_Tapped;

                //  childGrid.Tapped += ChildGrid_Tapped;

                childGrid.Margin = new Thickness(10, 10, 10, 10);
                //row1
                RowDefinition childGridRow1 = new RowDefinition();
                GridLength cgl1 = new GridLength(0.3, GridUnitType.Star);
                childGridRow1.Height = cgl1;
                childGrid.RowDefinitions.Add(childGridRow1);
                //row2
                RowDefinition childGridRow2 = new RowDefinition();
                GridLength cgl2 = new GridLength(0.2, GridUnitType.Star);
                childGridRow2.Height = cgl2;

                childGrid.RowDefinitions.Add(childGridRow2);
                //row3
                RowDefinition childGridRow3 = new RowDefinition();
                GridLength cgl3 = new GridLength(2, GridUnitType.Star);
                childGridRow3.Height = cgl3;
                childGrid.RowDefinitions.Add(childGridRow3);

                //StackPanel deliveryInfoStackTile = new StackPanel();
                //deliveryInfoStackTile.Orientation = Orientation.Vertical;

                //<Image  Grid.Row = "0" Source = "{Binding DeliveryUrl}" Stretch = "Fill" ></ Image >




                TextBlock tb1 = new TextBlock();
                tb1.Name = "TileName";
                tb1.Text = objJournelInfo[i].JournalTitle;
                tb1.TextTrimming = TextTrimming.WordEllipsis;
                tb1.FontSize = 16;
                tb1.Foreground = new SolidColorBrush(Colors.Black);
                tb1.FontWeight = FontWeights.Normal;
                tb1.Margin = new Thickness(10, 10, 0, 0);

                Grid.SetRow(tb1, 0);
                childGrid.Children.Add(tb1);

                //StackPanel stackpanelobj = new StackPanel();
                //Image img1 = new Image();


                //img1.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/share.png", UriKind.Absolute));
                //img1.Height = 30;
                //img1.HorizontalAlignment = HorizontalAlignment.Right;

                //Grid.SetRow(img1, 0);
                //childGrid.Children.Add(img1);
                //Image img2 = new Image();


                //img2.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Edit.png", UriKind.Absolute));
                //img2.Height = 30;
                //img2.HorizontalAlignment = HorizontalAlignment.Right;

                //Grid.SetRow(img1, 0);
                StackPanel stackpanelobj = new StackPanel();
                stackpanelobj.Margin = new Thickness(0, 5, 5, 0);
                stackpanelobj.Orientation = Orientation.Horizontal;
                stackpanelobj.HorizontalAlignment = HorizontalAlignment.Right;
                Image img1 = new Image();


                img1.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/share.png", UriKind.Absolute));
                img1.Height = 30;
                img1.HorizontalAlignment = HorizontalAlignment.Right;
                stackpanelobj.Children.Add(img1);
                //Grid.SetRow(img1, 0);
                //childGrid.Children.Add(img1);
                Image img2 = new Image();
                img2.Tapped += imgEdit_Tapped;
                img1.Tapped += imgShare_Tapped;


                img2.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Edit.png", UriKind.Absolute));
                img2.Height = 30;
                img2.HorizontalAlignment = HorizontalAlignment.Right;
               
                stackpanelobj.Children.Add(img2);
                Grid.SetRow(stackpanelobj, 0);
                childGrid.Children.Add(stackpanelobj);
               




                //  < TextBlock Grid.Row = "2" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "17"  Foreground = "Black"  Text = "{Binding DeliveryInfo}"   Margin = "10,0,0,0" ></ TextBlock >

                TextBlock tb2 = new TextBlock();
                tb2.Text = Convert.ToString(objJournelInfo[i].CreatedDate);
                tb2.TextTrimming = TextTrimming.WordEllipsis;
                tb2.FontSize = 12;
                tb2.Foreground = new SolidColorBrush(Colors.Black);
                tb2.FontWeight = FontWeights.SemiLight;
                tb2.VerticalAlignment = VerticalAlignment.Top;
                tb2.Margin = new Thickness(10, 0, 0, 0);

                Grid.SetRow(tb2, 1);
                childGrid.Children.Add(tb2);


                TextBlock hidenTB1 = new TextBlock();
                hidenTB1.Name = "TileName";
                hidenTB1.Text = objJournelInfo[i].ProfileJournalID;
                hidenTB1.Visibility = Visibility.Collapsed;

                Grid.SetRow(hidenTB1, row);
                Grid.SetColumn(hidenTB1, col);
                childGrid.Children.Add(hidenTB1);

                Grid.SetRow(hidenTB1, 0);






                TextBlock tb3 = new TextBlock();
                tb3.Text = objJournelInfo[i]._JournalInfo;
                //   tb3.TextTrimming = TextTrimming.WordEllipsis;
                tb3.TextWrapping = TextWrapping.Wrap;
                tb3.FontSize = 14;
                tb3.Foreground = new SolidColorBrush(Colors.Black);
                tb3.FontWeight = FontWeights.Normal;
                tb3.Margin = new Thickness(10, 15, 0, 0);

                Grid.SetRow(tb3, 2);
                childGrid.Children.Add(tb3);



                if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
                {
                        if ((i + 2) > 2* (row + 1))
                        {
                            RowDefinition rd = new RowDefinition();
                            GridLength gl = new GridLength(300);
                            rd.Height = gl;
                            JournaltileGrid.RowDefinitions.Add(rd);

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
                    if ((i + 2) > 1 * (row + 1))
                    {
                        RowDefinition rd = new RowDefinition();
                        GridLength gl = new GridLength(300);
                        rd.Height = gl;
                        JournaltileGrid.RowDefinitions.Add(rd);

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
                    if ((i + 2) > 2 * (row + 1))
                    {
                        RowDefinition rd = new RowDefinition();
                        GridLength gl = new GridLength(300);
                        rd.Height = gl;
                        JournaltileGrid.RowDefinitions.Add(rd);

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
                JournaltileGrid.Children.Add(childGrid);




            }

        }
       
        private async void ChildGrid2_Tapped(object sender, TappedRoutedEventArgs e)
        {


            foreach (Grid cg in JournaltileGrid.Children)
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
                    if (t.Text == Windows.Storage.ApplicationData.Current.LocalSettings.Values["userName"] + "'s Journal")
                    {
                        await Task.Delay(1000);
                        this.Frame.Navigate(typeof(JournalEntry));
                        break;

                    }
                    if (t.Text != Windows.Storage.ApplicationData.Current.LocalSettings.Values["userName"] + "'s Journal")
                    {
                       // (((Windows.UI.Xaml.Controls.Panel)((Windows.UI.Xaml.FrameworkElement)((Windows.UI.Xaml.FrameworkElement)e.OriginalSource).Parent).Parent).Children).ToList();
                        var JournalDetailed = (((Windows.UI.Xaml.Controls.Panel)((Windows.UI.Xaml.FrameworkElement)e.OriginalSource).Parent).Children).ToList();
                        await Task.Delay(1000);
                        this.Frame.Navigate(typeof(JournalDetailed), JournalDetailed);
                        break;
                    }
                }
            }
        }
        //public void ConstructTileGrid(List<Journalinfo> objJournelInfo)
        //{ 
        //    tileGrid.RowDefinitions.Clear();
        //    tileGrid.ColumnDefinitions.Clear();
        //    tileGrid.Children.Clear();

            //    if (orientation == DisplayOrientations.PortraitFlipped || orientation == DisplayOrientations.Portrait)
            //    {
            //        AddColumnsToTileGridPortrait();

            //    }
            //    else if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
            //    {
            //        AddColumnsToTileGridLandscape();

            //    }
            //    else
            //    {
            //        AddColumnsToTileGridLandscape();

            //    }

            //    int row = 0;
            //    int col = -1;
            //    //1st row
            //    RowDefinition rd1 = new RowDefinition();
            //    GridLength gl1 = new GridLength(2, GridUnitType.Star);
            //    rd1.Height = gl1;
            //    tileGrid.RowDefinitions.Add(rd1);


            //    for (int i = 0; i < objJournelInfo.Count - 1; i++)
            //    {
            //        //ScrollViewer scrollViewer = new ScrollViewer();

            //        Grid childGrid = null;
            //        childGrid = new Grid();

            //        //event
            //        childGrid.Background = new SolidColorBrush(Colors.White);
            //        childGrid.BorderThickness = new Thickness(1);
            //        childGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));


            //      //  childGrid.Tapped += ChildGrid_Tapped;

            //        childGrid.Margin = new Thickness(10, 10, 10, 10);

            //        //row1
            //        RowDefinition childGridRow1 = new RowDefinition();
            //        GridLength cgl1 = new GridLength(1, GridUnitType.Star);
            //        childGridRow1.Height = cgl1;
            //        childGrid.RowDefinitions.Add(childGridRow1);
            //        //row2
            //        RowDefinition childGridRow2 = new RowDefinition();
            //        GridLength cgl2 = new GridLength(0.2, GridUnitType.Star);
            //        childGridRow2.Height = cgl2;

            //        childGrid.RowDefinitions.Add(childGridRow2);
            //        //row3
            //        RowDefinition childGridRow3 = new RowDefinition();
            //        GridLength cgl3 = new GridLength(0.2, GridUnitType.Star);
            //        childGridRow3.Height = cgl3;
            //        childGrid.RowDefinitions.Add(childGridRow3);

            //        //StackPanel deliveryInfoStackTile = new StackPanel();
            //        //deliveryInfoStackTile.Orientation = Orientation.Vertical;

            //        // < Image  Grid.Row = "0" Source = "{Binding DeliveryUrl}" Stretch = "Fill" ></ Image >
            //        Image img = new Image();
            //        BitmapImage bitmapImage = new BitmapImage();
            //        bitmapImage.UriSource = deliveryInfoList[i].DeliveryUrl;
            //        img.Source = bitmapImage;
            //        img.Stretch = Stretch.Fill;

            //        Grid.SetRow(img, 0);
            //        childGrid.Children.Add(img);

            //        //  < TextBlock Grid.Row = "1" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "20"   Foreground = "#DF6C3F"  Text = "{Binding DeliveryTitle}" Margin = "10,0,0,0" ></ TextBlock >

            //        TextBlock tb1 = new TextBlock();
            //        tb1.Name = "TileName";
            //        tb1.Text = deliveryInfoList[i].DeliveryTitle;
            //        tb1.TextTrimming = TextTrimming.WordEllipsis;
            //        tb1.FontSize = 20;
            //        tb1.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
            //        tb1.FontWeight = FontWeights.SemiLight;
            //        tb1.Margin = new Thickness(10, 0, 0, 0);

            //        Grid.SetRow(tb1, 1);
            //        childGrid.Children.Add(tb1);

            //        //  < TextBlock Grid.Row = "2" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "17"  Foreground = "Black"  Text = "{Binding DeliveryInfo}"   Margin = "10,0,0,0" ></ TextBlock >

            //        TextBlock tb2 = new TextBlock();
            //        tb2.Text = deliveryInfoList[i].DeliveryInfo;
            //        tb2.TextTrimming = TextTrimming.WordEllipsis;
            //        tb2.FontSize = 17;
            //        tb2.Foreground = new SolidColorBrush(Colors.Black);
            //        tb2.FontWeight = FontWeights.SemiLight;
            //        tb2.Margin = new Thickness(10, 0, 0, 0);

            //        Grid.SetRow(tb2, 2);
            //        childGrid.Children.Add(tb2);


            //        if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
            //        {
            //            if ((i + 1) > 2 * (row + 1))
            //            {
            //                RowDefinition rd = new RowDefinition();
            //                GridLength gl = new GridLength(1, GridUnitType.Star);
            //                rd.Height = gl;
            //                tileGrid.RowDefinitions.Add(rd);

            //                row = row + 1;
            //                col = 0;
            //            }
            //            else
            //            {
            //                col = col + 1;
            //            }
            //        }
            //        else if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
            //        {
            //            if ((i + 1) > 1 * (row + 1))
            //            {
            //                RowDefinition rd = new RowDefinition();
            //                GridLength gl = new GridLength(1, GridUnitType.Star);
            //                rd.Height = gl;
            //                tileGrid.RowDefinitions.Add(rd);

            //                row = row + 1;
            //                col = 0;
            //            }
            //            else
            //            {
            //                col = col + 1;
            //            }
            //        }
            //        else
            //        {
            //            if ((i + 1) > 2 * (row + 1))
            //            {
            //                RowDefinition rd = new RowDefinition();
            //                GridLength gl = new GridLength(1, GridUnitType.Star);
            //                rd.Height = gl;
            //                tileGrid.RowDefinitions.Add(rd);

            //                row = row + 1;
            //                col = 0;
            //            }
            //            else
            //            {
            //                col = col + 1;
            //            }
            //        }

            //        //Add to Grid
            //        Grid.SetRow(childGrid, row);
            //        Grid.SetColumn(childGrid, col);
            //        tileGrid.Children.Add(childGrid);

            //    }

            //}




        public void AddColumnsToTileGrid()
        {

            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            JournaltileGrid.ColumnDefinitions.Add(cd1);

            //2st column
            ColumnDefinition cd2 = new ColumnDefinition();
            GridLength gl2 = new GridLength(1, GridUnitType.Star);
            cd2.Width = gl2;
            JournaltileGrid.ColumnDefinitions.Add(cd2);

            //3st column
            //ColumnDefinition cd3 = new ColumnDefinition();
            //GridLength gl3 = new GridLength(1, GridUnitType.Star);
            //cd3.Width = gl3;
            //tileGrid.ColumnDefinitions.Add(cd3);

            //4st column
            //ColumnDefinition cd4 = new ColumnDefinition();
            //GridLength gl4 = new GridLength(1, GridUnitType.Star);
            //cd4.Width = gl4;
            //tileGrid.ColumnDefinitions.Add(cd4);
        }
        public void AddColumnsToTileGridLandscape()
        {

            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            JournaltileGrid.ColumnDefinitions.Add(cd1);

            //2st column
            ColumnDefinition cd2 = new ColumnDefinition();
            GridLength gl2 = new GridLength(1, GridUnitType.Star);
            cd2.Width = gl2;
            JournaltileGrid.ColumnDefinitions.Add(cd2);

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
        public void AddColumnsToTileGridPortrait()
        {

            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            JournaltileGrid.ColumnDefinitions.Add(cd1);

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

    }
}

