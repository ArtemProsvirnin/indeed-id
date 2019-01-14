using System;
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    public abstract class Employee
    {
        private TechTask _currentTask;
        private CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();
        private ITaskManager _taskManager;

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
            _taskManager = manager;

            Suscribe();
        }

        private void Suscribe()
        {
            _taskManager.OnNotification += HandleTask;
            _taskManager.OnCancelation += CancelTask;
        }

        private void Unsuscribe()
        {
            _taskManager.OnNotification -= HandleTask;
            _taskManager.OnCancelation -= CancelTask;
        }

        private void HandleTask(TechTask task, NotificationArgs args)
        {
            if (!Check(task, args))
                return;

            args.Handled = true;
            _currentTask = task;
            _currentTask.Handler = this;

            Task.Run(() => HandleTask(task, args.Config.TimeRange));
        }

        private bool Check(TechTask task, NotificationArgs args)
        {
            if (args.Handled || IsBusy)
                return false;

            return CheckTaskTime(task, args.Config);
        }

        private async Task HandleTask(TechTask task, TimeRange timeRange)
        {
            TimeSpan delay = timeRange.Random();

            await Task.Delay(delay, _cancelTokenSource.Token)
                .ContinueWith(ResolveTask);
        }

        private void ResolveTask(Task t)
        {
            if (t.IsCanceled)
                _taskManager.EnqueueTask(_currentTask);
            else
                _taskManager.DoneTask(_currentTask);

            _currentTask = null;
        }

        private void CancelTask(TechTask task)
        {
            if (task == null || task != _currentTask)
                return;

            _currentTask = null;
            _cancelTokenSource.Cancel();
        }

        internal virtual void YouFired()
        {
            Unsuscribe();
            _cancelTokenSource.Cancel();
        }

        protected abstract bool CheckTaskTime(TechTask task, TechServiceConfig config);
        protected abstract string GetPositionName();
    }

    public class Director : Employee
    {
        internal Director(string name, ITaskManager manager) : base(name, manager) { }

        protected override bool CheckTaskTime(TechTask task, TechServiceConfig config)
        {
            return task.TimeSpent > config.Td;
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

        protected override bool CheckTaskTime(TechTask task, TechServiceConfig config)
        {
            return task.TimeSpent > config.Tm;
        }

        protected override string GetPositionName()
        {
            return "Manager";
        }
    }

    public class Operator : Employee
    {
        internal Operator(string name, ITaskManager manager) : base(name, manager) { }

        protected override bool CheckTaskTime(TechTask task, TechServiceConfig config)
        {
            return true;
        }

        protected override string GetPositionName()
        {
            return "Operator";
        }
    }
}