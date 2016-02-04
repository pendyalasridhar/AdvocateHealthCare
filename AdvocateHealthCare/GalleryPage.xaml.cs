
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net.Http;
//using System.Runtime.InteropServices.WindowsRuntime;
//using System.Text;
//using System.Threading.Tasks;
//using Windows.ApplicationModel.DataTransfer;
//using Windows.Foundation;
//using Windows.Foundation.Collections;
//using Windows.Graphics.Display;
//using Windows.Graphics.Imaging;
//using Windows.Media.Capture;
//using Windows.Storage;
//using Windows.Storage.Streams;
//using Windows.UI;
//using Windows.UI.Popups;
//using Windows.UI.Text;
//using Windows.UI.Xaml;
//using Windows.UI.Xaml.Controls;
//using Windows.UI.Xaml.Controls.Primitives;
//using Windows.UI.Xaml.Data;
//using Windows.UI.Xaml.Input;
//using Windows.UI.Xaml.Media;
//using Windows.UI.Xaml.Media.Imaging;
//using Windows.UI.Xaml.Navigation;

//// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

//namespace AdvocateHealthCare
//{
//    /// <summary>
//    /// An empty page that can be used on its own or navigated to within a Frame.
//    /// </summary>
//    public sealed partial class GalleryPage : Page
//    {
//        public DisplayOrientations orientation = DisplayOrientations.Landscape;
//        public class GalleryHelper
//        {
//            public string _id { get; set; }
//            public string CreatedDate { get; set; }
//            public BitmapImage imgProp { get; set; }
//            //  public string ImageServerPath { get; set; }
//            public string ProfileName { get; set; }
//            public string Picture { get; set; }


//        }

//        public class ProfileJournal
//        {
//            public int ProfileJournalID { get; set; }
//            public int ProfileID { get; set; }
//            public string JournalTitle { get; set; }
//            public string JournalInfo { get; set; }
//            public string JournalAsset { get; set; }
//            public byte JournalTypeID { get; set; }
//            public string CreatedDate { get; set; }
//            public string CreatedBy { get; set; }
//            public string LoggedInUser { get; set; }

//        }

//        public GalleryPage()
//        {

//            this.InitializeComponent();
//            this.Loaded += JournalPage_Loaded;
//            txtNotificationCount.Text = HomePage.unreadNotificationCount.ToString(); //Displays the notification count
//            var bounds = Window.Current.Bounds;

//            double height = bounds.Height;

//            double width = bounds.Width;
//            if (height < width)
//            {
//                orientation = DisplayOrientations.Landscape;
//            }
//            else
//            {
//                orientation = DisplayOrientations.Portrait;
//            }
//            /// public DisplayOrientations orientation = DisplayOrientations.Portrait;
//            DisplayProperties.OrientationChanged += Page_OrientationChanged;
//        }
//        public void Page_OrientationChanged(object sender)
//        {
//            //The orientation of the device is ...
//            orientation = DisplayProperties.CurrentOrientation;
//            if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
//            {
//                ConstructTileGridgallery(objListGallery);

//            }

//            if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
//            {

//                ConstructTileGridgallery(objListGallery);
//            }

//        }
//        public string ImageToServerString { get; set; }
//        List<GalleryHelper> objListGallery = new List<GalleryHelper>();
//        //gets all the user saved images based on userid
//        async void JournalPage_Loaded(object sender, RoutedEventArgs e)
//        {

//            if (App.IsInternet() == true)
//            {
//                pring1.IsActive = true;
//                try
//                {

//                    //string ServiceCall = App.BASE_URL + "/api/UserSavedImages/GetUserSavedImages?UserId=2";
//                    string ServiceCall = App.BASE_URL + "/api/UserSavedImages/GetUserSavedImages?UserId=" + App.userId;
//                    var client = new HttpClient();
//                    HttpResponseMessage response = await client.GetAsync(new Uri(ServiceCall));
//                    var jsonString = await response.Content.ReadAsStringAsync();
//                    JArray jobject = JArray.Parse(jsonString);
//                    foreach (var item in jobject)
//                    {
//                        if (item["JournalAsset"].ToString() == "" || item["JournalAsset"].ToString() == "")
//                        { continue; }
//                        GalleryHelper objGallery = new GalleryHelper();
//                        objGallery._id = (string)item["$id"];
//                        objGallery.CreatedDate = (string)item["CreatedDate"];
//                        string imagePath = App.BASE_URL + (string)item["JournalAsset"];
//                        objGallery.ProfileName = (string)item["ProfileName"];

//                        if (imagePath != string.Empty)
//                        {

//                            objGallery.imgProp = new BitmapImage(new Uri(imagePath, UriKind.Absolute));

//                        }
//                        objListGallery.Add(objGallery);

//                    }

//                    ConstructTileGridgallery(objListGallery);
//                }
//                catch (Exception)
//                {
//                    throw;
//                }
//                pring1.Visibility = Visibility.Collapsed;
//            }
//            else
//            {
//                pring1.Visibility = Visibility.Collapsed;
//                MessageDialog msgDialog = new MessageDialog("Please check your internet connection and try again", "Internet Connection is not available");
//                msgDialog.ShowAsync();
//            }

//        }
//        //converts photo to stream
//        private async Task<BitmapImage> getImageFromString(string ImageToServer)
//        {
//            try
//            {
//                int NumberChars = ImageToServer.Length;
//                byte[] imageinbyte = new byte[NumberChars / 2];
//                for (int i = 0; i < NumberChars; i += 2)
//                    imageinbyte[i / 2] = Convert.ToByte(ImageToServer.Substring(i, 2), 16);
//                MemoryStream streams = new MemoryStream(imageinbyte);
//                BitmapImage image = new BitmapImage();


//                InMemoryRandomAccessStream randomAccessStream = new InMemoryRandomAccessStream();
//                await randomAccessStream.WriteAsync(imageinbyte.AsBuffer());
//                randomAccessStream.Seek(0);


//                BitmapImage img = new BitmapImage();
//                await img.SetSourceAsync(randomAccessStream);
//                return img;
//            }
//            catch (Exception)
//            {

//                throw;
//            }
//        }


//        private void galleryNotificationsBtnImg_Tapped(object sender, TappedRoutedEventArgs e)
//        {
//            this.Frame.Navigate(typeof(Notifications));
//        }

//        private void mySearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
//        {
//            this.Frame.Navigate(typeof(SearchPage), args.QueryText);
//        }


//        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
//        {
//            DataTransferManager.ShowShareUI();
//        }

