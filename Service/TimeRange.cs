using System;

namespace Service
{
    public class TimeRange
    {
        public TimeSpan MinTime { get; set; }
        public TimeSpan MaxTime { get; set; }

        public TimeRange(TimeSpan time) : this(time, time) { }

        public TimeRange(TimeSpan minTime, TimeSpan maxTime)
        {
            if (maxTime < minTime)
                throw new ArgumentException("Верхняя граница должна быть больше нижней");

            MinTime = minTime;
            MaxTime = maxTime;
        }

        public TimeSpan Random()
        {
            if (MinTime == MaxTime)
                return MinTime;

            return GenerateRandom();
        }

        private TimeSpan GenerateRandom()
        {
            Random rnd = new Random();

            int from = (int)MinTime.TotalSeconds;
            int to = (int)MaxTime.TotalSeconds;
            int random = rnd.Next(from, to);

            return TimeSpan.FromSeconds(random);
        }
    }
}
