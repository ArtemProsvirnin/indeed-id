using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public abstract class Employee
    {
        private TechTask _currentTask;

        protected TaskManager _taskManager { get; }

        public string Name { get; }
        
        internal Employee(string name, TaskManager manager)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Имя сотрудника не должно быть пустым");

            if (manager == null)
                throw new ArgumentException("TaskManager is null");

            Name = name;
            _taskManager = manager;
        }

        protected async void InfiniteWork()
        {
            while (true)
            {
                if (!await DoWork()) //Если нет подходящей задачи, то ждем
                    await Wait();
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
                await Task.Delay(_taskManager.TimeRange.Random());
                _taskManager.DoneTask(_currentTask);
                _currentTask = null;

                return true;
            });
        }

        protected abstract TechTask GetNextTask();
        protected abstract Task Wait();
    }

    public class Director: Employee
    {
        private TimeSpan _td;

        internal Director(string name, TaskManager manager, TimeSpan td): base(name, manager)
        {
            _td = td;
            InfiniteWork();// Запускаем бесконечный цикл обработки запросов
        }

        protected override TechTask GetNextTask()
        {
            return _taskManager.GetNextTask(_td);
        }

        protected override Task Wait()
        {
            return Task.Delay(5000);//todo время ожидания из конфига
        }
    }

    public class Manager : Employee
    {
        private TimeSpan _tm;

        internal Manager(string name, TaskManager manager, TimeSpan tm) : base(name, manager)
        {
            _tm = tm;
            InfiniteWork();// Запускаем бесконечный цикл обработки запросов
        }

        protected override TechTask GetNextTask()
        {
            return _taskManager.GetNextTask(_tm);
        }

        protected override Task Wait()
        {
            return Task.Delay(1000);//todo время ожидания из конфига
        }
    }

    public class Operator : Employee
    {
        internal Operator(string name, TaskManager manager) : base(name, manager)
        {
            InfiniteWork();// Запускаем бесконечный цикл обработки запросов
        }

        protected override TechTask GetNextTask()
        {
            return _taskManager.GetNextTask(TimeSpan.Zero);
        }

        protected override Task Wait()
        {
            return Task.Delay(500);//todo время ожидания из конфига
        }
    }
}
