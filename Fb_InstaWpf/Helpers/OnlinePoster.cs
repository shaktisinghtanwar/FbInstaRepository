using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Threading;

namespace Fb_InstaWpf
{
    public class OnlinePoster
    {
        ConcurrentQueue<PostMessage> _producerConsumerCollection;

        public OnlinePoster()
        {
            _producerConsumerCollection = new ConcurrentQueue<PostMessage>();

           // RestartThread();
        }
      //
        //public void ProcessMessage()
        //{
        //    PostMessage message;
        //    try
        //    {
        //        if (_producerConsumerCollection.TryPeek(out message))
        //        {
        //            if (TryMessagePosting(message))
        //            {
        //                _producerConsumerCollection.TryDequeue(out message);
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        //Log message
        //    }
        //    finally
        //    {
        //        RestartThread();
        //    }

        //}

        //private void RestartThread()
        //{
        //    System.Threading.Thread thread;
        //    thread = new System.Threading.Thread(ProcessMessage);
        //    thread.Start();
        //}

        private  bool TryMessagePosting(PostMessage message)
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
        public  ChromeDriver GetDriver()
        {
            return new ChromeDriver();
        }

        public bool TryPostMessageToFBMessenger(PostMessage message)
        {
            try
            {
                 var chromeWebDriver = GetDriver();
                chromeWebDriver.Navigate().GoToUrl("https://www.facebook.com/TP-1996120520653285/inbox/?selected_item_id=100002324267540");
                Thread.Sleep(2000);
                OnlineFetcher.SetCookies(chromeWebDriver);
                chromeWebDriver.Navigate().GoToUrl("https://www.facebook.com/TP-1996120520653285/inbox/?selected_item_id=100002324267540");

                //  ChromeWebDriver.Navigate().GoToUrl("https://www.facebook.com/pages/?category=your_pages");
                string pageSource = chromeWebDriver.PageSource;


                ReadOnlyCollection<IWebElement> writeNode =
                       chromeWebDriver.FindElements(By.XPath("//*[@placeholder='Write a reply...']"));
                if (writeNode.Count > 0)
                {
                    Thread.Sleep(2000);
                    writeNode[0].SendKeys(message.Message);
                    Thread.Sleep(2000);
                }

                ReadOnlyCollection<IWebElement> submitnode =
                       chromeWebDriver.FindElements(By.XPath("//*[@type='submit']"));
                if (submitnode.Count > 0)
                {
                    Thread.Sleep(2000);
                    submitnode[1].Click();
                    Thread.Sleep(2000);
                }
            }
            catch (Exception)
            {

            }
            return false;
        }

        public bool TryPostImageToFBMessenger(PostMessage message)
        {
            var ChromeWebDriver = GetDriver();
            //string url = "https://www.facebook.com/TP-1996120520653285/inbox/?selected_item_id=100002324267540";
            //ChromeWebDriver.Navigate().GoToUrl(url);
            ReadOnlyCollection<IWebElement> emailElement = ChromeWebDriver.FindElements(By.ClassName("_4dvy"));
            if (emailElement.Count > 0)
            {
                emailElement[0].Click();
            }
            Thread.Sleep(7000);

            ReadOnlyCollection<IWebElement> sendimage = ChromeWebDriver.FindElements(By.ClassName("_4dw3"));
            if (sendimage.Count > 0)
            {
                Thread.Sleep(3000);
                sendimage[0].Click();
                Thread.Sleep(3000);
            }
            return false;
        }

        public bool TryPostImageToFacebook(PostMessage message)
        {
            try
            {
                var ChromeWebDriver = GetDriver();
                ReadOnlyCollection<IWebElement> emailElement1 = ChromeWebDriver.FindElements(By.ClassName("UFICommentPhotoIcon"));
                if (emailElement1.Count > 0)
                {
                    emailElement1[0].Click();

                }
                Thread.Sleep(7000);

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


        public bool TryPostMessageToFacebook(PostMessage message)
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

        public bool TryPostMessageToInsta(PostMessage message)
        {
            var ChromeWebDriver = GetDriver();
            //string url = "https://www.facebook.com/TP-1996120520653285/inbox/?selected_item_id=1996142927317711";
            //_chromeWebDriver.Navigate().GoToUrl(url);
            Thread.Sleep(3000);
            var emailElement = ChromeWebDriver.FindElements(By.XPath("//input[@class='_58al']"));
            if (emailElement.Count > 0)
            {
                Thread.Sleep(3000);
                emailElement[0].SendKeys("hiiii");
            }
            Thread.Sleep(3000);
            var sendmessage = ChromeWebDriver.FindElements(By.XPath("//div[@class='_1fn8 _45dg']"));
            //ReadOnlyCollection<IWebElement> sendmessage = _chromeWebDriver.FindElements(By.ClassName("input"));
            if (sendmessage.Count > 0)
            {
                sendmessage[0].Click();
            }
            return true;
        }

    }
}
