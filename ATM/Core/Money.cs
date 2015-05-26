using System;
using System.Collections.Generic;

namespace ATM.Core
{   
    public class Money
    {
        public SortedList<Banknote, int> Banknotes { get; set; }

        public Money()
        {
            Banknotes = new SortedList<Banknote, int>(new DescendingComparer<Banknote>());
        }

        public void Add(int banknoteNomimal, int banknotesCount)
        {
            Banknotes.Add(new Banknote(banknoteNomimal), banknotesCount);
        }
    }
}
