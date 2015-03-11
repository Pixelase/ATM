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

        public CashMachine(string path)
        {
            _money = new Money();
            _path = path;
            StreamReader sr = new StreamReader(_path);
            while (!sr.EndOfStream)
            {
                try
                {
                    string[] temp = sr.ReadLine().Split(' ');
                    int banknoteNomimal = int.Parse(temp[0]);
                    int banknotesCount = int.Parse(temp[1]);
                    _money.Add(banknoteNomimal, banknotesCount);
                    _sum += banknoteNomimal * banknotesCount;
                }
                catch (Exception)
                {
                    Console.WriteLine("Что-то пошло не так\n");
                }
            }
            sr.Close();
        }

        public Money GetMoney(int userSum)
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

            }

            using (StreamWriter sw = new StreamWriter(_path))
            {
                foreach (KeyValuePair<Banknote, int> item in this._money.Banknotes)
                {
                    sw.WriteLine("{0} {1}", item.Key.nominal, item.Value);
                }
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
