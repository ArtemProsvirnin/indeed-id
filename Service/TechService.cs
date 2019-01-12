﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Service
{
    public class TechService
    {
        public TechServiceConfig Config { get; private set; }
        
        public TimeSpan MinTime {
            get => Config.TimeRange.MinTime;
            set => Config.TimeRange.MinTime = value;
        }

        public TimeSpan MaxTime
        {
            get => Config.TimeRange.MaxTime;
            set => Config.TimeRange.MaxTime = value;
        }

        public TimeSpan Td
        {
            get => Config.Td;
            set => Config.Td = value;
        }

        public TimeSpan Tm
        {
            get => Config.Tm;
            set => Config.Tm = value;
        }

        public Employees Employees { get; }
        public TaskManager TaskManager { get; }

        public TechService(TechServiceConfig config = null)
        {
            Config = config ?? new TechServiceConfig();
            Employees = new Employees();
            TaskManager = new TaskManager(Config);
        }

        public void CreateTask(string description)
        {
            TaskManager.CreateTask(description);
        }
    }
}
