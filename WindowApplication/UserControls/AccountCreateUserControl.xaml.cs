using System.Windows;
using System.Windows.Controls;
using ClassLibrary.Data.Models;
using ClassLibrary.Data.TDOs;

namespace WindowApplication.UserControls;

public partial class AccountCreateUserControl : UserControl
{
    public Bank bank;
    
    public AccountCreateUserControl(Bank bank)
    {
        InitializeComponent();
        this.bank = bank;
    }
    
    private void CloseForm(object sender, RoutedEventArgs e)
    {
        Grid parent = (Grid)Parent;
        parent.Children.Remove(this);
    }
    
    private void Entered(object sender, RoutedEventArgs e)
    {
        if (PinCodeTextBox.Password != ConfirmPinCodeTextBox.Password)
        {
            MessageBox.Show("Pin codes do not match!");
            return;
        }

        if (PinCodeTextBox.Password.Length != 4)
        {
            MessageBox.Show("Pin code must be 4 numbers long!");
            return;
        }

        AccountTDO account = new AccountTDO()
        {
            FirstName = FirstNameTextBox.Text,
            LastName = LastNameTextBox.Text,
            PinCode = PinCodeTextBox.Password,
            CompanyId = bank.CompanyId,
        };
        
        Account.Create(account);
        
        CloseForm(null, null);
    }
}