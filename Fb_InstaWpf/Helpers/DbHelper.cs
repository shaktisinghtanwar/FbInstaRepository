﻿using Fb_InstaWpf.Helper;
using Fb_InstaWpf.Model;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading;
using HtmlAgilityPack;
using Fb_InstaWpf.DbModel;
using Fb_InstaWpf.Enums;
using Fb_InstaWpf.ViewModel;

namespace Fb_InstaWpf
{
    public class DbHelper
    {
        DatabaseContext _databaseContext;
        public DbHelper()
        {
            _databaseContext = new DatabaseContext();
        }

        private static SqLiteHelper GetSqliteHelper()
        {
            return new SqLiteHelper();
        }

		
        public void Add(PostMessage message)
        {
            //ObservableCollection<FbUserMessageInfo> messagingListInfo = new ObservableCollection<FbUserMessageInfo>();
            var sql = GetSqliteHelper();
            string query = "INSERT INTO Jobs(FromUserId,ToUserId, Message,ImagePath,MessageType,PlateformType) values('" + message.FromUserId + "','" + message.ToUserId + "','" + message.Message + "','" + message.ImagePath + "','" + (int)message.MessageType + "','" + "1" + "')";
            int yy = sql.ExecuteNonQuery(query);

            


        }
        public void AddLoginUser(SocialUser loginUser)
        {
            var sql = GetSqliteHelper();
            string query = "INSERT INTO Users(UserName,Password,FacebookId) values('" + loginUser.InboxUserName + "','" + loginUser.Password + "','" + loginUser.InboxUserId + "')";
            int yy = sql.ExecuteNonQuery(query);
        }

        public void AddFacebookPage(string pageId,string pagename,string pageurl,string loginuserId)
        {

            var sql1 = GetSqliteHelper();
            string query1 = "select Count(*) from FbPages where PageId='" + pageId + "'";

            int count = Convert.ToInt32(sql1.ExecuteScalar(query1));
            if (count == 0)
            {
                var sql = GetSqliteHelper();
                string query = "INSERT INTO FbPages(PageId,PageName,PageUrl,Parent_User_Id) values('" + pageId + "','" + pagename + "','" + pageurl + "','" + loginuserId + "')";
                int yy = sql.ExecuteNonQuery(query);
            }


        }     

        public ObservableCollection<SocialUser> GetLoginUsers()
        {
           // var usersList = _databaseContext.Users.ToList();

            var sql = GetSqliteHelper();
            //string query = "select * from TBLLogin";
            string query = "select * from Users";
            var dt = sql.GetDataTable(query);
            ObservableCollection<SocialUser> users = new ObservableCollection<SocialUser>();
            foreach (DataRow row in dt.Rows)
            {
                users.Add(new SocialUser() { UserId = row[0].ToString(), InboxUserId = row[3].ToString(), InboxUserName = row[1].ToString(), Password = row[2].ToString() });
            }
            return users;
        }
        public void InsertFacebookMessage(ListUsernameInfo listUsernameInfo, string userName, string currentURL, string imgUrl)
        {
            var sql = GetSqliteHelper();
            string query = "INSERT INTO TblFbComment(Fbcomment_InboxUserId, Fbcomment_InboxUserName,Fbcomment_InboxUserImage,FBInboxNavigationUrl,Status) values('" + listUsernameInfo.ListUserId + "','" + userName + "','" + imgUrl + "','" + currentURL + "','" + false + "')";
            int yy = sql.ExecuteNonQuery(query);
        }

        public  ObservableCollection<FbPageInfo> GetFacebookPage()
        {
            // var usersList = _databaseContext.Users.ToList();

            var sql = GetSqliteHelper();
            //string query = "select * from TBLLogin";
            string query = "select * from FbPages";
            var dt = sql.GetDataTable(query);
            ObservableCollection<FbPageInfo> pages = new ObservableCollection<FbPageInfo>();
            foreach (DataRow row in dt.Rows)
            {
                pages.Add(new FbPageInfo() { FbPageId = row[1].ToString(), FbPageName = row[2].ToString(), FbPageUrl = row[3].ToString() });
            }
            return pages;
        }


