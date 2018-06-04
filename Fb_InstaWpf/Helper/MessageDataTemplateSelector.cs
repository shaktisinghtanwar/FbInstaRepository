using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Fb_InstaWpf.Model;

namespace Fb_InstaWpf.Helper
{
    public class MessageDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate OtherUserTemplate;
        public DataTemplate LoginUserTemplate;
        public DataTemplate LoginImgUserTemplate;
        public DataTemplate otherImgUserTemplate;
        public DataTemplate DataTime;
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
            {
                return null;
            }
            FrameworkElement FrameworkElement = container as FrameworkElement;
            if (FrameworkElement != null)
            {
                var fbUserMessageInfo = item as FbUserMessageInfo;
                if (fbUserMessageInfo != null)
                {
                    if (fbUserMessageInfo.UserType == 0)
                    {
                        LoginUserTemplate = FrameworkElement.FindResource("LoginUserDataTemplate") as DataTemplate;
                        return LoginUserTemplate;
                    }
                    else if (fbUserMessageInfo.UserType == 1)
                    {
                        OtherUserTemplate = FrameworkElement.FindResource("OtherUserDataTemplate") as DataTemplate;
                        return OtherUserTemplate;
                    }
                    else if (fbUserMessageInfo.UserType == 2)
                    {
                        otherImgUserTemplate = FrameworkElement.FindResource("OtherimgDataTemplate") as DataTemplate;
                        return otherImgUserTemplate;
                    }

                    else
                    {
                        LoginImgUserTemplate = FrameworkElement.FindResource("LoginimgDataTemplate") as DataTemplate;
                        return LoginImgUserTemplate;
                    }



                }
            }
            return null;
        }
    }


    public class MessageFBComntDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate OtherUserTemplateForFbCmnt;
        public DataTemplate LoginUserTemplateForFbCmnt;
        public DataTemplate OtherimgForFbCmnt;
        public DataTemplate LoginimgForFbCmnt;
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
            {
                return null;
            }
            FrameworkElement FrameworkElement = container as FrameworkElement;
            if (FrameworkElement != null)
            {
                var fbUserMessageInfo = item as FbUserMessageInfo;
                if (fbUserMessageInfo != null)
                {
                    if (fbUserMessageInfo.UserType == 0)
                    {
                        LoginUserTemplateForFbCmnt = FrameworkElement.FindResource("LoginUserTemplateForFbCmnt") as DataTemplate;
                        return LoginUserTemplateForFbCmnt;
                    }
                    else if (fbUserMessageInfo.UserType == 2)
                     {
                         OtherimgForFbCmnt = FrameworkElement.FindResource("OtherimgForFbCmnt") as DataTemplate;
                         return OtherimgForFbCmnt;
                         
                     }
                    else if (fbUserMessageInfo.UserType == 3)
                        {
                            LoginimgForFbCmnt = FrameworkElement.FindResource("LoginimgForFbCmnt") as DataTemplate;
                            return LoginimgForFbCmnt;
                        }

                    else
                    {
                        OtherUserTemplateForFbCmnt = FrameworkElement.FindResource("OtherUserTemplateForFbCmnt") as DataTemplate;
                        return OtherUserTemplateForFbCmnt;
                    }
                }
            }
            return null;
        }

    }


    public class MessageInstaComntDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate OtherUserTemplateForInstaCmnt;
        public DataTemplate LoginUserTemplateForInstaCmnt;
        public DataTemplate OtherimgForInstaCmnt;
        public DataTemplate LoginimgForInstaCmnt;
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
            {
                return null;
            }
            FrameworkElement FrameworkElement = container as FrameworkElement;
            if (FrameworkElement != null)
            {
                var fbUserMessageInfo = item as FbUserMessageInfo;
                if (fbUserMessageInfo != null)
                {
                    if (fbUserMessageInfo.UserType == 0)
                    {
                        LoginUserTemplateForInstaCmnt = FrameworkElement.FindResource("LoginUserTemplateForInstaCmnt") as DataTemplate;
                        return LoginUserTemplateForInstaCmnt;
                    }
                    else if (fbUserMessageInfo.UserType == 2)
                    {
                        LoginimgForInstaCmnt = FrameworkElement.FindResource("LoginimgForInstaCmnt") as DataTemplate;
                        return LoginimgForInstaCmnt;
                        
                    }
                    else if (fbUserMessageInfo.UserType == 3)
                    {
                        OtherimgForInstaCmnt = FrameworkElement.FindResource("OtherimgForInstaCmnt") as DataTemplate;
                        return OtherimgForInstaCmnt;
                    }

                    else
                    {
                        OtherUserTemplateForInstaCmnt = FrameworkElement.FindResource("OtherUserTemplateForInstaCmnt") as DataTemplate;
                        return OtherUserTemplateForInstaCmnt;
                    }
                }
            }
            return null;
        }

    }
}
