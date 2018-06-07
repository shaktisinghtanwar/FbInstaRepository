using Fb_InstaWpf.Helper;
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
using System.Windows;

namespace Fb_InstaWpf
{
    public class OnlineFetcher
    {
        //public OnlineFetcher()
        //{
        //    LoginWithSelenium();
            
        //}
        public static ChromeDriver GetDriver()
        {
            return new ChromeDriver();
        }
        ChromeDriver ChromeWebDriver { get; set; }
        ChromeOptions _options = new ChromeOptions();
        public List<String> LstPageUrl = new List<string>();
        private readonly Queue<string> _queueFbCmntImgUrl = new Queue<string>();
        
        DbHelper _dbHelper;

        public void LoginWithSelenium()
         {
            try
            {
               
                //FileOperation.UserName = cmbUser.Text;
                //FileOperation.Password = Convert.ToString(cmbUser.SelectedValue);
                // FacebookUserLoginInfo facebookUserLoginInfo=new FacebookUserLoginInfo();
             string appStartupPath = System.IO.Path.GetDirectoryName(Assembly.GetCallingAssembly().Location );
             //   string appStartupPath = "E:\\RAHUL_WORK\\WPF_Examples\\WPF_Projects\\FbInstaRepositoryNew\\Fb_InstaWpf\bin\\Debug\\chromedriver.exe";
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
                    emailElement[0].SendKeys("rishusingh77777@gmail.com");

                    //emailElement[0].SendKeys(FileOperation.UserName);

                    //CurrentLogedInFacebookUserinfo.Username = facebookUserinfo.Username
                }
                ReadOnlyCollection<IWebElement> passwordElement = ChromeWebDriver.FindElements(By.Id("pass"));
                if (passwordElement.Count > 0)
                {
                    passwordElement[0].SendKeys("1234567#rk");
                   // passwordElement[0].SendKeys(FileOperation.Password);

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
                          var  TemppageNodeItem = pageNodeItem.GetAttribute("href");
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
                    // ShowMessengerListData();
                    GetFbMessengerMessages();
                    Thread.Sleep(2000);
                    MessageBox.Show("Login Successful.......!");
                    //isLoggedIn = true;
                    Thread.Sleep(2000);

                   // Image.Visibility = Visibility.Hidden;
                }

            }
            catch (Exception ex)
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
            {_dbHelper=new DbHelper();
                List<ListUsernameInfo> _MyListUsernameInfo = new List<ListUsernameInfo>();
                Queue<string> myQueue = new Queue<string>();
                //var ChromeWebDriver = GetDriver();
                ListUsernameInfo listUsernameInfo = new ListUsernameInfo();
                var PageSource = ChromeWebDriver.PageSource;
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
                        var currentURL = ChromeWebDriver.Url;
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


        private void GetFacebookCommenter(string navigationUrl)
        {
            List<FbUserMessageInfo> messagingFbpageListInfo = null;
            try
            {
                var chromeWebDriver = GetDriver();
                // chromeWebDriver.Navigate().GoToUrl(SelectedFBPageInfo.FBInboxNavigationUrl);
                chromeWebDriver.Navigate().GoToUrl(navigationUrl);

                Thread.Sleep(3000);
                var pageSource = chromeWebDriver.PageSource;
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(pageSource);
                Thread.Sleep(3000);
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
    }
}
