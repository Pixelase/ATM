using System.Security.Cryptography.X509Certificates;

namespace ATM
{
    interface IMoneyWriter
    {
        void WriteMoney(Money money);
    }
}
