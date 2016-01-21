using Newtonsoft.Json.Linq;
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
    /// 

    public sealed partial class SearchPage : Page
    {
        public SearchPage()
        {
            this.InitializeComponent();
            txtNotificationCount.Text = HomePage.unreadNotificationCount.ToString();
        }
        public class SearchHelper
        {
            public string SearchResultTitle { get; set; }
            public string SearchResultContent { get; set; }
            public Uri SearchResultImageUrl { get; set; }
        }
        SearchHelper objSearchHelper;
        List<SearchHelper> lstSearChHelper;
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            //get the searched query result
            if (App.IsInternet() == true)
            {
                try
                {
                    lstSearChHelper = new List<SearchHelper>();
                    var SearchedQuery = e.Parameter;
                    string QuerySearchUrl = App.BASE_URL + "/api/ContentSearch/GetContentSearch?String=" + SearchedQuery;
                    var client = new HttpClient();
                    HttpResponseMessage response = await client.GetAsync(new Uri(QuerySearchUrl));
                    string jsonString = await response.Content.ReadAsStringAsync();
                    JArray jArr = JArray.Parse(jsonString);
                    if (jArr.Count != 0)
                    {
                        for (int itemCount = 0; itemCount < jArr.Count; itemCount++)
                        {
                            objSearchHelper = new SearchHelper();
                            objSearchHelper.SearchResultTitle = (string)jArr[itemCount]["Title"];
                            objSearchHelper.SearchResultContent = (string)jArr[itemCount]["Content"];
                            var x = App.BASE_URL + jArr[itemCount]["TileImage"];
                            //objSearchHelper.SearchResultImageUrl = (Uri)jArr[itemCount]["TileImage"];
                            Uri uri = new Uri(x);
                            objSearchHelper.SearchResultImageUrl = uri;
                            lstSearChHelper.Add(objSearchHelper);
                        }
                        grdSearch.ItemsSource = lstSearChHelper;
                    }
                    else
                    {
                        MessageDialog msgDialog = new MessageDialog("No data found on " + SearchedQuery, "No Result");
                        msgDialog.ShowAsync();
                        this.Frame.Navigate(typeof(HomePage));
                    }
                }
                catch (Exception)
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
        private void mySearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            this.Frame.Navigate(typeof(SearchPage), args.QueryText);
        }

        private void grdSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (SearchHelper)(sender as GridView).SelectedItem;
            if (item.SearchResultTitle == "Diet and Pregnancy")
            {
                this.Frame.Navigate(typeof(DietandPregnancy));
            }
            else if (item.SearchResultTitle == " My Advocate Portal")
            {
                this.Frame.Navigate(typeof(MyAdvocatePage));
            }
        }
    }
}