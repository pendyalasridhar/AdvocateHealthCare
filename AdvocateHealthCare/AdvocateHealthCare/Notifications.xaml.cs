using MyToolkit.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Text;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class Notifications : Page
    {
        ListView objListView = new ListView();
        JArray jArr;
        static NotificationDetails objNotificationDetails = new NotificationDetails();
        static List<NotificationDetails> objListNotificationDetails = new List<NotificationDetails>();
        public Notifications()
        {
            this.InitializeComponent();
            this.Loaded += Notifications_Loaded;
            txtNotificationCount.Text = HomePage.unreadNotificationCount.ToString();
        }

        ////All notifications details are assigned to below properties in Notifications_Loaded event handler.
        public class NotificationDetails
        {
            public string NotificationData { get; set; }
            public string NotificationText { get; set; }
            public string NotificationTitle { get; set; }
            public string TypeOfNotification { get; set; }
            public string ReadStatus { get; set; }
            public BitmapImage Mailimage { get; set; }
            public BitmapImage BindImage { get; set; }
            public SolidColorBrush FontColor { get; set; }
        }
        //Below Methods is called when page  loads
        private async void Notifications_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.IsInternet() == true)
            {
                try
                {
                    objListNotificationDetails = new List<NotificationDetails>();
                    //string getAllNotifications = App.BASE_URL + "/api/Notifications/GetNotifications?UserId=3&HospitalId=1";
                    string getAllNotifications = App.BASE_URL + "/api/Notifications/GetNotifications?UserId=" + App.userId + "&HospitalId=" + App.hospitalId;
                    var client = new HttpClient();
                    HttpResponseMessage response = await client.GetAsync(new Uri(getAllNotifications));
                    string jsonString = await response.Content.ReadAsStringAsync();
                    jArr = JArray.Parse(jsonString);

                    for (int x = 0; x < jArr.Count; x++)
                    {
                        objNotificationDetails = new NotificationDetails();
                        var HospitalID = (string)jArr[x]["HospitalID"];
                        objNotificationDetails.NotificationData = (string)jArr[x]["NotificationDate"];
                        string[] split = (objNotificationDetails.NotificationData).Split(' ');
                        objNotificationDetails.NotificationData = split[0];
                        objNotificationDetails.NotificationText = (string)jArr[x]["NotificationText"];
                        objNotificationDetails.NotificationTitle = (string)jArr[x]["NotificationTitle"];
                        objNotificationDetails.TypeOfNotification = (string)jArr[x]["TypeOfNotification"];
                        objNotificationDetails.ReadStatus = (string)jArr[x]["IsRead"];


                        bool readStatus = Convert.ToBoolean(objNotificationDetails.ReadStatus);
                        switch (readStatus)
                        {
                            case true:
                                objNotificationDetails.Mailimage = new BitmapImage(new Uri(@"ms-appx:/Assets/read.png", UriKind.Absolute));
                                objNotificationDetails.FontColor = new SolidColorBrush(Colors.Gray);
                                break;
                            case false:
                                objNotificationDetails.Mailimage = new BitmapImage(new Uri(@"ms-appx:/Assets/unread.png", UriKind.Absolute));
                                break;
                            default:
                                objNotificationDetails.Mailimage = new BitmapImage(new Uri(@"ms-appx:/Assets/unread.png", UriKind.Absolute));
                                break;
                        }


                        objListNotificationDetails.Add(objNotificationDetails);

                    }
                    grdNotifications.ItemsSource = objListNotificationDetails;
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

        private void NotificationsAllRead_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            List<NotificationDetails> objListUnread = new List<NotificationDetails>();
            try
            {
                grdNotificationsUnread.ItemsSource = null;
                if (jArr != null)
                {
                    for (int a = 0; a < objListNotificationDetails.Count; a++)
                    {
                        objNotificationDetails = new NotificationDetails();
                        objNotificationDetails.NotificationData = (string)jArr[a]["NotificationDate"];
                        string[] split = (objNotificationDetails.NotificationData).Split(' ');
                        objNotificationDetails.NotificationData = split[0];
                        objNotificationDetails.NotificationText = (string)jArr[a]["NotificationText"];
                        objNotificationDetails.NotificationTitle = (string)jArr[a]["NotificationTitle"];
                        objNotificationDetails.TypeOfNotification = (string)jArr[a]["TypeOfNotification"];
                        objNotificationDetails.ReadStatus = (string)jArr[a]["IsRead"];
                        if (objNotificationDetails.ReadStatus == "False")
                        {
                            objNotificationDetails.Mailimage = new BitmapImage(new Uri(@"ms-appx:/Assets/unread.png", UriKind.Absolute));


                            objListUnread.Add(objNotificationDetails);
                        }
                        else
                        {
                        }

                    }
                    objListNotificationDetails = objListUnread;
                    grdNotificationsUnread.ItemsSource = objListNotificationDetails;
                }
            }

            catch (Exception ex)
            {

                throw;
            }
        }


        private void SearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            this.Frame.Navigate(typeof(SearchPage), args.QueryText);
        }

        private void grdNotifications_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //this.Frame.Navigate(typeof(NotificationDetailsPage));

        }

        private void Notificationgridtapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Notifications));
        }

        private void mySearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            this.Frame.Navigate(typeof(SearchPage), args.QueryText);
        }
    }
}
