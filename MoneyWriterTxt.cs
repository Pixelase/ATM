using System;
using System.Collections.Generic;
using System.IO;

namespace ATM
{
    class MoneyWriterTxt: IMoneyWriter
    {
        private readonly string _path;

        public MoneyWriterTxt(string path)
        {
            _path = path;
        }

        public void WriteMoney(Money money)
        {
            if (File.Exists(_path))
            {
                StreamWriter sw = new StreamWriter(_path);
                try
                {
                    foreach (KeyValuePair<Banknote, int> item in money.Banknotes)
                    {
                        sw.WriteLine("{0} {1}", item.Key.Nominal, item.Value);
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Что-то пошло не так:\n" + ex.Message);
                }

                finally
                {
                    sw.Close();
                }

            }
        }
    }
}
