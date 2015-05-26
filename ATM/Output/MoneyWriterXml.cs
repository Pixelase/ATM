using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using ATM.Core;

namespace ATM.Output
{
    public class MoneyWriterXml : IMoneyWriter
    {
        private readonly string _path;

        public MoneyWriterXml(string path)
        {
            _path = path;
        }

        public void WriteMoney(Money money)
        {

            Stream stream = new FileStream(_path, FileMode.OpenOrCreate);
            DataContractSerializer ds = new DataContractSerializer(typeof(Money), new DataContractSerializerSettings());

            ds.WriteObject(stream, money);
            stream.Close();

        }
    }
}