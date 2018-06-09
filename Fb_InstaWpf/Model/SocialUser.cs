using Fb_InstaWpf.Enums;
using Fb_InstaWpf.ViewModel;
using System;
using System.Collections.ObjectModel;

namespace Fb_InstaWpf.Model
{
    public class SocialUser : BaseViewModel
    {
        private string _inboxUserId;
        private string _inboxUserImage;
        private string _inboxUserName;
        private string _inboxNavigationUrl;
        private string _messageUserType;
        private string _password;
        DbHelper _dbHelper;
        private ObservableCollection<FbUserMessageInfo> _messages;
        private string _userId;

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }

        }
        public SocialUser()
        {
            _dbHelper = new DbHelper();

        }

        private void LoadMessages()
        {
            //_messages = _dbHelper.GetFbMessengerListData(InboxUserId);
        }

        public string InboxUserId
        {
            get { return _inboxUserId; }
            set
            {
                _inboxUserId = value;
                LoadMessages();
                OnPropertyChanged();
            }
        }

        public string InboxUserImage
        {
            get { return _inboxUserImage; }
            set
            {
                _inboxUserImage = value;
                OnPropertyChanged();
            }
        }

        public string InboxUserName
        {
            get { return _inboxUserName; }
            set
            {
                _inboxUserName = value;
                OnPropertyChanged();
            }
        }
        public string InboxNavigationUrl
        {
            get { return _inboxNavigationUrl; }
            set
            {
                _inboxNavigationUrl = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<FbUserMessageInfo> Messages
        {
            get { return _messages; }
            set
            {
                _messages = value;
                OnPropertyChanged();
            }
        }

        public string MessageUserType
        {
            get { return _messageUserType; }
            set
            {
                _messageUserType = value;
                OnPropertyChanged();
            }
        }

        public string UserId
        {
            get { return _userId; }
            set
            {
                _userId = value;
                OnPropertyChanged();
            }
        }
    }

}
