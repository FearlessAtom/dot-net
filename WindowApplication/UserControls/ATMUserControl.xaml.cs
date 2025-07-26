using System.Windows;
using System.Windows.Controls;
using ClassLibrary.Data.Models;
using CLassLibrary.Data.Models;
using WindowApplication.Data;

namespace WindowApplication.UserControls;

public partial class ATMUserControl : UserControl
{
    public Account account;
    public ATM atm;
    
    public ATMUserControl(Account account, ATM atm)
    {
        InitializeComponent();
        this.account = account;
        this.atm = atm;
        Company company = Items.main_window.context.Companies.FirstOrDefault(c => c.Id == atm.CompanyId);
        NameTextBlock.Text = company.Name;
        LogoPath.Source = Utils.GetLogo(company.LogoPath);
        
        if (account.CompanyId != atm.CompanyId)
        {
            MessageTextBlock.Text = "Please note that you'll be charged 0.50 cents for using a foreign ATM";
        }
    }
    
    private void Close(object sender, RoutedEventArgs e)
    {
        Grid parent = (Grid)Parent;
        parent.Children.Remove(this);
    }
    
    private void ViewBalance(object sender, RoutedEventArgs e)
    {
        Account.CurrentAccount.ViewBalance();
    }

    private void Withdraw(object sender, RoutedEventArgs e)
    {
        AmountFormUserControl.AmountFormHandler Withdraw = (decimal amount) =>
        {
            if (amount > atm.Balance)
            {
                MessageBox.Show("Sorry, there are insufficient funds available in the ATM for your withdrawal.");
                return;
            }
            
            atm.Balance = atm.Balance - amount;
            Items.main_window.context.SaveChanges();
            
            Account.CurrentAccount.Withdraw(amount, atm.CompanyId == account.CompanyId ? 0 : Utils.ForeignFee);
        };

        Items.main_window.MainGrid.Children.Add(new AmountFormUserControl(Withdraw, "Cash Withdrawal"));
    }
    
    private void Deposit(object sender, RoutedEventArgs e)
    {
        Items.main_window.MainGrid.Children.Add(new AmountFormUserControl((decimal amount) => Account.CurrentAccount.Put(amount), "Deposit"));;
    }
    
    private void Transfer(object sender, RoutedEventArgs e)
    {
        Items.main_window.MainGrid.Children.Add(new TransferFormUserControl((decimal amount, string card_number) => Account.CurrentAccount.Transfer(amount, card_number), "Transfer"));
    }
}