using System;

namespace ATM
{
    public class Banknote: IComparable<Banknote>
    {
        public Banknote(int nominal)
        {
            Nominal = nominal;
        }

        public int Nominal { get; set; }

        public int CompareTo(Banknote other)
        {
            if (other == null) return 0;
            if (Nominal > other.Nominal)
                return 1;
            if (Nominal < other.Nominal)
                return -1;
            return 0;
        }
    }
}