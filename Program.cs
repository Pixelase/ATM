using System;

namespace ATM
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args == null) throw new ArgumentNullException("args");
            var atm = new CashMachine(@"data.txt");

            Console.WriteLine("Состояние счёта:");
            Console.WriteLine(atm.Status() + '\n');

            while (atm.Sum != 0)
            {
                Console.WriteLine("Введите нужную сумму:");
                int userMoney;
                int.TryParse(Console.ReadLine(), out userMoney);
                while (userMoney > atm.Sum || userMoney <= 0)
                {
                    if (userMoney > atm.Sum || userMoney <= 0)
                    {
                        Console.WriteLine("Некорректный ввод\n\n" + "Введите нужную сумму:");
                        int.TryParse(Console.ReadLine(), out userMoney);
                    }
                }

                Console.WriteLine("Ваши деньги:");
                foreach (var item in atm.WithdrawMoney(userMoney).Banknotes)
                {
                    Console.WriteLine("Купюра:" + item.Key.Nominal + " <-> Количество: " + item.Value);
                }

                Console.WriteLine("\nСостояние счёта:");
                Console.WriteLine(atm.Status() + '\n');
            }

            Console.ReadKey();
        }
    }
}