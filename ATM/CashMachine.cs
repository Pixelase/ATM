using System;
using System.IO;
using System.Linq;
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
        public static readonly ILog Log = LogManager.GetLogger(typeof (CashMachine));

        public decimal Sum
        {
            get { return _sum; }
            private set { _sum = value; }
        }

        private Money _money;
        private decimal _sum;
        private string _path;
        private IMoneyReader _moneyReader;
        private IMoneyWriter _moneyWriter;

        /// <summary>
        ///     Конструктор CashMachine
        /// </summary>
        /// <param name="path">Путь к файлу с кассетами</param>
        public CashMachine(string path)
        {
            XmlConfigurator.Configure();
            _path = path;
            IncertCassettes(path);
        }

        /// <summary>
        ///     Функция выдачи денег
        /// </summary>
        /// <param name="requestedSum">Требуемая сумма</param>
        /// <returns>Money</returns>
        public Money WithdrawMoney(decimal requestedSum)
        {
            Log.Debug("Withdraw operation running");

            var decompositionAlgorithm = new DecompositionAlgorithm();
            var outputedMoney = decompositionAlgorithm.Decompose(requestedSum, ref _sum, ref _money);

            try
            {
                _moneyWriter.WriteMoney(_money);
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

        /// <summary>
        ///     Функция, которая вставляет кассеты в банкомат
        /// </summary>
        /// <param name="path">Путь к файлу с кассетами</param>
        public void IncertCassettes(string path)
        {
            _path = path;
            DetectFileFormat();
            try
            {
                _money = _moneyReader.ReadMoney();

                foreach (var item in _money.Banknotes)
                {
                    var banknoteNomimal = item.Key.Nominal;
                    var banknotesCount = item.Value;
                    Sum += banknoteNomimal*banknotesCount;
                }
                Log.Debug("Сassettes have been successfully inserted");
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
        ///     Функция для изъятия кассет из банкомата
        /// </summary>
        public void RemoveCassettes()
        {
            _money.Banknotes.Clear();
            _sum = 0;
            _path = string.Empty;
            Log.Debug("Сassettes have been successfully removed");
        }

        private void DetectFileFormat()
        {
            var format = _path.Split('.').Last();
            format = format.ToLower();
            switch (format)
            {
                case "json":
                {
                    _moneyReader = new MoneyReaderJson(_path);
                    _moneyWriter = new MoneyWriterJson(_path);
                    break;
                }
                case "txt":
                {
                    _moneyReader = new MoneyReaderTxt(_path);
                    _moneyWriter = new MoneyWriterTxt(_path);
                    break;
                }
                case "csv":
                {
                    _moneyReader = new MoneyReaderCsv(_path);
                    _moneyWriter = new MoneyWriterCsv(_path);
                    break;
                }
                case "xml":
                {
                    _moneyReader = new MoneyReaderXml(_path);
                    _moneyWriter = new MoneyWriterXml(_path);
                    break;
                }
                default:
                {
                    Log.Error("Unsupported file format");
                    break;
                }
                    
            }
        }
    }
}