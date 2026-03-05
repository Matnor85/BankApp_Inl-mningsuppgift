using BankApp.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Accounts;

internal class SaveAccount : AccountBase
{
    
    private decimal Rate = 5;
    public SaveAccount(string name, decimal balance)
    {
        AccountType = "Sparkonto";
        AccountName = name;
        AccountNumber = GenerateNumber(16);
        CCV = GenerateNumber(3);
        StartingBalance = (int)balance;
        Id = Guid.NewGuid();
        InterestRate = Rate;

    }
    internal override decimal Balance()
    {
        var t = bankTransactions.Sum(x => x.Amount);
        return t + StartingBalance;
    }
}
