using RestSharp;
using System.Linq;
using kyrs.Models;
using Microsoft.EntityFrameworkCore;

public class CurrencyRates
{

    
    private const string ApiKey = "7ef39e9324a52f7e14147605a0348852"; // Замените на ваш API ключ
    private const string BaseUrl = "http://data.fixer.io/api/latest";
    private Dictionary<string, decimal> rates;

    public async Task<bool> GetRatesAsync()
    {
        var client = new RestClient();
        var request = new RestRequest(BaseUrl, Method.Get);
        request.AddParameter("access_key", ApiKey);
        request.AddParameter("symbols", "USD,EUR,RUB");

        var response = await client.ExecuteAsync<FixerResponse>(request);

        if (response.IsSuccessful)
        {
            rates = response.Data.Rates;
            Console.WriteLine($"USD: {rates["USD"]}");
            Console.WriteLine($"EUR: {rates["EUR"]}");
            Console.WriteLine($"RUB: {rates["RUB"]}");
            return true;
        }
        else
        {
            Console.WriteLine("Ошибка при по��учении данных: " + response.ErrorMessage);
            return false;
        }
    }

    public decimal GetLocalRate(string currencyCode)
    {
        using (var context = new ApplicationContext())
        {
            var currency = context.currency.SingleOrDefault(c => c.Code == currencyCode);            if (currency == null)
            {
                throw new InvalidOperationException($"Валюта с кодом {currencyCode} не найдена.");
            }
            return currency.Rate;
        }
    }
    public decimal ConvertCurrency(decimal amount, string fromCurrency, string toCurrency)
    {
        if (rates == null || !rates.ContainsKey(fromCurrency) || !rates.ContainsKey(toCurrency))
        {
            throw new InvalidOperationException("Курсы валют не загружены или указаны неверные валюты.");
        }

        decimal fromRate = rates[fromCurrency];
        decimal toRate = rates[toCurrency];
        return amount * (toRate / fromRate);
    }
}

public class FixerResponse
{
    public bool Success { get; set; }
    public Dictionary<string, decimal> Rates { get; set; }
}