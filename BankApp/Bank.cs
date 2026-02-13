using BankApp.Accounts;
using BankApp.Base;
using BankApp.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using System.Xml.Linq;

namespace BankApp;

internal class Bank
{
    internal List<AccountBase> accounts = new List<AccountBase>();
    internal void AddAccount(AccountBase account)
    {
        accounts.Add(account);
    }
    internal void RemoveAccount(Guid accountId)
    {
        var account = accounts.FirstOrDefault(x => x.Id == accountId);
        if (account != null)
        {
            accounts.Remove(account);
        }
    }

    public void ShowMenu()
    {
        var menu = new ConsoleMenu(new[]
        {
            new MenuItem("Skapa konto", ShowAccountTypes),
            new MenuItem("Visa konton", ShowAccounts),
            new MenuItem("Ta bort Konto", RemoveAccount),
            new MenuItem("Hantera konto", HandleAccount),

            new MenuItem("Avsluta", () => Environment.Exit(0)),
        });

        menu.Run();
    }

    private void HandleAccount()
    {
        if (accounts.Count == 0)
        {
            Console.WriteLine("Du har inga konton registrerade.");
            Console.WriteLine("Tryck Enter för att fortsätta...");
            Console.ReadLine();
            return;
        }

        // Bygg konto-listan som användaren kan välja från (visar endast namn + kontonummer).
        var accountMenuItems = new List<MenuItem>();
        foreach (var acc in accounts)
        {
            var local = acc; // viktig för closure
            accountMenuItems.Add(new MenuItem($"- Konto namn: {local.AccountName} - Kontonummer: ({local.AccountNumber})", () =>
            {
                // När ett konto väljs, visa submenu för det kontot.
                var accountActions = new List<MenuItem>
                {
                    // Rubrik / icke-markerbar rad
                    new MenuItem($"Konto: {local.AccountName} ({local.AccountNumber})"),
                    new MenuItem("Sätt in pengar", () =>
                    {
                        Console.Clear();
                        Console.WriteLine($"--- INSÄTTNING till {local.AccountName} ---");
                        Console.WriteLine("Ange belopp:");
                        if (!decimal.TryParse(Console.ReadLine(), out var amount) || amount <= 0)
                        {
                            Console.WriteLine("Ogiltigt belopp. Tryck Enter för att fortsätta...");
                            Console.ReadLine();
                            throw new OperationCanceledException();
                        }

                        local.Deposit(amount);
                        Console.WriteLine($"Insättning {amount} kr genomförd. Tryck Enter för att fortsätta...");
                        Console.ReadLine();
                        throw new OperationCanceledException();
                    }),
                    new MenuItem("Ta ut pengar", () =>
                    {
                        Console.Clear();
                        Console.WriteLine($"--- UTTAG från {local.AccountName} ---");
                        Console.WriteLine("Ange belopp:");
                        if (!decimal.TryParse(Console.ReadLine(), out var amount) || amount <= 0)
                        {
                            Console.WriteLine("Ogiltigt belopp. Tryck Enter för att fortsätta...");
                            Console.ReadLine();
                            throw new OperationCanceledException();
                        }

                        if (local.Balance() < amount)
                        {
                            Console.WriteLine("Inte tillräckligt med pengar på kontot. Tryck Enter för att fortsätta...");
                            Console.ReadLine();
                            throw new OperationCanceledException();
                        }

                        local.Withdraw(amount);
                        Console.WriteLine($"Uttag {amount} kr genomfört. Tryck Enter för att fortsätta...");
                        Console.ReadLine();
                        throw new OperationCanceledException();
                    }),
                    new MenuItem("Visa saldo", () =>
                    {
                        Console.Clear();
                        Console.WriteLine($"Saldo för {local.AccountName} ({local.AccountNumber}): {local.Balance()} kr");
                        Console.WriteLine("Tryck Enter för att fortsätta...");
                        Console.ReadLine();
                        throw new OperationCanceledException();
                    }),
                    new MenuItem("Tillbaka", () => { throw new OperationCanceledException(); })
                };

                var accMenu = new ConsoleMenu(accountActions);
                accMenu.Run();
            }));
        }

        // Tillbaka-alternativ i konto-listan
        accountMenuItems.Add(new MenuItem("Tillbaka", () => { throw new OperationCanceledException(); }));

        var menu = new ConsoleMenu(accountMenuItems);
        menu.Run();
    }
    public void ShowAccountTypes()
    {
        var menu = new ConsoleMenu(new[]
        {
            new MenuItem("Bankkonto", () =>
            {
                var (name, balance) = GetAccountDetails();
                accounts.Add(new BankAccount(name, balance));
                Console.WriteLine("Bankkonto skapat. Tryck Enter för att fortsätta...");
                Console.ReadLine();
                throw new OperationCanceledException();
            }),
            new MenuItem("ISK-konto", () =>
            {
                var (name, balance) = GetAccountDetails();
                accounts.Add(new IskAccount(name, balance));
                Console.WriteLine("ISK-konto skapat. Tryck Enter för att fortsätta...");
                Console.ReadLine();
                throw new OperationCanceledException();
            }),
            new MenuItem("Uddevalla-konto", () =>
            {
                var (name, balance) = GetAccountDetails();
                accounts.Add(new UddevallaAccount(name, balance));
                Console.WriteLine("Uddevalla-konto skapat. Tryck Enter för att fortsätta...");
                Console.ReadLine();
                throw new OperationCanceledException();
            }),
            new MenuItem("Tillbaka", () =>
            {
                throw new OperationCanceledException();
            }),
        });

        menu.Run();
    }

