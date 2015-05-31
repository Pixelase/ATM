using System;
using ATM;
using ATM.Language;
using log4net;
using log4net.Config;

namespace Console_User_Interface
{
    internal class Program
    {
        public static readonly ILog Log = LogManager.GetLogger(typeof (Program));

        private static void Main(string[] args)
        {
            var path = @"D:\Visual Studio\OOP\ATM\bin\Debug\data.json";
            var cashMachine = new CashMachine(path);
            var lang = new LanguageConfig("en-US");
            var commandPerformer = new CommandPerformer(cashMachine, lang);

            XmlConfigurator.Configure();

            Console.WriteLine(lang.Status + ":");
            Console.WriteLine(cashMachine.Status() + '\n');

            while (true)
            {
                Console.Write(lang.AskForMoney + ": ");

                var request = Console.ReadLine();
                Log.Debug("Users request: " + request);

                decimal userMoney = 0;
                var isCommand = request != null && commandPerformer.TryPerform(request.Trim().ToLower());
                if (isCommand || decimal.TryParse(request, out userMoney))
                {
                    if (!isCommand && userMoney < cashMachine.Sum && userMoney >= 0)
                    {
                        Console.WriteLine('\n' + lang.YourMoney + ":");
                        foreach (var item in cashMachine.WithdrawMoney(userMoney).Banknotes)
                        {
                            Console.WriteLine(lang.Banknote + ":" + item.Key.Nominal + " <-> " + lang.Number + ": " +
                                              item.Value);
                        }

                        Console.WriteLine('\n' + lang.Status + ':');
                        Console.WriteLine(cashMachine.Status() + '\n');
                    }
                    else if (userMoney > cashMachine.Sum)
                    {
                        Console.Write('\n' + lang.NotEnoughMoney + "\n\n");
                        Log.Error("Not enough money");
                    }
                }
                else
                {
                    Console.Write('\n' + lang.IncorrectInput + "\n\n");
                    Log.Error("Incorrect input");
                }
            }
        }
    }
}
