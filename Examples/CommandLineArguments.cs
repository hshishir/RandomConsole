using CommandLine;
using System;

namespace RandomConsole.Examples
{
    public class CommandLineArguments
    {
        /// <summary>
        /// https://github.com/gsscoder/commandline/wiki/Latest-Version#parsing
        /// </summary>
        public class Options
        {
            [Option("fname", Required = true, HelpText = "First name of person")]
            public string FirstName { get; set; }

            [Option("lname", Required = true, HelpText = "Last name of person")]
            public string LastName { get; set; }

            [Option("gender", Required = true, HelpText = "Gender of person")]
            public string Gender { get; set; }

            [Option("age", Required = false, HelpText = "Age of person")]
            public int Age { get; set; }

            [Option("monitorddrit", Required = false, HelpText = "Monitor DDRITs", Default = false)]
            public bool MonitorDDRITs { get; set; }

            [Option("createmessage", Required = false, HelpText = "Create build fabric message", Default = false)]
            public bool CreateMessage { get; set; }
        }

        private const int SuccessExitCode = 0;
        private const int FailureExitCode = -1;

        //private static int Main(string[] args)
        //{
        //    return Parser.Default.ParseArguments<Options>(args)
        //        .MapResult(
        //        options => DoWork(options),
        //        // If options passed doesn't match any of the options above return FailureExitCode
        //        _ => FailureExitCode
        //        );
        //}

        private static int DoWork(Options options)
        {
            try
            {
                Console.WriteLine($"Hello {options.FirstName} {options.LastName}. Your gender is {options.Gender}");

                if(options.MonitorDDRITs)
                {
                    Console.WriteLine("MonitorDDRITs stuff");
                }

                if(options.CreateMessage)
                {
                    Console.WriteLine("CreateMessage stuff");
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
