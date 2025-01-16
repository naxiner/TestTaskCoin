using System.Windows.Controls;
using TestTaskCoin.MVVM.ViewModels;
using TestTaskCoin.Services;

namespace TestTaskCoin.MVVM.Views
{
    public partial class SearchPage : Page
    {
        public SearchPage()
        {
            InitializeComponent();
            DataContext = new SearchViewModel(new CoinCapService());
        }
    }
}
