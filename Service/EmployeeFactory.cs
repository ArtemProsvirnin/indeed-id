namespace Service
{
    public class EmployeeFactory
    {
        private TechService _service;

        public EmployeeFactory(TechService service)
        {
            _service = service;
        }

        public Employee CreateDirector(string name)
        {
            var d = new Director(name, _service.TaskManager);
            _service.Employees.Add(d);
            return d;
        }

        public Employee CreateManager(string name)
        {
            var m = new Manager(name, _service.TaskManager);
            _service.Employees.Add(m);
            return m;
        }

        public Employee CreateOperator(string name)
        {
            var o = new Operator(name, _service.TaskManager);
            _service.Employees.Add(o);
            return o;
        }
    }
}
