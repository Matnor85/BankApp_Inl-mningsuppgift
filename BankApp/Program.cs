 using BankApp.Accounts;
using BankApp.Base;

namespace BankApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Nyrad(10);
            //Console.ReadLine();
            // För att säkerställa att konsolen kan visa alla tecken korrekt, inklusive eventuella special tecken
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            // Dölj markören för att förbättra användarupplevelsen i menyn
            Console.CursorVisible = false;

            Bank bank = new Bank();

            // Seeds för testdata
            var gen = new DataGenerator();
            gen.PopulateWithTestData(bank);

            while (true)
            {
                bank.ShowMenu();
            }
        }
        static void Nyrad(int x)
        {
            for (int i = 0; i < x; i++)
            {
                Console.WriteLine("Ny");
            }
        }
    }
}
