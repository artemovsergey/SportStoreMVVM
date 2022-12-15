using Microsoft.EntityFrameworkCore;
using SportStoreMVVM.Commands;
using SportStoreMVVM.Models;
using SportStoreMVVM.Services;
using SportStoreMVVM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static System.Reflection.Metadata.BlobBuilder;

namespace SportStoreMVVM.ViewModels
{
    internal class LoginViewModel : ViewModel
    {

        bool verify = true;
        int verifyCheck = 0;


        #region Команда LoginCommand
        public ICommand LoginCommand { get; set; }

        private bool CanLoginCommandExecute(object p)
        {
            return true;
        }

        private void OnLoginCommandExecuted(object p)
        {

            // проверка, если есть каптча
            if (CaptchaPanelVisibility == Visibility.Visible)
            {
                if (CaptchaText == CaptchaTextUser)
                {
                    verify = true;
                }
            }

            using(SportStoreContext db = new SportStoreContext())
            {
                 User user = db.Users.Where(u => u.Login == CurrentUser.Login && u.Password == ((PasswordBox)p).Password).Include(u => u.RoleNavigation).FirstOrDefault() as User;
           
                //if (CurrentUser.Login == "admin" && ((PasswordBox)p).Password == "admin" && verify)
                if(user != null)
                {
                    CurrentUser = user;
                 
                    
                    ProductsWindow main = new ProductsWindow();
                    ProductViewModel productViewModel = new ProductViewModel(user);
                    main.DataContext = productViewModel;

                    


                    App.Current.MainWindow.Close();
                    main.Show();
                }
                else
                {
                    MessageBox.Show("Неправильный логин или пароль");
                    verifyCheck += 1;
                    CaptchaPanelVisibility = Visibility.Visible;
                    CaptchaText = CaptchaBuilder.Refresh();
                    verify = false;

                    if (verifyCheck > 1)
                    {
                        disableButton();
                        CaptchaText = CaptchaBuilder.Refresh();
                    }
                }

            }


        }
        #endregion

        #region Команда GuestCommand
        public ICommand GuestCommand { get; set; }

        private bool CanGuestCommandExecute(object p)
        {
            return true;
        }

        private void OnGuestCommandExecuted(object p)
        {

            ProductsWindow main = new ProductsWindow();
            ProductViewModel productViewModel = new ProductViewModel(null);
            main.DataContext = productViewModel;

           // закрытие основного окна программы (первого)
           App.Current.MainWindow.Close();
         
           main.Show();
        
        }
        #endregion


        /// <summary>
        /// Асинхронное выключение кнопки на 10 сек.
        /// </summary>
        async void disableButton()
        {
            LoginEnableButton = false;
            await Task.Delay(TimeSpan.FromSeconds(10));
            LoginEnableButton = true;
        }


        #region Свойство CurrentUser
        private User _currentUser;
        public User? CurrentUser
        {
            get { return _currentUser; }

            set
            {
                //countUser = value;
                //OnPropertyChanged("CountUser");

                Set(ref _currentUser, value);
            }

        }
        #endregion

        #region Свойство CaptchaPanelVisibility
        private Visibility _captchaPanelVisibility;
        public Visibility CaptchaPanelVisibility
        {
            get { return _captchaPanelVisibility; }

            set
            {
                //countUser = value;
                //OnPropertyChanged("CountUser");

                Set(ref _captchaPanelVisibility, value);
            }

        }
        #endregion

        #region Свойство CaptchaText
        private string _captchaText;
        public string CaptchaText
        {
            get { return _captchaText; }

            set
            {
                //countUser = value;
                //OnPropertyChanged("CountUser");

                Set(ref _captchaText, value);
            }

        }
        #endregion

        #region Свойство CaptchaTextUser
        private string _captchaTextUser;
        public string CaptchaTextUser
        {
            get { return _captchaTextUser; }

            set
            {
                //countUser = value;
                //OnPropertyChanged("CountUser");

                Set(ref _captchaTextUser, value);
            }

        }
        #endregion

        #region Свойство LoginEnableButton
        private bool _loginEnableButton;
        public bool LoginEnableButton
        {
            get { return _loginEnableButton; }

            set
            {
                //countUser = value;
                //OnPropertyChanged("CountUser");

                Set(ref _loginEnableButton, value);
            }

        }
        #endregion




       






        public LoginViewModel()
        {
            LoginCommand = new LambdaCommand(OnLoginCommandExecuted, CanLoginCommandExecute);

            GuestCommand = new LambdaCommand(OnGuestCommandExecuted, CanGuestCommandExecute);

            CurrentUser = new User();

            CaptchaPanelVisibility= Visibility.Collapsed;

            LoginEnableButton = true;
       
        }
    }
}
