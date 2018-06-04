using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Fb_InstaWpf.ViewModel;

namespace Fb_InstaWpf
{
    /// <summary>
    /// Interaction logic for AddLoginUsers.xaml
    /// </summary>
    public partial class AddLoginUsers : Window
    {
        public AddLoginUsers()
        {
            InitializeComponent();
            this.DataContext = new AddLoginUsersViewModel();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            TxtUsername.Text = "";
            TxtPassword.Text = "";
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click_1(object sender, RoutedEventArgs e)
        {

        }


    }
}
