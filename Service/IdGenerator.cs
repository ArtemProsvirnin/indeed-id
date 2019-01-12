using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal class IdGenerator
    {
        private int _id;

        public int Next()
        {
            return _id++;
        }
    }
}
