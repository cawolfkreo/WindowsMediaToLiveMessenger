using System;
using System.Drawing;
using System.Windows.Forms;
using AudioSyncMSNUI.WindowsMediaToLiveMessenger;

namespace AudioSyncMSNUI
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TaskTrayContext());
        }

        public class TaskTrayContext : ApplicationContext
        {
            private readonly NotifyIcon trayIcon;
            private readonly Bridge bridge;
            private readonly string appName = "AudioSyncMSN";

            public TaskTrayContext()
            {
                Image exitImage = Properties.Resources.exitImage;
                trayIcon = new NotifyIcon
                {
                    Icon = new Icon("Resources/trayIcon.ico"),
                    Text = "Audio sync MSN",
                    ContextMenuStrip = new ContextMenuStrip()
                };

                trayIcon.ContextMenuStrip.Items.Add("Change message", null, SetMSNMessage);
                trayIcon.ContextMenuStrip.Items.Add("Exit", exitImage, Exit);

                trayIcon.Visible = true;

                bridge = new Bridge();
                Start();
            }            

            void Start()
            {
                RegistrySetup registry = new();
                registry.SetRegistry(appName);
                bridge.StartBridge();
            }

            void Exit(object sender, EventArgs e)
            {
                trayIcon.Visible = false;
                Bridge.StopBridge();
                Application.Exit();
            }

            void SetMSNMessage(object sender, EventArgs e)
            {
                string caption = "New MSN message";
                string text = "Please give a new message. Remember that \"{X}\" means where "
                                + "some information will be, based the value of X.\n"
                                + "0 = song title, 1 = song artist, 2 = song album.";
                string example = "Song: {0} \nArtist: {1} \nAlbum: {2}";
                string message = DialogForm.ShowDialog(text, caption, example);

                if(message.Length > 0)
                {
                    bridge.ChangeMSNMessage(message);
                }
                else
                {
                    caption = "Sorry! :C";
                    text = "You didn't provide a message :/.\nWould you like to set back the default?";
                    DialogResult result = MessageBox.Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result.Equals(DialogResult.Yes))
                    {
                        bridge.ChangeMSNMessage("");
                    }
                }
            }
        }
    }
}
