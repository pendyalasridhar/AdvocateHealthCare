using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
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
    public sealed partial class JournalPage : Page
    {
        //private DataTransferManager datatransfermanager;
        public JournalPage()
        {
            this.InitializeComponent();

            this.Loaded += JournalPage_Loaded;
            txtNotificationCount.Text = HomePage.unreadNotificationCount.ToString();

        }
        public class Journalinfo
        {
            public string _id { get; set; }
            public string JournalTitle { get; set; }
            public string _JournalInfo { get; set; }
            public string CreatedDate { get; set; }
            //public string ImageServerPath { get; set; }
            public BitmapImage ImageServerPath { get; set; }
            public string ProfileJournalID { get; set; }
            public string ProfileName { get; set; }


        }

        async void JournalPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.IsInternet() == true)
            {
                try
                {
                    List<Journalinfo> objJournelInfo = new List<Journalinfo>();
                    HttpResponseMessage response = null;

                    //string ServiceCall = App.BASE_URL + "/api/Journals/GetJournals?UserId=2&JournalTypeId=1";
                    string ServiceCall = App.BASE_URL + "/api/Journals/GetJournals?UserId=" + App.userId + "&JournalTypeId=" + "1";//JournalTypeId 1 is for journals
                    var client = new HttpClient();
                    response = await client.GetAsync(new Uri(ServiceCall));


                    var jsonString = await response.Content.ReadAsStringAsync();
                    JArray jobject = JArray.Parse(jsonString);

                    foreach (var item in jobject)
                    {
                        Journalinfo objjournalinfo = new Journalinfo();
                        objjournalinfo.ProfileJournalID = (string)item["ProfileJournalID"];
                        objjournalinfo._id = (string)item["$id"];
                        objjournalinfo.JournalTitle = (string)item["JournalTitle"];
                        objjournalinfo._JournalInfo = (string)item["JournalInfo"];
                        objjournalinfo.CreatedDate = (string)item["CreatedDate"];
                        if ((string)item["ProfileName"] == null) // this is for the first item which we are adding in api. So, first tile is always the same with "name's" format.
                            objjournalinfo.ProfileName = Windows.Storage.ApplicationData.Current.LocalSettings.Values["userName"] + "'s Journal";
                        else
                            objjournalinfo.ProfileName = (string)item["ProfileName"];
                        var x = (string)item["JournalAsset"];

                        if (!string.IsNullOrEmpty(x))

                        {
                            objjournalinfo.ImageServerPath = new BitmapImage(new Uri(App.BASE_URL + x, UriKind.Absolute));
                        }
                        objJournelInfo.Add(objjournalinfo);
                    }
                    gridjournal.ItemsSource = objJournelInfo;

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





        private void imgEdit_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var v = ((Windows.UI.Xaml.FrameworkElement)((Windows.UI.Xaml.FrameworkElement)e.OriginalSource).Parent).DataContext;
            this.Frame.Navigate(typeof(JournalEntry), v);
        }





        private void mySearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            this.Frame.Navigate(typeof(SearchPage), args.QueryText);
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Notifications));
        }

        //shares image with social sharing
        private async void imgShare_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {

                Journalinfo QuestionData = (Journalinfo)((Windows.UI.Xaml.FrameworkElement)((Windows.UI.Xaml.FrameworkElement)e.OriginalSource).Parent).DataContext;
                // string messageBody =  // "Hi Arun ..this mail is sent from Health care App";
                var emailMessage = new Windows.ApplicationModel.Email.EmailMessage();
                emailMessage.Body = QuestionData.ProfileName + " wants to share this journal " + QuestionData._JournalInfo;
                emailMessage.Subject = QuestionData.JournalTitle;
                var email = "Enter mail address";//recipient.Emails.FirstOrDefault<Windows.ApplicationModel.Contacts.ContactEmail>();
                if (email != null)
                {
                    var emailRecipient = new Windows.ApplicationModel.Email.EmailRecipient(email);
                    emailMessage.To.Add(emailRecipient);
                }
                await Windows.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(emailMessage);

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

        private void addJournal_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(JournalEntry));
        }
    }
}

