using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
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
using System.Windows;
using Windows.UI.Core;
using System.Text.RegularExpressions;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
namespace AdvocateHealthCare
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InputWeightPage : Page
    {
        public int dateweek;
        public int mydate;
        public int weeks;
        DateTime lmpValue = Convert.ToDateTime(Windows.Storage.ApplicationData.Current.LocalSettings.Values["LMPDATE"]);

        public class WeightHelper
        {
            public string Weight { get; set; }
            public string CreatedDate { get; set; }
            public DateTime LMPDATE { get; set; }
            public int calculatedweek { get; set; }
            public DateTime dt { get; set; }
        }

        public InputWeightPage()
        {
            this.InitializeComponent();
            this.Loaded += InputWeightPag_Load;
            txtNotificationCount.Text = HomePage.unreadNotificationCount.ToString();//Displays the notification count
            InputScope scope = new InputScope();
            InputScopeName name = new InputScopeName();
            name.NameValue = InputScopeNameValue.TelephoneNumber;
            scope.Names.Add(name);
            Weight.InputScope = scope;
            int presentdate = (int)((DateTime.Now - lmpValue).TotalDays) / 6;
            currentweek.Text = Convert.ToString(presentdate);
        }
        public string missedDate;
        //gets all the weightentries from DB
        async void InputWeightPag_Load(object sender, RoutedEventArgs e)
        {
            if (App.IsInternet() == true)
            {
                try
                {
                    List<WeightHelper> objWeightHelper = new List<WeightHelper>();
                    //string ServiceCall = App.BASE_URL + "/api/WeightEntries/GetWeightEntries?UserId=2";
                    string ServiceCall = App.BASE_URL + "/api/WeightEntries/GetWeightEntries?UserId=" + App.userId;
                    var client = new HttpClient();
                    HttpResponseMessage response = await client.GetAsync(new Uri(ServiceCall)); 
                    var jsonString = await response.Content.ReadAsStringAsync();
                    JArray jobject = JArray.Parse(jsonString);
                    var object1 = jobject.FirstOrDefault();
                    if (object1 != null)
                    {
                        string createddates = object1.ToList()[3].ToString();
                        var createddates_ = createddates.Split(' ');
                        DateTime createddate2 = Convert.ToDateTime(createddates_[2]);
                        string lmpdate = object1.ToList()[2].ToString();
                        var lmpdate_ = lmpdate.Split(' ');

                        string fff = (lmpdate_[1]);
                        DateTime lmpdate1_ = Convert.ToDateTime(fff.Substring(1, 10));
                        int dateq = (int)((createddate2 - lmpdate1_).TotalDays) / 6;
                        if (dateq == 0)
                        {
                            mydate = dateq + 1;
                        }
                        else
                        {
                            mydate = dateq;
                        }
                    }
                    foreach (var item in jobject)
                    {
                        WeightHelper objweight = new WeightHelper();
                        objweight.CreatedDate = (string)item["CreatedDate"];
                        var stringArray = ((string)item["CreatedDate"]).Split(' ');
                        objweight.dt = Convert.ToDateTime(stringArray[1]);
                        objweight.Weight = (string)item["Weight"];
                        objweight.LMPDATE = (DateTime)item["LMPDATE"];
                        int dateX = (int)((objweight.dt - objweight.LMPDATE).TotalDays) / 6;
                        if (dateX == 0)
                        {
                            objweight.calculatedweek = dateX + 1;
                            //double week_ = ((DateTime.Now - objweight.LMPDATE).TotalDays) / 7;
                            int weekx = (int)((DateTime.Now - objweight.LMPDATE).TotalDays) / 6;
                            //int week = Coelse {
                            if (weekx == 0)
                            {
                                weeks = weekx + 1;
                            }
                            else
                            {
                                weeks = weekx + 1;
                            }
                            currentweek.Text = Convert.ToString(weeks);
                            objWeightHelper.Add(objweight);
                            dateweek = objweight.calculatedweek;
                        }
                        else
                        {
                            objweight.calculatedweek = dateX + 1;
                            //double week_ = ((DateTime.Now - objweight.LMPDATE).TotalDays) / 7;
                            int weekx = (int)((DateTime.Now - objweight.LMPDATE).TotalDays) / 6;
                            //int week = Convert.ToInt32(Math.Round(week_));
                            if (weekx == 0)
                            {
                                weeks = weekx + 1;
                            }
                            else
                            {
                                weeks = weekx + 1;
                            }
                            currentweek.Text = Convert.ToString(weeks);
                            objWeightHelper.Add(objweight);
                            dateweek = objweight.calculatedweek;
                        }
                    }
                    gridWeight.ItemsSource = objWeightHelper;
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
        //submits the input weight to Api
        async void Weightinputbutton(object sender, RoutedEventArgs e)
        {
            bool flag = false;
            if (App.IsInternet() == true)
            {
                Regex r = new Regex("^[a-zA-Z]*$");
                Regex rs = new Regex(@"[^0-9]+");
                string txtToValidation = Weight.Text;

                if (r.IsMatch(txtToValidation) || rs.IsMatch(txtToValidation))
                {
                    MessageDialog msgDialog = new MessageDialog("Please enter valid weight to proceed. Example: 123", "Message");
                    msgDialog.ShowAsync();
                }
                else {
                    if (Weight.Text.Trim() != "" && Weight.Text != "0")
                    {
                        try
                        {
                            MODEL.WeightEntries weightEntries = new MODEL.WeightEntries();
                            //DateTime xxxx =Convert.ToDateTime( _WeightInputDate.Date);
                            //xxxx.ToUniversalTime(DateTime.Now);
                            weightEntries.CreatedDate =_WeightInputDate.Date.Value.DateTime;

                            DateTime dtm = Convert.ToDateTime( weightEntries.CreatedDate);
                            if ( dtm < lmpValue)
                            {
                                MessageDialog msgDialog = new MessageDialog("Date cannot be less than LMP date.", "Message");
                                msgDialog.ShowAsync();
                            }
                            else {
                                if (dtm > DateTime.Now)
                                {
                                    flag = true;
                                    MessageDialog msgDialog = new MessageDialog("You cannot input weight for future.", "Message");
                                    msgDialog.ShowAsync();
                                }
                                else if (currentweek.Text != "")
                                {
                                    List<WeightHelper> objWeightHelper = new List<WeightHelper>();
                                    string ServiceCall = App.BASE_URL + "/api/WeightEntries/GetWeightEntries?UserId=" + App.userId;
                                    var client1 = new HttpClient();
                                    HttpResponseMessage response = await client1.GetAsync(new Uri(ServiceCall));
                                    var jsonString = await response.Content.ReadAsStringAsync();
                                    JArray jobject = JArray.Parse(jsonString);
                                    foreach (var item in jobject)
                                    {
                                        WeightHelper objweight = new WeightHelper();
                                        objweight.CreatedDate = (string)item["CreatedDate"];
                                        var stringArray = ((string)item["CreatedDate"]).Split(' ');
                                        objweight.dt = Convert.ToDateTime(stringArray[1]);
                                        objweight.Weight = (string)item["Weight"];
                                        objweight.LMPDATE = (DateTime)item["LMPDATE"];
                                       


                                        int dateX = (int)((objweight.dt - objweight.LMPDATE).TotalDays) / 6;
                                        if (dateX == 0)
                                        {
                                            objweight.calculatedweek = dateX + 1;
                                            //double week_ = ((DateTime.Now - objweight.LMPDATE).TotalDays) / 7;
                                            int weekx = (int)((DateTime.Now - objweight.LMPDATE).TotalDays) / 6;
                                            //int week = Coelse {
                                            if (weekx == 0)
                                            {
                                                weeks = weekx + 1;
                                            }
                                            else
                                            {
                                                weeks = weekx;
                                            }
                                            currentweek.Text = Convert.ToString(weeks);
                                            objWeightHelper.Add(objweight);
                                            dateweek = objweight.calculatedweek;
                                        }
                                        else
                                        {
                                            objweight.calculatedweek = dateX + 1;
                                            //double week_ = ((DateTime.Now - objweight.LMPDATE).TotalDays) / 7;
                                            int weekx = (int)((DateTime.Now - objweight.LMPDATE).TotalDays) / 6;
                                            //int week = Convert.ToInt32(Math.Round(week_));
                                            if (weekx == 0)
                                            {
                                                weeks = weekx + 1;
                                            }
                                            else
                                            {
                                                weeks = weekx;
                                            }
                                            currentweek.Text = Convert.ToString(weeks);
                                            objWeightHelper.Add(objweight);
                                            dateweek = objweight.calculatedweek;
                                        }
                                    }
                                    var abca = objWeightHelper.GroupBy(q => q.dt).Select(group => new
                                    {
                                        Metric = group.Key,
                                        Count = group.Count()
                                    }).ToList();
                                    int currentweeks = (int)((lmpValue - Convert.ToDateTime(weightEntries.CreatedDate)).TotalDays) / 6;
                                    int currentweek_ = currentweeks + 1;
                                    int existnce = 0;
                                    int conter = 0;
                                    foreach (var item in abca)
                                    {
                                        existnce = (int)((lmpValue - Convert.ToDateTime(item.Metric)).TotalDays / 6) + 1;
                                        if (existnce == currentweek_)
                                        {
                                            conter = conter + 1;
                                        }
                                    }
                                    if (conter >= 1)
                                    {
                                        flag = true;
                                        var dialog = new MessageDialog("Are you sure you want to input weight more than once in a week?");
                                        dialog.Title = "Confirmation";
                                        dialog.Commands.Add(new UICommand { Label = "yes", Id = 0 });
                                        dialog.Commands.Add(new UICommand { Label = "No", Id = 1 });
                                        var res = await dialog.ShowAsync();
                                        if ((int)res.Id == 0)
                                        {
                                            weightEntries.ProfileID = App.userId;
                                            weightEntries.Weight = Convert.ToDecimal(Weight.Text);
                                            weightEntries.LoggedInUser = App.userName;
                                            var serializedPatchDoc = JsonConvert.SerializeObject(weightEntries);
                                            var method = new HttpMethod("POST");
                                            var request = new HttpRequestMessage(method,
                                            App.BASE_URL + "/api/WeightEntries/SaveWeightTrackerType")
                                            {
                                                Content = new StringContent(serializedPatchDoc,
                                            System.Text.Encoding.Unicode, "application/json")
                                            };
                                            HttpClient client = new HttpClient();
                                            var result = client.SendAsync(request).Result;
                                            client.Dispose();
                                            if (result.IsSuccessStatusCode == false)
                                            {
                                                MessageDialog msgDialog = new MessageDialog("unsuccessful", "Failure");
                                                msgDialog.ShowAsync();
                                            }

                                        }
                                        if ((int)res.Id == 1)
                                        {
                                            this.InputWeightPag_Load(null, null);
                                        };
                                    }
                                    else
                                    {
                                        weightEntries.ProfileID = App.userId;
                                        weightEntries.Weight = Convert.ToDecimal(Weight.Text);
                                        weightEntries.LoggedInUser = App.userName;
                                        var serializedPatchDoc = JsonConvert.SerializeObject(weightEntries);
                                        var method = new HttpMethod("POST");
                                        var request = new HttpRequestMessage(method,
                                        App.BASE_URL + "/api/WeightEntries/SaveWeightTrackerType")
                                        {
                                            Content = new StringContent(serializedPatchDoc,
                                        System.Text.Encoding.Unicode, "application/json")
                                        };
                                        HttpClient client = new HttpClient();
                                        var result = client.SendAsync(request).Result;
                                        client.Dispose();
                                        if (result.IsSuccessStatusCode == false)
                                        {
                                            MessageDialog msgDialog = new MessageDialog("unsuccessful", "Failure");
                                            msgDialog.ShowAsync();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageDialog msgDialog = new MessageDialog("The required resources are not downloaded.Please check your internet connectivity. If the problem persists, please contact advocate healthcare customer care associate.", "Message");
                            msgDialog.ShowAsync();
                        }
                    }
                    else
                    {
                        MessageDialog msgDialog = new MessageDialog("Input Weight should not be empty or zero.", "Message");
                        msgDialog.ShowAsync();
                    }
                }
            }
            else
            {
                MessageDialog msgDialog = new MessageDialog("Please check your internet connection and try again", "Internet Connection is not available");
                msgDialog.ShowAsync();
            }
            Weight.Text = "";
            _WeightInputDate.Date = System.DateTime.Now;
            this.InputWeightPag_Load(null, null);
        }
        #region Helper class
        public class inputclass
        {
        }
        #endregion
        private void mySearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            this.Frame.Navigate(typeof(SearchPage), args.QueryText);
        }

        private void Notificationgridtapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Notifications));
        }

    }
    namespace MODEL
    {
        public class WeightEntries
        {
            public decimal? Weight { get; set; }
            public DateTime CreatedDate { get; set; }
            public string LoggedInUser { get; set; }
            public int WeightTrackerID { get; set; }
            public int? ProfileID { get; set; }
            public DateTime? LMPDATE { get; set; }
        }
    }
}
