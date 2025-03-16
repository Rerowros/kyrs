using System.Configuration;
using System.Data;
using System.Windows;
using kyrs.Models;


namespace kyrs;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{        
    public static User CurrentUser { get; set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Инициализация подключения к базе данных
        using (var context = new ApplicationContext())
        {
            context.Database.EnsureCreated();
        }
    }
}