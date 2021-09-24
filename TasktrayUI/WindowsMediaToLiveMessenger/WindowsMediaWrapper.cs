using System;
using System.Threading.Tasks;
using Windows.Media.Control;

namespace AudioSyncMSNUI.WindowsMediaToLiveMessenger
{
    class CurrentMedia
    {
        public string artist;

        public string title;

        public string album;

        public bool isPlaying;

        public CurrentMedia(string artist, string title, string album, bool isPlaying)
        {
            this.artist = artist;
            this.title = title;
            this.album = album;
            this.isPlaying = isPlaying;
        }
    }

    class WindowsMediaWrapper
    {
        private GlobalSystemMediaTransportControlsSessionManager sessionManager;

        public event EventHandler<CurrentMedia> OnCurrentSessionChange;

        public WindowsMediaWrapper(EventHandler<CurrentMedia> mediaChange)
        {
            sessionManager = null;
            Start().Wait();
            SusbcribeSessionEvents(mediaChange);
        }

        private async Task Start()
        {
            sessionManager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
        }

        private async Task<CurrentMedia> GetCurrentMediaAsync()
        {
            var currentSession = sessionManager.GetCurrentSession();
            if(currentSession != null)
            {
                return await GetMediaBySession(currentSession);
            }
            return new CurrentMedia("", "", "", false);
        }

        private static async Task<CurrentMedia> GetMediaBySession(GlobalSystemMediaTransportControlsSession currentSession)
        {
            var mediaInfo = await currentSession.TryGetMediaPropertiesAsync();

            string artist = (mediaInfo.Artist.Length != 0) ? mediaInfo.Artist : mediaInfo.AlbumArtist;
            string title = mediaInfo.Title;
            string album = mediaInfo.AlbumTitle;

            bool isPlaying = currentSession.GetPlaybackInfo().PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing;

            return new CurrentMedia(artist, title, album, isPlaying);
        }

        public CurrentMedia GetCurrentMedia()
        {
            CurrentMedia currentMedia = GetCurrentMediaAsync().GetAwaiter().GetResult();

            return currentMedia;
        }

        public void SusbcribeSessionEvents(EventHandler<CurrentMedia> mediaChange)
        {
            OnCurrentSessionChange += mediaChange;
            sessionManager.CurrentSessionChanged += SessionHandler;
            sessionManager.SessionsChanged += (sessionManager, _) => {
                SessionHandler(sessionManager, null);
            };

            SubscribeMediaEvents();
        }

        void SessionHandler(GlobalSystemMediaTransportControlsSessionManager sessionMan, CurrentSessionChangedEventArgs _)
        {
            if (OnCurrentSessionChange == null)
            {
                return;
            }
            sessionManager = sessionMan;
            GlobalSystemMediaTransportControlsSession currentSession = sessionManager.GetCurrentSession();
            CurrentMedia currentMedia = GetMediaBySession(currentSession).GetAwaiter().GetResult();
            OnCurrentSessionChange?.Invoke(this, currentMedia);

            SubscribeMediaEvents(currentSession);
        }

        private void SubscribeMediaEvents(GlobalSystemMediaTransportControlsSession session = null)
        {
            var currentSession = session ?? sessionManager.GetCurrentSession();

            if (currentSession != null)
            {
                currentSession.MediaPropertiesChanged += MediaHandler;
                currentSession.PlaybackInfoChanged += InforHandler;
            }
        }

        private void InforHandler(GlobalSystemMediaTransportControlsSession sender, PlaybackInfoChangedEventArgs args)
        {
            MediaHandler(sender, null);
        }

        private void MediaHandler(GlobalSystemMediaTransportControlsSession session, MediaPropertiesChangedEventArgs args)
        {
            if(OnCurrentSessionChange == null)
            {
                return;
            }

            CurrentMedia currentMedia = GetMediaBySession(session).GetAwaiter().GetResult();
            OnCurrentSessionChange?.Invoke(this, currentMedia);
        }
    }    
}
