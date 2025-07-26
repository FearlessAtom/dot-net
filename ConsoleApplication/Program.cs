using ClassLibrary.Data.Models;
using ClassLibrary.Data.TDOs;
using CLassLibrary.Data.Models;
using static ClassLibrary.Data.Models.Account;

namespace ConsoleApplication;

class Program
{
    static void Main(string[] args)
    {
        AccountStateHandler SendMessage = (sender, e) =>
        {
            Console.WriteLine(e.Message);
        };

        WithdrawnEvent += SendMessage;
        AddedEvent += SendMessage;

        ViewBalanceEvent += (sender, e) =>
        {
            Console.WriteLine("Current balance: " + ((Account)sender).CurrentSum);
        };
        
        AuthorizedEvent += (sender, e) =>
        {
            Console.Write("Enter the card number: ");
            string card_number = Console.ReadLine();

            Console.Write("Enter the pin code: ");
            string pin_code = Console.ReadLine();

            Account account = context.Accounts.FirstOrDefault(a => a.CardNumber == card_number);

            if(account == null)
            {
                Console.WriteLine("Invalid credentials!");
                Console.ReadLine();
                return null;
            }

            if(!BCrypt.Net.BCrypt.Verify(pin_code, account.PinCodeHash))
            {
                Console.WriteLine("Invalid credentials!");
                Console.ReadLine();
                Authorize();
                return null;
            }

            CurrentAccount = account;
            ATMMenu();

            return account;
        };

        TransferEvent += (sender, e) =>
        {
            if(e.Sum > CurrentAccount.CurrentSum)
            {
                Console.WriteLine("Insufficient funds!");
                return;
            }

            Account receiver = context.Accounts.FirstOrDefault(a => a.CardNumber == e.Receiver);

            if(receiver == null)
            {
                Console.WriteLine("Invalid card number!");
                return;
            }

            CurrentAccount.CurrentSum -= (decimal)e.Sum;
            receiver.CurrentSum += (decimal)e.Sum;
            context.SaveChanges();

            Console.WriteLine($"You successully transerred {e.Sum} to {e.Receiver}");
        };

        CreatedEvent += (sender, e) =>
        {
            AccountTDO account_tdo = (AccountTDO)e.Object;
            
            Account new_account = new Account()
            {
                FirstName = account_tdo.FirstName,
                LastName = account_tdo.LastName,
                CompanyId = account_tdo.CompanyId, 
                PinCodeHash = BCrypt.Net.BCrypt.HashPassword(account_tdo.PinCode),
                CardNumber = GenerateCardNumber(),
            };

            context.Accounts.Add(new_account);
            context.SaveChanges();

            Console.WriteLine("Card number: " + new_account.CardNumber);

            return new_account;
        };

        MainMenu();
    }

    public static ActionEvent MainMenu = () => CreateOptions
    (
        new string[] {"ATMs", "Banks", "Exit"}, 
        new ActionEvent[] {ATMs, Banks, () => { Environment.Exit(0); }}
    );

    public static void PinCodeChange()
    {
        Console.Write("Enter your card number:" );
        string card_number = Console.ReadLine();

        Console.Write("Enter your current pin code:" );
        string current_pin_code = Console.ReadLine();

        Console.Write("Enter your new pin code:" );
        string new_pin_code = Console.ReadLine();

        Console.Write("Confirm your new pin code:" );
        string confirm_pin_code = Console.ReadLine();

        Account account = context.Accounts.FirstOrDefault(a => a.CardNumber == card_number);

        if (account == null)
        {
            Console.WriteLine("Invalid credentials!");
            Console.ReadLine();
            BanksMenu();
        }

        if (new_pin_code != confirm_pin_code)
        {
            Console.WriteLine("Pin codes do not match!");
            Console.ReadLine();
            BanksMenu();
        }

        if (!BCrypt.Net.BCrypt.Verify(current_pin_code, account.PinCodeHash))
        {
            Console.WriteLine("Invalid credentials!");
            Console.ReadLine();
            BanksMenu();
        }

        account.PinCodeHash = BCrypt.Net.BCrypt.HashPassword(new_pin_code);
        context.SaveChanges();

        Console.WriteLine("Your pin code was changed successfully!");
    }

    public static decimal GetAmount()
    {
        decimal amount;
        bool result;
        
        do
        {
            Console.Write("Amount: ");
            result = decimal.TryParse(Console.ReadLine(), out amount);

            if(!result)
            {
                Console.WriteLine("Invalid input!");
            }
        }while(!result);

        return amount;
    }

