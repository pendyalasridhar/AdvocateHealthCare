using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Windows;
using Windows.UI.Core;
using System.Text.RegularExpressions;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Devices.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
namespace AdvocateHealthCare
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InputWeightPage : Page
    {

        string currentweek;
        public DisplayOrientations orientation = DisplayOrientations.Landscape;
        public int dateweek;
        public int mydate;
        public int weeks;
        DateTime lmpValue = Convert.ToDateTime(Windows.Storage.ApplicationData.Current.LocalSettings.Values["LMPDATE"]);
        public InputWeightPage()
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
            this.Loaded += InputWeightPag_Load;
            txtNotificationCount.Text = HomePage.unreadNotificationCount.ToString();//Displays the notification count
            DisplayProperties.OrientationChanged += Page_OrientationChanged;
            //InputScope scope = new InputScope();
            //InputScopeName name = new InputScopeName();
            //name.NameValue = InputScopeNameValue.TelephoneNumber;
            //scope.Names.Add(name);
            //Weight.InputScope = scope;
            int presentdate = (int)((DateTime.Now - lmpValue).TotalDays) / 6;
            currentweek = Convert.ToString(presentdate);

        }

        public class WeightHelper
        {
            public string Weight { get; set; }
            public string CreatedDate { get; set; }
            public DateTime LMPDATE { get; set; }
            public int calculatedweek { get; set; }
            public DateTime dt { get; set; }
        }



        public void Page_OrientationChanged(object sender)
        {
            //The orientation of the device is ...
            orientation = DisplayProperties.CurrentOrientation;
            if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
            {

                ConstructTileGrid(objWeightHelper);
            }

            if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
            {

                //VerticallyFlipped();
                ConstructTileGrid(objWeightHelper);
            }

        }

        public string missedDate;
        //gets all the weightentries from DB

        public void VerticallyFlipped()
        {

            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            inputtileGrid.ColumnDefinitions.Add(cd1);
        }
        //   List<WeightHelper> objWeightHelper = new List<WeightHelper>();
        List<WeightHelper> objWeightHelper;

        async void InputWeightPag_Load(object sender, RoutedEventArgs e)
        //InputWeightPag_Load
        {
            if (App.IsInternet() == true)
            {
                try
                {
                    objWeightHelper = new List<WeightHelper>();
                    //string ServiceCall = App.BASE_URL + "/api/WeightEntries/GetWeightEntries?UserId=2";
                    string ServiceCall = App.BASE_URL + "/api/WeightEntries/GetWeightEntries?UserId=" + App.userId;
                    var client = new HttpClient();
                    HttpResponseMessage response = await client.GetAsync(new Uri(ServiceCall));
                    var jsonString = await response.Content.ReadAsStringAsync();
                    JArray jobject = JArray.Parse(jsonString);
                    var object1 = jobject.FirstOrDefault();
                    if (object1 != null)
                    {
                        string createddates = object1.ToList()[3].ToString();
                        var createddates_ = createddates.Split(' ');
                        DateTime createddate2 = Convert.ToDateTime(createddates_[2]);
                        string lmpdate = object1.ToList()[2].ToString();
                        var lmpdate_ = lmpdate.Split(' ');

                        string fff = (lmpdate_[1]);
                        DateTime lmpdate1_ = Convert.ToDateTime(fff.Substring(1, 10));
                        int dateq = (int)((createddate2 - lmpdate1_).TotalDays) / 6;
                        if (dateq == 0)
                        {
                            mydate = dateq + 1;
                        }
                        else
                        {
                            mydate = dateq;
                        }
                    }
                    foreach (var item in jobject)
                    {
                        WeightHelper objweight = new WeightHelper();
                        objweight.CreatedDate = (string)item["CreatedDate"];
                        var stringArray = ((string)item["CreatedDate"]).Split(' ');
                        objweight.dt = Convert.ToDateTime(stringArray[1]);
                        objweight.Weight = (string)item["Weight"];
                        objweight.LMPDATE = (DateTime)item["LMPDATE"];
                        int dateX = (int)((objweight.dt - objweight.LMPDATE).TotalDays) / 6;
                        if (dateX == 0)
                        {
                            objweight.calculatedweek = dateX + 1;
                            //double week_ = ((DateTime.Now - objweight.LMPDATE).TotalDays) / 7;
                            int weekx = (int)((DateTime.Now - objweight.LMPDATE).TotalDays) / 6;
                            //int week = Coelse {
                            if (weekx == 0)
                            {
                                weeks = weekx + 1;
                            }
                            else
                            {
                                weeks = weekx + 1;
                            }
                            currentweek = Convert.ToString(weeks);
                            objWeightHelper.Add(objweight);
                            dateweek = objweight.calculatedweek;
                        }
                        else
                        {
                            objweight.calculatedweek = dateX + 1;
                            //double week_ = ((DateTime.Now - objweight.LMPDATE).TotalDays) / 7;
                            int weekx = (int)((DateTime.Now - objweight.LMPDATE).TotalDays) / 6;
                            //int week = Convert.ToInt32(Math.Round(week_));
                            if (weekx == 0)
                            {
                                weeks = weekx + 1;
                            }
                            else
                            {
                                weeks = weekx + 1;
                            }
                            currentweek = Convert.ToString(weeks);
                            objWeightHelper.Add(objweight);
                            dateweek = objweight.calculatedweek;
                        }
                    }
                    ConstructTileGrid(objWeightHelper);

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
        //submits the input weight to Api


        #region Helper class
        public class inputclass
        {
        }
        #endregion
        private void mySearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            this.Frame.Navigate(typeof(SearchPage), args.QueryText);
        }

        private void Notificationgridtapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Notifications));
        }

        DatePicker datep = new DatePicker();
        TextBox weighttxt = new TextBox();

        //TextBox weighttxt;
        // TextBox weighttxtport = new TextBox();
        MODEL.WeightEntries weightEntries = new MODEL.WeightEntries();
        public void ConstructTileGrid(List<WeightHelper> objWeightHelper)
        {
            inputtileGrid.RowDefinitions.Clear();
            inputtileGrid.ColumnDefinitions.Clear();
            inputtileGrid.Children.Clear();

            int row = 0;
            int col = -1;
            if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
            {

                AddColumnsToTileGridLandscape();
                if (objWeightHelper.Count < 1)
                {

                    RowDefinition rd8 = new RowDefinition();
                    GridLength gl51 = new GridLength(300);//new GridLength(2, GridUnitType.Star);
                    rd8.Height = gl51;
                    inputtileGrid.RowDefinitions.Add(rd8);
                    col = col + 1;

                    Grid grdstatic12 = new Grid();
                    grdstatic12.Margin = new Thickness(8, 8, 8, 8);
                    grdstatic12.Children.Clear();
                    grdstatic12.Background = new SolidColorBrush(Colors.White);

                    RowDefinition s1 = new RowDefinition();
                    GridLength s12 = new GridLength(0.2, GridUnitType.Star);//new GridLength(2, GridUnitType.Star);
                    s1.Height = s12;
                    grdstatic12.RowDefinitions.Add(s1);


                    RowDefinition s122 = new RowDefinition();
                    GridLength s123 = new GridLength(0.1, GridUnitType.Star);//new GridLength(2, GridUnitType.Star);
                    s122.Height = s123;
                    grdstatic12.RowDefinitions.Add(s122);


                    RowDefinition sdate = new RowDefinition();
                    GridLength s1234 = new GridLength(0.1, GridUnitType.Star);//new GridLength(2, GridUnitType.Star);
                    sdate.Height = s1234;
                    grdstatic12.RowDefinitions.Add(sdate);

                    RowDefinition stext = new RowDefinition();
                    GridLength s12345 = new GridLength(0.2, GridUnitType.Star);//new GridLength(2, GridUnitType.Star);
                    stext.Height = s12345;
                    grdstatic12.RowDefinitions.Add(stext);




                    RowDefinition sweight = new RowDefinition();
                    GridLength xw = new GridLength(0.1, GridUnitType.Star);//new GridLength(2, GridUnitType.Star);
                    sweight.Height = xw;
                    grdstatic12.RowDefinitions.Add(sweight);

                    RowDefinition stext1 = new RowDefinition();
                    GridLength s123456 = new GridLength(0.2, GridUnitType.Star);//new GridLength(2, GridUnitType.Star);
                    stext1.Height = s123456;
                    grdstatic12.RowDefinitions.Add(stext1);

                    RowDefinition sbtn = new RowDefinition();
                    GridLength s1234567 = new GridLength(0.3, GridUnitType.Star);//new GridLength(2, GridUnitType.Star);
                    sbtn.Height = s1234567;
                    grdstatic12.RowDefinitions.Add(sbtn);




                    TextBlock weighttraker = new TextBlock();
                    weighttraker.Name = "TileName";
                    weighttraker.HorizontalAlignment = HorizontalAlignment.Center;

                    weighttraker.Text = "Weight Tracker";
                    weighttraker.TextTrimming = TextTrimming.WordEllipsis;
                    weighttraker.FontSize = 24;
                    weighttraker.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
                    weighttraker.FontWeight = FontWeights.Normal;
                    weighttraker.Margin = new Thickness(10, 0, 0, 0);

                    Grid.SetRow(weighttraker, 0);
                    grdstatic12.Children.Add(weighttraker);







                    TextBlock week = new TextBlock();
                    week.Name = "TileName";
                    week.HorizontalAlignment = HorizontalAlignment.Center;

                    week.Text = "week" + currentweek;
                    week.TextTrimming = TextTrimming.WordEllipsis;
                    week.FontSize = 16;
                    week.Foreground = new SolidColorBrush(Colors.Black);
                    week.FontWeight = FontWeights.Normal;
                    week.Margin = new Thickness(10, 0, 0, 0);

                    Grid.SetRow(week, 1);
                    grdstatic12.Children.Add(week);



                    TextBlock date = new TextBlock();
                    date.Name = "TileName";
                    date.HorizontalAlignment = HorizontalAlignment.Left;

                    date.Text = "Date:";
                    date.TextTrimming = TextTrimming.WordEllipsis;
                    date.FontSize = 18;
                    date.Foreground = new SolidColorBrush(Colors.Black);
                    date.FontWeight = FontWeights.Normal;
                    date.Margin = new Thickness(90, 0, 0, 0);

                    Grid.SetRow(date, 2);
                    grdstatic12.Children.Add(date);



                     datep = new DatePicker();
                    //date.Name = "TileName";
                    //  weightEntries.CreatedDate = Convert.ToDateTime(datep.Date);
                    //   weightEntries.CreatedDate =Convert.ToDateTime(datep.Date);
                    datep.HorizontalAlignment = HorizontalAlignment.Center;

                    //date.Text = "Date:";
                    //date.TextTrimming = TextTrimming.WordEllipsis;
                    //date.FontSize = 18;
                    //date.Foreground = new SolidColorBrush(Colors.Black);
                    //date.FontWeight = FontWeights.Normal;
                    datep.Margin = new Thickness(0, 0, 0, 0);

                    Grid.SetRow(datep, 3);
                    grdstatic12.Children.Add(datep);



                    TextBlock weight = new TextBlock();
                    weight.Name = "TileName";
                    weight.HorizontalAlignment = HorizontalAlignment.Left;

                    weight.Text = "Weight:";
                    weight.TextTrimming = TextTrimming.WordEllipsis;
                    weight.FontSize = 18;
                    weight.Foreground = new SolidColorBrush(Colors.Black);
                    weight.FontWeight = FontWeights.Normal;
                    weight.Margin = new Thickness(90, 0, 0, 0);

                    Grid.SetRow(weight, 4);
                    grdstatic12.Children.Add(weight);


                    weighttxt = new TextBox();
                    InputScope inputScope = new InputScope();
                    InputScopeName inputScopeName = new InputScopeName();
                    inputScopeName.NameValue = InputScopeNameValue.Number;
                    inputScope.Names.Add(inputScopeName);
                    weighttxt.InputScope = inputScope;
                  
                    weighttxt.Width = 300;
                    weighttxt.Height = 20;

                    weighttxt.HorizontalAlignment = HorizontalAlignment.Center;
                    weighttxt.Margin = new Thickness(0, 0, 0, 0);
                    Grid.SetRow(weighttxt, 5);
                    grdstatic12.Children.Add(weighttxt);

                    Button btn = new Button();
                    btn.Name = "Save";
                    btn.Width = 300;
                    btn.Height = 30;
                    btn.Content = "Input Weight";
                    btn.FontSize = 12;
                    btn.Click += new RoutedEventHandler(Weightinputbutton);
                    btn.Foreground = new SolidColorBrush(Colors.White);
                    btn.Background = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
                    btn.HorizontalAlignment = HorizontalAlignment.Center;
                    btn.Margin = new Thickness(0, 0, 0, 0);
                    Grid.SetRow(btn, 6);
                    grdstatic12.Children.Add(btn);










                    //   grdstatic1.Tapped += ChildGrid_Tapped;


                    //Image imgStatic = new Image();
                    //imgStatic.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Humera.png", UriKind.Absolute));
                    //imgStatic.Margin = new Thickness(10, 10, 10, 10);
                    //imgStatic.Stretch = Stretch.Fill;
                    //grdstatic1.Children.Add(imgStatic);



                    Grid.SetRow(grdstatic12, row);
                    Grid.SetColumn(grdstatic12, col);
                    inputtileGrid.Children.Add(grdstatic12);


                    col = col + 1;

                    Grid grdstatic2 = new Grid();

                    //  grdstatic2.Tapped += ChildGrid_Tapped;

                    Image imgStatic1 = new Image();
                    imgStatic1.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/weightpregnancy2.png", UriKind.Absolute));

                    imgStatic1.Margin = new Thickness(10, 0, 0, 0);
                    imgStatic1.Stretch = Stretch.Fill;
                    grdstatic2.Children.Add(imgStatic1);
                    Grid.SetRow(grdstatic2, row);
                    Grid.SetColumn(grdstatic2, col);
                    inputtileGrid.Children.Add(grdstatic2);


                    row = row + 1;
                    col = 0;
                    RowDefinition rd69 = new RowDefinition();
                    GridLength gl515 = new GridLength(50);
                    rd69.Height = gl515;
                    inputtileGrid.RowDefinitions.Add(rd69);

                    Grid grdstatic3 = new Grid();
                    grdstatic3.Margin = new Thickness(0, 8, 8, 8);

                    TextBlock txtweight = new TextBlock();
                    txtweight.Text = "Weight at a glance";
                    txtweight.FontSize = 24;
                   
                    txtweight.Margin = new Thickness(10, 0, 0, 0);




                    grdstatic3.Children.Add(txtweight);
                    Grid.SetRow(grdstatic3, row);
                    Grid.SetColumn(grdstatic3, col);

                    inputtileGrid.Children.Add(grdstatic3);


                }
                else
                {



                    RowDefinition rd5 = new RowDefinition();
                    GridLength gl5 = new GridLength(300);//, GridUnitType.Star);//new GridLength(2, GridUnitType.Star);
                    rd5.Height = gl5;
                    inputtileGrid.RowDefinitions.Add(rd5);

                    col = col + 1;

                    Grid grdstatic1 = new Grid();
                    grdstatic1.Margin = new Thickness(8, 8, 8, 8);


                    grdstatic1.Background = new SolidColorBrush(Colors.White);

                    RowDefinition s1 = new RowDefinition();
                    GridLength s12 = new GridLength(0.2, GridUnitType.Star);//new GridLength(2, GridUnitType.Star);
                    s1.Height = s12;
                    grdstatic1.RowDefinitions.Add(s1);


                    RowDefinition s122 = new RowDefinition();
                    GridLength s123 = new GridLength(0.1, GridUnitType.Star);//new GridLength(2, GridUnitType.Star);
                    s122.Height = s123;
                    grdstatic1.RowDefinitions.Add(s122);


                    RowDefinition sdate = new RowDefinition();
                    GridLength s1234 = new GridLength(0.1, GridUnitType.Star);//new GridLength(2, GridUnitType.Star);
                    sdate.Height = s1234;
                    grdstatic1.RowDefinitions.Add(sdate);

                    RowDefinition stext = new RowDefinition();
                    GridLength s12345 = new GridLength(0.2, GridUnitType.Star);//new GridLength(2, GridUnitType.Star);
                    stext.Height = s12345;
                    grdstatic1.RowDefinitions.Add(stext);




                    RowDefinition sweight = new RowDefinition();
                    GridLength xw = new GridLength(0.1, GridUnitType.Star);//new GridLength(2, GridUnitType.Star);
                    sweight.Height = xw;
                    grdstatic1.RowDefinitions.Add(sweight);

                    RowDefinition stext1 = new RowDefinition();
                    GridLength s123456 = new GridLength(0.2, GridUnitType.Star);//new GridLength(2, GridUnitType.Star);
                    stext1.Height = s123456;
                    grdstatic1.RowDefinitions.Add(stext1);

                    RowDefinition sbtn = new RowDefinition();
                    GridLength s1234567 = new GridLength(0.3, GridUnitType.Star);//new GridLength(2, GridUnitType.Star);
                    sbtn.Height = s1234567;
                    grdstatic1.RowDefinitions.Add(sbtn);




                    TextBlock weighttraker = new TextBlock();
                    weighttraker.Name = "TileName";
                    weighttraker.HorizontalAlignment = HorizontalAlignment.Center;

                    weighttraker.Text = "Weight Tracker";
                    weighttraker.TextTrimming = TextTrimming.WordEllipsis;
                    weighttraker.FontSize = 24;
                    weighttraker.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
                    weighttraker.FontWeight = FontWeights.Normal;
                    weighttraker.Margin = new Thickness(10, 0, 0, 0);

                    Grid.SetRow(weighttraker, 0);
                    grdstatic1.Children.Add(weighttraker);







                    TextBlock week = new TextBlock();
                    week.Name = "TileName";
                    week.HorizontalAlignment = HorizontalAlignment.Center;

                    week.Text = "Week-" + currentweek;
                    week.TextTrimming = TextTrimming.WordEllipsis;
                    week.FontSize = 16;
                    week.Foreground = new SolidColorBrush(Colors.Black);
                    week.FontWeight = FontWeights.Normal;
                    week.Margin = new Thickness(10, 0, 0, 0);

                    Grid.SetRow(week, 1);
                    grdstatic1.Children.Add(week);



                    TextBlock date = new TextBlock();
                    date.Name = "TileName";
                    date.HorizontalAlignment = HorizontalAlignment.Left;

                    date.Text = "Date:";
                    date.TextTrimming = TextTrimming.WordEllipsis;
                    date.FontSize = 18;
                    date.Foreground = new SolidColorBrush(Colors.Black);
                    date.FontWeight = FontWeights.Normal;
                    date.Margin = new Thickness(90, 0, 0, 0);

                    Grid.SetRow(date, 2);
                    grdstatic1.Children.Add(date);

                    datep = new DatePicker();
                    //  DatePicker datep = new DatePicker();
                    //else date          
                    //date.Name = "TileName";
                    //  weightEntries.CreatedDate = Convert.ToDateTime(datep.Date);
                    //   weightEntries.CreatedDate =Convert.ToDateTime(datep.Date);
                    datep.HorizontalAlignment = HorizontalAlignment.Center;

                    //date.Text = "Date:";
                    //date.TextTrimming = TextTrimming.WordEllipsis;
                    //date.FontSize = 18;
                    //date.Foreground = new SolidColorBrush(Colors.Black);
                    //date.FontWeight = FontWeights.Normal;
                    datep.Margin = new Thickness(0, 0, 0, 0);

                    Grid.SetRow(datep, 3);
                    grdstatic1.Children.Add(datep);// here is exception 



                    TextBlock weight = new TextBlock();
                    weight.Name = "TileName";
                    weight.HorizontalAlignment = HorizontalAlignment.Left;

                    weight.Text = "Weight:";
                    weight.TextTrimming = TextTrimming.WordEllipsis;
                    weight.FontSize = 18;
                    weight.Foreground = new SolidColorBrush(Colors.Black);
                    weight.FontWeight = FontWeights.Normal;
                    weight.Margin = new Thickness(90, 0, 0, 0);

                    Grid.SetRow(weight, 4);
                    grdstatic1.Children.Add(weight);


                    weighttxt = new TextBox();

                    InputScope inputScope = new InputScope();
                    InputScopeName inputScopeName = new InputScopeName();
                    inputScopeName.NameValue = InputScopeNameValue.Number;
                    inputScope.Names.Add(inputScopeName);
                    weighttxt.InputScope = inputScope;

                    weighttxt.Width = 300;
                    weighttxt.Height = 20;
                    weighttxt.HorizontalAlignment = HorizontalAlignment.Center;
                    weighttxt.Margin = new Thickness(0, 0, 0, 0);
                    Grid.SetRow(weighttxt, 5);
                    grdstatic1.Children.Add(weighttxt);

                    Button btn = new Button();
                    btn.Name = "Save";
                    btn.Width = 300;
                    btn.Height = 30;
                    btn.Content = "Input Weight";
                    btn.FontSize = 12;
                    btn.Click += new RoutedEventHandler(Weightinputbutton);
                    btn.Foreground = new SolidColorBrush(Colors.White);
                    btn.Background = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
                    btn.HorizontalAlignment = HorizontalAlignment.Center;
                    btn.Margin = new Thickness(0, 0, 0, 0);
                    Grid.SetRow(btn, 6);
                    grdstatic1.Children.Add(btn);








                    //   grdstatic1.Tapped += ChildGrid_Tapped;


                    //Image imgStatic = new Image();
                    //imgStatic.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Humera.png", UriKind.Absolute));
                    //imgStatic.Margin = new Thickness(10, 10, 10, 10);
                    //imgStatic.Stretch = Stretch.Fill;
                    //grdstatic1.Children.Add(imgStatic);



                    Grid.SetRow(grdstatic1, row);
                    Grid.SetColumn(grdstatic1, col);
                    inputtileGrid.Children.Add(grdstatic1);



                    col = col + 1;

                    Grid grdstatic2 = new Grid();
                    grdstatic2.Margin = new Thickness(0, 8, 8, 8);

                    //  grdstatic2.Tapped += ChildGrid_Tapped;

                    Image imgStatic1 = new Image();
                    imgStatic1.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/weightpregnancy2.png", UriKind.Absolute));
                    imgStatic1.Margin = new Thickness(10, 0, 0, 0);
                    imgStatic1.Stretch = Stretch.Fill;
                    grdstatic2.Children.Add(imgStatic1);
                    Grid.SetRow(grdstatic2, row);
                    Grid.SetColumn(grdstatic2, col);
                    inputtileGrid.Children.Add(grdstatic2);






                    row = row + 1;
                    col = 0;
                    RowDefinition rd69 = new RowDefinition();
                    GridLength gl515 = new GridLength(50);
                    rd69.Height = gl515;
                    inputtileGrid.RowDefinitions.Add(rd69);

                    Grid grdstatic3 = new Grid();

                    TextBlock txtweight = new TextBlock();
                    txtweight.Text = "Weight at a glance";
                    txtweight.VerticalAlignment = VerticalAlignment.Center;
                    txtweight.FontSize = 24;

                    txtweight.Margin = new Thickness(10, 0, 0, 0);




                    grdstatic3.Children.Add(txtweight);
                    Grid.SetRow(grdstatic3, row);
                    Grid.SetColumn(grdstatic3, col);

                    inputtileGrid.Children.Add(grdstatic3);

                }
            }




            else if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
            {


              

                AddColumnsToTileGridPortrait();

                RowDefinition rd1 = new RowDefinition();
                GridLength gl1 = new GridLength(300);//new GridLength(2, GridUnitType.Star);
                rd1.Height = gl1;
                inputtileGrid.RowDefinitions.Add(rd1);

                RowDefinition rd2 = new RowDefinition();
                GridLength gl2 = new GridLength(300);//new GridLength(2, GridUnitType.Star);
                rd2.Height = gl2;
                inputtileGrid.RowDefinitions.Add(rd2);



                RowDefinition rd69 = new RowDefinition();
                GridLength gl515 = new GridLength(50);
                rd69.Height = gl515;
                inputtileGrid.RowDefinitions.Add(rd69);

                col = col + 1;







                Grid grdstatic25 = new Grid();


                grdstatic25.Background = new SolidColorBrush(Colors.White);

                RowDefinition s1 = new RowDefinition();
                GridLength s12 = new GridLength(0.2, GridUnitType.Star);//new GridLength(2, GridUnitType.Star);
                s1.Height = s12;
                grdstatic25.RowDefinitions.Add(s1);


                RowDefinition s122 = new RowDefinition();
                GridLength s123 = new GridLength(0.1, GridUnitType.Star);//new GridLength(2, GridUnitType.Star);
                s122.Height = s123;
                grdstatic25.RowDefinitions.Add(s122);


                RowDefinition sdate = new RowDefinition();
                GridLength s1234 = new GridLength(0.1, GridUnitType.Star);//new GridLength(2, GridUnitType.Star);
                sdate.Height = s1234;
                grdstatic25.RowDefinitions.Add(sdate);

                RowDefinition stext = new RowDefinition();
                GridLength s12345 = new GridLength(0.2, GridUnitType.Star);//new GridLength(2, GridUnitType.Star);
                stext.Height = s12345;
                grdstatic25.RowDefinitions.Add(stext);




                RowDefinition sweight = new RowDefinition();
                GridLength xw = new GridLength(0.1, GridUnitType.Star);//new GridLength(2, GridUnitType.Star);
                sweight.Height = xw;
                grdstatic25.RowDefinitions.Add(sweight);

                RowDefinition stext1 = new RowDefinition();
                GridLength s123456 = new GridLength(0.2, GridUnitType.Star);//new GridLength(2, GridUnitType.Star);
                stext1.Height = s123456;
                grdstatic25.RowDefinitions.Add(stext1);

                RowDefinition sbtn = new RowDefinition();
                GridLength s1234567 = new GridLength(0.2, GridUnitType.Star);//new GridLength(2, GridUnitType.Star);
                sbtn.Height = s1234567;
                grdstatic25.RowDefinitions.Add(sbtn);




                TextBlock weighttraker = new TextBlock();
                weighttraker.Name = "TileName";
                weighttraker.HorizontalAlignment = HorizontalAlignment.Center;

                weighttraker.Text = "Weight Tracker";
                weighttraker.TextTrimming = TextTrimming.WordEllipsis;
                weighttraker.FontSize = 24;
                weighttraker.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
                weighttraker.FontWeight = FontWeights.Normal;
                weighttraker.Margin = new Thickness(10, 0, 0, 0);

                Grid.SetRow(weighttraker, 0);
                grdstatic25.Children.Add(weighttraker);







                TextBlock week = new TextBlock();
                week.Name = "TileName";
                week.HorizontalAlignment = HorizontalAlignment.Center;

                week.Text = "Week- " + currentweek;
                week.TextTrimming = TextTrimming.WordEllipsis;
                week.FontSize = 16;
                week.Foreground = new SolidColorBrush(Colors.Black);
                week.FontWeight = FontWeights.Normal;
                week.Margin = new Thickness(10, 0, 0, 0);

                Grid.SetRow(week, 1);
                grdstatic25.Children.Add(week);



                TextBlock date = new TextBlock();
                date.Name = "date";
                //    date.HorizontalAlignment = HorizontalAlignment.Center;

                date.Text = "Date:";
                date.TextTrimming = TextTrimming.WordEllipsis;
                date.FontSize = 18;
                date.Foreground = new SolidColorBrush(Colors.Black);
                date.FontWeight = FontWeights.Normal;
                date.Margin = new Thickness(170, 0, 0, 0);

                Grid.SetRow(date, 2);
                grdstatic25.Children.Add(date);


                 datep = new DatePicker();


                //date.Name = "TileName";
                datep.HorizontalAlignment = HorizontalAlignment.Center;

                //date.Text = "Date:";
                //date.TextTrimming = TextTrimming.WordEllipsis;
                //date.FontSize = 18;
                //date.Foreground = new SolidColorBrush(Colors.Black);
                //date.FontWeight = FontWeights.Normal;
                datep.Margin = new Thickness(0, 0, 0, 0);

                Grid.SetRow(datep, 3);
                grdstatic25.Children.Add(datep);



                TextBlock weight = new TextBlock();
                weight.Name = "weight";
                weight.HorizontalAlignment = HorizontalAlignment.Left;

                weight.Text = "Weight:";
                weight.TextTrimming = TextTrimming.WordEllipsis;
                weight.FontSize = 18;
                weight.Foreground = new SolidColorBrush(Colors.Black);
                weight.FontWeight = FontWeights.Normal;
                weight.Margin = new Thickness(170, 0, 0, 0);

                Grid.SetRow(weight, 4);
                grdstatic25.Children.Add(weight);


                 weighttxt = new TextBox();



                weighttxt.Width = 300;
                weighttxt.Height = 20;
                InputScope inputScope = new InputScope();
                InputScopeName inputScopeName = new InputScopeName();
                inputScopeName.NameValue = InputScopeNameValue.Number;
                inputScope.Names.Add(inputScopeName);
                weighttxt.InputScope = inputScope;

                weighttxt.HorizontalAlignment = HorizontalAlignment.Center;
                weighttxt.Margin = new Thickness(0, 0, 0, 0);
                Grid.SetRow(weighttxt, 5);
                grdstatic25.Children.Add(weighttxt);

                Button btn = new Button();
                btn.Name = "Save";
                btn.Width = 300;
                btn.Height = 30;
                btn.Content = "Input Weight";
                btn.FontSize = 12;
                btn.Foreground = new SolidColorBrush(Colors.White);
                btn.Background = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
                btn.HorizontalAlignment = HorizontalAlignment.Center;
                btn.Margin = new Thickness(0, 0, 0, 0);
                //btn.Click += new RoutedEventHandler(this.btn_click);
                btn.Click += new RoutedEventHandler(Weightinputbutton);
                Grid.SetRow(btn, 6);
                grdstatic25.Children.Add(btn);

                Grid.SetRow(grdstatic25, row);
                Grid.SetColumn(grdstatic25, col);
                inputtileGrid.Children.Add(grdstatic25);


                row = row + 1;

                Grid grdstatic3 = new Grid();
                // grdstatic3.Background = new SolidColorBrush(Colors.Red);
                Image imgStatic1 = new Image();
                imgStatic1.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/weightpregnancy2.png", UriKind.Absolute));
                imgStatic1.Margin = new Thickness(0, 10, 0, 0);
                imgStatic1.Stretch = Stretch.Fill;
                grdstatic3.Children.Add(imgStatic1);

                Grid.SetRow(grdstatic3, row);
                Grid.SetColumn(grdstatic3, col);
                inputtileGrid.Children.Add(grdstatic3);



                row = row + 1;

                Grid grdstatic6 = new Grid();

                TextBlock txtweight = new TextBlock();
                txtweight.Text = "Weight at a glance";
                txtweight.VerticalAlignment = VerticalAlignment.Center;
                txtweight.FontSize = 24;

                txtweight.Margin = new Thickness(10, 0, 0, 0);




                grdstatic6.Children.Add(txtweight);
                Grid.SetRow(grdstatic6, row);
                Grid.SetColumn(grdstatic6, col);

                inputtileGrid.Children.Add(grdstatic6);



            }





            for (int i = 0; i < objWeightHelper.Count; i++)
            {


                if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
                {



                    Grid childGrid = new Grid();
                    //event

                    childGrid.Background = new SolidColorBrush(Colors.White);
                    childGrid.BorderThickness = new Thickness(1);
                    childGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));

                    childGrid.Tapped += ChildGrid4_Tapped;

                    //  childGrid.Tapped += ChildGrid_Tapped;

                    childGrid.Margin = new Thickness(8, 8, 8, 8);

                    //row1
                    RowDefinition childGridRow1 = new RowDefinition();
                    GridLength cgl1 = new GridLength(0.5, GridUnitType.Star);
                    childGridRow1.Height = cgl1;
                    childGrid.RowDefinitions.Add(childGridRow1);
                    //row2
                    RowDefinition childGridRow2 = new RowDefinition();
                    GridLength cgl2 = new GridLength(1, GridUnitType.Star);
                    childGridRow2.Height = cgl2;

                    childGrid.RowDefinitions.Add(childGridRow2);
                    //row3
                    RowDefinition childGridRow3 = new RowDefinition();
                    GridLength cgl3 = new GridLength(0.3, GridUnitType.Star);
                    childGridRow3.Height = cgl3;
                    childGrid.RowDefinitions.Add(childGridRow3);
                    ////row4
                    RowDefinition childGridRow4 = new RowDefinition();
                    GridLength cgl4 = new GridLength(0.3, GridUnitType.Star);
                    childGridRow3.Height = cgl4;
                    childGrid.RowDefinitions.Add(childGridRow4);


                    //StackPanel deliveryInfoStackTile = new StackPanel();
                    //deliveryInfoStackTile.Orientation = Orientation.Vertical;

                    //<Image  Grid.Row = "0" Source = "{Binding DeliveryUrl}" Stretch = "Fill" ></ Image >




                    TextBlock tb1 = new TextBlock();
                    tb1.HorizontalAlignment = HorizontalAlignment.Center;
                    tb1.Name = "TileName";
                    tb1.Text = "Week-" + Convert.ToString(objWeightHelper[i].calculatedweek);
                    tb1.TextTrimming = TextTrimming.WordEllipsis;
                    tb1.FontSize = 16;
                    tb1.Foreground = new SolidColorBrush(Colors.Black);
                    tb1.FontWeight = FontWeights.SemiLight;
                    tb1.Margin = new Thickness(0, 25, 0, 0);

                    Grid.SetRow(tb1, 0);
                    childGrid.Children.Add(tb1);


                    //  < TextBlock Grid.Row = "2" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "17"  Foreground = "Black"  Text = "{Binding DeliveryInfo}"   Margin = "10,0,0,0" ></ TextBlock >

                    TextBlock tb2 = new TextBlock();
                    tb2.HorizontalAlignment = HorizontalAlignment.Center;
                    tb2.Text = Convert.ToString(objWeightHelper[i].Weight);
                    tb2.TextTrimming = TextTrimming.WordEllipsis;
                    tb2.FontSize = 48;
                    tb2.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
                    tb2.FontWeight = FontWeights.SemiLight;
                    tb2.Margin = new Thickness(0, 30, 0, 0);

                    Grid.SetRow(tb2, 1);
                    childGrid.Children.Add(tb2);

                    TextBlock tb3 = new TextBlock();
                    tb3.HorizontalAlignment = HorizontalAlignment.Center;
                    tb3.Text = "pounds";
                    tb3.TextTrimming = TextTrimming.WordEllipsis;
                    tb3.FontSize = 24;
                    tb3.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
                    tb3.FontWeight = FontWeights.SemiLight;
                    tb3.Margin = new Thickness(0, 0, 0, 0);

                    Grid.SetRow(tb3, 2);
                    childGrid.Children.Add(tb3);


                    TextBlock tb4 = new TextBlock();
                    tb4.HorizontalAlignment = HorizontalAlignment.Center;
                    tb4.Text = "";
                    tb4.TextTrimming = TextTrimming.WordEllipsis;
                    tb4.FontSize = 24;
                    tb4.Foreground = new SolidColorBrush(Colors.Black);
                    tb4.FontWeight = FontWeights.SemiLight;
                    tb4.Margin = new Thickness(0, 10, 0, 10);

                    Grid.SetRow(tb4, 3);
                    childGrid.Children.Add(tb4);



                    if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
                    {
                        if ((i + 5) > 2 * (row + 1))
                        {
                            RowDefinition rd = new RowDefinition();
                            GridLength gl = new GridLength(300);//(, GridUnitType.Star);
                            rd.Height = gl;
                            inputtileGrid.RowDefinitions.Add(rd);

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
                        if ((i + 4) > 1 * (row + 1))
                        {
                            RowDefinition rd = new RowDefinition();
                            GridLength gl = new GridLength(300);
                            rd.Height = gl;
                            inputtileGrid.RowDefinitions.Add(rd);

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
                            GridLength gl = new GridLength(300);//(1, GridUnitType.Star);
                            rd.Height = gl;
                            inputtileGrid.RowDefinitions.Add(rd);

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
                    inputtileGrid.Children.Add(childGrid);
                    //  tileGrid.Children.Add(staticgrid);


                }
                else if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
                {

                    Grid childGrid = new Grid();
                    //event

                    childGrid.Background = new SolidColorBrush(Colors.White);
                    childGrid.BorderThickness = new Thickness(1);
                    childGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));

                    childGrid.Tapped += ChildGrid4_Tapped;

                    //  childGrid.Tapped += ChildGrid_Tapped;

                    childGrid.Margin = new Thickness(0, 10, 0, 0);

                    //row1
                    RowDefinition childGridRow1 = new RowDefinition();
                    GridLength cgl1 = new GridLength(0.5, GridUnitType.Star);
                    childGridRow1.Height = cgl1;
                    childGrid.RowDefinitions.Add(childGridRow1);
                    //row2
                    RowDefinition childGridRow2 = new RowDefinition();
                    GridLength cgl2 = new GridLength(1, GridUnitType.Star);
                    childGridRow2.Height = cgl2;

                    childGrid.RowDefinitions.Add(childGridRow2);
                    //row3
                    RowDefinition childGridRow3 = new RowDefinition();
                    GridLength cgl3 = new GridLength(0.3, GridUnitType.Star);
                    childGridRow3.Height = cgl3;
                    childGrid.RowDefinitions.Add(childGridRow3);
                    ////row4
                    RowDefinition childGridRow4 = new RowDefinition();
                    GridLength cgl4 = new GridLength(0.3, GridUnitType.Star);
                    childGridRow3.Height = cgl4;
                    childGrid.RowDefinitions.Add(childGridRow4);


                    //StackPanel deliveryInfoStackTile = new StackPanel();
                    //deliveryInfoStackTile.Orientation = Orientation.Vertical;

                    //<Image  Grid.Row = "0" Source = "{Binding DeliveryUrl}" Stretch = "Fill" ></ Image >




                    TextBlock tb1 = new TextBlock();
                    tb1.HorizontalAlignment = HorizontalAlignment.Center;
                    tb1.Name = "TileName";
                    tb1.Text = "Week-" + Convert.ToString(objWeightHelper[i].calculatedweek);
                    tb1.TextTrimming = TextTrimming.WordEllipsis;
                    tb1.FontSize = 19;
                    tb1.Foreground = new SolidColorBrush(Colors.Black);
                    tb1.FontWeight = FontWeights.SemiLight;
                    tb1.Margin = new Thickness(0, 25, 0, 0);

                    Grid.SetRow(tb1, 0);
                    childGrid.Children.Add(tb1);


                    //  < TextBlock Grid.Row = "2" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "17"  Foreground = "Black"  Text = "{Binding DeliveryInfo}"   Margin = "10,0,0,0" ></ TextBlock >

                    TextBlock tb2 = new TextBlock();
                    tb2.HorizontalAlignment = HorizontalAlignment.Center;
                    tb2.Text = Convert.ToString(objWeightHelper[i].Weight);
                    tb2.TextTrimming = TextTrimming.WordEllipsis;
                    tb2.FontSize = 48;
                    tb2.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
                    tb2.FontWeight = FontWeights.SemiLight;
                    tb2.Margin = new Thickness(0, 30, 0, 0);

                    Grid.SetRow(tb2, 1);
                    childGrid.Children.Add(tb2);

                    TextBlock tb3 = new TextBlock();
                    tb3.HorizontalAlignment = HorizontalAlignment.Center;
                    tb3.Text = "pounds";
                    tb3.TextTrimming = TextTrimming.WordEllipsis;
                    tb3.FontSize = 24;
                    tb3.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
                    tb3.FontWeight = FontWeights.SemiLight;
                    tb3.Margin = new Thickness(0, 0, 0, 0);

                    Grid.SetRow(tb3, 2);
                    childGrid.Children.Add(tb3);


                    TextBlock tb4 = new TextBlock();
                    tb4.HorizontalAlignment = HorizontalAlignment.Center;
                    tb4.Text = "";
                    tb4.TextTrimming = TextTrimming.WordEllipsis;
                    tb4.FontSize = 24;
                    tb4.Foreground = new SolidColorBrush(Colors.Black);
                    tb4.FontWeight = FontWeights.SemiLight;
                    tb4.Margin = new Thickness(0, 10, 0, 10);

                    Grid.SetRow(tb4, 3);
                    childGrid.Children.Add(tb4);



                    if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
                    {
                        if ((i + 5) > 2 * (row + 1))
                        {
                            RowDefinition rd = new RowDefinition();
                            GridLength gl = new GridLength(300);//(, GridUnitType.Star);
                            rd.Height = gl;
                            inputtileGrid.RowDefinitions.Add(rd);

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
                        if ((i + 4) > 1 * (row + 1))
                        {
                            RowDefinition rd = new RowDefinition();
                            GridLength gl = new GridLength(300);
                            rd.Height = gl;
                            inputtileGrid.RowDefinitions.Add(rd);

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
                            GridLength gl = new GridLength(300);//(1, GridUnitType.Star);
                            rd.Height = gl;
                            inputtileGrid.RowDefinitions.Add(rd);

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
                    inputtileGrid.Children.Add(childGrid);
                    //  tileGrid.Children.Add(staticgrid);

                }
            }
           
        }






        private void ChildGrid4_Tapped(object sender, TappedRoutedEventArgs e)
        {


            foreach (Grid cg in inputtileGrid.Children)
            {
                cg.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
            }

      ((Windows.UI.Xaml.Controls.Grid)sender).BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
        }

        public void AddColumnsToTileGridPortrait()
        {

            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            inputtileGrid.ColumnDefinitions.Add(cd1);

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
        public void AddColumnsToTileGridLandscape()
        {

            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            inputtileGrid.ColumnDefinitions.Add(cd1);

            //2st column
            ColumnDefinition cd2 = new ColumnDefinition();
            GridLength gl2 = new GridLength(1, GridUnitType.Star);
            cd2.Width = gl2;
            inputtileGrid.ColumnDefinitions.Add(cd2);

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
        public void AddColumnsToTileGrid()
        {

            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            inputtileGrid.ColumnDefinitions.Add(cd1);

            //2st column
            ColumnDefinition cd2 = new ColumnDefinition();
            GridLength gl2 = new GridLength(1, GridUnitType.Star);
            cd2.Width = gl2;
            inputtileGrid.ColumnDefinitions.Add(cd2);

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
        //DateTime dtm;
        //DateTime createddate222;
        //async void Weightinputbutton1(object sender, RoutedEventArgs e)
        //{
        //    bool flag = false;
        //    if (App.IsInternet() == true)
        //    {
        //        Regex r = new Regex("^[a-zA-Z]*$");
        //        Regex rs = new Regex(@"[^0-9]+");
        //        string txtToValidation;

        //        txtToValidation = weighttxtland.Text;





        //        if (r.IsMatch(txtToValidation) || rs.IsMatch(txtToValidation))
        //        {
        //            MessageDialog msgDialog = new MessageDialog("Please enter valid weight to proceed. Example: 123", "Message");
        //            msgDialog.ShowAsync();
        //        }
        //        else {
        //            if (weighttxtland.Text.Trim() != "" && weighttxtland.Text != "0")
        //            {
        //                try
        //                {
        //                    MODEL.WeightEntries weightEntries = new MODEL.WeightEntries();
        //                    //DateTime xxxx =Convert.ToDateTime( _WeightInputDate.Date);
        //                    //xxxx.ToUniversalTime(DateTime.Now);
        //                    //   weightEntries.CreatedDate = 


        //                    string datess = Convert.ToString(datep1.Date);
        //                    var createddates_ = datess.Split(' ');
        //                    createddate222 = Convert.ToDateTime(createddates_[0]);

        //                    weightEntries.CreatedDate = createddate222;
        //                    dtm = createddate222;




        //                    if (dtm < lmpValue)
        //                    {
        //                        MessageDialog msgDialog = new MessageDialog("Date cannot be less than LMP date.", "Message");
        //                        msgDialog.ShowAsync();
        //                    }
        //                    else {
        //                        if (dtm > DateTime.Now)
        //                        {
        //                            flag = true;
        //                            MessageDialog msgDialog = new MessageDialog("You cannot input weight for future.", "Message");
        //                            msgDialog.ShowAsync();
        //                        }
        //                        else if (currentweek != "")
        //                        {
        //                            objWeightHelper = new List<WeightHelper>();
        //                            string ServiceCall = App.BASE_URL + "/api/WeightEntries/GetWeightEntries?UserId=" + App.userId;
        //                            var client1 = new HttpClient();
        //                            HttpResponseMessage response = await client1.GetAsync(new Uri(ServiceCall));
        //                            var jsonString = await response.Content.ReadAsStringAsync();
        //                            JArray jobject = JArray.Parse(jsonString);
        //                            foreach (var item in jobject)
        //                            {
        //                                WeightHelper objweight = new WeightHelper();
        //                                objweight.CreatedDate = (string)item["CreatedDate"];
        //                                var stringArray = ((string)item["CreatedDate"]).Split(' ');
        //                                objweight.dt = Convert.ToDateTime(stringArray[1]);
        //                                objweight.Weight = (string)item["Weight"];
        //                                objweight.LMPDATE = (DateTime)item["LMPDATE"];



        //                                int dateX = (int)((objweight.dt - objweight.LMPDATE).TotalDays) / 6;
        //                                if (dateX == 0)
        //                                {
        //                                    objweight.calculatedweek = dateX + 1;
        //                                    //double week_ = ((DateTime.Now - objweight.LMPDATE).TotalDays) / 7;
        //                                    int weekx = (int)((DateTime.Now - objweight.LMPDATE).TotalDays) / 6;
        //                                    //int week = Coelse {
        //                                    if (weekx == 0)
        //                                    {
        //                                        weeks = weekx + 1;
        //                                    }
        //                                    else
        //                                    {
        //                                        weeks = weekx;
        //                                    }
        //                                    currentweek = Convert.ToString(weeks);
        //                                    objWeightHelper.Add(objweight);
        //                                    dateweek = objweight.calculatedweek;
        //                                }
        //                                else
        //                                {
        //                                    objweight.calculatedweek = dateX + 1;
        //                                    //double week_ = ((DateTime.Now - objweight.LMPDATE).TotalDays) / 7;
        //                                    int weekx = (int)((DateTime.Now - objweight.LMPDATE).TotalDays) / 6;
        //                                    //int week = Convert.ToInt32(Math.Round(week_));
        //                                    if (weekx == 0)
        //                                    {
        //                                        weeks = weekx + 1;
        //                                    }
        //                                    else
        //                                    {
        //                                        weeks = weekx;
        //                                    }
        //                                    currentweek = Convert.ToString(weeks);
        //                                    objWeightHelper.Add(objweight);
        //                                    dateweek = objweight.calculatedweek;
        //                                }
        //                            }
        //                            var abca = objWeightHelper.GroupBy(q => q.dt).Select(group => new
        //                            {
        //                                Metric = group.Key,
        //                                Count = group.Count()
        //                            }).ToList();
        //                            int currentweeks = (int)((Convert.ToDateTime(createddate222) - lmpValue).TotalDays) / 6;
        //                            int currentweek_ = currentweeks + 1;
        //                            int existnce = 0;
        //                            int conter = 0;
        //                            foreach (var item in abca)
        //                            {
        //                                existnce = (int)((Convert.ToDateTime(item.Metric) - lmpValue).TotalDays / 6) + 1;
        //                                if (existnce == currentweek_)
        //                                {
        //                                    conter = conter + 1;
        //                                }
        //                            }
        //                            if (conter >= 1)
        //                            {
        //                                flag = true;
        //                                var dialog = new MessageDialog("Are you sure you want to input weight more than once in a week?");
        //                                dialog.Title = "Confirmation";
        //                                dialog.Commands.Add(new UICommand { Label = "yes", Id = 0 });
        //                                dialog.Commands.Add(new UICommand { Label = "No", Id = 1 });
        //                                var res = await dialog.ShowAsync();
        //                                if ((int)res.Id == 0)
        //                                {
        //                                    weightEntries.ProfileID = App.userId;
        //                                    weightEntries.Weight = Convert.ToDecimal(weighttxt.Text);
        //                                    weightEntries.LoggedInUser = App.userName;
        //                                    var serializedPatchDoc = JsonConvert.SerializeObject(weightEntries);
        //                                    var method = new HttpMethod("POST");
        //                                    var request = new HttpRequestMessage(method,
        //                                    App.BASE_URL + "/api/WeightEntries/SaveWeightTrackerType")
        //                                    {
        //                                        Content = new StringContent(serializedPatchDoc,
        //                                    System.Text.Encoding.Unicode, "application/json")
        //                                    };
        //                                    HttpClient client = new HttpClient();
        //                                    var result = client.SendAsync(request).Result;
        //                                    client.Dispose();
        //                                    if (result.IsSuccessStatusCode == false)
        //                                    {
        //                                        MessageDialog msgDialog = new MessageDialog("unsuccessful", "Failure");
        //                                        msgDialog.ShowAsync();
        //                                    }

        //                                }
        //                                if ((int)res.Id == 1)
        //                                {
        //                                    this.InputWeightPag_Load(null, null);
        //                                };
        //                            }
        //                            else
        //                            {
        //                                weightEntries.ProfileID = App.userId;
        //                                weightEntries.Weight = Convert.ToDecimal(weighttxtland.Text);
        //                                weightEntries.LoggedInUser = App.userName;
        //                                var serializedPatchDoc = JsonConvert.SerializeObject(weightEntries);
        //                                var method = new HttpMethod("POST");
        //                                var request = new HttpRequestMessage(method,
        //                                App.BASE_URL + "/api/WeightEntries/SaveWeightTrackerType")
        //                                {
        //                                    Content = new StringContent(serializedPatchDoc,
        //                                System.Text.Encoding.Unicode, "application/json")
        //                                };
        //                                HttpClient client = new HttpClient();
        //                                var result = client.SendAsync(request).Result;
        //                                client.Dispose();
        //                                if (result.IsSuccessStatusCode == false)
        //                                {
        //                                    MessageDialog msgDialog = new MessageDialog("unsuccessful", "Failure");
        //                                    msgDialog.ShowAsync();
        //                                }

        //                            }
        //                        }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    MessageDialog msgDialog = new MessageDialog("The required resources are not downloaded.Please check your internet connectivity. If the problem persists, please contact advocate healthcare customer care associate.", "Message");
        //                    msgDialog.ShowAsync();
        //                }
        //            }
        //            else
        //            {
        //                MessageDialog msgDialog = new MessageDialog("Input Weight should not be empty or zero.", "Message");
        //                msgDialog.ShowAsync();
        //            }
        //        }
        //    }
        //    else
        //    {
        //        MessageDialog msgDialog = new MessageDialog("Please check your internet connection and try again", "Internet Connection is not available");
        //        msgDialog.ShowAsync();
        //    }
        //    weighttxtland.Text = "";
        //    datep1.Date = System.DateTime.Now;
        //    this.InputWeightPag_Load(null, null);
        //}

        //public void GetKeyboardProperties()
        //{
        //    KeyboardCapabilities keyboardCapabilities = new Windows.Devices.Input.KeyboardCapabilities();
        //    KeyboardPresent.Text = keyboardCapabilities.KeyboardPresent != 0 ? "Yes" : "No";
        //}

        async void Weightinputbutton(object sender, RoutedEventArgs e)
        {
            bool flag = false;
            if (App.IsInternet() == true)
            {
                Regex r = new Regex("^[a-zA-Z]*$");
                Regex rs = new Regex(@"[^0-9]+");
                string txtToValidation;

                txtToValidation = weighttxt.Text;





                if (r.IsMatch(txtToValidation) || rs.IsMatch(txtToValidation))
                {
                    MessageDialog msgDialog = new MessageDialog("Please enter valid weight to proceed. Example: 123", "Message");
                    msgDialog.ShowAsync();
                }
                else {
                    if (weighttxt.Text.Trim() != "" && weighttxt.Text != "0")
                    {
                        try
                        {
                            MODEL.WeightEntries weightEntries = new MODEL.WeightEntries();
                            //DateTime xxxx =Convert.ToDateTime( _WeightInputDate.Date);
                            //xxxx.ToUniversalTime(DateTime.Now);
                            //   weightEntries.CreatedDate = 


                            string datess = Convert.ToString(datep.Date);
                            var createddates_ = datess.Split(' ');
                            DateTime createddate222 = Convert.ToDateTime(createddates_[0]);

                            weightEntries.CreatedDate = createddate222;

                            DateTime dtm = createddate222;


                            if (dtm.Date < lmpValue.Date)
                            {
                                MessageDialog msgDialog = new MessageDialog("Date cannot be less than LMP date.", "Message");
                                msgDialog.ShowAsync();
                            }
                            else {
                                if (dtm > DateTime.Now)
                                {
                                    flag = true;
                                    MessageDialog msgDialog = new MessageDialog("You cannot input weight for future.", "Message");
                                    msgDialog.ShowAsync();
                                }
                                else if (currentweek != "")
                                {
                                    objWeightHelper = new List<WeightHelper>();
                                    string ServiceCall = App.BASE_URL + "/api/WeightEntries/GetWeightEntries?UserId=" + App.userId;
                                    var client1 = new HttpClient();
                                    HttpResponseMessage response = await client1.GetAsync(new Uri(ServiceCall));
                                    var jsonString = await response.Content.ReadAsStringAsync();
                                    JArray jobject = JArray.Parse(jsonString);
                                    foreach (var item in jobject)
                                    {
                                        WeightHelper objweight = new WeightHelper();
                                        objweight.CreatedDate = (string)item["CreatedDate"];
                                        var stringArray = ((string)item["CreatedDate"]).Split(' ');
                                        objweight.dt = Convert.ToDateTime(stringArray[1]);
                                        objweight.Weight = (string)item["Weight"];
                                        objweight.LMPDATE = (DateTime)item["LMPDATE"];



                                        int dateX = (int)((objweight.dt - objweight.LMPDATE).TotalDays) / 6;
                                        if (dateX == 0)
                                        {
                                            objweight.calculatedweek = dateX + 1;
                                            //double week_ = ((DateTime.Now - objweight.LMPDATE).TotalDays) / 7;
                                            int weekx = (int)((DateTime.Now - objweight.LMPDATE).TotalDays) / 6;
                                            //int week = Coelse {
                                            if (weekx == 0)
                                            {
                                                weeks = weekx + 1;
                                            }
                                            else
                                            {
                                                weeks = weekx;
                                            }
                                            currentweek = Convert.ToString(weeks);
                                            objWeightHelper.Add(objweight);
                                            dateweek = objweight.calculatedweek;
                                        }
                                        else
                                        {
                                            objweight.calculatedweek = dateX + 1;
                                            //double week_ = ((DateTime.Now - objweight.LMPDATE).TotalDays) / 7;
                                            int weekx = (int)((DateTime.Now - objweight.LMPDATE).TotalDays) / 6;
                                            //int week = Convert.ToInt32(Math.Round(week_));
                                            if (weekx == 0)
                                            {
                                                weeks = weekx + 1;
                                            }
                                            else
                                            {
                                                weeks = weekx;
                                            }
                                            currentweek = Convert.ToString(weeks);
                                            objWeightHelper.Add(objweight);
                                            dateweek = objweight.calculatedweek;
                                        }
                                    }
                                    var abca = objWeightHelper.GroupBy(q => q.dt).Select(group => new
                                    {
                                        Metric = group.Key,
                                        Count = group.Count()
                                    }).ToList();
                                    int currentweeks = (int)((Convert.ToDateTime(createddate222) - lmpValue).TotalDays) / 6;
                                    int currentweek_ = currentweeks + 1;
                                    int existnce = 0;
                                    int conter = 0;
                                    foreach (var item in abca)
                                    {
                                        existnce = (int)((Convert.ToDateTime(item.Metric) - lmpValue).TotalDays / 6) + 1;
                                        if (existnce == currentweek_)
                                        {
                                            conter = conter + 1;
                                        }
                                    }
                                    if (conter >= 1)
                                    {
                                        flag = true;
                                        var dialog = new MessageDialog("Are you sure you want to input weight more than once in a week?");
                                        dialog.Title = "Confirmation";
                                        dialog.Commands.Add(new UICommand { Label = "yes", Id = 0 });
                                        dialog.Commands.Add(new UICommand { Label = "No", Id = 1 });
                                        var res = await dialog.ShowAsync();
                                        if ((int)res.Id == 0)
                                        {
                                            weightEntries.ProfileID = App.userId;
                                            weightEntries.Weight = Convert.ToDecimal(weighttxt.Text);
                                            weightEntries.LoggedInUser = App.userName;
                                            var serializedPatchDoc = JsonConvert.SerializeObject(weightEntries);
                                            var method = new HttpMethod("POST");
                                            var request = new HttpRequestMessage(method,
                                            App.BASE_URL + "/api/WeightEntries/SaveWeightTrackerType")
                                            {
                                                Content = new StringContent(serializedPatchDoc,
                                            System.Text.Encoding.Unicode, "application/json")
                                            };
                                            HttpClient client = new HttpClient();
                                            var result = client.SendAsync(request).Result;
                                            client.Dispose();
                                            if (result.IsSuccessStatusCode == false)
                                            {
                                                MessageDialog msgDialog = new MessageDialog("unsuccessful", "Failure");
                                                msgDialog.ShowAsync();
                                            }

                                        }
                                        if ((int)res.Id == 1)
                                        {
                                            this.InputWeightPag_Load(null, null);
                                        };
                                    }
                                    else
                                    {
                                        weightEntries.ProfileID = App.userId;
                                        weightEntries.Weight = Convert.ToDecimal(weighttxt.Text);
                                        weightEntries.LoggedInUser = App.userName;
                                        var serializedPatchDoc = JsonConvert.SerializeObject(weightEntries);
                                        var method = new HttpMethod("POST");
                                        var request = new HttpRequestMessage(method,
                                        App.BASE_URL + "/api/WeightEntries/SaveWeightTrackerType")
                                        {
                                            Content = new StringContent(serializedPatchDoc,
                                        System.Text.Encoding.Unicode, "application/json")
                                        };
                                        HttpClient client = new HttpClient();
                                        var result = client.SendAsync(request).Result;
                                        client.Dispose();
                                        if (result.IsSuccessStatusCode == false)
                                        {
                                            MessageDialog msgDialog = new MessageDialog("unsuccessful", "Failure");
                                            msgDialog.ShowAsync();
                                        }

                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageDialog msgDialog = new MessageDialog("The required resources are not downloaded.Please check your internet connectivity. If the problem persists, please contact advocate healthcare customer care associate.", "Message");
                            msgDialog.ShowAsync();
                        }
                    }
                    else
                    {
                        MessageDialog msgDialog = new MessageDialog("Input Weight should not be empty or zero.", "Message");
                        msgDialog.ShowAsync();
                    }
                }
                this.InputWeightPag_Load(null, null);
            }
            else
            {
                MessageDialog msgDialog = new MessageDialog("Please check your internet connection and try again", "Internet Connection is not available");
                msgDialog.ShowAsync();
            }
            weighttxt.Text = "";
            datep.Date = System.DateTime.Now;
            //  this.InputWeightPag_Load(null, null);
            //    GetAll(null, null);
        }


        async void GetAll(object sender, RoutedEventArgs e)
        {
            objWeightHelper = new List<WeightHelper>();
            //string ServiceCall = App.BASE_URL + "/api/WeightEntries/GetWeightEntries?UserId=2";
            string ServiceCall = App.BASE_URL + "/api/WeightEntries/GetWeightEntries?UserId=" + App.userId;
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(new Uri(ServiceCall));
            var jsonString = await response.Content.ReadAsStringAsync();
            JArray jobject = JArray.Parse(jsonString);
            var object1 = jobject.FirstOrDefault();
            if (object1 != null)
            {
                string createddates = object1.ToList()[3].ToString();
                var createddates_ = createddates.Split(' ');
                DateTime createddate2 = Convert.ToDateTime(createddates_[2]);
                string lmpdate = object1.ToList()[2].ToString();
                var lmpdate_ = lmpdate.Split(' ');

                string fff = (lmpdate_[1]);
                DateTime lmpdate1_ = Convert.ToDateTime(fff.Substring(1, 10));
                int dateq = (int)((createddate2 - lmpdate1_).TotalDays) / 6;
                if (dateq == 0)
                {
                    mydate = dateq + 1;
                }
                else
                {
                    mydate = dateq;
                }
            }
            foreach (var item in jobject)
            {
                WeightHelper objweight = new WeightHelper();
                objweight.CreatedDate = (string)item["CreatedDate"];
                var stringArray = ((string)item["CreatedDate"]).Split(' ');
                objweight.dt = Convert.ToDateTime(stringArray[1]);
                objweight.Weight = (string)item["Weight"];
                objweight.LMPDATE = (DateTime)item["LMPDATE"];
                int dateX = (int)((objweight.dt - objweight.LMPDATE).TotalDays) / 6;
                if (dateX == 0)
                {
                    objweight.calculatedweek = dateX + 1;
                    //double week_ = ((DateTime.Now - objweight.LMPDATE).TotalDays) / 7;
                    int weekx = (int)((DateTime.Now - objweight.LMPDATE).TotalDays) / 6;
                    //int week = Coelse {
                    if (weekx == 0)
                    {
                        weeks = weekx + 1;
                    }
                    else
                    {
                        weeks = weekx + 1;
                    }
                    currentweek = Convert.ToString(weeks);
                    objWeightHelper.Add(objweight);
                    dateweek = objweight.calculatedweek;
                }
                else
                {
                    objweight.calculatedweek = dateX + 1;
                    //double week_ = ((DateTime.Now - objweight.LMPDATE).TotalDays) / 7;
                    int weekx = (int)((DateTime.Now - objweight.LMPDATE).TotalDays) / 6;
                    //int week = Convert.ToInt32(Math.Round(week_));
                    if (weekx == 0)
                    {
                        weeks = weekx + 1;
                    }
                    else
                    {
                        weeks = weekx + 1;
                    }
                    currentweek = Convert.ToString(weeks);
                    objWeightHelper.Add(objweight);
                    dateweek = objweight.calculatedweek;
                }
            }
            ConstructTileGrid(objWeightHelper);
        }
    }

    namespace MODEL
    {
        public class WeightEntries
        {
            public decimal? Weight { get; set; }
            public DateTime CreatedDate { get; set; }
            public string LoggedInUser { get; set; }
            public int WeightTrackerID { get; set; }
            public int? ProfileID { get; set; }
            public DateTime? LMPDATE { get; set; }
        }

    }

}

