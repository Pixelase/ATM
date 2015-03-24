using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ATM
{
    class MoneyUploader
    {
        private string _path;

        public MoneyUploader(string path)
        {
            _path = path;
        }

        public void UploadMoney(Money money)
        {
            if (File.Exists(_path))
            {
                StreamWriter sw = new StreamWriter(_path);
                try
                {
                    foreach (KeyValuePair<Banknote, int> item in money.Banknotes)
                    {
                        sw.WriteLine("{0} {1}", item.Key.nominal, item.Value);
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
