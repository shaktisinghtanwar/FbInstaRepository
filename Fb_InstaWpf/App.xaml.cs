using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Fb_InstaWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            this.Exit += App_Exit;
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            OnlineFetcher.chromeWebDriver.Quit();
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Some Error has occured. ");
            e.Handled = true;
        }
    }
}
