using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ATM
{
    class CashMachine
    {
        private int _sum;
        private string _path;
        public int Sum
        {
            get
            {
                return _sum;
            }
            private set
            {
                _sum = value;
            }
        }
        private Money _money;
        private MoneyLoader _moneyLoader;
        private MoneyUploader _moneyUploader;

        public CashMachine(string path)
        {
            _path = path;
            _moneyLoader = new MoneyLoader(path);
            _money = _moneyLoader.LoadMoney();
            foreach (KeyValuePair<Banknote, int> item in _money.Banknotes)
            {
                int banknoteNomimal = item.Key.nominal;
                int banknotesCount = item.Value;
                _sum += banknoteNomimal * banknotesCount;
            }

        }

        public Money WithdrawMoney(int userSum)
        {
            Money temp = new Money();
            if (userSum <= _sum)
            {
                Dictionary<Banknote, int> tempMoney = new Dictionary<Banknote, int>(_money.Banknotes);
                foreach (KeyValuePair<Banknote, int> item in tempMoney)
                {
                    while (userSum >= item.Key.nominal && _money.Banknotes[item.Key] != 0)
                    {
                        userSum -= item.Key.nominal;
                        _sum -= item.Key.nominal;
                        if (!temp.Banknotes.ContainsKey(item.Key))
                        {
                            temp.Banknotes.Add(item.Key, 0);
                            temp.Banknotes[item.Key]++;
                        }
                        else
                        {
                            temp.Banknotes[item.Key]++;
                        }

                        if (item.Value != 0) _money.Banknotes[item.Key]--;
                    }
                }

                //Записывает текущее состояние денег в файл
               this._moneyUploader = new MoneyUploader(_path);
               this._moneyUploader.UploadMoney(this._money);

            }

            return temp;
        }

        public string Status()
        {
            StringBuilder temp = new StringBuilder();
            foreach (KeyValuePair<Banknote, int> item in this._money.Banknotes)
            {
                temp.Append("Купюра:" + item.Key.nominal + " <-> Количество: " + item.Value + '\n');
            }
            temp.Append("Остаток: " + _sum);
            return temp.ToString();
        }
    }
}
