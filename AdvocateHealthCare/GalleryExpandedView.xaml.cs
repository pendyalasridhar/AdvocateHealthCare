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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AdvocateHealthCare
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GalleryExpandedView : Page
    {
        public GalleryExpandedView()
        {
            this.InitializeComponent();
            txtNotificationCount.Text = HomePage.unreadNotificationCount.ToString();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            List<string> data = new List<string>();
            data = (List<string>)e.Parameter;
            txtDate.Text = data[0];

            Uri uri = new Uri(data[1]);
            //BitmapImage bmp = new BitmapImage();
            //bmp.SetSource(uri)

            imgUrl.Source = new BitmapImage(new Uri(uri.ToString(), UriKind.Absolute));
        }

        private void mySearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            this.Frame.Navigate(typeof(SearchPage), args.QueryText);
        }

        private void Canvas_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Notifications));
        }

        private void BackNav_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GalleryPage));
        }
    }
}
