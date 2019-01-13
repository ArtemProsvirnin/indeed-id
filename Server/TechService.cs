using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Service;

namespace Server
{
    public class TechServiceSingleton
    {
        public static TechService Instance { get; }

        protected TechServiceSingleton() { }

        static TechServiceSingleton()
        {
            var config = new TechServiceConfig()
            {
                TimeRange = new TimeRange(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(60)),
                Tm = TimeSpan.FromSeconds(10),
                Td = TimeSpan.FromSeconds(20)
            };

            Instance = new TechService(config);
        }
    }
}