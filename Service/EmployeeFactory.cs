using System;

namespace Service
{
    internal class EmployeeFactory
    {
        private Employees _employees;
        private ITaskManager _taskManager;
        private IdGenerator _idGenerator = new IdGenerator();

        public EmployeeFactory(Employees employees, ITaskManager taskManager)
        {
            _employees = employees;
            _taskManager = taskManager;
        }

        public Employee CreateDirector(string name)
        {
            var d = new Director(name, _taskManager);
            _employees.Add(d);
            return GenerateId(d);
        }

        public Employee CreateManager(string name)
        {
            var m = new Manager(name, _taskManager);
            _employees.Add(m);
            return GenerateId(m);
        }

        public Employee CreateOperator(string name)
        {
            var o = new Operator(name, _taskManager);
            _employees.Add(o);
            return GenerateId(o);
        }

        private Employee GenerateId(Employee e)
        {
            e.Id = _idGenerator.Next();
            return e;
        }

        public Employee CreateByPositionName(string position, string name)
        {
            switch (position.ToLower())
            {
                case "operator":
                    return CreateOperator(name);
                case "manager":
                    return CreateManager(name);
                case "director":
                    return CreateDirector(name);
            }

            throw new ArgumentException("Unknown position");
        }
    }
}
