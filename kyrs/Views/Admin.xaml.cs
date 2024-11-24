using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using kyrs.Models;
using Microsoft.EntityFrameworkCore;

namespace kyrs.Views;

public partial class Admin : Page
{
    public Admin()
    {
        InitializeComponent();
        
    }

    private async void AddUserButton_Click(object sender, RoutedEventArgs e)
    {
        string login = UserName.Text;
        string password = UserPassword.Text;
        string role = UserRole.Text;
        
        await using var context = new ApplicationContext();
        var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Login == login);
        if (existingUser != null)
        {
            MessageBox.Show("Пользователь с таким логином уже существует");
            return;
        }

        var newUser = new User
        {
            Login = login,
            Role = role
        };
        newUser.SetPassword(password); // Хеширование пароля
        context.Users.Add(newUser);
        await context.SaveChangesAsync();
        MessageBox.Show("Пользователь добавлен");
    }
    

    
    private void ViewLogsButton_Click(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void ConfigurationButton_Click(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void BackupButton_Click(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
    
    private void TextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        TextBox textBox = (TextBox)sender;
        if (textBox.Text == "Введите имя пользователя" || textBox.Text == "Введите пароль")
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
            if (textBox.Name == "UserName")
            {
                textBox.Text = "Введите имя пользователя";
            }
            else if (textBox.Name == "UserPassword")
            {
                textBox.Text = "Введите пароль";
            }
            textBox.Foreground = new SolidColorBrush(Colors.White);
        }
    }

    private void ToUserList(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new UserList());
    }
}


