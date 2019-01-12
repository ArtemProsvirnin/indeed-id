using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class EmployeeFactory
    {
        private TechService service;

        public EmployeeFactory(TechService service)
        {
            this.service = service;
        }

        public Director CreateDirector(string name)
        {
            var d = new Director(name, service.TaskManager, service.Td);
            service.Employees.Add(d);

            return d;
        }

        public Manager CreateManager(string name)
        {
            var m = new Manager(name, service.TaskManager, service.Tm);
            service.Employees.Add(m);

            return m;
        }

        public Operator CreateOperator(string name)
        {
            var o = new Operator(name, service.TaskManager);            
            service.Employees.Add(o);

            return o;
        }
    }
}
