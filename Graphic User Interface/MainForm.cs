using System;
using System.Windows.Forms;
using log4net;
using log4net.Config;

namespace Graphic_User_Interface
{
    public partial class MainForm : Form
    {
        public static readonly ILog Log = LogManager.GetLogger(typeof(MainForm));

        public MainForm()
        {
            InitializeComponent();
            XmlConfigurator.Configure();
        }

        private void buttonNumber_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null) inputTextBox.Text += button.Text;
        }
    }
}
