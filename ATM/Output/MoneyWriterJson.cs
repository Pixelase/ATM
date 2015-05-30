using System.IO;
using System.Runtime.Serialization.Json;
using ATM.Core;

namespace ATM.Output
{
    public class MoneyWriterJson : IMoneyWriter
    {
        private readonly string _path;

        public MoneyWriterJson(string path)
        {
            _path = path;
        }

        public void WriteMoney(Money money)
        {
            Stream stream = new FileStream(_path, FileMode.Create);
            var ds = new DataContractJsonSerializer(typeof(Money), new DataContractJsonSerializerSettings());

            ds.WriteObject(stream, money);
            stream.Close();
        }
    }
}