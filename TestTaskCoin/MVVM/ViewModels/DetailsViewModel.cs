using TestTaskCoin.MVVM.Models;

namespace TestTaskCoin.MVVM.ViewModels
{
    public class DetailsViewModel : BaseViewModel
    {
        public CryptoCurrency SelectedCryptocurrency { get; }

        public DetailsViewModel(CryptoCurrency cryptocurrency)
        {
            SelectedCryptocurrency = cryptocurrency;
        }
    }
}
