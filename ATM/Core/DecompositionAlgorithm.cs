using System;
using System.Collections.Generic;
using log4net;

namespace ATM.Core
{
    /// <summary>
    /// Алгоритм разбиения суммы на различными купюрами
    /// </summary>
    public class DecompositionAlgorithm
    {
        /// <summary>
        /// Состояние алгоритма
        /// </summary>
        public AlgorithmStates State { get; set; }
        private static readonly ILog Log = LogManager.GetLogger(typeof(DecompositionAlgorithm));


        /// <summary>
        /// Возвращает ведённую пользователем сумму различными купюрами.
        /// </summary>
        /// <param name="requestedSum"></param>
        /// <param name="currentSum"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public Money Decompose(decimal requestedSum, ref decimal currentSum, ref Money money)
        {
            Log.Debug("Decomposition started");

            var outputedMoney = new Money();
            if (requestedSum <= currentSum)
            {
                State = AlgorithmStates.Ok;
                var tempMoney = new Dictionary<Banknote, int>(money.Banknotes);

                try
                {
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

                    Log.Debug("Decomposition is successfully finished");
                }

                catch (Exception ex)
                {
                    State = AlgorithmStates.Error;
                    Log.Error("Unexpected error: " + ex.Message);
                }
            }
            else
            {
                State = AlgorithmStates.NotEnoughMoney;
                Log.Debug("Not enough money in the CashMashine");
            }

            Log.Debug("Algorithm state is " + State.ToString() + ".");

            return outputedMoney;
        }
    }
}