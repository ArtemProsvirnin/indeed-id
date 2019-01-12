using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal class ChainOfHandlers
    {
        private TimeSpan _timeCondition;
        private IEnumerable<Employee> _employees;

        public ChainOfHandlers Next { get; set; }

        public ChainOfHandlers(IEnumerable<Employee> employees, TimeSpan timeCondition)
        {
            _timeCondition = timeCondition;
            _employees = employees;
        }

        public bool Process(TechTask task, TimeRange range)
        {
            if (task.TimeSpent > _timeCondition)
                InternalProcess(task, range);

            if (Next != null)
                return Next.Process(task, range);

            return false;
        }

        private bool InternalProcess(TechTask task, TimeRange range)
        {
            var handler = GetFreeEmployee();

            if (handler == null)
                return false;

            handler.Process(task, range);

            return true;
        }

        public Employee GetFreeEmployee()
        {
            return _employees.FirstOrDefault(e => !e.IsBusy);
        }
    }
}
