using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using log4net;
using log4net.Config;

namespace ATM.Language
{
    /// <summary>
    ///     Класс для локализации банкомата
    /// </summary>
    public class LanguageConfig
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LanguageConfig));

        public readonly string AskForMoney;
        public readonly string Banknote;
        public readonly string InvalidData;
        public readonly string NotEnoughMoney;
        public readonly string IncorrectInput;
        public readonly string Number;
        public readonly string ExitCommand;
        public readonly string Balance;
        public readonly string YourMoney;
        public readonly string Status;
        public readonly string InsertCassettes;
        public readonly string RemoveCassettes;
        public readonly string InsertCommand;
        public readonly string RemoveCommand;
        public readonly string Help;
        public readonly string HelpCommand;
        public readonly string StatsCommand;
        public readonly string Statistics;
        public readonly string AllCommands;
        public readonly string Exit;
        public readonly string Clear;
        public readonly string ClearCommand;
        public readonly string StatusCommand;
        public readonly string Date;
        public readonly string WithdrawnSum;
        public readonly string EmptyStats;

        /// <summary>
        ///     Конструктор класса LanguageConfig
        /// </summary>
        /// <param name="culture">Язык</param>
        public LanguageConfig(string culture)
        {
            XmlConfigurator.Configure();
            try
            {
                var assemblyAtm = Assembly.Load("ATM");
                var rm = new ResourceManager("ATM.Resources.langres", assemblyAtm);
                var ci = new CultureInfo(culture);

                AskForMoney = rm.GetString("AskForMoney", ci);
                Banknote = rm.GetString("Banknote", ci);
                IncorrectInput = rm.GetString("IncorrectInput", ci);
                InvalidData = rm.GetString("InvalidData", ci);
                NotEnoughMoney = rm.GetString("NotEnoughMoney", ci);
                Number = rm.GetString("Number", ci);
                YourMoney = rm.GetString("YourMoney", ci);
                ExitCommand = rm.GetString("ExitCommand", ci);
                Balance = rm.GetString("Balance", ci);
                Status = rm.GetString("Status", ci);
                InsertCassettes = rm.GetString("InsertCassettes", ci);
                RemoveCassettes = rm.GetString("RemoveCassettes", ci);
                InsertCommand = rm.GetString("InsertCommand", ci);
                RemoveCommand = rm.GetString("RemoveCommand", ci);
                Help = rm.GetString("Help", ci);
                HelpCommand = rm.GetString("HelpCommand", ci);
                StatsCommand = rm.GetString("StatsCommand", ci);
                Statistics = rm.GetString("Statistics", ci);
                AllCommands = rm.GetString("AllCommands", ci);
                Exit = rm.GetString("Exit", ci);
                Clear = rm.GetString("Clear", ci);
                ClearCommand = rm.GetString("ClearCommand", ci);
                StatusCommand = rm.GetString("StatusCommand", ci);
                Date = rm.GetString("Date", ci);
                WithdrawnSum = rm.GetString("WithdrawnSum", ci);
                EmptyStats = rm.GetString("EmptyStats", ci);
            }
            catch (CultureNotFoundException ex)
            {
                Log.Error("Incorrect culture setup: " + ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error("Something goes wrong: " + ex.Message);
            }
        }
    }
}