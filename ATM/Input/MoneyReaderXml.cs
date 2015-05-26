using System.IO;
using System.Runtime.Serialization;
using ATM.Core;

namespace ATM.Input
{
    public class MoneyReaderXml: IMoneyReader
    {
        private readonly string _path;

        public MoneyReaderXml(string path)
        {
            _path = path;
        }

        public Money ReadMoney()
        {
            Stream stream = new FileStream(_path, FileMode.Open);
            var ds = new DataContractSerializer(typeof(Money));
            return (Money)ds.ReadObject(stream);
        }
    }
}
