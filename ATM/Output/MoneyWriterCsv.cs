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
            CsvSerializer.SerializeToStream(money, new FileStream(_path, FileMode.OpenOrCreate));
        }
    }
}
