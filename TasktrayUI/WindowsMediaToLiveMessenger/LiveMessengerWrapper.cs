using System;
using System.Runtime.InteropServices;

namespace AudioSyncMSNUI.WindowsMediaToLiveMessenger
{
    class LiveMessengerWrapper
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct COPYDATASTRUCT
        {
            /// <summary>
            /// The size, in bytes, of the data pointed to by the lpData member.
            /// </summary>
            public IntPtr dwData;

            /// <summary>
            /// The type of the data to be passed to the receiving application. The receiving application defines the valid types.
            /// </summary>
            public int cbData;

            /// <summary>
            /// The data to be passed to the receiving application. This member can be NULL.
            /// </summary>
            //[MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }

        [DllImport("User32.dll", EntryPoint = "FindWindowEx", CharSet = CharSet.Unicode, SetLastError = false)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        [DllImport("User32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, ref COPYDATASTRUCT lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SendMessageTimeout(IntPtr hwnd, uint wMsg, IntPtr wParam,
            ref COPYDATASTRUCT lParam, uint fuFlags, uint uTimeout, out uint lpdwResult);

        /// <summary>
        /// This is a constant defined on the
        /// Data copy documentation
        /// <seealso cref="https://docs.microsoft.com/en-us/windows/win32/dataxchg/wm-copydata"/>
        /// </summary>
        private const uint WM_COPYDATA = 0x004A;

        /// <summary>
        /// Magic number for the timeout message
        /// function.
        /// </summary>
        public const uint SMTO_ABORTIFHUNG = 0x0002;

        /// <summary>
        /// This is the window class for messenger, used
        /// to search for the application on Windows.
        /// </summary>
        public const string MESSENGER_CLASS = "MsnMsgrUIManager";

        /// <summary>
        /// This is the structure of the message that
        /// Live messenger and messenger expects.
        /// </summary>
        private const string MSG_FORMAT = @"{0}\0{1}\0{2}\0{3}\0{4}\0{5}\0{6}\0WMContentID\0";

        /// <summary>
        /// This is the format of the message that 
        /// will be displayed on MSN. The values 
        /// "{0}", "{1}" and "{3}", represent the
        /// song name, artist and album name
        /// respectively.
        /// </summary>
        private string _format = "On Windows 10: ♪♫{1} - {0}♪♫";
        private const string _formatDefault = "On Windows 10: ♪♫{1} - {0}♪♫";

        /// <summary>
        /// This is the format of the message that 
        /// will be displayed on MSN. The values 
        /// "{0}", "{1}" and "{3}", represent the
        /// song name, artist and album name
        /// respectively.
        /// </summary>
        public string MsgFormat
        {
            get => _format;
            set => _format = value.Length == 0 ? _formatDefault : value;
        }

        public void SendUpdate(string player, CurrentMedia currentMedia)
        {
            string message = BuildMessage(player, _format, currentMedia);

            SendToMSN(message);
        }

        public static void EndsUpdate(string player)
        {
            CurrentMedia noMedia = new("", "", "", false);
            string message = BuildMessage(player, "", noMedia);

            SendToMSN(message);
        }

        private static void SendToMSN(string message)
        {
            IntPtr messengerWindow = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "MsnMsgrUIManager", null);
            if (messengerWindow != IntPtr.Zero)
            {
                do
                {
                    SendToMessenger(message, messengerWindow);

                    messengerWindow = FindWindowEx(IntPtr.Zero, messengerWindow, "MsnMsgrUIManager", null);
                } while (messengerWindow != IntPtr.Zero);
            }
        }

        private static string BuildMessage(string player, string format, CurrentMedia currentMedia)
        {
            string isPlaying = currentMedia.isPlaying ? "1" : "0";

            string message = string.Format(MSG_FORMAT,
                                        player,
                                        "Music",
                                        isPlaying,
                                        format,
                                        currentMedia.title,
                                        currentMedia.artist,
                                        currentMedia.album);

            return message;
        }
        

        private static void SendToMessenger(string message, IntPtr messengerWindow)
        {
            var payload = new COPYDATASTRUCT
            {
                dwData = (IntPtr)0x547,
                cbData = (message.Length * sizeof(char)) + 2,
                lpData = message
            };

            _ = SendMessageTimeout(messengerWindow, WM_COPYDATA, IntPtr.Zero, ref payload, SMTO_ABORTIFHUNG, 3000u, out uint response);

            /*IntPtr response = SendMessage(messengerWindow, WM_COPYDATA, IntPtr.Zero, ref payload);

            System.Diagnostics.Debug.WriteLine("Enviado " + message);
            System.Diagnostics.Debug.WriteLine($"a la ventana: {messengerWindow}");
            System.Diagnostics.Debug.WriteLine($"Recibí {response} de messenger");*/
        }
    }
}
