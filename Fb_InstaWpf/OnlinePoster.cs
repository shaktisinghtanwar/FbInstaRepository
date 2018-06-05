using Fb_InstaWpf.Helper;
using Fb_InstaWpf.Model;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading;
using HtmlAgilityPack;

namespace Fb_InstaWpf
{
    public class OnlinePoster
    {
        static ConcurrentQueue<PostMessage> _producerConsumerCollection;

        public OnlinePoster()
        {
            _producerConsumerCollection = new ConcurrentQueue<PostMessage>();
            RestartThread();
        }

        public static void Add(PostMessage message)
        {


            _producerConsumerCollection.Enqueue(message);



        }

        public static void ProcessMessage()
        {
            PostMessage message;
            try
            {
                if (_producerConsumerCollection.TryPeek(out message))
                {
                    if (TryMessagePosting(message))
                    {
                        _producerConsumerCollection.TryDequeue(out message);
                    }
                }
            }
            catch (Exception)
            {
                //Log message
            }
            finally
            {
                RestartThread();
            }

        }

        private static void RestartThread()
        {
            System.Threading.Thread thread;
            thread = new System.Threading.Thread(ProcessMessage);
            thread.Start();
        }

        private static bool TryMessagePosting(PostMessage message)
        {
            switch (message.MessageType)
            {
                case MessageType.FacebookMessage:
                    return TryPostMessageToFacebook(message);
                case MessageType.FacebookImage:
                    return TryPostImageToFacebook(message);
                case MessageType.InstaMessage:
                    return TryPostMessageToInsta(message);
                case MessageType.FacebookMessengerMessage:
                    return TryPostMessageToFBMessenger(message);
                case MessageType.FacebookMessengerImage:
                    return TryPostImageToFBMessenger(message);
                default:
                    return false;
            }
        }
        public static ChromeDriver GetDriver()
        {
            return new ChromeDriver();
        }
        public static bool TryPostMessageToFacebook(PostMessage message)
        {
            try
            {
                var ChromeWebDriver = GetDriver();
                ChromeWebDriver.Navigate().GoToUrl("https://www.facebook.com/TP-1996120520653285/inbox/?selected_item_id=100002948674558");
                Thread.Sleep(2000);
                //  ChromeWebDriver.Navigate().GoToUrl("https://www.facebook.com/pages/?category=your_pages");
                string pageSource = ChromeWebDriver.PageSource;


                ReadOnlyCollection<IWebElement> writeNode =
                       ChromeWebDriver.FindElements(By.XPath("//*[@placeholder='Write a reply...']"));
                if (writeNode.Count > 0)
                {
                    Thread.Sleep(2000);
                    writeNode[0].SendKeys(message.Message);
                    Thread.Sleep(2000);
                }

                ReadOnlyCollection<IWebElement> submitnode =
                       ChromeWebDriver.FindElements(By.XPath("//*[@type='submit']"));
                if (submitnode.Count > 0)
                {
                    Thread.Sleep(2000);
                    submitnode[1].Click();
                    Thread.Sleep(2000);
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        public static bool TryPostImageToFacebook(PostMessage message)
        {
            try
            {
                var ChromeWebDriver = GetDriver();
                ReadOnlyCollection<IWebElement> emailElement1 = ChromeWebDriver.FindElements(By.ClassName("UFICommentPhotoIcon"));
                if (emailElement1.Count > 0)
                {
                    emailElement1[0].Click();

                }
                Thread.Sleep(3000);

                ReadOnlyCollection<IWebElement> sendimage = ChromeWebDriver.FindElements(By.XPath(".//span[@data-testid='ufi_photo_preview_test_id']"));
                if (sendimage.Count > 0)
                {
                    Thread.Sleep(3000);
                    sendimage[0].SendKeys(Keys.Enter);
                    Thread.Sleep(3000);
                }
            }
            catch (Exception)
            {

                // ;
            }
            return false;
        }

        public static bool TryPostMessageToInsta(PostMessage message)
        {
            var ChromeWebDriver = GetDriver();
           
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
                    postcomghghment[0].SendKeys(message.Message);
                    Thread.Sleep(1000);
                    postcomghghment[0].SendKeys(OpenQA.Selenium.Keys.Enter);
                }

            }
            return false;
        }

        public static bool TryPostMessageToFBMessenger(PostMessage message)
        {
            var ChromeWebDriver = GetDriver();
            ReadOnlyCollection<IWebElement> emailElement = ChromeWebDriver.FindElements(By.ClassName("_4dvy"));
            if (emailElement.Count > 0)
            {
                emailElement[0].Click();
            }
            Thread.Sleep(3000);

            ReadOnlyCollection<IWebElement> sendimage = ChromeWebDriver.FindElements(By.ClassName("_4dw3"));
            if (sendimage.Count > 0)
            {
                Thread.Sleep(3000);
                sendimage[0].Click();
                Thread.Sleep(3000);
            }
            return false;
        }
        public static bool TryPostImageToFBMessenger(PostMessage message)
        {
            return false;
        }
    }

