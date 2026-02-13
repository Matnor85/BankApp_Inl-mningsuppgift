using BankApp.Accounts;
using BankApp.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp;

internal class DataGenerator
{
    public void PopulateWithTestData(Bank bank)
    {
        Random random = new Random();
        string[] names = { "Anna", "Björn", "Cecilia", "David", "Martin", "Sara", "Robert", "Ellen", "Peter", "Johanna" };

        for (int i = 0; i < names.Length; i++)
        {
            // 1. Skapa konton
            AccountBase account;
            if (i % 2 == 0)
                account = new BankAccount(names[i], random.Next(1000, 10000));
            else
                account = new IskAccount(names[i], random.Next(5000, 50000));

            bank.AddAccount(account);

            // 2. Lägg till slumpmässiga transaktioner
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
}
