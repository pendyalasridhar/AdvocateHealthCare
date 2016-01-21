using Newtonsoft.Json;
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
    public sealed partial class DietandPregnancy : Page
    {

        public static string questionsTapped = "1";
        public DietandPregnancy()
        {
            this.InitializeComponent();
            getback.Visibility = Visibility.Collapsed;
            txtNotificationCount.Text = HomePage.unreadNotificationCount.ToString();//Displays the notification count
        }
        public class SaveJournalQues
        {
            public int JournalorQuestionid { get; set; }
            public string NotesforJornalorQuestion { get; set; }
        }
        private void CloseImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (grdAddNotes.Visibility == Visibility.Visible)
            {
                grdAddNotes.Visibility = Visibility.Collapsed;
                GridDiet.ColumnDefinitions.RemoveAt(1);
                getback.Visibility = Visibility.Visible;
                txtvisible.Visibility = Visibility.Collapsed;
                txtvisible1.Visibility = Visibility.Collapsed;
                txtvisible2.Visibility = Visibility.Collapsed;
                txtvisible3.Visibility = Visibility.Visible;
                txtvisible4.Visibility = Visibility.Visible;
                txtvisible5.Visibility = Visibility.Visible;
            }
        }
        //reopens add your notes coloum
        private void GetbackButton_Click(object sender, TappedRoutedEventArgs e)
        {
            ColumnDefinition column = new ColumnDefinition();
            column.Width = new GridLength(0.35, GridUnitType.Star);
            GridDiet.ColumnDefinitions.Add(column);
            Grid.SetColumn(grdAddNotes, 1);
            grdAddNotes.Visibility = Visibility.Visible;
            getback.Visibility = Visibility.Collapsed;
            txtvisible.Visibility = Visibility.Visible;
            txtvisible1.Visibility = Visibility.Visible;
            txtvisible2.Visibility = Visibility.Visible;
            txtvisible3.Visibility = Visibility.Collapsed;
            txtvisible4.Visibility = Visibility.Collapsed;
            txtvisible5.Visibility = Visibility.Collapsed;

        }
        //toggles colors of journal button color
        private void Journaltext_Tapped(object sender, TappedRoutedEventArgs e)
        {
            
            text.Background = new SolidColorBrush(Color.FromArgb(255, 229, 103, 58));
            text2.Background = new SolidColorBrush(Color.FromArgb(224, 224, 224, 224));
            txtjournal.Foreground = new SolidColorBrush(Colors.White);
            txtquestions.Foreground = new SolidColorBrush(Colors.Black);
            questionsTapped = "1";
            JorQtext1.Visibility = Visibility.Collapsed;
            JorQtext.Visibility = Visibility.Visible;
        }
        //toggles colors of questions button color
        private void Questionstext_Tapped(object sender, TappedRoutedEventArgs e)
        {
            text2.Background = new SolidColorBrush(Color.FromArgb(255, 229, 103, 58));
            text.Background = new SolidColorBrush(Color.FromArgb(224, 224, 224, 224));
            txtjournal.Foreground = new SolidColorBrush(Colors.Black);
            txtquestions.Foreground = new SolidColorBrush(Colors.White);
            questionsTapped = "2";
            JorQtext.Visibility = Visibility.Collapsed;
            JorQtext1.Visibility = Visibility.Visible;
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
        //saves users entered journals/questions
        private void saveJorQnotes(object sender, RoutedEventArgs e)
        {
            if (App.IsInternet() == true)
            {

                try
                {
                    if (JorQtext.Text.Trim() != "" || JorQtext1.Text.Trim() != "")
                    {
                        ProfileJournal profilejournal = new ProfileJournal();
                        profilejournal.CreatedDate = Convert.ToString(DateTime.Now);
                        profilejournal.ProfileJournalID = null;
                        profilejournal.ProfileID = App.userId;


                        profilejournal.JournalAsset = null;
                        if (questionsTapped == "2")
                        {
                            profilejournal.JournalTypeID = 2;
                            profilejournal.JournalTitle = "New Question Entry";
                            profilejournal.JournalInfo = JorQtext1.Text;
                        }
                        else
                        {
                            profilejournal.JournalTypeID = 1;
                            profilejournal.JournalTitle = "New Journal Entry";
                            profilejournal.JournalInfo = JorQtext.Text;
                        }
                        profilejournal.LoggedInUser = App.userName;

                        var serializedPatchDoc = JsonConvert.SerializeObject(profilejournal);
                        var method = new HttpMethod("POST");
                        var request = new HttpRequestMessage(method,
                        App.BASE_URL + "/api/ProfileJournal/SaveProfileJournal")

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
                        }
                        else {
                            MessageDialog msgDialog = new MessageDialog("Unsuccessful.", "Failure");
                            msgDialog.ShowAsync();
                        }
                    }
                    else
                    {
                        if (questionsTapped == "1")
                        {
                            MessageDialog msgDialog = new MessageDialog("Please add your journal.", "Incomplete data");
                            msgDialog.ShowAsync();
                        }
                        else
                        {
                            MessageDialog msgDialog = new MessageDialog("Please add your questions.", "Incomplete data");
                            msgDialog.ShowAsync();
                        }
                    }

                    //this.Frame.Navigate(typeof(DietandPregnancy));
                }
                catch (Exception ex)
                {
                    string meg = ex.StackTrace;
                    MessageDialog msgDialog = new MessageDialog(ex.Message, "Message");
                    msgDialog.ShowAsync();
                }
                JorQtext1.Text = "";
                JorQtext.Text = "";
            }
            else
            {
                MessageDialog msgDialog = new MessageDialog("Please check your internet connection and try again", "Internet Connection is not available");
                msgDialog.ShowAsync();
            }
        }

        private void mySearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            this.Frame.Navigate(typeof(SearchPage), args.QueryText);
        }
        //shares context through social mails
        private async void dietAndPregnancyShare_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (App.IsInternet() == true)
            {
                try
                {
                    var emailMessage = new Windows.ApplicationModel.Email.EmailMessage();
                    emailMessage.Subject = "Diet And Pregnancy";
                    emailMessage.Body = "http://www.advocatehealth.com/cmc/nutritionservices";
                    var email = "Enter mail address";//recipient.Emails.FirstOrDefault<Windows.ApplicationModel.Contacts.ContactEmail>();
                    if (email != null)
                    {
                        var emailRecipient = new Windows.ApplicationModel.Email.EmailRecipient(email);
                        emailMessage.To.Add(emailRecipient);
                    }
                    await Windows.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(emailMessage);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            else
            {

                MessageDialog msgDialog = new MessageDialog("Please check your internet connection and try again", "Internet Connection is not available");
                msgDialog.ShowAsync();
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
                    MessageDialog msgDialog = new MessageDialog("Please check your internet connectivity. If the problem persists, please contact administrator.", "Message");
                    msgDialog.ShowAsync();
                }
            }
            else
            {
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

    }
}
