using System.Windows.Controls;
using TestTaskCoin.MVVM.ViewModels;
using TestTaskCoin.Services;

namespace TestTaskCoin.MVVM.Views
{
    public partial class TopListPage : Page
    {
        public TopListPage()
        {
            InitializeComponent();
            DataContext = new TopListViewModel(new CoinCapService());
        }
    }
}
