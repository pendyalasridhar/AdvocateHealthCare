using Newtonsoft.Json;
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
    public sealed partial class JournalDetailed : Page
    {
        public DisplayOrientations orientation = DisplayOrientations.Landscape;
       
       
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

           var  obj = e.Parameter;
            if (obj != null)
            {
                int i = 0;
                foreach (var data in (dynamic)obj)
                {
                    if (i == 0)
                    {
                        txtvalue.Text = Convert.ToString(data.Text);

                    }
                    if (i == 2)
                    {
                        txtdate.Text = Convert.ToString(data.Text);
                    }
                    if (i == 4)
                    {
                        txtjournalinfo.Text = Convert.ToString(data.Text);
                    }
                    if(i==3)
                    {
                        textprofilejournalid.Text =Convert.ToString(data.Text);
                    }
                    i++;
                }
            }
            else
            {

            }  

        }
        public JournalDetailed()
        {
            this.InitializeComponent();
            var count = HomePage.unreadNotificationCount;
            txtNotificationCount.Text = HomePage.unreadNotificationCount.ToString();
            txtdate.Text = DateTime.Now.ToString("dddd") + ", " + DateTime.Now.ToString("d");
            //txtNotificationCount.Text = HomePage.unreadNotificationCount.ToString();
            //DisplayProperties.OrientationChanged += Page_OrientationChanged;
        }
      
        private void Notificationgridtapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Notifications));
        }
        private void mySearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            this.Frame.Navigate(typeof(SearchPage), args.QueryText);
        }

        public class ProfileJournal
        {
            public string ProfileJournalID { get; set; }
            public int ProfileID { get; set; }
            public string JournalTitle { get; set; }
            public string JournalInfo { get; set; }
            public string JournalAsset { get; set; }
            public byte JournalTypeID { get; set; }
            public string CreatedDate { get; set; }
            public string CreatedBy { get; set; }
            public string LoggedInUser { get; set; }

        }
        private void JournalButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.IsInternet() == true)
            {
                try
                {
                    ProfileJournal profilejournal = new ProfileJournal();
                    profilejournal.CreatedDate = Convert.ToString(DateTime.Now);
                    if (textprofilejournalid.Text == "")
                    {
                        profilejournal.ProfileJournalID = null;
                    }
                    else {
                        profilejournal.ProfileJournalID = textprofilejournalid.Text;
                    }

                    profilejournal.ProfileID = App.userId;
                    profilejournal.JournalTitle = txtvalue.Text;
                    profilejournal.JournalInfo = txtjournalinfo.Text;
                    profilejournal.JournalAsset = null;
                    profilejournal.JournalTypeID = 1;
                    profilejournal.LoggedInUser = App.userName;




                    if (txtvalue.Text != "" && txtjournalinfo.Text != "")
                    {
                        var serializedPatchDoc = JsonConvert.SerializeObject(profilejournal);
                        var method = new HttpMethod("POST");
                        var request = new HttpRequestMessage(method,
                        App.BASE_URL + "/api/ProfileJournal/SaveProfileJournal")
                        // "http://localhost:53676//api/ProfileJournal/SaveProfileJournal")

                        {
                            Content = new StringContent(serializedPatchDoc,
                            System.Text.Encoding.Unicode, "application/json")
                        };


                        HttpClient client = new HttpClient();
                        var result = client.SendAsync(request).Result;
                        client.Dispose();

                        if (result.IsSuccessStatusCode == true)
                        {
                            MessageDialog msgDialog = new MessageDialog("Successfully saved.", "Success");
                            msgDialog.ShowAsync();
                            this.Frame.Navigate(typeof(JournalPage));
                        }
                        else {
                            MessageDialog msgDialog = new MessageDialog("Unsuccessful.", "Failure");
                            msgDialog.ShowAsync();
                        }
                    }
                    else
                    {
                        MessageDialog msgDialog = new MessageDialog("Please enter both fields to proceed.", "Message");
                        msgDialog.ShowAsync();
                    }

                }

                catch (Exception ex)
                {
                    string meg = ex.StackTrace;
                    MessageDialog msgDialog = new MessageDialog(ex.Message, "Message");
                    msgDialog.ShowAsync();
                }
            }
            else
            {
                MessageDialog msgDialog = new MessageDialog("Please check your internet connection and try again", "Internet Connection is not available");
                msgDialog.ShowAsync();
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            txtvalue.IsReadOnly = false;
            txtjournalinfo.IsReadOnly = false;
           
            edit1.Visibility = Visibility.Visible;
        }

        private void BackNav_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(JournalPage));
        }
    }
}
