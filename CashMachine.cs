using System.Collections.Generic;
using System.Text;

namespace ATM
{
    internal class CashMachine
    {
        private readonly Money _money;
        private MoneyLoader _moneyLoader;
        private readonly string _path;
        private MoneyUploader _moneyUploader;

        public CashMachine(string path)
        {
            _path = path;
            _moneyLoader = new MoneyLoader(path);
            _money = _moneyLoader.LoadMoney();
            foreach (var item in _money.Banknotes)
            {
                var banknoteNomimal = item.Key.Nominal;
                var banknotesCount = item.Value;
                Sum += banknoteNomimal * banknotesCount;
            }
        }

        public int Sum { get; private set; }

        public Money WithdrawMoney(int userSum)
        {
            var temp = new Money();
            if (userSum <= Sum)
            {
                var tempMoney = new Dictionary<Banknote, int>(_money.Banknotes);
                foreach (var item in tempMoney)
                {
                    while (userSum >= item.Key.Nominal && _money.Banknotes[item.Key] != 0)
                    {
                        userSum -= item.Key.Nominal;
                        Sum -= item.Key.Nominal;
                        if (!temp.Banknotes.ContainsKey(item.Key))
                        {
                            temp.Banknotes.Add(item.Key, 0);
                            temp.Banknotes[item.Key]++;
                        }
                        else
                        {
                            temp.Banknotes[item.Key]++;
                        }

                        if (item.Value != 0)
                        {
                            _money.Banknotes[item.Key]--;
                        }
                    }
                }

                //Записывает текущее состояние денег в файл
                _moneyUploader = new MoneyUploader(_path);
                _moneyUploader.UploadMoney(_money);
            }

            return temp;
        }

        public string Status()
        {
            var temp = new StringBuilder();
            foreach (var item in _money.Banknotes)
            {
                temp.Append("Купюра:" + item.Key.Nominal + " <-> Количество: " + item.Value + '\n');
            }
            temp.Append("Остаток: " + Sum);
            return temp.ToString();
        }
    }
}