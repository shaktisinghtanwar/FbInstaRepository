using Fb_InstaWpf.Enums;
using Fb_InstaWpf.Helper;
using Fb_InstaWpf.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;

namespace Fb_InstaWpf.ViewModel
{
    public class SocialTabViewModel : BaseViewModel
    {
        private ObservableCollection<SocialUser> _userListInfo;
        private SocialUser _loginUser;

        private SocialUser _selectedUserInfo;

        private SocialUser _activeTabUserInfo;
        OnlineFetcher _onlineFetcher;

        private ObservableCollection<SocialUser> _selectedUsers;

        PubSubEvent<SocialUser> _pubSubEvent;

        string _tabType;
        string _messageToSend;

        DbHelper _dbHelper=new DbHelper();
        
        public SocialUser SelectedItem
        {
            get {return _selectedUserInfo;}
            set
            {
                _selectedUserInfo = value;
                if ( _selectedUserInfo !=null)
                    BindUserMessage(_selectedUserInfo);
                OnPropertyChanged();
            }
        }

        public ObservableCollection<SocialUser> UserListInfo
        {
            get {return _userListInfo;}
            set
            {
                _userListInfo = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<SocialUser> SelectedUsers
        {
            get {return  _selectedUsers;}
            set
            {
                _selectedUsers = value;
                OnPropertyChanged();
            }
        }

        public SocialUser ActiveTabUser
        {
            get {return _activeTabUserInfo;}
            set
            {
                _activeTabUserInfo = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand SendMessageCommand { get; set; }
        public DelegateCommand SendimageCommand { get; set; }
        public DelegateCommand SendMessageInstaCommand { get; set; }
        public DelegateCommand SendimageFBCommand { get; set; }
        public DelegateCommand SendFbCommentCommand { get; set; }

        public SocialUser LoginUser
        {
            get {return _loginUser;}
            set
            {
                _loginUser = value;
                OnPropertyChanged();
            }
        }

        public string MessageToSend
        {
            get {return _messageToSend;}
            set
            {
                _messageToSend = value;
                OnPropertyChanged();
            }
        }

        public string TabType
        {
            get { return _tabType; }
            set
            {
                _tabType = value;
                OnPropertyChanged();
            }
        }

        public SocialTabViewModel(TabType tabType, SocialUser loginUser)
        {
            LoginUser = loginUser;
            TabType = tabType.ToString();
            SendMessageCommand = new DelegateCommand(SendMessageCommandHandler, null);
            SelectedUsers = new ObservableCollection<SocialUser>();
            SendMessageInstaCommand = new DelegateCommand(SendMessageInstaCommandhandlar, null);
            SendimageFBCommand = new DelegateCommand(SendimageFBCommandHandler, null);
            SendFbCommentCommand = new DelegateCommand(SendFbCommentCommandHandler, null);
            _onlineFetcher = new OnlineFetcher();
            _dbHelper = new DbHelper();
        }

        private void OnSelectedUserChanged(SocialUser socialUser)
        {
            if (SelectedUsers == null)
                SelectedUsers = new ObservableCollection<SocialUser>();

            if ( SelectedUsers.FirstOrDefault(s=> s.InboxUserId == socialUser.InboxUserId) == null)
                SelectedUsers.Add(socialUser);
        }

        private static string ShowDialogAndFetchFileName()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg)|*.png|All files (*.*)|*.*";
            string fileName = string.Empty;
            if (openFileDialog.ShowDialog() == true)
            {
                fileName = openFileDialog.FileName;
            }
            else
                MessageBox.Show("Please select image");
            return fileName;
        }

        private void SendImageCommandHandler(object obj)
        {
            string fileName = ShowDialogAndFetchFileName();

            PostMessage message = new PostMessage()
            {
                FromUserId = LoginUser.UserId,
                ToUserId = ActiveTabUser.InboxUserId,
                MessageType = MessageType.FacebookMessengerImage,
                ImagePath = fileName
            };
            _dbHelper.Add(message);
        }

        private void SendMessageInstaCommandhandlar(object obj)
        {
            _dbHelper.Add(new PostMessage()
            {
                FromUserId = LoginUser.InboxUserId,
                ToUserId = ActiveTabUser.InboxUserId,
                ImagePath = MessageToSend,
                MessageType = MessageType.InstaMessage
                 
            });
            MessageToSend = string.Empty;
        }

    

        public void SendMessageCommandHandler(object message)
        {

            
            try
            {

                _dbHelper.Add(new PostMessage()
                {
                    FromUserId = LoginUser.InboxUserId,
                    ToUserId = SelectedItem.InboxUserId,
                    Message = MessageToSend,
                    MessageType = MessageType.FacebookMessage,
                    ImagePath = null,
                    Status = 0,
                    ToUrl = SelectedItem.InboxNavigationUrl
                });
                MessageBox.Show("Message save successfully");
                _selectedUserInfo.Messages.Add(new FbUserMessageInfo { UserType = 0, Message = MessageToSend });
                MessageToSend = "";
            }
            catch (Exception)
            {
                    
                throw;
            }

        }
        private void SendimageFBCommandHandler(object obj)
        {
            _dbHelper.Add(new PostMessage()
            {
                FromUserId = LoginUser.InboxUserId,
                ToUserId = ActiveTabUser.InboxUserId,
                Message = MessageToSend,
                MessageType = MessageType.FacebookImage,
            });
            MessageToSend = string.Empty;
        }
        private void SendFbCommentCommandHandler(object obj)
        {
            _dbHelper.Add(new PostMessage()
            {
                FromUserId = LoginUser.InboxUserId,
                ToUserId = ActiveTabUser.InboxUserId,
                Message = MessageToSend,
                MessageType = MessageType.FacebookMessage
            });
            MessageToSend = string.Empty;
        }

        private void BindUserMessage(SocialUser fbpageInboxUserInfo)
        {
            if (!SelectedUsers.Any(m => m.InboxUserName.Equals(fbpageInboxUserInfo.InboxUserName)))
            {
                SelectedUsers.Add(fbpageInboxUserInfo);
            
               fbpageInboxUserInfo.Messages = _dbHelper.GetMessengerUserComments(fbpageInboxUserInfo.InboxUserId);
             
            }
        }
    }
}
