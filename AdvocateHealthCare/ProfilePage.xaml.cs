using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using System.Text;
using Windows.Networking.Connectivity;
using System.Text.RegularExpressions;
using System.Globalization;



// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AdvocateHealthCare
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProfilePage : Page
    {
        int SelectedHosPitalId;
        bool isValidEmail = false;

        public ProfilePage()
        {
            this.InitializeComponent();
            this.Loaded += ProfilePage_Loaded;
        }
        public class ProfileSetup
        {
            public string HospitalName { get; set; }
            public int HospitalID { get; set; }
            public int MyProperty { get; set; }
        }
        //loads the hospitals lists to combobox
        async void ProfilePage_Loaded(object sender, RoutedEventArgs e)
        {
            //IsInternet();
            if (App.IsInternet() == true)
            {
                try
                {
                    List<ProfileSetup> objlistProfileSetup = new List<ProfileSetup>();
                    string ServiceCall = App.BASE_URL + "/api/HospitalInfo/GetHosipitalDetails";
                    var client = new HttpClient();
                    HttpResponseMessage response = await client.GetAsync(new Uri(ServiceCall));
                    var jsonString = await response.Content.ReadAsStringAsync();
                    JArray objJArray = JArray.Parse(jsonString);
                    var len = objJArray.Count;

                    ProfileSetup objProfileSetup = new ProfileSetup();

                    objProfileSetup.HospitalID = 0;

                    objProfileSetup.HospitalName = "Choose your primary care hospital...";
                    objlistProfileSetup.Add(objProfileSetup);
                    for (int x = 0; x < objJArray.Count; x++)
                    {
                        objProfileSetup = new ProfileSetup();
                        objProfileSetup.HospitalID = (int)objJArray[x]["HospitalID"];
                        objProfileSetup.HospitalName = (string)objJArray[x]["HospitalName"];
                        objlistProfileSetup.Add(objProfileSetup);
                    }
                    ComboBoxitemsTest.ItemsSource = null;
                    ComboBoxitemsTest.ItemsSource = objlistProfileSetup;
                    ComboBoxitemsTest.SelectedIndex = -1;
                }

                catch (Exception ex)
                {
                    string meg = ex.StackTrace;
                    MessageDialog msgDialog = new MessageDialog("'" + ex + "'", "Message");
                    msgDialog.ShowAsync();

                }
            }
            else {
                MessageDialog msgDialog = new MessageDialog("Please check your internet connection and try again", "Internet Connection is not available");
                msgDialog.ShowAsync();
            }
        }
        public string ImageToServerString { get; set; }
        public class ProfileInfo
        {
            public int ProfileID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public DateTime? LMPdate { get; set; }
            public string Picture { get; set; }
            public int HospitalID { get; set; }
            public string CreatedDate { get; set; }
            public string CreatedBy { get; set; }
            public string LoggedInUser { get; set; }
            public byte[] PostUserImage { get; set; }
        }
        public class comboxselected
        {
            public int carehospital { get; set; }
        }
        private void ComboBoxitemsTest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            SelectedHosPitalId = (int)ComboBoxitemsTest.SelectedValue;
            //  var  SelectedHosPitalIds = ComboBoxitemsTest.SelectedItem;
            if (SelectedHosPitalId == 100)
            {
                ComboBoxitemsTest.SelectedIndex = 100;
            }


        }
        //email validation
        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                isValidEmail = true;
            }
            return match.Groups[1].Value + domainName;
        }
        public bool IsValidEmail(string strIn)
        {
            isValidEmail = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", this.DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (isValidEmail)
                return false;

            // Return true if strIn is in valid e-mail format.
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }





        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
        CameraCaptureUI captureUI = new CameraCaptureUI();
        //enable camera to capture images from device
       public static string  userid;
        private async void CapturePhoto_Tapped(object sender, TappedRoutedEventArgs e)
        {

             userid = App.userId.ToString();
            if (userid == "0") {
                userid = null;
            }

            try
            {
                captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
                captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);

                StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

                if (photo != null)
                {
                    byte[] ImageToServer = await BufferFromImage(photo);


                    StringBuilder hex = new StringBuilder(ImageToServer.Length * 2);
                    foreach (byte b in ImageToServer)
                        hex.AppendFormat("{0:x2}", b);

                    ImageToServerString = hex.ToString();
                    IRandomAccessStream stream = await photo.OpenAsync(FileAccessMode.Read);
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                    SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                    SoftwareBitmap softwareBitmapBGR8 = SoftwareBitmap.Convert(softwareBitmap,
                    BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Premultiplied);
                    SoftwareBitmapSource bitmapSource = new SoftwareBitmapSource();
                    await bitmapSource.SetBitmapAsync(softwareBitmapBGR8);
                    imageControl.Source = bitmapSource;
                }
            }

            catch (Exception ex)
            {
                ErrorLog(ex.Message);
               
            }
            
        }
        public static void ErrorLog( string message)
        {
            var msg = message;
            //string profileid = null;

        
            var serializedPatchDoc = JsonConvert.SerializeObject(msg);
            var method = new HttpMethod("POST");
            var request = new HttpRequestMessage(method,
          App.BASE_URL + "/api/LogErrorMessage/LogErrorMessage?Message=" + msg + "&&ProfileID" + userid)
            //"http://localhost:53676//api/LogErrorMessage/LogErrorMessage")
            {
                Content = new StringContent(serializedPatchDoc,
                System.Text.Encoding.Unicode, "application/json")
            };
            HttpClient client = new HttpClient();
            var result = client.SendAsync(request).Result;
            client.Dispose();
        }
        //converts images to byte array
        public async Task<byte[]> BufferFromImage(StorageFile imageSource)
        {
            try
            {


                byte[] fileBytes = null;
                if (imageSource != null)
                {
                    using (IRandomAccessStreamWithContentType streamsource = await imageSource.OpenReadAsync())
                    {
                        fileBytes = new byte[streamsource.Size];
                        using (DataReader reader = new DataReader(streamsource))
                        {
                            await reader.LoadAsync((uint)streamsource.Size);
                            reader.ReadBytes(fileBytes);
                        }
                    }
                }
                return fileBytes;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (App.IsInternet() == true)
            {
                //Regex r = new Regex("^[a-zA-Z\\s]+");
                try
                {
                    Regex r = new Regex(@"^[A-z][A-z|\.|\s]+$");
                   
                        string namevalidation = txtfirstname.Text;
                        string namelvalidation = txtlastname.Text;
                    //if (r.IsMatch(namevalidation) && r.IsMatch(namelvalidation))
                    //{
                        if (txtfirstname.Text.Trim() != "" && txtlastname.Text.Trim() != "" && txtemail.Text.Trim() != "" && txtpassword.Password.Trim() != "" && txtcpassword.Password.Trim() != "" && txtdatemissed.Date.HasValue == true && ComboBoxitemsTest.SelectedItem != null)
                        {
                            if (IsValidEmail(txtemail.Text))
                            {
                                if (txtpassword.Password.Trim() == txtcpassword.Password.Trim())
                                {
                                    if (txtpassword.Password.Trim().Length < 6 && txtcpassword.Password.Trim().Length < 6)
                                    {
                                        MessageDialog msgDialog = new MessageDialog("Password should be minimun 5 characters", "Warning");
                                        msgDialog.ShowAsync();
                                    }
                                    else
                                    {
                                        ProfileInfo objProfileInfo = new ProfileInfo();
                                        objProfileInfo.FirstName = txtfirstname.Text;
                                        objProfileInfo.LastName = txtlastname.Text;
                                        objProfileInfo.Email = txtemail.Text;
                                        objProfileInfo.Password = txtpassword.Password;
                                        objProfileInfo.CreatedDate = Convert.ToString(DateTime.Now);

                                        objProfileInfo.LMPdate = Convert.ToDateTime(txtdatemissed.Date.ToString());
                                        objProfileInfo.HospitalID = SelectedHosPitalId;
                                        objProfileInfo.LoggedInUser = "null";
                                        objProfileInfo.Picture = ImageToServerString;


                                        var serializedPatchDoc = JsonConvert.SerializeObject(objProfileInfo);
                                        var method = new HttpMethod("POST");
                                        var request = new HttpRequestMessage(method,
                                         App.BASE_URL + "/api/ProfileInfo/SaveProfileInfo")
                                        //"http://localhost:53676/api/ProfileInfo/SaveProfileInfo")

                                        {
                                            Content = new StringContent(serializedPatchDoc,
                                            System.Text.Encoding.Unicode, "application/json")
                                        };
                                        HttpClient client = new HttpClient();
                                        var result = client.SendAsync(request).Result;
                                        var jsonString = result.Content.ReadAsStringAsync();
                                        var userAlreadyExist = jsonString.Result;
                                        if (userAlreadyExist != "0")
                                        {
                                            client.Dispose();

                                            if (result.IsSuccessStatusCode == true)
                                            {
                                                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                                                localSettings.Values["IsFirstTimeLogin"] = "True";
                                                localSettings.Values["userName"] = txtfirstname.Text;
                                                localSettings.Values["LMPDATE"] = txtdatemissed.Date.ToString();
                                               

                                                Window.Current.Content = new LoginPage();

                                            }

                                            else {
                                                MessageDialog msgDialog = new MessageDialog("Unsuccessfull", "Failure");
                                                msgDialog.ShowAsync();
                                            }

                                        }
                                        else
                                        {
                                            MessageDialog msgDialog = new MessageDialog("User Already Exists", "Message");
                                            msgDialog.ShowAsync();
                                        }

                                    }

                                }
                                else
                                {
                                    MessageDialog msgDialog = new MessageDialog("Password and Confirm Password didn't match", "Password Mismatch");
                                    msgDialog.ShowAsync();
                                }

                            }
                            else
                            {
                                MessageDialog msgDialog = new MessageDialog("Email id format is incorrect. Please enter correct mail id. (Example - John@outlook.com)", "Profile creation failed");
                                msgDialog.ShowAsync();
                            }
                        }
                        else
                        {
                            MessageDialog msgDialog = new MessageDialog("Please fill all the details above", "Incomplete data");
                            msgDialog.ShowAsync();

                        }
                    //}
                    //else
                    //{
                    //    MessageDialog msgDialog = new MessageDialog("Please fill all the details correctly ", "Incorrect data");
                    //    msgDialog.ShowAsync();
                    //}
                }
                //}
                catch (Exception ex)
                {
                    string meg = ex.StackTrace;
                    MessageDialog msgDialog = new MessageDialog("'" + ex + "'", "Message");
                    msgDialog.ShowAsync();

                }
                //txtfirstname.Text = "";
                //txtlastname.Text = "";
                //txtemail.Text = "";
                //txtpassword.Password = "";
                //txtcpassword.Password = "";
                //ComboBoxitemsTest.SelectedValue = 000;
            }
            else
            {
                MessageDialog msgDialog = new MessageDialog("Please check your internet connection and try again", "Internet Connection is not available");
                msgDialog.ShowAsync();
            }

        }
    }
}


