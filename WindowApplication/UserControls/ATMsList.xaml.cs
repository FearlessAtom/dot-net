using System.Windows;
using System.Windows.Controls;
using ClassLibrary.Data;
using CLassLibrary.Data.Models;
using WindowApplication.Data;

namespace WindowApplication.UserControls;

public partial class ATMsList : UserControl
{
    public ATMsList(bool ATMsSort)
    {
        InitializeComponent();

        List<ATM> ATMs = Items.main_window.context.ATMs.ToList();
        
        ATMs.Sort((a, b) =>
        {
            string a_name = Items.main_window.context.Companies.FirstOrDefault(c => c.Id == a.CompanyId).Name;
            string b_name = Items.main_window.context.Companies.FirstOrDefault(c => c.Id == b.CompanyId).Name;

            if (ATMsSort)
            {
                return string.Compare(b_name, a_name);
            }

            return string.Compare(a_name, b_name);
        });
        
        for (int index = 0; index < ATMs.Count; index++)
        {
            ATMsListItem atms_list_item = new ATMsListItem(ATMs[index]);

            if (index + 1 != ATMs.Count)
            {
                atms_list_item.Margin = new Thickness(0, 0, 0, 5);
            }

            MainStackPanel.Children.Add(atms_list_item);
        }
    }
}