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
        DbHelper _dbHelper;
        private ObservableCollection<FbUserMessageInfo> _messages;
      

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
            get => _inboxUserId;
            set
            {
                _inboxUserId = value;
                LoadMessages();
                OnPropertyChanged();
            }
        }

        public string InboxUserImage
        {
            get => _inboxUserImage;
            set
            {
                _inboxUserImage = value;
                OnPropertyChanged();
            }
        }

        public string InboxUserName
        {
            get => _inboxUserName;
            set
            {
                _inboxUserName = value;
                OnPropertyChanged();
            }
        }
        public string InboxNavigationUrl
        {
            get => _inboxNavigationUrl;
            set
            {
                _inboxNavigationUrl = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<FbUserMessageInfo> Messages {
            get => _messages;
            set
            {
                _messages = value;
                OnPropertyChanged();
            }
        }

        public string MessageUserType
        {
            get => _messageUserType;
            set
            {
                _messageUserType = value;
                OnPropertyChanged();
            }
        }


    }


}
