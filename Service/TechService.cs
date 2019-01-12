using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Service
{
    public class TechService
    {
        private TechServiceConfig _config;
        
        public TimeSpan MinTime {
            get => _config.TimeRange.MinTime;
            set => _config.TimeRange.MinTime = value;
        }

        public TimeSpan MaxTime
        {
            get => _config.TimeRange.MaxTime;
            set => _config.TimeRange.MaxTime = value;
        }

        public TimeSpan Td
        {
            get => _config.Td;
            set => _config.Td = value;
        }

        public TimeSpan Tm
        {
            get => _config.Tm;
            set => _config.Tm = value;
        }

        public Employees Employees { get; }
        public TaskManager TaskManager { get; }

        public TechService(TechServiceConfig config = null)
        {
            _config = config ?? new TechServiceConfig();
            Employees = new Employees();
            TaskManager = new TaskManager(_config.TimeRange);
        }

        public void CreateTask(string description)
        {
            TaskManager.CreateTask(description);
        }
    }
}
