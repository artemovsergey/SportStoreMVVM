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

        #region Команда ClearCommand
        public ICommand ClearCommand { get; set; }

        private bool CanClearCommandExecute(object p)
        {
            return true;
        }

        private void OnClearCommandExecuted(object p)
        {
            FilterSelectedIndex = -1;
            SortSelectedIndex= -1;
            SearchText = "";
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

                    }

                    if (SortSelectedValue.Contains("По возрастанию цены"))
                    {
                        CurrentProducts = CurrentProducts.OrderBy(u => u.Cost);

                    }
                }


                // Фильтрация
                if (FilterSelectedIndex != -1)
                {

                    if (db.Products.Select(u => u.Manufacturer).Distinct().ToList().Contains(FilterSelectedValue))
                    {
                        CurrentProducts = CurrentProducts.Where(u => u.Manufacturer == FilterSelectedValue.ToString()).ToList();
                    }
                    else
                    {
                        CurrentProducts = CurrentProducts.ToList();
                    }
                }

                //Поиск

                if (SearchText != null)
                {
                    CurrentProducts = CurrentProducts.Where(u => u.Name.Contains(SearchText) || u.Description.Contains(SearchText)).ToList();

                }

                CountProducts = $"Количество: {CurrentProducts.Count()} из {db.Products.ToList().Count}";
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
        private int _sortSelectedIndex;
        public int SortSelectedIndex
        {
            get => _sortSelectedIndex;
            set => Set(ref _sortSelectedIndex, value);
        }
        #endregion

        #region Свойство SortSelectedValue

        public string SortSelectedValue { get; set; }
        #endregion

        #region Свойство SortSource

        public List<string> SortSource { get; set; }
        #endregion

        #region Свойство FilterSelectedIndex
   
        private int _filterSelectedIndex;
        public int FilterSelectedIndex
        {
            get => _filterSelectedIndex;
            set => Set(ref _filterSelectedIndex, value);
        }
        #endregion

        #region Свойство FilterSelectedValue

        public string FilterSelectedValue { get; set; }
        #endregion

        #region Свойство FilterSource

        public List<string> FilterSource { get; set; }
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

        #region Свойство SearchText

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => Set(ref _searchText, value);
        }
        #endregion

        #region Свойство CountProducts

        private string _countProducts;
        public string CountProducts
        {
            get => _countProducts;
            set => Set(ref _countProducts, value);
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

                List<string> filterList = db.Products.Select(p => p.Manufacturer).Distinct().ToList();
                filterList.Insert(0, "Все производители");
                FilterSource = filterList;

                SortSource = new List<string>() { "По возрастанию цены", "По убыванию цены" };


                CountProducts = $"Количество: {CurrentProducts.Count()} из {db.Products.ToList().Count}";
            }

            ExitCommand = new LambdaCommand(OnExitCommandExecuted,CanExitCommandExecute);
            UpdateProductCommand = new LambdaCommand(OnUpdateProductCommandExecuted, CanUpdateProductCommandExecute);
            ClearCommand = new LambdaCommand(OnClearCommandExecuted, CanClearCommandExecute);

        }
    }
}
