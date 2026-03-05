using BankApp.Accounts;
using BankApp.Base;
using BankApp.Models;
using BankApp.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Faktory;

internal static class AccountFaktory 
{
        public static AccountBase CreateAccount(AccountDetails accountDetails)
    {
        switch (accountDetails.AccountType)
        {
            case AccountType.Bankkonto:
                //return new Accounts.BankAccount(accountDetails.AccountName, accountDetails.Balance);
                return new BankAccount(accountDetails.AccountName, accountDetails.Balance);
                // break;
             
            case AccountType.ISK:
                return new IskAccount(accountDetails.AccountName, accountDetails.Balance);
            
            case AccountType.Uddevallakonto:
                return new UddevallaAccount(accountDetails.AccountName, accountDetails.Balance);
           
            case AccountType.Sparkonto:
                return new SaveAccount(accountDetails.AccountName, accountDetails.Balance);
            
            case AccountType.Fasträntekonto:
                return new FastränteAccount(accountDetails.AccountName, accountDetails.Balance);
            
            default:
                throw new NotImplementedException();
        }
    }
}
