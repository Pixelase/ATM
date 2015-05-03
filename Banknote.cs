namespace ATM
{
    internal class Banknote
    {
        public Banknote(int nominal)
        {
            Nominal = nominal;
        }

        public int Nominal { get; set; }
    }
}