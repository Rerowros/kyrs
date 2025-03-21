using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.EntityFrameworkCore;
using kyrs.Models;
using System;
using System.Threading.Tasks;

namespace kyrs.Views
{
    public partial class Authorization : Page
    {
        public Authorization()
        {
            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Введите логин")
            {
                textBox.Text = string.Empty;
                textBox.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Введите логин";
                textBox.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        private async void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordBox.Password;

            // Проверка на пустые поля
            if (string.IsNullOrEmpty(login) || login == "Введите логин" || 
                string.IsNullOrEmpty(password))
            {
                LabelError.Content = "Заполните все поля";
                LabelError.Visibility = Visibility.Visible;
                return;
            }

            var user = await AuthenticateUser(login, password);
            
            if (user != null)
            {
                // Сохраняем информацию о пользователе (например, в статическом классе)
                App.CurrentUser = user;
                
                // Перенаправляем в зависимости от роли
                NavigateByRole(user.Role);
            }
            else
            {
                LabelError.Content = "Неверный логин или пароль";
                LabelError.Visibility = Visibility.Visible;
            }
        }
        
        private async Task<User> AuthenticateUser(string login, string password)
        {
            await using var context = new ApplicationContext();
            var user = await context.Users.FirstOrDefaultAsync(u => u.Login == login);
            
            if (user != null && user.ValidatePassword(password))
            {
                return user;
            }
            
            return null;
        }
        
        private void NavigateByRole(string role)
        {
            switch (role.Trim().ToLower())
            {
                case "администратор":
                    NavigationService.Navigate(new Admin());
                    break;
                case "кассир":
                    NavigationService.Navigate(new Cashier());
                    break;
                default:
                    MessageBox.Show($"Неизвестная роль пользователя: {role}", "Ошибка", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }

        private void Label_Error_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // заглушка
        }
    }
}