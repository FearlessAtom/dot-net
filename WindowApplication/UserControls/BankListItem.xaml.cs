using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ClassLibrary.Data;
using ClassLibrary.Data.Models;
using ClassLibrary.Migrations;
using WindowApplication.Data;

namespace WindowApplication.UserControls;

public partial class BankListItem : UserControl
{
    private int Id;
    
    public BankListItem(Bank bank)
    {
        InitializeComponent();
        Id = bank.Id;
        Company company = Items.main_window.context.Companies.FirstOrDefault(c => c.Id == bank.CompanyId);
        NameTextBlock.Text = bank.Address;
        AddressTextBlock.Text = company.Name;
        LogoImage.Source = new BitmapImage(new Uri("../Data/Images/Logos/" + company.LogoPath, UriKind.Relative));
    }

    private void GoToBank(object sender, MouseButtonEventArgs e)
    {
        Items.main_window.MainGrid.Children.Add(new BankUserControl(Id));
        Items.main_window.WindowState = WindowState.Maximized;
    }
}