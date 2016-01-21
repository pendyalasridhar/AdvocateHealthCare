using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
        public VideosPage()
        {
            this.InitializeComponent();
            //this.Loaded += VideosPage_Loaded;
            txtNotificationCount.Text = HomePage.unreadNotificationCount.ToString();

        }
        public class VideoPlayerHelper
        {
            public string VideoHeader { get; set; }
            public Uri VideoUrl { get; set; }
            public BitmapImage LocalImagePath { get; set; }
            public BitmapImage LocalImagePath2 { get; set; }
            public int VideoID { get; set; }

        }

        //private async void VideosPage_Loaded(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        List
//<VideoPlayerHelper>objListVideoPlayer = new List
//    <VideoPlayerHelper>();
        //        string GetVideos = App.BASE_URL + "/api/JournalVideos/GetJournalVideos";
        //        var client = new HttpClient();
        //        HttpResponseMessage response = await client.GetAsync(new Uri(GetVideos));
        //        var jsonString = await response.Content.ReadAsStringAsync();
        //        JArray jArr = JArray.Parse(jsonString);
        //        foreach (var item in jArr)
        //        {
        //            VideoPlayerHelper objVideoPlayerHelper = new VideoPlayerHelper();
        //            objVideoPlayerHelper.VideoID = (int)item["$id"];

        //            string VideoID = objVideoPlayerHelper.VideoID.ToString();
        //            switch (VideoID)
        //            {
        //                case "1":
        //                    objVideoPlayerHelper.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/video_thumb1.png", UriKind.Absolute));
        //                    break;
        //                case "2":
        //                    objVideoPlayerHelper.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/video_thumb2.png", UriKind.Absolute));
        //                    break;
        //                case "3":
        //                    objVideoPlayerHelper.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/video_thumb3.png", UriKind.Absolute));
        //                    break;
        //                case "4":
        //                    objVideoPlayerHelper.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/video_thumb4.png", UriKind.Absolute));
        //                    break;
        //                case "5":
        //                    objVideoPlayerHelper.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/video_thumb5.png", UriKind.Absolute));
        //                    break;
        //                case "6":
        //                    objVideoPlayerHelper.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/video_thumb6.png", UriKind.Absolute));
        //                    break;
        //                case "7":
        //                    objVideoPlayerHelper.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/video_thumb1.png", UriKind.Absolute));
        //                    break;
        //                case "8":
        //                    objVideoPlayerHelper.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/video_thumb1.png", UriKind.Absolute));
        //                    break;
        //                case "9":
        //                    objVideoPlayerHelper.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/video_thumb3.png", UriKind.Absolute));
        //                    break;
        //                case "10":
        //                    objVideoPlayerHelper.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Breastfeeding-and-Smoking.png", UriKind.Absolute));
        //                    break;
        //                case "11":
        //                    objVideoPlayerHelper.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/video_thumb2.png", UriKind.Absolute));
        //                    break;
        //                case "12":
        //                    objVideoPlayerHelper.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/img2.png", UriKind.Absolute));
        //                    break;
        //                case "13":
        //                    objVideoPlayerHelper.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/video_thumb4.png", UriKind.Absolute));
        //                    break;
        //                case "14":
        //                    objVideoPlayerHelper.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/video_thumb5.png", UriKind.Absolute));
        //                    break;
        //                case "15":
        //                    objVideoPlayerHelper.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/video_thumb6.png", UriKind.Absolute));
        //                    break;
        //                case "16":
        //                    objVideoPlayerHelper.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/video_thumb1.png", UriKind.Absolute));
        //                    break;
        //                case "17":
        //                    objVideoPlayerHelper.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Dollarphotoclub_88132471.png", UriKind.Absolute));
        //                    break;
        //                case "18":
        //                    objVideoPlayerHelper.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/video_thumb3.png", UriKind.Absolute));
        //                    break;
        //            }
        //            objVideoPlayerHelper.VideoHeader = (string)item["JournalVideoName"];

        //            string x = App.BASE_URL + item["JournalVideoAsset"];
        //            Uri uri = new Uri(x);
        //            objVideoPlayerHelper.VideoUrl = uri;
        //            objListVideoPlayer.Add(objVideoPlayerHelper);
        //        }
        //        gridVideosDisplay.ItemsSource = objListVideoPlayer;
        //    }
        //    catch (Exception)
        //    {
        //        MessageDialog msgDialog = new MessageDialog("The required resources are not downloaded. Please check your internet connectivity. If the problem persists, please contact advocate healthcare customer care associate.", "Message");
        //        msgDialog.ShowAsync();
        //    }

        //}

        private void gridVideosDisplay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VideoPlayerHelper value = (VideoPlayerHelper)(sender as GridView).SelectedItem;
            int uri = value.VideoID;
            (App.Current as App).NavigateText = uri;
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
        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
        DeliveryInformation objDeliveryInformation = new DeliveryInformation();
        VideoPlayerHelper objVideoPlayerHelper;
        string VideoID;
        public async void DeliveryInfo(string id)
        {
            if (App.IsInternet() == true)
            {
                try
                {
                    List
        <VideoPlayerHelper> objListVideoPlayer = new List
            <VideoPlayerHelper>();
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
                            VideoPlayerHelper objVideoPlayerHelper1 = new VideoPlayerHelper();
                            objVideoPlayerHelper1.VideoID = (int)item["$id"];
                            VideoID = objVideoPlayerHelper1.VideoID.ToString();
                            //VideoID = objVideoPlayerHelper.VideoID.ToString();
                            switch (VideoID)
                            {

                                case "1":
                                    objVideoPlayerHelper1.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_PregnancyPlanner.jpg", UriKind.Absolute));//done
                                    break;
                                case "2":
                                    objVideoPlayerHelper1.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_MeetDrSasaki.jpg", UriKind.Absolute));//done
                                    break;
                                case "3":
                                    objVideoPlayerHelper1.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_MeetDrSeetal.jpg", UriKind.Absolute));//done
                                    break;
                                case "4":
                                    objVideoPlayerHelper1.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_HowOldIsTooOld.jpg", UriKind.Absolute));//done
                                    break;
                                case "5":
                                    objVideoPlayerHelper1.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_Mommyrexia.jpg", UriKind.Absolute));//done
                                    break;
                            }
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

                                case "1":
                                    objVideoPlayerHelper2.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_FluVaccinations.jpg", UriKind.Absolute));//done
                                    break;
                                case "2":
                                    objVideoPlayerHelper2.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_OpioidPainkillers.jpg", UriKind.Absolute));//done
                                    break;
                                case "3":
                                    objVideoPlayerHelper2.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_HighRiskPregnancy.jpg", UriKind.Absolute));//done
                                    break;
                                case "4":
                                    objVideoPlayerHelper2.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_MeetDrTherese.jpg", UriKind.Absolute));//done
                                    break;
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

                                case "1":
                                    objVideoPlayerHelper3.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_KnowAboutBreastfeeding.jpg", UriKind.Absolute));//done
                                    break;
                                case "2":
                                    objVideoPlayerHelper3.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_Swaddle.jpg", UriKind.Absolute));//done
                                    break;
                                case "3":
                                    objVideoPlayerHelper3.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_BabysFirstBath.jpg", UriKind.Absolute));//done
                                    break;
                                case "4":
                                    objVideoPlayerHelper3.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_BathingTips.jpg", UriKind.Absolute));//done
                                    break;
                                case "5":
                                    objVideoPlayerHelper3.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_BreastfeedingBenefits.jpg", UriKind.Absolute));//done
                                    break;
                                case "6":
                                    objVideoPlayerHelper3.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_DiaperRash.jpg", UriKind.Absolute));//done
                                    break;
                                case "7":
                                    objVideoPlayerHelper3.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_PickAPediatrician.jpg", UriKind.Absolute));//done
                                    break;
                                case "8":
                                    objVideoPlayerHelper3.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_InfantCheckup.jpg", UriKind.Absolute));//done
                                    break;
                                case "9":
                                    objVideoPlayerHelper3.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_CommonNewParentQuestions.jpg", UriKind.Absolute));//done
                                    break;
                            }
                            objVideoPlayerHelper3.VideoHeader = (string)item["JournalVideoName"];
                            string x = App.BASE_URL + item["JournalVideoAsset"];
                            Uri uri = new Uri(x);
                            objVideoPlayerHelper3.VideoUrl = uri;
                            objListVideoPlayer.Add(objVideoPlayerHelper3);
                        }

                        if (ActiveItemHeaderName == "All")
                        {
                            VideoPlayerHelper objVideoPlayerHelper4 = new VideoPlayerHelper();
                            objVideoPlayerHelper4.VideoID = (int)item["$id"];
                            VideoID = objVideoPlayerHelper4.VideoID.ToString();
                            switch (VideoID)
                            {

                                case "1":
                                    objVideoPlayerHelper4.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_PregnancyPlanner.jpg", UriKind.Absolute));//done
                                    break;
                                case "2":
                                    objVideoPlayerHelper4.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_MeetDrSasaki.jpg", UriKind.Absolute));//done
                                    break;
                                case "3":
                                    objVideoPlayerHelper4.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_MeetDrSeetal.jpg", UriKind.Absolute));//done
                                    break;
                                case "4":
                                    objVideoPlayerHelper4.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_HowOldIsTooOld.jpg", UriKind.Absolute));//done
                                    break;
                                case "5":
                                    objVideoPlayerHelper4.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/General/General_Mommyrexia.jpg", UriKind.Absolute));//done
                                    break;
                                case "6":
                                    objVideoPlayerHelper4.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_FluVaccinations.jpg", UriKind.Absolute));//done
                                    break;
                                case "7":
                                    objVideoPlayerHelper4.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_OpioidPainkillers.jpg", UriKind.Absolute));//done
                                    break;
                                case "8":
                                    objVideoPlayerHelper4.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_HighRiskPregnancy.jpg", UriKind.Absolute));//done
                                    break;
                                case "9":
                                    objVideoPlayerHelper4.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Pre_Delivery/PreDelivery_MeetDrTherese.jpg", UriKind.Absolute));//done
                                    break;
                                case "10":
                                    objVideoPlayerHelper4.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_KnowAboutBreastfeeding.jpg", UriKind.Absolute));//done
                                    break;
                                case "11":
                                    objVideoPlayerHelper4.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_Swaddle.jpg", UriKind.Absolute));//done
                                    break;
                                case "12":
                                    objVideoPlayerHelper4.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_BabysFirstBath.jpg", UriKind.Absolute));//done
                                    break;
                                case "13":
                                    objVideoPlayerHelper4.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_BathingTips.jpg", UriKind.Absolute));//done
                                    break;
                                case "14":
                                    objVideoPlayerHelper4.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_BreastfeedingBenefits.jpg", UriKind.Absolute));//done
                                    break;
                                case "15":
                                    objVideoPlayerHelper4.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_DiaperRash.jpg", UriKind.Absolute));//done
                                    break;
                                case "16":
                                    objVideoPlayerHelper4.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_PickAPediatrician.jpg", UriKind.Absolute));//done
                                    break;
                                case "17":
                                    objVideoPlayerHelper4.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_InfantCheckup.jpg", UriKind.Absolute));//done
                                    break;



                                case "18":
                                    objVideoPlayerHelper4.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/Post_Delivery/PostDelivery_CommonNewParentQuestions.jpg", UriKind.Absolute));//done
                                    break;
                            }
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
                            gridVideosDisplay.ItemsSource = objListVideoPlayer;
                            break;
                        case "General":
                            gridVideosDisplay2.ItemsSource = objListVideoPlayer;
                            break;
                        case "Pre Delivery":
                            gridVideosDisplay1.ItemsSource = objListVideoPlayer;
                            break;
                        case "Post Delivery":
                            gridVideosDisplay3.ItemsSource = objListVideoPlayer;
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
            (App.Current as App).NavigateText = uri;
            this.Frame.Navigate(typeof(PlayVideo));
        }

        private void gridVideosDisplay2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VideoPlayerHelper value = (VideoPlayerHelper)(sender as GridView).SelectedItem;
            int uri = value.VideoID;
            (App.Current as App).NavigateText = uri;
            this.Frame.Navigate(typeof(PlayVideo));
        }

        private void gridVideosDisplay3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VideoPlayerHelper value = (VideoPlayerHelper)(sender as GridView).SelectedItem;
            int uri = value.VideoID;
            (App.Current as App).NavigateText = uri;
            this.Frame.Navigate(typeof(PlayVideo));
        }
    }
}


