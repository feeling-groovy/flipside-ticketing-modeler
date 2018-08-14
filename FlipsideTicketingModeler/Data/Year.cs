using System;
using System.Collections.Generic;

namespace FlipsideTicketingModeler.Data
{
    /**
     * @brief A year contains an entire set of buyers, their tickets and the transactions they undertook to transfer them.
     */
    [Serializable]
    public class Year
    {
        // What year is it?
        public string YearNumber = "";

        // The buyers who bought tickets this year.
        public Dictionary<string, Buyer> BuyerByName = new Dictionary<string, Buyer>();

        // Configuration information for this application.
        private Configuration.Configuration _configuration;

        /**
         * @constructor
         * @param configuration - contains the configuration information for this application.
         * @param yearNumber - The number for this year.
         */
        public Year(Configuration.Configuration configuration, string yearNumber)
        {
            _configuration = configuration;
            YearNumber = yearNumber;
        }

        /**
         * @method Get or create a ticket buyer in this year.
         * @param name - The name of the buyer to retrieve.
         * @returns an object representing this buyer in this year.
         */
        public Buyer GetBuyer(string name)
        {
            if (!BuyerByName.ContainsKey(name))
            {
                BuyerByName[name] = new Buyer(_configuration, name);
            }
            return BuyerByName[name];
        }

        /**
         * @property Obtain a list of all of the names of the buyers during this year.
         * @returns A list of the names of all the buyers in this year.
         */
        public List<string> BuyerNames
        {
            get
            {
                return new List<string>(BuyerByName.Keys);
            }
        }
    }
}
