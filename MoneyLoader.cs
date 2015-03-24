using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ATM
{
    class MoneyLoader
    {
        private string _path;

        public MoneyLoader(string path)
        {
            _path = path;
        }

        public Money LoadMoney()
        {
            Money money = new Money();
            if (File.Exists(_path))
            {
                StreamReader sr = new StreamReader(_path);
                try
                {
                    while (!sr.EndOfStream)
                    {
                        string[] temp = sr.ReadLine().Split(' ');
                        int banknoteNomimal = int.Parse(temp[0]);
                        int banknotesCount = int.Parse(temp[1]);
                        money.Add(banknoteNomimal, banknotesCount);
                    }
                }

                catch (FileLoadException)
                {
                    Console.WriteLine("Невозможно считать кассету с деньгами:\n");
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Что-то пошло не так:\n" + ex.Message);
                }

                finally
                {
                    sr.Close();
                }
            }
            
            return money;
        }
    }
}
