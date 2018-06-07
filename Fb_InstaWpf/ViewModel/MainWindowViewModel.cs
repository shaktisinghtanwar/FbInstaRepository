﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Fb_InstaWpf.Helper;
using Fb_InstaWpf.Model;
using OpenQA.Selenium.Chrome;
using HtmlAgilityPack;
using System.Windows;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using Microsoft.Win32;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Fb_InstaWpf.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Commands

        public DelegateCommand SendFbCommentCommand { get; set; }
        public DelegateCommand SendMessageCommand { get; set; }
        public DelegateCommand SendMessageInstaCommand { get; set; }
        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand FbMessengerListCommand { get; set; }
        public DelegateCommand FbPageInboxCommand { get; set; }
        public DelegateCommand InstaInboxCommand { get; set; }

        public DelegateCommand SendimageCommand { get; set; }
        public DelegateCommand SendimageFBCommand { get; set; }
        public DelegateCommand TabCtrlLoaded { get; set; }
        public DelegateCommand Tab2CtrlLoaded { get; set; }
        public DelegateCommand Tab0CtrlLoaded { get; set; }
        public DelegateCommand ImageProgressBarLoaded { get; set; }


        public ObservableCollection<FacebookUserLoginInfo> LoginUsersList
        {
            get { return _loginUsersList; }
            set { _loginUsersList = value; OnPropertyChanged(); }
        }


        public FacebookUserLoginInfo SelectedMainUser
        {
            get { return _selectedMainUser; }
            set { _selectedMainUser = value; OnPropertyChanged(); }
        }


        #endregion

        DbHelper _dbHelper;
        OnlineFetcher _onlineFetcher;
        OnlinePoster _onlinePoster;


        #region Contructor
        public MainWindowViewModel()
        {
            UserMessengerTabItemList = new ObservableCollection<UserMsgTabItem>();
            UserMsgTabItemListFb = new ObservableCollection<UserMsgTabItem>();
            UserMsgTabItemListInsta = new ObservableCollection<UserMsgTabItem>();
            LoginImageInfo = new ObservableCollection<ImageLoginTextbox>();
            InstaListmembers = new ObservableCollection<InstaInboxmember>();
            FbPageListmembers = new ObservableCollection<FacebookPageInboxmember>();
            UserListInfo = new ObservableCollection<FbpageInboxUserInfo>();
            MessagingListInfo = new ObservableCollection<FbUserMessageInfo>();
            messagingFbpageListInfo = new ObservableCollection<FbUserMessageInfo>();
            messagingInstapageListInfo = new ObservableCollection<FbUserMessageInfo>();

            SendMessageCommand = new DelegateCommand(SendMessageCommandHandler, null);
            SendMessageInstaCommand = new DelegateCommand(SendMessageInstaCommandhandlar, null);
            LoginCommand = new DelegateCommand(LoginCommandHandler, null);
            FbMessengerListCommand = new DelegateCommand(FbMessengerListCommandHandler, null);
            InstaInboxCommand = new DelegateCommand(IntaInboxCommandHandler, null);
            FbPageInboxCommand = new DelegateCommand(FbPageInboxCommandHandler, null);
            SendimageCommand = new DelegateCommand(SendImageCommandHandler, null);
            SendimageFBCommand = new DelegateCommand(SendimageFBCommandHandler, null);
            SendFbCommentCommand = new DelegateCommand(SendFbCommentCommandHandler, null);
            TabCtrlLoaded = new DelegateCommand(TabCtrlLoadedCommandHandler, null);
            Tab2CtrlLoaded = new DelegateCommand(Tab2CtrlLoadedCommandHandler, null);
            Tab0CtrlLoaded = new DelegateCommand(Tab0CtrlLoadedCommandHandler, null);
            ImageProgressBarLoaded = new DelegateCommand(ImageProgressBarLoadedCommandHandler, null);
            CloseTabCommand = new DelegateCommand(CloseTab);

            CreateColumn();

            _onlineFetcher = new OnlineFetcher();
            _onlinePoster = new OnlinePoster();
            _dbHelper = new DbHelper();

            Task.Factory.StartNew(() => FillUserList());

            Messenger = "Messenger";
            Facebook = "Facebook";
            Instagram = "Instagram";

        }

        private void FillUserList()
        {
            var data = _dbHelper.GetLoginUsers();
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                LoginUsersList = data;
                SelectedMainUser = data.FirstOrDefault();
                Task.Factory.StartNew(() => ShowMessengerListData(SelectedMainUser));
            });
        }

        private void ShowMessengerListData(FacebookUserLoginInfo user)
        {
            if (user == null) return;
            var data = _dbHelper.GetFbMessengerListData(user.UserId);
            Application.Current.Dispatcher.Invoke(() =>
            {
                UserListInfo = data;
                SelectedUserInfo = data.FirstOrDefault();
            });
        }

        private void ShowFbCommentListData(FacebookUserLoginInfo user)
        {
            var data = _dbHelper.GetFacebookListData(user.UserId);
            Application.Current.Dispatcher.Invoke(() =>
            {
                FbPageListmembers = data;
                SelectedFBPageInfo = data.FirstOrDefault();
            });
        }

        private void ShowInstaListData(FacebookUserLoginInfo user)
        {
            var data = _dbHelper.GetInstaUserList(user.UserId);
            Application.Current.Dispatcher.Invoke(() =>
            {
                SelectedInstaInboxmemberInfo = data.FirstOrDefault();
                InstaListmembers = data;

            });
        }

        private void SendMessageInstaCommandhandlar(object obj)
        {
            MessagingInstapageListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = MessageToSend });

            _dbHelper.Add(new PostMessage() { FromUserId = SelectedMainUser.UserId, ToUserId = SelectedFBPageInfo.Fbcomment_InboxUserId, ImagePath = MessageToSend, MessageType = MessageType.InstaMessage });
            MessageToSend = string.Empty;
        }

        private void ImageProgressBarLoadedCommandHandler(object obj)
        {
            Image = obj as System.Windows.Controls.Image;
            Image.Visibility = Visibility.Visible;
        }

        void CreateColumn()
        {
            dtuserCredential.Columns.Add("UserName");
            dtuserCredential.Columns.Add("Password");
        }

        #endregion

        #region Field


        private ObservableCollection<ImageLoginTextbox> _LoginImageInfo;

        private ObservableCollection<FbpageInboxUserInfo> _userListInfo;

        private ObservableCollection<FacebookPageInboxmember> _fbPageInboxmember;

        private FbpageInboxUserInfo _selectedUserInfo;

        private FacebookPageInboxmember _selectedFbPageInboxmember;

        private InstaInboxmember _selectedInstaInboxmember;

        public string UrlName;

        private string _textcommet = string.Empty;
    
        public string PageSource { get; set; }

        public string FbCommentTextBxValue
        {
            get => _fbCommentTextBxValue; set
            {
                _fbCommentTextBxValue = value;
                OnPropertyChanged();
            }
        }

        public string currentURL { get; set; }
        private ObservableCollection<FbUserMessageInfo> messagingListInfo { get; set; }
        private ObservableCollection<FbUserMessageInfo> messagingFbpageListInfo { get; set; }
        private ObservableCollection<FbUserMessageInfo> messagingInstapageListInfo { get; set; }
        public string DisplayProgressBarPath { get { return @"Images\GrayBar.gif"; } }
        private readonly BackgroundWorker _worker = new BackgroundWorker();
        readonly HtmlDocument _htmlDocument = new HtmlDocument();
        readonly ChromeOptions _options = new ChromeOptions();
        private readonly Queue<string> _myqueue = new Queue<string>();
        private readonly Queue<string> _queueUrl = new Queue<string>();
        private readonly Queue<string> _queueFbCmntImgUrl = new Queue<string>();
        private readonly Queue<string> _queueInstaImgUrl = new Queue<string>();
        public List<String> LstPageUrl = new List<string>();
        public List<String> comboxList = new List<string>();

        public List<ListUsernameInfo> _MyListUsernameInfo = new List<ListUsernameInfo>();

        #endregion

        #region Property
        private ObservableCollection<UserMsgTabItem> userMsgTabItemList;
        public DelegateCommand CloseTabCommand { get; set; }

        public string Messenger { get; set; }

        public string Facebook { get; set; }

        public string Instagram { get; set; }

        private void CloseTab(object obj)
        {
            string[] array = obj.ToString().Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
            string tabName = array[0];
            string parentTabName = array[1];
            switch (parentTabName)
            {
                case "Messenger":
                    UserMessengerTabItemList.Remove(UserMessengerTabItemList.FirstOrDefault(m => m.Header == tabName));
                    break;
                case "Facebook":
                    UserMsgTabItemListFb.Remove(UserMsgTabItemListFb.FirstOrDefault(m => m.HeaderFb == tabName));
                    break;
                case "Instagram":
                    UserMsgTabItemListInsta.Remove(UserMsgTabItemListInsta.FirstOrDefault(m => m.HeaderInsta == tabName));
                    break;

                default:
                    break;
            }
        }
        public ObservableCollection<UserMsgTabItem> UserMessengerTabItemList
        {
            get
            {
                return userMsgTabItemList;
            }
            set
            {
                userMsgTabItemList = value;
                OnPropertyChanged("UserMessengerTabItemList");
            }
        }
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


        public ObservableCollection<FbpageInboxUserInfo> UserListInfo
        {

            get
            {
                if (_userListInfo == null)
                {
                    _userListInfo = new ObservableCollection<FbpageInboxUserInfo>();
                }
                return _userListInfo;
            }
            set
            {
                _userListInfo = value;
                OnPropertyChanged("UserListInfo");

            }
        }
        public FbpageInboxUserInfo SelectedUserInfo
        {

            get { return _selectedUserInfo; }
            set
            {
                _selectedUserInfo = value;
                BindUserMessage(SelectedUserInfo);
                OnPropertyChanged("SelectedUserInfo");

            }
        }

        #endregion



        #region FbPageListmembers Details 

        private ObservableCollection<UserMsgTabItem> userMsgTabItemListFb;

        public ObservableCollection<UserMsgTabItem> UserMsgTabItemListFb
        {
            get
            {
                return userMsgTabItemListFb;
            }
            set
            {
                userMsgTabItemListFb = value;
                OnPropertyChanged("UserMsgTabItemListFb");
            }
        }






        public ObservableCollection<FacebookPageInboxmember> FbPageListmembers
        {
            get { return _fbPageInboxmember; }
            set { _fbPageInboxmember = value; OnPropertyChanged(); }
        }


        public FacebookPageInboxmember SelectedFBPageInfo
        {

            get { return _selectedFbPageInboxmember; }
            set
            {
                _selectedFbPageInboxmember = value;
                BindFBPageUserMessage(SelectedFBPageInfo);
                OnPropertyChanged("SelectedFBPageInfo");

            }
        }


        #endregion

        #region InstagramInboxmembers Details
        private ObservableCollection<UserMsgTabItem> userMsgTabItemListInsta;

        public ObservableCollection<UserMsgTabItem> UserMsgTabItemListInsta
        {
            get
            {
                return userMsgTabItemListInsta;
            }
            set
            {
                userMsgTabItemListInsta = value;
                OnPropertyChanged("UserMsgTabItemListInsta");
            }
        }


        public ObservableCollection<InstaInboxmember> InstaListmembers
        {
            get { return InstaInboxmember; }
            set { InstaInboxmember = value; OnPropertyChanged(); }
        }

        public InstaInboxmember SelectedInstaInboxmemberInfo
        {

            get { return _selectedInstaInboxmember; }
            set
            {
                _selectedInstaInboxmember = value;
                BindInstaUserMessage(SelectedInstaInboxmemberInfo);
                OnPropertyChanged("SelectedInstaInboxmemberInfo");

            }
        }


        #endregion

        #region Messaging Info

        public ObservableCollection<FbUserMessageInfo> MessagingListInfo
        {

            get { return messagingListInfo; }
            set
            {
                messagingListInfo = value;
                OnPropertyChanged("MessagingListInfo");

            }
        }

        public ObservableCollection<FbUserMessageInfo> MessagingFbpageListInfo
        {

            get { return messagingFbpageListInfo; }
            set
            {
                messagingFbpageListInfo = value;
                OnPropertyChanged("MessagingFbpageListInfo");

            }
        }

        public ObservableCollection<FbUserMessageInfo> MessagingInstapageListInfo
        {

            get { return messagingInstapageListInfo; }
            set
            {
                messagingInstapageListInfo = value;
                OnPropertyChanged("MessagingInstapageListInfo");

            }
        }

        #endregion

        #endregion

        #region Methods
        private void SendimageFBCommandHandler(object obj)
        {
            _dbHelper.Add(new PostMessage()
            {
                FromUserId = SelectedMainUser.UserId,
                ToUserId = SelectedFBPageInfo.Fbcomment_InboxUserId,
                Message = MessageToSend,
                MessageType = MessageType.FacebookImage,
                // FromUserId = cmbUser.SelectedItem.ToString();

            });
            MessageToSend = string.Empty;
        }

        private void Tab0CtrlLoadedCommandHandler(object obj)
        {
            TabControl = obj as System.Windows.Controls.TabControl;
        }

        private void Tab2CtrlLoadedCommandHandler(object obj)
        {
            TabControl = obj as System.Windows.Controls.TabControl;
        }
        public System.Windows.Controls.TabControl TabControl
        {
            get;
            set;
        }
        private void TabCtrlLoadedCommandHandler(object obj)
        {
            TabControl = obj as System.Windows.Controls.TabControl;
        }

        private void SendFbCommentCommandHandler(object obj)
        {
            MessagingFbpageListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = FbCommentTextBxValue });          

            _dbHelper.Add(new PostMessage() { FromUserId = SelectedMainUser.UserId, ToUserId = SelectedFBPageInfo.Fbcomment_InboxUserId, Message = MessageToSend, MessageType = MessageType.FacebookMessage });
            MessageToSend = string.Empty;
        }

        private void SendImageCommandHandler(object obj)
        {
            string fileName = ShowDialogAndFetchFileName();

            PostMessage message = new PostMessage()
            {
                FromUserId = SelectedMainUser.UserId,
                ToUserId = SelectedFBPageInfo.Fbcomment_InboxUserId,
                MessageType = MessageType.FacebookMessengerImage,
                ImagePath = fileName,
            };
            _dbHelper.Add(message);
        }

        private static string ShowDialogAndFetchFileName()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg)|*.png|All files (*.*)|*.*";
            string fileName = string.Empty;
            if (openFileDialog.ShowDialog() == true)
            {
                fileName = openFileDialog.FileName;
            }
            else
                MessageBox.Show("Please select image");
            return fileName;
        }

        private void FbPageInboxCommandHandler(object obj)
        {
            try
            {
                Task.Factory.StartNew(() => ShowFbCommentListData(SelectedMainUser));


                // Showimage();
                TabControl.SelectedIndex = Convert.ToInt16(obj);

            }
            catch (Exception)
            {

            }
        }

        void fbTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // ShowFacebookListData();
            Application.Current.Dispatcher.Invoke(() =>
              Image.Visibility = Visibility.Hidden
            );

        }

        //private void ShowFacebookListData()
        //{
        //    FbPageListmembers = _dbHelper.GetFacebookListData();
        //}

        private void Showimage()
        {
            Image.Visibility = Visibility.Visible;
        }

        private void IntaInboxCommandHandler(object obj)
        {
            // InstaTimer

            TabControl.SelectedIndex = Convert.ToInt16(obj);
            Task.Factory.StartNew(() => ShowInstaListData(SelectedMainUser));
        }






        private void BIndFbPageData()
        {


            FbPageListmembers.Add(new FacebookPageInboxmember { FbPageName = "Facebook Page1 Post", FbPageImage = "E:\\RAHUL_WORK\\WPF_Examples\\Fb_InstaWpf12052018\\Fb_InstaWpf\\Fb_InstaWpf\\Images\\download.jpg" });
            FbPageListmembers.Add(new FacebookPageInboxmember { FbPageName = "Facebook Page2 Post", FbPageImage = "E:\\RAHUL_WORK\\WPF_Examples\\Fb_InstaWpf12052018\\Fb_InstaWpf\\Fb_InstaWpf\\Images\\download.jpg" });
        }

        private void BIndInstData()
        {
            InstaListmembers.Add(new InstaInboxmember { InstaInboxUserName = "Instagram Page1 Post", InstaInboxUserImage = "E:\\RAHUL_WORK\\WPF_Examples\\Fb_InstaWpf12052018\\Fb_InstaWpf\\Fb_InstaWpf\\Images\\download.jpg" });
            InstaListmembers.Add(new InstaInboxmember { InstaInboxUserName = "Instagram Page2 Post", InstaInboxUserImage = "E:\\RAHUL_WORK\\WPF_Examples\\Fb_InstaWpf12052018\\Fb_InstaWpf\\Fb_InstaWpf\\Images\\download.jpg" });
        }

        private string fileName = Properties.Settings.Default.Filename;
        private DataTable dtuserCredential = new DataTable();

        public System.Windows.Controls.Image Image { get; set; }

        public System.Windows.Controls.ComboBox cmbUser
        {
            get;
            set;
        }

        private void LoginCommandHandler(object obj)
        {
            _onlineFetcher.LoginWithSelenium();


        }

        SqLiteHelper sql = new SqLiteHelper();


        private void FbMessengerListCommandHandler(object obj)
        {
            TabControl.SelectedIndex = Convert.ToInt16(obj);
            //GetFbMessengerListData();
        }

        private void BindUserInfo(string userId)
        {
            //string query = "select M_InboxUserId,PlateformType,PostType,Message,ImgSource from TblJob where M_InboxUserId='" + userId + "'";

            string query = "select M_InboxUserId,PlateformType,PostType,Message,ImgSource from TblJob where M_InboxUserId='" + userId + "'";

            var dt = sql.GetDataTable(query);
            foreach (DataRow item in dt.Rows)
            {

                string inboxUserId = Convert.ToString(item["M_InboxUserId"]);
                string PlateformType = Convert.ToString(item["PlateformType"]);
                string PostType = Convert.ToString(item["PostType"]);
                string Message = Convert.ToString(item["Message"]);
                string ImgSource = Convert.ToString(item["ImgSource"]);
                if (PostType == "0")
                {
                    MessagingListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = Message, loginguserFbimage = ImgSource });
                }
                else if (PostType == "1")
                {
                    MessagingListInfo.Add(new FbUserMessageInfo { UserType = 1, Message = Message, otheruserimage = ImgSource });
                }

                MessagingListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = Message });



            }




        }

        private void BindUserMessage(FbpageInboxUserInfo fbpageInboxUserInfo)
        {
            MessagingListInfo = new ObservableCollection<FbUserMessageInfo>();
            if (!UserMessengerTabItemList.Any(m => m.Header.Equals(fbpageInboxUserInfo.InboxUserName)))
            {
                UserMessengerTabItemList.Add(new UserMsgTabItem() { Header = fbpageInboxUserInfo.InboxUserName, MessagingListInfo = MessagingListInfo });
                if (isLoggedIn)
                {
                    GetUserChatBoxHistory();
                }
                else
                {
                    BindUserInfo(fbpageInboxUserInfo.InboxUserId);
                }
            }
        }

        private void BindFBPageUserMessage(FacebookPageInboxmember selectedFBPageInfo)
        {
            try
            {
                MessagingFbpageListInfo = new ObservableCollection<FbUserMessageInfo>();
                if (!UserMsgTabItemListFb.Any(m => m.HeaderFb.Equals(selectedFBPageInfo.FbPageName)))
                {
                    UserMsgTabItemListFb.Add(new UserMsgTabItem() { HeaderFb = selectedFBPageInfo.FbPageName, MessagingListInfo = MessagingFbpageListInfo });
                    BindFbComments(selectedFBPageInfo.Fbcomment_InboxUserId);
                }
            }
            catch (Exception)
            {
            }
        }

        private void BindFbComments(string userId)
        {
            string query = "select Fbcomment_InboxUserId,PlateformType,Message,ImageSource from TblJobFb where Fbcomment_InboxUserId='" + userId + "'";

            var dt = sql.GetDataTable(query);
            foreach (DataRow item in dt.Rows)
            {

                string inboxUserId = Convert.ToString(item["Fbcomment_InboxUserId"]);
                string PlateformType = Convert.ToString(item["PlateformType"]);
                string Message = Convert.ToString(item["Message"]);
                string ImgSource = Convert.ToString(item["ImageSource"]);
                if (PlateformType == "0")
                {
                    messagingFbpageListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = Message, loginguserFbimage = ImgSource });
                }
                else if (PlateformType == "1")
                {
                    messagingFbpageListInfo.Add(new FbUserMessageInfo { UserType = 1, Message = Message, otheruserFbimage = ImgSource });
                }

                messagingFbpageListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = Message });
            }
        }

        private void BindInstaUserMessage(InstaInboxmember SelectedInstaInboxmemberInfo)
        {
            MessagingInstapageListInfo = new ObservableCollection<FbUserMessageInfo>();

            if (!UserMsgTabItemListInsta.Any(m => m.HeaderInsta.Equals(SelectedInstaInboxmemberInfo.InstaInboxUserName)))
            {
                UserMsgTabItemListInsta.Add(new UserMsgTabItem()
                {
                    HeaderInsta = SelectedInstaInboxmemberInfo.InstaInboxUserName,
                    MessagingListInfo = MessagingInstapageListInfo
                });

               // GetInstaCommenter(SelectedInstaInboxmemberInfo.Insta_inboxUserId);
            }
        }

        //private void GetInstaCommenter(string userId)
        //{
        //    GetInstagramMessages(userId);
        //}
           

        string PlateformType = "1";
        string PostType = "1";
        string Status = "1";
        bool isLoggedIn = false;
        private ObservableCollection<FacebookUserLoginInfo> _loginUsersList;
        private FacebookUserLoginInfo _selectedMainUser;
        private string _fbCommentTextBxValue;
        private string _messageToSend;
        private ObservableCollection<InstaInboxmember> _instaInboxmember;

        public void GetUserChatBoxHistory()
        {
            try
            {
                if (isLoggedIn)
                    GetUserChatBoxDataOnline();
            }
            catch (Exception)
            {
            }
        }

        private void GetUserChatBoxDataOnline()
        {
            ChromeWebDriver.Navigate().GoToUrl(SelectedUserInfo.InboxNavigationUrl);
            Thread.Sleep(2000);
            PageSource = ChromeWebDriver.PageSource;
            _htmlDocument.LoadHtml(PageSource);
            Thread.Sleep(2000);
            HtmlNodeCollection imgNode = _htmlDocument.DocumentNode.SelectNodes("//div[@class='_41ud']");

            foreach (HtmlNode htmlNodeDiv in imgNode)
            {
                currentURL = ChromeWebDriver.Url;
                var tempId = currentURL.Split('?')[1].Split('=')[1];

                HtmlNode selectSingleNode = htmlNodeDiv.SelectSingleNode(".//*[@class='clearfix _o46 _3erg _29_7 direction_ltr text_align_ltr']");

                if (selectSingleNode != null)
                {
                    string otheruser = selectSingleNode.InnerText;
                    MessagingListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = otheruser });
                    string query = "INSERT INTO TblJob(M_InboxUserId,PlateformType,PostType,Message,ImgSource,Status) values('" + tempId + "','" + PlateformType + "','" + 0 + "','" + otheruser + "','" + imagesrc + "','" + Status + "')";
                    SqLiteHelper sql = new SqLiteHelper();
                    int yy = sql.ExecuteNonQuery(query);
                }

                HtmlNode selectSingleimgNode = htmlNodeDiv.SelectSingleNode(".//*[@class='clearfix _o46 _3erg _29_7 direction_ltr text_align_ltr _ylc']");
                if (selectSingleimgNode != null)
                {
                    Regex regex = new Regex(@"src(.*?)style");
                    Match match = regex.Match(selectSingleimgNode.InnerHtml);
                    string msgId = match.Value.Replace("src=", "").Replace("style", "").Replace("\"", "").Replace(@"""", "").Replace("amp;", "");
                    MessagingListInfo.Add(new FbUserMessageInfo { UserType = 2, otheruserimage = msgId });
                }
                HtmlNode selectSingleNode2 = htmlNodeDiv.SelectSingleNode(".//*[@class='clearfix _o46 _3erg _3i_m _nd_ direction_ltr text_align_ltr']");
                if (selectSingleNode2 != null)
                {
                    string loginuser = selectSingleNode2.InnerText;
                    MessagingListInfo.Add(new FbUserMessageInfo { UserType = 1, Message = loginuser });
                }

                HtmlNode selectSingleimgRightNode = htmlNodeDiv.SelectSingleNode(".//*[@class='clearfix _o46 _3erg _3i_m _nd_ direction_ltr text_align_ltr _ylc']");
                if (selectSingleimgRightNode != null)
                {
                    Regex regex = new Regex(@"src(.*?)style");
                    Match match = regex.Match(selectSingleimgRightNode.InnerHtml);
                    string msgId = match.Value.Replace("src=", "").Replace("style", "").Replace("\"", "").Replace(@"""", "").Replace("amp;", "");
                    MessagingListInfo.Add(new FbUserMessageInfo { UserType = 3, loginguserimage = msgId });
                    string query = "INSERT INTO TblJob(M_InboxUserId,PlateformType,PostType,Message,ImgSource,Status) values('" + tempId + "','" + PlateformType + "','" + 0 + "','" + chat + "','" + imagesrc + "','" + Status + "')";
                    SqLiteHelper sql = new SqLiteHelper();
                    int yy = sql.ExecuteNonQuery(query);
                }

            }
            Thread.Sleep(1000); // 5 Minutes

            for (int i = 0; i < MessagingListInfo.Count; i++)
            {
                chat = MessagingListInfo[i].Message;
                imagesrc = MessagingListInfo[i].loginguserimage;
                otherimagesrc = MessagingListInfo[i].otheruserimage;
                currentURL = ChromeWebDriver.Url;
                var tempId = currentURL.Split('?')[1].Split('=')[1];
                // listUsernameInfo.ListUserId = tempId;
                string query1 = "select Count(*) from TblJob where Message='" + chat + "'and ImgSource='" + imagesrc + "'";
                SqLiteHelper sql1 = new SqLiteHelper();
                int count = Convert.ToInt32(sql1.ExecuteScalar(query1));

                if (count == 0)
                {
                    string query = "INSERT INTO TblJob(M_InboxUserId,PlateformType,PostType,Message,ImgSource,Status) values('" + tempId + "','" + PlateformType + "','" + PostType + "','" + chat + "','" + imagesrc + "','" + Status + "')";
                    SqLiteHelper sql = new SqLiteHelper();
                    int yy = sql.ExecuteNonQuery(query);
                }
            }
        }

        public void SendMessageCommandHandler(object j)
        {
            _dbHelper.Add(new PostMessage() { FromUserId = SelectedMainUser.UserId, ToUserId = SelectedUserInfo.InboxUserId, Message = MessageToSend, MessageType = MessageType.FacebookMessage });
            MessageToSend = string.Empty;
        }       

        #endregion

        public string chat { get; set; }

        public string imagesrc { get; set; }

        public string otherimagesrc { get; set; }

        public ChromeDriver ChromeWebDriver { get; set; }

        public ObservableCollection<InstaInboxmember> InstaInboxmember
        {
            get { return _instaInboxmember; }
            set { _instaInboxmember = value; }
        }

        public string MessageToSend {
            get => _messageToSend;
            set
            {
                _messageToSend = value;
                OnPropertyChanged();
            }
        }
    }
}
