using MyToolkit.Multimedia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.ComponentModel;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
namespace AdvocateHealthCare
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlayVideo : Page
    {
        int VideoId;
        string videoSource;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {//play videos from homepage
                //string DoctorVideo = e.Parameter.ToString();
                AdvocateHealthCare.HomePage.navigationvideos obj = (AdvocateHealthCare.HomePage.navigationvideos)e.Parameter;
                videoSource = obj.source;
                PlayYoutubeVideo(obj.videosource.ToString());
            }
            else
            {
                //plays videos from videos page
                PlayVideoFromVideoPage();
            }
        }
        public PlayVideo()
        {
            this.InitializeComponent();
            //}
            //PlayYoutubeVideo(VideoId);
        }
        public void PlayVideoFromVideoPage()
        {
            VideoId = (App.Current as App).NavigateText;
            switch (VideoId)
            {
                case 1:
                    PlayYoutubeVideo("wLWQ9XwlKao");
                    break;
                case 2:
                    PlayYoutubeVideo("bcLPRubq5cM");
                    break;
                case 3:
                    PlayYoutubeVideo("L1sFlTsq34k");
                    break;
                case 4:
                    PlayYoutubeVideo("B8dp85rnAQg");
                    break;
                case 5:
                    PlayYoutubeVideo("wWIgBJRtuJ0");
                    break;
                case 6:
                    PlayYoutubeVideo("DA51I8PuiHw");
                    break;
                case 7:
                    PlayYoutubeVideo("wNexVdG4EwM");
                    break;
                case 8:
                    PlayYoutubeVideo("BJWjbhnOLbE");
                    break;
                case 9:
                    PlayYoutubeVideo("lJOVYaXvH2c");
                    break;
                case 10:
                    PlayYoutubeVideo("qucsu1iSM4c");
                    break;
                case 11:
                    PlayYoutubeVideo("6fo_zG8ktIo");
                    break;
                case 12:
                    PlayYoutubeVideo("HF1DvS9Y-7Y");
                    break;
                case 13:
                    PlayYoutubeVideo("Xyci1-x5Zt4");
                    break;
                case 14:
                    PlayYoutubeVideo("X9fvHK2ZN6s");
                    break;
                case 15:
                    PlayYoutubeVideo("8ExOVKeg120");
                    break;
                case 16:
                    PlayYoutubeVideo("vvUw2WWwqJw");
                    break;
                case 17:
                    PlayYoutubeVideo("QE9hcf2QWaY");
                    break;
                case 18:
                    PlayYoutubeVideo("k1cxL22IEUc");
                    break;

            }
        }
        DispatcherTimer dispatcherTimer;
        public async void PlayYoutubeVideo(string _videoId)
        {
            var url = await YouTube.GetVideoUriAsync(_videoId, YouTubeQuality.Quality1080P);
            var YoutubePlayer = new MediaElement();
            mediaYoutube.Source = url.Uri;
            mediaYoutube.Play();
            mediaYoutube.Volume = 40;
            mediaYoutube.MediaOpened += new RoutedEventHandler(MediaYoutube_MediaOpened);

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            //TimerStatus.Text = "dispatcherTimer.IsEnabled = " + dispatcherTimer.IsEnabled + "\n";
            //System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            //dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            //dispatcherTimer.Interval = new TimeSpan(0, 5, 0);
            //dispatcherTimer.Start();
        }
        int time;
        private void MediaYoutube_MediaOpened(object sender, RoutedEventArgs e)
        {
            time = (int)mediaYoutube.NaturalDuration.TimeSpan.TotalSeconds;
            double inMinutes = time / 60;
        }
        //shows minutes and seconds count down for videos
        void dispatcherTimer_Tick(object sender, object e)
        {
            if (time > 0)
            {
                time--;
                int seconds = time % 60;
                int minutes = time / 60;
                bool cond = true;
                if (seconds < 10)
                //minutes = Convert.ToUInt16( "0" + minutes);
                {
                    // seconds = Convert.ToInt16("0") + seconds;
                    txtCoutDown.Text = "Video Duration Left: " + minutes + ":0" + seconds;
                    cond = false;
                }
                if (cond)
                    txtCoutDown.Text = "Video Duration Left: " + minutes + ":" + seconds;
            }
        }
        public void PlayVideoFromPreviuosPage(Uri VideoUri)
        {
            mediaYoutube.Source = VideoUri;
        }
        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            mediaYoutube.Pause();
            imgPause.Visibility = Visibility.Visible;
            dispatcherTimer.Stop();
        }
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            mediaYoutube.Stop();
            dispatcherTimer.Stop();
        }
        private void btnPLay_Click(object sender, RoutedEventArgs e)
        {
            mediaYoutube.Play();
            imgPause.Visibility = Visibility.Collapsed;
            dispatcherTimer.Start();
        }
        private void BackNav_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (videoSource == "Home")
            {
                this.Frame.Navigate(typeof(HomePage));
            }
            else
            {
                this.Frame.Navigate(typeof(VideosPage));
            }
            rngProgress.Visibility = Visibility.Collapsed;
        }

    }
}
