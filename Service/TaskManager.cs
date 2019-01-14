using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public interface ITaskManager
    {
        List<TechTask> InWork { get; }
        List<TechTask> Done { get; }
        IEnumerable<TechTask> InQueue { get; }
        IEnumerable<TechTask> All { get; }
        TechServiceConfig Config { get; }
        bool AllDone { get; }

        TechTask CreateTask(string description);
        void DeleteTask(int id);
        void DeleteTask(TechTask task);
        TechTask GetNextTask(TimeSpan timeSpanCondition);
        void DoneTask(TechTask t);
        void EnqueueTask(TechTask t);
    }

    public class TaskManager : ITaskManager
    {
        private Queue<TechTask> _queue = new Queue<TechTask>();
        private IdGenerator _idGenerator = new IdGenerator();
        private object _objectForLock = new object();

        public List<TechTask> InWork { get; }
        public List<TechTask> Done { get; }

        public IEnumerable<TechTask> InQueue { get => _queue; }
        public TechServiceConfig Config { get; }

        public IEnumerable<TechTask> All { get => InWork.Concat(InQueue).Concat(Done).Where(t => t.Status != TechTaskStatus.Canceled); }
        public bool AllDone { get => InQueue.Count() == 0 && InWork.Count() == 0; }

        public TaskManager(TechServiceConfig config)
        {
            InWork = new List<TechTask>();
            Done = new List<TechTask>();

            Config = config;
        }

        public TechTask CreateTask(string description)
        {
            var task = new TechTask(description)
            {
                Id = _idGenerator.Next()
            };

            EnqueueTask(task);

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

            if (task.Status == TechTaskStatus.Done) //Запрос выполнен, нельзя отменить
                return;
            
            task.Status = TechTaskStatus.Canceled;
        }

        public void EnqueueTask(TechTask t)
        {
            lock (_objectForLock)
            {
                if (InWork.Contains(t))
                {
                    InWork.Remove(t);
                    t.Handler = null;
                }

                _queue.Enqueue(t);
            }
        }

        public TechTask GetNextTask(TimeSpan timeSpanCondition)
        {
            TechTask task;

            lock (_objectForLock)
            {
                if (!_queue.Any())
                    return null;

                if (_queue.Peek().TimeSpent < timeSpanCondition) //Время еще не пришло
                    return null;

                task = _queue.Dequeue();
            }

            if (task.Status == TechTaskStatus.Canceled) //Отменен, берем следующий
                return GetNextTask(timeSpanCondition);

            InWork.Add(task);

            return task;
        }

        public void DoneTask(TechTask task)
        {
            lock (_objectForLock)
            {
                InWork.Remove(task);

                if (task.Status == TechTaskStatus.Canceled) //Отменен, просто забываем
                    return;

                task.Status = TechTaskStatus.Done;
                Done.Add(task);
            }
        }
    }
}
