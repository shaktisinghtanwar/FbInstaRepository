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
using Fb_InstaWpf.Enums;

namespace Fb_InstaWpf.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        DatabaseContext _databaseContext;
        SocialUser _loginUser;
        private FbPageInfo _fbPageInfo;
        DbHelper _dbHelper;
        OnlineFetcher _onlineFetcher;
        OnlinePoster _onlinePoster;

        Task _onlineFetcherGetAllPagesTask;
        Task _onlineFetcherFacebookMessengerTask;
        Task _onlineFetcherInstagramMessagesTask;
        Task _onlinePosterTask;


        #region Commands

        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand FetchAllLoggedinUserPages { get; set; }
        public DelegateCommand FbMessengerListCommand { get; set; }
        public DelegateCommand FbPageInboxCommand { get; set; }
        public DelegateCommand InstaInboxCommand { get; set; }

        public DelegateCommand TabCtrlLoaded { get; set; }
        public DelegateCommand Tab2CtrlLoaded { get; set; }
        public DelegateCommand Tab0CtrlLoaded { get; set; }
        public DelegateCommand ImageProgressBarLoaded { get; set; }

        public DelegateCommand CloseTabCommand { get; set; }

        #endregion

        public SocialUser LoginUser
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
        public FbPageInfo FbPageInfo
        {
            get { return _fbPageInfo; }
            set
            {
                if (value != null)
                {
                    _fbPageInfo = value;
                    OnPropertyChanged();
                }
            }
        }




        public ObservableCollection<SocialUser> LoginUsersList
        {
            get { return _loginUsersList; }
            set { _loginUsersList = value; OnPropertyChanged(); }
        }

        public ObservableCollection<FbPageInfo> PageList
        {
            get { return _pageList; }
            set { _pageList = value; OnPropertyChanged(); }
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
            FetchAllLoggedinUserPages = new DelegateCommand(FetchAllLoggedinUserPagesCommandHandler, null);
            FbMessengerListCommand = new DelegateCommand(LeftFbMessengerListCommandHandler, null);
            InstaInboxCommand = new DelegateCommand(LeftInstaInboxCommandHandler, null);
            FbPageInboxCommand = new DelegateCommand(LeftFbPageInboxCommandHandler, null);
       
            ImageProgressBarLoaded = new DelegateCommand(ImageProgressBarLoadedCommandHandler, null);
            CloseTabCommand = new DelegateCommand(CloseTab);

            _databaseContext = new DatabaseContext();
            _onlineFetcher = new OnlineFetcher();
            _onlinePoster = new OnlinePoster();
            _dbHelper = new DbHelper();
            _onlineFetcher.LoginSuccessEvent += _onlineFetcher_LoginSuccessEvent;
            Task.Factory.StartNew(() => FillLoginUserList());
            Task.Factory.StartNew(() => FillPageList());

        }

        private void FetchAllLoggedinUserPagesCommandHandler(object obj)
        {
            //GetAllPaLoggedinUserPages
            _onlineFetcherGetAllPagesTask = Task.Factory.StartNew(() => _onlineFetcher.GetAllPaLoggedinUserPages(LoginUser.InboxUserName));
        }

        private void _onlineFetcher_LoginSuccessEvent()
        {
            _onlineFetcherGetAllPagesTask = Task.Factory.StartNew(() => _onlineFetcher.GetFacebookMessages());
            _onlineFetcherInstagramMessagesTask = Task.Factory.StartNew(() => _onlineFetcher.GetInstaMesages());
            _onlineFetcherFacebookMessengerTask = Task.Factory.StartNew(() => _onlineFetcher.GetFbMessengerMessages());
            _onlinePosterTask = Task.Factory.StartNew(() => _onlinePoster.ProcessMessage());
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

        private void FillPageList()
        {
            var data = _dbHelper.GetFacebookPage();
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                PageList = data;
                FbPageInfo = data.FirstOrDefault();
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
            if (MessengerUserListViewModel != null) return;
            var data = _dbHelper.GetLeftMessengerListData(LoginUser.InboxUserId, TabType.Messenger);

            Application.Current.Dispatcher.Invoke(() =>
            {
                MessengerUserListViewModel = MessengerUserListViewModel ?? new SocialTabViewModel(Enums.TabType.Messenger,LoginUser)
                {
                    UserListInfo = data
                };
                if (MessengerUserListViewModel.SelectedItem == null)
                    MessengerUserListViewModel.SelectedItem = data.FirstOrDefault();
            });
        }

        private void LeftFacebookData()
        {
            if (FacebookUserListViewModel != null) return;

            var data = _dbHelper.GetLeftMessengerListData(LoginUser.InboxUserId, TabType.Facebook);
           // var data = _dbHelper.GetFacebookListData(LoginUser.UserId);
            Application.Current.Dispatcher.Invoke(() =>
            {
                FacebookUserListViewModel = FacebookUserListViewModel ?? new SocialTabViewModel(Enums.TabType.Facebook,LoginUser)
                {
                    UserListInfo = data
                };
                if (FacebookUserListViewModel.SelectedItem == null)
                    FacebookUserListViewModel.SelectedItem = data.FirstOrDefault();
            });
        }

        private void LeftInstagramData()
        {
            if (InstagramUserListViewModel != null) return;

            //var data = _dbHelper.GetInstaUserList(LoginUser.UserId);
            var data = _dbHelper.GetLeftMessengerListData(LoginUser.InboxUserId, TabType.Instagram);
            Application.Current.Dispatcher.Invoke(() =>
            {
                InstagramUserListViewModel = InstagramUserListViewModel ?? new SocialTabViewModel(Enums.TabType.Instagram,LoginUser)
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
            _onlineFetcher.LoginWithSelenium(LoginUser.InboxUserName,LoginUser.Password);
       }

        private ObservableCollection<SocialUser> _loginUsersList;


        private string _messageToSend;

        private SocialTabViewModel _instaInboxmember;

        private SocialTabViewModel _messengerUserListViewModel;
        private int _tabControlSelectedIndex;
        private ObservableCollection<FbPageInfo> _pageList;
  
            

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
