using System;
using ATM;
using ATM.Language;
using log4net;
using log4net.Config;
using Statistics;

namespace Console_User_Interface
{
    internal class Program
    {
        //Ведение логирования
        public static readonly ILog Log = LogManager.GetLogger(typeof (Program));

        private static void Main(string[] args)
        {
            //Путь по умолчанию к файлу с кассетами
            var path = @"D:\Visual Studio\OOP\ATM\bin\Debug\data.json";
            var cashMachine = new CashMachine(path);
            var lang = new LanguageConfig("en-US");
            var statsCounter = new StatsCounter();
            var commandPerformer = new CommandPerformer(cashMachine, lang, statsCounter);


            XmlConfigurator.Configure();

            //Вывод текущего состояния счёта
            Console.WriteLine(lang.Status + ":");
            Console.WriteLine(cashMachine.Status() + '\n');

            //Начало обработки пользовательского ввода
            while (true)
            {
                Console.Write(lang.AskForMoney + ": ");

                //Пользовательский запрос
                var request = Console.ReadLine();
                Log.Debug("Users request: " + request);

                //Введённая пользователем сумма
                decimal usersMoney = 0;

                //Выполнение команды && проверка, команда ли это
                var isCommand = request != null && commandPerformer.TryPerform(request.Trim().ToLower());
                if (isCommand || decimal.TryParse(request, out usersMoney))
                {
                    //Начало работы с введённой пользователем суммой
                    if (!isCommand && usersMoney < cashMachine.Balance && usersMoney >= 0)
                    {
                        //Выданная сумма
                        decimal withdrawnSum = 0;

                        //Вывод && подсчёт выданной суммы
                        Console.WriteLine('\n' + lang.YourMoney + ":");
                        foreach (var item in cashMachine.WithdrawMoney(usersMoney).Banknotes)
                        {
                            var banknoteNomimal = item.Key.Nominal;
                            var banknotesCount = item.Value;
                            withdrawnSum += banknoteNomimal*banknotesCount;
                            Console.WriteLine(lang.Banknote + ":" + item.Key.Nominal + " <-> " + lang.Number + ": " +
                                              item.Value);
                        }
                        Console.WriteLine(lang.WithdrawnSum + ": " + withdrawnSum);

                        //Вызов события добавления записи статистики
                        statsCounter.Add(cashMachine.Balance, withdrawnSum);

                        //Вывод текущего состояния счёта
                        Console.WriteLine('\n' + lang.Status + ':');
                        Console.WriteLine(cashMachine.Status() + '\n');
                    }
                    else if (usersMoney > cashMachine.Balance)
                    {
                        Console.Write('\n' + lang.NotEnoughMoney + "\n\n");
                        Log.Error("Not enough money");
                    }
                }
                //Обработка некорректного ввода
                else
                {
                    Console.Write('\n' + lang.IncorrectInput + "\n\n");
                    Log.Error("Incorrect input");
                }
            }
        }
    }
}