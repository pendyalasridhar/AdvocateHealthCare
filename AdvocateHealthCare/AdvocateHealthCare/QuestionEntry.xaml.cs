﻿using Newtonsoft.Json;
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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AdvocateHealthCare
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class QuestionEntry : Page
    {

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            AdvocateHealthCare.QuestionsPage.QuestionsHelper obj = (AdvocateHealthCare.QuestionsPage.QuestionsHelper)e.Parameter;
            //  var myList = e.Parameter as List<object>;

            if (obj != null)
            {

                if (obj.CreatedDate == null)
                {
                    obj.CreatedDate = "";
                }
                txtdate.Text = obj.CreatedDate;
                if (obj.QuestionTitle == null)
                {
                    obj.QuestionTitle = "";
                }
                txtquestionvalue.Text = obj.QuestionTitle;
                ProfileJournalID.Text = Convert.ToString(obj.ProfileJournalID);
                if (obj.QuestionInfo == null)
                {
                    obj.QuestionInfo = "";
                }
                txtquestioninfo.Text = obj.QuestionInfo;
                textprofilejournalid.Text = obj.ProfileJournalID;
            }


            else
            {

            }

        }

        public QuestionEntry()
        {
            this.InitializeComponent();
            txtNotificationCount.Text = HomePage.unreadNotificationCount.ToString();
            txtdate.Text = Convert.ToString(DateTime.Now);
        }
        public class ProfileJournal
        {
            public string ProfileJournalID { get; set; }

            public int ProfileID { get; set; }
            public string JournalTitle { get; set; }
            public string JournalInfo { get; set; }
            public string JournalAsset { get; set; }
            public byte JournalTypeID { get; set; }
            public DateTime CreatedDate { get; set; }
            public string CreatedBy { get; set; }
            public string LoggedInUser { get; set; }

        }
        private void QuestionsButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.IsInternet() == true)
            {
                try
                {
                    ProfileJournal profilejournal = new ProfileJournal();
                    profilejournal.CreatedDate = System.DateTime.Today;
                    if (textprofilejournalid.Text == "")
                    {
                        profilejournal.ProfileJournalID = null;
                    }
                    else {
                        profilejournal.ProfileJournalID = textprofilejournalid.Text;
                    }
                    profilejournal.ProfileID = App.userId;
                    profilejournal.JournalTitle = txtquestionvalue.Text;
                    profilejournal.JournalInfo = txtquestioninfo.Text;
                    profilejournal.JournalAsset = null;
                    profilejournal.JournalTypeID = 2;
                    profilejournal.LoggedInUser = App.userName;


                    if (txtquestionvalue.Text == "" || txtquestioninfo.Text == "")
                    {
                        MessageDialog msgDialog = new MessageDialog("Please enter both fields to proceed.", "Message");
                        msgDialog.ShowAsync();
                    }
                    else
                    {
                        var serializedPatchDoc = JsonConvert.SerializeObject(profilejournal);
                        var method = new HttpMethod("POST");
                        var request = new HttpRequestMessage(method,
                      App.BASE_URL + "/api/ProfileJournal/SaveProfileJournal")
                        //"http://localhost:53676//api/ProfileJournal/SaveProfileJournal")
                        {
                            Content = new StringContent(serializedPatchDoc,
                            System.Text.Encoding.Unicode, "application/json")
                        };
                        HttpClient client = new HttpClient();
                        var result = client.SendAsync(request).Result;
                        client.Dispose();
                        if (result.IsSuccessStatusCode == true)
                        {
                            MessageDialog msgDialog = new MessageDialog("Sucessfully Saved", "Success");
                            msgDialog.ShowAsync();
                            this.Frame.Navigate(typeof(QuestionsPage));
                        }
                        else {
                            MessageDialog msgDialog = new MessageDialog("Unsucessfull", "Failure");
                            msgDialog.ShowAsync();
                        }
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