    private (string name, decimal balance) GetAccountDetails()
    {
        Console.Clear();
        //ShowAccountTypes();
        //var accountType = Console.ReadLine();
        Console.WriteLine("Ange kontonamn:");
        var name = Console.ReadLine();
        Console.WriteLine("Ange startbelopp:");
        var balance = decimal.Parse(Console.ReadLine());
        // Logik för att skapa konto baserat på vald kontotyp, kontonamn och startbelopp
        //accounts.Add(new BankAccount(accountName, startingBalance));
        return (name, balance);
    }

    public void RemoveAccount()
    {
        Console.Clear();

        if (accounts.Count == 0)
        {
            Console.WriteLine("Du har inga konton registrerade.");
            Console.WriteLine("Tryck Enter för att fortsätta...");
            Console.ReadLine();
            return;
        }

        bool removedAny;
        do
        {
            removedAny = false;

            var menuItems = new List<MenuItem>();

            // Skapa ett menyalternativ per konto. Kopiera lokala referensen så closure fungerar korrekt.
            for (int i = 0; i < accounts.Count; i++)
            {
                var acc = accounts[i];
                menuItems.Add(new MenuItem($"{acc.AccountName} ({acc.AccountNumber})", () =>
                {
                    // Bekräftelsemeny med rubrik (icke markerbar) + Ja/Nej
                    var confirmMenu = new ConsoleMenu(new[]
                    {
                        new MenuItem($"Vill du ta bort kontot '{acc.AccountName}' ({acc.AccountNumber})?"),
                        new MenuItem("Ja", () =>
                        {
                            accounts.Remove(acc);
                            Console.Clear();
                            Console.WriteLine($"Kontot {acc.AccountNumber} har tagits bort.");
                            Console.WriteLine("Tryck Enter för att fortsätta...");
                            Console.ReadLine();

                            // Sätt flaggan så yttre loop vet att listan ändrats
                            removedAny = true;

                            // Avsluta confirm-menyn
                            throw new OperationCanceledException();
                        }),
                        new MenuItem("Nej", () =>
                        {
                            Console.Clear();
                            Console.WriteLine("Åtgärd avbröts.");
                            Console.WriteLine("Tryck Enter för att fortsätta...");
                            Console.ReadLine();
                            throw new OperationCanceledException();
                        }),
                    });

                    confirmMenu.Run();

                    // Om ett konto togs bort, avsluta den yttre menyn så den kan byggas upp på nytt
                    if (removedAny)
                        throw new OperationCanceledException();
                }));
            }

            // Tillbaka-alternativ
            menuItems.Add(new MenuItem("Tillbaka", () =>
            {
                throw new OperationCanceledException();
            }));

            var menu = new ConsoleMenu(menuItems);
            menu.Run();

            // Om ett konto tog bort, loopen körs igen och menyn byggs upp från uppdaterad 'accounts'-lista.
        } while (removedAny);
    }
    public void ShowAccounts()
    {
        Console.Clear();
        Console.WriteLine("Dina konton:");
        if (accounts.Count == 0)
        {
            Console.WriteLine("Du har inga konton registrerade.");
        }
        foreach (var account in accounts)
        {
            Console.WriteLine($"- Konto: {account.AccountType}" +
                $"\n- Kontonamn: {account.AccountName} " +
                $"\n- Kontonr: {account.AccountNumber}" +
                $"\n- CCV: {account.CCV} " +
                $"\n- Saldo: {account.Balance()}kr" +
                $"\n- Räntesats: {account.InterestRate}%");
            Console.WriteLine("***************************************");
        }
        Console.ReadLine();
    }

