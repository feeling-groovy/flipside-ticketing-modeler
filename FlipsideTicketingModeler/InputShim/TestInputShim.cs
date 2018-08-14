using System;

namespace FlipsideTicketingModeler.InputShim
{
    /**
     * @brief This kind of input shim produces mocked data for our program.
     */
    public class TestInputShim : IInputShim
    {
        // The database we should populate.
        private Data.Database _database;

        /**
         * @constructor
         * @param configuration - The configuration for this application.
         */
        public TestInputShim(Configuration.Configuration configuration)
        {
            // Intentionally left blank.
        }

        // IInputShim

        public void SetDatabase(Data.Database database)
        {
            _database = database;
        }

        public void Run()
        {
            Console.WriteLine("Running application with mocked test data...");

            /**
             * Dossier
             * 
             * Family Buyers: buys the maximum number of tickets and sells none of them
             *      Kind Kim
             *      Family-Minded Fanny
             * 
             * Affluent Buyers: buys the maximum number of tickets each year and sells all but one
             *      Affluent Alice
             *      Rich Rich
             *      
             * Scalper Buyers: buys the maximum number of tickets and sells all of them
             *      Scalper Sam
             *      Lame Lenny
             * 
             * Unfortunate Buyers: buys one ticket and has to sell it to someone else
             *      Sad Sally
             *      Melancholy Melanie
             * 
             * Normal Buyers: buys one ticket and does not sell it
             *      Burner Bob
             *      Sparkle Pony
             */

            // Add some junk data to this database for testing.

            // 2016 data
            string year;
            year = "2016";
            _database.GetYear(year).GetBuyer("Kind Kim").GetTicket("Ticket 1");
            _database.GetYear(year).GetBuyer("Kind Kim").GetTicket("Ticket 2");
            _database.GetYear(year).GetBuyer("Kind Kim").GetTicket("Child Ticket 1").IsChildTicket = true;
            _database.GetYear(year).GetBuyer("Kind Kim").GetTicket("Child Ticket 2").IsChildTicket = true;

            _database.GetYear(year).GetBuyer("Affluent Alice").GetTicket("Ticket 3");
            _database.GetYear(year).GetBuyer("Affluent Alice").GetTicket("Ticket 4").WasTransacted();

            _database.GetYear(year).GetBuyer("Scalper Sam").GetTicket("Ticket 5").WasTransacted();
            _database.GetYear(year).GetBuyer("Scalper Sam").GetTicket("Ticket 6").WasTransacted();

            _database.GetYear(year).GetBuyer("Melancholy Melanie").GetTicket("Ticket 7").WasTransacted();

            _database.GetYear(year).GetBuyer("Burner Bob").GetTicket("Ticket 8");
            _database.GetYear(year).GetBuyer("Sparkle Pony").GetTicket("Ticket 9");

            // 2017 data
            year = "2017";
            _database.GetYear(year).GetBuyer("Family-Minded Fanny").GetTicket("Ticket 1");
            _database.GetYear(year).GetBuyer("Family-Minded Fanny").GetTicket("Ticket 2");
            _database.GetYear(year).GetBuyer("Family-Minded Fanny").GetTicket("Child Ticket 1").IsChildTicket = true;
            _database.GetYear(year).GetBuyer("Family-Minded Fanny").GetTicket("Child Ticket 2").IsChildTicket = true;
            _database.GetYear(year).GetBuyer("Kind Kim").GetTicket("Ticket 3");
            _database.GetYear(year).GetBuyer("Kind Kim").GetTicket("Ticket 4");
            _database.GetYear(year).GetBuyer("Kind Kim").GetTicket("Child Ticket 3").IsChildTicket = true;

            _database.GetYear(year).GetBuyer("Affluent Alice").GetTicket("Ticket 5");
            _database.GetYear(year).GetBuyer("Affluent Alice").GetTicket("Ticket 6").WasTransacted();
            _database.GetYear(year).GetBuyer("Rich Rich").GetTicket("Ticket 7");
            _database.GetYear(year).GetBuyer("Rich Rich").GetTicket("Ticket 8").WasTransacted();

            _database.GetYear(year).GetBuyer("Lame Lenny").GetTicket("Ticket 9").WasTransacted();
            _database.GetYear(year).GetBuyer("Lame Lenny").GetTicket("Ticket 10").WasTransacted();

            _database.GetYear(year).GetBuyer("Melancholy Melanie").GetTicket("Ticket 11").WasTransacted();
            _database.GetYear(year).GetBuyer("Sad Sally").GetTicket("Ticket 12").WasTransacted();

            _database.GetYear(year).GetBuyer("Burner Bob").GetTicket("Ticket 13");
            _database.GetYear(year).GetBuyer("Sparkle Pony").GetTicket("Ticket 14");

            // 2018 data
            year = "2018";
            _database.GetYear(year).GetBuyer("Kind Kim").GetTicket("Ticket 1");
            _database.GetYear(year).GetBuyer("Kind Kim").GetTicket("Ticket 2");
            _database.GetYear(year).GetBuyer("Kind Kim").GetTicket("Child Ticket 1").IsChildTicket = true;
            _database.GetYear(year).GetBuyer("Kind Kim").GetTicket("Child Ticket 2").IsChildTicket = true;
            _database.GetYear(year).GetBuyer("Family-Minded Fanny").GetTicket("Ticket 3");
            _database.GetYear(year).GetBuyer("Family-Minded Fanny").GetTicket("Ticket 4");
            _database.GetYear(year).GetBuyer("Family-Minded Fanny").GetTicket("Child Ticket 3").IsChildTicket = true;

            _database.GetYear(year).GetBuyer("Affluent Alice").GetTicket("Ticket 5");
            _database.GetYear(year).GetBuyer("Affluent Alice").GetTicket("Ticket 6").WasTransacted();
            _database.GetYear(year).GetBuyer("Rich Rich").GetTicket("Ticket 7");
            _database.GetYear(year).GetBuyer("Rich Rich").GetTicket("Ticket 8").WasTransacted();

            _database.GetYear(year).GetBuyer("Scalper Sam").GetTicket("Ticket 9").WasTransacted();
            _database.GetYear(year).GetBuyer("Scalper Sam").GetTicket("Ticket 10").WasTransacted();
            _database.GetYear(year).GetBuyer("Lame Lenny").GetTicket("Ticket 11").WasTransacted();
            _database.GetYear(year).GetBuyer("Lame Lenny").GetTicket("Ticket 12").WasTransacted();

            _database.GetYear(year).GetBuyer("Melancholy Melanie").GetTicket("Ticket 13").WasTransacted();

            _database.GetYear(year).GetBuyer("Burner Bob").GetTicket("Ticket 14");
            _database.GetYear(year).GetBuyer("Sparkle Pony").GetTicket("Ticket 15");
        }
    }
}
