using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AdvocateHealthCare
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage(Frame frame)
        {
            this.InitializeComponent();
            this.ShellSplitView.Content = frame;
        }
        private void OnMenuButtonClicked(object sender, RoutedEventArgs e)
        {
            ShellSplitView.IsPaneOpen = false;
        }

        private void OnSearchButtonChecked(object sender, RoutedEventArgs e)
        {
            ShellSplitView.IsPaneOpen = false;
            if (ShellSplitView.Content != null)
                ((Frame)ShellSplitView.Content).Navigate(typeof(SettingsPage));
        }

        private void OnSettingsButtonChecked(object sender, RoutedEventArgs e)
        {
            ShellSplitView.IsPaneOpen = false;
            if (ShellSplitView.Content != null)
                ((Frame)ShellSplitView.Content).Navigate(typeof(SettingsPage));
        }

        private void HomeBtn_Click(object sender, RoutedEventArgs e)
        {
            ShellSplitView.IsPaneOpen = false;
            //  ShellSplitView.PaneBackground = new SolidColorBrush(Colors.Yellow);
            HomeBtn.Background = new SolidColorBrush(Color.FromArgb(225, 35, 35, 98));
            JournalBtn.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            WeightBtn.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            GoalsBtn.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            Gallery.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            Help.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            Video.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            Setting.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            User.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));

            if (ShellSplitView.Content != null)
                ((Frame)ShellSplitView.Content).Navigate(typeof(HomePage));
        }

        private void JournalBtn_Click(object sender, RoutedEventArgs e)
        {
            ShellSplitView.IsPaneOpen = false;
            JournalBtn.Background = new SolidColorBrush(Color.FromArgb(225, 35, 35, 98));
            HomeBtn.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            WeightBtn.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            GoalsBtn.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            Gallery.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            Help.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            Video.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            Setting.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            User.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            if (ShellSplitView.Content != null)
                ((Frame)ShellSplitView.Content).Navigate(typeof(JournalPage));
        }

        private void WeightBtn_Click(object sender, RoutedEventArgs e)
        {
            WeightBtn.Background = new SolidColorBrush(Color.FromArgb(225, 35, 35, 98));
            JournalBtn.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            HomeBtn.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));

            GoalsBtn.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            Gallery.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            Help.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            Video.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            Setting.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            User.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));




            ((Frame)ShellSplitView.Content).Navigate(typeof(InputWeightPage));
        }

        private void GoalsBtn_Click(object sender, RoutedEventArgs e)
        {
            GoalsBtn.Background = new SolidColorBrush(Color.FromArgb(225, 35, 35, 98));
            JournalBtn.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            HomeBtn.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            WeightBtn.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            Gallery.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            Video.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            Setting.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            User.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));

            ((Frame)ShellSplitView.Content).Navigate(typeof(GoalsPage));
        }

        private void Gallery_Click(object sender, RoutedEventArgs e)
        {
            Gallery.Background = new SolidColorBrush(Color.FromArgb(225, 35, 35, 98));
            JournalBtn.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            HomeBtn.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));

            WeightBtn.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            GoalsBtn.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            Help.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            Video.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            Setting.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            User.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));

            ((Frame)ShellSplitView.Content).Navigate(typeof(GalleryPage));
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {

            Help.Background = new SolidColorBrush(Color.FromArgb(225, 35, 35, 98));
            JournalBtn.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            HomeBtn.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));

            WeightBtn.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            GoalsBtn.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            Gallery.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            Video.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            Setting.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            User.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));




            ((Frame)ShellSplitView.Content).Navigate(typeof(QuestionsPage));
        }

        private void Video_Click(object sender, RoutedEventArgs e)
        {
            Video.Background = new SolidColorBrush(Color.FromArgb(225, 35, 35, 98));
            JournalBtn.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            HomeBtn.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));

            WeightBtn.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            GoalsBtn.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            Help.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            Gallery.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            Setting.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            User.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));

            ((Frame)ShellSplitView.Content).Navigate(typeof(VideosPage));
        }
        //private void OnSettingsButtonClicked(object sender, RoutedEventArgs e)
        //{
        //    ((Frame)ShellSplitView.Content).Navigate(typeof());
        //}
        //private void OnUser_Click(object sender, RoutedEventArgs e)
        //{
        //    ((Frame)ShellSplitView.Content).Navigate(typeof(VideosPage));
        //}
        private void MenuBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void User_Click(object sender, RoutedEventArgs e)
        {

            User.Background = new SolidColorBrush(Color.FromArgb(225, 35, 35, 98));
            Video.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            JournalBtn.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            HomeBtn.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));

            WeightBtn.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            GoalsBtn.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            Help.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            Gallery.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            Setting.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));

        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            Setting.Background = new SolidColorBrush(Color.FromArgb(225, 35, 35, 98));
            User.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            Video.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            JournalBtn.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            HomeBtn.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));

            WeightBtn.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            GoalsBtn.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            Help.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));
            Gallery.Background = new SolidColorBrush(Color.FromArgb(0, 75, 0, 130));


        }
    }
}
