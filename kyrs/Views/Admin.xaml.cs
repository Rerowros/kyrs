using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using kyrs.Models;
using Microsoft.EntityFrameworkCore;
using kyrs.Views;
using NLog;



namespace kyrs.Views;

public partial class Admin : Page
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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
        string logFilePath = "logfile.txt"; 
        if (File.Exists(logFilePath))
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = logFilePath,
                UseShellExecute = true
            });
        }
        else
        {
            MessageBox.Show("Файл логов не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
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

    private void ViewTransactionsButton_Click(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new TransactionHistory());
    }
    
    
    private void GoToPage1_Click(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new Authorization());
        Logger.Info("Переход на страницу авторизации");
    }

    private void GoToPage2_Click(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new Cashier());
    }
    
    private void GoToPage3_Click(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new Admin());
    }


}


