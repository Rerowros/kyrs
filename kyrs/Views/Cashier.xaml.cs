using System.Windows;
using System.Windows.Controls;
using kyrs.Models;
using kyrs.Views;

namespace kyrs
{
    public partial class Cashier : Page
    {
        
        public Cashier()
        {
            InitializeComponent();
        }

        private async Task UpdateCurrencyRates()
        {
            var currencyRates = new CurrencyRates();
            await currencyRates.GetRatesAsync();

            Usd.Content = $"USD: {currencyRates.ConvertCurrency(1, "USD", "USD")}";
            Eur.Content = $"EUR: {currencyRates.ConvertCurrency(1, "USD", "EUR")}";
            Rub.Content = $"RUB: {currencyRates.ConvertCurrency(1, "USD", "RUB")}";
        }

        private void GoToPage1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Authorization());
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Введите сумму")
            {
                textBox.Text = "";
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Введите сумму";
            }
        }

        // kyrs/Views/Cashier.xaml.cs - обновить существующий метод
    private async void ExchangeButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(ExchangeAmount.Text) || !decimal.TryParse(ExchangeAmount.Text, out decimal amount))
            {
                MessageBox.Show("Пожалуйста, введите корректную сумму.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (ExchangeCurrencyFrom.SelectedItem == null || ExchangeCurrencyTo.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите валюты для обмена.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string fromCurrency = ((ComboBoxItem)ExchangeCurrencyFrom.SelectedItem).Content.ToString();
            string toCurrency = ((ComboBoxItem)ExchangeCurrencyTo.SelectedItem).Content.ToString();

            var currencyRates = new CurrencyRates();

            MessageBoxResult result = MessageBox.Show("Использовать локальный курс?", "Выбор курса", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                await currencyRates.GetRatesAsync();
            }

            decimal fromRate = result == MessageBoxResult.Yes ? currencyRates.GetLocalRate(fromCurrency) : currencyRates.ConvertCurrency(1, "USD", fromCurrency);
            decimal toRate = result == MessageBoxResult.Yes ? currencyRates.GetLocalRate(toCurrency) : currencyRates.ConvertCurrency(1, "USD", toCurrency);

            decimal convertedAmount = amount * (toRate / fromRate);

            // Запрос подтверждения обмена
            MessageBoxResult confirmResult = MessageBox.Show(
                $"Подтвердите обмен:\n{amount} {fromCurrency} = {convertedAmount} {toCurrency}\n\nПродолжить?", 
                "Подтверждение обмена", 
                MessageBoxButton.YesNo, 
                MessageBoxImage.Question);
                
            if (confirmResult == MessageBoxResult.Yes)
            {
                // Сохранение транзакции в базу данных
                await using var context = new ApplicationContext();
                
                // Получаем имя текущего пользователя (например, из сессии)
                string currentOperator = App.CurrentUser?.Login ?? "Неизвестный оператор";
                
                var transaction = new Transaction
                {
                    FromCurrency = fromCurrency,
                    ToCurrency = toCurrency,
                    AmountFrom = amount,
                    AmountTo = convertedAmount,
                    TransactionDate = DateTime.Now,
                    OperatorName = currentOperator
                };
                
                context.Transactions.Add(transaction);
                await context.SaveChangesAsync();

                // Показываем сообщение об успешном обмене
                MessageBox.Show(
                    $"Обмен выполнен успешно!\n\nСумма после обмена: {convertedAmount} {toCurrency}\nИнформация сохранена в базе данных.", 
                    "Успешный обмен", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Information);
                    
                // Очистка поля ввода
                ExchangeAmount.Text = "Введите сумму";
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка обмена", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
        private async void UpdateRatesButton_Click(object sender, RoutedEventArgs e)
        {
            await UpdateCurrencyRates();
        }
    }
}