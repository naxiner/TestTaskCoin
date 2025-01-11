using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TestTaskCoin.Core;
using TestTaskCoin.MVVM.Models;
using TestTaskCoin.Services;

namespace TestTaskCoin.MVVM.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly ICoinCapService _coinCapService;
        private ObservableCollection<CryptoCurrency> _cryptocurrencies;
        public RelayCommand RefreshCommand { get; }

        public ObservableCollection<CryptoCurrency> Cryptocurrencies
        {
            get => _cryptocurrencies;
            set => SetProperty(ref _cryptocurrencies, value);
        }

        public MainViewModel(ICoinCapService coinCapService)
        {
            _coinCapService = coinCapService;
            Cryptocurrencies = new ObservableCollection<CryptoCurrency>();
            RefreshCommand = new RelayCommand(async () => await RefreshDataAsync());
            _ = RefreshDataAsync();
        }

        private async Task RefreshDataAsync()
        {
            IsBusy = true;

            try
            {
                var data = await _coinCapService.GetTopCryptoCurrenciesAsync();
                Cryptocurrencies.Clear();
                foreach (var crypto in data)
                {
                    Cryptocurrencies.Add(crypto);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
                RefreshCommand.RaiseCanExecuteChanged();
            }
        }
    }
}
