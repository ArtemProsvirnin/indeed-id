using System;
using System.Threading.Tasks;

namespace Service
{
    public abstract class Employee
    {
        private TechTask _currentTask;

        protected TaskManager TaskManager { get; }

        public string Name { get; }
        public bool IsBusy { get => _currentTask != null; }

        internal Employee(string name, TaskManager manager)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Имя сотрудника не должно быть пустым");

            if (manager == null)
                throw new ArgumentException("TaskManager is null");

            Name = name;
            TaskManager = manager;

            InfiniteWork();// Запускаем бесконечный цикл обработки запросов
        }

        protected async void InfiniteWork()
        {
            while (true)
            {
                if (!await DoWork()) //Если нет подходящей задачи, то ждем
                    await Task.Delay(GetWaitTime());
            }
        }

        private async Task<bool> DoWork()
        {
            _currentTask = GetNextTask();

            if (_currentTask == null)
                return false;

            _currentTask.Handler = this;

            return await Task.Run(async () =>
            {
                await Task.Delay(TaskManager.Config.TimeRange.Random());
                TaskManager.DoneTask(_currentTask);
                _currentTask = null;

                return true;
            });
        }

        protected abstract TechTask GetNextTask();
        protected abstract TimeSpan GetWaitTime();
    }

    public class Director: Employee
    {
        internal Director(string name, TaskManager manager): base(name, manager) { }

        protected override TechTask GetNextTask()
        {
            TimeSpan delay = GetWaitTime();
            return TaskManager.GetNextTask(delay);
        }

        protected override TimeSpan GetWaitTime()
        {
            return TaskManager.Config.Td;
        }
    }

    public class Manager : Employee
    {        
        internal Manager(string name, TaskManager manager) : base(name, manager) { }

        protected override TechTask GetNextTask()
        {
            TimeSpan delay = GetWaitTime();
            return TaskManager.GetNextTask(delay);
        }

        protected override TimeSpan GetWaitTime()
        {
            return TaskManager.Config.Tm;
        }
    }

    public class Operator : Employee
    {
        internal Operator(string name, TaskManager manager) : base(name, manager) { }

        protected override TechTask GetNextTask()
        {
            return TaskManager.GetNextTask(TimeSpan.Zero);
        }

        protected override TimeSpan GetWaitTime()
        {
            return TimeSpan.FromSeconds(0.5);
        }
    }
}
