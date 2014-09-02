using System;
using NLog;

namespace ConsoleClient
{
    class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        static void Main()
        {
            Console.WriteLine("Press <ENTER> to create errors...");
            Console.ReadLine();

            for (int i = 0; i < 50; i++)
            {
                var exception = new InvalidOperationException("OH NO!!");
                Logger.ErrorException("CONSOLE -- Oh no!", exception);
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }
    }
}
