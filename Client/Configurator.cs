using System;
using System.Configuration;

namespace Client
{
    internal class Configurator
    {
        private string[] _args;

        public Configurator(string[] args)
        {
            _args = args;
        }

        internal Config Config()
        {
            var config =  new Config() { Host = ConfigurationManager.AppSettings["Host"] };
            ApplyParameters(config);
            return config;
        }

        private void ApplyParameters(Config config)
        {
            int length = _args.Length;

            if (length == 0)
                return;

            ApplyTimeRange(config);

            if (length > 1)
                ApplyRequestCount(config);
        }

        private void ApplyTimeRange(Config config)
        {
            config.TimeRange = ApplyParameter(_args[0], nameof(config.TimeRange), config.TimeRange);
        }

        private void ApplyRequestCount(Config config)
        {
            config.RequestCount = ApplyParameter(_args[1], nameof(config.RequestCount), config.RequestCount);
        }

        private int ApplyParameter(string value, string name, int parameter)
        {
            int initial = parameter;

            if (!int.TryParse(value, out parameter) || parameter < 0)
            {
                Program.WriteLine($"Invalid parameter {name}, default value is used", ConsoleColor.DarkRed);
                return initial;
            }

            return parameter;
        }
    }

    internal class Config
    {
        public int TimeRange { get; set; }
        public int RequestCount { get; set; }
        public string Host { get; set; }

        public Config()
        {
            TimeRange = 5000; //milliseconds
            RequestCount = -1;
        }
    }
}
