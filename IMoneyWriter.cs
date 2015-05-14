using System.Security.Cryptography.X509Certificates;

namespace ATM
{
    public interface IMoneyWriter
    {
        void WriteMoney(Money money);
    }
}
