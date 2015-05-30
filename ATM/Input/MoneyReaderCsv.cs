using System;
using System.IO;
using System.Text.RegularExpressions;
using ATM.Core;
using log4net;

namespace ATM.Input
{
    internal class MoneyReaderCsv : IMoneyReader
    {
        public static readonly ILog Log = LogManager.GetLogger(typeof(MoneyReaderCsv));
        private readonly string _path;

        public MoneyReaderCsv(string path)
        {
            _path = path;
        }

        public Money ReadMoney()
        {
            var money = new Money();
            var stream = new StreamReader(new FileStream(_path, FileMode.Open));

            stream.ReadLine();
            var str = stream.ReadLine();
            if (str != null)
            {
                var strs = str.Split(',');
                try
                {
                    foreach (var item in strs)
                    {
                        var temp = Regex.Matches(item, @"\d+");
                        var number = int.Parse(temp[1].ToString());
                        var nominal = int.Parse(temp[0].ToString());
                        money.Add(nominal, number);
                    }
                }
                catch (Exception e)
                {
                    Log.Error("Parse error: " + e.Message);
                }
            }
            stream.Close();
            return money;
        }
    }
}