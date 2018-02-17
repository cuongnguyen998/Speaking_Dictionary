using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dictionary
{
    public partial class Form1 : Form
    {
        DictionaryManager dictionary;
        SpeakText Vietnamese;
        SpeakText English;
        bool isLoading1 = true;
        bool isLoading2 = true;
        public Form1()
        {
            
            InitializeComponent();
            ChangeLoading();
            WebBrowser wbVN = new WebBrowser();
            wbVN.Height = 0;
            wbVN.Width = 0;
            wbVN.Visible = false;
            wbVN.ScriptErrorsSuppressed = true;
            this.Controls.Add(wbVN);
            wbVN.Navigate(Cons.VNLink);
            wbVN.DocumentCompleted += WbVN_DocumentCompleted;

            WebBrowser wbEng = new WebBrowser();
            wbEng.Height = 0;
            wbEng.Width = 0;
            wbEng.Visible = false;
            wbEng.ScriptErrorsSuppressed = true;
            this.Controls.Add(wbEng);
            wbEng.Navigate(Cons.USLink);
            wbEng.DocumentCompleted += WbEng_DocumentCompleted;

            Vietnamese = new SpeakText(wbVN);
            English = new SpeakText(wbEng);
            cbWord.DisplayMember = "Key";
            dictionary = new DictionaryManager();
            dictionary.LoadDataToComboBox(cbWord);
        }

        private void WbEng_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            isLoading1 = false;
            ChangeLoading();
        }

        private void WbVN_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            isLoading2 = false;
            ChangeLoading();
        }
        void ChangeLoading()
        {
            this.Enabled = !(isLoading1 && isLoading2);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do you want to exit?", "Notifycation", MessageBoxButtons.OKCancel)!= DialogResult.OK)
            {
                e.Cancel = true;
                return;
            }
            dictionary.Serialize();
        }

        private void cbWord_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if (cb.DataSource == null)
            {
                return;
            }
            DictionaryData data = cb.SelectedItem as DictionaryData;
            txtMeaning.Text = data.Meaning;
            txtExplaination.Text = data.Explaination;

        }

        private void btnSpeakEnglish_Click(object sender, EventArgs e)
        {
            
            English.Speak((cbWord.SelectedItem as DictionaryData).Key);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            Vietnamese.Speak(txtMeaning.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            Vietnamese.Speak(txtExplaination.Text);
        }
    }
}
