using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TestTaskCoin.Core;
using TestTaskCoin.MVVM.Models;
using TestTaskCoin.Services;

namespace TestTaskCoin.MVVM.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        private readonly ICoinCapService _coinCapService;
        private ObservableCollection<CryptoCurrency> _cryptocurrencies;
        private CryptoCurrency _foundCryptoCurrency;
        private string _searchQuery;
        private bool _isBusy;
        public RelayCommand<string> SearchCommand { get; }
        public RelayCommand<object> RefreshCommand { get; }

        public ObservableCollection<CryptoCurrency> Cryptocurrencies
        {
            get => _cryptocurrencies;
            set => SetProperty(ref _cryptocurrencies, value);
        }

        public CryptoCurrency FoundCryptoCurrency
        {
            get => _foundCryptoCurrency;
            set => SetProperty(ref _foundCryptoCurrency, value);
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                if (SetProperty(ref _searchQuery, value))
                {
                    SearchCryptoCurrency(_searchQuery);
                }
            }
        }

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

        public SearchViewModel(ICoinCapService coinCapService, int limitOfCryptocurrencies = 2000)
        {
            _coinCapService = coinCapService;
            Cryptocurrencies = new ObservableCollection<CryptoCurrency>();
            SearchCommand = new RelayCommand<string>(SearchCryptoCurrency);
            RefreshCommand = new RelayCommand<object>(async _ =>
                await RefreshDataAsync(limitOfCryptocurrencies));
            _ = RefreshDataAsync(limitOfCryptocurrencies);
        }

        private async Task RefreshDataAsync(int limit)
        {
            IsBusy = true;
            try
            {
                var data = await _coinCapService.GetCryptoCurrenciesAsync(limit);
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

        private void SearchCryptoCurrency(string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                FoundCryptoCurrency = null;
                return;
            }

            FoundCryptoCurrency = Cryptocurrencies.FirstOrDefault(c =>
                (int.TryParse(searchQuery, out int rank) && c.Rank == rank) ||
                c.Symbol.Equals(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                c.Name.Equals(searchQuery, StringComparison.OrdinalIgnoreCase));
        }
    }
}
