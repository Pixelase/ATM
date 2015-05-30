using System;
using ATM;
using ATM.Language;
using log4net;
using log4net.Config;

namespace Console_User_Interface
{

    internal class Program
    {
        public static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        private static void Main(string[] args)
        {
            var path = @"D:\Visual Studio\OOP\ATM\bin\Debug\data.csv";
            var cashMachine = new CashMachine(path);
            Log.Debug("CashMashine started");

            var lang = new LanguageConfig("en-US");
            XmlConfigurator.Configure();

            Console.WriteLine(lang.Status + ":");
            Console.WriteLine(cashMachine.Status() + '\n');

            while (cashMachine.Sum != 0)
            {
                Console.Write(lang.AskForMoney + ": ");

                string request = Console.ReadLine();
                Log.Debug("Users request: " + request);

                if (request != null && request.ToLower() == lang.Exit)
                {
                    Log.Debug("CashMashine finished");
                    return;
                }
                
                decimal userMoney;
                decimal.TryParse(request, out userMoney);
                while (userMoney > cashMachine.Sum || userMoney <= 0)
                {
                    if (userMoney > cashMachine.Sum || userMoney <= 0)
                    {
                        if (userMoney > cashMachine.Sum)
                        {
                            Console.Write(lang.NotEnoughMoney + "\n\n" + lang.AskForMoney + ": ");
                            Log.Error("Not enough money");
                        }
                        else
                        {
                            Console.Write(lang.IncorrectInput + "\n\n" + lang.AskForMoney + ": ");
                            Log.Error("Incorrect input");
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
