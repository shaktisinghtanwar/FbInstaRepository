using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using Fb_InstaWpf.Helper;
using Fb_InstaWpf.Model;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using HtmlAgilityPack;
using System.IO;
using System.Windows;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using Keys = OpenQA.Selenium.Keys;
using Microsoft.Win32;

namespace Fb_InstaWpf.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Commands

        public DelegateCommand SendFbCommentCommand { get; set; }
        public DelegateCommand SendMessageCommand { get; set; }
        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand FbMessengerListCommand { get; set; }
        public DelegateCommand FbPageInboxCommand { get; set; }
        public DelegateCommand IntaInboxCommand { get; set; }

        public DelegateCommand SendimageCommand { get; set; }
        public DelegateCommand SendimageFBCommand { get; set; }
        public DelegateCommand TabCtrlLoaded { get; set; }
        public DelegateCommand Tab2CtrlLoaded { get; set; }
        public DelegateCommand Tab0CtrlLoaded { get; set; }
        public DelegateCommand cmbUserLoaded { get; set; }
        public DelegateCommand ImageProgressBarLoaded { get; set; }

        #endregion
        
        private System.Timers.Timer timer;
        private System.Timers.Timer fbTimer;
        private System.Timers.Timer InstaTimer;
        //Thread printer;

        #region Contructor
        public MainWindowViewModel()
        {
            UserMessengerTabItemList = new ObservableCollection<UserMsgTabItem>();
            UserMsgTabItemListFb = new ObservableCollection<UserMsgTabItem>();
             UserMsgTabItemListInsta=new ObservableCollection<UserMsgTabItem>();
            //UserMsgTabItemListInsta = new ObservableCollection<UserInstaMsgTabItem>();
            LoginImageInfo = new ObservableCollection<ImageLoginTextbox>();
            InstaListmembers = new ObservableCollection<InstaInboxmember>();
            FbPageListmembers = new ObservableCollection<FacebookPageInboxmember>();
            //FbPageInboxmember = new ObservableCollection<FacebookPageInboxmember>();
            UserListInfo = new ObservableCollection<FbpageInboxUserInfo>();
            MessagingListInfo = new ObservableCollection<FbUserMessageInfo>();
            messagingFbpageListInfo = new ObservableCollection<FbUserMessageInfo>();
            messagingInstapageListInfo = new ObservableCollection<FbUserMessageInfo>();

            SendMessageCommand = new DelegateCommand(SendMessageCommandHandler, null);
            LoginCommand = new DelegateCommand(LoginCommandHandler, null);
            FbMessengerListCommand = new DelegateCommand(FbMessengerListCommandHandler, null);
            IntaInboxCommand = new DelegateCommand(IntaInboxCommandHandler, null);
            FbPageInboxCommand = new DelegateCommand(FbPageInboxCommandHandler, null);
            SendimageCommand = new DelegateCommand(SendImageCommandHandler, null);
            SendimageFBCommand = new DelegateCommand(SendimageFBCommandHandler, null);
            //
            SendFbCommentCommand = new DelegateCommand(SendFbCommentCommandHandler, null);
            TabCtrlLoaded = new DelegateCommand(TabCtrlLoadedCommandHandler, null);
            Tab2CtrlLoaded = new DelegateCommand(Tab2CtrlLoadedCommandHandler, null);
            Tab0CtrlLoaded = new DelegateCommand(Tab0CtrlLoadedCommandHandler, null);
            cmbUserLoaded = new DelegateCommand(cmbUserLoadedHandler, null);
            ImageProgressBarLoaded = new DelegateCommand(ImageProgressBarLoadedCommandHandler, null);
              CloseTabCommand = new DelegateCommand(CloseTab);

            CreateColumn();
            ShowMessengerListData();
            //ShowFacebookListData();

            timer = new System.Timers.Timer(1000); //it will run every one second
            fbTimer = new System.Timers.Timer(1000);
            InstaTimer = new System.Timers.Timer(1000);

            //printer = new Thread(new ThreadStart(GetUserChatBoxHistory));
            //printer.Start();
            }

     

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            ShowMessengerListData();
        }
        
        private void ShowMessengerListData()
         {

             string query = "select M_InboxUserId,M_inboxUserName,M_InboxUserImage,M_InboxNavigationUrl from TblMessengerList";
            var dt = sql.GetDataTable(query);
            foreach (DataRow item in dt.Rows)
            {

                string inboxUserId = Convert.ToString(item["M_InboxUserId"]);
                string inboxUserName = Convert.ToString(item["M_inboxUserName"]);
                string inboxUserImage = Convert.ToString(item["M_InboxUserImage"]);
                string inboxNavigationUrl = Convert.ToString(item["M_InboxNavigationUrl"]);
                //if (!UserListInfo.Any(m => m.InboxUserName.Equals(M_inboxUserName)))
                if (!UserListInfo.Any(m => m.InboxUserName.Equals(inboxUserName)))
                {
                    UserListInfo.Add(new FbpageInboxUserInfo() { InboxUserId = inboxUserId, InboxUserName = inboxUserName, InboxUserImage = inboxUserImage, InboxNavigationUrl = inboxNavigationUrl });
                   
                }
            }
        }





        private void ImageProgressBarLoadedCommandHandler(object obj)
        {
            Image = obj as System.Windows.Controls.Image;
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
        private ObservableCollection<InstaInboxmember> _instaInboxmember;
        private FbpageInboxUserInfo _selectedUserInfo;
        private FacebookPageInboxmember _selectedFbPageInboxmember;
        private InstaInboxmember _selectedInstaInboxmember;
        private FacebookPageInboxmember SelectedFbPageInboxmember;
        
        private int _messageIdount;
        public string UrlName;
        private string _textcommet = string.Empty;
        public string LstItemUserName { get; set; }
        public string TextBxValue { get; set; }
        public string Getimgurl { get; set; }
        public string TemppageNodeItem { get; set; }
        public string DataImg { get; set; }
        public string PageSource { get; set; }
        public string LstfbItemUserName { get; set; }
        public string FbCommentTextBxValue { get; set; }
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
        private readonly Queue<string> _queueFbImgUrl = new Queue<string>();
        public List<String> LstPageUrl = new List<string>();
        public List<String> comboxList = new List<string>();

        public List<ListUsernameInfo> _MyListUsernameInfo = new List<ListUsernameInfo>();

        // private string sqliteDatabase = @"Data Source=FbInstaCommentDb.s3db;";

        #endregion

        #region Property
        private ObservableCollection<UserMsgTabItem> userMsgTabItemList;
        public DelegateCommand CloseTabCommand { get; set; }

        private void CloseTab(object obj)
        {
            string tabName = obj.ToString();
            UserMessengerTabItemList.Remove(UserMessengerTabItemList.FirstOrDefault(m => m.Header == tabName));
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
            set { _fbPageInboxmember = value; }
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
            get { return _instaInboxmember; }
            set { _instaInboxmember = value; }
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
            OnlinePoster.Add(new PostMessage()
            {
                Message = TextBxValue,
                MessageType = MessageType.FacebookImage,
               // FromUserId = cmbUser.SelectedItem.ToString();
               
            })
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
            // string url = "https://www.facebook.com/TP-1996120520653285/inbox/?selected_item_id=1996233970641940";
            //    ChromeWebDriver.Navigate().GoToUrl(url);
            Thread.Sleep(2000);
            ReadOnlyCollection<IWebElement> postcomment = ChromeWebDriver.FindElements(By.XPath("//*[@class='UFICommentContainer']"));
            if (postcomment.Count > 0)
            {
                postcomment[0].Click();
                ReadOnlyCollection<IWebElement> postcomghghment = ChromeWebDriver.FindElements(By.XPath("//*[@class='notranslate _5rpu']"));
                if (postcomghghment.Count > 0)
                {
                    postcomghghment[0].SendKeys(FbCommentTextBxValue);
                    Thread.Sleep(1000);
                    postcomghghment[0].SendKeys(OpenQA.Selenium.Keys.Enter);
                }

            }
        }

        private void SendImageCommandHandler(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg)|*.png|All files (*.*)|*.*";
            string fileName = string.Empty;
            if (openFileDialog.ShowDialog() == true)
            {
                fileName = openFileDialog.FileName;
            }
            else
                System.Windows.MessageBox.Show("Please select image");

            PostMessage message = new PostMessage()
            {
                MessageType = MessageType.FacebookMessengerImage,
                ImagePath = fileName,
            };
            OnlinePoster.Add(message);
        }

        private void FbPageInboxCommandHandler(object obj)
        {
            try
            {
                ShowFacebookListData();

                fbTimer.Elapsed += fbTimer_Elapsed;
                fbTimer.Start();
               // Showimage();
                TabControl.SelectedIndex = Convert.ToInt16(obj);
               
            }
            catch (Exception)
            {

                ;
            }
        }

        void fbTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ShowFacebookListData();
            Application.Current.Dispatcher.Invoke(()=>
              Image.Visibility = Visibility.Hidden
            );
          
        }

        private void ShowFacebookListData()
        {
            FbPageListmembers = DbHelper.ShowFacebookListData();
            //string query = "select Fbcomment_InboxUserName,Fbcomment_InboxUserImage,FBInboxNavigationUrl from TblFbComment";
            //var dt = sql.GetDataTable(query);
            //foreach (DataRow item in dt.Rows)
            //{
            //    string Fbcomment_InboxUserName = Convert.ToString(item["Fbcomment_InboxUserName"]);
            //    string Fbcomment_InboxUserImage = Convert.ToString(item["Fbcomment_InboxUserImage"]);
            //    string FBInboxNavigationUrl = Convert.ToString(item["FBInboxNavigationUrl"]);
            //    if (!FbPageListmembers.Any(m => m.FbPageName.Equals(Fbcomment_InboxUserName)))
            //    {
            //        FbPageListmembers.Add(new FacebookPageInboxmember()
            //        {
            //            FbPageName = Fbcomment_InboxUserName,
            //            FbPageImage = Fbcomment_InboxUserImage,
            //            FBInboxNavigationUrl = FBInboxNavigationUrl
            //        });
            //    }
            //}
        }

        private void Showimage()
        {
            Image.Visibility = Visibility.Visible;
        }

        private void IntaInboxCommandHandler(object obj)
        {
            // InstaTimer
            InstaTimer.Elapsed += InstaTimer_Elapsed;
            InstaTimer.Start();
            TabControl.SelectedIndex = Convert.ToInt16(obj);
            ListUsernameInfo listUsernameInfo = new ListUsernameInfo();
            // string commentinsta = (new TextRange(RichTextBoxinsta.Document.ContentStart, RichTextBoxinsta.Document.ContentEnd).Text).Trim();
            string url = "https://www.facebook.com/TP-1996120520653285/inbox/";
            ChromeWebDriver.Navigate().GoToUrl(url);
            Thread.Sleep(3000);

            ReadOnlyCollection<IWebElement> collection = ChromeWebDriver.FindElements(By.ClassName("_32wr"));
            {
                if (collection.Count > 0)
                {
                    collection[2].Click();
                    Thread.Sleep(3000);
                }
            }

            ReadOnlyCollection<IWebElement> commentpostImgNodCollection = ChromeWebDriver.FindElements(By.XPath(".//*[@class='_11eg _5aj7']/div/div/img"));
            if (commentpostImgNodCollection.Count > 0)
            {
                for (int i = 0; i < commentpostImgNodCollection.Count; i++)
                {
                    DataImg = commentpostImgNodCollection[i].GetAttribute("src");
                    _queueInstaImgUrl.Enqueue(DataImg);
                }
            }

            ReadOnlyCollection<IWebElement> userlistnode = ChromeWebDriver.FindElements(By.ClassName("_4k8x"));
            if (userlistnode.Count > 0)
            {
                foreach (var itemurl in userlistnode)
                {
                    itemurl.Click();
                    Thread.Sleep(3000);
                    string userName = itemurl.Text;
                    listUsernameInfo.ListUsername = userName;
                    #region Rahul
                    //listUsernameInfo.ListUsername;
                    currentURL = ChromeWebDriver.Url;
                    var tempId = currentURL.Split('?')[1].Split('=')[1];
                    listUsernameInfo.ListUserId = tempId;
                    listUsernameInfo.InboxNavigationUrl = currentURL;
                    // _listUsernameInfo.ListUsername=LstItemUserName;
                    _MyListUsernameInfo.Add(listUsernameInfo);

                    var imgUrl = _queueInstaImgUrl.Dequeue();
                    string query = "INSERT INTO Tbl_Instagram(Insta_inboxUserName,Insta_inboxUserImage,InstaInboxNavigationUrl,Status) values('" + userName + "','" + imgUrl + "','" + currentURL + "','" + false + "')";

                    int yy = sql.ExecuteNonQuery(query);

                    //InstaListmembers.Add(new InstaInboxmember()
                    //{
                    //    InstaInboxUserName = userName,
                    //    InstaInboxUserImage = imgUrl,
                    //    InstaInboxNavigationUrl = currentURL
                    //});
                    #endregion
                }
            }



            //ReadOnlyCollection<IWebElement> commentpostNameCollection = ChromeWebDriver.FindElements(By.XPath(".//*[@class='_284g _4bl9']/div/div/span"));
            //if (commentpostNameCollection.Count>0)
            //{
            //    for (int i = 0; i < commentpostNameCollection.Count; i++)
            //    {
            //        if (i % 2 == 0)
            //        {
            //            string dataName = commentpostNameCollection[i].Text;
            //            InstaListmembers.Add(new InstaInboxmember { InstaInboxUserImage = _queueInstaImgUrl.Dequeue(), InstaInboxUserName = dataName });
            //        }

            //    }
            //}
            //
        }

        void InstaTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ShowInstaUserList();
        }

        private void ShowInstaUserList()
        {
            string query = "select Insta_inboxUserName,Insta_inboxUserImage,InstaInboxNavigationUrl from Tbl_Instagram";
            var dt = sql.GetDataTable(query);
            foreach (DataRow item in dt.Rows)
            {
                string Insta_inboxUserName = Convert.ToString(item["Insta_inboxUserName"]);
                string Insta_inboxUserImage = Convert.ToString(item["Insta_inboxUserImage"]);
                string InstaInboxNavigationUrl = Convert.ToString(item["InstaInboxNavigationUrl"]);
                if (!InstaListmembers.Any(m => m.InstaInboxUserName.Equals(Insta_inboxUserName)))
                {
                    InstaListmembers.Add(new InstaInboxmember()
                    {
                        InstaInboxUserName = Insta_inboxUserName,
                        InstaInboxUserImage = Insta_inboxUserImage,
                        InstaInboxNavigationUrl = InstaInboxNavigationUrl
                    });
                }
            }
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

        private void cmbUserLoadedHandler(object obj)
        {
            cmbUser = obj as System.Windows.Controls.ComboBox;
            BindComboBox();

        }

        private string fileName = Properties.Settings.Default.Filename;
        private DataTable dtuserCredential = new DataTable();
        void BindComboBox()
        {

            if (File.Exists(fileName))
            {
                //File.Delete(fileName);
                var file = File.ReadAllLines(fileName);
                dtuserCredential.Clear();
                DataRow dr = dtuserCredential.NewRow();
                dr["Password"] = 0;
                dr["UserName"] = "--Select--";
                dtuserCredential.Rows.InsertAt(dr, 0);
                //ds.Tables[0].Rows.InsertAt(rowSelect, 0)
                foreach (string s in file)
                {
                    string[] splitedItems = s.Split(':');
                    if (splitedItems.Length > 0)
                    {
                        if (splitedItems.Length > 1)
                        {
                            dr = dtuserCredential.NewRow();
                            dr["UserName"] = splitedItems[0];
                            dr["Password"] = splitedItems[1];

                            dtuserCredential.Rows.Add(dr);
                        }

                    }

                }
                cmbUser.ItemsSource = dtuserCredential.DefaultView;
                cmbUser.DisplayMemberPath = Convert.ToString(dtuserCredential.Columns["UserName"]);
                cmbUser.SelectedValuePath = Convert.ToString(dtuserCredential.Columns["Password"]);
            }
        }

        public System.Windows.Controls.Image Image { get; set; }

        public System.Windows.Controls.ComboBox cmbUser
        {
            get;
            set;
        }

        private void LoginCommandHandler(object obj)
        {
            try
            {
                Image.Visibility = Visibility.Visible;
                FileOperation.UserName = cmbUser.Text;
                FileOperation.Password = Convert.ToString(cmbUser.SelectedValue);
                // FacebookUserLoginInfo facebookUserLoginInfo=new FacebookUserLoginInfo();
                string appStartupPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                // string appStartupPath = Path.Combine(Environment.CurrentDirectory);
                const string url = "https://en-gb.facebook.com/login/";
                _options.AddArgument("--disable-notifications");
                _options.AddArgument("--disable-extensions");
                _options.AddArgument("--test-type");
                //options.AddArgument("--headless");
                _options.AddArgument("--log-level=3");
                ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService(appStartupPath);
                chromeDriverService.HideCommandPromptWindow = true;
                ChromeWebDriver = new ChromeDriver(chromeDriverService, _options);
                ChromeWebDriver.Manage().Window.Maximize();
                ChromeWebDriver.Navigate().GoToUrl(url);
                try
                {
                    ((IJavaScriptExecutor)ChromeWebDriver).ExecuteScript("window.onbeforeunload = function(e){};");
                }
                catch (Exception)
                {

                }

                ReadOnlyCollection<IWebElement> emailElement = ChromeWebDriver.FindElements(By.Id("email"));
                if (emailElement.Count > 0)
                {

                    //emailElement[0].SendKeys("rishusingh77777@gmail.com");

                    emailElement[0].SendKeys(FileOperation.UserName);

                    //CurrentLogedInFacebookUserinfo.Username = facebookUserinfo.Username
                }
                ReadOnlyCollection<IWebElement> passwordElement = ChromeWebDriver.FindElements(By.Id("pass"));
                if (passwordElement.Count > 0)
                {
                    //passwordElement[0].SendKeys("1234567#rk");
                    passwordElement[0].SendKeys(FileOperation.Password);

                }


                ReadOnlyCollection<IWebElement> signInElement = ChromeWebDriver.FindElements(By.Id("loginbutton"));
                if (signInElement.Count > 0)
                {
                    signInElement[0].Click();
                    Thread.Sleep(3000);
                    ChromeWebDriver.Navigate().GoToUrl("https://www.facebook.com/pages/?category=your_pages");
                    // ChromeWebDriver.Navigate().GoToUrl(url);
                    Thread.Sleep(2000);
                    //ChromeWebDriver.Navigate().GoToUrl("https://www.facebook.com/TP-1996120520653285/inbox/?selected_item_id=100002948674558");
                    ReadOnlyCollection<IWebElement> pageNodeImgUrl = ChromeWebDriver.FindElements((By.XPath("//*[@class='clearfix _1vh8']/a")));
                    if (pageNodeImgUrl.Count > 0)
                    {
                        foreach (var pageNodeItem in pageNodeImgUrl)
                        {
                            TemppageNodeItem = pageNodeItem.GetAttribute("href");
                            LstPageUrl.Add(TemppageNodeItem);

                            _queueFbCmntImgUrl.Enqueue(TemppageNodeItem);
                        }

                        // for (int i = 0; i < LstPageUrl.Count; i++)
                        // {
                        ChromeWebDriver.Navigate().GoToUrl(new Uri(LstPageUrl[2]));
                        Thread.Sleep(2000);

                        ReadOnlyCollection<IWebElement> collection1 = ChromeWebDriver.FindElements(By.XPath("//*[@id='u_0_u']/div/div/div[1]/ul/li[2]/a"));
                        if (collection1.Count > 0)
                        {
                            collection1[0].Click();
                        }


                    }

                    Thread.Sleep(5000);
                    ShowMessengerListData();
                    GetFbMessengerListData();
                    Thread.Sleep(2000);
                    MessageBox.Show("Login Successful.......!");
                    isLoggedIn = true;
                    Thread.Sleep(2000);

                    Image.Visibility = Visibility.Hidden;
                }

            }
            catch (Exception ex)
            {

                //;
            }
        }
        SqLiteHelper sql = new SqLiteHelper();
        #region GetFbMessengerListData()
        private void GetFbMessengerListData()
        {
            try
            {
                timer.Elapsed += dispatcherTimer_Tick;
                timer.Start();

                ListUsernameInfo listUsernameInfo = new ListUsernameInfo();
                PageSource = ChromeWebDriver.PageSource;
                Thread.Sleep(3000);
                _htmlDocument.LoadHtml(PageSource);
                Thread.Sleep(3000);
                HtmlNodeCollection imgNode = _htmlDocument.DocumentNode.SelectNodes("//*[@id='u_0_t']/div/div/div/table/tbody/tr/td[1]/div/div[2]/div/div[1]/div/div/div/div/div/div/img");
                if (imgNode != null)
                {
                    foreach (var imgNodeItem in imgNode)
                    {
                        Getimgurl = imgNodeItem.Attributes["src"].Value.Replace(";", "&");
                        _myqueue.Enqueue(Getimgurl);
                    }
                }
                var listNodeElements = _htmlDocument.DocumentNode.SelectNodes("//div[@class='_4ik4 _4ik5']");



                ReadOnlyCollection<IWebElement> userlistnode = ChromeWebDriver.FindElements(By.ClassName("_4k8x"));
                if (userlistnode.Count > 0)
                {
                    foreach (var itemurl in userlistnode)
                    {
                        Thread.Sleep(3000);
                        itemurl.Click();
                        Thread.Sleep(3000);
                        string userName = itemurl.Text;
                        listUsernameInfo.ListUsername = userName;
                        #region Rahul
                        //listUsernameInfo.ListUsername;
                        currentURL = ChromeWebDriver.Url;
                        var tempId = currentURL.Split('?')[1].Split('=')[1];
                        listUsernameInfo.ListUserId = tempId;
                        listUsernameInfo.InboxNavigationUrl = currentURL;
                        // _listUsernameInfo.ListUsername=LstItemUserName;
                        _MyListUsernameInfo.Add(listUsernameInfo);

                        var imgUrl = _myqueue.Dequeue();
                        Thread.Sleep(3000);


                        //if ()
                        //{
                       
                            //}
                          
                        //if (!UserListInfo.Any(m => m.InboxUserId.Equals(listUsernameInfo.ListUserId)))
                        //    {
                                string query =
                                    "INSERT INTO TblMessengerList(M_InboxUserId,M_inboxUserName,M_InboxUserImage,M_InboxNavigationUrl,Status) values('" +
                                    listUsernameInfo.ListUserId + "','" + userName + "','" + imgUrl + "','" +
                                    listUsernameInfo.InboxNavigationUrl + "','" + false + "')";
                                int yy = sql.ExecuteNonQuery(query);
                                //UserListInfo.Add(new FbpageInboxUserInfo()
                                //{
                                //    InboxUserName = userName,
                                //    InboxUserImage = imgUrl,
                                //    InboxNavigationUrl = currentURL
                                //});
                           // }
                        

                        #endregion
                    }
                }
                HtmlNodeCollection loginimgNode = _htmlDocument.DocumentNode.SelectNodes("//div[@class='_4u-c _5n4j']");

                foreach (HtmlNode htmlNodeimg in loginimgNode)
                {
                    HtmlNode selectSingleNode = htmlNodeimg.SelectSingleNode(".//*[@class='_1cjc img']");

                    if (selectSingleNode != null)
                    {
                        var loginuserimage = selectSingleNode.Attributes["src"].Value.Replace(";", "&");
                        LoginImageInfo.Add(new ImageLoginTextbox() { loginimageurl = loginuserimage });
                    }
                }
            }
            catch (Exception)
            {


            }

        }
        #endregion

        private void FbMessengerListCommandHandler(object obj)
        {
            TabControl.SelectedIndex = Convert.ToInt16(obj);
            GetFbMessengerListData();
        }

        private void BindUserInfo(string userId)
        {
            //UserListInfo.Add(new FbpageInboxUserInfo { InboxUserName = "rahul baba" });
            //UserListInfo.Add(new FbpageInboxUserInfo { InboxUserName = "YoYO baba" });
            //UserListInfo.Add(new FbpageInboxUserInfo { InboxUserName = "Tiger baba" });


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
                    MessagingListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = Message,loginguserFbimage= ImgSource });
                }
                else if (PostType == "1")
                {
                    MessagingListInfo.Add(new FbUserMessageInfo { UserType = 1, Message = Message,otheruserimage= ImgSource });
                }
                  
             
            }




        }

        private void BindUserMessage(FbpageInboxUserInfo fbpageInboxUserInfo)
        {
            //Data Retrive
            MessagingListInfo = new ObservableCollection<FbUserMessageInfo>();
            if (!UserMessengerTabItemList.Any(m => m.Header.Equals(fbpageInboxUserInfo.InboxUserName)))
            {
                UserMessengerTabItemList.Add(new UserMsgTabItem() { Header = fbpageInboxUserInfo.InboxUserName, MessagingListInfo = MessagingListInfo });
               //printer.Start();
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
            //Data Retrive
            try
            {
                MessagingFbpageListInfo = new ObservableCollection<FbUserMessageInfo>();
                if (!UserMsgTabItemListFb.Any(m => m.HeaderFb.Equals(selectedFBPageInfo.FbPageName)))
                {
                    UserMsgTabItemListFb.Add(new UserMsgTabItem() { HeaderFb = selectedFBPageInfo.FbPageName, MessagingListInfo = MessagingFbpageListInfo });
                    //if (isLoggedIn)
                    //{
                    GetFacebookCommenter();
                    //}
                    //else
                    //{
                    //    BindFbComments();
                    //}
                }

            }
            catch (Exception)
            {

                ;
            }


        }

        private void BindFbComments()
        {
            throw new NotImplementedException();
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

                GetInstaCommenter();
            }
        }


        public string msgId { get; set; }
        string PlateformType = "1";
        string PostType = "1";
        string ImgSource = "1";
        string Status = "1";
        bool isLoggedIn = false;

        public void GetUserChatBoxHistory()
        {
            try
            {
                ShowMessengerListData();
                if ( isLoggedIn)
                    GetUserChatBoxDataOnline();
            }
            catch (Exception)
            {
                ;
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
                timer.Elapsed += TimeRefreshchatBox_Tick;
                timer.Start();
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

        private void TimeRefreshchatBox_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            string query1 = "select Count(*) from TblJob where Message='" + chat + "'and ImgSource='" + imagesrc + "'";
            sql.ExecuteScalar(query1);
        }

        private void GetFacebookCommenter()
        {
            try
            {
                ChromeWebDriver.Navigate().GoToUrl(SelectedFBPageInfo.FBInboxNavigationUrl);
                Thread.Sleep(3000);
                PageSource = ChromeWebDriver.PageSource;
                _htmlDocument.LoadHtml(PageSource);
                Thread.Sleep(3000);
                HtmlNodeCollection commentNode = _htmlDocument.DocumentNode.SelectNodes("//div[@class='_5v3q _5jmm _5pat _11m5']");

                foreach (HtmlNode htmlcommentNode in commentNode)
                {
                    HtmlNode selectNode = htmlcommentNode.SelectSingleNode("//div[@class='_4vv0 _3ccb']");
                    var pagename = selectNode.InnerText;
                    MessagingFbpageListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = pagename });

                    HtmlNode pageimg = htmlcommentNode.SelectSingleNode("//img[@class='scaledImageFitWidth img']");

                    var imgsrc = pageimg.Attributes["src"].Value.Replace(";", "&");
                    MessagingFbpageListInfo.Add(new FbUserMessageInfo { UserType = 3, loginguserFbimage = imgsrc });
                }

                HtmlNodeCollection commentBlock = _htmlDocument.DocumentNode.SelectNodes("//div[@class='UFICommentContent']");
                var commentImg = string.Empty;
                foreach (HtmlNode commentitem in commentBlock)
                {
                    var pagenamee = commentitem.InnerText;
                    var comment = pagenamee.Replace("Manage", "");

                    MessagingFbpageListInfo.Add(new FbUserMessageInfo { UserType = 1, Message = comment });


                    Regex regex = new Regex(@"src(.*?)alt");
                    Match match = regex.Match(commentitem.InnerHtml);
                    if (match.Length != 0)
                    {
                        string[] msgId = match.Value.Replace(";", "&").Split('"');
                        var img = msgId[1];

                        MessagingFbpageListInfo.Add(new FbUserMessageInfo { UserType = 2, otheruserFbimage = img });
                    }
                }
                
            }
            catch (Exception)
            {

                ;
            }
            for (int i = 0; i < MessagingFbpageListInfo.Count; i++)
            {
                chatFb = MessagingFbpageListInfo[i].Message;
                imagesrcFb = MessagingFbpageListInfo[i].loginguserimage;
                var otherimagesrc = MessagingFbpageListInfo[i].otheruserimage;

                string query1 = "select Count(*) from TblJobFb where Message='" + chatFb + "'and ImageSource='" + imagesrcFb + "'";
                SqLiteHelper sql1 = new SqLiteHelper();
                int count = Convert.ToInt32(sql1.ExecuteScalar(query1));

                if (count == 0)
                {
                    string query = "INSERT INTO TblJobFb(PlateformType,Message,ImageSource) values('" + PlateformType + "','" + chatFb + "','" + imagesrcFb + "')";
                    SqLiteHelper sql = new SqLiteHelper();
                    int yy = sql.ExecuteNonQuery(query);
                }
            }


        }

        private void GetInstaCommenter()
        {
            try
            {
                ChromeWebDriver.Navigate().GoToUrl(SelectedInstaInboxmemberInfo.InstaInboxNavigationUrl);
                Thread.Sleep(3000);
                PageSource = ChromeWebDriver.PageSource;
                _htmlDocument.LoadHtml(PageSource);
                Thread.Sleep(2000);
                HtmlNodeCollection commentNode = _htmlDocument.DocumentNode.SelectNodes("//div[@class='_4cye _4-u2  _4-u8']");

                foreach (HtmlNode htmlcommentNode in commentNode)
                {
                    HtmlNode selectNode = htmlcommentNode.SelectSingleNode("//div[@class='_4cyh']");
                    var pagename = selectNode.InnerText;
                    MessagingInstapageListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = pagename });
                    HtmlNode pageimg = htmlcommentNode.SelectSingleNode("//img[@class='img']");
                    var imgsrc = pageimg.Attributes["src"].Value.Replace(";", "&");

                    MessagingInstapageListInfo.Add(new FbUserMessageInfo { UserType = 2, loginguserInstaimage = imgsrc });
                }

                HtmlNodeCollection commentBlock = _htmlDocument.DocumentNode.SelectNodes("//div[@class='_3i4z _4-u2  _4-u8']");

                foreach (HtmlNode commentitem in commentBlock)
                {
                    string pagenamee = commentitem.InnerText;
                    MessagingInstapageListInfo.Add(new FbUserMessageInfo { UserType = 1, Message = pagenamee });

                    Regex regex = new Regex(@"src(.*?)alt");
                    Match match = regex.Match(commentitem.InnerHtml);

                    if (match.Length != 0)
                    {
                        string[] msgId = match.Value.Split('"');

                    }

                }

            }
            catch (Exception)
            {

                ;
            }
            for (int i = 0; i < MessagingInstapageListInfo.Count; i++)
            {
                chatInsta = MessagingInstapageListInfo[i].Message;
                imagesrcInsta = MessagingInstapageListInfo[i].loginguserimage;
                var otherimagesrcInsta = MessagingInstapageListInfo[i].otheruserimage;

                string query1 = "select Count(*) from TblJobInsta where Message='" + chatInsta + "'and ImageSource='" + imagesrc + "'";
                SqLiteHelper sql1 = new SqLiteHelper();
                int count = Convert.ToInt32(sql1.ExecuteScalar(query1));

                if (count == 0)
                {
                    string query = "INSERT INTO TblJobInsta(PlateformType,Message,ImageSource) values('" + PlateformType + "','" + chatInsta + "','" + imagesrc + "')";
                    SqLiteHelper sql = new SqLiteHelper();
                    int yy = sql.ExecuteNonQuery(query);
                }
            }

        }



        public void SendMessageCommandHandler(object j)
        {
            //_worker.DoWork += worker_DoWork;
            //_worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            //_worker.RunWorkerAsync();


            MessagingListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = TextBxValue });

            //MessagingListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = textBxValue });
            // string textcommet = (new TextRange(RichTextBoxmsngr.Document.ContentStart, RichTextBoxmsngr.Document.ContentEnd).Text).Trim();
            //string textcommet = TextBox2.Text;
           

        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //  new NotImplementedException();
        }
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
          
        }
        #endregion



        public string chat { get; set; }

        public string imagesrc { get; set; }

        public string otherimagesrc { get; set; }

        public string chatInsta { get; set; }

        public string imagesrcInsta { get; set; }

        public string chatFb { get; set; }

        public string imagesrcFb { get; set; }
    }
}
