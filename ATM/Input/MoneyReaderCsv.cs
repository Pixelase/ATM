using System;
using ServiceStack.Text;
using System.IO;
using ATM.Core;

namespace ATM.Input
{
    internal class MoneyReaderCsv : IMoneyReader
    {
        private readonly string _path;

        public MoneyReaderCsv(string path)
        {
            _path = path;
        }

        public Money ReadMoney()
        {
            //Не сейчас
            throw new NotImplementedException();

            //var stream = new FileStream(_path, FileMode.Open);
            //Money money = null;
            //try
            //{
            //    money = CsvSerializer.DeserializeFromStream<Money>(stream);
            //}
            //catch (Exception)
            //{

            //}
            //return money;
            

        }
    }
}
