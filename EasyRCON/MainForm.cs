using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyRCON
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            // Connect
            if (!RconHelper.connected)
            {
                await RconHelper.Connect(textBox1.Text);
            }
            else
                MessageBox.Show("Error: Already connected to RCON or connecting is in process.",
                       "EasyRCON", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Disconnect
            if (RconHelper.connected)
            {
                RconHelper.Disconnect();
            }
            else
                MessageBox.Show("Error: Not connected to RCON.",
                       "EasyRCON", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Send
            if (RconHelper.connected)
            {
                string cmd = textBox1.Text;
                RconHelper.rcon.SendCommandAsync(cmd);
                AddText("> " + cmd);
            }
            else
                MessageBox.Show("Error: Not connected to RCON.",
                       "EasyRCON", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void AddText(string text)
        {
            richTextBox1.Text += "\n" + text;
        }
    }
}
