﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp1.Models;
using WpfApp1.Repositories;

namespace WpfApp1.ViewModels
{
    public class MainViewModelWorker : ViewModelBase
    {
        private UserAccountModel _currentUserAccount;
        private string _errorMessage;
        private string _currentPassword;
        private WorkerModel _workerModule;
        private WorkerRepository _workerRepository;

        public WorkerModel WorkerModule
        {
            get
            {
                return _workerModule;
            }
            set
            {
                _workerModule = value;
                OnPropertyChanged(nameof(WorkerModule));
            }
        }
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }

            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        public string CurrentPassword
        {
            get
            {
                return _currentPassword;
            }

            set
            {
                _currentPassword = value;
                OnPropertyChanged(nameof(CurrentPassword));
            }
        }
        public UserAccountModel CurrentUserAccount
        {
            get
            {
                return _currentUserAccount;
            }

            set
            {
                _currentUserAccount = value;
                OnPropertyChanged(nameof(CurrentUserAccount));
            }
        }
        public ICommand SaveCommand { get; }
        public ICommand SaveCommand3 { get; }
        public MainViewModelWorker()
        {
            SaveCommand = new ViewModelCommand(ExecuteSave, canExecuteSave);
            SaveCommand3 = new ViewModelCommand(ExecuteSave3, canExecuteSave3);
            _workerRepository = new WorkerRepository();
            _workerModule = _workerRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            CurrentUserAccount = new UserAccountModel();
            LoadCurrentUserData();
        }
        private bool canExecuteSave3(object obj)
        {
            bool ValiData = true;
            if (string.IsNullOrWhiteSpace(WorkerModule.Username))
            {
                ErrorMessage = "* Invalid Username";
                ValiData = false;
            }
            else if (string.IsNullOrWhiteSpace(CurrentPassword))
            {
                ErrorMessage = "* Invalid Current Password";
                ValiData = false;
            }
            else if (string.IsNullOrWhiteSpace(WorkerModule.Password))
            {
                ErrorMessage = "* Invalid New Password";
                ValiData = false;
            }
            else
            {
                ErrorMessage = string.Empty;
            }
            return ValiData;
        }

        private void ExecuteSave3(object obj)
        {
            if (CurrentPassword == _workerRepository.GetPassword(Thread.CurrentPrincipal.Identity.Name))
            {
                _workerRepository.UpdateOnlyPasswordUsername(WorkerModule);
                System.Windows.MessageBox.Show("Username and password has been updated succesfully !!");
            }
            else
                System.Windows.MessageBox.Show("Invalid Current Password ??");
        }

        private bool canExecuteSave(object obj)
        {
            bool ValiData = true;
            if (string.IsNullOrWhiteSpace(WorkerModule.Name))
            {
                ErrorMessage = "* Invalid Name";
                ValiData = false;
            }
            else if (string.IsNullOrWhiteSpace(WorkerModule.LastName))
            {
                ErrorMessage = "* Invalid LastName";
                ValiData = false;
            }
            else if (string.IsNullOrWhiteSpace(WorkerModule.NumTel))
            {
                ErrorMessage = "* Invalid NumTel";
                ValiData = false;
            }
            else if (string.IsNullOrWhiteSpace(WorkerModule.Cin))
            {
                ErrorMessage = "* Invalid Cin";
                ValiData = false;
            }
            else if (string.IsNullOrEmpty(WorkerModule.Date.ToString()))
            {
                ErrorMessage = "* Invalid Date";
                ValiData = false;
            }
            else
            {
                ErrorMessage = string.Empty;
            }

            return ValiData;
        }

        private void ExecuteSave(object obj)
        {
            if (canExecuteSave(obj))
            {
                try
                {
                    _workerRepository.UpdateWithoutPasswordUsername(WorkerModule);

                    CurrentUserAccount.DisplayName = $"{WorkerModule.Name} {WorkerModule.LastName}";
                    OnPropertyChanged(nameof(CurrentUserAccount));
                    System.Windows.MessageBox.Show("User Profile has been updated succesfully !!");
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Error saving admin details: {ex.Message}";
                }
            }
        }
        private void LoadCurrentUserData()
        {
            WorkerModel worker = _workerRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if (worker != null)
            {
                CurrentUserAccount.Username = worker.Username;
                CurrentUserAccount.DisplayName = $" {worker.Name} {worker.LastName}";
                CurrentUserAccount.ProfilePicture = null;
            }
            else
            {
                CurrentUserAccount.DisplayName = "Invalid user, not logged in";
                //Hide child views.
            }
        }
    }
}
