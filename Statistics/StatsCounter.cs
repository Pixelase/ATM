using System;
using System.Collections.Generic;

namespace Statistics
{
    /// <summary>
    ///     Класс для ведения статистики сессии банкомата
    /// </summary>
    public class StatsCounter
    {
        /// <summary>
        ///     Записи статистики
        /// </summary>
        private List<MoneyWithdrawnEventArgs> _statsEntries;

        public StatsCounter()
        {
            MoneyWithdrawn += c_MoneyWithdrawn;
            _statsEntries = new List<MoneyWithdrawnEventArgs>();
        }

        /// <summary>
        ///     Класс для ведения статистики
        /// </summary>
        public List<MoneyWithdrawnEventArgs> StatsEntries
        {
            get { return _statsEntries; }
            set { _statsEntries = value; }
        }

        /// <summary>
        ///     Добавить запись статистики
        /// </summary>
        /// <param name="balance"></param>
        /// <param name="userSum"></param>
        public void Add(decimal balance, decimal userSum)
        {
            var args = new MoneyWithdrawnEventArgs
            {
                Balance = balance,
                UserSum = userSum,
                WithdrawnTime = DateTime.Now
            };
            OnMoneyWithdrawn(args);
        }

        /// <summary>
        ///     Функция, вызывающая обработчик события выдачи денег
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMoneyWithdrawn(MoneyWithdrawnEventArgs e)
        {
            var handler = MoneyWithdrawn;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        ///     Обработчик события выдачи денег
        /// </summary>
        public event EventHandler<MoneyWithdrawnEventArgs> MoneyWithdrawn;

        /// <summary>
        ///     Функция, определяющая действие при выдаче денег
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void c_MoneyWithdrawn(object sender, MoneyWithdrawnEventArgs e)
        {
            _statsEntries.Add(e);
        }
    }
}