    public void Deposit()
    {
        Console.Clear();
        Console.WriteLine("--- INSÄTTNING ---");

        if (accounts.Count == 0)
        {
            Console.WriteLine("Du har inga konton registrerade.");
            Console.ReadKey();
            return;
        }

        // 1. Fråga efter kontonummer
        Console.WriteLine("Ange kontonummer eller kontonamn att sätta in till:");
        var inputNumber = Console.ReadLine().Trim();

        // 2. Hitta rätt konto
        var account = accounts.FirstOrDefault(a => a.AccountNumber == inputNumber || a.AccountName == inputNumber);

        if (account == null)
        {
            Console.WriteLine("Kunde inte hitta kontot.");
            Console.ReadKey();
            return;
        }

        // 3. Fråga efter belopp
        Console.WriteLine($"Hur mycket vill du sätta in på {account.AccountName}?");
        if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
        {
            Console.WriteLine("Ogiltigt belopp.");
            Console.ReadKey();
            return;
        }
        // Anropar jag metoden från AccountBase klassen
        account.Deposit(amount);

        Console.WriteLine($"Insättning på {amount} kr genomfört.");
        Console.ReadKey();
    }


    public void Withdraw()
    {
        Console.Clear();
        Console.WriteLine("--- UTTAG ---");

        if (accounts.Count == 0)
        {
            Console.WriteLine("Du har inga konton registrerade.");
            Console.ReadKey();
            return;
        }

        // 1. Fråga efter kontonummer
        Console.WriteLine("Ange kontonummer eller kontonamn att ta ut från:");
        var inputNumber = Console.ReadLine().Trim();

        // 2. Hitta rätt konto
        var account = accounts.FirstOrDefault(a => a.AccountNumber == inputNumber || a.AccountName == inputNumber);

        if (account == null)
        {
            Console.WriteLine("Kunde inte hitta kontot.");
            Console.ReadKey();
            return;
        }

        // 3. Fråga efter belopp
        Console.WriteLine($"Hur mycket vill du ta ut från {account.AccountName}?");
        if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
        {
            Console.WriteLine("Ogiltigt belopp.");
            Console.ReadKey();
            return;
        }

        // 4. Kontrollera om det finns täckning
        if (account.Balance() < amount)
        {
            Console.WriteLine("Inte tillräckligt med pengar på kontot.");
            Console.ReadKey();
            return;
        }
        account.Withdraw(amount);

        Console.WriteLine($"Uttag på {amount} kr genomfört.");
        Console.ReadKey();
    }
}