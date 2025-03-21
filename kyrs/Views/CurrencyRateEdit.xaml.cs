using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using kyrs.Models;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace kyrs.Views
{
    public partial class CurrencyRateEdit : Page
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private List<Currency> _currencies;

        public CurrencyRateEdit()
        {
            InitializeComponent();
            LoadCurrencies();
        }

        private async void LoadCurrencies()
        {
            try
            {
                await using var context = new ApplicationContext();
                _currencies = await context.currency.ToListAsync();
                CurrenciesGrid.ItemsSource = _currencies;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при загрузке списка валют");
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await using var context = new ApplicationContext();
                
                foreach (var currency in _currencies)
                {
                    var dbCurrency = await context.currency
                        .FirstOrDefaultAsync(c => c.Id == currency.Id);
                    
                    if (dbCurrency != null)
                    {
                        dbCurrency.Rate = currency.Rate;
                    }
                }
                
                await context.SaveChangesAsync();
                Logger.Info("Курсы валют успешно обновлены");
                MessageBox.Show("Курсы валют успешно обновлены", "Успех", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при сохранении курсов валют");
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var currencyRates = new CurrencyRates();
                bool success = await currencyRates.GetRatesAsync();
                
                if (success)
                {
                    await using var context = new ApplicationContext();
                    var currencies = await context.currency.ToListAsync();
                    
                    foreach (var currency in currencies)
                    {
                        try
                        {
                            // Обновляем курсы из API
                            if (currencyRates.Rates.ContainsKey(currency.Code))
                            {
                                currency.Rate = currencyRates.Rates[currency.Code];
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex, $"Не удалось обновить курс для валюты {currency.Code}");
                        }
                    }
                    
                    await context.SaveChangesAsync();
                    LoadCurrencies();
                    Logger.Info("Курсы валют успешно обновлены из API");
                    MessageBox.Show("Курсы валют успешно обновлены из API", "Успех", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Не удалось получить данные из API", "Ошибка", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при обновлении курсов валют из API");
                MessageBox.Show($"Ошибка при обновлении курсов: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}