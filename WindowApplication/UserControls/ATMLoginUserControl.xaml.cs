using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ClassLibrary.Data;
using ClassLibrary.Data.Models;
using CLassLibrary.Data.Models;
using WindowApplication.Data;

namespace WindowApplication.UserControls;

public partial class ATMLoginUserControl : UserControl
{
    public ATM atm { get; set; }
    
    public ATMLoginUserControl(ATM atm)
    {
        InitializeComponent();
        Items.atm_login_usercontrol = this;
        this.atm = atm;
        Company company = Items.main_window.context.Companies.FirstOrDefault(c => c.Id == atm.CompanyId);
        NameTextBlock.Text = company.Name;
        LogoPath.Source = new BitmapImage(new Uri("../Data/Images/Logos/" + company.LogoPath, UriKind.Relative));
    }

    private void CloseATM(object sender, RoutedEventArgs e)
    {
        Grid parent = (Grid)Parent;
        parent.Children.Remove(this);
        Items.main_window.WindowState = WindowState.Normal;
    }

    private void Reset(object sender, RoutedEventArgs e)
    {
        CardNumberTextBox.Text = string.Empty;
        PinCodePasswordBox.Password = string.Empty;
    }

    private void Enter(object sender, RoutedEventArgs e)
    {
        Account account = Account.Authorize();

        if (account == null)
        {
            return;
        }
        
        Reset(null, null);
        Items.main_window.MainGrid.Children.Add(new ATMUserControl(account, atm));
    }
}