using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using kyrs.Models;
using Microsoft.EntityFrameworkCore;

namespace kyrs.Views
{
    public partial class TransactionHistory : Page
    {
        public TransactionHistory()
        {
            InitializeComponent();
            LoadTransactions();
        }

        private async void LoadTransactions()
        {
            try
            {
                await using var context = new ApplicationContext();
                var transactions = await context.Transactions
                    .OrderByDescending(t => t.TransactionDate)
                    .ToListAsync();
                
                TransactionsGrid.ItemsSource = transactions;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке транзакций: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadTransactions();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}