using System;
using System.Collections.Generic;

namespace FlipsideTicketingModeler
{
    /**
     * @brief Entry point
     */
    class Program
    {
        // The name of this program.
        public static readonly string AppName = "Flipside Ticketing Modeler";

        /**
         * @CLI arguments:
         * 
         * -v or --version : Display version and exit
         * -u or --usage : Display usage string and exit
         * -h or --help : Display help text and exit
         * -t : Run application in test mode (do not connect to a live database)
         * -i : If this is specified, any caches of databases will be invalidated.
         */
        static void Main(string[] args)
        {
            // Show help string
            if (args.Length > 0 && (args[0].Equals("-h") || args[0].Equals("--help")))
            {
                // Write app name and version string
                Console.WriteLine("\n" + AppName + " " + Version.VersionString);

                // Write command line parameters.
                Console.WriteLine("Command line parameters:\n");
                Console.WriteLine("-v or --version : Display version and exit");
                Console.WriteLine("-u or --usage : Display usage string and exit");
                Console.WriteLine("-h or --help : Display this help text and exit");
                Console.WriteLine("-t : Run the application in test mode (do not connect to a database)");
                Console.WriteLine("-i : Invalidates any cached database information on the local disk");

                // Write usage string
                Console.WriteLine("\n" + Usage.UsageString);
                return;
            }

            // Print version string.
            if (args.Length > 0 && (args[0].Equals("-v") || args[0].Equals("--version")))
            {
                Console.WriteLine("\n" + AppName + " " + Version.VersionString);
                return;
            }

            // Print usage string.
            if (args.Length > 0 && (args[0].Equals("-u") || args[0].Equals("--usage")))
            {
                Console.WriteLine("\n" + Usage.UsageString);
                return;
            }

            // Gather command line directives.
            bool isInTestMode = false;
            bool invalidateCache = false;
            foreach (string arg in args)
            {
                if (arg.Equals("-t"))
                {
                    isInTestMode = true;
                }
                else if (arg.Equals("-i"))
                {
                    invalidateCache = true;
                }
            }

            // Read our configuration file or create an empty configuration.
            Configuration.Configuration configuration = Configuration.Configuration.Load("config.json") ?? new Configuration.Configuration();

            // Create our desired input shim.
            InputShim.IInputShim inputShim = null;
            Data.DatabaseCache databaseCache = null;
            if (isInTestMode)
            {
                // Consume only mocked data.
                inputShim = new InputShim.TestInputShim(configuration);
                databaseCache = new Data.DatabaseCache(configuration, "Test");
            }
            else
            {
                // Pull live data from our database.
                inputShim = new InputShim.FlipsideDatabaseInputShim(configuration);
                databaseCache = new Data.DatabaseCache(configuration, "Production");
            }

            // If we need to invalidate the database cache, do it now.
            if (invalidateCache)
            {
                databaseCache.Invalidate();
            }

            // Created output shims.
            List<OutputShim.IOutputShim> outputShims = new List<OutputShim.IOutputShim>();
            if (isInTestMode)
            {
                // Use test shims.
                outputShims.Add(new OutputShim.ConsoleOutputShim());
            }
            else
            {
                // Use real shims.
                outputShims.Add(new OutputShim.ConsoleOutputShim());
            }

            // Build the object that will process our data.
            DataProcessor.DataProcessor dataProcessor = new DataProcessor.DataProcessor(configuration, inputShim, outputShims, databaseCache);

            // And process it.
            dataProcessor.Run();
        }
    }
}
