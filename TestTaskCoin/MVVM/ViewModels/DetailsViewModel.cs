using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TestTaskCoin.Core;
using TestTaskCoin.MVVM.Models;
using TestTaskCoin.Services;

namespace TestTaskCoin.MVVM.ViewModels
{
    public class DetailsViewModel : BaseViewModel
    {
        private readonly ICoinCapService _coinCapService;
        private ObservableCollection<Market> _markets;
        public RelayCommand<object> LoadMarketsCommand { get; }
        public CryptoCurrency SelectedCryptocurrency { get; }

        public ObservableCollection<Market> Markets
        {
            get => _markets;
            set => SetProperty(ref _markets, value);
        }

        public DetailsViewModel(ICoinCapService coinCapService, CryptoCurrency cryptocurrency)
        {
            _coinCapService = coinCapService;
            Markets = new ObservableCollection<Market>();
            SelectedCryptocurrency = cryptocurrency;
            LoadMarketsCommand = new RelayCommand<object>(async _ => await LoadMarketsAsync(SelectedCryptocurrency.Name));
            _ = LoadMarketsAsync(SelectedCryptocurrency.Name);
        }

        private async Task LoadMarketsAsync(string baseId)
        {
            IsBusy = true;

            try
            {
                var data = await _coinCapService.GetMarketsByIdAsync(baseId);
                Markets.Clear();
                foreach (var market in data)
                {
                    Markets.Add(market);
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
                LoadMarketsCommand.RaiseCanExecuteChanged();
            }
        }
    }
}
