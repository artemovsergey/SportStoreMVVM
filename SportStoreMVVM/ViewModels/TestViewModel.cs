using SportStoreMVVM.Commands;
using SportStoreMVVM.Models;
using SportStoreMVVM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SportStoreMVVM.ViewModels
{
    public class TestViewModel : ViewModel
    {
        public bool canUpdate;

        #region Команда AddUserCommand
        public ICommand AddUserCommand { get; set; }

        private bool CanAddUserCommandExecute(object p)
        {
            return true;
        }

        private void OnAddUserCommandExecuted(object p)
        {
            canUpdate = false;
            CurrentUser = new User();

            AddUserWindow addUserWindow = new AddUserWindow();
            addUserWindow.DataContext = this;
            addUserWindow.ShowDialog();


        }
        #endregion

        #region Команда SaveUserCommand
        public ICommand SaveUserCommand { get; set; }
        private bool CanSaveUserCommandExecute(object p)
        {
            return true;
        }

        private void OnSaveUserCommandExecuted(object p)
        {
            
            if (canUpdate == false)
            {
                User user = new User();
                user.Name = CurrentUser.Name;
                user.Age = CurrentUser.Age;
                Users.Add(user);
                CountUser = Users.Count();

                MessageBox.Show("Пользователь сохранен!");
            }
            else
            {
                CurrentUser.Name = ((User)p).Name;
                CurrentUser.Age = ((User)p).Age;
                MessageBox.Show("Пользователь обновлен!");
            }

        }
        #endregion

        #region Команда DeleteUserCommand
        public ICommand DeleteUserCommand { get; set; }

        private bool CanDeleteUserCommandExecute(object p)
        {
            return true;
        }

        private void OnDeleteUserCommandExecuted(object p)
        {
            if (p != null)
            {
                Users.Remove((User)p);
                CountUser = Users.Count();
                MessageBox.Show($"Пользователь {((User)p).Name} удален!");
            }
            else
            {
                MessageBox.Show("Выберите пользователя!");
            }

        }
        #endregion

        #region Команда EditUserCommand
        public ICommand EditUserCommand { get; set; }

        private bool CanEditUserCommandExecute(object p)
        {
            return true;
        }

        private void OnEditUserCommandExecuted(object p)
        {
            canUpdate = true;

            if (p != null)
            {
                AddUserWindow addUserWindow = new AddUserWindow();
                CurrentUser = (User)p;
                addUserWindow.DataContext = this;
                addUserWindow.ShowDialog();
                canUpdate = true;
            }
            else
            {
                MessageBox.Show("Выберите пользователя!");
            }

        }
        #endregion

        #region Свойство Users
        public ObservableCollection<User> Users { get; set; }
        #endregion

        #region Свойство CurrentUser
        public User CurrentUser { get; set; }
        #endregion

        #region Свойство CountUser
        private int countUser;
        public int CountUser
        {
            get { return countUser; }

            set
            {
                //countUser = value;
                //OnPropertyChanged("CountUser");

                Set(ref countUser, value);
            }

        }
        #endregion


        public TestViewModel()
        {

            Users = new ObservableCollection<User>();

            Users.Add(new User() { Name = "user1", Age = 1 });
            Users.Add(new User() { Name = "user2", Age = 2 });
            Users.Add(new User() { Name = "user3", Age = 3 });
            Users.Add(new User() { Name = "user4", Age = 4 });

            AddUserCommand = new LambdaCommand(OnAddUserCommandExecuted, CanAddUserCommandExecute);

            SaveUserCommand = new LambdaCommand(OnSaveUserCommandExecuted, CanSaveUserCommandExecute);

            DeleteUserCommand = new LambdaCommand(OnDeleteUserCommandExecuted, CanDeleteUserCommandExecute);

            EditUserCommand = new LambdaCommand(OnEditUserCommandExecuted, CanEditUserCommandExecute);

            CurrentUser = new User() { };

            CountUser = Users.Count();

        }

    }
}
