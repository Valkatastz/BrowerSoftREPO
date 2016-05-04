using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech;
using System.Speech.Recognition;

namespace Star_Browser
{
    public partial class frmStarBrowser : Form
    {
        SpeechRecognitionEngine voiceEngine = new SpeechRecognitionEngine();
        
        public frmStarBrowser()
        {
            InitializeComponent();
        }

        WebBrowser Star = new WebBrowser();
        int i = 0;

        private void frmStarBrowser_Load(object sender, EventArgs e)
        {
            Star = new WebBrowser();
            Star.ScriptErrorsSuppressed = true;
            Star.Dock = DockStyle.Fill;
            Star.Visible = true;
            Star.DocumentCompleted += star_DocumentCompleted;
            Star.Navigate("http://google.com"); // initial startup page
            tabControl1.TabPages.Add("New Tab");
            tabControl1.SelectTab(i);
            tabControl1.SelectedTab.Controls.Add(Star);
            i += 1;
            
            //the voice commands which are under development for the AI
            Choices sscommands = new Choices();
            sscommands.Add(new string[] {"open youtube", "google", "press enter", "new tab"});
            GrammarBuilder ssBuilder = new GrammarBuilder();
            ssBuilder.Append(sscommands);
            Grammar ssgrammar = new Grammar(ssBuilder);

            voiceEngine.LoadGrammarAsync(ssgrammar);
            voiceEngine.SetInputToDefaultAudioDevice();
            voiceEngine.SpeechRecognized += voiceEngine_SpeechRecognized;
        }

        //Voice engine recognizer.
        void voiceEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            switch (e.Result.Text)
            {
                case "new tab":
                    toolStripButton6.Enabled = true;
                    break;
                case "google":
                    break;
            }
        }

        void star_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            tabControl1.SelectedTab.Text = ((WebBrowser)tabControl1.SelectedTab.Controls[0]).DocumentTitle;

            toolStripTextBox1.Text = Star.Url.AbsoluteUri;
        }

        private void Navigate(String address)
        {
            if (String.IsNullOrEmpty(address)) return;
            if (address.Equals("about:blank")) return;
            if (!address.StartsWith("http://") &&
                !address.StartsWith("https://"))
            {
                address = "http://" + address;
            }
            try
            {
                Star.Navigate(new Uri(address));
            }
            catch (System.UriFormatException)
            {
                return;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ((WebBrowser)tabControl1.SelectedTab.Controls[0]).GoBack();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ((WebBrowser)tabControl1.SelectedTab.Controls[0]).GoForward();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            ((WebBrowser)tabControl1.SelectedTab.Controls[0]).Refresh();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            ((WebBrowser)tabControl1.SelectedTab.Controls[0]).Navigate("http://google.com");
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            ((WebBrowser)tabControl1.SelectedTab.Controls[0]).Navigate(toolStripTextBox1.Text);
        }

        private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                ((WebBrowser)tabControl1.SelectedTab.Controls[0]).Navigate(toolStripTextBox1.Text);
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            Star = new WebBrowser();
            Star.ScriptErrorsSuppressed = true;
            Star.Dock = DockStyle.Fill;
            Star.Visible = true;
            Star.DocumentCompleted += star_DocumentCompleted;
            Star.Navigate("http://google.com");
            tabControl1.TabPages.Add("New Tab");
            tabControl1.SelectTab(i);
            tabControl1.SelectedTab.Controls.Add(Star);
            i += 1;
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.Count - 1 > 0)
            {
                tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);
                tabControl1.SelectTab(tabControl1.TabPages.Count - 1);
                i -= 1;
            }
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            voiceEngine.RecognizeAsync(RecognizeMode.Multiple);
            toolStripButton9.Enabled = true;
            toolStripButton8.Enabled = false;
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            voiceEngine.RecognizeAsyncStop();
            toolStripButton9.Enabled = false;
            toolStripButton8.Enabled = true;
        }
    }
}
