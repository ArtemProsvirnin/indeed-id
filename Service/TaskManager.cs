using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service
{
    public interface ITaskManager
    {
        event Notification OnNotification;
        event Cancelation OnCancelation;

        List<TechTask> InWork { get; }
        List<TechTask> Done { get; }
        List<TechTask> InQueue { get; }
        List<TechTask> All { get; }
        TechServiceConfig Config { get; }
        bool AllDone { get; }

        TechTask CreateTask(string description);
        void DeleteTask(int id);
        void DeleteTask(TechTask task);
        void DoneTask(TechTask t);
        void EnqueueTask(TechTask t);
    }

    public class TaskManager : ITaskManager
    {
        private Queue<TechTask> _queue = new Queue<TechTask>();
        private IdGenerator _idGenerator = new IdGenerator();
        private object _objectForLock = new object();

        public event Notification OnNotification;
        public event Cancelation OnCancelation;

        public List<TechTask> InWork { get; }
        public List<TechTask> Done { get; }
        public List<TechTask> InQueue { get => GetInQueue(); }
        public List<TechTask> All { get => GetAll(); }

        public TechServiceConfig Config { get; }
        public bool AllDone { get => InQueue.Count() == 0 && InWork.Count() == 0; }

        public TaskManager(TechServiceConfig config)
        {
            InWork = new List<TechTask>();
            Done = new List<TechTask>();

            Config = config;

            InfiniteWork();
        }

        private void InfiniteWork()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    var t = GetNextTask();

                    if (t == null)
                        await Task.Delay(500);
                    else
                        Notificate(t);
                }
            });
        }

        private void Notificate(TechTask task)
        {
            //Отменена, просто забываем о задаче
            if (task.Status == TechTaskStatus.Canceled)
                return;

            var args = new NotificationArgs(Config);

            OnNotification?.Invoke(task, args);

            if (args.Handled)
                InWork.Add(task);
            else
                EnqueueTask(task);
        }

        private TechTask GetNextTask()
        {
            lock (_objectForLock)
            {
                if (!_queue.Any())
                    return null;

                return _queue.Dequeue();
            }
        }

        public TechTask CreateTask(string description)
        {
            var task = new TechTask(description)
            {
                Id = _idGenerator.Next()
            };

            Notificate(task);

            return task;
        }

        public void DeleteTask(int id)
        {
            TechTask task = All.FirstOrDefault(t => t.Id == id);
            DeleteTask(task);
        }

        public void DeleteTask(TechTask task)
        {
            if (task == null)
                return;

            //Запрос выполнен, нельзя отменить
            if (task.Status == TechTaskStatus.Done)
                return;
            else
                task.Status = TechTaskStatus.Canceled;

            InWork.Remove(task);
            OnCancelation?.Invoke(task);
        }

        public void EnqueueTask(TechTask task)
        {
            if (task == null)
                return;

            task.Handler = null;

            lock (_objectForLock)
            {
                InWork.Remove(task);
                Done.Remove(task);
                _queue.Enqueue(task);
            }
        }

        public void DoneTask(TechTask task)
        {
            lock (_objectForLock)
            {
                InWork.Remove(task);
                Done.Add(task);
                task.Status = TechTaskStatus.Done;
            }
        }

        private List<TechTask> GetAll()
        {
            lock (_objectForLock)
            {
                return InWork.Concat(InQueue).Concat(Done).ToList();
            }
        }

        private List<TechTask> GetInQueue()
        {
            lock (_objectForLock)
            {
                return _queue.ToList();
            }
        }
    }
}
