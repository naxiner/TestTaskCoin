using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TestTaskCoin.Core;
using TestTaskCoin.Enums;
using TestTaskCoin.MVVM.Models;
using TestTaskCoin.Services;

namespace TestTaskCoin.MVVM.ViewModels
{
    public class ChartViewModel : BaseViewModel
    {
        private readonly ICoinCapService _coinCapService;
        private ObservableCollection<History> _history;
        private bool _isBusy;
        public RelayCommand<object> RefreshDataCommand { get; }
        public string SelectedCryptocurrencyId { get; }
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public ObservableCollection<History> History
        {
            get => _history;
            set => SetProperty(ref _history, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
                RefreshDataCommand.RaiseCanExecuteChanged();
            }
        }

        public ChartViewModel(ICoinCapService coinCapService, string id)
        {
            _coinCapService = coinCapService;
            History = new ObservableCollection<History>();
            SelectedCryptocurrencyId = id;
            SeriesCollection = new SeriesCollection();
            HistoryRequest request = new HistoryRequest()
            {
                Id = SelectedCryptocurrencyId,
                Interval = HistoryInterval.D1
            };
            RefreshDataCommand = new RelayCommand<object>(async _ =>
                await RefreshDataAsync(request));
            _ = RefreshDataAsync(request);
        }

        private async Task RefreshDataAsync(HistoryRequest request)
        {
            IsBusy = true;

            try
            {
                var data = await _coinCapService.GetHistoryByIdAsync(request);
                History.Clear();
                foreach (var history in data)
                {
                    History.Add(history);
                }
                RefreshChart();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void RefreshChart()
        {
            SeriesCollection.Clear();

            SeriesCollection.Add(new LineSeries
            {
                Title = SelectedCryptocurrencyId,
                Values = new ChartValues<decimal>(_history.Select(c => c.PriceUsd))
            });

            YFormatter = value => value.ToString("N2") + "$";
            Labels = _history.Select(c => c.Date.ToString()).ToArray();
        }
    }
}
