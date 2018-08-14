using System;

namespace FlipsideTicketingModeler.InputShim
{
    /**
     * @brief Instantiates a live connection to the flipside ticketing database to pull information about users, tickets and years.
     */
    public class FlipsideDatabaseInputShim : IInputShim
    {
        // The database that we will populate.
        private Data.Database _database;

        /**
         * @constructor
         * @param configuration - Contains configuration information for this application.
         */
        public FlipsideDatabaseInputShim(Configuration.Configuration configuration)
        {
            // Left blank purposefully.
        }

        // IInputShim

        public void SetDatabase(Data.Database database)
        {
            _database = database;
        }

        public void Run()
        {
            Console.WriteLine("Live database connection is currently unsupported.");

            // TODO database connection and data gathering.
        }
    }
}
