using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    public sealed partial class QuestionsPage : Page
    {
        public QuestionsPage()
        {
            this.InitializeComponent();
            Loaded += QuestionsPage_Loaded;
            txtNotificationCount.Text = HomePage.unreadNotificationCount.ToString();
        }

        public List<QuestionsHelper> objListQuestionHelper = new List<QuestionsHelper>();
        List<QuestionsHelper> objListQuestion = new List<QuestionsHelper>();
        public class QuestionsHelper
        {

            public string CreatedDate { get; set; }
            public BitmapImage imgProp { get; set; }
            public string ImageServerPath { get; set; }
            public string QuestionTitle { get; set; }
            public string QuestionInfo { get; set; }
            public string JournelID { get; set; }
            public string _id { get; set; }
            public string ProfileJournalID { get; set; }
            public string ProfileName { get; set; }
        }


        private async void QuestionsPage_Loaded(object sender, RoutedEventArgs e)
        {
            //Flag = 0;
            //throw new NotImplementedException();
            if (App.IsInternet() == true)
            {
                try
                {
                    //string ServiceCall = App.BASE_URL + "/api/Journals/GetJournals?UserId=2&JournalTypeId=2";
                    // JournalTypeId 2 is for questions
                    string ServiceCall = App.BASE_URL + "/api/Journals/GetJournals?UserId=" + App.userId + "&JournalTypeId=" + "2";
                    //string ServiceCall = App.BASE_URL + "/api/Journals/GetJournals?UserId=" + 2 + "&JournalTypeId=" + "1";
                    var client = new HttpClient();
                    HttpResponseMessage response = await client.GetAsync(new Uri(ServiceCall));
                    var jsonString = await response.Content.ReadAsStringAsync();
                    JArray jobject = JArray.Parse(jsonString);

                    foreach (var item in jobject)
                    {
                        QuestionsHelper objQuestion = new QuestionsHelper();
                        objQuestion.JournelID = (string)item["$id"];
                        objQuestion._id = (string)item["$id"];
                        objQuestion.ProfileJournalID = (string)item["ProfileJournalID"];
                        objQuestion.CreatedDate = (string)item["CreatedDate"];
                        objQuestion.QuestionTitle = (string)item["JournalTitle"];
                        objQuestion.QuestionInfo = (string)item["JournalInfo"];
                        if ((string)item["ProfileName"] == null)
                            objQuestion.ProfileName = (string)item["ProfileName"];
                        else
                            objQuestion.ProfileName = (string)item["ProfileName"];

                        var x = (string)item["JournalAsset"];



                        if (!string.IsNullOrEmpty(x))
                        {
                            objQuestion.imgProp = new BitmapImage(new Uri(App.BASE_URL + x, UriKind.Absolute));
                        }
                        objListQuestion.Add(objQuestion);
                    }


                    gridGallary.ItemsSource = objListQuestion;
                }
                catch (Exception)
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

        //private void Imageedit_Tapped(object sender, TappedRoutedEventArgs e)
        //{
        //    var v = ((Windows.UI.Xaml.FrameworkElement)((Windows.UI.Xaml.FrameworkElement)e.OriginalSource).Parent).DataContext;
        //    this.Frame.Navigate(typeof(QuestionEntry), v);
        //}

        private void mySearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            this.Frame.Navigate(typeof(SearchPage), args.QueryText);
        }

        private void imgEdit_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var v = ((Windows.UI.Xaml.FrameworkElement)((Windows.UI.Xaml.FrameworkElement)e.OriginalSource).Parent).DataContext;
            this.Frame.Navigate(typeof(QuestionEntry), v);
        }

        private async void imgShare_Tapped(object sender, TappedRoutedEventArgs e)
        {
            QuestionsHelper QuestionData = (QuestionsHelper)((Windows.UI.Xaml.FrameworkElement)((Windows.UI.Xaml.FrameworkElement)e.OriginalSource).Parent).DataContext;
            // string messageBody =  // "Hi Arun ..this mail is sent from Health care App";
            var emailMessage = new Windows.ApplicationModel.Email.EmailMessage();
            emailMessage.Body = QuestionData.ProfileName + " wants to share this journal " + QuestionData.QuestionInfo;
            emailMessage.Subject = QuestionData.QuestionTitle;
            var email = "Enter mail address";//recipient.Emails.FirstOrDefault<Windows.ApplicationModel.Contacts.ContactEmail>();
            if (email != null)
            {
                var emailRecipient = new Windows.ApplicationModel.Email.EmailRecipient(email);
                emailMessage.To.Add(emailRecipient);
            }
            await Windows.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(emailMessage);
        }
        public void GetTrimestersData(int condition)
        {
            objListQuestionHelper.Clear();
            objListQuestionHelper = new List<QuestionsHelper>();
            //DateTime lmpValue = Convert.ToDateTime(Windows.Storage.ApplicationData.Current.LocalSettings.Values["Lmpdate"]);
            DateTime lmpDate = Convert.ToDateTime(Windows.Storage.ApplicationData.Current.LocalSettings.Values["LMPDATE"]);
            DateTime FirstTrimesterPeriod = lmpDate.AddMonths(3);
            DateTime SecondTrimesterPeriod = FirstTrimesterPeriod.AddMonths(3);
            DateTime ThirdTrimesterPeriod = SecondTrimesterPeriod.AddMonths(3);

            //var ListCount = objListQuestion.Count;
            for (int count = 0; count < objListQuestion.Count; count++)
            {
                QuestionsHelper objQuestions = new QuestionsHelper();
                objQuestions.CreatedDate = objListQuestion[count].CreatedDate;
                objQuestions.QuestionTitle = objListQuestion[count].QuestionTitle;
                objQuestions.QuestionInfo = objListQuestion[count].QuestionInfo;

                if (objQuestions.CreatedDate != null)
                {
                    string[] spilt = objListQuestion[count].CreatedDate.Split(' ');
                    string SplittedDate = spilt[1];
                    DateTime datetime = Convert.ToDateTime(SplittedDate);
                    TimeSpan GetDaysToCompare = datetime - lmpDate;
                    double days = GetDaysToCompare.TotalDays;




                    if (days > 0 && days <= 90)
                    {
                        int cond = 1;
                        if (cond == condition)
                            objListQuestionHelper.Add(objQuestions);

                    }
                    else if (days >= 91 && days <= 180)
                    {
                        int cond = 2;
                        if (cond == condition)
                            objListQuestionHelper.Add(objQuestions);
                    }
                    else if (days >= 181 && days <= 270)
                    {
                        int cond = 3;
                        if (cond == condition)
                            objListQuestionHelper.Add(objQuestions);
                    }
                    //}
                }
            }
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                PivotItem ActiveItem = e.AddedItems[0] as PivotItem;
                string ActiveItemName = ActiveItem.Header.ToString();

                //switch (ActiveItemName)
                //{
                //    case "First Trimester":
                //        GetTrimestersData();
                //        break;
                //    case "Second Trimester":
                //        GetTrimestersData();
                //        break;
                //    case "Third Trimester":
                //        GetTrimestersData();
                //        break;
                //}

                if (ActiveItemName == "First Trimester")
                {
                    GetTrimestersData(1);
                    gridGallary1.ItemsSource = objListQuestionHelper;

                }
                else if (ActiveItemName == "Second Trimester")
                {
                    GetTrimestersData(2);
                    gridGallary2.ItemsSource = objListQuestionHelper;
                }
                else if (ActiveItemName == "Third Trimester")
                {
                    GetTrimestersData(3);
                    gridGallary3.ItemsSource = objListQuestionHelper;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void Notificationgridtapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Notifications));
        }

        private void addQuestions_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(QuestionEntry));
        }
    }
}
