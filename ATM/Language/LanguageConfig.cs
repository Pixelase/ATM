using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using log4net;
using log4net.Config;

namespace ATM.Language
{
    public class LanguageConfig
    {
        public string AskForMoney { get; set; }
        public string Banknote { get; set; }
        public string InvalidData { get; set; }
        public string NotEnoughMoney { get; set; }
        public string IncorrectInput { get; set; }
        public string Number { get; set; }
        public string Exit { get; set; }
        public string Sum { get; set; }
        public string YourMoney { get; set; }
        public string Status { get; set; }

        private static readonly ILog Log = LogManager.GetLogger(typeof (LanguageConfig));


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
                Exit = rm.GetString("Exit", ci);
                Sum = rm.GetString("Sum", ci);
                Status = rm.GetString("Status", ci);
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
