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

   
}
