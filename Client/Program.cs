using System;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = new Configurator(args);
            var config = c.Config();
            
            var tester = new HttpTester(config.Host);
            tester.Run(config.RequestCount, config.TimeRange);

            Console.ReadLine();
        }

        public static void WriteLine(string message, ConsoleColor? color = null)
        {
                var old = Console.ForegroundColor;
                Console.ForegroundColor = color ?? Console.ForegroundColor;
                Console.WriteLine(message);
                Console.ForegroundColor = old;
        }
    }
}
