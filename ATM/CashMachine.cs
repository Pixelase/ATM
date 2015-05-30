using System;
using System.IO;
using System.Text;
using ATM.Core;
using ATM.Input;
using ATM.Language;
using ATM.Output;
using log4net;
using log4net.Config;

namespace ATM
{
    /// <summary>
    ///     Библиотека - эмулятор банкомата
    /// </summary>
    public class CashMachine
    {
        public static readonly ILog Log = LogManager.GetLogger(typeof(CashMachine));

        public decimal Sum
        {
            get { return _sum; }
            private set { _sum = value; }
        }

        private Money _money;
        private decimal _sum;
        private readonly string _path;

        /// <summary>
        ///     Конструктор CashMachine
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        public CashMachine(string path)
        {
            XmlConfigurator.Configure();
            _path = path;
            try
            {
                var moneyReader = new MoneyReaderTxt(path);
                _money = moneyReader.ReadMoney();

                foreach (var item in _money.Banknotes)
                {
                    var banknoteNomimal = item.Key.Nominal;
                    var banknotesCount = item.Value;
                    Sum += banknoteNomimal * banknotesCount;
                }
            }

            catch (FileLoadException ex)
            {
                Log.Error("Can't load money :" + ex);
            }

            catch (FileNotFoundException)
            {
                Log.Error("Cassette not found");
            }

            catch (Exception ex)
            {
                Log.Error("An unexpected error :" + ex);
            }
        }

        /// <summary>
        ///     Выдать деньги
        /// </summary>
        /// <param name="requestedSum">Требуемая сумма</param>
        /// <returns>Money</returns>
        public Money WithdrawMoney(decimal requestedSum)
        {
            Log.Debug("Withdraw operation running");

            var moneyWriter = new MoneyWriterTxt(_path);
            var decompositionAlgorithm = new DecompositionAlgorithm();
            var outputedMoney = decompositionAlgorithm.Decompose(requestedSum, ref _sum, ref _money);

            try
            {
                moneyWriter.WriteMoney(_money);
            }

            catch (Exception ex)
            {
                Log.Error("Could not update current money status:" + ex);
            }

            return outputedMoney;
        }

        /// <summary>
        ///     Функция, которая возвращает отчёт о состоянии счёта
        /// </summary>
        /// <returns>String</returns>
        public string Status()
        {
            Log.Debug("Status operation running");

            var lang = new LanguageConfig("en-US");
            var temp = new StringBuilder();

            if (_money != null && _money.Banknotes != null)
            {
                foreach (var item in _money.Banknotes)
                {
                    temp.Append(lang.Banknote + ": " + item.Key.Nominal + " <-> " + lang.Number + ": " + item.Value +
                                '\n');
                }
            }

            temp.Append(lang.Sum + ": " + _sum);
            return temp.ToString();
        }
    }
}