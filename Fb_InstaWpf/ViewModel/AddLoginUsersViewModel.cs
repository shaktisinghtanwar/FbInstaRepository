using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Text;
using Fb_InstaWpf.Helper;
using Fb_InstaWpf.Model;
using System.IO;
using System.Windows;

namespace Fb_InstaWpf.ViewModel
{
    public class AddLoginUsersViewModel : BaseViewModel
    {
        #region Filds

        public string TxtUserId { get; set; }
        public string TxtPassword { get; set; }
        public string Lblproxy { get; set; }
        private string fileName = Properties.Settings.Default.Filename;
        private ObservableCollection<FacebookUserLoginInfo> _newUserNameInfoList;
        private ObservableCollection<FacebookUserLoginInfo> _deleteListviewItem;

        #endregion

        #region Constructor

        public AddLoginUsersViewModel()
        {
            NewUserCommand = new DelegateCommand(NewUserCommandHandler, null);
            NewUserNameInfoList = new ObservableCollection<FacebookUserLoginInfo>();
            DeleteListviewItem = new ObservableCollection<FacebookUserLoginInfo>();
            clearListViewItemCommand = new DelegateCommand(clearListViewItemCommandHandler, null);
           // cmbUserLoaded = new DelegateCommand(cmbUserLoadedHandler, null);
            CreateColumn();
            BindListView();
         
            // 
        }

        void mainWindowViewModel_LoginCommandMethod()
        {
           System.Windows. MessageBox.Show("hello............");
        }
       



        //private void cmbUserLoadedHandler(object obj)
        //{
        //    cmbUser = obj as System.Windows.Controls.ComboBox;
        //    BindComboBox();

        //}

        private void clearListViewItemCommandHandler(object obj)
        {
            string temp = "";
            StringBuilder newFile = new StringBuilder();
            string[] file = File.ReadAllLines(fileName);
            foreach (var line in file)
            {
                //if (!line.Contains(Lblproxy))
                //{

                temp = line;
                newFile.Append(temp + "\r\n");
                continue;
                //}

            }
            File.WriteAllText(fileName, newFile.ToString());
            File.Delete(fileName);
        }

        public ObservableCollection<FacebookUserLoginInfo> DeleteListviewItem
        {
            get { return _deleteListviewItem; }
            set
            {
                _deleteListviewItem = value;
                OnPropertyChanged("DeleteListviewItem");

            }
        }

        public ObservableCollection<FacebookUserLoginInfo> NewUserNameInfoList
        {

            get { return _newUserNameInfoList; }
            set
            {
                _newUserNameInfoList = value;
                OnPropertyChanged("NewUserNameInfoList");

            }
        }


        #endregion
        
        #region Property


        #endregion

        #region Command

        public DelegateCommand NewUserCommand { get; set; }
        public DelegateCommand clearListViewItemCommand { get; set; }
        //public DelegateCommand cmbUserLoaded { get; set; }

        #endregion

        #region Method
        SqLiteHelper sql = new SqLiteHelper();
        private void NewUserCommandHandler(object obj)
        {
            MessageBox.Show("UserId= " + TxtUserId + Environment.NewLine + "Password= " + TxtPassword);
            string Credential = TxtUserId + ":" + TxtPassword;
            SqLiteHelper sql1 = new SqLiteHelper();
            string query = "INSERT INTO TblLogin(FbUserName,FbPassword) values('" + TxtUserId + "','" + TxtPassword + "')";
          
            int yy = sql.ExecuteNonQuery(query);

            BindListView();

        }
        private DataTable dtuserCredential = new DataTable();

        void CreateColumn()
        {
            dtuserCredential.Columns.Add("UserName");
            dtuserCredential.Columns.Add("Password");
        }
    
        public System.Windows.Controls.ComboBox cmbUser
        {
            get;
            set;
        }

        void BindListView()
        {
            try
            {
                var file = File.ReadAllLines(fileName);
                foreach (string item in file)
                {
                    string[] splitedItems = item.Split(':');
                    NewUserNameInfoList.Add(new FacebookUserLoginInfo { LoginUserName = splitedItems[0] });
                }
            }
            catch (Exception)
            {

            }

        }
        #endregion
    }
}
