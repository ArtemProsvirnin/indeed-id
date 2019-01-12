using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public interface ITaskManager
    {
        void CreateTask(string description);
        TechTask GetNextTask(TimeSpan timeSpanCondition);
        void DoneTask(TechTask t);

        List<TechTask> InWork { get; }
        List<TechTask> Done { get; }
        IEnumerable<TechTask> InQueue { get; }
    }

    public class TaskManager: ITaskManager
    {
        private Queue<TechTask> _queue = new Queue<TechTask>();
        private object _objectForLock = new object();

        public List<TechTask> InWork { get; }
        public List<TechTask> Done { get; }

        public IEnumerable<TechTask> InQueue { get => _queue; }
        public TechServiceConfig Config { get; internal set; }

        public bool AllDone { get => InQueue.Count() == 0 && InWork.Count() == 0; }

        public TaskManager(TechServiceConfig config)
        {
            InWork = new List<TechTask>();
            Done = new List<TechTask>();

            Config = config;
        }

        public void CreateTask(string description)
        {
            var task = new TechTask(description);
            _queue.Enqueue(task);
        }

        public TechTask GetNextTask(TimeSpan timeSpanCondition)
        {
            lock (_objectForLock)
            {
                if (!_queue.Any())
                    return null;

                if (_queue.Peek().TimeSpent < timeSpanCondition) //Время еще не пришло
                    return null;

                var t = _queue.Dequeue();
                InWork.Add(t);

                return t;
            }
        }
        
        public void DoneTask(TechTask task)
        {
            task.Status = TechTaskStatus.Done;
            InWork.Remove(task);
            Done.Add(task);
        }
    }
}
