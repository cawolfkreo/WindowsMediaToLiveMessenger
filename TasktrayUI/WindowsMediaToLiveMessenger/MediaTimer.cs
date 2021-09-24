using System.Timers;

namespace AudioSyncMSNUI.WindowsMediaToLiveMessenger
{
    class MediaTimer
    {
        private static Timer timer;

        public static Timer CreateTimer(int interval)
        {
            timer = new Timer(interval);
            timer.AutoReset = true;

            return timer;
        }

        public static void StartTimer()
        {
            timer.Enabled = true;
        }

        public static void PauseTimer()
        {
            timer.Enabled = false;
        }

        public static void StopTimer()
        {
            timer.Stop();
            timer.Dispose();
        }
    }
}
