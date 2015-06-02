using System;
using System.Linq;
using ATM;
using ATM.Language;
using Statistics;
using Statistics.Viewers;

namespace Console_User_Interface
{
    /// <summary>
    ///     Класс, для обработки консольных комманд
    /// </summary>
    public class CommandPerformer : ICommandPerformer
    {
        private readonly CashMachine _cashMachine;
        private readonly LanguageConfig _lang;
        private readonly StatsCounter _statsCounter;

        public CommandPerformer(CashMachine cashMachine, LanguageConfig lang, StatsCounter statsCounter)
        {
            _cashMachine = cashMachine;
            _lang = lang;
            _statsCounter = statsCounter;
        }

        public bool TryPerform(string command)
        {
            if (command == _lang.ExitCommand)
            {
                _cashMachine.Exit();
                return true;
            }

            if (command.Split().First() == _lang.InsertCommand)
            {
                if (_cashMachine.TryInsertCassettes(command.Substring(command.IndexOf(" ", StringComparison.Ordinal))))
                {
                    Console.WriteLine("\n" + _lang.InsertSuccess + "\n");
                    Console.WriteLine(_lang.Status + ":");
                    Console.WriteLine(_cashMachine.Status() + '\n');
                }
                else
                {
                    Console.WriteLine("\n" + _lang.InsertFail + "\n");
                }
                return true;
            }

            if (command == _lang.RemoveCommand)
            {
                _cashMachine.RemoveCassettes();
                Console.WriteLine("\n" + _lang.RemoveSuccess + "\n");
                return true;
            }

            if (command == _lang.StatsCommand)
            {
                if (_statsCounter.StatsEntries.Count != 0)
                {
                    Console.WriteLine("\n" + _lang.Date + ":                                " + _lang.Balance +
                                      ":                    " + _lang.WithdrawnSum + ':');
                    Console.WriteLine(new StatsStringViewer().Show(_statsCounter));
                }
                else
                {
                    Console.WriteLine('\n' + _lang.EmptyStats + '\n');
                }
                return true;
            }

            if (command == _lang.HelpCommand)
            {
                Console.WriteLine('\n' + _lang.AllCommands + '\n' +
                                  _lang.HelpCommand + "                     " + _lang.Help + '\n' +
                                  _lang.ExitCommand + "                     " + _lang.Exit + '\n' +
                                  _lang.InsertCommand + "                   " + _lang.InsertCassettes + '\n' +
                                  _lang.RemoveCommand + "                   " + _lang.RemoveCassettes + '\n' +
                                  _lang.ClearCommand + "                    " + _lang.Clear + '\n' +
                                  _lang.StatsCommand + "                    " + _lang.Statistics + '\n' +
                                  _lang.StatusCommand + "                   " + _lang.Status + '\n'
                    );
                return true;
            }

            if (command == _lang.ClearCommand)
            {
                Console.Clear();
                return true;
            }

            if (command == _lang.StatusCommand)
            {
                Console.WriteLine('\n' + _lang.Status + ':');
                Console.WriteLine(_cashMachine.Status() + '\n');
                return true;
            }

            return false;
        }
    }
}