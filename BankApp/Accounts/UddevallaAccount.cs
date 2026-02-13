using BankApp.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Accounts;

internal class UddevallaAccount : AccountBase
{
    private double Rate = 1.5;
    public UddevallaAccount(string name, decimal balance)
    {
        AccountType = "Uddevallakonto";
        AccountName = name;
        AccountNumber = GenerateNumber(16);
        CCV = GenerateNumber(3);
        StartingBalance = (int)balance;
        Id = Guid.NewGuid();
        InterestRate = (decimal)Rate;

    }
    internal override decimal Balance()
    {
        var t = bankTransactions.Sum(x => x.Amount);
        return t + StartingBalance;
    }
}
