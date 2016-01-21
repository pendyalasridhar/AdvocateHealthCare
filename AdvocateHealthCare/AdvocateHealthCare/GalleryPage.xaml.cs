using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;
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
    public sealed partial class GalleryPage : Page
    {
        public class GalleryHelper
        {
            public string _id { get; set; }
            public string CreatedDate { get; set; }
            public BitmapImage imgProp { get; set; }
            //  public string ImageServerPath { get; set; }
            public string ProfileName { get; set; }
            public string Picture { get; set; }

        }

        public class ProfileJournal
        {
            public int ProfileJournalID { get; set; }
            public int ProfileID { get; set; }
            public string JournalTitle { get; set; }
            public string JournalInfo { get; set; }
            public string JournalAsset { get; set; }
            public byte JournalTypeID { get; set; }
            public DateTime CreatedDate { get; set; }
            public string CreatedBy { get; set; }
            public string LoggedInUser { get; set; }

        }

        public GalleryPage()
        {

            this.InitializeComponent();
            this.Loaded += JournalPage_Loaded;
            txtNotificationCount.Text = HomePage.unreadNotificationCount.ToString();
        }
        public string ImageToServerString { get; set; }
        async void JournalPage_Loaded(object sender, RoutedEventArgs e)
        {
            List<GalleryHelper> objListGallery = new List<GalleryHelper>();
            //string ServiceCall = App.BASE_URL + "/api/UserSavedImages/GetUserSavedImages?UserId=2";
            string ServiceCall = App.BASE_URL + "/api/UserSavedImages/GetUserSavedImages?UserId=" + App.userId;
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(new Uri(ServiceCall));
            var jsonString = await response.Content.ReadAsStringAsync();
            JArray jobject = JArray.Parse(jsonString);
            foreach (var item in jobject)
            {
                GalleryHelper objGallery = new GalleryHelper();
                objGallery._id = (string)item["$id"];
                objGallery.CreatedDate = (string)item["CreatedDate"];
                string imagePath = App.BASE_URL + (string)item["JournalAsset"];
                objGallery.ProfileName = (string)item["ProfileName"];

                if (imagePath != string.Empty)
                {
                    objGallery.imgProp = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
                }
                objListGallery.Add(objGallery);

            }

            gridGallary.ItemsSource = objListGallery;

        }

        private async Task<BitmapImage> getImageFromString(string ImageToServer)
        {
            int NumberChars = ImageToServer.Length;
            byte[] imageinbyte = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                imageinbyte[i / 2] = Convert.ToByte(ImageToServer.Substring(i, 2), 16);
            MemoryStream streams = new MemoryStream(imageinbyte);
            BitmapImage image = new BitmapImage();


            InMemoryRandomAccessStream randomAccessStream = new InMemoryRandomAccessStream();
            await randomAccessStream.WriteAsync(imageinbyte.AsBuffer());
            randomAccessStream.Seek(0);


            BitmapImage img = new BitmapImage();
            await img.SetSourceAsync(randomAccessStream);
            return img;
        }


        private void galleryNotificationsBtnImg_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Notifications));
        }

        private void mySearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            this.Frame.Navigate(typeof(SearchPage), args.QueryText);
        }


        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
        }

        private void Notificationgridtapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Notifications));
        }

        private async void gridGallary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GalleryHelper objGHelper = (GalleryHelper)(sender as GridView).SelectedValue;
            if (objGHelper._id == "1")
            {
                CameraCaptureUI captureUI = new CameraCaptureUI();
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
                }


                ProfileJournal objGalley = new ProfileJournal()
                {
                    JournalAsset = ImageToServerString,
                    ProfileID = App.userId,
                    JournalTypeID = 1
                };

                var serializedPatchDoc = JsonConvert.SerializeObject(objGalley);
                var method = new HttpMethod("POST");
                var request = new HttpRequestMessage(method,
                   App.BASE_URL + "api/ProfileJournal/SaveProfileJournal")

                //"http://localhost:53676/api/ProfileJournal/SaveProfileJournal")
                //"http://localhost:53676/api/ProfileInfo/SaveProfileInfo")

                {
                    Content = new StringContent(serializedPatchDoc,
                    System.Text.Encoding.Unicode, "application/json")
                };
                HttpClient client = new HttpClient();
                var result = client.SendAsync(request).Result;
                client.Dispose();

                if (photo != null)
                {
                    IRandomAccessStream stream = await photo.OpenAsync(FileAccessMode.Read);
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                    SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                    SoftwareBitmap softwareBitmapBGR8 = SoftwareBitmap.Convert(softwareBitmap,
                    BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Premultiplied);
                    SoftwareBitmapSource bitmapSource = new SoftwareBitmapSource();
                    await bitmapSource.SetBitmapAsync(softwareBitmapBGR8);

                }

            }
            this.Frame.Navigate(typeof(GalleryPage));
        }

        public async Task<byte[]> BufferFromImage(StorageFile imageSource)
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
    }
}
