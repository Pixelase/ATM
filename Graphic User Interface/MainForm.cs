using System;
using System.Linq;
using System.Windows.Forms;
using ATM;
using ATM.Language;
using log4net;
using log4net.Config;
using Statistics;
using Statistics.Viewers;

namespace Graphic_User_Interface
{
    public partial class MainForm : Form
    {
        public static readonly ILog Log = LogManager.GetLogger(typeof (MainForm));
        private readonly CashMachine _cashMachine;
        private readonly StatsCounter _statsCounter;
        private LanguageConfig _lang;
        //Путь по умолчанию к файлу с кассетами
        private string _path;
        private decimal _usersRequest;

        public MainForm()
        {
            InitializeComponent();

            //Путь по умолчанию к файлу с кассетами
            _path = @"D:\Visual Studio\OOP\ATM\bin\Debug\data.json";
            _cashMachine = new CashMachine(_path);
            _lang = new LanguageConfig(_cashMachine.CurrentCulture);
            LoadLang();
            _statsCounter = new StatsCounter();
            XmlConfigurator.Configure();

            //Вывод текущего состояния счёта
            DisplayPrintln(_lang.Status + ":");
            DisplayPrintln(_cashMachine.Status() + '\n');
        }

        private void buttonNumber_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null) inputTextBox.Text += button.Text;
        }

        private void DisplayPrintln(string line)
        {
            outputRichTextBox.AppendText(line + '\n');
            outputRichTextBox.ScrollToCaret();
        }

        private void enterButton_Click(object sender, EventArgs e)
        {
            //Пользовательский запрос
            decimal.TryParse(inputTextBox.Text, out _usersRequest);
            Log.Debug("Users request: " + _usersRequest);

            //Начало работы с введённой пользователем суммой
            if (_usersRequest < _cashMachine.Balance && _usersRequest >= 0)
            {
                //Выданная сумма
                decimal withdrawnSum = 0;

                //Вывод && подсчёт выданной суммы
                DisplayPrintln('\n' + _lang.YourMoney + ":");
                foreach (var item in _cashMachine.WithdrawMoney(_usersRequest).Banknotes)
                {
                    var banknoteNomimal = item.Key.Nominal;
                    var banknotesCount = item.Value;
                    withdrawnSum += banknoteNomimal*banknotesCount;
                    DisplayPrintln(_lang.Banknote + ":" + item.Key.Nominal + " <-> " + _lang.Number + ": " +
                                   item.Value);
                }
                DisplayPrintln(_lang.WithdrawnSum + ": " + withdrawnSum);

                //Вызов события добавления записи статистики
                _statsCounter.Add(_cashMachine.Balance, withdrawnSum);

                //Вывод текущего состояния счёта
                DisplayPrintln('\n' + _lang.Status + ':');
                DisplayPrintln(_cashMachine.Status() + '\n');
            }
            else if (_usersRequest > _cashMachine.Balance)
            {
                DisplayPrintln('\n' + _lang.NotEnoughMoney + "\n\n");
                Log.Error("Not enough money");
            }

            inputTextBox.Clear();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _cashMachine.Exit();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            inputTextBox.Clear();
            outputRichTextBox.Clear();
        }

        private void insertButton_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = @"Image Files(*.txt;*.json;*.csv;*.xml)|*.txt;*.json;*.csv;*.xml|All files (*.*)|*.*",
                Title = @"Select a Cassette File"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _path = openFileDialog.FileName;
                if (_cashMachine.TryInsertCassettes(_path))
                {
                    DisplayPrintln("\n" + _lang.InsertSuccess + "\n");
                    DisplayPrintln(_lang.Status + ":");
                    DisplayPrintln(_cashMachine.Status() + '\n');
                }
                else
                {
                    DisplayPrintln("\n" + _lang.InsertFail + "\n");
                }
            }
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            _cashMachine.RemoveCassettes();
            DisplayPrintln("\n" + _lang.RemoveSuccess + "\n");
        }

        private void statsButton_Click(object sender, EventArgs e)
        {
            if (_statsCounter.StatsEntries.Count != 0)
            {
                MessageBox.Show("\n" + _lang.Date + ":                                      " + _lang.Balance +
                                ":                    " + _lang.WithdrawnSum + ":\n" +
                                new StatsStringViewer().Show(_statsCounter), _lang.Statistics);
            }
            else
            {
                MessageBox.Show('\n' + _lang.EmptyStats + '\n', _lang.Statistics);
            }
        }

        private void backspaceButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(inputTextBox.Text))
                inputTextBox.Text = inputTextBox.Text.Remove(inputTextBox.Text.Length - 1);
        }

        private void LoadLang()
        {
            enterButton.Text = _lang.Enter;
            clearButton.Text = _lang.Clear.Split().First();
            langButton.Text = _lang.SwitchLang;
            insertButton.Text = _lang.InsertCassettes;
            removeButton.Text = _lang.RemoveCassettes;
            statsButton.Text = _lang.Statistics.Split().First();
            displayLabel.Text = _lang.Display;
        }

        private void langButton_Click(object sender, EventArgs e)
        {
            _cashMachine.CurrentCulture = _cashMachine.CurrentCulture == "en-US" ? "ru-RU" : "en-US";
            _lang = new LanguageConfig(_cashMachine.CurrentCulture);

            LoadLang();
        }
    }
}