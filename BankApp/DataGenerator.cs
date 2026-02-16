using BankApp.Accounts;
using BankApp.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp;

internal class DataGenerator
{
    // Shared Random så att vi inte får lika sekvenser vid snabba anrop
    private static readonly Random s_random = new Random();

    public void PopulateWithTestData(Bank bank)
    {
        // Skydda mot att fylla på testdata flera gånger (orsakar dubbletter)
        if (bank == null) throw new ArgumentNullException(nameof(bank));
        if (bank.accounts.Count > 0) return;

        string[] names =
            {
            "Anna Andersson",
            "Björn Borg",
            "Cecilia Ceder",
            "David Dahl",
            "Elin Eriksson",
            "Martin Molin",
            "Sara Sandström",
            "Robert Rosberg",
            "Ellen Ek",
            "Peter Persson",
            "Johanna Johansson"
            };
        for (int i = 0; i < names.Length; i++)
        {
            // Skapa konto
            AccountBase account;
            if (i % 2 == 0)
                account = new BankAccount(names[i], s_random.Next(1000, 10000));
            else
                account = new IskAccount(names[i], s_random.Next(5000, 50000));

            // Sätt typ (valfritt men bra att ha)
            account.AccountType = account.GetType().Name;

            // Lägg till via bank.AddAccount så eventuell dubblettskydd i Bank används
            bank.AddAccount(account);

            // Lägg till slumpmässiga transaktioner
            for (int t = 0; t < 10; t++)
            {
                decimal amount = s_random.Next(-1000, 2000); // Både insättning och uttag
                if (amount == 0) continue;

                if (amount > 0)
                    account.Deposit(amount);
                else
                    account.Withdraw(Math.Abs(amount));
            }
        }
    }
}
/*
  public void PopulateWithTestData(Bank bank)
{
    Random random = new Random();
    string[] names = { "Anna", "Björn", "Cecilia", "David", "Martin", "Sara", "Robert", "Ellen", "Peter", "Johanna" };

    for (int i = 0; i < names.Length; i++)
    {
        // Skapa konton
        AccountBase account;
        if (i % 2 == 0)
            account = new BankAccount(names[i], random.Next(1000, 10000));
        else
            account = new IskAccount(names[i], random.Next(5000, 50000));

        bank.accounts.Add(account); // Lägg till kontot direkt i bankens lista
        //bank.AddAccount(account);

        // Lägg till slumpmässiga transaktioner
        for (int t = 0; t < 10; t++)
        {
            decimal amount = random.Next(-1000, 2000); // Både insättning och uttag
            if (amount != 0)
            {
                // Lägg till transaktionen direkt i kontots lista
                // (Här använder vi din Deposit/Withdraw logik)
                if (amount > 0)
                    account.Deposit(amount);
                else
                    account.Withdraw(Math.Abs(amount));
            }
        }
    }
}
 */


