using System;
using System.IO;
using ATM.Core;
using log4net;

namespace ATM.Input
{
    public class MoneyReaderTxt : IMoneyReader
    {
        public static readonly ILog Log = LogManager.GetLogger(typeof (MoneyReaderTxt));
        private readonly string _path;

        public MoneyReaderTxt(string path)
        {
            _path = path;
        }

        public Money ReadMoney()
        {
            var money = new Money();
            using (var sr = new StreamReader(_path))
            {
                while (!sr.EndOfStream)
                {
                    var readLine = sr.ReadLine();
                    if (readLine != null)
                    {
                        try
                        {
                            var temp = readLine.Split(' ');
                            var banknoteNomimal = int.Parse(temp[0]);
                            var banknotesCount = int.Parse(temp[1]);
                            money.Add(banknoteNomimal, banknotesCount);
                        }
                        catch (Exception e)
                        {
                            Log.Error("Parse error: " + e.Message);
                        }
                    }
                }
            }

            return money;
        }
    }
}