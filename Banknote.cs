using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM
{
    class Banknote
    {
        public int nominal
        {
            get;
            set;
        }

        public Banknote(int Nominal)
        {
            nominal = Nominal;
        }
    }
}
