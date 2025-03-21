using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using kyrs.Models;
using Microsoft.EntityFrameworkCore;
using kyrs.Views;
using NLog;
namespace kyrs.Views
{
    public partial class EditUserWindow 
    {
        private readonly User _user;


        
        public EditUserWindow(User user) 
        {
            InitializeComponent();
            _user = user;
            LoadUserData();
        }

        private void LoadUserData()
        {
            UserName.Text = _user.Login;
            UserRole.SelectedItem = UserRole.Items.Cast<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == _user.Role);
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _user.Login = UserName.Text;
            if (!string.IsNullOrWhiteSpace(UserPassword.Password))
            {
                _user.SetPassword(UserPassword.Password);
            }
            _user.Role = ((ComboBoxItem)UserRole.SelectedItem).Content.ToString();

            await using var context = new ApplicationContext();
            context.Users.Update(_user);
            await context.SaveChangesAsync();
            MessageBox.Show("Изменения сохранены");
            Close();
        }

        private void back(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}