namespace Statistics.Viewers
{
    public interface IStatsViewer<out T>
    {
        /// <summary>
        /// Отобразить статистику
        /// </summary>
        /// <param name="statsCounter">Основной класс статистики</param>
        /// <returns></returns>
        T Show(StatsCounter statsCounter);
    }
}
