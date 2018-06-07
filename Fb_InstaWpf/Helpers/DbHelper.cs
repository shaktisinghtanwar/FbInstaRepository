using Fb_InstaWpf.Helper;
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
            var sql = GetSqliteHelper();
            string query = "INSERT INTO Jobs(From_User_id,To_UserId, User_Message,Image_Path,MessageType) values('" + message.FromUserId + "','" + message.ToUserId + "','" + message.Message + "','" + message.ImagePath + "','" + (int)message.MessageType + "')";
            int yy = sql.ExecuteNonQuery(query);
        }

        public ObservableCollection<FacebookUserLoginInfo> GetLoginUsers()
        {
           // var usersList = _databaseContext.Users.ToList();

            var sql = GetSqliteHelper();
            string query = "select * from TBLLogin";
            var dt = sql.GetDataTable(query);
            ObservableCollection<FacebookUserLoginInfo> users = new ObservableCollection<FacebookUserLoginInfo>();
            foreach (DataRow row in dt.Rows)
            {
                users.Add(new FacebookUserLoginInfo() { LoginUserName = row[1].ToString(), UserId = row[0].ToString(), Password = row[2].ToString() });
            }
            return users;
        }
        public void InsertFacebookMessage(ListUsernameInfo listUsernameInfo, string userName, string currentURL, string imgUrl)
        {
            var sql = GetSqliteHelper();
            string query = "INSERT INTO TblFbComment(Fbcomment_InboxUserId, Fbcomment_InboxUserName,Fbcomment_InboxUserImage,FBInboxNavigationUrl,Status) values('" + listUsernameInfo.ListUserId + "','" + userName + "','" + imgUrl + "','" + currentURL + "','" + false + "')";
            int yy = sql.ExecuteNonQuery(query);
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

                string query1 = "select Count(*) from TblMessengerList where M_InboxUserId='" + listUsernameInfo.ListUserId + "'";

                int count = Convert.ToInt32(sql1.ExecuteScalar(query1));
                if (count == 0)
                {
                    string query =
                        "INSERT INTO TblMessengerList(M_InboxUserId,M_inboxUserName,M_InboxUserImage,M_InboxNavigationUrl,Status) values('" +
                        listUsernameInfo.ListUserId + "','" + userName + "','" + imgUrl + "','" +
                        listUsernameInfo.InboxNavigationUrl + "','" + false + "')";
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

        public ObservableCollection<SocialUser> GetFbMessengerListData(string userId)
        {
            SqLiteHelper sql = new SqLiteHelper();
            ObservableCollection<SocialUser> userListInfo = new ObservableCollection<SocialUser>();
            string query = "select M_InboxUserId,M_inboxUserName,M_InboxUserImage,M_InboxNavigationUrl from TblMessengerList where Parent_User_Id='"+ userId +"'";
            var dt = sql.GetDataTable(query);
            foreach (DataRow item in dt.Rows)
            {

                string inboxUserId = Convert.ToString(item["M_InboxUserId"]);
                string inboxUserName = Convert.ToString(item["M_inboxUserName"]);
                string inboxUserImage = Convert.ToString(item["M_InboxUserImage"]);
                string inboxNavigationUrl = Convert.ToString(item["M_InboxNavigationUrl"]);
                //if (!UserListInfo.Any(m => m.InboxUserName.Equals(M_inboxUserName)))
                if (!userListInfo.Any(m => m.InboxUserName.Equals(inboxUserName)))
                {
                    userListInfo.Add(new SocialUser() { InboxUserId = inboxUserId, InboxUserName = inboxUserName,
                        InboxUserImage = inboxUserImage, InboxNavigationUrl = inboxNavigationUrl, MessageUserType = TabType.Messenger.ToString() });

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

   

        internal void InsertFacebookCommentToDb(List<FbUserMessageInfo> messagingFbpageListInfo)
        {
            throw new NotImplementedException();
        }


        public ObservableCollection<FbUserMessageInfo> GetMessengerUserComments(string userId)
        {
            ObservableCollection<FbUserMessageInfo> messagingListInfo = new ObservableCollection<FbUserMessageInfo>();
            string query = "select M_InboxUserId,PlateformType,PostType,Message,ImgSource from TblJob where M_InboxUserId='" + userId + "'";

            var dt = GetSqliteHelper().GetDataTable(query);
            foreach (DataRow item in dt.Rows)
            {
                string inboxUserId = Convert.ToString(item["M_InboxUserId"]);
                string PlateformType = Convert.ToString(item["PlateformType"]);
                string PostType = Convert.ToString(item["PostType"]);
                string Message = Convert.ToString(item["Message"]);
                string ImgSource = Convert.ToString(item["ImgSource"]);
                if (PostType == "0")
                {
                    messagingListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = Message, loginguserFbimage = ImgSource });
                }
                else if (PostType == "1")
                {
                    messagingListInfo.Add(new FbUserMessageInfo { UserType = 1, Message = Message, otheruserimage = ImgSource });
                }

                messagingListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = Message });
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


     
    }
}
