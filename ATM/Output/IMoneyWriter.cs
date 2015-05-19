using ATM.Core;

namespace ATM.Output
{
    public interface IMoneyWriter
    {
        void WriteMoney(Money money);
    }
}
