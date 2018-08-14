using System;
using System.Collections.Generic;

namespace FlipsideTicketingModeler.Data
{
    /**
     * @brief This application's representation of the entire ticket database
     */
    [Serializable]
    public class Database
    {
        // The years that this database contains information for.
        public Dictionary<string, Year> YearByYearNumber = new Dictionary<string, Year>();

        // Configuration for this application.
        private Configuration.Configuration _configuration;

        /**
         * @constructor
         * @param configuration - Contains configuration information for this application.
         */
        public Database(Configuration.Configuration configuration)
        {
            _configuration = configuration;
        }

        /**
         * @method Get or add the year for a given year number.
         * @param yearNumber - The number of the year we wish to find in this database.
         * @returns An object representing the specified year.
         */
        public Year GetYear(string yearNumber)
        {
            // Get or create pattern.
            if (!YearByYearNumber.ContainsKey(yearNumber))
            {
                YearByYearNumber[yearNumber] = new Year(_configuration, yearNumber);
            }
            return YearByYearNumber[yearNumber];
        }

        #region Queries
        /**
         * @property Obtain the list of year numbers held in this database.
         * @returns A list of each of the year numbers held in this database.
         */
        public List<string> YearNumbers
        {
            get
            {
                return new List<string>(YearByYearNumber.Keys);
            }
        }

        /**
         * @method Count the number of individual buyers in a given year.
         * @param yearNumber - The year number we wish to count buyers for.
         * @returns The number of buyers who bought at least one ticket in the given year.
         */
        public int GetBuyerCountForYear(string yearNumber)
        {
            Year year = GetYear(yearNumber);
            List<string> allBuyerNames = year.BuyerNames;

            // Filter only the buyers who actually bought tickets.
            int count = 0;
            foreach (string buyerName in allBuyerNames)
            {
                if (year.GetBuyer(buyerName).DidBuyAdultTickets)
                {
                    ++count;
                }
            }
            return count;
        }

        /**
         * @method Obtain the number of tickets bought for a given year.
         * @param yearNumber - The year for which we wish to obtain this tally.
         * @returns The number of tickets bought during the given year.
         */
        public int GetTicketCountForYear(string yearNumber)
        {
            Year year = GetYear(yearNumber);
            List<string> allBuyerNames = year.BuyerNames;

            // Filter only the buyers who actually bought tickets.
            int count = 0;
            foreach (string buyerName in allBuyerNames)
            {
                count += year.GetBuyer(buyerName).AdultTicketCount;
            }
            return count;
        }

        /**
         * @method Get the number of tickets sold throughout the given year.
         * @param yearNumber - The year for which we wish to obtain this tally.
         * @returns The number of tickets sold during ths year.
         */
        public int GetSoldTicketCountForYear(string yearNumber)
        {
            Year year = GetYear(yearNumber);
            List<string> allBuyerNames = year.BuyerNames;

            // Filter only the buyers who actually bought tickets.
            int count = 0;
            foreach (string buyerName in allBuyerNames)
            {
                count += year.GetBuyer(buyerName).SoldAdultTicketCount;
            }
            return count;
        }

        /**
         * @method Obtain the list of buyer names that have bought a maximum ticket order over several years.
         * @params None
         * @returns A list of all buyer names in this database representing buyers who have bought the maximum number of allowable tickets over several years.
         */
        public HashSet<string> GetAffluentBuyerNames()
        {
            Dictionary<string, int> fullPurchasesByBuyerName = new Dictionary<string, int>();

            // Iterate through all year and all buyers in all years.
            foreach (string yearNumber in YearNumbers)
            {
                Year year = GetYear(yearNumber);
                foreach (string buyerName in year.BuyerNames)
                {
                    // Did this buyer buy the maximum number of tickets?
                    if (year.GetBuyer(buyerName).HadFullAdultTicketOrder(yearNumber))
                    {
                        // Add this user to our working dictionary.
                        if (!fullPurchasesByBuyerName.ContainsKey(buyerName))
                        {
                            fullPurchasesByBuyerName[buyerName] = 0;
                        }
                        ++fullPurchasesByBuyerName[buyerName];
                    }
                }
            }

            // Count the number of buyers who have maxed out their ticket orders across several years.
            HashSet<string> affluentBuyerNames = new HashSet<string>();
            foreach (string buyerName in fullPurchasesByBuyerName.Keys)
            {
                if (fullPurchasesByBuyerName[buyerName] >= _configuration.GetAffluentBuyerYearThreshold())
                {
                    affluentBuyerNames.Add(buyerName);
                }
            }

            return affluentBuyerNames;
        }
        #endregion
    }
}