    public class PostMessage
    {
        public string FromUserId { get; set; }

        public string Message { get; set; }

        public MessageType MessageType { get; set; }

        public string ImagePath { get; set; }
    }

    public enum MessageType
    {
        FacebookMessage,
        FacebookImage,
        InstaMessage,
        FacebookMessengerMessage,
        FacebookMessengerImage
    }


    public class OnlineFetcher
    {
        public List<String> LstPageUrl = new List<string>();
        private readonly Queue<string> _queueFbCmntImgUrl = new Queue<string>();
        public string PageSource { get; set; }
        string TemppageNodeItem = string.Empty;
        string Getimgurl = string.Empty;
        public List<ListUsernameInfo> _MyListUsernameInfo = new List<ListUsernameInfo>();
        private readonly Queue<string> _queueFbImgUrl = new Queue<string>();
        readonly HtmlDocument _htmlDocument = new HtmlDocument();
        public static ChromeDriver GetDriver()
        {
            return new ChromeDriver();
        }
        public void GetFacebookMessaegs()
        {
            SqLiteHelper sql =new SqLiteHelper();
           
            var ChromeWebDriver = GetDriver();
            ListUsernameInfo listUsernameInfo = new ListUsernameInfo();
            string url = "https://www.facebook.com/pages/?category=your_pages";
            ChromeWebDriver.Navigate().GoToUrl(url);
            Thread.Sleep(2000);
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
                Thread.Sleep(2000);
                ChromeWebDriver.Navigate().GoToUrl(new Uri(LstPageUrl[2]));
                Thread.Sleep(2000);

                ReadOnlyCollection<IWebElement> collection1 = ChromeWebDriver.FindElements(By.XPath("//*[@id='u_0_u']/div/div/div[1]/ul/li[2]/a"));
                if (collection1.Count > 0)
                {
                    collection1[0].Click();
                }
                Thread.Sleep(2000);
                ReadOnlyCollection<IWebElement> collectionTab2 = ChromeWebDriver.FindElements(By.ClassName("_32wr"));
                if (collectionTab2.Count > 0)
                {
                    collectionTab2[1].Click();
                }
                Thread.Sleep(2000);
                PageSource = ChromeWebDriver.PageSource;
                _htmlDocument.LoadHtml(PageSource);
                Thread.Sleep(2000);
                HtmlNodeCollection imgNode = _htmlDocument.DocumentNode.SelectNodes("//*[@id='u_0_t']/div/div/div/table/tbody/tr/td[1]/div/div[2]/div/div[1]/div/div/div/div/div/div/img");


                if (imgNode != null)
                {
                    foreach (var imgNodeItem in imgNode)
                    {
                        Getimgurl = imgNodeItem.Attributes["src"].Value.Replace(";", "&");
                        _queueFbImgUrl.Enqueue(Getimgurl);
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
                        string currentURL = ChromeWebDriver.Url;
                        var tempId = currentURL.Split('?')[1].Split('=')[1];
                        listUsernameInfo.ListUserId = tempId;
                        listUsernameInfo.InboxNavigationUrl = currentURL;
                        // _listUsernameInfo.ListUsername=LstItemUserName;
                        _MyListUsernameInfo.Add(listUsernameInfo);

                        var imgUrl = _queueFbImgUrl.Dequeue();

                        //FbPageListmembers.Add(new FacebookPageInboxmember { FbPageImage = imgUrl, FbPageName = LstfbItemUserName });
                        string query = "INSERT INTO TblFbComment(Fbcomment_InboxUserId, Fbcomment_InboxUserName,Fbcomment_InboxUserImage,FBInboxNavigationUrl,Status) values('" + listUsernameInfo.ListUserId + "','" + userName + "','" + imgUrl + "','" + currentURL + "','" + false + "')";
                        int yy = sql.ExecuteNonQuery(query);



                        //FbPageListmembers.Add(new FacebookPageInboxmember()
                        //{
                        //    FbPageName = userName,
                        //    FbPageImage = imgUrl,
                        //    FBInboxNavigationUrl = currentURL
                        //});
                        #endregion
                    }
                }

            }
        }

    }

    public class DbHelper
    {
        private readonly Queue<string> _queueFbImgUrl = new Queue<string>();
        private readonly Queue<string> _queueInstaImgUrl = new Queue<string>();
        public List<String> LstPageUrl = new List<string>();
        private readonly Queue<string> _queueFbCmntImgUrl = new Queue<string>();
        public string PageSource { get; set; }