        public void InsertInstagramMessage(string userName, string currentURL, string imgUrl)
        {
            var sql = GetSqliteHelper();

            string query =
                "INSERT INTO Tbl_Instagram(Insta_inboxUserName,Insta_inboxUserImage,InstaInboxNavigationUrl,Status) values('" +
                userName + "','" + imgUrl + "','" + currentURL + "','" + false + "')";

            int yy = sql.ExecuteNonQuery(query);
        }

        public void InsertFbMessengerMessage( ListUsernameInfo listUsernameInfo, string userName, string imgUrl)
        {
            try
            {
                var sql = GetSqliteHelper();
                var sql1 = GetSqliteHelper();

                string query1 = "select Count(*) from FacebookUsers where FacebookId='" + listUsernameInfo.ListUserId + "'";

                int count = Convert.ToInt32(sql1.ExecuteScalar(query1));
                if (count == 0)
                {
                    string query =
                        "INSERT INTO FacebookUsers(FacebookId,DisplayName,ImageUrl,NavigationUrl,JobType,Parent_User_Id) values('" +
                        listUsernameInfo.ListUserId + "','" + userName + "','" + imgUrl + "','" +
                        listUsernameInfo.InboxNavigationUrl + "','1','100012494199316')";

                    
                    int yy = sql.ExecuteNonQuery(query);
                }
            
            }
            catch (Exception)
            {
                    
               
            }
        }


        public ObservableCollection<SocialUser> GetFacebookListData(string userId)
        {
            SqLiteHelper sql = GetSqliteHelper();
            ObservableCollection<SocialUser> FbPageListmembers = new ObservableCollection<SocialUser>();

            string query = "select Fbcomment_InboxUserId, Fbcomment_InboxUserName,Fbcomment_InboxUserImage,FBInboxNavigationUrl from TblFbComment where Parent_User_Id='" + userId + "'";
               
            var dt = sql.GetDataTable(query);
            foreach (DataRow item in dt.Rows)
            {
                string Fbcomment_InboxUserId = Convert.ToString(item["Fbcomment_InboxUserId"]);
                string Fbcomment_InboxUserName = Convert.ToString(item["Fbcomment_InboxUserName"]);
                string Fbcomment_InboxUserImage = Convert.ToString(item["Fbcomment_InboxUserImage"]);
                string FBInboxNavigationUrl = Convert.ToString(item["FBInboxNavigationUrl"]);
                if (!FbPageListmembers.Any(m => m.InboxUserName.Equals(Fbcomment_InboxUserName)))
                {
                    FbPageListmembers.Add(new SocialUser()
                    {
                        InboxUserId=Fbcomment_InboxUserId,
                        InboxUserName = Fbcomment_InboxUserName,
                        InboxUserImage = Fbcomment_InboxUserImage,
                        InboxNavigationUrl = FBInboxNavigationUrl,
                        MessageUserType = TabType.Facebook.ToString()
                    });
                }
            }

            return FbPageListmembers;
        }

