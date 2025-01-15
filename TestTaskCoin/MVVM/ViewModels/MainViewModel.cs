﻿using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TestTaskCoin.Core;
using TestTaskCoin.MVVM.Models;
using TestTaskCoin.MVVM.Views;
using TestTaskCoin.Services;

namespace TestTaskCoin.MVVM.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly ICoinCapService _coinCapService;
        private ObservableCollection<CryptoCurrency> _cryptocurrencies;
        private bool _isBusy;
        public RelayCommand<CryptoCurrency> NavigateToDetailsCommand { get; }
        public RelayCommand<object> RefreshCommand { get; }

        public ObservableCollection<CryptoCurrency> Cryptocurrencies
        {
            get => _cryptocurrencies;
            set => SetProperty(ref _cryptocurrencies, value);
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

        public MainViewModel(ICoinCapService coinCapService, int limitOfCryptocurrencies = 10)
        {
            _coinCapService = coinCapService;
            Cryptocurrencies = new ObservableCollection<CryptoCurrency>();
            NavigateToDetailsCommand = new RelayCommand<CryptoCurrency>(NavigateToDetails);
            RefreshCommand = new RelayCommand<object>(async _ =>
                await RefreshDataAsync(limitOfCryptocurrencies));
            _ = RefreshDataAsync(limitOfCryptocurrencies);
        }

        private void NavigateToDetails(CryptoCurrency crypto)
        {
            var detailsPage = new DetailsPage();
            var detailsViewModel = new DetailsViewModel(_coinCapService, crypto);
            detailsPage.DataContext = detailsViewModel;
            NavigationService.NavigateToPage(detailsPage);
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
    }
}
