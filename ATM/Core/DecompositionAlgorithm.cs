using System.Collections.Generic;

namespace ATM.Core
{
    public class DecompositionAlgorithm
    {
        public Money Decompose(decimal requestedSum, ref decimal currentSum, ref Money money)
        {
            var outputedMoney = new Money();
            if (requestedSum <= currentSum)
            {
                var tempMoney = new Dictionary<Banknote, int>(money.Banknotes);
                foreach (var item in tempMoney)
                {
                    while (requestedSum >= item.Key.Nominal && money.Banknotes[item.Key] != 0)
                    {
                        requestedSum -= item.Key.Nominal;
                        currentSum -= item.Key.Nominal;
                        if (!outputedMoney.Banknotes.ContainsKey(item.Key))
                        {
                            outputedMoney.Banknotes.Add(item.Key, 0);
                            outputedMoney.Banknotes[item.Key]++;
                        }
                        else
                        {
                            outputedMoney.Banknotes[item.Key]++;
                        }

                        if (item.Value != 0)
                        {
                            money.Banknotes[item.Key]--;
                        }
                    }
                }
            }

            return outputedMoney;
        }
    }
}