using System.Text;
using ATM.Core;
using ATM.Input;
using ATM.Language;
using ATM.Output;
using log4net.Config;

namespace ATM
{
    /// <summary>
    /// Библиотека - эмулятор банкомата
    /// </summary>
    public class CashMachine
    {
        public decimal Sum
        {
            get { return _sum; }
            private set { _sum = value; }
        }

        private Money _money;
        private decimal _sum;
        private readonly string _path;

        public CashMachine(string path)
        {
            XmlConfigurator.Configure();
            _path = path;
            var moneyReader = new MoneyReaderTxt(path);
            _money = moneyReader.ReadMoney();
            foreach (var item in _money.Banknotes)
            {
                var banknoteNomimal = item.Key.Nominal;
                var banknotesCount = item.Value;
                Sum += banknoteNomimal*banknotesCount;
            }
        }

        public Money WithdrawMoney(decimal requestedSum)
        {
            var moneyWriter = new MoneyWriterTxt(_path);
            var decompositionAlgorithm = new DecompositionAlgorithm();
            var outputedMoney = decompositionAlgorithm.Decompose(requestedSum, ref _sum, ref _money);
            moneyWriter.WriteMoney(_money);
            return outputedMoney;
        }

        public string Status()
        {
            var lang = new LanguageConfig("en-US");
            var temp = new StringBuilder();

            foreach (var item in _money.Banknotes)
            {
                temp.Append(lang.Banknote  + ": " + item.Key.Nominal + " <-> " + lang.Number + ": " + item.Value + '\n');
            }

            temp.Append(lang.Sum + ": " + _sum);
            return temp.ToString();
        }
    }
}