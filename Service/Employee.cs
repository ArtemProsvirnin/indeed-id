using System;
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    public abstract class Employee
    {
        private TechTask _currentTask;
        private CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();
        private bool _fired = false;

        protected ITaskManager TaskManager { get; }

        public int Id { get; internal set; }
        public string Name { get; }
        public bool IsBusy { get => _currentTask != null; }
        public string PositionName { get => GetPositionName(); }

        internal Employee(string name, ITaskManager manager)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Имя сотрудника не должно быть пустым");

            if (manager == null)
                throw new ArgumentException("TaskManager is null");

            Name = name;
            TaskManager = manager;

            InfiniteWork();// Запускаем цикл обработки запросов
        }

        internal virtual void YouFired()
        {
            _fired = true;
            _cancelTokenSource.Cancel();
        }

        protected void InfiniteWork()
        {
            Task.Run(async () =>
            {
                while (!_fired)
                {
                    if (!await DoWork()) //Если нет подходящей задачи, то ждем
                        await Task.Delay(GetWaitTime());
                }
            });
        }

        private async Task<bool> DoWork()
        {
            _currentTask = GetNextTask();

            if (_currentTask == null)
                return false;

            _currentTask.Handler = this;

            return await Task.Run(async () =>
            {
                await HandlerTask();
                return true;
            });
        }

        private async Task HandlerTask()
        {
            TimeSpan delay = TaskManager.Config.TimeRange.Random();

            await Task.Delay(delay, _cancelTokenSource.Token)
                .ContinueWith(ResolveTask);

            _currentTask = null;
        }

        private void ResolveTask(Task t)
        {
            //Если выполенение запроса отменено, то возвращаем его в очередь, иначе сообщаем о выполнении

            if (t.IsCanceled)
                TaskManager.EnqueueTask(_currentTask);
            else
                TaskManager.DoneTask(_currentTask);
            
        }

        protected abstract TechTask GetNextTask();
        protected abstract TimeSpan GetWaitTime();
        protected abstract string GetPositionName();
    }

    public class Director : Employee
    {
        internal Director(string name, ITaskManager manager) : base(name, manager) { }

        protected override TechTask GetNextTask()
        {
            TimeSpan delay = GetWaitTime();
            return TaskManager.GetNextTask(delay);
        }

        protected override TimeSpan GetWaitTime()
        {
            return TaskManager.Config.Td;
        }

        protected override string GetPositionName()
        {
            return "Director";
        }

        internal override void YouFired()
        {
            //Директора просто так не уволить...
        }
    }

    public class Manager : Employee
    {
        internal Manager(string name, ITaskManager manager) : base(name, manager) { }

        protected override TechTask GetNextTask()
        {
            TimeSpan delay = GetWaitTime();
            return TaskManager.GetNextTask(delay);
        }

        protected override TimeSpan GetWaitTime()
        {
            return TaskManager.Config.Tm;
        }

        protected override string GetPositionName()
        {
            return "Manager";
        }
    }

    public class Operator : Employee
    {
        internal Operator(string name, ITaskManager manager) : base(name, manager) { }

        protected override TechTask GetNextTask()
        {
            return TaskManager.GetNextTask(TimeSpan.Zero);
        }

        protected override TimeSpan GetWaitTime()
        {
            return TimeSpan.FromSeconds(0.5);
        }

        protected override string GetPositionName()
        {
            return "Operator";
        }
    }
}