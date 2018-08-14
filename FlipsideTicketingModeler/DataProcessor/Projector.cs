using System.Collections.Generic;

namespace FlipsideTicketingModeler.DataProcessor
{
    /**
     * @brief Contains the logic that allows this application to predict ticket purchasing outcomes in the future.
     */
    public class Projector
    {
        // Application configuration information.
        private Configuration.Configuration _configuration;

        // Contains the input information that we can use to project into the future.
        private Data.Database _database;

        /**
         * @constructor
         * @param configuration - Contains configuration information for this application.
         * @param database - The database containing all of the tickets for all of the years read from the database.
         */
        public Projector(Configuration.Configuration configuration, Data.Database database)
        {
            // Cache resources.
            _configuration = configuration;
            _database = database;
        }

        /**
         * @method Project the future for a ticket scenario.
         * @param scenario - The scenario to project into the future.
         * @returns An object that represents the projected outcome for future years if the given ticketing scenario is implemented.
         */
        public ProjectedFuture Project(ProjectionScenario scenario)
        {
            // Create the object to contain our answer.
            ProjectedFuture future = new ProjectedFuture(_configuration);

            // Base these results on the highest year in our application.
            string highestYearNumber = _configuration.GetHighestYearNumber();

            // Determine outcomes.
            DetermineOutcome(highestYearNumber, future.IfApplied, scenario);
            DetermineOutcome(highestYearNumber, future.IfNotApplied, null);

            // We are done computing.  Return the result.
            return future;
        }

        /**
         * @method Build a potential future outcome given a scenario and based on a past year.
         * @param basedOnYearNumber - The outcome will be based on the historical data in the given year.
         * @param outcome - This object will be populated with the results of this computation.
         * @param scenario - If this is null, the values held in the given year will be populated in the outcome.
         *      If it is not null, the outcome will take the parameters of the scenario into question.
         * @returns None; the outcome variable will have its member values set.
         */
        private void DetermineOutcome(string basedOnYearNumber, ProjectedFuture.Outcome outcome, ProjectionScenario scenario)
        {
            // Copy historical data from this year.
            outcome.ExpectedTotalTicketBuyers = _database.GetBuyerCountForYear(basedOnYearNumber);
            outcome.TotalAdultTicketsSold = _database.GetTicketCountForYear(basedOnYearNumber);
            outcome.TotalAdultTicketsTransferred = _database.GetSoldTicketCountForYear(basedOnYearNumber);

            // If we have a null scenario, just plug the values for this outcome that occurred in the given year.
            if (scenario == null)
            {
                return;
            }

            // Does the scenario include a different ticket cap?
            if (scenario.TicketCap.HasValue)
            {
                // Project into the future.
                outcome.ExpectedTotalTicketBuyers = _database.GetBuyerCountForYear(basedOnYearNumber);
                outcome.TotalAdultTicketsSold = 0;
                outcome.TotalAdultTicketsTransferred = 0;

                // Determine the future behavior of affluent buyers
                HashSet<string> affluentUserNames = _database.GetAffluentBuyerNames();
                Data.Year year = _database.GetYear(basedOnYearNumber);

                // Iterate over the buyers in this year.
                foreach (string buyerName in year.BuyerNames)
                {
                    // Is this an affluent buyer?
                    bool isAffluent = affluentUserNames.Contains(buyerName);

                    // If this is an affluent buyer, assume that they will buy the max tickets and sell all but 1.
                    if (isAffluent)
                    {
                        outcome.TotalAdultTicketsSold += scenario.TicketCap.Value;
                        outcome.TotalAdultTicketsTransferred += scenario.TicketCap.Value - 1;
                    }
                    else
                    {
                        // Otherwise, just repeat this buyer's behavior.
                        Data.Buyer buyer = year.GetBuyer(buyerName);
                        outcome.TotalAdultTicketsSold += buyer.AdultTicketCount;
                        outcome.TotalAdultTicketsTransferred += buyer.SoldAdultTicketCount;
                    }
                }
            }
        }
    }
}
