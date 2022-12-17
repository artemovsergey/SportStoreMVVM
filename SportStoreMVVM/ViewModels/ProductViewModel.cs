using SportStoreMVVM.Commands;
using SportStoreMVVM.Models;
using SportStoreMVVM.Services;
using SportStoreMVVM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace SportStoreMVVM.ViewModels
{
    internal class ProductViewModel : ViewModel
    {

        Product? currentProduct;

        string? oldImage;
        string? newImage;
        string? newImagePath;


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

        #region Команда AddImageCommand
        public ICommand AddImageCommand { get; set; }

        private bool CanAddImageCommandExecute(object p)
        {
            return true;
        }

        private void OnAddImageCommandExecuted(object p)
        {
            Stream myStream;

            if (currentProduct != null)
            {
                oldImage = System.IO.Path.Combine(Environment.CurrentDirectory, $"images/{currentProduct.Photo}");
            }
            else
            {
                oldImage = null;
            }

            // проверяем, есть ли изображение у товара и запоминаем путь к изображению сейчас

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            if (dlg.ShowDialog() == true)
            {
                if ((myStream = dlg.OpenFile()) != null)
                {
                    dlg.DefaultExt = ".png";
                    dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
                    dlg.Title = "Open Image";
                    dlg.InitialDirectory = "./";

                    // Предпросмотр изображения
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    image.UriSource = new Uri(dlg.FileName);

                    //MessageBox.Show($"Изображения: {image.Width} на {image.Height} пикселей. Размер будет приведен к 200 на 300 пикселей! ");
                    //image.DecodePixelWidth = 300;
                    //image.DecodePixelHeight = 200;
                    //imageBoxPath.Source = image;

                    CurrentProduct.Photo = dlg.SafeFileName;
                    //MessageBox.Show(CurrentProduct.Photo);
                    CurrentProduct.ImagePath = image.UriSource.ToString();
                    //MessageBox.Show(CurrentProduct.ImagePath);


                    image.EndInit();

                    try
                    {

                        newImage = dlg.SafeFileName;
                        //MessageBox.Show($"newImage: {newImage}");
                        newImagePath = dlg.FileName;
                        //MessageBox.Show($"newImagePath: {newImagePath}");
                        //MessageBox.Show(File.Exists(newImagePath).ToString());

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                myStream.Dispose();
            }
        }
        #endregion

        #region Команда AddProductCommand
        public ICommand AddProductCommand { get; set; }

        private bool CanAddProductCommandExecute(object p)
        {
            return true;
        }

        private void OnAddProductCommandExecuted(object p)
        {
            //MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            AddProductWindow addProductWindow = new AddProductWindow();
            addProductWindow.DataContext = this;
            addProductWindow.ShowDialog();
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

        #region Свойство CurrentProduct

        private Product _currentProduct;
        public Product CurrentProduct
        {
            get => _currentProduct;
            set => Set(ref _currentProduct, value);
        }
        #endregion


        public ProductViewModel(User user)
        {
            CurrentProduct = new Product(); //{ Name = "product!"};

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
            AddProductCommand = new LambdaCommand(OnAddProductCommandExecuted, CanAddProductCommandExecute);
            AddImageCommand = new LambdaCommand(OnAddImageCommandExecuted, CanAddImageCommandExecute);

        }
    }
}
