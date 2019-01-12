using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public interface ITasksManager
    {
        void CreateTask(string description);
        TechTask GetNextTask();
        void DoneTask(TechTask t);

        List<TechTask> InWork { get; }
        List<TechTask> Done { get; }
        IEnumerable<TechTask> InQueue { get; }
    }

    public class TasksManager: ITasksManager
    {
        private Queue<TechTask> _queue = new Queue<TechTask>();

        public List<TechTask> InWork { get; }
        public List<TechTask> Done { get; }

        public IEnumerable<TechTask> InQueue { get => _queue; }

        public TasksManager()
        {
            InWork = new List<TechTask>();
            Done = new List<TechTask>();
        }

        public void CreateTask(string description)
        {
            var task = new TechTask(description);
            _queue.Enqueue(task);
        }

        public TechTask GetNextTask()
        {
            if (!_queue.Any())
                return null;

            var t = _queue.Dequeue();
            InWork.Add(t);

            return t;
        }

        public void DoneTask(TechTask task)
        {
            task.Status = TechTaskStatus.Done;
            InWork.Remove(task);
            Done.Add(task);
        }
    }
}
