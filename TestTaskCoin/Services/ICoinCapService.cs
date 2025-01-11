using System.Collections.Generic;
using System.Threading.Tasks;
using TestTaskCoin.MVVM.Models;

namespace TestTaskCoin.Services
{
    public interface ICoinCapService
    {
        Task<List<CryptoCurrency>> GetTopCryptoCurrenciesAsync();
    }
}