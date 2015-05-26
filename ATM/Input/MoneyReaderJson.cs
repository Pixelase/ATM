using System.IO;
using System.Runtime.Serialization.Json;
using ATM.Core;

namespace ATM.Input
{
    public class MoneyReaderJson : IMoneyReader
    {
        private readonly string _path;

        public MoneyReaderJson(string path)
        {
            _path = path;
        }

        public Money ReadMoney()
        {
            Stream stream = new FileStream(_path, FileMode.Open);
            var ds = new DataContractJsonSerializer(typeof (Money));
            var money = (Money) ds.ReadObject(stream);
            stream.Close();
            return money;
        }
    }
}