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

        #region Команда UpdateProductCommand
        public ICommand UpdateProductCommand { get; set; }

        private bool CanUpdateProductCommandExecute(object p)
        {
            return true;
        }

        private void OnUpdateProductCommandExecuted(object p)
        {

            using (SportStoreContext db = new SportStoreContext())
            {

                 CurrentProducts = db.Products.ToList();
               
                //Сортировка
                if (SortSelectedIndex != -1)
                {
                    if (SortSelectedValue.Contains("По убыванию цены"))
                    {
                        CurrentProducts = CurrentProducts.OrderByDescending(u => u.Cost);
                        MessageBox.Show("Работает!");

                    }

                    if (SortSelectedValue.Contains("По возрастанию цены"))
                    {
                        CurrentProducts = CurrentProducts.OrderBy(u => u.Cost);

                    }
                }


                // Фильтрация
                //if (filterUserComboBox.SelectedIndex != -1)
                //{
                //    if (db.Products.Select(u => u.Manufacturer).Distinct().ToList().Contains(filterUserComboBox.SelectedValue))
                //    {
                //        currentProducts = currentProducts.Where(u => u.Manufacturer == filterUserComboBox.SelectedValue.ToString()).ToList();
                //    }
                //    else
                //    {
                //        currentProducts = currentProducts.ToList();
                //    }
                //}

                // Поиск

                //if (searchBox.Text.Length > 0)
                //{

                //    currentProducts = currentProducts.Where(u => u.Name.Contains(searchBox.Text) || u.Description.Contains(searchBox.Text)).ToList();

                //}

                //productlistView.ItemsSource = currentProducts;

                //countProducts.Text = $"Количество: {currentProducts.Count} из {db.Products.ToList().Count}";
                }
            
            }
        #endregion

        #region Свойство Products

        public ObservableCollection<Product> Products { get; set; }
        #endregion

        #region Свойство CurrentProducts

        private IEnumerable<Product> _currentProducts;
        public IEnumerable<Product> CurrentProducts
        {
            get { return _currentProducts; }

            set
            {
                Set(ref _currentProducts, value);
            }

        }

        #endregion

        #region Свойство SortSelectedIndex

        public int SortSelectedIndex { get; set; }
        #endregion

        #region Свойство SortSelectedValue

        public string SortSelectedValue { get; set; }
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
            }
            else
            {
                CurrentRole = "Здравствуйте,гость";
            }

            Products = new ObservableCollection<Product>();
            using (SportStoreContext db = new SportStoreContext())
            {
                List<Product> temp = db.Products.ToList();
                foreach (var item in temp)
                {
                    Products.Add(item);
                }

                CurrentProducts = db.Products.ToList();
            }

            ExitCommand = new LambdaCommand(OnExitCommandExecuted,CanExitCommandExecute);

            UpdateProductCommand = new LambdaCommand(OnUpdateProductCommandExecuted, CanUpdateProductCommandExecute);
        }
    }
}