//        private void Notificationgridtapped(object sender, TappedRoutedEventArgs e)
//        {
//            this.Frame.Navigate(typeof(Notifications));
//        }
//        //launches the camera to capture image
//        //private async void gridGallary_SelectionChanged(object sender, SelectionChangedEventArgs e)
//        //{
//        //    ProfilePage.userid = App.userId.ToString();
//        //    try
//        //    {
//        //        GalleryHelper objGHelper = (GalleryHelper)(sender as GridView).SelectedValue;
//        //        if (objGHelper._id == "1")
//        //        {
//        //            CameraCaptureUI captureUI = new CameraCaptureUI();
//        //            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
//        //            captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);

//        //            StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

//        //            if (photo != null)
//        //            {
//        //                byte[] ImageToServer = await BufferFromImage(photo);


//        //                StringBuilder hex = new StringBuilder(ImageToServer.Length * 2);
//        //                foreach (byte b in ImageToServer)
//        //                    hex.AppendFormat("{0:x2}", b);

//        //                ImageToServerString = hex.ToString();
//        //            }


//        //            {

//        //            }


//        //            ProfileJournal objGalley = new ProfileJournal()
//        //            {
//        //                CreatedDate = Convert.ToString(DateTime.Now),
//        //                JournalAsset = ImageToServerString,
//        //                ProfileID = App.userId,
//        //                JournalTypeID = 1,
//        //                LoggedInUser = App.userName

//        //            };

//        //            var serializedPatchDoc = JsonConvert.SerializeObject(objGalley);
//        //            var method = new HttpMethod("POST");
//        //            var request = new HttpRequestMessage(method,
//        //              App.BASE_URL + "api/ProfileJournal/SaveProfileJournal")

//        //            //"http://localhost:53677/api/ProfileJournal/SaveProfileJournal")
//        //            //"http://localhost:53676/api/ProfileInfo/SaveProfileInfo")

//        //            {
//        //                Content = new StringContent(serializedPatchDoc,
//        //                System.Text.Encoding.Unicode, "application/json")
//        //            };
//        //            HttpClient client = new HttpClient();
//        //            var result = client.SendAsync(request).Result;
//        //            client.Dispose();

//        //            if (photo != null)
//        //            {
//        //                IRandomAccessStream stream = await photo.OpenAsync(FileAccessMode.Read);
//        //                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
//        //                SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync();
//        //                SoftwareBitmap softwareBitmapBGR8 = SoftwareBitmap.Convert(softwareBitmap,
//        //                BitmapPixelFormat.Bgra8,
//        //                BitmapAlphaMode.Premultiplied);
//        //                SoftwareBitmapSource bitmapSource = new SoftwareBitmapSource();
//        //                await bitmapSource.SetBitmapAsync(softwareBitmapBGR8);

//        //            }

//        //        }
//        //        this.Frame.Navigate(typeof(GalleryPage));
//        //    }
//        //    catch (Exception ex)
//        //    {

//        //        ProfilePage.ErrorLog(ex.Message);
//        //    }
//        //}

//        //converts photo to byte array
//        //public async Task<byte[]> BufferFromImage(StorageFile imageSource)
//        //{
//        //    try
//        //    {
//        //        byte[] fileBytes = null;
//        //        if (imageSource != null)
//        //        {
//        //            using (IRandomAccessStreamWithContentType streamsource = await imageSource.OpenReadAsync())
//        //            {
//        //                fileBytes = new byte[streamsource.Size];
//        //                using (DataReader reader = new DataReader(streamsource))
//        //                {
//        //                    await reader.LoadAsync((uint)streamsource.Size);
//        //                    reader.ReadBytes(fileBytes);
//        //                }
//        //            }
//        //        }
//        //        return fileBytes;
//        //    }
//        //    catch (Exception ex)
//        //    {

//        //        throw;

//        //    }
//        //}
//        public void ConstructTileGridgallery(List<GalleryHelper> objListGallery)
//        {
//            GalleryGrid.RowDefinitions.Clear();
//            GalleryGrid.ColumnDefinitions.Clear();
//            GalleryGrid.Children.Clear();

//            if (orientation == DisplayOrientations.PortraitFlipped || orientation == DisplayOrientations.Portrait)
//            {
//                AddColumnsToTileGridPortrait();

//            }
//            else if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
//            {
//                AddColumnsToTileGridLandscape();

//            }
//            else
//            {
//                AddColumnsToTileGridLandscape();

//            }

//            int row = 0;
//            int col = -1;
//            //1st row

//            if (objListGallery.Count < 1)
//            {
//                RowDefinition rd1 = new RowDefinition();
//                GridLength gl1 = new GridLength(300);

//                rd1.Height = gl1;

//                GalleryGrid.RowDefinitions.Add(rd1);

//                Grid staticgrid = new Grid();
//                staticgrid.Background = new SolidColorBrush(Colors.White);
//                staticgrid.BorderThickness = new Thickness(1);
//                staticgrid.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
//                staticgrid.Tapped += ChildGrid2_Tapped;

//                staticgrid.Margin = new Thickness(10, 10, 10, 10);
//                //row1
//                RowDefinition childGridRowstat1 = new RowDefinition();
//                GridLength cglstat1 = new GridLength(1, GridUnitType.Star);
//                childGridRowstat1.Height = cglstat1;
//                staticgrid.RowDefinitions.Add(childGridRowstat1);
//                ////row2
//                //RowDefinition childGridRowstatic = new RowDefinition();
//                //GridLength cglstat2 = new GridLength(2, GridUnitType.Star);
//                //childGridRowstatic.Height = cglstat2;

//                //staticgrid.RowDefinitions.Add(childGridRowstatic);
//                ////row3
//                //RowDefinition childGridRowstat3 = new RowDefinition();
//                //GridLength cglstat3 = new GridLength(0.5, GridUnitType.Star);
//                //childGridRowstat3.Height = cglstat3;
//                //staticgrid.RowDefinitions.Add(childGridRowstat3);
//                //  childGrid.Tapped += ChildGrid_Tapped;

//                //  childGrid.Tapped += ChildGrid_Tapped;
//                //TextBlock stattb1 = new TextBlock();
//                //stattb1.Name = "TileName";
//                //stattb1.HorizontalAlignment = HorizontalAlignment.Center;
//                //string name = Windows.Storage.ApplicationData.Current.LocalSettings.Values["userName"] + "'s Journal";
//                //stattb1.Text = name;
//                //stattb1.TextTrimming = TextTrimming.WordEllipsis;
//                //stattb1.FontSize = 24;
//                //stattb1.Foreground = new SolidColorBrush(Colors.Black);
//                //stattb1.FontWeight = FontWeights.Normal;
//                //stattb1.Margin = new Thickness(10, 0, 0, 0);

//                //Grid.SetRow(stattb1, 0);
//                //staticgrid.Children.Add(stattb1);



//                Image img1 = new Image();


//                img1.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/capture_photo.png", UriKind.Absolute));
//                img1.Height = 100;
//                img1.Margin = new Thickness(0, 8, 0, 0);
//                Grid.SetRow(img1, 1);
//                staticgrid.Children.Add(img1);



