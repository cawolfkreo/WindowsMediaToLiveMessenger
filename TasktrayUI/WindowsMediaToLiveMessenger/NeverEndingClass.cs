using System.Threading;
using System.Threading.Tasks;

namespace AudioSyncMSNUI.WindowsMediaToLiveMessenger
{
    class NeverEndingClass
    {
        public static Task StartNeverEndingTask(CancellationToken token)
        {
            return Task.Factory.StartNew(() => NeverEndingTask(token), token);
        }

        private static void NeverEndingTask(CancellationToken token)
        {
            bool shouldRun = true;
            while (shouldRun)
            {
                shouldRun = !token.IsCancellationRequested;
            }
        }
    }
}