    public static ActionEvent BanksMenu = () => CreateOptions
    (
        new string[] {"Create Account","Pin Change", "Open ATM", "Back"},
        new ActionEvent[] {() =>
        {
            AccountTDO account_tdo = new AccountTDO();

            Console.Write("Enter your first name: ");
            account_tdo.FirstName = Console.ReadLine();

            Console.Write("Enter your last name: ");
            account_tdo.LastName = Console.ReadLine();

            Console.Write("Enter your pin code: ");
            string pin_code = Console.ReadLine();

            Console.Write("Confirm your pin code:");
            string confirm_pin_code = Console.ReadLine();

            if(pin_code != confirm_pin_code)
            {
                Console.WriteLine("Pin codes do not match!");
                Console.ReadLine();
                BanksMenu();
            }

            if(pin_code.Length != 4)
            {
                Console.WriteLine("Pin code must be 5 characters long!");
                Console.ReadLine();
                BanksMenu();
            }

            account_tdo.PinCode = pin_code;
            account_tdo.CompanyId = Bank.CurrentBank.CompanyId;

            Create(account_tdo);
            Console.ReadKey();
            BanksMenu();
        }, () =>
        {
            PinCodeChange(); 
        }, () =>
        {
            ATM.CurrentATM = new()
            {
                CompanyId = Bank.CurrentBank.CompanyId,
                Balance = 100000,
            };

            AccountLogin();
        }, MainMenu}
    );

    public static ActionEvent ATMMenu = () => CreateOptions
    (
        new string[] {"View Balance","Cash Withdrawal", "Deposit", "Transfer", "Back"},
        new ActionEvent[] {() =>
        {
            CurrentAccount.ViewBalance();
            Console.ReadKey();
            Console.Clear();
            ATMMenu();
        }, () =>
        {
        //     if (CurrentAccount.CompanyId != ATM.CurrentATM.CompanyId)
        //     {
        //         Console.WriteLine("Please note that you'll be charged 0.50 cents for using a foreign ATM");
        //         Console.ReadLine();
        //     }

            CurrentAccount.Withdraw(GetAmount(), 0);
            Console.ReadKey();
            ATMMenu();
        }, () =>
        {
            CurrentAccount.Put(GetAmount());
            Console.ReadKey();
            ATMMenu();
        },
        () =>
        {
            decimal amount = GetAmount();

            Console.Write("Card number: ");
            string card_number = Console.ReadLine();

            CurrentAccount.Transfer(amount, card_number);

            Console.ReadLine();
            ATMMenu();
        },
        () =>
        {
            MainMenu();
        }}
    );

    public static void AccountLogin()
    {
        Authorize();
    }

    public delegate void ActionEvent();

    public static void CreateOptions(string[] Options, ActionEvent[] Actions)
    {
        if(Options.Length != Actions.Length) return;

        Console.Clear();

        while (true)
        {
            for (int index = 0; index < Options.Length; index++)
            {
                Console.WriteLine($"{index + 1} > {Options[index]}");
            }

            int option = Console.ReadKey().KeyChar - 48;
            Console.WriteLine();

            try
            {
                Console.Clear();
                Actions[option - 1]?.Invoke();
                break;
            }
            catch
            {
                Console.Clear();
            }
        }
    }

    public static void ATMs()
    {
        List<ATM> ATMs = context.ATMs.ToList();

        for(int index = 0; index < ATMs.Count; index++)
        {
            Company company = context.Companies.FirstOrDefault(c => c.Id == ATMs[index].CompanyId);

            Console.WriteLine($"{index + 1} | {company.Name} | {ATMs[index].Address}");
        }

        while(true)
        {
            try
            {
                int option = int.Parse(Console.ReadLine());
                ATM.CurrentATM = ATMs[option - 1];

                Console.Clear();
                Authorize();
            }
            catch
            {
                Console.WriteLine("Invalid id!");
            }
        }
    }

    public static void Banks()
    {
        List<Bank> Banks = context.Banks.ToList();

        for(int index = 0; index < Banks.Count; index++)
        {
            Company company = context.Companies.FirstOrDefault(c => c.Id == Banks[index].CompanyId);

            Console.WriteLine($"{index + 1} | {company.Name} | {Banks[index].Address}");
        }

        while(true)
        {
            try
            {
                int option = int.Parse(Console.ReadLine());
                Bank.CurrentBank = Banks[option - 1];

                Console.Clear();
                BanksMenu();
            }
            catch
            {
                Console.WriteLine("Invalid id!");
            }
        }
    }
}