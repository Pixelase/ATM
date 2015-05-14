using System;
using System.IO;

namespace ATM
{
    internal class MoneyReaderTxt: IMoneyReader
    {
        private readonly string _path;

        public MoneyReaderTxt(string path)
        {
            _path = path;
        }

        public Money ReadMoney()
        {
            var money = new Money();
            if (File.Exists(_path))
            {
                var sr = new StreamReader(_path);
                try
                {
                    while (!sr.EndOfStream)
                    {
                        var readLine = sr.ReadLine();
                        if (readLine != null)
                        {
                            var temp = readLine.Split(' ');
                            var banknoteNomimal = int.Parse(temp[0]);
                            var banknotesCount = int.Parse(temp[1]);
                            money.Add(banknoteNomimal, banknotesCount);
                        }
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
