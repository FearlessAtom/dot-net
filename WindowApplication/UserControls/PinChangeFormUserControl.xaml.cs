using System.Windows;
using System.Windows.Controls;
using ClassLibrary.Data.Models;
using WindowApplication.Data;

namespace WindowApplication.UserControls;

public partial class PinChangeFormUserControl : UserControl
{
    public PinChangeFormUserControl()
    {
        InitializeComponent();
    }

    private void Entered(object sender, RoutedEventArgs e)
    {
        Account account = Items.main_window.context.Accounts.FirstOrDefault(a => a.CardNumber == CardNumberTextBox.Text);

        if (account == null)
        {
            MessageBox.Show("Invalid credentials!");
            return;
        }

        if (NewPinCodeTextBox.Password != ConfirmPinCodeTextBox.Password)
        {
            MessageBox.Show("Pin codes do not match!");
            return;
        }

        if (!BCrypt.Net.BCrypt.Verify(CurrentPinCodeTextBox.Password, account.PinCodeHash))
        {
            MessageBox.Show("Invalid credentials!");
            return;
        }

        account.PinCodeHash = BCrypt.Net.BCrypt.HashPassword(NewPinCodeTextBox.Password);
        Items.main_window.context.SaveChanges();
        MessageBox.Show("Your pin code was changed successfully!");
        
        CloseForm(null, null);
    }

    private void CloseForm(object sender, RoutedEventArgs e)
    {
        Grid parent = (Grid)Parent;
        parent.Children.Remove(this);
    }
}