        public ObservableCollection<SocialUser> GetLeftMessengerListData(string userId,TabType tabType,string pageid)
        {
            SqLiteHelper sql = new SqLiteHelper();
            ObservableCollection<SocialUser> userListInfo = new ObservableCollection<SocialUser>();


            //string query = string.Format("select FacebookId,DisplayName,ImageUrl,NavigationUrl from FacebookUsers where Parent_User_Id='{0}' and JobType={1} ", userId, (int)tabType);
            string query = "select FacebookId,DisplayName,ImageUrl,NavigationUrl,PageId from FacebookUsers where Parent_User_Id='" + userId + "' and JobType='" + (int)tabType + "' and PageId='" + pageid + "'";
          
            var dt = sql.GetDataTable(query);
            foreach (DataRow item in dt.Rows)
            {
                string inboxUserId = Convert.ToString(item["FacebookId"]);
                string inboxUserName = Convert.ToString(item["DisplayName"]);
                string inboxUserImage = Convert.ToString(item["ImageUrl"]);
                string inboxNavigationUrl = Convert.ToString(item["NavigationUrl"]);
                string pageId = Convert.ToString(item["PageId"]);

                //if (!UserListInfo.Any(m => m.InboxUserName.Equals(M_inboxUserName)))
                if (!userListInfo.Any(m => m.InboxUserId.Equals(inboxUserId)))
                {
                    userListInfo.Add(new SocialUser() { InboxUserId = inboxUserId, InboxUserName = inboxUserName,
                                                        InboxUserImage = inboxUserImage,
                                                        InboxNavigationUrl = inboxNavigationUrl,
                                                        MessageUserType = tabType.ToString(),
                                                        PageId = pageId
                    });

                }
            }
            return userListInfo;
        }

        //private void GetInstagramMessages(string userId)
        //{
        //    string query = "select Insta_inboxUserId,PlateformType,Message,ImageSource from TblJobFb where Fbcomment_InboxUserId='" + userId + "'";
        //    var dt = sql.GetDataTable(query);
        //    foreach (DataRow item in dt.Rows)
        //    {
        //        string InstainboxUserId = Convert.ToString(item["Insta_inboxUserId"]);
        //        string PlateformType = Convert.ToString(item["PlateformType"]);
        //        string Message = Convert.ToString(item["Message"]);
        //        string ImgSource = Convert.ToString(item["ImageSource"]);
        //        messagingInstapageListInfo.Add(new FbUserMessageInfo() { Message = Message });
        //    }
        //}

        public ObservableCollection<SocialUser> GetInstaUserList(string userId)
        {
            SqLiteHelper sql = new SqLiteHelper();

            ObservableCollection<SocialUser> InstaListmembers = new ObservableCollection<SocialUser>();
            string query = "select Insta_inboxUserId,Insta_inboxUserName,Insta_inboxUserImage,InstaInboxNavigationUrl from Tbl_Instagram where Parent_User_Id='"+ userId +"'";
            var dt = sql.GetDataTable(query);
            foreach (DataRow item in dt.Rows)
            {
                string InstainboxUserId = Convert.ToString(item["Insta_inboxUserId"]);
                string Insta_inboxUserName = Convert.ToString(item["Insta_inboxUserName"]);
                string Insta_inboxUserImage = Convert.ToString(item["Insta_inboxUserImage"]);
                string InstaInboxNavigationUrl = Convert.ToString(item["InstaInboxNavigationUrl"]);
                if (!InstaListmembers.Any(m => m.InboxUserName.Equals(Insta_inboxUserName)))
                {
                    InstaListmembers.Add(new SocialUser()
                    {
                        InboxUserId = InstainboxUserId,
                        InboxUserName = Insta_inboxUserName,
                        InboxUserImage = Insta_inboxUserImage,
                        InboxNavigationUrl = InstaInboxNavigationUrl,
                        MessageUserType = TabType.Instagram.ToString()
                    });
                }
            }
            return InstaListmembers;
        }

   

        public void InsertFacebookCommentToDb(List<FbUserMessageInfo> messagingFbpageListInfo,string userId)
        {
            var sql = GetSqliteHelper();
            foreach (var messagingFbpageListInfoItem in messagingFbpageListInfo)
            {
                string query = "Insert into Messages(FromUserId,ToUserId,Message,MessageType,MessageDate,ImagePath) values('" + messagingFbpageListInfoItem.otheruserId + "','" + userId + "','" + messagingFbpageListInfoItem.Message + "','" + messagingFbpageListInfoItem.UserType + "','" + messagingFbpageListInfoItem.OtherUserDateTime + "','" + messagingFbpageListInfoItem.loginguserFbimage + "')";
            int yy = sql.ExecuteNonQuery(query);
            }
          
        }


