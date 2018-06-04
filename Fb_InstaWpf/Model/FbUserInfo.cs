using Fb_InstaWpf.Helper;
using Fb_InstaWpf.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Fb_InstaWpf.Model
{
   public class FbUserInfo :INotifyPropertyChanged
    {
       private bool _select;
       private int _id;
       private string _message;
       private string _pageUrl;

       public bool Select
       {
           get { return _select; }
           set { _select = value;OnPropertyChanged(); }
       }

       public int Id
       {
           get { return _id; }
           set { _id = value; OnPropertyChanged(); }
       }

       public string Message
       {
           get { return _message; }
           set { _message = value; OnPropertyChanged(); }
       }

       public string PageUrl
       {
           get { return _pageUrl; }
           set { _pageUrl = value; OnPropertyChanged(); }
       }

       public event PropertyChangedEventHandler PropertyChanged;

       protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
       {
           var handler = PropertyChanged;
           if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
       }
    }


    public class FacebookUserLoginInfo
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string LoginUserName { get; set; }
        
    }

    public class UserMsgTabItem: BaseViewModel
    {
        public DelegateCommand CloseTabCommand { get; set; }

        public UserMsgTabItem()
        {
                        CloseTabCommand = new DelegateCommand(CloseTab);

        }
        private void CloseTab(object obj)
        {
            string tabName = obj.ToString();
            MessagingListInfo.Remove(messagingListInfo.FirstOrDefault(m => m.UserName == Header));
        }
        public string Header { get; set; }
        public string HeaderFb { get; set; }
        public string HeaderInsta { get; set;}

        private ObservableCollection<FbUserMessageInfo> messagingListInfo;
        public ObservableCollection<FbUserMessageInfo> MessagingListInfo
        {

            get { return messagingListInfo; }
            set
            {
                messagingListInfo = value;
              
                OnPropertyChanged("MessagingListInfo");

            }
        }

     

        
    }


    public class UserFbMsgTabItem : BaseViewModel
    {
        public string Header { get; set; }
        public string HeaderFb { get; set; }
        //public string HeaderInsta { get; set; }
        private ObservableCollection<FbUserMessageInfo> messagingListInfo { get; set; }
        public ObservableCollection<FbUserMessageInfo> MessagingListInfo
        {

            get { return messagingListInfo; }
            set
            {
                messagingListInfo = value;
                OnPropertyChanged("MessagingListInfo");

            }
        }

       

    }

    public class UserInstaMsgTabItem : BaseViewModel
    {
        //public string Header { get; set; }
        //public string HeaderFb { get; set; }
        public string HeaderInsta { get; set; }
        private ObservableCollection<FbUserMessageInfo> messagingListInfo { get; set; }
        public ObservableCollection<FbUserMessageInfo> MessagingListInfo
        {

            get { return messagingListInfo; }
            set
            {
                messagingListInfo = value;
                OnPropertyChanged("MessagingListInfo");

            }
        }
       
       
    }

}
