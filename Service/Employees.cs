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
