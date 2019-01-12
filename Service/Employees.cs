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
        public List<Director> Directors { get; }
        public List<Manager> Managers { get; }
        public List<Operator> Operators { get; }

        public int Count { get => Directors.Count + Managers.Count + Operators.Count; }

        internal Employees()
        {
            Directors = new List<Director>();
            Managers = new List<Manager>();
            Operators = new List<Operator>();
        }

        public IEnumerator<Employee> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Employee>)this).GetEnumerator();
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
    }
}
