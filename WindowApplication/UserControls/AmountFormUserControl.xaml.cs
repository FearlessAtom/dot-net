using System.Windows;
using System.Windows.Controls;

namespace WindowApplication.UserControls;

public partial class AmountFormUserControl : UserControl
{
    public delegate void AmountFormHandler(decimal amount);
    public event AmountFormHandler EnteredEvent;
    
    public AmountFormUserControl(AmountFormHandler handler, string Title)
    {
        InitializeComponent();
        EnteredEvent += handler;
        TitleTextBlock.Text = Title;
    }

    private void Entered(object sender, RoutedEventArgs e)
    {
        decimal amount;
        
        if (!decimal.TryParse(AmountTextBox.Text, out amount))
        {
            MessageBox.Show("Invalid input!");
            return;
        }
        
        CloseForm(null, null);
        EnteredEvent?.Invoke(amount);
    }

    private void CloseForm(object sender, RoutedEventArgs e)
    {
        Grid parent = (Grid)Parent;
        parent.Children.Remove(this);
    }
}