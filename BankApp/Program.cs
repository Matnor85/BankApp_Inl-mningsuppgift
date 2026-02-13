using BankApp.Accounts;
using BankApp.Base;

namespace BankApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
            Bank bank = new Bank();
            while (true)
            {
                bank.ShowMenu();


                //var list = new List<AccountBase>();

                //Console.WriteLine("Hello, World!");
                //var b = new BankAccount();
                //var s = new IskAccount();
                //var u = new UddevallaAccount();


                //list.Add(b);
                //list.Add(s);
                //list.Add(u);
                // foreach (var item in list)
                // {
                //    item.Withdraw(1000);
                //     Console.WriteLine(item.Balance());
                //}
                //Console.WriteLine(Guid.NewGuid());
            }
        }
    }
}
