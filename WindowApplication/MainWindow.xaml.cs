using System.Windows;
using ClassLibrary.Data;
using ClassLibrary.Data.Models;
using ClassLibrary.Data.TDOs;
using WindowApplication.Data;
using WindowApplication.UserControls;

namespace WindowApplication;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Items.main_window = this;
        ToggleList(List.ATMs);

        Account.AccountStateHandler SendMessage = (sender, e) =>
        {
            MessageBox.Show(e.Message);
        };

        Account.ViewBalanceEvent += (sender, e) =>
        {
            MessageBox.Show($"Current balance: {((Account)sender).CurrentSum.ToString()}$");
        };

        Account.AuthorizedEvent += (sender, e) =>
        {
           string card_number = Items.atm_login_usercontrol.CardNumberTextBox.Text;
           string pin_code = Items.atm_login_usercontrol.PinCodePasswordBox.Password;
           
           Account? account = context.Accounts.FirstOrDefault(a => a.CardNumber == card_number);
            
           if(account == null)
           {
               MessageBox.Show("Invalid credentials!");
               return null;
           }

           if(!BCrypt.Net.BCrypt.Verify(pin_code, account.PinCodeHash))
           {
               MessageBox.Show("Invalid credentials!");
               return null;
           }
            
           Account.CurrentAccount = account;
           Account.CurrentAccount.UpdateAccount();
           return account;
        };

        Account.TransferEvent += (sender, e) =>
        {
            if (Account.CurrentAccount.CurrentSum < e.Sum)
            {
                MessageBox.Show("Insufficient funds!");
                return;
            }

            Account receiver = context.Accounts.FirstOrDefault(a => a.CardNumber == e.Receiver);

            if (receiver == null)
            {
                MessageBox.Show("Invalid card number!");
                return;
            }
            
            Account.CurrentAccount.CurrentSum -= (decimal)e.Sum;
            receiver.CurrentSum += (decimal)e.Sum;

            MessageBox.Show($"You successfully transferred {e.Sum} to {e.Receiver}");
            
            context.SaveChanges();
        };

        Account.CreatedEvent += (sender, e) =>
        {
            AccountTDO account_tdo = (AccountTDO)e.Object;

            Account new_account = new()
            {
                FirstName = account_tdo.FirstName,
                LastName = account_tdo.LastName,
                PinCodeHash = BCrypt.Net.BCrypt.HashPassword(account_tdo.PinCode),
                CardNumber = Account.GenerateCardNumber(),
                CompanyId = account_tdo.CompanyId,
            };
            
            Items.main_window.context.Accounts.Add(new_account);
            Items.main_window.context.SaveChanges();

            MessageBox.Show("Card number: " + new_account.CardNumber);
            Clipboard.SetText(new_account.CardNumber);
            
            return new_account;
        };
        
        Account.WithdrawnEvent += SendMessage;
        Account.AddedEvent += SendMessage;
    }
    
    private bool BanksSort = true;
    private bool ATMsSort = true;
    
    public readonly ApplicationDbContext context = new ApplicationDbContext();

    private void ToggleList(List list)
    {
        ListGrid.Children.Clear();
        
        switch (list)
        {
            case List.ATMs:
                ATMsButton.Background = UIColors.Active;
                BanksButton.Background = UIColors.Default;
                ListGrid.Children.Add(new ATMsList(ATMsSort));
                ATMsSort = !ATMsSort;
                return;
            case List.Banks:
                BanksButton.Background = UIColors.Active;
                ATMsButton.Background = UIColors.Default;
                ListGrid.Children.Add(new BanksList(BanksSort));
                BanksSort = !BanksSort;
                return;
        }
    }

    private void ATMsClick(object sender, RoutedEventArgs e)
    {
        ToggleList(List.ATMs);
    }

    private void BanksClick(object sender, RoutedEventArgs e)
    {
        ToggleList(List.Banks);
    }
}