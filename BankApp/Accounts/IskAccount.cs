using BankApp.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Accounts;

internal class IskAccount : AccountBase
{
    private decimal Rate = 2;

    public IskAccount(string name, decimal balance)
    {
        AccountType = "ISK-Konto";
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
