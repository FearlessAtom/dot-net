using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ClassLibrary.Data;
using ClassLibrary.Data.Models;
using CLassLibrary.Data.Models;
using WindowApplication.Data;

namespace WindowApplication.UserControls;

public partial class ATMsListItem : UserControl
{
    public ATM atm;
    
    public ATMsListItem(ATM atm)
    {
        InitializeComponent();
        this.atm = atm;
        Company company = Items.main_window.context.Companies.FirstOrDefault(c => c.Id == atm.CompanyId);
        NameTextBlock.Text = atm.Address;
        AddressTextBlock.Text = company.Name;
        LogoImage.Source = new BitmapImage(new Uri("../Data/Images/Logos/" + company.LogoPath, UriKind.Relative));
    }

    private void OpenATM(object sender, MouseButtonEventArgs e)
    {
        Items.main_window.MainGrid.Children.Add(new ATMLoginUserControl(atm));
        Items.main_window.WindowState = WindowState.Maximized;
    }
}