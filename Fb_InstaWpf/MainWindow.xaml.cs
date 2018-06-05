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
      
        //public ObservableCollection<FbpageInboxUserInfo> Files { get; set; }
        List<FbpageInboxUserInfo> Files = new List<FbpageInboxUserInfo>();
        public FbpageInboxUserInfo ObjFbpageInboxUserInfo = new FbpageInboxUserInfo();
        private readonly BackgroundWorker worker = new BackgroundWorker();
        public ChatMessenger()
        {
            InitializeComponent();

            this.DataContext = new MainWindowViewModel();
          
            //this.DataContext = new MasterViewModel();

        }
      
        private void RichTextBoxmsngr_KeyDown(object sender, KeyEventArgs e)
        {
            if (msgtxtbox2.Text.Contains("Write a reply..."))
            {
                msgtxtbox2.Text = "";
            }
        }
        
        public string TextMessage;
        public ObservableCollection<FbUserInfo> LstFbUserInfo = new ObservableCollection<FbUserInfo>();
        public string urlName;
        public ChromeDriver ChromeWebDriver;
        readonly ChromeOptions _options = new ChromeOptions();
        private readonly Queue<string> _queueUrl = new Queue<string>();
        public string LstItemUserName { get; set; }

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
            //cmbUser.Text = "---Select---";
            //cmbUser.Items.Insert(0, "Please select any value");

        }

        private void btnUserLogins_Click(object sender, RoutedEventArgs e)
        {
            AddLoginUsers addLoginUsers = new AddLoginUsers();
            addLoginUsers.ShowDialog();
        }

    }
}
