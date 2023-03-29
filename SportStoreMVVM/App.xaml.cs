using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using SportStoreMVVM.ViewModels;
using SportStoreMVVM.Views;

namespace SportStoreMVVM
{
    
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            LoginWindow lw = new LoginWindow();

            //MainWindow mv = new MainWindow();

            LoginViewModel vm = new LoginViewModel();
            lw.DataContext = vm;

            lw.Show();
        }

    }
}
