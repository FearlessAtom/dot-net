using System.Windows;
using System.Windows.Controls;
using ClassLibrary.Data.Models;
using WindowApplication.Data;

namespace WindowApplication.UserControls;

public partial class BanksList : UserControl
{
    public BanksList(bool BanksSort)
    {
        InitializeComponent();
        
        List<Bank> Banks = Items.main_window.context.Banks.ToList();
        
        Banks.Sort((a, b) =>
        {
            string a_name = Items.main_window.context.Companies.FirstOrDefault(c => c.Id == a.CompanyId).Name;
            string b_name = Items.main_window.context.Companies.FirstOrDefault(c => c.Id == b.CompanyId).Name;
            
            if (BanksSort)
            {
                return string.Compare(a_name, b_name);
            }
            
            return string.Compare(b_name, a_name);
        });

        for (int index = 0; index < Banks.Count; index++)
        {
            var bank_list_item = new BankListItem(Banks[index]);
            
            if (index + 1 != Banks.Count)
            {
                bank_list_item.Margin = new Thickness(0, 0, 0, 5);
            }
            
            MainStackPanel.Children.Add(bank_list_item);
        }
    }
}