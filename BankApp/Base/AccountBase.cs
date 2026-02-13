using System;
using System.Collections.Generic;
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
    protected AccountBase()
    {

    }

    internal abstract decimal Balance();

    internal virtual void Deposit(decimal amount)
    {
        var t = new BankTransaction
        {
            Amount = amount,
            TrancactionDate = DateTime.Now
        };
        bankTransactions.Add(t);

    }

    internal virtual void Withdraw(decimal amount)
    {
        if (Balance() - amount < 0)
        {
            Console.WriteLine("Du har inte tillräkligt med pengar på kontot!");
            Console.ReadLine();
            return;
        }
        var t = new BankTransaction
        {
            Amount = -amount,
            TrancactionDate = DateTime.Now
        };
        bankTransactions.Add(t);
    }
    public string GenerateNumber(int t)
    {
        Random random = new Random();
        string newAccountNumber;
        bool isUnique;
        do
        {
            newAccountNumber = "";
            isUnique = true;

            for (int i = 0; i < t; i++)
            {
                // Slumpa en siffra mellan 0 och 9 och lägg till i strängen
                newAccountNumber += random.Next(0, 10).ToString();

                if ((i + 1) % 4 == 0 && i < t - 1) // en if-sats som kontrollerar
                {
                    newAccountNumber += "-";
                }
            }
            Bank bank = new Bank();
            foreach (var account in bank.accounts)
            {
                if (account.AccountNumber == newAccountNumber)
                {
                    isUnique = false; 
                    break;
                }
            }
        } while (!isUnique);
        return newAccountNumber;
    }
   
}
