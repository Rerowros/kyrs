using System.Linq;
using System.Windows;
using System.Windows.Controls;
using kyrs.Models;
using Microsoft.EntityFrameworkCore;

namespace kyrs.Views;

public partial class UserList : Page
{
    public UserList()
    {
        InitializeComponent();
        LoadUsers();
    }

    private async void LoadUsers()
    {
        await using var context = new ApplicationContext();
        var users = await context.Users.ToListAsync();
        UserDataGrid.ItemsSource = users;
    }

    private void EditUserButton_Click(object sender, RoutedEventArgs e)
    {
        var user = (User)((Button)sender).DataContext;
        var editUserWindow = new EditUserWindow(user);
        editUserWindow.ShowDialog();
        LoadUsers(); // Перезагрузить список пользователей после редактирования
    }
    private async void DeleteUserButton_Click(object sender, RoutedEventArgs e)
    {
        var user = (User)((Button)sender).DataContext;
        var result = MessageBox.Show($"Вы уверены, что хотите удалить пользователя: {user.Login}?", "Подтверждение удаления", MessageBoxButton.YesNo);
        if (result == MessageBoxResult.Yes)
        {
            await using var context = new ApplicationContext();
            context.Users.Remove(user);
            await context.SaveChangesAsync();
            LoadUsers();
        }
    }
    private void back(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new Admin());        
    }
}