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
        private Money _money;
        private IMoneyReader _moneyReader;
        private IMoneyWriter _moneyWriter;
        private string _path;
        private decimal _balance;
        private string _currentCulture;

        /// <summary>
        ///     Конструктор CashMachine
        /// </summary>
        /// <param name="path">Путь к файлу с кассетами</param>
        public CashMachine(string path)
        {
            XmlConfigurator.Configure();
            _currentCulture = "en-US";
            Log.Debug("CashMashine session started");
            _path = path;
            TryInsertCassettes(path);
        }

        public decimal Balance
        {
            get { return _balance; }
            private set { _balance = value; }
        }

        public string CurrentCulture
        {
            get { return _currentCulture; }
            set { _currentCulture = value; }
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
            var outputedMoney = decompositionAlgorithm.Decompose(requestedSum, ref _balance, ref _money);

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

            var lang = new LanguageConfig(_currentCulture);
            var temp = new StringBuilder();

            if (_money != null && _money.Banknotes != null)
            {
                foreach (var item in _money.Banknotes)
                {
                    temp.Append(lang.Banknote + ": " + item.Key.Nominal + " <-> " + lang.Number + ": " + item.Value +
                                '\n');
                }
            }

            temp.Append(lang.Balance + ": " + _balance);
            return temp.ToString();
        }

        /// <summary>
        ///     Функция для вставки кассет в банкомат
        /// </summary>
        /// <param name="path">Путь к файлу с кассетами</param>
        public bool TryInsertCassettes(string path)
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
                    Balance += banknoteNomimal * banknotesCount;
                }
                Log.Debug("Сassettes have been successfully inserted");
                return true;

            }

            catch (FileLoadException ex)
            {
                Log.Error("Can't load money :" + ex);
            }

            catch (FileNotFoundException)
            {
                Log.Error("Cassette not found");
            }

            catch (NullReferenceException)
            {
                Log.Error("_moneyReader and _moneyWriter are not initialised");
            }

            catch (Exception ex)
            {
                Log.Error("An unexpected error :" + ex);
            }

            return false;
        }

        /// <summary>
        ///     Функция для изъятия кассет из банкомата
        /// </summary>
        public void RemoveCassettes()
        {
            _money.Banknotes.Clear();
            _balance = 0;
            _path = string.Empty;
            Log.Debug("Сassettes have been successfully removed");
        }

        /// <summary>
        ///     Функция, которая определяет формат файла с кассетами
        /// </summary>
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
                    Log.Debug("Casset format detected - json");
                    break;
                }
                case "txt":
                {
                    _moneyReader = new MoneyReaderTxt(_path);
                    _moneyWriter = new MoneyWriterTxt(_path);
                    Log.Debug("Casset format detected - txt");
                    break;
                }
                case "csv":
                {
                    _moneyReader = new MoneyReaderCsv(_path);
                    _moneyWriter = new MoneyWriterCsv(_path);
                    Log.Debug("Casset format detected - csv");
                    break;
                }
                case "xml":
                {
                    _moneyReader = new MoneyReaderXml(_path);
                    _moneyWriter = new MoneyWriterXml(_path);
                    Log.Debug("Casset format detected - xml");
                    break;
                }
                default:
                {
                    _moneyReader = null;
                    _moneyWriter = null;
                    Log.Error("Unsupported file format");
                    break;
                }
            }
        }

        /// <summary>
        ///     Выход
        /// </summary>
        public void Exit()
        {
            Log.Debug("CashMashine session finished");
            Environment.Exit(1);
        }
    }
}
