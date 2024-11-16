using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using kyrs.Views;

namespace kyrs;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainFrame.Navigate(new Authorization());
    }
    private void GoToPage1_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new Authorization());
        Console.WriteLine("2");
    }

    private void GoToPage2_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new Cashier());
    }
    private void GoToPage3_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new Admin());
    }
    
    private void GoToPage4_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new Manager());
    }
    
}