//                TextBlock hidenTB1 = new TextBlock();
//                hidenTB1.Name = "TileName";
//                hidenTB1.Text = "capture";
//                hidenTB1.Visibility = Visibility.Collapsed;

//                Grid.SetRow(hidenTB1, 1);

//                staticgrid.Children.Add(hidenTB1);





//                //TextBlock stattb2 = new TextBlock();
//                //stattb2.Name = "TileName";

//                //stattb2.Text = "New Journal Entry";
//                //stattb2.HorizontalAlignment = HorizontalAlignment.Center;
//                //stattb2.TextTrimming = TextTrimming.WordEllipsis;
//                //stattb2.FontSize = 18;
//                //stattb2.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
//                //stattb2.FontWeight = FontWeights.Normal;
//                //stattb2.Margin = new Thickness(10, 0, 0, 0);
//                //stattb2.VerticalAlignment = VerticalAlignment.Top;


//                //Grid.SetRow(stattb2, 2);
//                //staticgrid.Children.Add(stattb2);

//                Grid.SetRow(staticgrid, 0);
//                Grid.SetColumn(staticgrid, 0);
//                GalleryGrid.Children.Add(staticgrid);
//                col = col + 1;
//            }
//            else
//            {
//                RowDefinition rd1 = new RowDefinition();
//                GridLength gl1 = new GridLength(300);

//                rd1.Height = gl1;

//                GalleryGrid.RowDefinitions.Add(rd1);
//                Grid staticgrid = new Grid();
//                staticgrid.Background = new SolidColorBrush(Colors.White);
//                staticgrid.BorderThickness = new Thickness(1);
//                staticgrid.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
//                staticgrid.Tapped += ChildGrid2_Tapped;

//                staticgrid.Margin = new Thickness(10, 10, 10, 10);
//                //row1
//                RowDefinition childGridRowstat1 = new RowDefinition();
//                GridLength cglstat1 = new GridLength(1, GridUnitType.Star);
//                childGridRowstat1.Height = cglstat1;
//                staticgrid.RowDefinitions.Add(childGridRowstat1);
//                //row2
//                //RowDefinition childGridRowstatic = new RowDefinition();
//                //GridLength cglstat2 = new GridLength(2, GridUnitType.Star);
//                //childGridRowstatic.Height = cglstat2;

//                //staticgrid.RowDefinitions.Add(childGridRowstatic);
//                ////row3
//                //RowDefinition childGridRowstat3 = new RowDefinition();
//                //GridLength cglstat3 = new GridLength(0.5, GridUnitType.Star);
//                //childGridRowstat3.Height = cglstat3;
//                //staticgrid.RowDefinitions.Add(childGridRowstat3);
//                //   childGrid.Tapped += ChildGrid_Tapped;

//                //  childGrid.Tapped += ChildGrid_Tapped;
//                //TextBlock stattb1 = new TextBlock();
//                //stattb1.Name = "TileName";
//                //stattb1.HorizontalAlignment = HorizontalAlignment.Center;
//                //string name = Windows.Storage.ApplicationData.Current.LocalSettings.Values["userName"] + "'s Journal";
//                //stattb1.Text = name;
//                //stattb1.TextTrimming = TextTrimming.WordEllipsis;
//                //stattb1.FontSize = 24;
//                //stattb1.Foreground = new SolidColorBrush(Colors.Black);
//                //stattb1.FontWeight = FontWeights.Normal;
//                //stattb1.Margin = new Thickness(10, 10, 0, 0);

//                //Grid.SetRow(stattb1, 0);
//                //staticgrid.Children.Add(stattb1);

//                Image img1 = new Image();
//                img1.Margin = new Thickness(0, 8, 0, 0);


//                img1.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/capture_photo.png", UriKind.Absolute));
//                img1.Height = 100;

//                Grid.SetRow(img1, 1);
//                staticgrid.Children.Add(img1);

//                //hidden 

//                TextBlock hidenTB1 = new TextBlock();
//                hidenTB1.Name = "TileName";
//                hidenTB1.Text = "capture";
//                hidenTB1.Visibility = Visibility.Collapsed;

//                Grid.SetRow(hidenTB1, 1);

//                staticgrid.Children.Add(hidenTB1);





//                //TextBlock stattb2 = new TextBlock();
//                //stattb2.Name = "TileName";

//                //stattb2.Text = "New Journal Entry";
//                //stattb2.HorizontalAlignment = HorizontalAlignment.Center;
//                //stattb2.TextTrimming = TextTrimming.WordEllipsis;
//                //stattb2.FontSize = 18;
//                //stattb2.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
//                //stattb2.FontWeight = FontWeights.Normal;
//                //stattb2.Margin = new Thickness(10, 0, 0, 0);
//                //stattb2.VerticalAlignment = VerticalAlignment.Top;


//                //Grid.SetRow(stattb2, 2);
//                //staticgrid.Children.Add(stattb2);

//                Grid.SetRow(staticgrid, 0);
//                Grid.SetColumn(staticgrid, 0);
//                GalleryGrid.Children.Add(staticgrid);
//                col = col + 1;
//            }
//            for (int i = 0; i < objListGallery.Count; i++)
//            {
//                //ScrollViewer scrollViewer = new ScrollViewer();



//                Grid childGrid = null;
//                childGrid = new Grid();
//                //event
//                //   childGrid.HorizontalAlignment = HorizontalAlignment.Center;
//                childGrid.Background = new SolidColorBrush(Colors.White);
//                childGrid.BorderThickness = new Thickness(1);
//                childGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));

//                childGrid.Tapped += ChildGrid2_Tapped;

//                //  childGrid.Tapped += ChildGrid_Tapped;

//                childGrid.Margin = new Thickness(10, 10, 10, 10);
//                //row1
//                RowDefinition childGridRow1 = new RowDefinition();
//                GridLength cgl1 = new GridLength(0.3, GridUnitType.Star);
//                childGridRow1.Height = cgl1;
//                childGrid.RowDefinitions.Add(childGridRow1);
//                //row2
//                RowDefinition childGridRow2 = new RowDefinition();
//                GridLength cgl2 = new GridLength(0.2, GridUnitType.Star);
//                childGridRow2.Height = cgl2;

//                childGrid.RowDefinitions.Add(childGridRow2);
//                //row3
//                RowDefinition childGridRow3 = new RowDefinition();
//                GridLength cgl3 = new GridLength(2, GridUnitType.Star);
//                childGridRow3.Height = cgl3;
//                childGrid.RowDefinitions.Add(childGridRow3);

//                //StackPanel deliveryInfoStackTile = new StackPanel();
//                //deliveryInfoStackTile.Orientation = Orientation.Vertical;

//                //<Image  Grid.Row = "0" Source = "{Binding DeliveryUrl}" Stretch = "Fill" ></ Image >




