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

namespace Fb_InstaWpf
{
    public class DbHelper
    {
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
            var sql = GetSqliteHelper();

            string query =
                "INSERT INTO TblMessengerList(M_InboxUserId,M_inboxUserName,M_InboxUserImage,M_InboxNavigationUrl,Status) values('" +
                listUsernameInfo.ListUserId + "','" + userName + "','" + imgUrl + "','" +
                listUsernameInfo.InboxNavigationUrl + "','" + false + "')";
            int yy = sql.ExecuteNonQuery(query);
        }


        public ObservableCollection<FacebookPageInboxmember> GetFacebookListData()
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

        public ObservableCollection<FbpageInboxUserInfo> GetFbMessengerListData(string userId)
        {
            SqLiteHelper sql = new SqLiteHelper();
            ObservableCollection<FbpageInboxUserInfo> userListInfo = new ObservableCollection<FbpageInboxUserInfo>();
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
                    userListInfo.Add(new FbpageInboxUserInfo() { InboxUserId = inboxUserId, InboxUserName = inboxUserName, InboxUserImage = inboxUserImage, InboxNavigationUrl = inboxNavigationUrl });

                }
            }
            return userListInfo;
        }

        internal void InsertFacebookCommentToDb(List<FbUserMessageInfo> messagingFbpageListInfo)
        {
            throw new NotImplementedException();
        }
    }
}
