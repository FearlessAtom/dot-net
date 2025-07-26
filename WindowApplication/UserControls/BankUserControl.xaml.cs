using System.Windows;
using System.Windows.Controls;
using ClassLibrary.Data.Models;
using CLassLibrary.Data.Models;
using WindowApplication.Data;

namespace WindowApplication.UserControls;

public partial class BankUserControl : UserControl
{
    public Bank bank;   
    
    public BankUserControl(int Id)
    {
        InitializeComponent();
        Bank bank = Items.main_window.context.Banks.FirstOrDefault(b => b.Id == Id);
        this.bank = bank;
        string Name = Items.main_window.context.Companies.FirstOrDefault(c => c.Id == bank.CompanyId).Name;
        NameTextBlock.Text = Name;

        Company company = Items.main_window.context.Companies.FirstOrDefault(c => c.Id == bank.CompanyId);
        
        LogoPath.Source = Utils.GetLogo(company.LogoPath);
    }

    private void CloseBank(object sender, RoutedEventArgs e)
    {
        Grid parent = (Grid)Parent;
        parent.Children.Remove(this);
        Items.main_window.WindowState = WindowState.Normal;
    }

    private void PinChange(object sender, RoutedEventArgs e)
    {
        Items.main_window.MainGrid.Children.Add(new PinChangeFormUserControl());
    }

    private void CreateAccount(object sender, RoutedEventArgs e)
    {
        Items.main_window.MainGrid.Children.Add(new AccountCreateUserControl(bank));
    }

    private void OpenATM(object sender, RoutedEventArgs e)
    {
        ATM atm = new ATM()
        {
            CompanyId = bank.CompanyId,
            Balance = 100000,
        };
        
        Grid parent = (Grid)Parent;
        parent.Children.Remove(this);
        
        Items.main_window.MainGrid.Children.Add(new ATMLoginUserControl(atm));
    }
}