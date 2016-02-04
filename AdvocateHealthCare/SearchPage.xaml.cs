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
    /// 

    public sealed partial class SearchPage : Page
    {

        public DisplayOrientations orientation = DisplayOrientations.Landscape;
        public SearchPage()
        {
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
            this.InitializeComponent();
            txtNotificationCount.Text = HomePage.unreadNotificationCount.ToString();
            DisplayProperties.OrientationChanged += Page_OrientationChanged;
        }
        public void Page_OrientationChanged(object sender)
        {
            //The orientation of the device is ...
            orientation = DisplayProperties.CurrentOrientation;
            if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
            {

                ConstructTileGrids(lstSearChHelper);
            }

            if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
            {

                //VerticallyFlipped();
                ConstructTileGrids(lstSearChHelper);
            }

        }
        public class SearchHelper
        {
            public string SearchResultTitle { get; set; }
            public string SearchResultContent { get; set; }
            public Uri SearchResultImageUrl { get; set; }
        }
        SearchHelper objSearchHelper;
        List<SearchHelper> lstSearChHelper;
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            //get the searched query result
            if (App.IsInternet() == true)
            {
                try
                {
                    lstSearChHelper = new List<SearchHelper>();
                    var SearchedQuery = e.Parameter;
                    string QuerySearchUrl = App.BASE_URL + "/api/ContentSearch/GetContentSearch?String=" + SearchedQuery;
                    var client = new HttpClient();
                    HttpResponseMessage response = await client.GetAsync(new Uri(QuerySearchUrl));
                    string jsonString = await response.Content.ReadAsStringAsync();
                    JArray jArr = JArray.Parse(jsonString);
                    if (jArr.Count != 0)
                    {
                        for (int itemCount = 0; itemCount < jArr.Count; itemCount++)
                        {
                            objSearchHelper = new SearchHelper();
                            objSearchHelper.SearchResultTitle = (string)jArr[itemCount]["Title"];
                            objSearchHelper.SearchResultContent = (string)jArr[itemCount]["Content"];
                            var x = App.BASE_URL + jArr[itemCount]["TileImage"];
                            //objSearchHelper.SearchResultImageUrl = (Uri)jArr[itemCount]["TileImage"];
                            Uri uri = new Uri(x);
                            objSearchHelper.SearchResultImageUrl = uri;
                            lstSearChHelper.Add(objSearchHelper);
                        }
                        ConstructTileGrids(lstSearChHelper);
                    }
                    else
                    {
                        MessageDialog msgDialog = new MessageDialog("No data found on " + SearchedQuery, "No Result");
                        msgDialog.ShowAsync();
                        this.Frame.Navigate(typeof(HomePage));
                    }
                }
                catch (Exception)
                {

                    throw;
                }

            }
            else
            {
                MessageDialog msgDialog = new MessageDialog("Please check your internet connection and try again", "Internet Connection is not available");
                msgDialog.ShowAsync();

            }
        }
        private void Notificationgridtapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Notifications));
        }
        private void mySearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            this.Frame.Navigate(typeof(SearchPage), args.QueryText);
        }

        private void grdSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (SearchHelper)(sender as GridView).SelectedItem;
            if (item.SearchResultTitle == "Diet and Pregnancy")
            {
                this.Frame.Navigate(typeof(DietandPregnancy));
            }
            else if (item.SearchResultTitle == " My Advocate Portal")
            {
                this.Frame.Navigate(typeof(MyAdvocatePage));
            }
        }
        public void ConstructTileGrids(List<SearchHelper> lstSearChHelper)
        {
            searchgrid.RowDefinitions.Clear();
            searchgrid.ColumnDefinitions.Clear();
            searchgrid.Children.Clear();

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
            if (lstSearChHelper.Count < 2)
            {
                //1st row
                RowDefinition rd1 = new RowDefinition();
                GridLength gl1 = new GridLength(330);

                rd1.Height = gl1;
                searchgrid.RowDefinitions.Add(rd1);
            }
            else
            {
                RowDefinition rd12 = new RowDefinition();
                GridLength gl12 = new GridLength(330);
                rd12.Height = gl12;
                searchgrid.RowDefinitions.Add(rd12);
            }
           
               


            for (int i = 0; i < lstSearChHelper.Count; i++)
            {
                //ScrollViewer scrollViewer = new ScrollViewer();




                Grid childGrid = null;
                childGrid = new Grid();
                //event
                //   childGrid.HorizontalAlignment = HorizontalAlignment.Center;
                childGrid.Background = new SolidColorBrush(Colors.White);
                childGrid.BorderThickness = new Thickness(1);
                childGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));

              childGrid.Tapped += ChildGrid_Tapped;

                //  childGrid.Tapped += ChildGrid_Tapped;

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
                bitmapImage.UriSource = lstSearChHelper[i].SearchResultImageUrl;
                img.Source = bitmapImage;
                img.Stretch = Stretch.Fill;

                Grid.SetRow(img, 0);
                childGrid.Children.Add(img);

                //  < TextBlock Grid.Row = "1" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "20"   Foreground = "#DF6C3F"  Text = "{Binding DeliveryTitle}" Margin = "10,0,0,0" ></ TextBlock >

                TextBlock tb1 = new TextBlock();
                tb1.Name = "TileName";
                if (lstSearChHelper[i].SearchResultTitle == null)
                {

                }
                else
                {
                    tb1.Text = lstSearChHelper[i].SearchResultTitle;
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
                if (lstSearChHelper[i].SearchResultContent == null)
                {

                }
                else
                {
                    tb2.Text = lstSearChHelper[i].SearchResultContent;
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
                        RowDefinition rd = new RowDefinition();
                        GridLength gl = new GridLength(330);
                        rd.Height = gl;
                        searchgrid.RowDefinitions.Add(rd);

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
                        GridLength gl = new GridLength(330);
                        rd.Height = gl;
                        searchgrid.RowDefinitions.Add(rd);

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
                        GridLength gl = new GridLength(300);
                        rd.Height = gl;
                        searchgrid.RowDefinitions.Add(rd);

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
                searchgrid.Children.Add(childGrid);
            }
        }
        private async void ChildGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {


            foreach (Grid cg in searchgrid.Children)
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
                    if (t.Text == "Diet and Pregnancy")
                    {

                        this.Frame.Navigate(typeof(DietandPregnancy));

                    }
                    else if (t.Text == "My Advocate Portal")
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
            searchgrid.ColumnDefinitions.Add(cd1);

            //2st column
            ColumnDefinition cd2 = new ColumnDefinition();
            GridLength gl2 = new GridLength(1, GridUnitType.Star);
            cd2.Width = gl2;
            searchgrid.ColumnDefinitions.Add(cd2);

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
            searchgrid.ColumnDefinitions.Add(cd1);

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