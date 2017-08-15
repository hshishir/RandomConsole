using System;
using System.IO;
using CommandLine;

namespace RandomConsole
{
    public class Program
    {
        /// <summary>
        /// https://github.com/gsscoder/commandline/wiki/Latest-Version#parsing
        /// </summary>
        public class Options
        {
            [Option('m', "monitorddrit", Required = false, HelpText = "Monitor DDRITs", Default = false)]
            public bool MonitorDDRITs { get; set; }

            [Option('c', "createmessage", Required = false, HelpText = "Create build fabric message", Default = false)]
            public bool CreateMessage { get; set; }

            [Option('n', "name", Required = false, HelpText = "Full name", Default = "")]
            public string Name { get; set; }
        }

        private const int SuccessExitCode = 0;
        private const int FailureExitCode = -1;

        private static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<Options>(args)
                .MapResult(
                options => DoWork(options),
                // If options passed doesn't match any of the options above return FailureExitCode
                _ => FailureExitCode
                );
        }

        private static int DoWork(Options options)
        {
            Console.WriteLine("DoWork!!");
            try
            {
                if(options.MonitorDDRITs)
                {
                    Console.WriteLine("Monitor DDRITS work");
                    if (!string.IsNullOrWhiteSpace(options.Name))
                    {
                        Console.WriteLine($"Name: {options.Name}");
                    }
                }

                if (options.CreateMessage)
                {
                    Console.WriteLine("Create Meesage work");
                }

                return SuccessExitCode;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception {e}");
                return FailureExitCode;
            }
        }
    }
}
