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
    ///<summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    ///</summary>
    public sealed partial class VideosPage : Page
    {
        public DisplayOrientations orientation = DisplayOrientations.Landscape;
        public VideosPage()
        {
            this.InitializeComponent();
            //this.Loaded += VideosPage_Loaded;
            txtNotificationCount.Text = HomePage.unreadNotificationCount.ToString();
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

                ConstructTileGridall(objListVideoPlayer);
                ConstructTileGrid2(objListVideoPlayer);
                ConstructTileGrid3(objListVideoPlayer);
                ConstructTileGrid4(objListVideoPlayer);
            }

            if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
            {

                //VerticallyFlipped();
                ConstructTileGridall(objListVideoPlayer);
                ConstructTileGrid2(objListVideoPlayer);
                ConstructTileGrid3(objListVideoPlayer);
                ConstructTileGrid4(objListVideoPlayer);
            }

        }
        public class VideoPlayerHelper
        {
            public string VideoHeader { get; set; }
            public Uri VideoUrl { get; set; }
            public string LocalImagePath { get; set; }
            public BitmapImage LocalImagePath2 { get; set; }
            public int VideoID { get; set; }

        }

        private void gridVideosDisplay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VideoPlayerHelper value = (VideoPlayerHelper)(sender as GridView).SelectedItem;
            int uri = value.VideoID;
            // (App.Current as App).NavigateText = uri;
            this.Frame.Navigate(typeof(PlayVideo));

        }


        private void mySearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            this.Frame.Navigate(typeof(SearchPage), args.QueryText);
        }

        private void Notificationgridtapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Notifications));
        }
        public class DeliveryInformation
        {
            public string DeliveryTitle { get; set; }
            public string DeliveryInfo { get; set; }
            public Uri DeliveryUrl { get; set; }
        }
        string ActiveItemHeaderName;
        List<VideoPlayerHelper> objListVideoPlayer = new List<VideoPlayerHelper>();
        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            objListVideoPlayer.Clear();
            PivotItem currentItem = e.AddedItems[0] as PivotItem;
            ActiveItemHeaderName = currentItem.Header.ToString();
            switch (ActiveItemHeaderName)
            {
                case "All":
                    DeliveryInfo(null);
                    break;

                case "Pre Delivery":
                    DeliveryInfo("1");
                    break;

                case "General":
                    DeliveryInfo("4");
                    break;

                case "Post Delivery":
                    DeliveryInfo("3");
                    break;
            }


        }
        VideoPlayerHelper objVideoPlayerHelper4 = new VideoPlayerHelper();
        VideoPlayerHelper objVideoPlayerHelper1 = new VideoPlayerHelper();
        DeliveryInformation objDeliveryInformation = new DeliveryInformation();
        VideoPlayerHelper objVideoPlayerHelper;
        //List<VideoPlayerHelper> listHelper4 = new List<VideoPlayerHelper>();

        string VideoID;
        public async void DeliveryInfo(string id)
        {
            if (App.IsInternet() == true)
            {
                try
                {

                    //string DeliveryInfoUri = App.BASE_URL + "/api/Tiles/GetTilesById?SUBCATEGORYID=" + id;
                    string GetVideos = App.BASE_URL + "/api/JournalVideos/GetJournalVideos?SUBCATEGORYID= " + id;
                    var client = new HttpClient();
                    HttpResponseMessage response = await client.GetAsync(new Uri(GetVideos));
                    var jsonString = await response.Content.ReadAsStringAsync();
                    JArray jArr = JArray.Parse(jsonString);
                    var elementsCount = jArr.Count;
                    foreach (var item in jArr)
                    {
                        //VideoPlayerHelper objVideoPlayerHelper = new VideoPlayerHelper();
                        //objVideoPlayerHelper.VideoID = (int)item["$id"];
                        //VideoID = objVideoPlayerHelper.VideoID.ToString();

                        if (ActiveItemHeaderName == "General")
                        {
                            objVideoPlayerHelper1 = new VideoPlayerHelper();
                            objVideoPlayerHelper1.VideoID = (int)item["$id"];
                            VideoID = objVideoPlayerHelper1.VideoID.ToString();
                            //VideoID = objVideoPlayerHelper.VideoID.ToString();
                            objVideoPlayerHelper1.VideoHeader = (string)item["JournalVideoName"];
                            string x = App.BASE_URL + item["JournalVideoAsset"];
                            Uri uri = new Uri(x);
                            objVideoPlayerHelper1.VideoUrl = uri;
                            //  objListVideoPlayer.Add(objVideoPlayerHelper);
                            objListVideoPlayer.Add(objVideoPlayerHelper1);

                        }
                        else if (ActiveItemHeaderName == "Pre Delivery")
                        {
                            VideoPlayerHelper objVideoPlayerHelper2 = new VideoPlayerHelper();
                            objVideoPlayerHelper2.VideoID = (int)item["$id"];
                            VideoID = objVideoPlayerHelper2.VideoID.ToString();
                            switch (VideoID)
                            {

                                //case "1":
                                //    objVideoPlayerHelper2.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_FluVaccinations.jpg", UriKind.Absolute));//done
                                //    break;
                                //case "2":
                                //    objVideoPlayerHelper2.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_OpioidPainkillers.jpg", UriKind.Absolute));//done
                                //    break;
                                //case "3":
                                //    objVideoPlayerHelper2.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_HighRiskPregnancy.jpg", UriKind.Absolute));//done
                                //    break;
                                //case "4":
                                //    objVideoPlayerHelper2.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_MeetDrTherese.jpg", UriKind.Absolute));//done
                                //    break;
                            }
                            objVideoPlayerHelper2.VideoHeader = (string)item["JournalVideoName"];
                            string x = App.BASE_URL + item["JournalVideoAsset"];
                            Uri uri = new Uri(x);
                            objVideoPlayerHelper2.VideoUrl = uri;
                            objListVideoPlayer.Add(objVideoPlayerHelper2);
                        }
                        else if (ActiveItemHeaderName == "Post Delivery")
                        {
                            VideoPlayerHelper objVideoPlayerHelper3 = new VideoPlayerHelper();
                            objVideoPlayerHelper3.VideoID = (int)item["$id"];
                            VideoID = objVideoPlayerHelper3.VideoID.ToString();
                            switch (VideoID)
                            {

                                //case "1":
                                //    objVideoPlayerHelper3.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_KnowAboutBreastfeeding.jpg", UriKind.Absolute));//done
                                //    break;
                                //case "2":
                                //    objVideoPlayerHelper3.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_Swaddle.jpg", UriKind.Absolute));//done
                                //    break;
                                //case "3":
                                //    objVideoPlayerHelper3.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_BabysFirstBath.jpg", UriKind.Absolute));//done
                                //    break;
                                //case "4":
                                //    objVideoPlayerHelper3.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_BathingTips.jpg", UriKind.Absolute));//done
                                //    break;
                                //case "5":
                                //    objVideoPlayerHelper3.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_BreastfeedingBenefits.jpg", UriKind.Absolute));//done
                                //    break;
                                //case "6":
                                //    objVideoPlayerHelper3.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_DiaperRash.jpg", UriKind.Absolute));//done
                                //    break;
                                //case "7":
                                //    objVideoPlayerHelper3.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_PickAPediatrician.jpg", UriKind.Absolute));//done
                                //    break;
                                //case "8":
                                //    objVideoPlayerHelper3.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_InfantCheckup.jpg", UriKind.Absolute));//done
                                //    break;
                                //case "9":
                                //    objVideoPlayerHelper3.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_CommonNewParentQuestions.jpg", UriKind.Absolute));//done
                                //    break;
                            }
                            objVideoPlayerHelper3.VideoHeader = (string)item["JournalVideoName"];
                            string x = App.BASE_URL + item["JournalVideoAsset"];
                            Uri uri = new Uri(x);
                            objVideoPlayerHelper3.VideoUrl = uri;
                            objListVideoPlayer.Add(objVideoPlayerHelper3);

                        }

                        if (ActiveItemHeaderName == "All")
                        {
                            objVideoPlayerHelper4 = new VideoPlayerHelper();
                            objVideoPlayerHelper4.VideoID = (int)item["$id"];
                            VideoID = objVideoPlayerHelper4.VideoID.ToString();
                            List<VideoPlayerHelper> listHelper4 = new List<VideoPlayerHelper>();


                            objVideoPlayerHelper4.VideoHeader = (string)item["JournalVideoName"];
                            string x = App.BASE_URL + item["JournalVideoAsset"];
                            Uri uri = new Uri(x);
                            objVideoPlayerHelper4.VideoUrl = uri;
                            objListVideoPlayer.Add(objVideoPlayerHelper4);
                        }

                    }
                    switch (ActiveItemHeaderName)
                    {
                        case "All":
                            // gridVideosDisplay.ItemsSource = objListVideoPlayer;

                            ConstructTileGridall(objListVideoPlayer);
                            break;
                        case "General":
                            ConstructTileGrid2(objListVideoPlayer);
                            //gridVideosDisplay2.ItemsSource = objListVideoPlayer;
                            break;
                        case "Pre Delivery":
                            //gridVideosDisplay1.ItemsSource = objListVideoPlayer;
                            ConstructTileGrid3(objListVideoPlayer);
                            break;
                        case "Post Delivery":
                            ConstructTileGrid4(objListVideoPlayer);
                            break;
                    }
                    //gridVideosDisplay.ItemsSource = objListVideoPlayer;
                }
                catch (Exception ex)
                {
                    MessageDialog msgDialog = new MessageDialog("The required resources are not downloaded. Please check your internet connectivity. If the problem persists, please contact advocate healthcare customer care associate.", "Message");
                    msgDialog.ShowAsync();
                }

            }
        }

        private void gridVideosDisplay1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VideoPlayerHelper value = (VideoPlayerHelper)(sender as GridView).SelectedItem;
            int uri = value.VideoID;
            // (App.Current as App).NavigateText = uri;
            this.Frame.Navigate(typeof(PlayVideo));
        }
        public void AddColumnsToTileGridPortrait()
        {
          
            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            tileGridVideos2.ColumnDefinitions.Add(cd1);


        }

        private void gridVideosDisplay2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VideoPlayerHelper value = (VideoPlayerHelper)(sender as GridView).SelectedItem;
            int uri = value.VideoID;
            // (App.Current as App).NavigateText = uri;
            this.Frame.Navigate(typeof(PlayVideo));
        }

        private void gridVideosDisplay3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VideoPlayerHelper value = (VideoPlayerHelper)(sender as GridView).SelectedItem;
            int uri = value.VideoID;
            // (App.Current as App).NavigateText = uri;
            this.Frame.Navigate(typeof(PlayVideo));
        }
        public void ConstructTileGridall(List<VideoPlayerHelper> deliveryInfoList)
        {
            tileGridVideos123.Children.Clear();
            tileGridVideos123.RowDefinitions.Clear();
            tileGridVideos123.ColumnDefinitions.Clear();

            
           

            if (orientation == DisplayOrientations.PortraitFlipped || orientation == DisplayOrientations.Portrait)
            {
                AddColumnsToTileGridPortraitalall();

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
            RowDefinition rd1 = new RowDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            rd1.Height = gl1;
            tileGridVideos123.RowDefinitions.Add(rd1);


            for (int i = 0; i < deliveryInfoList.Count; i++)
            {
                //ScrollViewer scrollViewer = new ScrollViewer();

                Grid childGrid = null;
                childGrid = new Grid();
                //event

                childGrid.Background = new SolidColorBrush(Colors.White);
                childGrid.BorderThickness = new Thickness(1);
                childGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));

                childGrid.Tapped += ChildGrid1_Tapped;

                childGrid.Margin = new Thickness(10, 10, 10, 10);

                //row1
                RowDefinition childGridRow1 = new RowDefinition();
                GridLength cgl1 = new GridLength(0.3, GridUnitType.Star);
                childGridRow1.Height = cgl1;
                childGrid.RowDefinitions.Add(childGridRow1);
                //row2
                RowDefinition childGridRow2 = new RowDefinition();
                //childGridRow2.
                GridLength cgl2 = new GridLength(1, GridUnitType.Star);
                childGridRow2.Height = cgl2;

                childGrid.RowDefinitions.Add(childGridRow2);



                // < Image  Grid.Row = "0" Source = "{Binding DeliveryUrl}" Stretch = "Fill" ></ Image >
                Image img = new Image();

                var videoID = deliveryInfoList[i].VideoID;
                if (ActiveItemHeaderName == "All")
                {
                    switch (videoID)
                    {
                        case 1:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_PregnancyPlanner.jpg", UriKind.Absolute));//done
                            break;
                        case 2:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_MeetDrSasaki.jpg", UriKind.Absolute));//done
                            break;
                        case 3:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_MeetDrSeetal.jpg", UriKind.Absolute));//done
                            break;
                        case 4:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_HowOldIsTooOld.jpg", UriKind.Absolute));//done
                            break;
                        case 5:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_Mommyrexia.jpg", UriKind.Absolute));//done
                            break;
                        case 6:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_FluVaccinations.jpg", UriKind.Absolute));//done
                            break;
                        case 7:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_OpioidPainkillers.jpg", UriKind.Absolute));//done
                            break;
                        case 8:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_HighRiskPregnancy.jpg", UriKind.Absolute));//done
                            break;
                        case 9:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_MeetDrTherese.jpg", UriKind.Absolute));//done
                            break;
                        case 10:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_KnowAboutBreastfeeding.jpg", UriKind.Absolute));//done
                            break;
                        case 11:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_Swaddle.jpg", UriKind.Absolute));//done
                            break;
                        case 12:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_BabysFirstBath.jpg", UriKind.Absolute));//done
                            break;
                        case 13:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_BathingTips.jpg", UriKind.Absolute));//done
                            break;
                        case 14:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_BreastfeedingBenefits.jpg", UriKind.Absolute));//done
                            break;
                        case 15:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_DiaperRash.jpg", UriKind.Absolute));//done
                            break;
                        case 16:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_PickAPediatrician.jpg", UriKind.Absolute));//done
                            break;
                        case 17:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_InfantCheckup.jpg", UriKind.Absolute));//done
                            break;

                        case 18:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_CommonNewParentQuestions.jpg", UriKind.Absolute));//done
                            break;
                    }
                }
                else if (ActiveItemHeaderName == "General")
                {
                    switch (videoID)
                    {
                        case 1:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_PregnancyPlanner.jpg", UriKind.Absolute));//done
                            break;
                        case 2:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_MeetDrSasaki.jpg", UriKind.Absolute));//done
                            break;
                        case 3:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_MeetDrSeetal.jpg", UriKind.Absolute));//done
                            break;
                        case 4:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_HowOldIsTooOld.jpg", UriKind.Absolute));//done
                            break;
                        case 5:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_Mommyrexia.jpg", UriKind.Absolute));//done
                            break;
                    }
                }
                //BitmapImage bitmapImage = new BitmapImage();
                //bitmapImage.UriSource = Imguri;
                // img.Source = bitmapImage;
                img.Stretch = Stretch.Fill;
                img.Margin = new Thickness(0, 12, 0, 0);
                //  img.HorizontalAlignment = HorizontalAlignment.Stretch;
                //img.VerticalAlignment = VerticalAlignment.Stretch;


                Grid.SetRow(img, 1);
                childGrid.Children.Add(img);

                //  < TextBlock Grid.Row = "1" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "20"   Foreground = "#DF6C3F"  Text = "{Binding DeliveryTitle}" Margin = "10,0,0,0" ></ TextBlock >

                TextBlock tb1 = new TextBlock();
                tb1.Name = "TileName";
                tb1.Text = deliveryInfoList[i].VideoHeader;
                tb1.TextTrimming = TextTrimming.WordEllipsis;
                tb1.FontSize = 20;
                tb1.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
                tb1.FontWeight = FontWeights.SemiLight;
                tb1.Margin = new Thickness(10, 0, 0, 0);

                TextBlock hidenTb = new TextBlock();
                hidenTb.Name = "TileName";
                hidenTb.Text = deliveryInfoList[i].VideoID.ToString();
                hidenTb.Visibility = Visibility.Collapsed;
                hidenTb.TextTrimming = TextTrimming.WordEllipsis;
                hidenTb.FontSize = 20;
                hidenTb.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
                hidenTb.FontWeight = FontWeights.SemiLight;
                hidenTb.Margin = new Thickness(10, 0, 0, 0);

                Grid.SetRow(hidenTb, 0);
                childGrid.Children.Add(hidenTb);

                Grid.SetRow(tb1, 0);
                childGrid.Children.Add(tb1);

                //  < TextBlock Grid.Row = "2" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "17"  Foreground = "Black"  Text = "{Binding DeliveryInfo}"   Margin = "10,0,0,0" ></ TextBlock >

                //TextBlock tb2 = new TextBlock();
                //tb2.Text = deliveryInfoList[i].DeliveryInfo;
                //tb2.TextTrimming = TextTrimming.WordEllipsis;
                //tb2.FontSize = 17;
                //tb2.Foreground = new SolidColorBrush(Colors.Black);
                //tb2.FontWeight = FontWeights.SemiLight;
                //tb2.Margin = new Thickness(10, 0, 0, 0);

                //Grid.SetRow(tb2, 2);
                //childGrid.Children.Add(tb2);

                if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
                {
                    if ((i + 1) > 2 * (row + 1))
                    {
                        RowDefinition rd = new RowDefinition();
                        GridLength gl = new GridLength(1, GridUnitType.Star);
                        rd.Height = gl;
                        tileGridVideos123.RowDefinitions.Add(rd);

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
                        tileGridVideos123.RowDefinitions.Add(rd);

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
                        tileGridVideos123.RowDefinitions.Add(rd);

                        row = row + 1;
                        col = 0;
                    }
                    else
                    {
                        col = col + 1;
                    }
                }

                //if ((i + 1) > 2 * (row + 1))
                //{
                //    RowDefinition rd = new RowDefinition();
                //    GridLength gl = new GridLength(1, GridUnitType.Star);
                //    rd.Height = gl;
                //    tileGridVideos123.RowDefinitions.Add(rd);

                //    row = row + 1;
                //    col = 0;
                //}
                //else
                //{
                //    col = col + 1;
                //}

                //Add to Grid
                Grid.SetRow(childGrid, row);
                Grid.SetColumn(childGrid, col);
                tileGridVideos123.Children.Add(childGrid);

            }

        }
        public void AddColumnsToTileGridPortraitalall()
        {

            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            tileGridVideos123.ColumnDefinitions.Add(cd1);


        }
        private async void ChildGrid13_Tapped(object sender, TappedRoutedEventArgs e)
        {
            foreach (Grid cg in tileGridVideos3.Children)
            {
                cg.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
            }

          ((Windows.UI.Xaml.Controls.Grid)sender).BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
            foreach (Grid cg in tileGridVideos1.Children)
            {
                cg.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
            }

          ((Windows.UI.Xaml.Controls.Grid)sender).BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));

            var childrens = ((Windows.UI.Xaml.Controls.Panel)sender).Children;

            var textblocks = childrens.OfType<TextBlock>().FirstOrDefault();

            HomePage.navigationvideos navigationvide = new HomePage.navigationvideos();
            navigationvide.videosource = textblocks.Text;
            navigationvide.source = "VideoPage";
            await Task.Delay(1000);
            this.Frame.Navigate(typeof(PlayVideo), navigationvide);


            //  foreach (Grid cg in tileGridVideos.Children)
            //  {
            //      cg.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
            //  }

            //((Windows.UI.Xaml.Controls.Grid)sender).BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));


            //  DisplayOrientations x = orientation;
            //  var childrens = ((Windows.UI.Xaml.Controls.Panel)sender).Children;

            //  var textblocks = childrens.OfType<TextBlock>();


            //  foreach (TextBlock t in textblocks)
            //  {
            //      if (t.Name == "TileName")
            //      {
            //          if (t.Text == "Diet and Pregnancy")
            //          {


            //              this.Frame.Navigate(typeof(DietandPregnancy));

            //          }
            //          else if (t.Text == "My Advocate Portal")
            //          {
            //              this.Frame.Navigate(typeof(MyAdvocatePage));
            //          }
            //          else if (t.Text == "Humera")
            //          {
            //              HomePage.navigationvideos navigationvide = new HomePage.navigationvideos();
            //              navigationvide.videosource = "aANj9_oeyUM";
            //              navigationvide.source = "Home";
            //              this.Frame.Navigate(typeof(PlayVideo), navigationvide);
            //          }
            //          else if (t.Text == "Register")
            //          {
            //              await Windows.System.Launcher.LaunchUriAsync(new Uri("https://healthadvisor.advocatehealth.com/Classes"));
            //          }
            //      }
            //  }
        }
        private async void ChildGrid1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            foreach (Grid cg in tileGridVideos123.Children)
            {
                cg.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
            }

            ((Windows.UI.Xaml.Controls.Grid)sender).BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
            foreach (Grid cg in tileGridVideos1.Children)
            {
                cg.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
            }

            ((Windows.UI.Xaml.Controls.Grid)sender).BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));

            var childrens = ((Windows.UI.Xaml.Controls.Panel)sender).Children;

            var textblocks = childrens.OfType<TextBlock>().FirstOrDefault();


            // (App.Current as App).NavigateText = t.Text;
            //this.Frame.Navigate(typeof(PlayVideo));

            HomePage.navigationvideos navigationvide = new HomePage.navigationvideos();
            navigationvide.videosource = textblocks.Text;
            navigationvide.source = "VideoPage";
            await Task.Delay(1000);
            this.Frame.Navigate(typeof(PlayVideo), navigationvide);



            //  foreach (Grid cg in tileGridVideos.Children)
            //  {
            //      cg.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
            //  }

            //((Windows.UI.Xaml.Controls.Grid)sender).BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));


            //  DisplayOrientations x = orientation;
            //  var childrens = ((Windows.UI.Xaml.Controls.Panel)sender).Children;

            //  var textblocks = childrens.OfType<TextBlock>();


            //  foreach (TextBlock t in textblocks)
            //  {
            //      if (t.Name == "TileName")
            //      {
            //          if (t.Text == "Diet and Pregnancy")
            //          {


            //              this.Frame.Navigate(typeof(DietandPregnancy));

            //          }
            //          else if (t.Text == "My Advocate Portal")
            //          {
            //              this.Frame.Navigate(typeof(MyAdvocatePage));
            //          }
            //          else if (t.Text == "Humera")
            //          {
            //              HomePage.navigationvideos navigationvide = new HomePage.navigationvideos();
            //              navigationvide.videosource = "aANj9_oeyUM";
            //              navigationvide.source = "Home";
            //              this.Frame.Navigate(typeof(PlayVideo), navigationvide);
            //          }
            //          else if (t.Text == "Register")
            //          {
            //              await Windows.System.Launcher.LaunchUriAsync(new Uri("https://healthadvisor.advocatehealth.com/Classes"));
            //          }
            //      }
            //  }
        }
        private async void ChildGrid12_Tapped(object sender, TappedRoutedEventArgs e)
        {
            foreach (Grid cg in tileGridVideos2.Children)
            {
                cg.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
            }

          ((Windows.UI.Xaml.Controls.Grid)sender).BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
            foreach (Grid cg in tileGridVideos1.Children)
            {
                cg.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
            }

          ((Windows.UI.Xaml.Controls.Grid)sender).BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));

            var childrens = ((Windows.UI.Xaml.Controls.Panel)sender).Children;

            var textblocks = childrens.OfType<TextBlock>().FirstOrDefault();

            HomePage.navigationvideos navigationvide = new HomePage.navigationvideos();
            navigationvide.videosource = textblocks.Text;
            navigationvide.source = "VideoPage";
            await Task.Delay(1000);
            this.Frame.Navigate(typeof(PlayVideo), navigationvide);


            //  foreach (Grid cg in tileGridVideos.Children)
            //  {
            //      cg.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
            //  }

            //((Windows.UI.Xaml.Controls.Grid)sender).BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));


            //  DisplayOrientations x = orientation;
            //  var childrens = ((Windows.UI.Xaml.Controls.Panel)sender).Children;

            //  var textblocks = childrens.OfType<TextBlock>();


            //  foreach (TextBlock t in textblocks)
            //  {
            //      if (t.Name == "TileName")
            //      {
            //          if (t.Text == "Diet and Pregnancy")
            //          {


            //              this.Frame.Navigate(typeof(DietandPregnancy));

            //          }
            //          else if (t.Text == "My Advocate Portal")
            //          {
            //              this.Frame.Navigate(typeof(MyAdvocatePage));
            //          }
            //          else if (t.Text == "Humera")
            //          {
            //              HomePage.navigationvideos navigationvide = new HomePage.navigationvideos();
            //              navigationvide.videosource = "aANj9_oeyUM";
            //              navigationvide.source = "Home";
            //              this.Frame.Navigate(typeof(PlayVideo), navigationvide);
            //          }
            //          else if (t.Text == "Register")
            //          {
            //              await Windows.System.Launcher.LaunchUriAsync(new Uri("https://healthadvisor.advocatehealth.com/Classes"));
            //          }
            //      }
            //  }
        }
        /// <summary>
        /// ///common auto jany//
        /// </summary>
        /// <param name="deliveryInfoList"></param>
        //public void ConstructTileGrid(List<DeliveryInformation> deliveryInfoList)
        //{
        //    tileGridVideos123.Children.Clear();
        //    tileGridVideos123.RowDefinitions.Clear();
        //    tileGridVideos123.Children.Clear();

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
        //    tileGridVideos123.RowDefinitions.Add(rd1);


        //    for (int i = 0; i < deliveryInfoList.Count - 1; i++)
        //    {
        //        //ScrollViewer scrollViewer = new ScrollViewer();

        //        Grid childGrid = null;
        //        childGrid = new Grid();

        //        //event
        //        childGrid.Background = new SolidColorBrush(Colors.White);
        //        childGrid.BorderThickness = new Thickness(1);
        //        childGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));


        //        childGrid.Tapped += ChildGrid_Tapped;

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
        //        img.Margin = new Thickness(0, 12, 0, 0);
        //        Grid.SetRow(img, 0);
        //        childGrid.Children.Add(img);

        //        //  < TextBlock Grid.Row = "1" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "20"   Foreground = "#DF6C3F"  Text = "{Binding DeliveryTitle}" Margin = "10,0,0,0" ></ TextBlock >

        //        TextBlock tb1 = new TextBlock();
        //        tb1.Name = "TileName";
        //        if (deliveryInfoList[i].DeliveryInfo == null)
        //        {

        //        }
        //        else
        //        {
        //            tb1.Text = deliveryInfoList[i].DeliveryTitle;
        //        }

        //        tb1.TextTrimming = TextTrimming.WordEllipsis;
        //        tb1.FontSize = 20;
        //        tb1.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
        //        tb1.FontWeight = FontWeights.SemiLight;
        //        tb1.Margin = new Thickness(10, 0, 0, 0);

        //        Grid.SetRow(tb1, 1);
        //        childGrid.Children.Add(tb1);

        //        //  < TextBlock Grid.Row = "2" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "17"  Foreground = "Black"  Text = "{Binding DeliveryInfo}"   Margin = "10,0,0,0" ></ TextBlock >

        //        TextBlock tb2 = new TextBlock();
        //        if (deliveryInfoList[i].DeliveryInfo == null)
        //        {

        //        }
        //        else
        //        {
        //            tb2.Text = deliveryInfoList[i].DeliveryInfo;
        //        }

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
        //                tileGridVideos123.RowDefinitions.Add(rd);

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
        //                tileGridVideos123.RowDefinitions.Add(rd);

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
        //                tileGridVideos123.RowDefinitions.Add(rd);

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
        //        tileGridVideos123.Children.Add(childGrid);

        //    }

        //}
        private void ChildGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            foreach (Grid cg in tileGridVideos123.Children)
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
            tileGridVideos123.ColumnDefinitions.Add(cd1);

            //2st column
            ColumnDefinition cd2 = new ColumnDefinition();
            GridLength gl2 = new GridLength(1, GridUnitType.Star);
            cd2.Width = gl2;
            tileGridVideos123.ColumnDefinitions.Add(cd2);

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
        public void AddColumnsToTileGridLandscape2()
        {

            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            tileGridVideos1.ColumnDefinitions.Add(cd1);

            //2st column
            ColumnDefinition cd2 = new ColumnDefinition();
            GridLength gl2 = new GridLength(1, GridUnitType.Star);
            cd2.Width = gl2;
            tileGridVideos1.ColumnDefinitions.Add(cd2);

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
            tileGridVideos123.ColumnDefinitions.Add(cd1);

            //2st column
            ColumnDefinition cd2 = new ColumnDefinition();
            GridLength gl2 = new GridLength(1, GridUnitType.Star);
            cd2.Width = gl2;
            tileGridVideos123.ColumnDefinitions.Add(cd2);

            //// 3rd column
            //ColumnDefinition cd3 = new ColumnDefinition();
            //GridLength gl3 = new GridLength(0.75, GridUnitType.Star);
            //cd3.Width = gl3;
            //tileGrid.ColumnDefinitions.Add(cd3);

            ////4rd column
            //ColumnDefinition cd4 = new ColumnDefinition();
            //GridLength gl4 = new GridLength(0.75, GridUnitType.Star);
            //cd4.Width = gl4;
            //tileGrid.ColumnDefinitions.Add(cd4);
        }
        public void ConstructTileGrid2(List<VideoPlayerHelper> deliveryInfoList)
        {
            tileGridVideos1.Children.Clear();
            tileGridVideos1.RowDefinitions.Clear();
            tileGridVideos1.ColumnDefinitions.Clear();

            if (orientation == DisplayOrientations.PortraitFlipped || orientation == DisplayOrientations.Portrait)
            {
                AddColumnsToTileGridPortrait2();

            }
            else if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
            {

                AddColumnsToTileGridLandscape2();

            }
            else
            {
                AddColumnsToTileGridLandscape();

            }

            int row = 0;
            int col = -1;
            //1st row
            RowDefinition rd1 = new RowDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            rd1.Height = gl1;
            tileGridVideos1.RowDefinitions.Add(rd1);


            for (int i = 0; i < deliveryInfoList.Count; i++)
            {
                Grid childGrid = null;
                childGrid = new Grid();
                //event
                childGrid.Background = new SolidColorBrush(Colors.White);
                childGrid.BorderThickness = new Thickness(1);
                childGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
                childGrid.Tapped += ChildGrid1_Tapped;
                childGrid.Margin = new Thickness(10, 10, 10, 10);

                //row1
                RowDefinition childGridRow1 = new RowDefinition();
                GridLength cgl1 = new GridLength(0.3, GridUnitType.Star);
                childGridRow1.Height = cgl1;
                childGrid.RowDefinitions.Add(childGridRow1);
                //row2
                RowDefinition childGridRow2 = new RowDefinition();
                GridLength cgl2 = new GridLength(1, GridUnitType.Star);
                childGridRow2.Height = cgl2;

                childGrid.RowDefinitions.Add(childGridRow2);
                // < Image  Grid.Row = "0" Source = "{Binding DeliveryUrl}" Stretch = "Fill" ></ Image >
                Image img = new Image();
                var videoID = deliveryInfoList[i].VideoID;
                if (ActiveItemHeaderName == "All")
                {
                    switch (videoID)
                    {
                        case 1:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_PregnancyPlanner.jpg", UriKind.Absolute));//done
                            break;
                        case 2:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_MeetDrSasaki.jpg", UriKind.Absolute));//done
                            break;
                        case 3:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_MeetDrSeetal.jpg", UriKind.Absolute));//done
                            break;
                        case 4:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_HowOldIsTooOld.jpg", UriKind.Absolute));//done
                            break;
                        case 5:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_Mommyrexia.jpg", UriKind.Absolute));//done
                            break;
                        case 6:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_FluVaccinations.jpg", UriKind.Absolute));//done
                            break;
                        case 7:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_OpioidPainkillers.jpg", UriKind.Absolute));//done
                            break;
                        case 8:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_HighRiskPregnancy.jpg", UriKind.Absolute));//done
                            break;
                        case 9:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_MeetDrTherese.jpg", UriKind.Absolute));//done
                            break;
                        case 10:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_KnowAboutBreastfeeding.jpg", UriKind.Absolute));//done
                            break;
                        case 11:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_Swaddle.jpg", UriKind.Absolute));//done
                            break;
                        case 12:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_BabysFirstBath.jpg", UriKind.Absolute));//done
                            break;
                        case 13:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_BathingTips.jpg", UriKind.Absolute));//done
                            break;
                        case 14:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_BreastfeedingBenefits.jpg", UriKind.Absolute));//done
                            break;


                        case 15:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_DiaperRash.jpg", UriKind.Absolute));//done
                            break;
                        case 16:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_PickAPediatrician.jpg", UriKind.Absolute));//done
                            break;
                        case 17:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_InfantCheckup.jpg", UriKind.Absolute));//done
                            break;

                        case 18:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_CommonNewParentQuestions.jpg", UriKind.Absolute));//done
                            break;
                    }
                }
                else if (ActiveItemHeaderName == "General")
                {
                    switch (videoID)
                    {
                        case 1:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_PregnancyPlanner.jpg", UriKind.Absolute));//done
                            break;
                        case 2:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_MeetDrSasaki.jpg", UriKind.Absolute));//done
                            break;
                        case 3:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_MeetDrSeetal.jpg", UriKind.Absolute));//done
                            break;
                        case 4:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_HowOldIsTooOld.jpg", UriKind.Absolute));//done
                            break;
                        case 5:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_Mommyrexia.jpg", UriKind.Absolute));//done
                            break;
                    }
                }
                img.Stretch = Stretch.Fill;
                img.Margin = new Thickness(0, 12, 0, 0);
                Grid.SetRow(img, 1);
                childGrid.Children.Add(img);
                //  < TextBlock Grid.Row = "1" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "20"   Foreground = "#DF6C3F"  Text = "{Binding DeliveryTitle}" Margin = "10,0,0,0" ></ TextBlock >

                TextBlock tb1 = new TextBlock();
                tb1.Name = "TileName";
                tb1.Text = deliveryInfoList[i].VideoHeader;
                tb1.TextTrimming = TextTrimming.WordEllipsis;
                tb1.FontSize = 20;
                tb1.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
                tb1.FontWeight = FontWeights.SemiLight;
                tb1.Margin = new Thickness(10, 0, 0, 0);



                TextBlock hidenTb = new TextBlock();
                hidenTb.Name = "TileName";
                hidenTb.Text = deliveryInfoList[i].VideoID.ToString();
                hidenTb.Visibility = Visibility.Collapsed;
                hidenTb.TextTrimming = TextTrimming.WordEllipsis;
                hidenTb.FontSize = 20;
                hidenTb.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
                hidenTb.FontWeight = FontWeights.SemiLight;
                hidenTb.Margin = new Thickness(10, 0, 0, 0);

                Grid.SetRow(hidenTb, 0);
                childGrid.Children.Add(hidenTb);

                Grid.SetRow(tb1, 0);
                childGrid.Children.Add(tb1);


                if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
                {
                    if ((i + 1) > 2 * (row + 1))
                    {
                        RowDefinition rd = new RowDefinition();
                        GridLength gl = new GridLength(1, GridUnitType.Star);
                        rd.Height = gl;
                        tileGridVideos1.RowDefinitions.Add(rd);

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
                        tileGridVideos1.RowDefinitions.Add(rd);

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
                        tileGridVideos1.RowDefinitions.Add(rd);

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
                tileGridVideos1.Children.Add(childGrid);
            }
        }
        public void ConstructTileGrid3(List<VideoPlayerHelper> deliveryInfoList)
        {
            tileGridVideos2.Children.Clear();
            tileGridVideos2.RowDefinitions.Clear();
            tileGridVideos2.ColumnDefinitions.Clear();

            if (orientation == DisplayOrientations.PortraitFlipped || orientation == DisplayOrientations.Portrait)
            {
                AddColumnsToTileGridPortraitpre();

            }
            else if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
            {

                AddColumnsToTileGridLandscape3();

            }
            else
            {
                AddColumnsToTileGridLandscape();

            }

            int row = 0;
            int col = -1;
            //1st row
            RowDefinition rd1 = new RowDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            rd1.Height = gl1;
            tileGridVideos2.RowDefinitions.Add(rd1);


            for (int i = 0; i < deliveryInfoList.Count; i++)
            {
                Grid childGrid = null;
                childGrid = new Grid();
                //event
                childGrid.Background = new SolidColorBrush(Colors.White);
                childGrid.BorderThickness = new Thickness(1);
                childGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
                childGrid.Tapped += ChildGrid12_Tapped;
                childGrid.Margin = new Thickness(10, 10, 10, 10);

                //row1
                RowDefinition childGridRow1 = new RowDefinition();
                GridLength cgl1 = new GridLength(0.3, GridUnitType.Star);
                childGridRow1.Height = cgl1;
                childGrid.RowDefinitions.Add(childGridRow1);
                //row2
                RowDefinition childGridRow2 = new RowDefinition();
                GridLength cgl2 = new GridLength(1, GridUnitType.Star);
                childGridRow2.Height = cgl2;

                childGrid.RowDefinitions.Add(childGridRow2);
                // < Image  Grid.Row = "0" Source = "{Binding DeliveryUrl}" Stretch = "Fill" ></ Image >
                Image img = new Image();
                var videoID = deliveryInfoList[i].VideoID;
                if (ActiveItemHeaderName == "All")
                {
                    switch (videoID)
                    {
                        case 1:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_PregnancyPlanner.jpg", UriKind.Absolute));//done
                            break;
                        case 2:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_MeetDrSasaki.jpg", UriKind.Absolute));//done
                            break;
                        case 3:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_MeetDrSeetal.jpg", UriKind.Absolute));//done
                            break;
                        case 4:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_HowOldIsTooOld.jpg", UriKind.Absolute));//done
                            break;
                        case 5:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_Mommyrexia.jpg", UriKind.Absolute));//done
                            break;
                        case 6:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_FluVaccinations.jpg", UriKind.Absolute));//done
                            break;
                        case 7:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_OpioidPainkillers.jpg", UriKind.Absolute));//done
                            break;
                        case 8:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_HighRiskPregnancy.jpg", UriKind.Absolute));//done
                            break;
                        case 9:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_MeetDrTherese.jpg", UriKind.Absolute));//done
                            break;
                        case 10:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_KnowAboutBreastfeeding.jpg", UriKind.Absolute));//done
                            break;
                        case 11:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_Swaddle.jpg", UriKind.Absolute));//done
                            break;
                        case 12:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_BabysFirstBath.jpg", UriKind.Absolute));//done
                            break;
                        case 13:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_BathingTips.jpg", UriKind.Absolute));//done
                            break;
                        case 14:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_BreastfeedingBenefits.jpg", UriKind.Absolute));//done
                            break;


                        case 15:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_DiaperRash.jpg", UriKind.Absolute));//done
                            break;
                        case 16:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_PickAPediatrician.jpg", UriKind.Absolute));//done
                            break;
                        case 17:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_InfantCheckup.jpg", UriKind.Absolute));//done
                            break;

                        case 18:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_CommonNewParentQuestions.jpg", UriKind.Absolute));//done
                            break;
                    }
                }
                else if (ActiveItemHeaderName == "General")
                {
                    switch (videoID)
                    {
                        case 1:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_PregnancyPlanner.jpg", UriKind.Absolute));//done
                            break;
                        case 2:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_MeetDrSasaki.jpg", UriKind.Absolute));//done
                            break;
                        case 3:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_MeetDrSeetal.jpg", UriKind.Absolute));//done
                            break;
                        case 4:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_HowOldIsTooOld.jpg", UriKind.Absolute));//done
                            break;
                        case 5:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_Mommyrexia.jpg", UriKind.Absolute));//done
                            break;
                    }
                }
                else if (ActiveItemHeaderName == "Pre Delivery")
                {
                    switch (videoID)
                    {
                        case 1:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_FluVaccinations.jpg", UriKind.Absolute));//done
                            break;
                        case 2:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_OpioidPainkillers.jpg", UriKind.Absolute));//done
                            break;
                        case 3:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_HighRiskPregnancy.jpg", UriKind.Absolute));//done
                            break;
                        case 4:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_MeetDrTherese.jpg", UriKind.Absolute));//done
                            break;
                    }
                }
                img.Stretch = Stretch.Fill;
                img.Margin = new Thickness(0, 0, 0, 0);
                Grid.SetRow(img, 1);
                childGrid.Children.Add(img);
                //  < TextBlock Grid.Row = "1" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "20"   Foreground = "#DF6C3F"  Text = "{Binding DeliveryTitle}" Margin = "10,0,0,0" ></ TextBlock >

                TextBlock tb1 = new TextBlock();
                tb1.Name = "TileName";
                tb1.Text = deliveryInfoList[i].VideoHeader;
                tb1.TextTrimming = TextTrimming.WordEllipsis;
                tb1.FontSize = 20;
                tb1.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
                tb1.FontWeight = FontWeights.SemiLight;
                tb1.Margin = new Thickness(10, 0, 0, 0);



                TextBlock hidenTb = new TextBlock();
                hidenTb.Name = "TileName";
                hidenTb.Text = deliveryInfoList[i].VideoID.ToString();
                hidenTb.Visibility = Visibility.Collapsed;
                hidenTb.TextTrimming = TextTrimming.WordEllipsis;
                hidenTb.FontSize = 20;
                hidenTb.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
                hidenTb.FontWeight = FontWeights.SemiLight;
                hidenTb.Margin = new Thickness(10, 0, 0, 0);

                Grid.SetRow(hidenTb, 0);
                childGrid.Children.Add(hidenTb);

                Grid.SetRow(tb1, 0);
                childGrid.Children.Add(tb1);
                if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
                {
                    if ((i + 1) > 2 * (row + 1))
                    {
                        RowDefinition rd = new RowDefinition();
                        GridLength gl = new GridLength(1, GridUnitType.Star);
                        rd.Height = gl;
                        tileGridVideos2.RowDefinitions.Add(rd);

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
                        tileGridVideos2.RowDefinitions.Add(rd);

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
                        tileGridVideos2.RowDefinitions.Add(rd);

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
                tileGridVideos2.Children.Add(childGrid);
            }
        }
        public void AddColumnsToTileGridPortraitpre()
        {

            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            tileGridVideos3.ColumnDefinitions.Add(cd1);


        }
        public void AddColumnsToTileGridLandscape3()
        {

            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            tileGridVideos2.ColumnDefinitions.Add(cd1);

            //2st column
            ColumnDefinition cd2 = new ColumnDefinition();
            GridLength gl2 = new GridLength(1, GridUnitType.Star);
            cd2.Width = gl2;
            tileGridVideos2.ColumnDefinitions.Add(cd2);

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
        public void AddColumnsToTileGridPortrait2()
        {

            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            tileGridVideos1.ColumnDefinitions.Add(cd1);


        }
        public void ConstructTileGrid4(List<VideoPlayerHelper> deliveryInfoList)
        {
            tileGridVideos3.Children.Clear();
            tileGridVideos3.RowDefinitions.Clear();
            tileGridVideos3.ColumnDefinitions.Clear();

            if (orientation == DisplayOrientations.PortraitFlipped || orientation == DisplayOrientations.Portrait)
            {
                AddColumnsToTileGridPortrait();

            }
            else if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
            {

                AddColumnsToTileGridLandscape4();

            }
            else
            {
                AddColumnsToTileGridLandscape();

            }

            int row = 0;
            int col = -1;
            //1st row
            RowDefinition rd1 = new RowDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            rd1.Height = gl1;
            tileGridVideos3.RowDefinitions.Add(rd1);


            for (int i = 0; i < deliveryInfoList.Count; i++)
            {
                Grid childGrid = null;
                childGrid = new Grid();
                //event
                childGrid.Background = new SolidColorBrush(Colors.White);
                childGrid.BorderThickness = new Thickness(1);
                childGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
                childGrid.Tapped += ChildGrid13_Tapped;
                childGrid.Margin = new Thickness(10, 10, 10, 10);

                //row1
                RowDefinition childGridRow1 = new RowDefinition();
                GridLength cgl1 = new GridLength(0.3, GridUnitType.Star);
                childGridRow1.Height = cgl1;
                childGrid.RowDefinitions.Add(childGridRow1);
                //row2
                RowDefinition childGridRow2 = new RowDefinition();
                GridLength cgl2 = new GridLength(1, GridUnitType.Star);
                childGridRow2.Height = cgl2;

                childGrid.RowDefinitions.Add(childGridRow2);
                // < Image  Grid.Row = "0" Source = "{Binding DeliveryUrl}" Stretch = "Fill" ></ Image >
                Image img = new Image();
                var videoID = deliveryInfoList[i].VideoID;
                if (ActiveItemHeaderName == "All")
                {
                    switch (videoID)
                    {
                        case 1:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_PregnancyPlanner.jpg", UriKind.Absolute));//done
                            break;
                        case 2:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_MeetDrSasaki.jpg", UriKind.Absolute));//done
                            break;
                        case 3:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_MeetDrSeetal.jpg", UriKind.Absolute));//done
                            break;
                        case 4:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_HowOldIsTooOld.jpg", UriKind.Absolute));//done
                            break;
                        case 5:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_Mommyrexia.jpg", UriKind.Absolute));//done
                            break;
                        case 6:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_FluVaccinations.jpg", UriKind.Absolute));//done
                            break;
                        case 7:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_OpioidPainkillers.jpg", UriKind.Absolute));//done
                            break;
                        case 8:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_HighRiskPregnancy.jpg", UriKind.Absolute));//done
                            break;
                        case 9:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_MeetDrTherese.jpg", UriKind.Absolute));//done
                            break;
                        case 10:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_KnowAboutBreastfeeding.jpg", UriKind.Absolute));//done
                            break;
                        case 11:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_Swaddle.jpg", UriKind.Absolute));//done
                            break;
                        case 12:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_BabysFirstBath.jpg", UriKind.Absolute));//done
                            break;
                        case 13:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_BathingTips.jpg", UriKind.Absolute));//done
                            break;
                        case 14:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_BreastfeedingBenefits.jpg", UriKind.Absolute));//done
                            break;


                        case 15:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_DiaperRash.jpg", UriKind.Absolute));//done
                            break;
                        case 16:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_PickAPediatrician.jpg", UriKind.Absolute));//done
                            break;
                        case 17:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_InfantCheckup.jpg", UriKind.Absolute));//done
                            break;

                        case 18:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_CommonNewParentQuestions.jpg", UriKind.Absolute));//done
                            break;
                    }
                }
                else if (ActiveItemHeaderName == "General")
                {
                    switch (videoID)
                    {
                        case 1:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_PregnancyPlanner.jpg", UriKind.Absolute));//done
                            break;
                        case 2:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_MeetDrSasaki.jpg", UriKind.Absolute));//done
                            break;
                        case 3:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_MeetDrSeetal.jpg", UriKind.Absolute));//done
                            break;
                        case 4:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_HowOldIsTooOld.jpg", UriKind.Absolute));//done
                            break;
                        case 5:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_Mommyrexia.jpg", UriKind.Absolute));//done
                            break;
                    }
                }
                else if (ActiveItemHeaderName == "Pre Delivery")
                {
                    switch (videoID)
                    {
                        case 1:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_FluVaccinations.jpg", UriKind.Absolute));//done
                            break;
                        case 2:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_OpioidPainkillers.jpg", UriKind.Absolute));//done
                            break;
                        case 3:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_HighRiskPregnancy.jpg", UriKind.Absolute));//done
                            break;
                        case 4:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_MeetDrTherese.jpg", UriKind.Absolute));//done
                            break;
                    }
                }
                else if (ActiveItemHeaderName == "Post Delivery")
                {
                    switch (videoID)
                    {

                        case 1:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_KnowAboutBreastfeeding.jpg", UriKind.Absolute));//done
                            break;
                        case 2:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_Swaddle.jpg", UriKind.Absolute));//done
                            break;
                        case 3:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_BabysFirstBath.jpg", UriKind.Absolute));//done
                            break;
                        case 4:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_BathingTips.jpg", UriKind.Absolute));//done
                            break;
                        case 5:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_BreastfeedingBenefits.jpg", UriKind.Absolute));//done
                            break;
                        case 6:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_DiaperRash.jpg", UriKind.Absolute));//done
                            break;
                        case 7:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_PickAPediatrician.jpg", UriKind.Absolute));//done
                            break;
                        case 8:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_InfantCheckup.jpg", UriKind.Absolute));//done
                            break;
                        case 9:
                            img.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_CommonNewParentQuestions.jpg", UriKind.Absolute));//done
                            break;
                    }
                }
                img.Stretch = Stretch.Fill;
                img.Margin = new Thickness(0, 12, 0, 0);
                Grid.SetRow(img, 1);
                childGrid.Children.Add(img);
                //  < TextBlock Grid.Row = "1" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "20"   Foreground = "#DF6C3F"  Text = "{Binding DeliveryTitle}" Margin = "10,0,0,0" ></ TextBlock >

                TextBlock tb1 = new TextBlock();
                tb1.Name = "TileName";
                tb1.Text = deliveryInfoList[i].VideoHeader;
                tb1.TextTrimming = TextTrimming.WordEllipsis;
                tb1.FontSize = 20;
                tb1.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
                tb1.FontWeight = FontWeights.SemiLight;
                tb1.Margin = new Thickness(10, 0, 0, 0);



                TextBlock hidenTb = new TextBlock();
                hidenTb.Name = "TileName";
                hidenTb.Text = deliveryInfoList[i].VideoID.ToString();
                hidenTb.Visibility = Visibility.Collapsed;
                hidenTb.TextTrimming = TextTrimming.WordEllipsis;
                hidenTb.FontSize = 20;
                hidenTb.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
                hidenTb.FontWeight = FontWeights.SemiLight;
                hidenTb.Margin = new Thickness(10, 0, 0, 0);

                Grid.SetRow(hidenTb, 0);
                childGrid.Children.Add(hidenTb);

                Grid.SetRow(tb1, 0);
                childGrid.Children.Add(tb1);
                if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
                {
                    if ((i + 1) > 2 * (row + 1))
                    {
                        RowDefinition rd = new RowDefinition();
                        GridLength gl = new GridLength(1, GridUnitType.Star);
                        rd.Height = gl;
                        tileGridVideos3.RowDefinitions.Add(rd);

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
                        tileGridVideos3.RowDefinitions.Add(rd);

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
                        tileGridVideos3.RowDefinitions.Add(rd);

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
                tileGridVideos3.Children.Add(childGrid);
            }
        }
        public void AddColumnsToTileGridLandscape4()
        {

            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            tileGridVideos3.ColumnDefinitions.Add(cd1);

            //2st column
            ColumnDefinition cd2 = new ColumnDefinition();
            GridLength gl2 = new GridLength(1, GridUnitType.Star);
            cd2.Width = gl2;
            tileGridVideos3.ColumnDefinitions.Add(cd2);

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
        public class navigationvideos
        {
            public string videosource;
            public string source;
        }
    }
}



