using System;

namespace Service
{
    public class TechTask
    {
        private TechTaskStatus _status;
        private DateTime _startTime;
        private TimeSpan _finalTimeSpent;

        public TechTaskStatus Status
        {
            get => _status;
            internal set => SetStatus(value);
        }

        private void SetStatus(TechTaskStatus status)
        {
            _status = status;

            if (status == TechTaskStatus.Done)
                _finalTimeSpent = DateTime.Now - _startTime;
            else
                _finalTimeSpent = TimeSpan.Zero;
        }

        public string Description { get; }
        public TimeSpan TimeSpent { get => GetTimeSpent(); }

        public Employee Handler { get; internal set; }

        internal TechTask(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Описание запроса не должно быть пустым");

            Description = description;
            _startTime = DateTime.Now;
        }

        private TimeSpan GetTimeSpent()
        {
            if (Status == TechTaskStatus.Active)
                return DateTime.Now - _startTime;
            return _finalTimeSpent;
        }
    }

    public enum TechTaskStatus { Active, Done }
}
