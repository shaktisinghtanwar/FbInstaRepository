using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using Fb_InstaWpf.Helper;
using Fb_InstaWpf.Model;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Threading;
using Fb_InstaWpf.DbModel;

namespace Fb_InstaWpf.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        DatabaseContext _databaseContext;
        FacebookUserLoginInfo _loginUser;
        DbHelper _dbHelper;
        OnlineFetcher _onlineFetcher;
        OnlinePoster _onlinePoster;

        #region Commands

        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand FbMessengerListCommand { get; set; }
        public DelegateCommand FbPageInboxCommand { get; set; }
        public DelegateCommand InstaInboxCommand { get; set; }

        public DelegateCommand TabCtrlLoaded { get; set; }
        public DelegateCommand Tab2CtrlLoaded { get; set; }
        public DelegateCommand Tab0CtrlLoaded { get; set; }
        public DelegateCommand ImageProgressBarLoaded { get; set; }

        public DelegateCommand CloseTabCommand { get; set; }

        #endregion

        public FacebookUserLoginInfo LoginUser
        {
            get { return _loginUser; }
            set
            {
                if ( value != null)
                {
                    _loginUser = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<FacebookUserLoginInfo> LoginUsersList
        {
            get { return _loginUsersList; }
            set { _loginUsersList = value; OnPropertyChanged(); }
        }

        public SocialTabViewModel MessengerUserListViewModel
        {
            get {return _messengerUserListViewModel;}
            set
            {
                _messengerUserListViewModel = value;
                OnPropertyChanged();
            }
        }
        public SocialTabViewModel InstagramUserListViewModel
        {
            get { return _instaInboxmember; }
            set { _instaInboxmember = value; OnPropertyChanged(); }
        }

        public SocialTabViewModel FacebookUserListViewModel
        {
            get { return _fbPageInboxmember; }
            set { _fbPageInboxmember = value; OnPropertyChanged(); }
        }

        #region Contructor
        public MainWindowViewModel()
        {
            LoginImageInfo = new ObservableCollection<ImageLoginTextbox>();
            LoginCommand = new DelegateCommand(LoginCommandHandler, null);
            FbMessengerListCommand = new DelegateCommand(LeftFbMessengerListCommandHandler, null);
            InstaInboxCommand = new DelegateCommand(LeftInstaInboxCommandHandler, null);
            FbPageInboxCommand = new DelegateCommand(LeftFbPageInboxCommandHandler, null);
       
            ImageProgressBarLoaded = new DelegateCommand(ImageProgressBarLoadedCommandHandler, null);
            CloseTabCommand = new DelegateCommand(CloseTab);

            _databaseContext = new DatabaseContext();
            _onlineFetcher = new OnlineFetcher();
            _onlinePoster = new OnlinePoster();
            _dbHelper = new DbHelper();
            
            Task.Factory.StartNew(() => FillLoginUserList());
        }

        private void FillLoginUserList()
        {
            var data = _dbHelper.GetLoginUsers();
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                LoginUsersList = data;
                LoginUser = data.FirstOrDefault();
            });
        }

        public void LeftFbMessengerListCommandHandler(object obj)
        {
            TabControlSelectedIndex = Convert.ToInt16(obj);
            if (MessengerUserListViewModel == null)
                Task.Factory.StartNew(() => LeftMessengerData());
        }

        private void LeftFbPageInboxCommandHandler(object obj)
        {
            Task.Factory.StartNew(() => LeftFacebookData());
            TabControlSelectedIndex = Convert.ToInt16(obj);
        }

        private void LeftInstaInboxCommandHandler(object obj)
        {
            TabControlSelectedIndex = Convert.ToInt16(obj);
            Task.Factory.StartNew(() => LeftInstagramData());
        }

        private void LeftMessengerData()
        {
            var data = _dbHelper.GetLeftMessengerListData(LoginUser.UserId);

            Application.Current.Dispatcher.Invoke(() =>
            {
                MessengerUserListViewModel = MessengerUserListViewModel ?? new SocialTabViewModel(Enums.TabType.Messenger)
                {
                    UserListInfo = data
                };
                if (MessengerUserListViewModel.SelectedItem == null)
                    MessengerUserListViewModel.SelectedItem = data.FirstOrDefault();
            });
        }

        private void LeftFacebookData()
        {
            var data = _dbHelper.GetLeftMessengerListData(LoginUser.UserId);
           // var data = _dbHelper.GetFacebookListData(LoginUser.UserId);
            Application.Current.Dispatcher.Invoke(() =>
            {
                FacebookUserListViewModel = FacebookUserListViewModel ?? new SocialTabViewModel(Enums.TabType.Facebook)
                {
                    UserListInfo = data
                };
                if (FacebookUserListViewModel.SelectedItem == null)
                    FacebookUserListViewModel.SelectedItem = data.FirstOrDefault();
            });
        }

        private void LeftInstagramData()
        {
            //var data = _dbHelper.GetInstaUserList(LoginUser.UserId);
             var data = _dbHelper.GetLeftMessengerListData(LoginUser.UserId);
            Application.Current.Dispatcher.Invoke(() =>
            {
                InstagramUserListViewModel = InstagramUserListViewModel ?? new SocialTabViewModel(Enums.TabType.Instagram)
                {
                    UserListInfo = data
                };
                if (InstagramUserListViewModel.SelectedItem == null)
                    InstagramUserListViewModel.SelectedItem = data.FirstOrDefault();
            });
        }

        private void CloseTab(object obj)
        {
            string[] array = obj.ToString().Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
            string tabName = array[0];
            string parentTabName = array[1];
            switch (parentTabName)
            {
                case "Messenger":
                    MessengerUserListViewModel?.SelectedUsers?.Remove(MessengerUserListViewModel.SelectedUsers.FirstOrDefault(m => m.InboxUserName == tabName));
                    if (MessengerUserListViewModel.SelectedUsers.Count == 0)
                    {
                        MessengerUserListViewModel.SelectedItem = null;
                    }
                    break;
                case "Facebook":
                    FacebookUserListViewModel?.SelectedUsers?.Remove(FacebookUserListViewModel.SelectedUsers.FirstOrDefault(m => m.InboxUserName == tabName));
                    if (FacebookUserListViewModel.SelectedUsers.Count == 0)
                    {
                        FacebookUserListViewModel.SelectedItem = null;
                    }
                    break;
                case "Instagram":
                    InstagramUserListViewModel?.SelectedUsers?.Remove(InstagramUserListViewModel.SelectedUsers.FirstOrDefault(m => m.InboxUserName == tabName));
                    if (InstagramUserListViewModel.SelectedUsers.Count == 0)
                    {
                        InstagramUserListViewModel.SelectedItem = null;
                    }
                    break;

                default:
                    break;
            }
        }

        private void ImageProgressBarLoadedCommandHandler(object obj)
        {
            Image = obj as System.Windows.Controls.Image;
            Image.Visibility = Visibility.Visible;
        }

        #endregion

        #region Field

        private ObservableCollection<ImageLoginTextbox> _LoginImageInfo;

        private SocialTabViewModel _fbPageInboxmember;

        public string UrlName;

        private string _textcommet = string.Empty;

        public string DisplayProgressBarPath { get { return @"Images\GrayBar.gif"; } }
       
        #endregion

        #region Property

        #region User Info Details

        public ObservableCollection<ImageLoginTextbox> LoginImageInfo
        {
            get
            {
                return _LoginImageInfo;
            }
            set
            {
                _LoginImageInfo = value;
                OnPropertyChanged("LoginImageInfo");

            }
        }
        #endregion

        #endregion

        #region Methods

    
        private void Showimage()
        {
            Image.Visibility = Visibility.Visible;
        }

        private string fileName = Properties.Settings.Default.Filename;

        public System.Windows.Controls.Image Image { get; set; }

        private void LoginCommandHandler(object obj)
        {
            _onlineFetcher.LoginWithSelenium();
        }

        private ObservableCollection<FacebookUserLoginInfo> _loginUsersList;

        private string _messageToSend;

        private SocialTabViewModel _instaInboxmember;

        private SocialTabViewModel _messengerUserListViewModel;
        private int _tabControlSelectedIndex;

        #endregion

        public string chat { get; set; }

        public string imagesrc { get; set; }

        public string otherimagesrc { get; set; }
  
        public string MessageToSend
        {
            get {return _messageToSend;}
            set
            {
                _messageToSend = value;
                OnPropertyChanged();
            }
        }

        public int TabControlSelectedIndex
        {
            get { return _tabControlSelectedIndex; }
            set
            {
                _tabControlSelectedIndex = value;
                OnPropertyChanged();
            }
        }
    }
}
