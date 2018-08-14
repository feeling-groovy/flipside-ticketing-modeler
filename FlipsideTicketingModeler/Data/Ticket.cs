using System;
using System.Collections.Generic;

namespace FlipsideTicketingModeler.Data
{
    /**
     * @brief Describes a ticket purchased by a buyer during a specific year.
     */
    [Serializable]
    public class Ticket
    {
        // The ID of this ticket.
        public string ID = "";

        // Is this a child ticket?
        public bool IsChildTicket = false;

        // The list of transactions undertaken with this ticket.
        public List<Transaction> Transactions = new List<Transaction>();

        /**
         * @constructor
         * @param configuration - contains the configuration information for this application.
         * @param id - The ID of this ticket.
         */
        public Ticket(Configuration.Configuration configuration, string id)
        {
            ID = id;
        }

        /**
         * @method Called to notify this ticket that it was transacted.
         * @params None
         * @returns None; transaction data will be set in this ticket.
         */
        public void WasTransacted()
        {
            Transactions.Add(new Transaction());
        }

        /**
         * @property Determine if this ticket has been sold at least once.
         * @returns true if this ticket was sold and false otherwise.
         */
        public bool WasSold
        {
            get
            {
                return Transactions.Count > 0;
            }
        }
    }
}
