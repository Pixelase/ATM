using System;
using ATM;
using ATM.Language;

namespace Console_User_Interface
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var path = @"D:\Visual Studio\OOP\ATM\bin\Debug\data.txt";
            var cashMachine = new CashMachine(path);

            var lang = new LanguageConfig("en-US");

            Console.WriteLine(lang.Status + ":");
            Console.WriteLine(cashMachine.Status() + '\n');

            while (cashMachine.Sum != 0)
            {
                Console.Write(lang.AskForMoney + ": ");
                decimal userMoney;
                decimal.TryParse(Console.ReadLine(), out userMoney);
                while (userMoney > cashMachine.Sum || userMoney <= 0)
                {
                    if (userMoney > cashMachine.Sum || userMoney <= 0)
                    {
                        if (userMoney > cashMachine.Sum)
                        {
                            Console.Write(lang.NotEnoughMoney + "\n\n" + lang.AskForMoney + ": ");
                        }
                        else
                        {
                            Console.Write(lang.IncorrectInput + "\n\n" + lang.AskForMoney + ": ");
                        }
                        decimal.TryParse(Console.ReadLine(), out userMoney);
                    }
                }

                Console.WriteLine('\n' + lang.YourMoney + ":");
                foreach (var item in cashMachine.WithdrawMoney(userMoney).Banknotes)
                {
                    Console.WriteLine(lang.Banknote + ":" + item.Key.Nominal + " <-> " + lang.Number + ": " + item.Value);
                }

                Console.WriteLine('\n' + lang.Status + ':');
                Console.WriteLine(cashMachine.Status() + '\n');
            }

            Console.ReadKey();
        }
    }
}
