using System.Collections.Generic;
using System.Threading.Tasks;
using TestTaskCoin.MVVM.Models;

namespace TestTaskCoin.Services
{
    public interface ICoinCapService
    {
        Task<List<Market>> GetMarketsByIdAsync(string baseId);
        Task<List<CryptoCurrency>> GetCryptoCurrenciesAsync(int limit);
        Task<List<History>> GetHistoryByIdAsync(HistoryRequest request);
    }
}