using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public abstract class Employee
    {
        private TechTask _task;

        public string Name { get; set; }
        public bool IsBusy { get => _task != null; }
        public TasksManager TaskManager { get; set; }

        internal Employee(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Имя сотрудника не должно быть пустым");

            Name = name;
        }

        public void Process(TechTask task, TimeRange range)
        {
            if (TaskManager == null)
                throw new NullReferenceException("TaskManager is null");

            if (IsBusy)
                throw new Exception("Employee is busy");

            _task = task;
            _task.Handler = this;

            Task.Run(async () =>
            {
                await Task.Delay(range.Random());
                TaskManager.DoneTask(_task);
                SpecialProcess();
            });
        }

        protected abstract void SpecialProcess();
    }

    public class Director: Employee
    {
        internal Director(string name): base(name)
        {

        }

        protected override void SpecialProcess()
        {
            //Разобраться с менеджерами
        }
    }

    public class Manager : Employee
    {
        internal Manager(string name) : base(name)
        {

        }

        protected override void SpecialProcess()
        {
            //Разобраться с операторами
        }
    }

    public class Operator : Employee
    {
        internal Operator(string name) : base(name)
        {

        }

        protected override void SpecialProcess()
        {
            //Отчитаться о сделанной работе
        }
    }
}
