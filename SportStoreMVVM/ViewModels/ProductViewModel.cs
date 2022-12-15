using SportStoreMVVM.Commands;
using SportStoreMVVM.Models;
using SportStoreMVVM.Services;
using SportStoreMVVM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SportStoreMVVM.ViewModels
{
    internal class ProductViewModel : ViewModel
    {


        #region Команда ExitCommand
        public ICommand ExitCommand { get; set; }

        private bool CanExitCommandExecute(object p)
        {
            return true;
        }

        private void OnExitCommandExecuted(object p)
        {
            //MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            Window window = Application.Current.Windows[0];

           
            //MessageBox.Show(window.Name.ToString());

            LoginWindow loginWindow = new LoginWindow();
            LoginViewModel loginViewModel = new LoginViewModel();
            loginWindow.DataContext = loginViewModel;
            
            
            window.Close();
            loginWindow.Show();
            

        }
        #endregion

        #region Свойство Products

        public ObservableCollection<Product> Products { get; set; }
        #endregion

        #region Свойство CurrentRole
        private string _currentRole;
        public string CurrentRole
        {
            get { return _currentRole; }

            set
            {
                //countUser = value;
                //OnPropertyChanged("CountUser");

                Set(ref _currentRole, value);
            }

        }
        #endregion


        public ProductViewModel(User user)
        {

            if (user != null)
            {
                CurrentRole = $"Здравствуйте,{user.Surname} {user.Name} {user.Patronymic}.\n Ваша роль: {user.RoleNavigation.Name}";
               
                MessageBox.Show($"Здравствуйте,{user.Surname} {user.Name} {user.Patronymic}.\n Ваша роль: {user.RoleNavigation.Name} ");
            }
            else
            {
                CurrentRole = "Здравствуйте,гость";

                MessageBox.Show($"Здравствуйте,гость");
            }


            Products = new ObservableCollection<Product>();
            using (SportStoreContext db = new SportStoreContext())
            {
                List<Product> temp = db.Products.ToList();
                foreach (var item in temp)
                {
                    Products.Add(item);
                }
            }


            ExitCommand = new LambdaCommand(OnExitCommandExecuted,CanExitCommandExecute);

        }
    }
}
