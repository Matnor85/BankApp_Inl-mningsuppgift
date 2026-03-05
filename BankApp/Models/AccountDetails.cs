using BankApp.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Models;

internal class AccountDetails
{
    public string AccountName { get; set; } = "";
    public string AccountNumber { get; set; } ="";
    public decimal Balance { get; set; }
    public AccountType AccountType { get; set; } = new AccountType();
}
