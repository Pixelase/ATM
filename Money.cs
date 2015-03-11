using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM
{
    class Money
    {
        public Dictionary<Banknote, int> Banknotes
        {
            get;
            set;
        }

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
