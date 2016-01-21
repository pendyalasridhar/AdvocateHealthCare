using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
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
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AdvocateHealthCare
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 


    public sealed partial class HomePage : Page
    {
        UnreadClass objUnreadClass;
        public HomePage()
        {
            this.InitializeComponent();
            GetNotificationCount();
        }
        public class navigationvideos
        {
            public string videosource;
            public string source;
        }
        public class UnreadClass
        {
            public int Notificationcount { get; set; }
        }
        private void stackAdvocatePortal_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DietandPregnancy));
        }

        private void stackDietandPregnancy_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DietandPregnancy));
        }

        private void stackTypesofDelivery_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void stackAdvocatePortal_Tapped_1(object sender, TappedRoutedEventArgs e)
        {

        }


        private void StackNextClicked_Tapped(object sender, TappedRoutedEventArgs e)
        {
            navigationvideos navigationvide = new navigationvideos();
            navigationvide.videosource = "aANj9_oeyUM";
            navigationvide.source = "Home";
            this.Frame.Navigate(typeof(PlayVideo), navigationvide);
        }

        private async void HospitalGallery_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://healthadvisor.advocatehealth.com/Classes"));

        }

        private void mySearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            this.Frame.Navigate(typeof(SearchPage), args.QueryText);
        }

        private void notificationsImg_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Notifications));
        }
        public static int unreadNotificationCount;

        public object WebBrowserOpener { get; private set; }
        //gets all the notifications(counts)
        public async void GetNotificationCount()
        {
            try
            {
                if (App.IsInternet() == true)
                {
                    List<bool> objLiseUnreadStatus = new List<bool>();
                    // App.BASE_URL+"/api/Notifications/GetNotifications?UserId=3&HospitalId=1";
                    string getAllNotifications = App.BASE_URL + "/api/Notifications/GetNotifications?UserId=" + App.userId + "&HospitalId=" + App.hospitalId;
                    var client = new HttpClient();
                    HttpResponseMessage response = await client.GetAsync(new Uri(getAllNotifications));
                    string jsonString = await response.Content.ReadAsStringAsync();
                    JArray jArr = JArray.Parse(jsonString);
                    for (var count = 0; count < jArr.Count; count++)
                    {
                        bool isRead = (bool)jArr[count]["IsRead"];
                        if (isRead != true)
                        {
                            //objUnreadClass = new UnreadClass();
                            //objUnreadClass.Notificationcount = (int)jArr[count]["IsRead"];
                            objLiseUnreadStatus.Add(isRead);
                        }
                    }
                    unreadNotificationCount = objLiseUnreadStatus.Count;
                    //objUnreadClass = new UnreadClass();
                    txtNotificationCount.Text = objLiseUnreadStatus.Count.ToString();
                }
                else
                {
                    MessageDialog msgDialog = new MessageDialog("Please check your internet connection and try again", "Internet Connection is not available");
                    msgDialog.ShowAsync();
                }
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
                case "General":
                    DeliveryInfo("4");
                    break;
                case "Pre Delivery":
                    DeliveryInfo("1");
                    break;

                case "Delivery":
                    DeliveryInfo("2");
                    break;

                case "Post Delivery":
                    DeliveryInfo("3");
                    break;
            }

        }
        DeliveryInformation objDeliveryInformation = new DeliveryInformation();
        //gets the content of selected pivot item by passing id
        public async void DeliveryInfo(string id)
        {
            rngProgress.Visibility = Visibility.Visible;
            if (App.IsInternet() == true)
            {
                try
                {
                    List<DeliveryInformation> lstDeliveryInformation = new List<DeliveryInformation>();
                    string DeliveryInfoUri = App.BASE_URL + "/api/Tiles/GetTilesById?SUBCATEGORYID=" + id;
                    var client = new HttpClient();
                    HttpResponseMessage response = await client.GetAsync(new Uri(DeliveryInfoUri));
                    string jsonString = await response.Content.ReadAsStringAsync();

                    if (jsonString != "[]")
                    {
                        JArray jArr = JArray.Parse(jsonString);
                        for (int itemCount = 0; itemCount < jArr.Count; itemCount++)
                        {
                            objDeliveryInformation = new DeliveryInformation();
                            objDeliveryInformation.DeliveryTitle = (string)jArr[itemCount]["TITLE"];
                            objDeliveryInformation.DeliveryInfo = (string)jArr[itemCount]["CONTENT"];
                            var x = App.BASE_URL + jArr[itemCount]["TITLEIMAGE"];

                            Uri uri = new Uri(x);
                            objDeliveryInformation.DeliveryUrl = uri;
                            lstDeliveryInformation.Add(objDeliveryInformation);
                        }
                        switch (ActiveItemHeaderName)
                        {
                            case "All":
                                grdDeliveryDetails.ItemsSource = lstDeliveryInformation;
                                break;
                            case "General":
                                grdDeliveryDetailsGeneral.ItemsSource = lstDeliveryInformation;
                                break;
                            case "Pre Delivery":
                                grdDeliveryDetails1.ItemsSource = lstDeliveryInformation;
                                break;

                            case "Delivery":
                                grdDeliveryDetails2.ItemsSource = lstDeliveryInformation;
                                break;

                            case "Post Delivery":
                                grdDeliveryDetails3.ItemsSource = lstDeliveryInformation;
                                break;
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                    rngProgress.Visibility = Visibility.Collapsed;
                    MessageDialog msgDialog = new MessageDialog("Please check your internet connectivity. If the problem persists, please contact administrator.", "Message");
                    msgDialog.ShowAsync();
                }
                rngProgress.Visibility = Visibility.Collapsed;
            }
            else
            {
                rngProgress.Visibility = Visibility.Collapsed;
                MessageDialog msgDialog = new MessageDialog("Please check your internet connection and try again", "Internet Connection is not available");
                msgDialog.ShowAsync();

            }
        }


        public class DeliveryInformation
        {
            public string DeliveryTitle { get; set; }
            public string DeliveryInfo { get; set; }
            public Uri DeliveryUrl { get; set; }
        }

        private void grdDeliveryDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DeliveryInformation objDelivery = (DeliveryInformation)(sender as GridView).SelectedItem;
                string gridTitle = objDelivery.DeliveryTitle;
                if (gridTitle == "Diet and Pregnancy")
                {
                    this.Frame.Navigate(typeof(DietandPregnancy));
                }
                if (gridTitle == " My Advocate Portal")
                {
                    this.Frame.Navigate(typeof(MyAdvocatePage));
                }
            }
            catch (Exception ex)
            {
                MessageDialog msgDialog = new MessageDialog("The required resources are not downloaded.Please check your internet connectivity. If the problem persists, please contact advocate healthcare customer care associate.", "Message");
                msgDialog.ShowAsync();

            }
        }
        
       
       

      

       
    }
}