        private readonly Queue<string> _myqueue = new Queue<string>();
        public List<ListUsernameInfo> _MyListUsernameInfo = new List<ListUsernameInfo>();

        public static ChromeDriver GetDriver()
        {
            return new ChromeDriver();
        }

        private readonly HtmlDocument _htmlDocument = new HtmlDocument();

        private static SqLiteHelper GetSqliteHelper()
        {
            return new SqLiteHelper();
        }

        public static ObservableCollection<FacebookPageInboxmember> ShowFacebookListData()
        {
            SqLiteHelper sql = GetSqliteHelper();
            ObservableCollection<FacebookPageInboxmember> FbPageListmembers =
                new ObservableCollection<FacebookPageInboxmember>();
            string query =
                "select Fbcomment_InboxUserName,Fbcomment_InboxUserImage,FBInboxNavigationUrl from TblFbComment";
            var dt = sql.GetDataTable(query);
            foreach (DataRow item in dt.Rows)
            {
                string Fbcomment_InboxUserName = Convert.ToString(item["Fbcomment_InboxUserName"]);
                string Fbcomment_InboxUserImage = Convert.ToString(item["Fbcomment_InboxUserImage"]);
                string FBInboxNavigationUrl = Convert.ToString(item["FBInboxNavigationUrl"]);
                if (!FbPageListmembers.Any(m => m.FbPageName.Equals(Fbcomment_InboxUserName)))
                {
                    FbPageListmembers.Add(new FacebookPageInboxmember()
                    {
                        FbPageName = Fbcomment_InboxUserName,
                        FbPageImage = Fbcomment_InboxUserImage,
                        FBInboxNavigationUrl = FBInboxNavigationUrl
                    });
                }
            }

            return FbPageListmembers;
        }