//                TextBlock tb1 = new TextBlock();
//                tb1.Name = "TileName";
//                tb1.Text = "Photo Capture";
//                tb1.TextTrimming = TextTrimming.WordEllipsis;
//                tb1.FontSize = 16;
//                tb1.Foreground = new SolidColorBrush(Colors.Black);
//                tb1.FontWeight = FontWeights.Normal;
//                tb1.Margin = new Thickness(10, 10, 0, 0);

//                Grid.SetRow(tb1, 0);
//                childGrid.Children.Add(tb1);

//                //StackPanel stackpanelobj = new StackPanel();
//                //Image img1 = new Image();


//                //img1.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/share.png", UriKind.Absolute));
//                //img1.Height = 30;
//                //img1.HorizontalAlignment = HorizontalAlignment.Right;

//                //Grid.SetRow(img1, 0);
//                //childGrid.Children.Add(img1);
//                //Image img2 = new Image();


//                //img2.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Edit.png", UriKind.Absolute));
//                //img2.Height = 30;
//                //img2.HorizontalAlignment = HorizontalAlignment.Right;

//                //Grid.SetRow(img1, 0);






//                //  < TextBlock Grid.Row = "2" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "17"  Foreground = "Black"  Text = "{Binding DeliveryInfo}"   Margin = "10,0,0,0" ></ TextBlock >

//                TextBlock tb2 = new TextBlock();
//                tb2.Text = Convert.ToString(objListGallery[i].CreatedDate);
//                tb2.TextTrimming = TextTrimming.WordEllipsis;
//                tb2.FontSize = 12;
//                tb2.Foreground = new SolidColorBrush(Colors.Black);
//                tb2.FontWeight = FontWeights.SemiLight;
//                tb2.VerticalAlignment = VerticalAlignment.Top;
//                tb2.Margin = new Thickness(10, 0, 0, 0);

//                Grid.SetRow(tb2, 1);
//                childGrid.Children.Add(tb2);












//                //////
//                // < Image  Grid.Row = "0" Source = "{Binding DeliveryUrl}" Stretch = "Fill" ></ Image >
//                Image img = new Image();
//                BitmapImage bitmapImage = new BitmapImage();
//                //  string bitmapuri = Convert.ToString(objListGallery[i].imgProp);


//                bitmapImage.UriSource = objListGallery[i].imgProp.UriSource;
//                img.Source = bitmapImage;
//               img.Stretch = Stretch.Fill;

//                Grid.SetRow(img, 2);
//                childGrid.Children.Add(img);



//                if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
//                {
//                    if ((i + 2) > 2 * (row + 1))
//                    {
//                        RowDefinition rd = new RowDefinition();
//                        GridLength gl = new GridLength(300);
//                        rd.Height = gl;
//                        GalleryGrid.RowDefinitions.Add(rd);

//                        row = row + 1;
//                        col = 0;
//                    }
//                    else
//                    {
//                        col = col + 1;
//                    }
//                }



//                else if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
//                {
//                    if ((i + 2) > 1 * (row + 1))
//                    {
//                        RowDefinition rd = new RowDefinition();
//                        GridLength gl = new GridLength(300);
//                        rd.Height = gl;
//                        GalleryGrid.RowDefinitions.Add(rd);

//                        row = row + 1;
//                        col = 0;
//                    }
//                    else
//                    {
//                        col = col + 1;
//                    }
//                }
//                else
//                {
//                    if ((i + 2) > 2 * (row + 1))
//                    {
//                        RowDefinition rd = new RowDefinition();
//                        GridLength gl = new GridLength(300);
//                        rd.Height = gl;
//                        GalleryGrid.RowDefinitions.Add(rd);

//                        row = row + 1;
//                        col = 0;
//                    }
//                    else
//                    {
//                        col = col + 1;
//                    }
//                }

//                //Add to Grid
//                Grid.SetRow(childGrid, row);
//                Grid.SetColumn(childGrid, col);
//                GalleryGrid.Children.Add(childGrid);




//            }

//        }

//        private async void ChildGrid2_Tapped(object sender, TappedRoutedEventArgs e)
//        {


//            foreach (Grid cg in GalleryGrid.Children)
//            {
//                cg.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
//            }

//         ((Windows.UI.Xaml.Controls.Grid)sender).BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));


//            DisplayOrientations x = orientation;
//            var childrens = ((Windows.UI.Xaml.Controls.Panel)sender).Children;



//            var textblocks = childrens.OfType<TextBlock>();


//            foreach (TextBlock t in textblocks)
//            {
//                if (t.Name == "TileName")
//                {
//                    if (t.Text == "capture")
//                    {
//                        ProfilePage.userid = App.userId.ToString();
//                        try
//                        {

//                            if (t.Text == "capture")
//                            {
//                                CameraCaptureUI captureUI = new CameraCaptureUI();
//                                captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
//                                captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);

//                                StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

//                                if (photo != null)
//                                {

//                                    pring1.IsActive = true;
//                                    byte[] ImageToServer = await BufferFromImage(photo);


//                                    StringBuilder hex = new StringBuilder(ImageToServer.Length * 2);
//                                    foreach (byte b in ImageToServer)
//                                        hex.AppendFormat("{0:x2}", b);

//                                    ImageToServerString = hex.ToString();
//                                }


//                                {

//                                }


//                                ProfileJournal objGalley = new ProfileJournal()
//                                {
//                                    CreatedDate = Convert.ToString(DateTime.Now),
//                                    JournalAsset = ImageToServerString,
//                                    ProfileID = App.userId,
//                                    JournalTypeID = 1,
//                                    LoggedInUser = App.userName

//                                };

//                                var serializedPatchDoc = JsonConvert.SerializeObject(objGalley);
//                                var method = new HttpMethod("POST");
//                                var request = new HttpRequestMessage(method,
//                                  App.BASE_URL + "api/ProfileJournal/SaveProfileJournal")

//                                //"http://localhost:53677/api/ProfileJournal/SaveProfileJournal")
//                                //"http://localhost:53676/api/ProfileInfo/SaveProfileInfo")

//                                {
//                                    Content = new StringContent(serializedPatchDoc,
//                                    System.Text.Encoding.Unicode, "application/json")
//                                };
//                                HttpClient client = new HttpClient();
//                                var result = client.SendAsync(request).Result;
//                                client.Dispose();

//                                if (photo != null)
//                                {
//                                    IRandomAccessStream stream = await photo.OpenAsync(FileAccessMode.Read);
//                                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
//                                    SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync();
//                                    SoftwareBitmap softwareBitmapBGR8 = SoftwareBitmap.Convert(softwareBitmap,
//                                    BitmapPixelFormat.Bgra8,
//                                    BitmapAlphaMode.Premultiplied);
//                                    SoftwareBitmapSource bitmapSource = new SoftwareBitmapSource();
//                                    await bitmapSource.SetBitmapAsync(softwareBitmapBGR8);

