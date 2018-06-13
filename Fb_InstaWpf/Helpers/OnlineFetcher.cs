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
using System.Collections;

namespace Fb_InstaWpf
{
    public class OnlineFetcher
    {
        public event Action LoginSuccessEvent;


        public ChromeDriver GetDriver()
        {
            var driver = new ChromeDriver();
           
            return driver;
        }

        public static void SetCookies(ChromeDriver driver)
        {
            if (_cookieJar != null)
            {
                foreach (var cookie in _cookieJar.AllCookies)
                {
                    driver.Manage().Cookies.AddCookie(cookie); ;
                }
            }
        }
        public static ChromeDriver chromeWebDriver { get; set; }
        ChromeOptions _options = new ChromeOptions();
      
        private static ICookieJar _cookieJar;
        public void LoginWithSelenium(string userName ,string password)
         {
            try
            {
             FbPageInfo fbPageInfo=new FbPageInfo();
                 
             string appStartupPath = System.IO.Path.GetDirectoryName(Assembly.GetCallingAssembly().Location );
                const string url = "https://www.facebook.com/pages/?category=your_pages";
                _options.AddArgument("--disable-notifications");
                _options.AddArgument("--disable-extensions");
                _options.AddArgument("--test-type");
                _options.AddArgument("--log-level=3");
                ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService(appStartupPath);
                chromeDriverService.HideCommandPromptWindow = true;
                chromeWebDriver = new ChromeDriver(chromeDriverService, _options);
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
                  //  chromeWebDriver.Navigate().GoToUrl("https://www.facebook.com/pages/?category=your_pages");
                    // ChromeWebDriver.Navigate().GoToUrl(url);
                   // Thread.Sleep(2000);
                    //ChromeWebDriver.Navigate().GoToUrl("https://www.facebook.com/TP-1996120520653285/inbox/?selected_item_id=100002948674558");
                    try
                    {
                        var emailElement1 = chromeWebDriver.FindElements(By.XPath("//a[@class='_39g5']"));
                        foreach (var item in emailElement1)
                        {
                            string lin1k = item.GetAttribute("href");
                            if (item.GetAttribute("href").Contains("/live_video/launch_composer/?page_id="))
                            {
                                string pageId = lin1k.Replace("https://www.facebook.com/live_video/launch_composer/?page_id=", "");
                            }
                            if (item.GetAttribute("href").Contains("?modal=composer&ref=www_pages_browser_your_pages_section"))
                            {
                                string pageName = lin1k.Replace("https://www.facebook.com/", "").Replace("/?modal=composer&ref=www_pages_browser_your_pages_section", "");
                            }
                           
                        }
                    }
                    catch (Exception)
                    {

                    }
                    finally
                    {
                        _cookieJar = chromeWebDriver.Manage().Cookies;

                         // chromeWebDriver.Quit();

                    }
                    //Thread.Sleep(5000);

                    LoginSuccessEvent();
                   // isLoggedIn = true;
                  //  Thread.Sleep(2000);
                }

            }
            catch (Exception)
            {

                //;
            }
        }
        List<FbUserMessageInfo> messagingFbpageListInfo=new List<FbUserMessageInfo>();
        public void GetAllPaLoggedinUserPages(string userName)
        {
            var listFbPageInfo = new List<FbPageInfo>();
            Queue<string> queueFbCmntImgUrl = new Queue<string>();
            var chromeWebDriver = GetDriver();
            chromeWebDriver.Navigate().GoToUrl("https://www.facebook.com/pages/?category=your_pages");
            try
            {
                ReadOnlyCollection<IWebElement> pageNodepageUrl = chromeWebDriver.FindElements((By.XPath("//*[@class='clearfix _1vh8']/a")));
                if (pageNodepageUrl.Count > 0)
                {
                    foreach (var pageNodeItem in pageNodepageUrl)
                    {
                        var TemppageNodeItem = pageNodeItem.GetAttribute("href");
                        
                        //fbPageInfo.FbPageUrl = TemppageNodeItem;
                    //    LstPageUrl.Add(TemppageNodeItem);

                        queueFbCmntImgUrl.Enqueue(TemppageNodeItem);
                    }
                    ArrayList arrlist1 = new ArrayList();
                    ArrayList arrlist2 = new ArrayList();
                    FbPageInfo fbPageInfo = new FbPageInfo();

                var emailElement1 = chromeWebDriver.FindElements(By.XPath("//a[@class='_39g5']"));
                        foreach (var item in emailElement1)
                        {
                            string lin1k = item.GetAttribute("href");
                            if (item.GetAttribute("href").Contains("/live_video/launch_composer/?page_id="))
                            {
                                var pageId = lin1k.Replace("https://www.facebook.com/live_video/launch_composer/?page_id=", "");
                                arrlist1.Add(pageId);
                                //fbPageInfo.FbPageId = pageId;

                            }
                            if (item.GetAttribute("href").Contains("?modal=composer&ref=www_pages_browser_your_pages_section"))
                            {
                                string pageName = lin1k.Replace("https://www.facebook.com/", "").Replace("/?modal=composer&ref=www_pages_browser_your_pages_section", "");
                                arrlist2.Add(pageName);

                            }
                           
                        }
                    var dbHelper = new DbHelper();

                    for (int i = 0; i < queueFbCmntImgUrl.Count; i++)
                    {
                        fbPageInfo.FbPageId= arrlist1[i].ToString();
                        fbPageInfo.FbPageName = arrlist2[i].ToString();
                        fbPageInfo.FbPageUrl = queueFbCmntImgUrl.Dequeue();
                        listFbPageInfo.Add(fbPageInfo);
                        //     _dbHelper.GetLoginUsers();
                        dbHelper.AddFacebookPage(fbPageInfo.FbPageId,fbPageInfo.FbPageName,fbPageInfo.FbPageUrl,userName);
                        
                    }

                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                chromeWebDriver.Quit();
            }
        }

        public static void InsertFacebookCommentToDb(List<FbUserMessageInfo>  messagingFbpageListInfo)
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
        public void GetInstaMesages()
        {
            messagingFbpageListInfo = new List<FbUserMessageInfo>();
            var chromeWebDriver = GetDriver();
            try
            {
                Queue<string> queueInstaImgUrl = new Queue<string>();

                
                var dbHelper = new DbHelper();
                Thread.Sleep(3000);
                ListUsernameInfo listUsernameInfo = new ListUsernameInfo();
                string url = "https://www.facebook.com/TP-1996120520653285/inbox/";
                chromeWebDriver.Navigate().GoToUrl(url);

              //  url = "https://www.facebook.com/TP-1996120520653285/inbox/";

                
                SetCookies(chromeWebDriver);
                chromeWebDriver.Navigate().GoToUrl(url);
                Thread.Sleep(10000);
              //  Thread.Sleep(3000);

                ReadOnlyCollection<IWebElement> collection = chromeWebDriver.FindElements(By.ClassName("_32wr"));
                {
                    if (collection.Count > 0)
                    {
                        collection[2].Click();
                        Thread.Sleep(3000);
                    }
                }

                 ReadOnlyCollection<IWebElement> profilIdtempnode = chromeWebDriver.FindElements(By.XPath("//div[@data-click='profile_icon']/a"));
                if (profilIdtempnode.Count > 0)
                {
                    var urls = profilIdtempnode[0].GetAttribute("href").ToString();
                    profilIdtempinsta = urls.Split('?')[1].Split('=')[1].ToString();
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
                        dbHelper.InsertInstagramMessage(userName, currentURL, imgUrl);



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

                            Regex timeRegex = new Regex(@"utime=(.*?)<");
                            Match matchtime = timeRegex.Match(commentitem.OuterHtml);
                            string msgTimeng = matchtime.Value.Replace("utime=", "").Replace("<", "");
                            var instaCommentTime = msgTimeng.Split('>')[1];

                        }


                        dbHelper.InsertFacebookCommentToDb(messagingFbpageListInfo, profilIdtempinsta);
                    }
                }

                // var chromeWebDriver = GetDriver();
                // chromeWebDriver.Navigate().GoToUrl(SelectedFBPageInfo.FBInboxNavigationUrl); navigationUrl
               // chromeWebDriver.Navigate().GoToUrl("https://www.facebook.com/TP-1996120520653285/inbox/?selected_item_id=1996142970651040");

                

            }
            catch (Exception)
            {

            }
            finally
            {
                chromeWebDriver.Quit();
            }
        }
        public void GetFacebookMessages()
        {
            messagingFbpageListInfo = new List<FbUserMessageInfo>(); ;
            var chromeWebDriver = GetDriver();
            try
            {
               
                DbHelper dbHelper = new DbHelper();
                List<ListUsernameInfo> _MyListUsernameInfo = new List<ListUsernameInfo>();
                Queue<string> myQueue = new Queue<string>();
                string url = "https://www.facebook.com/TP-1996120520653285/inbox/";
                chromeWebDriver.Navigate().GoToUrl(url);
                SetCookies(chromeWebDriver);
                chromeWebDriver.Navigate().GoToUrl(url);
                Thread.Sleep(3000);
                ReadOnlyCollection<IWebElement> LeftTabTempnode = chromeWebDriver.FindElements(By.ClassName("_32wr"));
                if (LeftTabTempnode.Count>0)
                {
                    LeftTabTempnode[1].Click();
                }
                ReadOnlyCollection<IWebElement> profilIdtempnode = chromeWebDriver.FindElements(By.XPath("//div[@data-click='profile_icon']/a"));
                if (profilIdtempnode.Count > 0)
                {
                    var urls = profilIdtempnode[0].GetAttribute("href").ToString();
                  profilIdtempFb = urls.Split('?')[1].Split('=')[1].ToString();
                }

                ListUsernameInfo listUsernameInfo = new ListUsernameInfo();
                var PageSource = chromeWebDriver.PageSource;

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(PageSource);

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
                        // Thread.Sleep(3000);
                        string userName = itemurl.Text;
                        listUsernameInfo.ListUsername = userName;
                        var currentURL = chromeWebDriver.Url;
                        var tempId = currentURL.Split('?')[1].Split('=')[1];
                        listUsernameInfo.ListUserId = tempId;
                        listUsernameInfo.InboxNavigationUrl = currentURL;
                        _MyListUsernameInfo.Add(listUsernameInfo);
                        var imgUrl = myQueue.Dequeue();
                        dbHelper.InsertFbMessengerMessage(listUsernameInfo, userName, imgUrl);


                        ///////////////
                        Thread.Sleep(3000);
                        chromeWebDriver.Navigate().GoToUrl(currentURL);
                        var pageSource = chromeWebDriver.PageSource;
                        htmlDocument = new HtmlDocument();
                        htmlDocument.LoadHtml(pageSource);

                        HtmlNodeCollection commentNode = htmlDocument.DocumentNode.SelectNodes("//div[@class='_5v3q _5jmm _5pat _11m5']");
                        messagingFbpageListInfo = new List<FbUserMessageInfo>();
                        foreach (HtmlNode htmlcommentNode in commentNode)
                        {
                            HtmlNode selectNode = htmlcommentNode.SelectSingleNode("//div[@class='_4vv0 _3ccb']");
                            var pagename = selectNode.InnerText;
                            messagingFbpageListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = pagename, otheruserId = tempId });

                            HtmlNode pageimg = htmlcommentNode.SelectSingleNode("//img[@class='scaledImageFitWidth img']");

                            var imgsrc = pageimg.Attributes["src"].Value.Replace(";", "&");
                            messagingFbpageListInfo.Add(new FbUserMessageInfo { UserType = 3, loginguserFbimage = imgsrc, otheruserId = tempId });
                        }

                        HtmlNodeCollection commentBlock = htmlDocument.DocumentNode.SelectNodes("//div[@class='UFIImageBlockContent _42ef']");
                        var commentImg = string.Empty;
                        foreach (HtmlNode commentitem in commentBlock)
                        {
                            var pagenamee = commentitem.InnerText;
                            var comment = pagenamee.Replace("ManageLikeShow More Reactions ", "").Split('·');
                            var fbComment = comment[0];

                            Regex timeRegex = new Regex(@"title=(.*?)data");
                            Match matchtime = timeRegex.Match(commentitem.OuterHtml);
                            string msgTimeng = matchtime.Value.Replace("title=", "").Replace("data", "").Replace(@"""", "");



                            Regex regex = new Regex(@"src(.*?)alt");
                            Match match = regex.Match(commentitem.InnerHtml);
                            if (match.Length != 0)
                            {
                                string[] msgId = match.Value.Replace(";", "&").Split('"');
                                var img = msgId[1];

                                messagingFbpageListInfo.Add(new FbUserMessageInfo { UserType = 2, otheruserFbimage = img, otheruserId = tempId });
                            }
                        }
                        dbHelper.InsertFacebookCommentToDb(messagingFbpageListInfo, profilIdtempFb);
                        ///////////////







                    }
                }

                //chromeWebDriver.Navigate().GoToUrl("https://www.facebook.com/TP-1996120520653285/inbox/?selected_item_id=1996233970641940");

            
            }
            catch (Exception)
            {

            }
            finally
            {
                chromeWebDriver.Quit();
            }
        }

        //GetFacebookMessages
        //GetFbMessengerMessages



        public void GetFbMessengerMessages()
        {
            var chromeWebDriver = GetDriver();
            try
            {
                List<ListUsernameInfo> _MyListUsernameInfo = new List<ListUsernameInfo>();
                Queue<string> myQueue = new Queue<string>();
                ListUsernameInfo listUsernameInfo = new ListUsernameInfo();
                string url = "https://www.facebook.com/TP-1996120520653285/inbox/";
                chromeWebDriver.Navigate().GoToUrl(url);
                SetCookies(chromeWebDriver);
                chromeWebDriver.Navigate().GoToUrl(url);

                ReadOnlyCollection<IWebElement> LeftTabTempnode = chromeWebDriver.FindElements(By.ClassName("_32wr"));
                if (LeftTabTempnode.Count > 0)
                {
                    LeftTabTempnode[0].Click();
                }

                ReadOnlyCollection<IWebElement> profilIdtempnode = chromeWebDriver.FindElements(By.XPath("//div[@data-click='profile_icon']/a"));
                if (profilIdtempnode.Count > 0)
                {
                    var urls = profilIdtempnode[0].GetAttribute("href").ToString();
                    profilIdtempmsngr = urls.Split('?')[1].Split('=')[1].ToString();
                }

                var PageSource = chromeWebDriver.PageSource;
              
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(PageSource);

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
                var dbHelper = new DbHelper();
                ReadOnlyCollection<IWebElement> userlistnode = chromeWebDriver.FindElements(By.ClassName("_4k8x"));
                if (userlistnode.Count > 0)
                {
                    foreach (var itemurl in userlistnode)
                    {
                        Thread.Sleep(1000);
                        itemurl.Click();
                        // Thread.Sleep(3000);
                        string userName = itemurl.Text;
                        listUsernameInfo.ListUsername = userName;
                        var currentURL = chromeWebDriver.Url;
                        var tempId = currentURL.Split('?')[1].Split('=')[1];
                        listUsernameInfo.ListUserId = tempId;
                        listUsernameInfo.InboxNavigationUrl = currentURL;
                        _MyListUsernameInfo.Add(listUsernameInfo);
                        var imgUrl = myQueue.Dequeue();

                        
                        dbHelper.InsertFbMessengerMessage(listUsernameInfo, userName, imgUrl);


                        Thread.Sleep(2000);
                        var plateformType = "1";
                        var pageSource = chromeWebDriver.PageSource;
                        htmlDocument = new HtmlDocument();
                        htmlDocument.LoadHtml(pageSource);
                        Thread.Sleep(2000);


                        HtmlNodeCollection imgNodee = htmlDocument.DocumentNode.SelectNodes("//div[@class='_41ud']");

                        foreach (HtmlNode htmlNodeDiv in imgNodee)
                        {
                            

                            var selectSingleNode = htmlNodeDiv.SelectSingleNode("//div[@class='clearfix _o46 _3erg _29_7 direction_ltr text_align_ltr']");

                            if (selectSingleNode != null)
                            {
                                string otheruser = selectSingleNode.InnerText;

                                Regex timeRegex = new Regex(@"data-tooltip-content(.*?)data");
                                Match match1 = timeRegex.Match(selectSingleNode.OuterHtml);
                                string msgTimeng = match1.Value.Replace("data-tooltip-content=", "").Replace("data", "").Replace(@"""", "");
                                messagingFbpageListInfo.Add(new FbUserMessageInfo { UserType = 1, Message = otheruser, OtherUserDateTime = msgTimeng });

                            }

                            var selectSingleimgNode = htmlNodeDiv.SelectSingleNode(".//*[@class='clearfix _o46 _3erg _29_7 direction_ltr text_align_ltr _ylc']");
                            if (selectSingleimgNode != null)
                            {
                                Regex regex = new Regex(@"src(.*?)style");
                                Match match = regex.Match(selectSingleimgNode.InnerHtml);
                                string imgId = match.Value.Replace("src=", "").Replace("style", "").Replace("\"", "").Replace(@"""", "").Replace("amp;", "");

                                Regex timeRegex = new Regex(@"data-tooltip-content(.*?)data");
                                Match match1 = timeRegex.Match(selectSingleimgNode.OuterHtml);
                                string msgTimeng = match1.Value.Replace("data-tooltip-content=", "").Replace("data", "").Replace(@"""", "");
                                messagingFbpageListInfo.Add(new FbUserMessageInfo { UserType = 1, OtherUserDateTime = msgTimeng, otheruserimage = imgId });

                            }
                            var selectSingleNode2 = htmlNodeDiv.SelectSingleNode(".//*[@class='clearfix _o46 _3erg _3i_m _nd_ direction_ltr text_align_ltr']");
                            if (selectSingleNode2 != null)
                            {
                                string loginuser = selectSingleNode2.InnerText;

                                Regex timeRegex = new Regex(@"data-tooltip-content(.*?)data");
                                Match match = timeRegex.Match(selectSingleNode2.OuterHtml);
                                string msgTimeng = match.Value.Replace("data-tooltip-content=", "").Replace("data", "").Replace(@"""", "");
                                messagingFbpageListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = loginuser, OtherUserDateTime = msgTimeng});
                            }

                            HtmlNode selectSingleimgRightNode = htmlNodeDiv.SelectSingleNode(".//*[@class='clearfix _o46 _3erg _3i_m _nd_ direction_ltr text_align_ltr _ylc']");
                            if (selectSingleimgRightNode != null)
                            {
                                Regex regex = new Regex(@"src(.*?)style");
                                Match match = regex.Match(selectSingleimgRightNode.InnerHtml);
                                string msgId = match.Value.Replace("src=", "").Replace("style", "").Replace("\"", "").Replace(@"""", "").Replace("amp;", "");


                                Regex timeRegex = new Regex(@"data-tooltip-content(.*?)data");
                                Match match1 = timeRegex.Match(selectSingleimgRightNode.OuterHtml);
                                string msgTimeng = match1.Value.Replace("data-tooltip-content=", "").Replace("data", "").Replace(@"""", "");
                                messagingFbpageListInfo.Add(new FbUserMessageInfo { UserType = 0, loginguserimage = msgId, OtherUserDateTime = msgTimeng });
                            }
                            dbHelper.InsertFacebookCommentToDb(messagingFbpageListInfo, profilIdtempmsngr);
                        }




                    }
                }

             //   chromeWebDriver.Navigate().GoToUrl("https://www.facebook.com/TP-1996120520653285/inbox/?selected_item_id=100002324267540");
               
             //   Thread.Sleep(1000);

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
            catch (Exception)
            {


            }
            finally
            {
                chromeWebDriver.Quit();
            }
        }




        public string profilIdtempmsngr { get; set; }

        public string profilIdtempinsta { get; set; }

        public string profilIdtempFb { get; set; }
    }
}
