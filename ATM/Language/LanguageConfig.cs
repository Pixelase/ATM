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
        public string InvalidData { get; set; }
        public string NotEnoughMoney { get; set; }
        public string ImposibleToCollectSum { get; set; }
        public string TooManyBills { get; set; }
        public string Exit { get; set; }

        private static readonly ILog Log = LogManager.GetLogger(typeof(LanguageConfig));


        public LanguageConfig(string culture)
        {
            XmlConfigurator.Configure();
            try
            {
                Assembly assemblyAtm = Assembly.Load("ATM");
                ResourceManager rm = new ResourceManager("ATM.Resources.langres", assemblyAtm);
                CultureInfo ci = new CultureInfo(culture);

                AskForMoney = rm.GetString("askForMoney", ci);
                ImposibleToCollectSum = rm.GetString("ImposibleToCollectSum", ci);
                InvalidData = rm.GetString("InvalidData", ci);
                NotEnoughMoney = rm.GetString("NotEnoughMoney", ci);
                TooManyBills = rm.GetString("TooManyBills", ci);
                Exit = rm.GetString("Exit", ci);
            }
            catch (CultureNotFoundException ex)
            {
                Log.Error("Incorrect culture setup: " + ex.Message);
            }
        }
    }
}
