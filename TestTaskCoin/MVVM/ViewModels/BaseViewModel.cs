using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TestTaskCoin.Core;
using TestTaskCoin.Services;

namespace TestTaskCoin.MVVM.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand<object> GoBackCommand { get; }

        public BaseViewModel()
        {
            GoBackCommand = new RelayCommand<object>(ExecuteGoBack, CanExecuteGoBack);
        }

        private void ExecuteGoBack(object param)
        {
            NavigationService.GoBack();
        }

        private bool CanExecuteGoBack(object param)
        {
            return NavigationService.CanGoBack();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return false;
            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }
    }
}
