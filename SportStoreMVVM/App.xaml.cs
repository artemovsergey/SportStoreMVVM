using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SportStoreMVVM
{
    
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SportStoreMVVM.MainWindow mv = new SportStoreMVVM.MainWindow();

            //MainWindowViewModel vm = new MainWindowViewModel();
            //mv.DataContext = vm;

            mv.Show();
        }

    }
}
