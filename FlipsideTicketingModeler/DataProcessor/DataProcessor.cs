using System;
using System.Collections.Generic;

namespace FlipsideTicketingModeler.DataProcessor
{
    /**
     * @brief Processes ticketing history data to make ticketing outcome predictions about subsequent years.
     */
    public class DataProcessor : OutputShim.IOutputShim
    {
        // The configuration data for this application.
        private Configuration.Configuration _configuration;

        // The object that collects input data from outside the application
        private InputShim.IInputShim _inputShim;

        // All of the different objects that will receive output from this application.
        private List<OutputShim.IOutputShim> _outputShims;

        // Can load cached database data off disk.
        private Data.DatabaseCache _databaseCache;

        /**
         * @constructor
         * @param configuration - Contains configuration data for this applications.
         * @param inputShim - Provides input data to this application.
         * @param outputShims - A list of the various objects that will consume the outputs from this processor.
         * @param databaseCache - Able to load cached database data off disk.
         */
        public DataProcessor(Configuration.Configuration configuration, InputShim.IInputShim inputShim, List<OutputShim.IOutputShim> outputShims, Data.DatabaseCache databaseCache)
        {
            // Cache inputs.
            _configuration = configuration;
            _inputShim = inputShim;
            _outputShims = outputShims;
            _databaseCache = databaseCache;
        }

        /**
         * @method Perform data processing.
         * @params None
         * @returns None; output will be generated as a result of this activity.
         */
        public void Run()
        {
            // Try to load our database from the on-disk cache.
            Data.Database database = _databaseCache.Load();

            // Did we succeed?  If not, load it manually via a connection to the database.
            if (database == null)
            {
                // Create a database to populate.
                database = new Data.Database(_configuration);

                Console.WriteLine("Gathering input data...");

                // Populate the database using our input shim.
                _inputShim.SetDatabase(database);
                _inputShim.Run();

                Console.WriteLine("All input data gathered.");

                // Save cache.
                _databaseCache.Save(database);
            }
            else
            {
                Console.WriteLine("Loaded cached input data from " + _databaseCache.Name);
            }

            // Create the object capable of projecting ticket scenarios into the future.
            Projector projector = new Projector(_configuration, database);

            // Start data processing.
            Start();

            // Which scenarios will we test?
            List<ProjectionScenario> projectionScenarios = _configuration.GetProjectionScenarios();
            foreach (ProjectionScenario projectionScenario in projectionScenarios)
            {
                // Project this scenario.
                ProjectedFuture projectionFuture = projector.Project(projectionScenario);

                // Inform our output shims of the result of this projection.
                OnProjectionScenarioCompleted(projectionScenario, projectionFuture);
            }

            // Finish data processing.
            End();
        }

        // IOutputShim

        public void Start()
        {
            // Pass the message to all output shims.
            foreach (OutputShim.IOutputShim outputShim in _outputShims)
            {
                outputShim.Start();
            }
        }

        public void OnProjectionScenarioCompleted(ProjectionScenario scenario, ProjectedFuture future)
        {
            // Pass the message to all output shims.
            foreach (OutputShim.IOutputShim outputShim in _outputShims)
            {
                outputShim.OnProjectionScenarioCompleted(scenario, future);
            }
        }

        public void End()
        {
            // Pass the message to all output shims.
            foreach (OutputShim.IOutputShim outputShim in _outputShims)
            {
                outputShim.End();
            }
        }
    }
}
