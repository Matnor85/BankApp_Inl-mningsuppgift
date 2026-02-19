using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankApp.Base;

internal abstract class AccountBase
{
    internal string AccountType { get; set; }
    internal Guid Id { get; set; } = Guid.NewGuid();
    internal int StartingBalance { get; set; } = 0;
    internal string AccountName { get; set; } = "";
    internal string AccountNumber { get; set; } = "";
    internal string CCV { get; set; }
    public decimal InterestRate { get; set; } = 0;

    protected List<BankTransaction> bankTransactions = new List<BankTransaction>();
   

    internal abstract decimal Balance();

    public List<BankTransaction> GetTransactions()
    {
        return bankTransactions;
    }

    internal virtual bool Deposit(decimal amount)
    {
        if (amount <= 0) return false;

        var t = new BankTransaction
        {
            Amount = amount,
            TrancactionDate = DateTime.Now
        };
        bankTransactions.Add(t);
        return true;
    }

    internal virtual bool Withdraw(decimal amount)
    {
        if (amount <= 0) return false;

        if (Balance() - amount < 0)
        {
            return false;
        }
        var t = new BankTransaction
        {
            Amount = -amount,
            TrancactionDate = DateTime.Now
        };
        bankTransactions.Add(t);
        return true;
    }
    // För att undvika att skapa en ny Random-instans varje gång,
    // vilket kan leda till samma nummer om det sker snabbt, använder vi en statisk instans.
    private static readonly Random s_random = new Random();

    // Robust GenerateNumber: kan ta en befintlig kontolista att kontrollera unikt mot.
    public string GenerateNumber(int t, IEnumerable<AccountBase>? existingAccounts = null)
    {
        // Validera att t är positivt för att undvika oändliga loopar eller ogiltiga nummer.
        if (t <= 0) throw new ArgumentOutOfRangeException(nameof(t), "t måste vara > 0");

        // Skapa en HashSet av befintliga kontonummer för snabb uppslagning. Ignorera null eller tomma nummer.
        var existing = (existingAccounts ?? Enumerable.Empty<AccountBase>())
                       .Select(a => a.AccountNumber)
                       .Where(n => !string.IsNullOrEmpty(n))
                       .ToHashSet(StringComparer.OrdinalIgnoreCase);

        string newAccountNumber;
        do
        {
            // Bygger kontonumren med en StringBuilder för bättre prestanda
            var sb = new StringBuilder(t + (t - 1) / 4);
            for (int i = 0; i < t; i++)
            {
                sb.Append(s_random.Next(0, 10));
                if ((i + 1) % 4 == 0 && i < t - 1)
                {
                    sb.Append('-');
                }
            }
            // Generera det nya kontonumret och kontrollera mot befintliga nummer. Loopar tills det är unikt.
            newAccountNumber = sb.ToString();
        } while (existing.Contains(newAccountNumber));
        // När vi har ett unikt nummer, returnerar man det.
        return newAccountNumber;
    }

    // Backwards-compatible overload om nån använder den gamla signaturen:
    public string GenerateNumber(int t)
    {
        return GenerateNumber(t, null);
    }

    public void CalculateInterest()
    {
        for (int i = 0; i < bankTransactions.Count; i++)
        {
            var transaction = bankTransactions[i];
            if (transaction.TrancactionDate.AddMonths(1) <= DateTime.Now)
            {
                var interest = Balance() * InterestRate / 100;
                var interestTransaction = new BankTransaction
                {
                    Amount = interest,
                    TrancactionDate = DateTime.Now
                };
                bankTransactions.Add(interestTransaction);
            }
        }
    }
}
    //public string GenerateNumber(int t)
    //{
    //    Random random = new Random();
    //    string newAccountNumber;
    //    bool isUnique;
    //    do
    //    {
    //        newAccountNumber = "";
    //        isUnique = true;

    //        for (int i = 0; i < t; i++)
    //        {
    //            // Slumpa en siffra mellan 0 och 9 och lägg till i strängen
    //            newAccountNumber += random.Next(0, 10).ToString();

    //            // Efter var fjärde tecken så läggs ett bindesträck till för att lättare kunna läsa nummret.
    //            if ((i + 1) % 4 == 0 && i < t - 1)
    //            {
    //                newAccountNumber += "-";
    //            }
    //        }

    //        // Här kontrollerar vi så att det blir ett unikt konmto nummer
    //        Bank bank = new Bank();
    //        foreach (var account in bank.accounts)
    //        {
    //            if (account.AccountNumber == newAccountNumber)
    //            {
    //                isUnique = false; 
    //                break;
    //            }
    //        }
    //      // körs tills det blir ett unikt nummer.
    //    } while (!isUnique);
    //    return newAccountNumber;
    //}
   
//}
