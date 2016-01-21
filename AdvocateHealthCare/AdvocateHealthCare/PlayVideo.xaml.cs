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
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                //string DoctorVideo = e.Parameter.ToString();
                PlayYoutubeVideo(e.Parameter.ToString());
            }
            else
            {
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
                    PlayYoutubeVideo("d9uAa9lECcA");
                    break;
                case 2:
                    PlayYoutubeVideo("CDJ7IebMo2A");
                    break;
                case 3:
                    PlayYoutubeVideo("Xb8aVX6nA88");
                    break;
                case 4:
                    PlayYoutubeVideo("U6118JszdCU");
                    break;
                case 5:
                    PlayYoutubeVideo("Xb8aVX6nA88");
                    break;
                case 6:
                    PlayYoutubeVideo("sjqfDru825I");
                    break;
                case 7:
                    PlayYoutubeVideo("usrh-1bnXgE");
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
            this.Frame.Navigate(typeof(VideosPage));
        }
    }
}
