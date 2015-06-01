using System.Text;

namespace Statistics.Viewers
{
    /// <summary>
    /// Строковое представление статистики
    /// </summary>
    public class StatsStringViewer : IStatsViewer<string>
    {
        public string Show(StatsCounter statsCounter)
        {
            var result = new StringBuilder();

            foreach (var item in statsCounter.StatsEntries)
            {
                result.AppendFormat("{0}                       {1}                       {2}\n",
                    string.Format("{0:d.M.yyyy HH:mm}", item.WithdrawnTime), item.Balance, item.UserSum);
            }

            return result.ToString();
        }
    }
}