//                                }

//                            }
//                            pring1.Visibility = Visibility.Collapsed;
//                            this.Frame.Navigate(typeof(GalleryPage));
//                        }

//                        catch (Exception ex)
//                        {

//                            ProfilePage.ErrorLog(ex.Message);
//                        }
//                        pring1.Visibility = Visibility.Collapsed;
//                    }
//                }
//            }
//        }

//        //converts photo to byte array
//        public async Task<byte[]> BufferFromImage(StorageFile imageSource)
//        {
//            try
//            {
//                byte[] fileBytes = null;
//                if (imageSource != null)
//                {
//                    using (IRandomAccessStreamWithContentType streamsource = await imageSource.OpenReadAsync())
//                    {
//                        fileBytes = new byte[streamsource.Size];
//                        using (DataReader reader = new DataReader(streamsource))
//                        {
//                            await reader.LoadAsync((uint)streamsource.Size);
//                            reader.ReadBytes(fileBytes);
//                        }
//                    }
//                }
//                return fileBytes;
//            }
//            catch (Exception ex)
//            {

//                throw;

//            }
//        }








//    public void AddColumnsToTileGridPortrait()
//    {

//        //1st column
//        ColumnDefinition cd1 = new ColumnDefinition();
//        GridLength gl1 = new GridLength(1, GridUnitType.Star);
//        cd1.Width = gl1;
//        GalleryGrid.ColumnDefinitions.Add(cd1);

//        ////2st column
//        //ColumnDefinition cd2 = new ColumnDefinition();
//        //GridLength gl2 = new GridLength(1, GridUnitType.Star);
//        //cd2.Width = gl2;
//        //tileGrid.ColumnDefinitions.Add(cd2);

//        //3st column
//        //ColumnDefinition cd3 = new ColumnDefinition();
//        //GridLength gl3 = new GridLength(1, GridUnitType.Star);
//        //cd3.Width = gl3;
//        //tileGrid.ColumnDefinitions.Add(cd3);

//        ////4st column
//        //ColumnDefinition cd4 = new ColumnDefinition();
//        //GridLength gl4 = new GridLength(1, GridUnitType.Star);
//        //cd4.Width = gl4;
//        //tileGrid.ColumnDefinitions.Add(cd4);
//    }
//    public void AddColumnsToTileGridLandscape()
//    {

//        //1st column
//        ColumnDefinition cd1 = new ColumnDefinition();
//        GridLength gl1 = new GridLength(1, GridUnitType.Star);
//        cd1.Width = gl1;
//        GalleryGrid.ColumnDefinitions.Add(cd1);

//        //2st column
//        ColumnDefinition cd2 = new ColumnDefinition();
//        GridLength gl2 = new GridLength(1, GridUnitType.Star);
//        cd2.Width = gl2;
//        GalleryGrid.ColumnDefinitions.Add(cd2);

//        //3st column
//        //ColumnDefinition cd3 = new ColumnDefinition();
//        //GridLength gl3 = new GridLength(1, GridUnitType.Star);
//        //cd3.Width = gl3;
//        //tileGrid.ColumnDefinitions.Add(cd3);

//        ////4st column
//        //ColumnDefinition cd4 = new ColumnDefinition();
//        //GridLength gl4 = new GridLength(1, GridUnitType.Star);
//        //cd4.Width = gl4;
//        //tileGrid.ColumnDefinitions.Add(cd4);
//    }
//}

