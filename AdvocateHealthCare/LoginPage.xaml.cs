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
    /// An empty page that can be used  on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        int checkBoxFlag = 0;
        public LoginPage()
        {
            this.InitializeComponent();
            //checks wether user clicked remember my password
            if (Windows.Storage.ApplicationData.Current.LocalSettings.Values["ChechedStatus"] == null)
            {
                checkBoxFlag = 1;
            }
            else
            {
                userNameText.Text = Windows.Storage.ApplicationData.Current.LocalSettings.Values["userMailAddress"].ToString();
                pwdText.Password = Windows.Storage.ApplicationData.Current.LocalSettings.Values["userPassword"].ToString();
                cbCheckBox.IsChecked = true;
            }

        }
        private Frame _rootFrame;
        //validates the user existance
        private async void Login_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (cbCheckBox.IsChecked == true)
            {
                if (checkBoxFlag == 1)
                {
                    Windows.Storage.ApplicationData.Current.LocalSettings.Values["userMailAddress"] = userNameText.Text;
                    Windows.Storage.ApplicationData.Current.LocalSettings.Values["userPassword"] = pwdText.Password;
                    Windows.Storage.ApplicationData.Current.LocalSettings.Values["ChechedStatus"] = "True";
                }
            }
            else
            {
                Windows.Storage.ApplicationData.Current.LocalSettings.Values.Remove("ChechedStatus");
            }


            if (App.IsInternet() == true)
            {
                try
                {
                    bool authenticate = false;
                    string userName = userNameText.Text;
                    string password = pwdText.Password;
                    JObject jobject = new JObject();
                    if (userName.ToString() != String.Empty && password.ToString() != String.Empty)
                    {
                        string serviceCall = App.BASE_URL + "api/AuthenticateLogin/GetAuthentication?UserEmailId=" + userName + "&UserPwd=" + password;
                        //string serviceCall = "http://localhost:53676/api/AuthenticateLogin/GetAuthentication?UserEmailId=" + userName + "&UserPwd=" + password;
                        var client = new HttpClient();
                        HttpResponseMessage response = await client.GetAsync(new Uri(serviceCall));
                        var jsonString = await response.Content.ReadAsStringAsync();
                        jobject = JObject.Parse(jsonString);
                        if (jobject != null)
                            authenticate = (bool)jobject.SelectToken(@"Flag");

                    }
                    if (authenticate)
                    {
                        App.userId = (int)jobject.SelectToken(@"ProfileID");
                        App.hospitalId = (int)jobject.SelectToken(@"HospitalID");

                        _rootFrame = new Frame();
                        Window.Current.Content = new MainPage(_rootFrame);
                        _rootFrame.Navigate(typeof(HomePage));
                    }
                    else
                    {
                        MessageDialog msgDialog = new MessageDialog("Login Failed. Please try again", "Failure");
                        msgDialog.ShowAsync();
                        Window.Current.Content = new LoginPage();
                    }


                }
                catch (Exception ex)
                {
                    MessageDialog msgDialog = new MessageDialog("The authentication failed. Please enter valid credentials.", "Message");
                    msgDialog.ShowAsync();
                }
            }
            else
            {
                MessageDialog msgDialog = new MessageDialog("Please check your internet connection and try again", "Internet Connection is not available");
                msgDialog.ShowAsync();
            }

        }
        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
