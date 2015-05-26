using System;
using System.Runtime.Serialization;

namespace ATM.Core
{
    [DataContract]
    public class Banknote: IComparable<Banknote>
    {
        public Banknote(int nominal)
        {
            Nominal = nominal;
        }

        [DataMember]
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