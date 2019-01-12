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
            Instance = new TechService();
        }
    }
}