using System;
using System.Collections.Generic;

namespace FlipsideTicketingModeler.Configuration
{
    /**
     * @brief Contains configuration information for this application.
     */
    [Serializable]
    public class Configuration : FileIO.FileOperator<Configuration>
    {
        // Public configuration fields.

        // The valid years to evaluate.
        public List<string> yearsToEvaluate = new List<string>();

        // Information about ticket parameters for different years.
        public ConfigurationTickets tickets = new ConfigurationTickets();

        // Information about how to use the projector
        public ConfigurationProjector projector = new ConfigurationProjector();

        // Information about how to treat buyer behavior
        public ConfigurationBuyers buyers = new ConfigurationBuyers();

        // Configuration classes.

        /**
         * @brief Contains configuration information pertaining to ticket constraints (i.e. max tickets) for different years.
         */
        [Serializable]
        public class ConfigurationTickets
        {
            // What will the default ticket max be if it is not specified for a particular year?
            public int defaultTicketMax = 2;

            // We can specify ticket maxes for years individually.
            public Dictionary<string, int> ticketMaxByYear = new Dictionary<string, int>();
        }

        /**
         * @brief Contains information about how we wish to use the future projector to determine possible future outcomes.
         */
        [Serializable]
        public class ConfigurationProjector
        {
            // The theoretical lottery ticket cap to use for the listed scenarios
            public int lotteryTicketCount = 0;

            // The highest population growth that we predict for this year
            public float maximumExpectedPopulationGrowthPercentage;

            // The list of scenarios we will test.
            public List<ConfigurationProjectorScenario> scenarios = new List<ConfigurationProjectorScenario>();
        }

        /**
         * @brief An individual scenario to test in our future projector.
         */
        [Serializable]
        public class ConfigurationProjectorScenario
        {
            // The scenario's name
            public string name = "";

            // Will we test the scenario where the max number of sold tickets will change, and if so, what will the new number be?
            public bool changeTicketMax = false;
            public int newTicketMax = 2;
        }

        /**
         * @brief Contains configuration data that we can use to understand buyer behavior.
         */
        [Serializable]
        public class ConfigurationBuyers
        {
            // How many years out of the entire set would a buyer have had to purchase a full ticket order and sold most of them to be considered "affluent"?
            public int aggregateAffluenceYearCount = 3;

            // How many years out of the entire set would a buyer need to sell all of their tickets to be considered a "scalper"?
            public int aggregateScalpingYearCount = 3;
        }

        // Methods

        /**
         * @method Determine the historical ticket cap for a given year.
         * @param yearNumber - The year that we wish to find the ticket cap for.
         * @returns The ticket cap for the given year.
         */
        public int GetTicketCapForYear(string yearNumber)
        {
            // Return a default if this year does not exist in our year map.
            if (!tickets.ticketMaxByYear.ContainsKey(yearNumber))
            {
                return tickets.defaultTicketMax;
            }

            return tickets.ticketMaxByYear[yearNumber];
        }

        /**
         * @method Determine the minimum number of years that a single buyer would need to purchase an entire ticket order and sell most of them
         *      for this application to consider that buyer "affluent"
         * @params None
         * @returns The minimum number of years as described above for "affluence"
         */
        public int GetAffluentBuyerYearThreshold()
        {
            return buyers.aggregateAffluenceYearCount;
        }

        /**
         * @method Obtain a list of years whose data we wish to use in this application.
         * @params None
         * @returns A list of year numbers that ought to be considered when running this application.
         */
        public List<string> GetValidYears()
        {
            return yearsToEvaluate;
        }

        /**
         * @method Obtain the highest year number in our application that we are aware of.
         * @params None
         * @returns null if we can't find such a year, or the highest year number if we can.
         */
        public string GetHighestYearNumber()
        {
            // Do we have any year numbers at all?
            if (yearsToEvaluate.Count == 0)
            {
                return null;
            }

            // Initialize our search by picking the first year.
            string highestYear = yearsToEvaluate[0];

            // Iterate through all of our known years to find the highest.
            int highestYearNumber = 0;
            foreach (string year in yearsToEvaluate)
            {
                int yearAsNumber = 0;
                if (int.TryParse(year, out yearAsNumber))
                {
                    // This is a valid year.  Is it higher than our highest year?
                    if (yearAsNumber > highestYearNumber)
                    {
                        highestYearNumber = yearAsNumber;
                    }
                }
            }

            return highestYearNumber == 0 ? null : highestYearNumber.ToString();
        }

        /**
         * @method Obtain the list of projection scenarios we wish to use to predict the future.
         * @params None
         * @returns A list of objects, each of which represents a scenario we wish to test via future projection.
         */
        public List<DataProcessor.ProjectionScenario> GetProjectionScenarios()
        {
            List<DataProcessor.ProjectionScenario> scenarios = new List<DataProcessor.ProjectionScenario>();
            
            // Convert to an intermediate object type.
            foreach (ConfigurationProjectorScenario configurationScenario in projector.scenarios)
            {
                scenarios.Add(new DataProcessor.ProjectionScenario(configurationScenario));
            }

            return scenarios;
        }
    }
}
