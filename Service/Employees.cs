using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class Employees : IEnumerable<Employee>
    {
        private EmployeeFactory _factory;

        public List<Director> Directors { get; }
        public List<Manager> Managers { get; }
        public List<Operator> Operators { get; }

        public int Count { get => Directors.Count + Managers.Count + Operators.Count; }

        internal Employees(ITaskManager taskManager)
        {
            Directors = new List<Director>();
            Managers = new List<Manager>();
            Operators = new List<Operator>();

            _factory = new EmployeeFactory(this, taskManager);
        }

        public IEnumerator<Employee> GetEnumerator()
        {
            foreach (var d in Directors)
                yield return d;

            foreach (var m in Managers)
                yield return m;

            foreach (var o in Operators)
                yield return o;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Employee>)this).GetEnumerator();
        }

        public void Remove(int id)
        {
            Employee employee = this.FirstOrDefault(e => e.Id == id);
            
            if(employee != null)
                Remove(employee);
        }

        public void Remove(Employee employee)
        {
            switch (employee)
            {
                case Director d:
                    Directors.Remove(d);
                    break;
                case Manager m:
                    Managers.Remove(m);
                    break;
                case Operator o:
                    Operators.Remove(o);
                    break;
            }

            employee.YouFired();
        }

        internal void Add(Director d)
        {
            Directors.Add(d);
        }

        internal void Add(Manager m)
        {
            Managers.Add(m);
        }

        internal void Add(Operator o)
        {
            Operators.Add(o);
        }

        public Employee CreateDirector(string name)
        {
            return _factory.CreateDirector(name);
        }

        public Employee CreateManager(string name)
        {
            return _factory.CreateManager(name);
        }

        public Employee CreateOperator(string name)
        {
            return _factory.CreateOperator(name);
        }

        public Employee CreateByPositionName(string position, string name)
        {
            return _factory.CreateByPositionName(position, name);
        }
    }
}