//}




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
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;
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
    public sealed partial class GalleryPage : Page
    {
        public DisplayOrientations orientation = DisplayOrientations.Landscape;
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
            public string CreatedDate { get; set; }
            public string CreatedBy { get; set; }
            public string LoggedInUser { get; set; }

        }

        public GalleryPage()
        {

            this.InitializeComponent();
            this.Loaded += JournalPage_Loaded;
            txtNotificationCount.Text = HomePage.unreadNotificationCount.ToString(); //Displays the notification count
            var bounds = Window.Current.Bounds;

            double height = bounds.Height;

            double width = bounds.Width;
            if (height < width)
            {
                orientation = DisplayOrientations.Landscape;
            }
            else
            {
                orientation = DisplayOrientations.Portrait;
            }
            /// public DisplayOrientations orientation = DisplayOrientations.Portrait;
            DisplayProperties.OrientationChanged += Page_OrientationChanged;
        }
        public void Page_OrientationChanged(object sender)
        {
            //The orientation of the device is ...
            orientation = DisplayProperties.CurrentOrientation;
            if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
            {
                ConstructTileGridgallery(objListGallery);

            }

            if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
            {

                ConstructTileGridgallery(objListGallery);
            }

        }
        public string ImageToServerString { get; set; }
        List<GalleryHelper> objListGallery = new List<GalleryHelper>();
        //gets all the user saved images based on userid
        async void JournalPage_Loaded(object sender, RoutedEventArgs e)
        {

            if (App.IsInternet() == true)
            {
                pring1.IsActive = true;
                try
                {

                    //string ServiceCall = App.BASE_URL + "/api/UserSavedImages/GetUserSavedImages?UserId=2";
                    string ServiceCall = App.BASE_URL + "/api/UserSavedImages/GetUserSavedImages?UserId=" + App.userId;
                    var client = new HttpClient();
                    HttpResponseMessage response = await client.GetAsync(new Uri(ServiceCall));
                    var jsonString = await response.Content.ReadAsStringAsync();
                    JArray jobject = JArray.Parse(jsonString);
                    foreach (var item in jobject)
                    {
                        if (item["JournalAsset"].ToString() == "" || item["JournalAsset"].ToString() == "")
                        { continue; }
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

                    ConstructTileGridgallery(objListGallery);
                }
                catch (Exception)
                {
                    throw;
                }
                pring1.Visibility = Visibility.Collapsed;
            }
            else
            {
                pring1.Visibility = Visibility.Collapsed;
                MessageDialog msgDialog = new MessageDialog("Please check your internet connection and try again", "Internet Connection is not available");
                msgDialog.ShowAsync();
            }

        }
        //converts photo to stream
        private async Task<BitmapImage> getImageFromString(string ImageToServer)
        {
            try
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
            catch (Exception)
            {

                throw;
            }
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
        //launches the camera to capture image
        //private async void gridGallary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    ProfilePage.userid = App.userId.ToString();
        //    try
        //    {
        //        GalleryHelper objGHelper = (GalleryHelper)(sender as GridView).SelectedValue;
        //        if (objGHelper._id == "1")
        //        {
        //            CameraCaptureUI captureUI = new CameraCaptureUI();
        //            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
        //            captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);

        //            StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

        //            if (photo != null)
        //            {
        //                byte[] ImageToServer = await BufferFromImage(photo);


        //                StringBuilder hex = new StringBuilder(ImageToServer.Length * 2);
        //                foreach (byte b in ImageToServer)
        //                    hex.AppendFormat("{0:x2}", b);

        //                ImageToServerString = hex.ToString();
        //            }


        //            {

        //            }


        //            ProfileJournal objGalley = new ProfileJournal()
        //            {
        //                CreatedDate = Convert.ToString(DateTime.Now),
        //                JournalAsset = ImageToServerString,
        //                ProfileID = App.userId,
        //                JournalTypeID = 1,
        //                LoggedInUser = App.userName

        //            };

        //            var serializedPatchDoc = JsonConvert.SerializeObject(objGalley);
        //            var method = new HttpMethod("POST");
        //            var request = new HttpRequestMessage(method,
        //              App.BASE_URL + "api/ProfileJournal/SaveProfileJournal")

        //            //"http://localhost:53677/api/ProfileJournal/SaveProfileJournal")
        //            //"http://localhost:53676/api/ProfileInfo/SaveProfileInfo")

        //            {
        //                Content = new StringContent(serializedPatchDoc,
        //                System.Text.Encoding.Unicode, "application/json")
        //            };
        //            HttpClient client = new HttpClient();
        //            var result = client.SendAsync(request).Result;
        //            client.Dispose();

        //            if (photo != null)
        //            {
        //                IRandomAccessStream stream = await photo.OpenAsync(FileAccessMode.Read);
        //                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
        //                SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync();
        //                SoftwareBitmap softwareBitmapBGR8 = SoftwareBitmap.Convert(softwareBitmap,
        //                BitmapPixelFormat.Bgra8,
        //                BitmapAlphaMode.Premultiplied);
        //                SoftwareBitmapSource bitmapSource = new SoftwareBitmapSource();
        //                await bitmapSource.SetBitmapAsync(softwareBitmapBGR8);

        //            }

        //        }
        //        this.Frame.Navigate(typeof(GalleryPage));
        //    }
        //    catch (Exception ex)
        //    {

        //        ProfilePage.ErrorLog(ex.Message);
        //    }
        //}

        //converts photo to byte array
        //public async Task<byte[]> BufferFromImage(StorageFile imageSource)
        //{
        //    try
        //    {
        //        byte[] fileBytes = null;
        //        if (imageSource != null)
        //        {
        //            using (IRandomAccessStreamWithContentType streamsource = await imageSource.OpenReadAsync())
        //            {
        //                fileBytes = new byte[streamsource.Size];
        //                using (DataReader reader = new DataReader(streamsource))
        //                {
        //                    await reader.LoadAsync((uint)streamsource.Size);
        //                    reader.ReadBytes(fileBytes);
        //                }
        //            }
        //        }
        //        return fileBytes;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;

        //    }
        //}

        public void ConstructTileGridgallery(List<GalleryHelper> objListGallery)
        {
            GalleryGrid.RowDefinitions.Clear();
            GalleryGrid.ColumnDefinitions.Clear();
            GalleryGrid.Children.Clear();

            if (orientation == DisplayOrientations.PortraitFlipped || orientation == DisplayOrientations.Portrait)
            {
                AddColumnsToTileGridPortrait();

            }
            else if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
            {
                AddColumnsToTileGridLandscape();

            }
            else
            {
                AddColumnsToTileGridLandscape();

            }

            int row = 0;
            int col = -1;
            //1st row

            if (objListGallery.Count < 1)
            {
                RowDefinition rd1 = new RowDefinition();
                GridLength gl1 = new GridLength(300);

                rd1.Height = gl1;

                GalleryGrid.RowDefinitions.Add(rd1);

                Grid staticgrid = new Grid();
                staticgrid.Background = new SolidColorBrush(Colors.White);
                staticgrid.BorderThickness = new Thickness(1);
                staticgrid.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
                staticgrid.Tapped += ChildGrid2_Tapped;

                staticgrid.Margin = new Thickness(10, 10, 10, 10);
                //row1
                RowDefinition childGridRowstat1 = new RowDefinition();
                GridLength cglstat1 = new GridLength(1, GridUnitType.Star);
                childGridRowstat1.Height = cglstat1;
                staticgrid.RowDefinitions.Add(childGridRowstat1);

                Image img1 = new Image();
                img1.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/capture_photo.png", UriKind.Absolute));
                img1.Height = 100;
                img1.Margin = new Thickness(0, 8, 0, 0);
                Grid.SetRow(img1, 1);
                staticgrid.Children.Add(img1);



                TextBlock hidenTB1 = new TextBlock();
                hidenTB1.Name = "TileName";
                hidenTB1.Text = "capture";
                hidenTB1.Visibility = Visibility.Collapsed;

                Grid.SetRow(hidenTB1, 1);

                staticgrid.Children.Add(hidenTB1);





                //TextBlock stattb2 = new TextBlock();
                //stattb2.Name = "TileName";

                //stattb2.Text = "New Journal Entry";
                //stattb2.HorizontalAlignment = HorizontalAlignment.Center;
                //stattb2.TextTrimming = TextTrimming.WordEllipsis;
                //stattb2.FontSize = 18;
                //stattb2.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
                //stattb2.FontWeight = FontWeights.Normal;
                //stattb2.Margin = new Thickness(10, 0, 0, 0);
                //stattb2.VerticalAlignment = VerticalAlignment.Top;


                //Grid.SetRow(stattb2, 2);
                //staticgrid.Children.Add(stattb2);

                Grid.SetRow(staticgrid, 0);
                Grid.SetColumn(staticgrid, 0);
                GalleryGrid.Children.Add(staticgrid);
                col = col + 1;
            }
            else
            {
                RowDefinition rd1 = new RowDefinition();
                GridLength gl1 = new GridLength(300);

                rd1.Height = gl1;

                GalleryGrid.RowDefinitions.Add(rd1);
                Grid staticgrid = new Grid();
                staticgrid.Background = new SolidColorBrush(Colors.White);
                staticgrid.BorderThickness = new Thickness(1);
                staticgrid.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
                staticgrid.Tapped += ChildGrid2_Tapped;

                staticgrid.Margin = new Thickness(10, 10, 10, 10);
                //row1
                RowDefinition childGridRowstat1 = new RowDefinition();
                GridLength cglstat1 = new GridLength(1, GridUnitType.Star);
                childGridRowstat1.Height = cglstat1;
                staticgrid.RowDefinitions.Add(childGridRowstat1);
                //row2
                //RowDefinition childGridRowstatic = new RowDefinition();
                //GridLength cglstat2 = new GridLength(2, GridUnitType.Star);
                //childGridRowstatic.Height = cglstat2;

                //staticgrid.RowDefinitions.Add(childGridRowstatic);
                ////row3
                //RowDefinition childGridRowstat3 = new RowDefinition();
                //GridLength cglstat3 = new GridLength(0.5, GridUnitType.Star);
                //childGridRowstat3.Height = cglstat3;
                //staticgrid.RowDefinitions.Add(childGridRowstat3);
                //   childGrid.Tapped += ChildGrid_Tapped;

                //  childGrid.Tapped += ChildGrid_Tapped;
                //TextBlock stattb1 = new TextBlock();
                //stattb1.Name = "TileName";
                //stattb1.HorizontalAlignment = HorizontalAlignment.Center;
                //string name = Windows.Storage.ApplicationData.Current.LocalSettings.Values["userName"] + "'s Journal";
                //stattb1.Text = name;
                //stattb1.TextTrimming = TextTrimming.WordEllipsis;
                //stattb1.FontSize = 24;
                //stattb1.Foreground = new SolidColorBrush(Colors.Black);
                //stattb1.FontWeight = FontWeights.Normal;
                //stattb1.Margin = new Thickness(10, 10, 0, 0);

                //Grid.SetRow(stattb1, 0);
                //staticgrid.Children.Add(stattb1);

                Image img1 = new Image();
                img1.Margin = new Thickness(0, 8, 0, 0);


                img1.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/capture_photo.png", UriKind.Absolute));
                img1.Height = 100;

                Grid.SetRow(img1, 1);
                staticgrid.Children.Add(img1);

                //hidden 

                TextBlock hidenTB1 = new TextBlock();
                hidenTB1.Name = "TileName";
                hidenTB1.Text = "capture";
                hidenTB1.Visibility = Visibility.Collapsed;

                Grid.SetRow(hidenTB1, 1);

                staticgrid.Children.Add(hidenTB1);





                //TextBlock stattb2 = new TextBlock();
                //stattb2.Name = "TileName";

                //stattb2.Text = "New Journal Entry";
                //stattb2.HorizontalAlignment = HorizontalAlignment.Center;
                //stattb2.TextTrimming = TextTrimming.WordEllipsis;
                //stattb2.FontSize = 18;
                //stattb2.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
                //stattb2.FontWeight = FontWeights.Normal;
                //stattb2.Margin = new Thickness(10, 0, 0, 0);
                //stattb2.VerticalAlignment = VerticalAlignment.Top;


                //Grid.SetRow(stattb2, 2);
                //staticgrid.Children.Add(stattb2);

                Grid.SetRow(staticgrid, 0);
                Grid.SetColumn(staticgrid, 0);
                GalleryGrid.Children.Add(staticgrid);
                col = col + 1;
            }
            for (int i = 0; i < objListGallery.Count; i++)
            {
                //ScrollViewer scrollViewer = new ScrollViewer();



                Grid childGrid = null;
                childGrid = new Grid();
                //event
                //   childGrid.HorizontalAlignment = HorizontalAlignment.Center;
                childGrid.Background = new SolidColorBrush(Colors.White);
                childGrid.BorderThickness = new Thickness(1);
                childGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));

                childGrid.Tapped += ChildGrid2_Tapped;

                //  childGrid.Tapped += ChildGrid_Tapped;

                childGrid.Margin = new Thickness(10, 10, 10, 10);
                //row1
                RowDefinition childGridRow1 = new RowDefinition();
                GridLength cgl1 = new GridLength(0.3, GridUnitType.Star);
                childGridRow1.Height = cgl1;
                childGrid.RowDefinitions.Add(childGridRow1);
                //row2
                RowDefinition childGridRow2 = new RowDefinition();
                GridLength cgl2 = new GridLength(0.2, GridUnitType.Star);
                childGridRow2.Height = cgl2;

                childGrid.RowDefinitions.Add(childGridRow2);
                //row3
                RowDefinition childGridRow3 = new RowDefinition();
                GridLength cgl3 = new GridLength(2, GridUnitType.Star);
                childGridRow3.Height = cgl3;
                childGrid.RowDefinitions.Add(childGridRow3);

                //StackPanel deliveryInfoStackTile = new StackPanel();
                //deliveryInfoStackTile.Orientation = Orientation.Vertical;

                //<Image  Grid.Row = "0" Source = "{Binding DeliveryUrl}" Stretch = "Fill" ></ Image >




                TextBlock tb1 = new TextBlock();
                tb1.Name = "TileName";
                tb1.Text = "Photo Capture";
                tb1.TextTrimming = TextTrimming.WordEllipsis;
                tb1.FontSize = 16;
                tb1.Foreground = new SolidColorBrush(Colors.Black);
                tb1.FontWeight = FontWeights.Normal;
                tb1.Margin = new Thickness(10, 10, 0, 0);

                Grid.SetRow(tb1, 0);
                childGrid.Children.Add(tb1);



                //  < TextBlock Grid.Row = "2" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "17"  Foreground = "Black"  Text = "{Binding DeliveryInfo}"   Margin = "10,0,0,0" ></ TextBlock >

                TextBlock tb2 = new TextBlock();
                tb2.Name = "CreatedDate";
                tb2.Text = Convert.ToString(objListGallery[i].CreatedDate);
                tb2.TextTrimming = TextTrimming.WordEllipsis;
                tb2.FontSize = 12;
                tb2.Foreground = new SolidColorBrush(Colors.Black);
                tb2.FontWeight = FontWeights.SemiLight;
                tb2.VerticalAlignment = VerticalAlignment.Top;
                tb2.Margin = new Thickness(10, 0, 0, 0);

                Grid.SetRow(tb2, 1);
                childGrid.Children.Add(tb2);


                TextBlock txtImageUri = new TextBlock();
                txtImageUri.Name = "ImageAsset";
                txtImageUri.Text = Convert.ToString(objListGallery[i].imgProp);
                txtImageUri.TextTrimming = TextTrimming.WordEllipsis;
                txtImageUri.FontSize = 12;
                txtImageUri.Visibility = Visibility.Collapsed;
                txtImageUri.Foreground = new SolidColorBrush(Colors.Black);
                txtImageUri.FontWeight = FontWeights.SemiLight;
                txtImageUri.VerticalAlignment = VerticalAlignment.Top;
                txtImageUri.Margin = new Thickness(10, 0, 0, 0);

                Grid.SetRow(txtImageUri, 1);
                childGrid.Children.Add(txtImageUri);


                TextBlock txtVideoID = new TextBlock();
                txtVideoID.Name = "VideoID";
                txtVideoID.Text = Convert.ToString(objListGallery[i]._id);
                txtVideoID.TextTrimming = TextTrimming.WordEllipsis;
                txtVideoID.FontSize = 12;
                txtVideoID.Visibility = Visibility.Collapsed;
                txtVideoID.Foreground = new SolidColorBrush(Colors.Black);
                txtVideoID.FontWeight = FontWeights.SemiLight;
                txtVideoID.VerticalAlignment = VerticalAlignment.Top;
                txtVideoID.Margin = new Thickness(10, 0, 0, 0);

                Grid.SetRow(txtVideoID, 1);
                childGrid.Children.Add(txtVideoID);







                //////
                // < Image  Grid.Row = "0" Source = "{Binding DeliveryUrl}" Stretch = "Fill" ></ Image >
                Image img = new Image();
                BitmapImage bitmapImage = new BitmapImage();
                //  string bitmapuri = Convert.ToString(objListGallery[i].imgProp);


                bitmapImage.UriSource = objListGallery[i].imgProp.UriSource;
                img.Source = bitmapImage;
                img.Stretch = Stretch.Fill;

                Grid.SetRow(img, 2);
                childGrid.Children.Add(img);



                if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
                {
                    if ((i + 2) > 2 * (row + 1))
                    {
                        RowDefinition rd = new RowDefinition();
                        GridLength gl = new GridLength(300);
                        rd.Height = gl;
                        GalleryGrid.RowDefinitions.Add(rd);

                        row = row + 1;
                        col = 0;
                    }
                    else
                    {
                        col = col + 1;
                    }
                }



                else if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
                {
                    if ((i + 2) > 1 * (row + 1))
                    {
                        RowDefinition rd = new RowDefinition();
                        GridLength gl = new GridLength(300);
                        rd.Height = gl;
                        GalleryGrid.RowDefinitions.Add(rd);

                        row = row + 1;
                        col = 0;
                    }
                    else
                    {
                        col = col + 1;
                    }
                }
                else
                {
                    if ((i + 2) > 2 * (row + 1))
                    {
                        RowDefinition rd = new RowDefinition();
                        GridLength gl = new GridLength(300);
                        rd.Height = gl;
                        GalleryGrid.RowDefinitions.Add(rd);

                        row = row + 1;
                        col = 0;
                    }
                    else
                    {
                        col = col + 1;
                    }
                }

                //Add to Grid
                Grid.SetRow(childGrid, row);
                Grid.SetColumn(childGrid, col);
                GalleryGrid.Children.Add(childGrid);




            }

        }
        public static List<string> lstDate;
        private async void ChildGrid2_Tapped(object sender, TappedRoutedEventArgs e)
        {

           

            //BitmapImage _reqImage = ((BitmapImage)((Image)e.OriginalSource).Parent);

            foreach (Grid cg in GalleryGrid.Children)
            {
                cg.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
            }

         ((Windows.UI.Xaml.Controls.Grid)sender).BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));


            DisplayOrientations x = orientation;
            var childrens = ((Windows.UI.Xaml.Controls.Panel)sender).Children;



            var textblocks = childrens.OfType<TextBlock>();


            foreach (TextBlock t in textblocks)
            {
                if (t.Name == "TileName")
                {
                    if (t.Text == "capture")
                    {
                        ProfilePage.userid = App.userId.ToString();
                        try
                        {

                            if (t.Text == "capture")
                            {
                                CameraCaptureUI captureUI = new CameraCaptureUI();
                                captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
                                captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);

                                StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

                                if (photo != null)
                                {

                                    pring1.IsActive = true;
                                    byte[] ImageToServer = await BufferFromImage(photo);


                                    StringBuilder hex = new StringBuilder(ImageToServer.Length * 2);
                                    foreach (byte b in ImageToServer)
                                        hex.AppendFormat("{0:x2}", b);

                                    ImageToServerString = hex.ToString();
                                }
                                ProfileJournal objGalley = new ProfileJournal()
                                {
                                    CreatedDate = Convert.ToString(DateTime.Now),
                                    JournalAsset = ImageToServerString,
                                    ProfileID = App.userId,
                                    JournalTypeID = 1,
                                    LoggedInUser = App.userName

                                };

                                var serializedPatchDoc = JsonConvert.SerializeObject(objGalley);
                                var method = new HttpMethod("POST");
                                var request = new HttpRequestMessage(method,
                                  App.BASE_URL + "api/ProfileJournal/SaveProfileJournal")

                                //"http://localhost:53677/api/ProfileJournal/SaveProfileJournal")
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

                            pring1.Visibility = Visibility.Collapsed;
                            this.Frame.Navigate(typeof(GalleryPage));
                        }

                        catch (Exception ex)
                        {

                            ProfilePage.ErrorLog(ex.Message);
                        }
                        pring1.Visibility = Visibility.Collapsed;
                    }
                    else if (t.Text == "Photo Capture")
                    {
                        var sources = new List<UIElement>(((((Panel)((FrameworkElement)e.OriginalSource).Parent).Children)));
                        TextBlock textBlock = (TextBlock)sources[1];
                        var valDate = textBlock.Text;
                        BitmapImage obj = (BitmapImage)((Image)sources[4]).Source;
                        var valImage = obj.UriSource;
                        lstDate = new List<string>();
                        lstDate.Add(valDate);
                        lstDate.Add(valImage.ToString());
                        this.Frame.Navigate(typeof(GalleryExpandedView), lstDate);
                    }
                    //else if()
                }


            }
        }

        //converts photo to byte array
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
            catch (Exception ex)
            {

                throw;

            }
        }








        public void AddColumnsToTileGridPortrait()
        {

            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            GalleryGrid.ColumnDefinitions.Add(cd1);

            ////2st column
            //ColumnDefinition cd2 = new ColumnDefinition();
            //GridLength gl2 = new GridLength(1, GridUnitType.Star);
            //cd2.Width = gl2;
            //tileGrid.ColumnDefinitions.Add(cd2);

            //3st column
            //ColumnDefinition cd3 = new ColumnDefinition();
            //GridLength gl3 = new GridLength(1, GridUnitType.Star);
            //cd3.Width = gl3;
            //tileGrid.ColumnDefinitions.Add(cd3);

            ////4st column
            //ColumnDefinition cd4 = new ColumnDefinition();
            //GridLength gl4 = new GridLength(1, GridUnitType.Star);
            //cd4.Width = gl4;
            //tileGrid.ColumnDefinitions.Add(cd4);
        }
        public void AddColumnsToTileGridLandscape()
        {

            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            GalleryGrid.ColumnDefinitions.Add(cd1);

            //2st column
            ColumnDefinition cd2 = new ColumnDefinition();
            GridLength gl2 = new GridLength(1, GridUnitType.Star);
            cd2.Width = gl2;
            GalleryGrid.ColumnDefinitions.Add(cd2);

            //3st column
            //ColumnDefinition cd3 = new ColumnDefinition();
            //GridLength gl3 = new GridLength(1, GridUnitType.Star);
            //cd3.Width = gl3;
            //tileGrid.ColumnDefinitions.Add(cd3);

            ////4st column
            //ColumnDefinition cd4 = new ColumnDefinition();
            //GridLength gl4 = new GridLength(1, GridUnitType.Star);
            //cd4.Width = gl4;
            //tileGrid.ColumnDefinitions.Add(cd4);
        }
    }

}





