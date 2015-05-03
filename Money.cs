using System.Collections.Generic;

namespace ATM
{
    internal class Money
    {
        public Dictionary<Banknote, int> Banknotes { get; set; }

        public Money()
        {
            Banknotes = new Dictionary<Banknote, int>();
        }

        public void Add(int banknoteNomimal, int banknotesCount)
        {
            Banknotes.Add(new Banknote(banknoteNomimal), banknotesCount);
        }
    }
}
