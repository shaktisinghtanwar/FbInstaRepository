using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Fb_InstaWpf.Helper;
using Fb_InstaWpf.Model;
using Fb_InstaWpf.ViewModel;
using OpenQA.Selenium.Chrome;

namespace Fb_InstaWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ChatMessenger : Window
    {
        public ChatMessenger()
        {
            InitializeComponent();

            this.DataContext = new MainWindowViewModel();
        }
            
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void TabLeftItemMessenger_GotFocus(object sender, RoutedEventArgs e)
        {
            TabRightItemInsta.Visibility = Visibility.Hidden;
            TabRightItemFacebook.Visibility = Visibility.Hidden;
            TabRightItemMessenger.IsSelected = true;
        }

        private void TabLeftFacebookItem_GotFocus(object sender, RoutedEventArgs e)
        {
            TabRightItemMessenger.Visibility = Visibility.Hidden;
            TabRightItemInsta.Visibility = Visibility.Hidden;
            TabRightItemFacebook.Visibility = Visibility.Visible;
            TabRightItemFacebook.IsSelected = true;
        }

        private void TabLeftItemInsta_GotFocus(object sender, RoutedEventArgs e)
        {
            TabRightItemMessenger.Visibility = Visibility.Hidden;
            TabRightItemFacebook.Visibility = Visibility.Hidden;
            TabRightItemInsta.Visibility = Visibility.Visible;
            TabRightItemInsta.IsSelected = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TabRightItemInsta.Visibility = Visibility.Hidden;
            TabRightItemFacebook.Visibility = Visibility.Hidden;
            ImageProgressbar.Visibility = Visibility.Hidden;
        }

        private void btnUserLogins_Click(object sender, RoutedEventArgs e)
        {
            AddLoginUsers addLoginUsers = new AddLoginUsers();
            addLoginUsers.ShowDialog();
        }

        private void cmbUser_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

    }
}
