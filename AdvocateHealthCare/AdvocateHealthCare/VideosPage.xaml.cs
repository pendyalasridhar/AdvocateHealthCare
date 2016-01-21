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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VideosPage : Page
    {
        public VideosPage()
        {
            this.InitializeComponent();
            this.Loaded += VideosPage_Loaded;
            txtNotificationCount.Text = HomePage.unreadNotificationCount.ToString();

        }
        public class VideoPlayerHelper
        {
            public string VideoHeader { get; set; }
            public Uri VideoUrl { get; set; }
            public BitmapImage LocalImagePath { get; set; }
            public int VideoID { get; set; }

        }

        private async void VideosPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<VideoPlayerHelper> objListVideoPlayer = new List<VideoPlayerHelper>();
                string GetVideos = App.BASE_URL + "/api/JournalVideos/GetJournalVideos";
                var client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(new Uri(GetVideos));
                var jsonString = await response.Content.ReadAsStringAsync();
                JArray jArr = JArray.Parse(jsonString);
                foreach (var item in jArr)
                {
                    VideoPlayerHelper objVideoPlayerHelper = new VideoPlayerHelper();
                    objVideoPlayerHelper.VideoID = (int)item["$id"];

                    string VideoID = objVideoPlayerHelper.VideoID.ToString();
                    switch (VideoID)
                    {
                        case "1":
                            objVideoPlayerHelper.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/video_thumb1.png", UriKind.Absolute));
                            break;
                        case "2":
                            objVideoPlayerHelper.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/video_thumb2.png", UriKind.Absolute));
                            break;
                        case "3":
                            objVideoPlayerHelper.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/video_thumb3.png", UriKind.Absolute));
                            break;
                        case "4":
                            objVideoPlayerHelper.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/video_thumb4.png", UriKind.Absolute));
                            break;
                        case "5":
                            objVideoPlayerHelper.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/video_thumb5.png", UriKind.Absolute));
                            break;
                        case "6":
                            objVideoPlayerHelper.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/video_thumb6.png", UriKind.Absolute));
                            break;
                        case "7":
                            objVideoPlayerHelper.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/video_thumb1.png", UriKind.Absolute));
                            break;
                        case "9":
                            objVideoPlayerHelper.LocalImagePath = new BitmapImage(new Uri(@"ms-appx:/Assets/video_thumb3.png", UriKind.Absolute));
                            break;

                    }
                    objVideoPlayerHelper.VideoHeader = (string)item["JournalVideoName"];

                    string x = App.BASE_URL + item["JournalVideoAsset"];
                    Uri uri = new Uri(x);
                    objVideoPlayerHelper.VideoUrl = uri;
                    objListVideoPlayer.Add(objVideoPlayerHelper);
                }
                gridVideosDisplay.ItemsSource = objListVideoPlayer;
            }
            catch (Exception)
            {
                MessageDialog msgDialog = new MessageDialog("The required resources are not downloaded.Please check your internet connectivity. If the problem persists, please contact advocate healthcare customer care associate.", "Message");
                msgDialog.ShowAsync();
            }

        }

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
    }
}