        private void GetFbMessengerListData()
        {
            try
            {
                SqLiteHelper sql = GetSqliteHelper();

                var ChromeWebDriver = GetDriver();
                ListUsernameInfo listUsernameInfo = new ListUsernameInfo();
                var PageSource = ChromeWebDriver.PageSource;
                Thread.Sleep(3000);
                _htmlDocument.LoadHtml(PageSource);
                Thread.Sleep(3000);
                HtmlNodeCollection imgNode =
                    _htmlDocument.DocumentNode.SelectNodes(
                        "//*[@id='u_0_t']/div/div/div/table/tbody/tr/td[1]/div/div[2]/div/div[1]/div/div/div/div/div/div/img");
                if (imgNode != null)
                {
                    foreach (var imgNodeItem in imgNode)
                    {
                        var Getimgurl = imgNodeItem.Attributes["src"].Value.Replace(";", "&");
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
                        var currentURL = ChromeWebDriver.Url;
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
            }
            catch (Exception)
            {


            }

        }


        public void GetFacebookMessaegs()
        {
            SqLiteHelper sql = new SqLiteHelper();

            var ChromeWebDriver = GetDriver();
            ListUsernameInfo listUsernameInfo = new ListUsernameInfo();
            string url = "https://www.facebook.com/pages/?category=your_pages";
            ChromeWebDriver.Navigate().GoToUrl(url);
            Thread.Sleep(2000);
            ReadOnlyCollection<IWebElement> pageNodeImgUrl =
                ChromeWebDriver.FindElements((By.XPath("//*[@class='clearfix _1vh8']/a")));
            if (pageNodeImgUrl.Count > 0)
            {
                foreach (var pageNodeItem in pageNodeImgUrl)
                {

                    var TemppageNodeItem = pageNodeItem.GetAttribute("href");
                    LstPageUrl.Add(TemppageNodeItem);

                    _queueFbCmntImgUrl.Enqueue(TemppageNodeItem);
                }

                // for (int i = 0; i < LstPageUrl.Count; i++)
                // { 
                Thread.Sleep(2000);
                ChromeWebDriver.Navigate().GoToUrl(new Uri(LstPageUrl[2]));
                Thread.Sleep(2000);

                ReadOnlyCollection<IWebElement> collection1 =
                    ChromeWebDriver.FindElements(By.XPath("//*[@id='u_0_u']/div/div/div[1]/ul/li[2]/a"));
                if (collection1.Count > 0)
                {
                    collection1[0].Click();
                }
                Thread.Sleep(2000);
                ReadOnlyCollection<IWebElement> collectionTab2 = ChromeWebDriver.FindElements(By.ClassName("_32wr"));
                if (collectionTab2.Count > 0)
                {
                    collectionTab2[1].Click();
                }
                Thread.Sleep(2000);
                PageSource = ChromeWebDriver.PageSource;
                _htmlDocument.LoadHtml(PageSource);
                Thread.Sleep(2000);
                HtmlNodeCollection imgNode =
                    _htmlDocument.DocumentNode.SelectNodes(
                        "//*[@id='u_0_t']/div/div/div/table/tbody/tr/td[1]/div/div[2]/div/div[1]/div/div/div/div/div/div/img");


                if (imgNode != null)
                {
                    foreach (var imgNodeItem in imgNode)
                    {
                        var Getimgurl = imgNodeItem.Attributes["src"].Value.Replace(";", "&");
                        _queueFbImgUrl.Enqueue(Getimgurl);
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
                        string currentURL = ChromeWebDriver.Url;
                        var tempId = currentURL.Split('?')[1].Split('=')[1];
                        listUsernameInfo.ListUserId = tempId;
                        listUsernameInfo.InboxNavigationUrl = currentURL;
                        // _listUsernameInfo.ListUsername=LstItemUserName;
                        _MyListUsernameInfo.Add(listUsernameInfo);

                        var imgUrl = _queueFbImgUrl.Dequeue();

                        //FbPageListmembers.Add(new FacebookPageInboxmember { FbPageImage = imgUrl, FbPageName = LstfbItemUserName });
                        string query =
                            "INSERT INTO TblFbComment(Fbcomment_InboxUserId, Fbcomment_InboxUserName,Fbcomment_InboxUserImage,FBInboxNavigationUrl,Status) values('" +
                            listUsernameInfo.ListUserId + "','" + userName + "','" + imgUrl + "','" + currentURL + "','" +
                            false + "')";
                        int yy = sql.ExecuteNonQuery(query);



                        //FbPageListmembers.Add(new FacebookPageInboxmember()
                        //{
                        //    FbPageName = userName,
                        //    FbPageImage = imgUrl,
                        //    FBInboxNavigationUrl = currentURL
                        //});

                        #endregion
                    }
                }

            }
        }


        private void GetInstaCommenter()
        {
           
          
                SqLiteHelper sql = new SqLiteHelper();

                var ChromeWebDriver = GetDriver();
                // ChromeWebDriver.Navigate().GoToUrl(SelectedInstaInboxmemberInfo.InstaInboxNavigationUrl);
                Thread.Sleep(3000);
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

                ReadOnlyCollection<IWebElement> commentpostImgNodCollection =
                    ChromeWebDriver.FindElements(By.XPath(".//*[@class='_11eg _5aj7']/div/div/img"));
                if (commentpostImgNodCollection.Count > 0)
                {
                    for (int i = 0; i < commentpostImgNodCollection.Count; i++)
                    {
                        var DataImg = commentpostImgNodCollection[i].GetAttribute("src");
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
                        var currentURL = ChromeWebDriver.Url;
                        var tempId = currentURL.Split('?')[1].Split('=')[1];
                        listUsernameInfo.ListUserId = tempId;
                        listUsernameInfo.InboxNavigationUrl = currentURL;
                        // _listUsernameInfo.ListUsername=LstItemUserName;
                        _MyListUsernameInfo.Add(listUsernameInfo);

                        var imgUrl = _queueInstaImgUrl.Dequeue();
                        string query =
                            "INSERT INTO Tbl_Instagram(Insta_inboxUserName,Insta_inboxUserImage,InstaInboxNavigationUrl,Status) values('" +
                            userName + "','" + imgUrl + "','" + currentURL + "','" + false + "')";

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
        }


        //private void ShowMessengerListData()
        //{
        //    SqLiteHelper sql = new SqLiteHelper();

        //    string query = "select M_InboxUserId,M_inboxUserName,M_InboxUserImage,M_InboxNavigationUrl from TblMessengerList";
        //    var dt = sql.GetDataTable(query);
        //    foreach (DataRow item in dt.Rows)
        //    {

        //        string inboxUserId = Convert.ToString(item["M_InboxUserId"]);
        //        string inboxUserName = Convert.ToString(item["M_inboxUserName"]);
        //        string inboxUserImage = Convert.ToString(item["M_InboxUserImage"]);
        //        string inboxNavigationUrl = Convert.ToString(item["M_InboxNavigationUrl"]);
        //        //if (!UserListInfo.Any(m => m.InboxUserName.Equals(M_inboxUserName)))
        //        if (!UserListInfo.Any(m => m.InboxUserName.Equals(inboxUserName)))
        //        {
        //            UserListInfo.Add(new FbpageInboxUserInfo() { InboxUserId = inboxUserId, InboxUserName = inboxUserName, InboxUserImage = inboxUserImage, InboxNavigationUrl = inboxNavigationUrl });

        //        }
        //    }
        //}

        
    }
}
