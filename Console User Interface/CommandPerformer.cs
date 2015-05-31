using System;
using System.Linq;
using ATM;
using ATM.Language;

namespace Console_User_Interface
{
    public class CommandPerformer : ICommandPerformer
    {
        private readonly CashMachine _cashMachine;
        private readonly LanguageConfig _lang;

        public CommandPerformer(CashMachine cashMachine, LanguageConfig lang)
        {
            _cashMachine = cashMachine;
            _lang = lang;
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
                    Console.WriteLine("\nСassettes have been successfully inserted\n");
                    Console.WriteLine(_lang.Status + ":");
                    Console.WriteLine(_cashMachine.Status() + '\n');
                }
                else
                {
                    Console.WriteLine("\nFailed to insert cassettes\n");
                }
                return true;
            }

            if (command == _lang.RemoveCommand)
            {
                _cashMachine.RemoveCassettes();
                Console.WriteLine("\nСassettes have been successfully removed\n");
                return true;
            }

            if (command == _lang.StatsCommand)
            {
                Console.WriteLine('\n' + _lang.Statistics + "\n<--> Not implemented <-->\n");
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