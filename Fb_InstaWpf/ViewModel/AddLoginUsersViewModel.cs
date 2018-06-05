using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Text;
using Fb_InstaWpf.Helper;
using Fb_InstaWpf.Model;
using System.IO;

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
            //SMessageBox.Show("UserId= " + TxtUserId + Environment.NewLine + "Password= " + TxtPassword);
            string Credential = TxtUserId + ":" + TxtPassword;
            //SqLiteHelper sql1 = new SqLiteHelper();
            string query = "INSERT INTO TblLogin(FbUserName,FbPassword) values('" + TxtUserId + "','" + TxtPassword + "')";
          
            int yy = sql.ExecuteNonQuery(query);

            if (string.IsNullOrEmpty(Lblproxy))
            {
                //bool CheckID = FileOpration.CheckId(Credential, fileName);
                //if (!CheckID)
                //{
                FileOperation.AddIntoTextFile(fileName, Credential);
            }

            else
            {
                string temp = "";
                StringBuilder newFile = new StringBuilder();
                string[] file = File.ReadAllLines(fileName);
                foreach (var line in file)
                {

                    if (line.Contains(Lblproxy))
                    {
                        temp = Lblproxy.Replace(Lblproxy, Credential);

                        newFile.Append(temp + "\r\n");
                        continue;
                    }
                    newFile.Append(line + "\r\n");
                }
                File.WriteAllText(fileName, newFile.ToString());

            }

            BindListView();

        }
        private DataTable dtuserCredential = new DataTable();

        void CreateColumn()
        {
            dtuserCredential.Columns.Add("UserName");
            dtuserCredential.Columns.Add("Password");
        }
        
        //void BindComboBox()
        //{

        //    if (File.Exists(fileName))
        //    {
        //        // File.Delete(fileName);
        //        var file = File.ReadAllLines(fileName);
        //        dtuserCredential.Clear();
        //        DataRow dr = dtuserCredential.NewRow();
        //        dr["Password"] = 0;
        //        dr["UserName"] = "--Select--";
        //        dtuserCredential.Rows.InsertAt(dr, 0);
        //        //ds.Tables[0].Rows.InsertAt(rowSelect, 0)
        //        foreach (string s in file)
        //        {
        //            string[] splitedItems = s.Split(':');
        //            if (splitedItems.Length > 0)
        //            {
        //                if (splitedItems.Length > 1)
        //                {
        //                    dr = dtuserCredential.NewRow();
        //                    dr["UserName"] = splitedItems[0];
        //                    dr["Password"] = splitedItems[1];
        //                    dtuserCredential.Rows.Add(dr);
        //                }

        //            }

        //        }
        //        cmbUser.ItemsSource = dtuserCredential.DefaultView;
        //        cmbUser.DisplayMemberPath = Convert.ToString(dtuserCredential.Columns["UserName"]);
        //        cmbUser.SelectedValuePath = Convert.ToString(dtuserCredential.Columns["Password"]);
        //    }
        //}
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

                    //var listitem = new ListViewItem(splitedItems[0]);
                    //listitem.SubItems.Add(splitedItems[1]);
                    //listView1.Items.Add(listitem);
                    //NewUserNameInfoList.Add(new FacebookUserLoginInfo { LoginUserName = });
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
