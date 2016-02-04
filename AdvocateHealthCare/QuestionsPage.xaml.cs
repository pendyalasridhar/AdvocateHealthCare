using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class QuestionsPage : Page
    {
        public DisplayOrientations orientation = DisplayOrientations.Landscape;
        public QuestionsPage()
        {
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
            this.InitializeComponent();
            Loaded += QuestionsPage_Loaded;
            txtNotificationCount.Text = HomePage.unreadNotificationCount.ToString();
            DisplayProperties.OrientationChanged += Page_OrientationChanged;
        }

        public void Page_OrientationChanged(object sender)
        {
            //The orientation of the device is ...
            orientation = DisplayProperties.CurrentOrientation;
            if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
            {

                ConstructTileGrid(objListQuestion);
            }

            if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
            {

                //VerticallyFlipped();
                ConstructTileGrid(objListQuestion);
            }

        }

        public List<QuestionsHelper> objListQuestionHelper = new List<QuestionsHelper>();
       
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
      
        QuestionsHelper objQuestion;
        List<QuestionsHelper> objListQuestion = new List<QuestionsHelper>();
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
                        if (item["JournalInfo"].ToString() == "" || item["JournalTitle"].ToString() == "")
                        { continue; }

                         objQuestion = new QuestionsHelper();
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
                        
                        if (objQuestion._id != "1")
                        {
                            objListQuestion.Add(objQuestion);
                        }
                      
                    }
                    ConstructTileGrid(objListQuestion);


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
            var QuestionsEdit = (((Windows.UI.Xaml.Controls.Panel)((Windows.UI.Xaml.FrameworkElement)((Windows.UI.Xaml.FrameworkElement)e.OriginalSource).Parent).Parent).Children).ToList();
            this.Frame.Navigate(typeof(QuestionEntry), QuestionsEdit);
        }
        string Title;
        string description;
        private async void imgShare_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var Questionsshare = (((Windows.UI.Xaml.Controls.Panel)((Windows.UI.Xaml.FrameworkElement)((Windows.UI.Xaml.FrameworkElement)e.OriginalSource).Parent).Parent).Children).ToList();
            int i = 0;
            foreach (var data in (dynamic)Questionsshare)
            {
                if (i == 0)
                {
                    Title = data.Text;
                }
                if (i == 3)
                {
                    description = data.Text;
                }
                i++;
            }

            // string messageBody =  // "Hi Arun ..this mail is sent from Health care App";
            var emailMessage = new Windows.ApplicationModel.Email.EmailMessage();
            emailMessage.Body = Windows.Storage.ApplicationData.Current.LocalSettings.Values["userName"] + " wants to share this journal " + description;
            emailMessage.Subject = Title;
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

                //if (ActiveItemName == "First Trimester")
                //{
                //    GetTrimestersData(1);
                //    gridGallary1.ItemsSource = objListQuestionHelper;

                //}
                //else if (ActiveItemName == "Second Trimester")
                //{
                //    GetTrimestersData(2);
                //    gridGallary2.ItemsSource = objListQuestionHelper;
                //}
                //else if (ActiveItemName == "Third Trimester")
                //{
                //    GetTrimestersData(3);
                //    gridGallary3.ItemsSource = objListQuestionHelper;
                //}
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
        public void ConstructTileGrid(List<QuestionsHelper> objListQuestion)
        {
            questionstileGrid.RowDefinitions.Clear();
            questionstileGrid.ColumnDefinitions.Clear();
            questionstileGrid.Children.Clear();

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
            if (objListQuestion.Count < 1)
            {
                //1st row
                RowDefinition rd1 = new RowDefinition();
                GridLength gl1 = new GridLength(300);

                rd1.Height = gl1;
                questionstileGrid.RowDefinitions.Add(rd1);


                Grid staticgrid = new Grid();
                staticgrid.Background = new SolidColorBrush(Colors.White);
                staticgrid.BorderThickness = new Thickness(1);
                staticgrid.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
                staticgrid.Tapped += ChildGrid5_Tapped;

                staticgrid.Margin = new Thickness(10, 10, 10, 10);
                //row1
                RowDefinition childGridRowstat1 = new RowDefinition();
                GridLength cglstat1 = new GridLength(0.5, GridUnitType.Star);
                childGridRowstat1.Height = cglstat1;
                staticgrid.RowDefinitions.Add(childGridRowstat1);
                //row2
                RowDefinition childGridRowstatic = new RowDefinition();
                GridLength cglstat2 = new GridLength(2, GridUnitType.Star);
                childGridRowstatic.Height = cglstat2;

                staticgrid.RowDefinitions.Add(childGridRowstatic);
                //row3
                RowDefinition childGridRowstat3 = new RowDefinition();
                GridLength cglstat3 = new GridLength(0.5, GridUnitType.Star);
                childGridRowstat3.Height = cglstat3;
                staticgrid.RowDefinitions.Add(childGridRowstat3);
              

                //  childGrid.Tapped += ChildGrid_Tapped;
                TextBlock stattb1 = new TextBlock();
                stattb1.Name = "TileName";
                stattb1.HorizontalAlignment = HorizontalAlignment.Center;
                string name = "Question for my care provider";
                stattb1.Text = name;
                stattb1.TextTrimming = TextTrimming.WordEllipsis;
                stattb1.FontSize = 24;
                stattb1.Foreground = new SolidColorBrush(Colors.Black);
                stattb1.FontWeight = FontWeights.Normal;
                stattb1.Margin = new Thickness(10, 10, 0, 0);

                Grid.SetRow(stattb1, 0);
                staticgrid.Children.Add(stattb1);

                Image img1 = new Image();


                img1.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/add_a_journal.png", UriKind.Absolute));
                img1.Height = 80;
                img1.Margin = new Thickness(0, 8, 0, 0);

                Grid.SetRow(img1, 1);
                staticgrid.Children.Add(img1);



                TextBlock stattb2 = new TextBlock();
                stattb2.Name = "TileName";

                stattb2.Text = "Ask a new question";
                stattb2.HorizontalAlignment = HorizontalAlignment.Center;
                stattb2.TextTrimming = TextTrimming.WordEllipsis;
                stattb2.FontSize = 18;
                stattb2.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
                stattb2.FontWeight = FontWeights.Normal;
                stattb2.Margin = new Thickness(10, 0, 0, 0);
                stattb2.VerticalAlignment = VerticalAlignment.Top;


                Grid.SetRow(stattb2, 2);
                staticgrid.Children.Add(stattb2);

                Grid.SetRow(staticgrid, 0);
                Grid.SetColumn(staticgrid, 0);
                questionstileGrid.Children.Add(staticgrid);
                col = col + 1;
            }
            else
            {
                //1st row
                RowDefinition rd1 = new RowDefinition();
                GridLength gl1 = new GridLength(300);

                rd1.Height = gl1;
                questionstileGrid.RowDefinitions.Add(rd1);


                Grid staticgrid = new Grid();
                staticgrid.Background = new SolidColorBrush(Colors.White);
                staticgrid.BorderThickness = new Thickness(1);
                staticgrid.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));
                staticgrid.Tapped += ChildGrid5_Tapped;

                staticgrid.Margin = new Thickness(10, 10, 10, 10);
                //row1
                RowDefinition childGridRowstat1 = new RowDefinition();
                GridLength cglstat1 = new GridLength(0.5, GridUnitType.Star);
                childGridRowstat1.Height = cglstat1;
                staticgrid.RowDefinitions.Add(childGridRowstat1);
                //row2
                RowDefinition childGridRowstatic = new RowDefinition();
                GridLength cglstat2 = new GridLength(2, GridUnitType.Star);
                childGridRowstatic.Height = cglstat2;

                staticgrid.RowDefinitions.Add(childGridRowstatic);
                //row3
                RowDefinition childGridRowstat3 = new RowDefinition();
                GridLength cglstat3 = new GridLength(0.5, GridUnitType.Star);
                childGridRowstat3.Height = cglstat3;
                staticgrid.RowDefinitions.Add(childGridRowstat3);
               //   childGrid.Tapped += ChildGrid_Tapped;

                //  childGrid.Tapped += ChildGrid_Tapped;
                TextBlock stattb1 = new TextBlock();
                stattb1.Name = "TileName";
                stattb1.HorizontalAlignment = HorizontalAlignment.Center;
                string name = "Question for my care provider";
                stattb1.Text = name;
                stattb1.TextTrimming = TextTrimming.WordEllipsis;
                stattb1.FontSize = 24;
                stattb1.Foreground = new SolidColorBrush(Colors.Black);
                stattb1.FontWeight = FontWeights.Normal;
                stattb1.Margin = new Thickness(10, 10, 0, 0);

                Grid.SetRow(stattb1, 0);
                staticgrid.Children.Add(stattb1);

                Image img1 = new Image();
                img1.Margin = new Thickness(0, 8, 0, 0);

                img1.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/add_a_journal.png", UriKind.Absolute));
                img1.Height = 70;

                Grid.SetRow(img1, 1);
                staticgrid.Children.Add(img1);



                TextBlock stattb2 = new TextBlock();
                stattb2.Name = "TileName";

                stattb2.Text = "Ask a new question";
                stattb2.HorizontalAlignment = HorizontalAlignment.Center;
                stattb2.TextTrimming = TextTrimming.WordEllipsis;
                stattb2.FontSize = 18;
                stattb2.Foreground = new SolidColorBrush(Color.FromArgb(225, 229, 103, 58));
                stattb2.FontWeight = FontWeights.Normal;
                stattb2.Margin = new Thickness(10, 0, 0, 0);
                stattb2.VerticalAlignment = VerticalAlignment.Top;


                Grid.SetRow(stattb2, 2);
                staticgrid.Children.Add(stattb2);

                Grid.SetRow(staticgrid, 0);
                Grid.SetColumn(staticgrid, 0);
                questionstileGrid.Children.Add(staticgrid);
                col = col + 1;
            }
               

            for (int i = 0; i < objListQuestion.Count; i++)
            {
                //ScrollViewer scrollViewer = new ScrollViewer();




                Grid childGrid = null;
                childGrid = new Grid();
                //event
                //   childGrid.HorizontalAlignment = HorizontalAlignment.Center;
                childGrid.Background = new SolidColorBrush(Colors.White);
                childGrid.BorderThickness = new Thickness(1);
                childGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(225, 229, 229, 229));

                childGrid.Tapped += ChildGrid5_Tapped;

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
                tb1.Text = objListQuestion[i].QuestionTitle;
                 tb1.TextTrimming = TextTrimming.WordEllipsis;
               // tb1.TextWrapping = TextWrapping.Wrap;
                tb1.FontSize = 16;
                tb1.Foreground = new SolidColorBrush(Colors.Black);
                tb1.FontWeight = FontWeights.Normal;
                tb1.Margin = new Thickness(10, 10, 0, 0);

                Grid.SetRow(tb1, 0);
                childGrid.Children.Add(tb1);


                StackPanel stackpanelobj = new StackPanel();
                stackpanelobj.Margin= new Thickness(0,5,5,0);

                stackpanelobj.Orientation = Orientation.Horizontal;
                stackpanelobj.HorizontalAlignment = HorizontalAlignment.Right;
                Image img1 = new Image();


                img1.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/share.png", UriKind.Absolute));
                img1.Height = 30;
                img1.HorizontalAlignment = HorizontalAlignment.Right;
                stackpanelobj.Children.Add(img1);
                //Grid.SetRow(img1, 0);
                //childGrid.Children.Add(img1);
                Image img2 = new Image();
                img2.Tapped += imgEdit_Tapped;
                img1.Tapped += imgShare_Tapped;


                img2.Source = new BitmapImage(new Uri(@"ms-appx:/Assets/Edit.png", UriKind.Absolute));
                img2.Height = 30;
                img2.HorizontalAlignment = HorizontalAlignment.Right;

                stackpanelobj.Children.Add(img2);
                Grid.SetRow(stackpanelobj, 0);
                childGrid.Children.Add(stackpanelobj);


                //  < TextBlock Grid.Row = "2" TextTrimming = "WordEllipsis" FontWeight = "SemiLight" FontSize = "17"  Foreground = "Black"  Text = "{Binding DeliveryInfo}"   Margin = "10,0,0,0" ></ TextBlock >

                TextBlock tb2 = new TextBlock();
                tb2.Text = Convert.ToString(objListQuestion[i].CreatedDate);
                tb2.TextTrimming = TextTrimming.WordEllipsis;
                tb2.FontSize = 12;
                tb2.Foreground = new SolidColorBrush(Colors.Black);
                tb2.VerticalAlignment = VerticalAlignment.Top;
                tb2.FontWeight = FontWeights.SemiLight;
                tb2.Margin = new Thickness(10, 0, 0, 0);

                Grid.SetRow(tb2, 1);
                childGrid.Children.Add(tb2);


                TextBlock hidenTB1 = new TextBlock();
                hidenTB1.Name = "TileName";
                hidenTB1.Text = objListQuestion[i].ProfileJournalID;
                hidenTB1.Visibility = Visibility.Collapsed;

                Grid.SetRow(hidenTB1, row);
                Grid.SetColumn(hidenTB1, col);
                childGrid.Children.Add(hidenTB1);

                Grid.SetRow(hidenTB1, 0);





                TextBlock tb3 = new TextBlock();
                tb3.Text = objListQuestion[i].QuestionInfo;
                //  tb3.TextTrimming = TextTrimming.WordEllipsis;
                tb3.TextWrapping = TextWrapping.Wrap;
                tb3.FontSize = 14;
                tb3.Foreground = new SolidColorBrush(Colors.Black);
                tb3.FontWeight = FontWeights.Normal;
                tb3.Margin = new Thickness(10, 15, 0, 0);

                Grid.SetRow(tb3, 2);
                childGrid.Children.Add(tb3);



                if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
                {
                    if ((i + 2) > 2 * (row + 1))
                    {
                        RowDefinition rd = new RowDefinition();
                        GridLength gl = new GridLength(300);
                        rd.Height = gl;
                        questionstileGrid.RowDefinitions.Add(rd);

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
                        questionstileGrid.RowDefinitions.Add(rd);

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
                        questionstileGrid.RowDefinitions.Add(rd);

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
                questionstileGrid.Children.Add(childGrid);
            }
        }
        private  async  void ChildGrid5_Tapped(object sender, TappedRoutedEventArgs e)
        {


            foreach (Grid cg in questionstileGrid.Children)
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
                    if (t.Text == "Question for my care provider") 
                    {
                        await Task.Delay(1000);
                        this.Frame.Navigate(typeof(QuestionEntry));
                        break;

                    }
                    if (t.Text != "Question for my care provider")
                    {
                        // (((Windows.UI.Xaml.Controls.Panel)((Windows.UI.Xaml.FrameworkElement)((Windows.UI.Xaml.FrameworkElement)e.OriginalSource).Parent).Parent).Children).ToList();
                        var QuestionsDetailed = (((Windows.UI.Xaml.Controls.Panel)((Windows.UI.Xaml.FrameworkElement)e.OriginalSource).Parent).Children).ToList();
                        await Task.Delay(1000);
                        this.Frame.Navigate(typeof(QuestionsDetailed), QuestionsDetailed);
                        break;
                    }
                }
            }
        }
        
        public void AddColumnsToTileGrid()
        {

            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            questionstileGrid.ColumnDefinitions.Add(cd1);

            //2st column
            ColumnDefinition cd2 = new ColumnDefinition();
            GridLength gl2 = new GridLength(1, GridUnitType.Star);
            cd2.Width = gl2;
            questionstileGrid.ColumnDefinitions.Add(cd2);

            //3st column
            //ColumnDefinition cd3 = new ColumnDefinition();
            //GridLength gl3 = new GridLength(1, GridUnitType.Star);
            //cd3.Width = gl3;
            //tileGrid.ColumnDefinitions.Add(cd3);

            //4st column
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
            questionstileGrid.ColumnDefinitions.Add(cd1);

            //2st column
            ColumnDefinition cd2 = new ColumnDefinition();
            GridLength gl2 = new GridLength(1, GridUnitType.Star);
            cd2.Width = gl2;
            questionstileGrid.ColumnDefinitions.Add(cd2);

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
        public void AddColumnsToTileGridPortrait()
        {

            //1st column
            ColumnDefinition cd1 = new ColumnDefinition();
            GridLength gl1 = new GridLength(1, GridUnitType.Star);
            cd1.Width = gl1;
            questionstileGrid.ColumnDefinitions.Add(cd1);

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

    }


}

