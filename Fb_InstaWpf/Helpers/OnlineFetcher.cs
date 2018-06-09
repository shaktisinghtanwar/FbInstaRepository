﻿using Fb_InstaWpf.Helper;
using Fb_InstaWpf.Model;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Fb_InstaWpf
{
    public class OnlineFetcher
    {
        public event Action LoginSuccessEvent;


        public ChromeDriver GetDriver()
        {
            var driver = new ChromeDriver();
            if (_cookieJar != null)
            {
                foreach (var cookie in _cookieJar.AllCookies)
                {
                    driver.Manage().Cookies.AddCookie(cookie); ;
                }
            }
            return driver;
        }
      //  ChromeDriver ChromeWebDriver { get; set; }
        ChromeOptions _options = new ChromeOptions();
        public List<String> LstPageUrl = new List<string>();
        private readonly Queue<string> _queueFbCmntImgUrl = new Queue<string>();
        
        DbHelper _dbHelper;
        private ICookieJar _cookieJar;
        public bool isLoggedIn = false;
        public void LoginWithSelenium(string userName ,string password)
         {
            try
            {
             
             string appStartupPath = System.IO.Path.GetDirectoryName(Assembly.GetCallingAssembly().Location );
                const string url = "https://en-gb.facebook.com/login/";
                _options.AddArgument("--disable-notifications");
                _options.AddArgument("--disable-extensions");
                _options.AddArgument("--test-type");
                _options.AddArgument("--log-level=3");
                ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService(appStartupPath);
                chromeDriverService.HideCommandPromptWindow = true;
                var chromeWebDriver = new ChromeDriver(chromeDriverService, _options);
                chromeWebDriver.Manage().Window.Maximize();
                chromeWebDriver.Navigate().GoToUrl(url);
                try
                {
                    ((IJavaScriptExecutor)chromeWebDriver).ExecuteScript("window.onbeforeunload = function(e){};");
                }
                catch (Exception)
                {

                }

                ReadOnlyCollection<IWebElement> emailElement = chromeWebDriver.FindElements(By.Id("email"));
                if (emailElement.Count > 0)
                {
                   // emailElement[0].SendKeys("rishusingh77777@gmail.com");

                    emailElement[0].SendKeys(userName);

                    //CurrentLogedInFacebookUserinfo.Username = facebookUserinfo.Username
                }
                ReadOnlyCollection<IWebElement> passwordElement = chromeWebDriver.FindElements(By.Id("pass"));
                if (passwordElement.Count > 0)
                {
                    passwordElement[0].SendKeys(password);
                   // passwordElement[0].SendKeys(FileOperation.Password);

                }


                ReadOnlyCollection<IWebElement> signInElement = chromeWebDriver.FindElements(By.Id("loginbutton"));
                if (signInElement.Count > 0)
                {
                    signInElement[0].Click();
                    Thread.Sleep(3000);
                    chromeWebDriver.Navigate().GoToUrl("https://www.facebook.com/pages/?category=your_pages");
                    // ChromeWebDriver.Navigate().GoToUrl(url);
                    Thread.Sleep(2000);
                    //ChromeWebDriver.Navigate().GoToUrl("https://www.facebook.com/TP-1996120520653285/inbox/?selected_item_id=100002948674558");
                    ReadOnlyCollection<IWebElement> pageNodeImgUrl = chromeWebDriver.FindElements((By.XPath("//*[@class='clearfix _1vh8']/a")));
                    if (pageNodeImgUrl.Count > 0)
                    {
                        foreach (var pageNodeItem in pageNodeImgUrl)
                        {
                          var  TemppageNodeItem = pageNodeItem.GetAttribute("href");
                            LstPageUrl.Add(TemppageNodeItem);

                            _queueFbCmntImgUrl.Enqueue(TemppageNodeItem);
                        }

                        // for (int i = 0; i < LstPageUrl.Count; i++)
                        // {
                        chromeWebDriver.Navigate().GoToUrl(new Uri(LstPageUrl[2]));
                        Thread.Sleep(2000);

                        ReadOnlyCollection<IWebElement> collection1 = chromeWebDriver.FindElements(By.XPath("//*[@id='u_0_u']/div/div/div[1]/ul/li[2]/a"));
                        if (collection1.Count > 0)
                        {
                            collection1[0].Click();
                        }


                    }

                    Thread.Sleep(5000);
                    // ShowMessengerListData();
                    GetFbMessengerMessages();
                    Thread.Sleep(2000);
                    LoginSuccessEvent();
                    _cookieJar = chromeWebDriver.Manage().Cookies;

                    isLoggedIn = true;
                    Thread.Sleep(2000);

                   // Image.Visibility = Visibility.Hidden;
                }

            }
            catch (Exception)
            {

                //;
            }
        }

        public void GetFacebookMessages()
        {
            var chromeWebDriver = GetDriver();
            ListUsernameInfo listUsernameInfo = new ListUsernameInfo();
            string url = "https://www.facebook.com/pages/?category=your_pages";
            chromeWebDriver.Navigate().GoToUrl(url);
            Thread.Sleep(2000);
            var pageUrlList = new List<string>();
            var queueFbCmntImgUrl = new Queue<string>();
            ReadOnlyCollection<IWebElement> pageNodeImgUrl = chromeWebDriver.FindElements((By.XPath("//*[@class='clearfix _1vh8']/a")));
            if (pageNodeImgUrl.Count > 0)
            {
                foreach (var pageNodeItem in pageNodeImgUrl)
                {
                    var temppageNodeItem = pageNodeItem.GetAttribute("href");
                    pageUrlList.Add(temppageNodeItem);
                    queueFbCmntImgUrl.Enqueue(temppageNodeItem);
                }

                //WebDriverWait wait = new WebDriverWait(chromeWebDriver, TimeSpan.FromSeconds(30));
                //wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.Id("h")));

                Thread.Sleep(2000);
                chromeWebDriver.Navigate().GoToUrl(new Uri(pageUrlList[2]));
                Thread.Sleep(2000);

                ReadOnlyCollection<IWebElement> collection1 = chromeWebDriver.FindElements(By.XPath("//*[@id='u_0_u']/div/div/div[1]/ul/li[2]/a"));
                if (collection1.Count > 0)
                {
                    collection1[0].Click();
                }
                Thread.Sleep(2000);
                ReadOnlyCollection<IWebElement> collectionTab2 = chromeWebDriver.FindElements(By.ClassName("_32wr"));
                if (collectionTab2.Count > 0)
                {
                    collectionTab2[1].Click();
                }
                Thread.Sleep(2000);
                var pageSource = chromeWebDriver.PageSource;
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(pageSource);
                Thread.Sleep(2000);
                HtmlNodeCollection imgNode = htmlDocument.DocumentNode.SelectNodes("//*[@id='u_0_t']/div/div/div/table/tbody/tr/td[1]/div/div[2]/div/div[1]/div/div/div/div/div/div/img");
                var queueFbImgUrl = new Queue<string>();

                if (imgNode != null)
                {
                    foreach (var imgNodeItem in imgNode)
                    {
                        var imageUrl = imgNodeItem.Attributes["src"].Value.Replace(";", "&");
                        queueFbImgUrl.Enqueue(imageUrl);
                    }
                }

                ReadOnlyCollection<IWebElement> userlistnode = chromeWebDriver.FindElements(By.ClassName("_4k8x"));
                if (userlistnode.Count > 0)
                {
                    foreach (var itemurl in userlistnode)
                    {
                        itemurl.Click();
                        Thread.Sleep(3000);
                        string userName = itemurl.Text;
                        listUsernameInfo.ListUsername = userName;

                        string currentURL = chromeWebDriver.Url;
                        var tempId = currentURL.Split('?')[1].Split('=')[1];
                        listUsernameInfo.ListUserId = tempId;
                        listUsernameInfo.InboxNavigationUrl = currentURL;
                        var imgUrl = queueFbImgUrl.Dequeue();


                        _dbHelper.InsertFacebookMessage( listUsernameInfo, userName, currentURL, imgUrl);
                    }
                }
            }
        }

        public void GetFbMessengerMessages()
        {
            try
            {
                _dbHelper =new DbHelper();
                List<ListUsernameInfo> _MyListUsernameInfo = new List<ListUsernameInfo>();
                Queue<string> myQueue = new Queue<string>();
                var chromeWebDriver = GetDriver();
                ListUsernameInfo listUsernameInfo = new ListUsernameInfo();
                var PageSource = chromeWebDriver.PageSource;
                Thread.Sleep(3000);
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(PageSource);
                Thread.Sleep(3000);
                HtmlNodeCollection imgNode =
                    htmlDocument.DocumentNode.SelectNodes("//*[@id='u_0_t']/div/div/div/table/tbody/tr/td[1]/div/div[2]/div/div[1]/div/div/div/div/div/div/img");
                       
                if (imgNode != null)
                {
                    foreach (var imgNodeItem in imgNode)
                    {
                        var Getimgurl = imgNodeItem.Attributes["src"].Value.Replace(";", "&");
                        myQueue.Enqueue(Getimgurl);
                    }
                }
                var listNodeElements = htmlDocument.DocumentNode.SelectNodes("//div[@class='_4ik4 _4ik5']");

                ReadOnlyCollection<IWebElement> userlistnode = chromeWebDriver.FindElements(By.ClassName("_4k8x"));
                if (userlistnode.Count > 0)
                {
                    foreach (var itemurl in userlistnode)
                    {
                        Thread.Sleep(3000);
                        itemurl.Click();
                        Thread.Sleep(3000);
                        string userName = itemurl.Text;
                        listUsernameInfo.ListUsername = userName;
                        var currentURL = chromeWebDriver.Url;
                        var tempId = currentURL.Split('?')[1].Split('=')[1];
                        listUsernameInfo.ListUserId = tempId;
                        listUsernameInfo.InboxNavigationUrl = currentURL;
                        _MyListUsernameInfo.Add(listUsernameInfo);
                        var imgUrl = myQueue.Dequeue();
                        Thread.Sleep(3000);
                        _dbHelper.InsertFbMessengerMessage(listUsernameInfo, userName, imgUrl);
                     
                    }
                }
            }
            catch (Exception)
            {


            }
        }

        public void GetInstaMesages()
        {
            Queue<string> queueInstaImgUrl = new Queue<string>();

            var chromeWebDriver = GetDriver();
            Thread.Sleep(3000);
            ListUsernameInfo listUsernameInfo = new ListUsernameInfo();
            string url = "https://www.facebook.com/TP-1996120520653285/inbox/";
            chromeWebDriver.Navigate().GoToUrl(url);
            Thread.Sleep(3000);

            ReadOnlyCollection<IWebElement> collection = chromeWebDriver.FindElements(By.ClassName("_32wr"));
            {
                if (collection.Count > 0)
                {
                    collection[2].Click();
                    Thread.Sleep(3000);
                }
            }

            ReadOnlyCollection<IWebElement> commentpostImgNodCollection =
                chromeWebDriver.FindElements(By.XPath(".//*[@class='_11eg _5aj7']/div/div/img"));
            if (commentpostImgNodCollection.Count > 0)
            {
                for (int i = 0; i < commentpostImgNodCollection.Count; i++)
                {
                    var DataImg = commentpostImgNodCollection[i].GetAttribute("src");
                    queueInstaImgUrl.Enqueue(DataImg);
                }
            }

            ReadOnlyCollection<IWebElement> userlistnode = chromeWebDriver.FindElements(By.ClassName("_4k8x"));
            if (userlistnode.Count > 0)
            {
                foreach (var itemurl in userlistnode)
                {
                    itemurl.Click();
                    Thread.Sleep(3000);
                    string userName = itemurl.Text;
                    listUsernameInfo.ListUsername = userName;

                    var currentURL = chromeWebDriver.Url;
                    var tempId = currentURL.Split('?')[1].Split('=')[1];
                    listUsernameInfo.ListUserId = tempId;
                    listUsernameInfo.InboxNavigationUrl = currentURL;

                    var imgUrl = queueInstaImgUrl.Dequeue();
                    _dbHelper.InsertInstagramMessage(userName, currentURL, imgUrl);

                }
            }
        }


        private void GetFacebookCommenter()
        {
            List<FbUserMessageInfo> messagingFbpageListInfo = null;
            try
            {
                var chromeWebDriver = GetDriver();
                // chromeWebDriver.Navigate().GoToUrl(SelectedFBPageInfo.FBInboxNavigationUrl); navigationUrl string FBInboxNavigationUrl
                chromeWebDriver.Navigate().GoToUrl("https://www.facebook.com/TP-1996120520653285/inbox/?selected_item_id=1996233970641940");
                 
                Thread.Sleep(3000);
                var pageSource = chromeWebDriver.PageSource;
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(pageSource);
                Thread.Sleep(1000);
                HtmlNodeCollection commentNode = htmlDocument.DocumentNode.SelectNodes("//div[@class='_5v3q _5jmm _5pat _11m5']");
                messagingFbpageListInfo = new List<FbUserMessageInfo>();
                foreach (HtmlNode htmlcommentNode in commentNode)
                {
                    HtmlNode selectNode = htmlcommentNode.SelectSingleNode("//div[@class='_4vv0 _3ccb']");
                    var pagename = selectNode.InnerText;
                    messagingFbpageListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = pagename });

                    HtmlNode pageimg = htmlcommentNode.SelectSingleNode("//img[@class='scaledImageFitWidth img']");

                    var imgsrc = pageimg.Attributes["src"].Value.Replace(";", "&");
                    messagingFbpageListInfo.Add(new FbUserMessageInfo { UserType = 3, loginguserFbimage = imgsrc });
                }

                HtmlNodeCollection commentBlock = htmlDocument.DocumentNode.SelectNodes("//div[@class='UFICommentContent']");
                var commentImg = string.Empty;
                foreach (HtmlNode commentitem in commentBlock)
                {
                    var pagenamee = commentitem.InnerText;
                    var comment = pagenamee.Replace("Manage", "");

                    messagingFbpageListInfo.Add(new FbUserMessageInfo { UserType = 1, Message = comment });


                    Regex regex = new Regex(@"src(.*?)alt");
                    Match match = regex.Match(commentitem.InnerHtml);
                    if (match.Length != 0)
                    {
                        string[] msgId = match.Value.Replace(";", "&").Split('"');
                        var img = msgId[1];

                        messagingFbpageListInfo.Add(new FbUserMessageInfo { UserType = 2, otheruserFbimage = img });
                    }
                }

            }
            catch (Exception)
            {

            }
            _dbHelper.InsertFacebookCommentToDb(messagingFbpageListInfo);
        }

        private static void InsertFacebookCommentToDb(List<FbUserMessageInfo>  messagingFbpageListInfo)
        {
            for (int i = 0; i < messagingFbpageListInfo.Count; i++)
            {
                var chatFb = messagingFbpageListInfo[i].Message;
                var imagesrcFb = messagingFbpageListInfo[i].loginguserimage;
                var otherimagesrc = messagingFbpageListInfo[i].otheruserimage;

                string query1 = "select Count(*) from TblJobFb where Message='" + chatFb + "'and ImageSource='" + imagesrcFb + "'";
                SqLiteHelper sql1 = new SqLiteHelper();
                int count = Convert.ToInt32(sql1.ExecuteScalar(query1));

                if (count == 0)
                {
                    string query = "INSERT INTO TblJobFb(PlateformType,Message,ImageSource) values('1" + "','" + chatFb + "','" + imagesrcFb + "')";
                    SqLiteHelper sql = new SqLiteHelper();
                    int yy = sql.ExecuteNonQuery(query);
                }
            }
        }

        private void GetUserChatBoxDataOnline(string url)
        {
            var chromeWebDriver = GetDriver();
             
            chromeWebDriver.Navigate().GoToUrl(url);
            Thread.Sleep(2000);
            var plateformType = "1";
            var pageSource = chromeWebDriver.PageSource;
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(pageSource);
            Thread.Sleep(2000); 
            HtmlNodeCollection imgNode = htmlDocument.DocumentNode.SelectNodes("//div[@class='_41ud']");

            //foreach (HtmlNode htmlNodeDiv in imgNode)
            //{
            //    var currentURL = ChromeWebDriver.Url;
            //    var tempId = currentURL.Split('?')[1].Split('=')[1];

            //    HtmlNode selectSingleNode = htmlNodeDiv.SelectSingleNode(".//*[@class='clearfix _o46 _3erg _29_7 direction_ltr text_align_ltr']");

            //    if (selectSingleNode != null)
            //    {
            //        string otheruser = selectSingleNode.InnerText;
            //       // MessagingListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = otheruser });
            //        string query = "INSERT INTO TblJob(M_InboxUserId,PlateformType,PostType,Message,ImgSource,Status) values('" + tempId + "','" + plateformType + "','" + 0 + "','" + otheruser + "','" + imagesrc + "','" + Status + "')";
            //        SqLiteHelper sql = new SqLiteHelper();
            //        int yy = sql.ExecuteNonQuery(query);
            //    }

            //    HtmlNode selectSingleimgNode = htmlNodeDiv.SelectSingleNode(".//*[@class='clearfix _o46 _3erg _29_7 direction_ltr text_align_ltr _ylc']");
            //    if (selectSingleimgNode != null)
            //    {
            //        Regex regex = new Regex(@"src(.*?)style");
            //        Match match = regex.Match(selectSingleimgNode.InnerHtml);
            //        string msgId = match.Value.Replace("src=", "").Replace("style", "").Replace("\"", "").Replace(@"""", "").Replace("amp;", "");
            //        //MessagingListInfo.Add(new FbUserMessageInfo { UserType = 2, otheruserimage = msgId });
            //    }
            //    HtmlNode selectSingleNode2 = htmlNodeDiv.SelectSingleNode(".//*[@class='clearfix _o46 _3erg _3i_m _nd_ direction_ltr text_align_ltr']");
            //    if (selectSingleNode2 != null)
            //    {
            //        string loginuser = selectSingleNode2.InnerText;
            //        //MessagingListInfo.Add(new FbUserMessageInfo { UserType = 1, Message = loginuser });
            //    }

            //    HtmlNode selectSingleimgRightNode = htmlNodeDiv.SelectSingleNode(".//*[@class='clearfix _o46 _3erg _3i_m _nd_ direction_ltr text_align_ltr _ylc']");
            //    if (selectSingleimgRightNode != null)
            //    {
            //        Regex regex = new Regex(@"src(.*?)style");
            //        Match match = regex.Match(selectSingleimgRightNode.InnerHtml);
            //        string msgId = match.Value.Replace("src=", "").Replace("style", "").Replace("\"", "").Replace(@"""", "").Replace("amp;", "");
            //        //MessagingListInfo.Add(new FbUserMessageInfo { UserType = 3, loginguserimage = msgId });
            //        string query = "INSERT INTO TblJob(M_InboxUserId,PlateformType,PostType,Message,ImgSource,Status) values('" + tempId + "','" + plateformType + "','" + 0 + "','" + chat + "','" + imagesrc + "','" + Status + "')";
            //        SqLiteHelper sql = new SqLiteHelper();
            //        int yy = sql.ExecuteNonQuery(query);
            //    }

            //}
            Thread.Sleep(1000); // 5 Minutes

            //for (int i = 0; i < MessagingListInfo.Count; i++)
            //{
            //    chat = MessagingListInfo[i].Message;
            //    imagesrc = MessagingListInfo[i].loginguserimage;
            //    otherimagesrc = MessagingListInfo[i].otheruserimage;
            //    currentURL = ChromeWebDriver.Url;
            //    var tempId = currentURL.Split('?')[1].Split('=')[1];
            //    // listUsernameInfo.ListUserId = tempId;
            //    string query1 = "select Count(*) from TblJob where Message='" + chat + "'and ImgSource='" + imagesrc + "'";
            //    SqLiteHelper sql1 = new SqLiteHelper();
            //    int count = Convert.ToInt32(sql1.ExecuteScalar(query1));

            //    if (count == 0)
            //    {
            //        string query = "INSERT INTO TblJob(M_InboxUserId,PlateformType,PostType,Message,ImgSource,Status) values('" + tempId + "','" + plateformType + "','" + PostType + "','" + chat + "','" + imagesrc + "','" + Status + "')";
            //        SqLiteHelper sql = new SqLiteHelper();
            //        int yy = sql.ExecuteNonQuery(query);
            //    }
            //}
        }



        private void GetInstagramComment()
        {
            List<FbUserMessageInfo> messagingFbpageListInfo = null;
            try
            { 
                 var chromeWebDriver = GetDriver();
                // chromeWebDriver.Navigate().GoToUrl(SelectedFBPageInfo.FBInboxNavigationUrl); navigationUrl
                chromeWebDriver.Navigate().GoToUrl("https://www.facebook.com/TP-1996120520653285/inbox/?selected_item_id=1996142970651040");

                Thread.Sleep(3000);
                var pageSource = chromeWebDriver.PageSource;
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(pageSource);
                Thread.Sleep(1000);
                HtmlNodeCollection commentNode = htmlDocument.DocumentNode.SelectNodes("//div[@class='_4cye _4-u2  _4-u8']");
                messagingFbpageListInfo = new List<FbUserMessageInfo>();
                foreach (HtmlNode htmlcommentNode in commentNode)
                {
                    HtmlNode selectNode = htmlcommentNode.SelectSingleNode("//div[@class='_4cyh']");
                    var pagename = selectNode.InnerText;
                    messagingFbpageListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = pagename });

                    HtmlNode pageimg = htmlcommentNode.SelectSingleNode("//img[@class='img']");

                    var imgsrc = pageimg.Attributes["src"].Value.Replace(";", "&");
                    messagingFbpageListInfo.Add(new FbUserMessageInfo { UserType = 3, loginguserFbimage = imgsrc });
                }

                HtmlNodeCollection commentBlock = htmlDocument.DocumentNode.SelectNodes("//div[@class='_3i4- _5aj7']");
                var commentImg = string.Empty;
                foreach (HtmlNode commentitem in commentBlock)
                {

                    var usernameAndComment = commentitem.InnerText.Split();
                    var ccomment = usernameAndComment[0];
                }

            }
            catch (Exception)
            {

            }
        }
    }
}
