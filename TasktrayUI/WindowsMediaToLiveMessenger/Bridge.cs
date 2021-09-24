using System;
using System.Threading;
using System.Threading.Tasks;

namespace AudioSyncMSNUI.WindowsMediaToLiveMessenger
{
    public class Bridge
    {
        /// <summary>
        /// The name of the music player
        /// messenger will receive.
        /// </summary>
        private const string PLAYER = "Cawolf";

        /// <summary>
        /// the windows media (the current songs
        /// being played on the OS) wrapper
        /// </summary>
        private WindowsMediaWrapper mediaWrapper;

        /// <summary>
        /// This is the MSN object that sends
        /// the messages to the MSN client (if
        /// there is a windows active at the 
        /// moment).
        /// </summary>
        private readonly LiveMessengerWrapper liveMessenger;

        /// <summary>
        /// cancellation Token source
        /// </summary>
        private CancellationTokenSource cts;

        public Bridge()
        {
            mediaWrapper = new(MediaChange);
            liveMessenger = new();
        }

        internal Bridge(bool blocking)
        {
            if (blocking)
            {
                mediaWrapper = new(MediaChange);
                StartBlock().Wait();
            }
        }

        public void StartBridge()
        {            
            var timer = MediaTimer.CreateTimer(4*1000); //Timer every 4 seconds.
            StartProcessingMedia();

            timer.Elapsed += (_, _) =>
            {
                mediaWrapper = new(MediaChange);
                StartProcessingMedia();
                System.Diagnostics.Debug.WriteLine("Timer was called!");
            };

            MediaTimer.StartTimer();
        }

        public static void StopBridge()
        {
            LiveMessengerWrapper.EndsUpdate(PLAYER);
            MediaTimer.StopTimer();
        }

        public void ChangeMSNMessage(string messageFormat)
        {
            liveMessenger.MsgFormat = messageFormat.Trim();
            Task t = new(() => StartProcessingMedia());
            t.Start();
            t.Wait();
        }

        private async Task StartBlock()
        {
            StartProcessingMedia();

            cts = new CancellationTokenSource();
            Task task = NeverEndingClass.StartNeverEndingTask(cts.Token);

            await task;
            LiveMessengerWrapper.EndsUpdate(PLAYER);
        }

        private void StartProcessingMedia()
        {
            CurrentMedia media = mediaWrapper.GetCurrentMedia();
            SendMediaToMSN(media);
        }

        private void SendMediaToMSN(CurrentMedia media)
        {
            if (media.isPlaying)
            {                
                System.Diagnostics.Debug.WriteLine($"Media {media.artist} - {media.title} sent to MSN");
                liveMessenger.SendUpdate(PLAYER, media);
                MediaTimer.PauseTimer();
            }
            else
            {
                LiveMessengerWrapper.EndsUpdate(PLAYER);
                MediaTimer.StartTimer();
            }
        }

        private void MediaChange(object windowsMedia, CurrentMedia media)
        {
            SendMediaToMSN(media);
        }

        private void ApplicationExit(object sender, EventArgs e)
        {
            LiveMessengerWrapper.EndsUpdate(PLAYER);
            cts.Cancel();
        }
    }
}
