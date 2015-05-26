using System.Text;
using ATM.Core;
using ATM.Input;
using ATM.Output;
using log4net.Config;

namespace ATM
{
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
            var temp = new StringBuilder();

            foreach (var item in _money.Banknotes)
            {
                temp.Append("Купюра:" + item.Key.Nominal + " <-> Количество: " + item.Value + '\n');
            }

            temp.Append("Остаток: " + _sum);
            return temp.ToString();
        }
    }
}