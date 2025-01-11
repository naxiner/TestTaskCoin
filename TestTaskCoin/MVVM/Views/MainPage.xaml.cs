using System.Windows.Controls;
using TestTaskCoin.MVVM.ViewModels;
using TestTaskCoin.Services;

namespace TestTaskCoin.MVVM.Views
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            DataContext = new MainViewModel(new CoinCapService());
        }
    }
}