        public ObservableCollection<FbUserMessageInfo> GetMessengerUserComments(string userId)
        {
            ObservableCollection<FbUserMessageInfo> messagingListInfo = new ObservableCollection<FbUserMessageInfo>();
            //string query = "select M_InboxUserId,PlateformType,PostType,Message,ImgSource from TblJob where M_InboxUserId='" + userId + "'";

            string query = "select FromUserId,ToUserId,Message,MessageType,MessageDate,ImagePath from Messages where ToUserId='" + userId + "'";

           // string query = "select M_InboxUserId,PlateformType,PostType,Message,ImgSource from TblJob where M_InboxUserId='" + userId + "'";
            var dt = GetSqliteHelper().GetDataTable(query);
            foreach (DataRow item in dt.Rows)
            {
               
                string inboxUserId = Convert.ToString(item["FromUserId"]);
                string ToUserId = Convert.ToString(item["ToUserId"]);
                string Message = Convert.ToString(item["Message"]);
                string PostType = Convert.ToString(item["MessageType"]);
                string MessageDate = Convert.ToString(item["MessageDate"]);
                string ImagePath = Convert.ToString(item["ImagePath"]);

                if (PostType == "0")
                {
                    messagingListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = Message, loginguserFbimage = ImagePath });
                }
                else if (PostType == "1")
                {
                    messagingListInfo.Add(new FbUserMessageInfo { UserType = 1, Message = Message, otheruserimage = ImagePath });
                }

                else
                {
                    messagingListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = Message });
                }
            }
            return messagingListInfo;
        }


        private ObservableCollection<FbUserMessageInfo> GetFacebookUserComments(string userId)
        {
            ObservableCollection<FbUserMessageInfo> messagingListInfo = new ObservableCollection<FbUserMessageInfo>();
            string query = "select Fbcomment_InboxUserId,PlateformType,Message,ImageSource from TblJobFb where Fbcomment_InboxUserId='" + userId + "'";

            var dt = GetSqliteHelper().GetDataTable(query);
            foreach (DataRow item in dt.Rows)
            {

                string inboxUserId = Convert.ToString(item["Fbcomment_InboxUserId"]);
                string PlateformType = Convert.ToString(item["PlateformType"]);
                string Message = Convert.ToString(item["Message"]);
                string ImgSource = Convert.ToString(item["ImageSource"]);
                if (PlateformType == "0")
                {
                    messagingListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = Message, loginguserFbimage = ImgSource });
                }
                else if (PlateformType == "1")
                {
                    messagingListInfo.Add(new FbUserMessageInfo { UserType = 1, Message = Message, otheruserFbimage = ImgSource });
                }

                messagingListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = Message });
            }
            return messagingListInfo;
        }

		public ObservableCollection<PostMessage> GetMessages()
		{
		ObservableCollection<PostMessage> jobsMessage = new ObservableCollection<PostMessage>();
			try 
			{
			  var sql = GetSqliteHelper();
			  var query="select ToUserId,MessageType,Status,ToUrl,Message,Id from Jobs Where Status='0' And FromUserId='"+OnlineFetcher.profilLoginId+"'";			
			   var dt = sql.GetDataTable(query);			  
              foreach (DataRow row in dt.Rows)
              {
                jobsMessage.Add(new PostMessage() {Id=Convert.ToInt32(row[5].ToString()), MessageTypeResponse = Convert.ToInt32(row[1].ToString()), Status = Convert.ToInt32(row[2].ToString()), ToUrl = row[3].ToString(),ToUserId = row[0].ToString(),Message =row[4].ToString() });
				break;
              }			
			} catch(Exception) {
			}
			return jobsMessage;
		
		}		
	
	    public int UpdateMessageTable(int Id)
		{
		  int dt = 0;
			try {
			 var sql = GetSqliteHelper();
			 var query="Update Jobs set Status = '1' Where Id='"+Id+"'";			
			 dt = sql.ExecuteNonQuery(query);			 
			}
			catch(Exception ex) {			
			}
			return dt;
		}
     
    }
}
