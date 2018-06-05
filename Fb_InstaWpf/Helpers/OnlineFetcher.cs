using Fb_InstaWpf.Helper;
using Fb_InstaWpf.Model;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Fb_InstaWpf
{
    public class OnlineFetcher
    {        
        public static ChromeDriver GetDriver()
        {
            return new ChromeDriver();
        }

        DbHelper _dbHelper;     

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
            {
                Queue<string> myQueue = new Queue<string>();
                var ChromeWebDriver = GetDriver();
                ListUsernameInfo listUsernameInfo = new ListUsernameInfo();
                var PageSource = ChromeWebDriver.PageSource;
                Thread.Sleep(3000);
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(PageSource);
                Thread.Sleep(3000);
                HtmlNodeCollection imgNode =
                    htmlDocument.DocumentNode.SelectNodes(
                        "//*[@id='u_0_t']/div/div/div/table/tbody/tr/td[1]/div/div[2]/div/div[1]/div/div/div/div/div/div/img");
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
