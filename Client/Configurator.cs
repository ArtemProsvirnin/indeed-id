using System;

namespace Client
{
    internal class Configurator
    {
        private string[] _args;

        public Configurator(string[] args)
        {
            _args = args;
        }

        internal bool Config()
        {
            int length = _args.Length;

            if (length == 0)
                return true;

            if (length == 1)
                return ApplyTimeRange(_args[0]);

            return ApplyTimeRange(_args[0]) && ApplyRequestCount(_args[1]);
        }

        private static bool ApplyTimeRange(string value)
        {
            return ApplyParameter(value, nameof(Program.TimeRange), out Program.TimeRange);
        }
        
        private static bool ApplyRequestCount(string value)
        {
            return ApplyParameter(value, nameof(Program.RequestCount), out Program.RequestCount);
        }

        private static bool ApplyParameter(string value, string name, out int parameter)
        {
            if (!int.TryParse(value, out parameter) || parameter < 0)
            {
                Console.WriteLine($"Invalid parameter {name}");
                return false;
            }

            return true;
        }
    }
}

