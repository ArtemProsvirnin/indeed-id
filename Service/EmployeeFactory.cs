using System;

namespace Service
{
    public class EmployeeFactory
    {
        private TechService _service;
        private IdGenerator _idGenerator = new IdGenerator();

        public EmployeeFactory(TechService service)
        {
            _service = service;
        }

        public Employee CreateDirector(string name)
        {
            var d = new Director(name, _service.TaskManager);
            _service.Employees.Add(d);
            return GenerateId(d);
        }

        public Employee CreateManager(string name)
        {
            var m = new Manager(name, _service.TaskManager);
            _service.Employees.Add(m);
            return GenerateId(m);
        }

        public Employee CreateOperator(string name)
        {
            var o = new Operator(name, _service.TaskManager);
            _service.Employees.Add(o);
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
