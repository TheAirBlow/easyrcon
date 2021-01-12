using CoreRCON;
using CoreRCON.Parsers.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyRCON
{
    class RconHelper
    {
        public static IPAddress ip;
        public static ushort port;
        public static string password;
        public static RCON rcon;
        public static LogReceiver log;
        public static bool connected = false;

        public static async Task Connect(string str)
        {
            string[] data = str.Split(':');
            if (data.Length <= 2)
            {
                MessageBox.Show("Error: Not the right format." +
                    "\nYou should write IPHere:PortHere:PasswordHere replacing ip and port." +
                    "\nPort should be a number, and IP an address like 189.658.5.20.", 
                    "EasyRCON", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                try
                {
                    ip = IPAddress.Parse(data[0]);
                    port = ushort.Parse(data[1]);
                    password = data[2];
                }
                catch
                {
                    MessageBox.Show("Error: Not the right content." +
                        "\nYou should write IPHere:PortHere:PasswordHere replacing ip and port." +
                        "\nPort should be a number, and IP address should be like 189.658.5.20", 
                        "EasyRCON", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            connected = true;
            Program.form.AddText($"[EasyRCON] Connecting to {ip} (port: {port}, password: {password})");

            rcon = new RCON(ip, port, password);
            await rcon.ConnectAsync();
            Status status = await rcon.SendCommandAsync<Status>("status");

            Program.form.AddText($"[EasyRCON] Connected to: {status.Hostname}");

            log = new LogReceiver(50000, new IPEndPoint(ip, port));
            log.Listen<ChatMessage>(chat =>
            {
                Program.form.AddText($"{chat.Player.Name}: {chat.Message} (channel {chat.Channel})");
            });
        }

        public static void Disconnect()
        {
            Program.form.AddText($"[EasyRCON] Disconnecting from {ip} (port: {port}, password: {password})");

            rcon.Dispose();
            if (log != null)
                log.Dispose();

            connected = false;
        }
    }
}
