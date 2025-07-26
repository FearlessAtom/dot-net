using System.ComponentModel.DataAnnotations.Schema;
using CLassLibrary;
using ClassLibrary.Data.TDOs;

namespace ClassLibrary.Data.Models;

public class Account
{
    [NotMapped] public static ApplicationDbContext context = new ApplicationDbContext();
    
    public int Id { get; set; }

    [NotMapped] static public Account? CurrentAccount = null;

    public delegate Account AccountHandler(object? sender, AccountEventArgs e);
    static public event AccountHandler? CreatedEvent;
    static public event AccountHandler? AuthorizedEvent;

    public delegate void AccountStateHandler(object? sender, AccountEventArgs e);
    static public event AccountStateHandler? AddedEvent;
    static public event AccountStateHandler? WithdrawnEvent;
    static public event AccountStateHandler? ErrorEvent;
    static public event AccountStateHandler? TransferEvent;
    static public event AccountStateHandler? ViewBalanceEvent;
    
    public decimal CurrentSum { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PinCodeHash { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public int CompanyId { get; set; }
    
    static public void Logout()
    {
        CurrentAccount = null;
    }
    
    static public string GenerateCardNumber()
    {
        string card_number;
        Random random = new Random();

        do
        {
            card_number = "5";

            for(int index = 0; index < 15; index++)
            {
                card_number = card_number + random.Next(0, 9);
            }

        }while(context.Accounts.FirstOrDefault(a => a.CardNumber == card_number) != null);

        return card_number;
    }
    
    public void ViewBalance()
    {
        ViewBalanceEvent?.Invoke(this, new AccountEventArgs($"{CurrentSum}$", 0, null, null));
    }

    static public Account Create(AccountTDO account)
    {
        return CreatedEvent?.Invoke(null, new AccountEventArgs(null, 0, null, account));
    }

    static public Account Authorize()
    {
        return AuthorizedEvent?.Invoke(null, null);
    }

    public void Transfer(decimal sum, string card_number)
    {
        TransferEvent?.Invoke(this, new AccountEventArgs(string.Empty, sum, card_number, null));
    }

    public void UpdateAccount()
    {
        CurrentAccount = context.Accounts.FirstOrDefault(a => a.CardNumber == CurrentAccount.CardNumber);
    }

    public void Put(decimal sum)
    {
        if (CurrentAccount == null)
        {
            return;
        }
        
        AddedEvent?.Invoke(this, new AccountEventArgs($"{(CurrentAccount.CardNumber == CardNumber ? "Your account" : CardNumber)} received {sum}$", sum, null, null));
        CurrentSum = CurrentSum + sum;
        context.SaveChanges();
    }

    public bool Withdraw(decimal sum, decimal fee)
    {
        if (CurrentAccount == null)
        {
            return false;
        }
        
        if(CurrentSum < sum)
        {
            WithdrawnEvent?.Invoke(this, new AccountEventArgs("Insufficient funds!", sum, null, null));
            return false;
        }
        
        CurrentSum = CurrentSum - sum;

        if (fee < CurrentSum)
        {
            CurrentSum = CurrentSum - fee;
        }

        WithdrawnEvent?.Invoke(this, new AccountEventArgs($"{sum}$ was withdrawn from your account!", sum, null, null));
        
        context.SaveChanges();
        return true;
    }
}