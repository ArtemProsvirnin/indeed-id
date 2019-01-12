using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class TechServiceConfig
    {
        public TimeRange TimeRange { get; set; }
        public TimeSpan Tm { get; set; }
        public TimeSpan Td { get; set; }

        public TechServiceConfig()
        {
            TimeRange = new TimeRange(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(2));
            Tm = TimeSpan.FromSeconds(5);
            Td = TimeSpan.FromSeconds(10);
        }
    }
}
