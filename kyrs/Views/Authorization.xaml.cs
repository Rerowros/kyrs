using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using kyrs.Models;
using Microsoft.EntityFrameworkCore;

namespace kyrs.Views
{
    public partial class Authorization
    {


        public Authorization()
        {
            InitializeComponent();
        }
        
        private async void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordBox.Password;

            await using var context = new ApplicationContext();
            var user = await context.Users.FirstOrDefaultAsync(u => u.Login == login);
            if (user != null && user.ValidatePassword(password))
            {
                // Сохраняем информацию о текущем пользователе
                App.CurrentUser = user;
        
                MessageBox.Show("Успешный вход");
                if (NavigationService != null) NavigationService.Navigate(new Cashier());
            }
            else
            {
                LabelError.Visibility = Visibility.Visible;
            }
        }
        
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Введите логин")
            {
                textBox.Text = "";
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

        private void Label_Error_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Логика для изменения видимости ошибки
        }
    }
}