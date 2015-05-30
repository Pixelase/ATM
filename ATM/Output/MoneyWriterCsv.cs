using System.IO;
using ATM.Core;
using ServiceStack.Text;

namespace ATM.Output
{
    class MoneyWriterCsv: IMoneyWriter
    {
        private readonly string _path;

        public MoneyWriterCsv(string path)
        {
            _path = path;
        }

        public void WriteMoney(Money money)
        {
            //Stream stream = new FileStream(_path, FileMode.Create);
            StreamWriter stream = new StreamWriter(_path);
            //CsvSerializer.SerializeToStream(money, stream);
            CsvSerializer<Money>.WriteObject(stream, money);
            stream.Close();
        }
    }
}
