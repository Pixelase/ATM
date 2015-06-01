using System;

namespace Statistics
{
    /// <summary>
    /// Аргументы события выдачи денег
    /// </summary>
    public class MoneyWithdrawnEventArgs : EventArgs
    {
        public DateTime WithdrawnTime { get; set; }
        public decimal Balance { get; set; }
        public decimal UserSum { get; set; }
    }
}
