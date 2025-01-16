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
        public RelayCommand<object> DayCommand { get; }
        public RelayCommand<object> WeekCommand { get; }
        public RelayCommand<object> MonthCommand { get; }
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

            RefreshDataCommand = new RelayCommand<object>(async _ =>await LoadPeriodData(HistoryInterval.H1));
            DayCommand = new RelayCommand<object>(async _ => await LoadPeriodData(HistoryInterval.H1));
            WeekCommand = new RelayCommand<object>(async _ => await LoadPeriodData(HistoryInterval.H6));
            MonthCommand = new RelayCommand<object>(async _ => await LoadPeriodData(HistoryInterval.D1));

            _ = LoadPeriodData(HistoryInterval.M30);
        }

        private async Task LoadPeriodData(HistoryInterval interval)
        {
            var (start, end) = GetStartAndEndTimestamps(interval);
            var request = new HistoryRequest
            {
                Id = SelectedCryptocurrencyId,
                Interval = interval,
                Start = start,
                End = end
            };
            await RefreshDataAsync(request);
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

            OnPropertyChanged(nameof(SeriesCollection));
            OnPropertyChanged(nameof(Labels));
        }

        private (long start, long end) GetStartAndEndTimestamps(HistoryInterval interval)
        {
            var now = DateTime.UtcNow;
            DateTime start;

            switch (interval)
            {
                case HistoryInterval.H1:
                    start = now.AddDays(-1);
                    break;
                case HistoryInterval.H6:
                    start = now.AddDays(-7);
                    break;
                case HistoryInterval.D1:
                    start = now.AddDays(-30);
                    break;
                default:
                    start = now.AddDays(-1);
                    break;
            }

            long startTimestamp = new DateTimeOffset(start).ToUnixTimeMilliseconds();
            long endTimestamp = new DateTimeOffset(now).ToUnixTimeMilliseconds();
            return (startTimestamp, endTimestamp);
        }
    }
}
