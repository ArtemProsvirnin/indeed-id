using System;

namespace Service
{
    public delegate void Notification(TechTask task, NotificationArgs args);
    public delegate void Cancelation(TechTask task);

    public class NotificationArgs: EventArgs
    {
        public bool Handled { get; set; }
        public TechServiceConfig Config { get; }

        public NotificationArgs(TechServiceConfig config)
        {
            Config = config;
        }
    }
}
