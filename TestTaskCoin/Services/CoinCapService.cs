using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TestTaskCoin.Constants;
using TestTaskCoin.MVVM.Models;
using Newtonsoft.Json;

namespace TestTaskCoin.Services
{
    public class CoinCapService : ICoinCapService
    {
        private readonly HttpClient _httpClient;

        public CoinCapService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<CryptoCurrency>> GetCryptoCurrenciesAsync(int limit)
        {
            if (limit < 0 || limit > 2000)
            {
                throw new Exception("Limit for receiving cryptocurrencies " +
                    "should be in range of 0-2000.");
            }

            var url = $"{ApiConstants.BaseUrl}assets?limit={limit}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to fetch data from API.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TopCryptoCurrenciesResponse>(json);

            return result?.Data ?? new List<CryptoCurrency>();
        }

        public async Task<List<Market>> GetMarketsByIdAsync(string baseId)
        {
            var url = $"{ApiConstants.BaseUrl}assets/{baseId.ToLower()}/markets";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to fetch data from API.");

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<MarketsResponse>(json);

            return result?.Data ?? new List<Market>();
        }
    }
}
