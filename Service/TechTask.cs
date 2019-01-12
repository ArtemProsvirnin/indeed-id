using System;

namespace Service
{
    public class TechTask
    {
        private DateTime _startTime;

        public TechTaskStatus Status { get; internal set; }
        public string Description { get; }
        public TimeSpan TimeSpent { get => DateTime.Now - _startTime; }
        public Employee Handler { get; internal set; }

        internal TechTask(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Описание запроса не должно быть пустым");

            Description = description;
            _startTime = DateTime.Now;
        }
    }

    public enum TechTaskStatus { Active, Done }
}
