using System;
using System.Collections.Generic;

namespace FlipsideTicketingModeler.Data
{
    /**
     * @brief Contains the information for a single ticket buyer.
     */
    [Serializable]
    public class Buyer
    {
        // The buyer's name
        public string Name = "";

        // The list of tickets that this user bought.
        public Dictionary<string, Ticket> TicketById = new Dictionary<string, Ticket>();

        // Configuration information for this application.
        private Configuration.Configuration _configuration;

        /**
         * @constructor
         * @param configuration - contains the configuration information for this application.
         * @param name - This buyer's name.
         */
        public Buyer(Configuration.Configuration configuration, string name)
        {
            _configuration = configuration;
            Name = name;
        }

        /**
         * @method Get or obtain the ticket with the given ID.
         * @param id - The ID of the ticket to retrieve.
         * @returns An object representing this ticket.
         */
        public Ticket GetTicket(string id)
        {
            if (!TicketById.ContainsKey(id))
            {
                TicketById[id] = new Ticket(_configuration, id);
            }
            return TicketById[id];
        }

        /**
         * @method Obtain a list of only the adult tickets bought by this buyer.
         * @params None
         * @returns A list of only the adult tickets bought by this buyer.
         */
        private List<Ticket> GetAdultTickets()
        {
            List<Ticket> adultTickets = new List<Ticket>();

            foreach (string ticketId in TicketById.Keys)
            {
                Ticket ticket = GetTicket(ticketId);
                if (!ticket.IsChildTicket)
                {
                    adultTickets.Add(ticket);
                }
            }

            return adultTickets;
        }

        /**
         * @properties Obtain the number of tickets bought by this buyer.
         * @returns The number of tickets bought by this buyer.
         */
        public int AdultTicketCount
        {
            get
            {
                return GetAdultTickets().Count;
            }
        }

        /**
         * @property Obtain the number of tickets sold for this buyer.
         * @returns The number of purchased tickets that this buyer sold.
         */
        public int SoldAdultTicketCount
        {
            get
            {
                int count = 0;
                foreach (Ticket ticket in GetAdultTickets())
                {
                    if (ticket.WasSold)
                    {
                        ++count;
                    }
                }
                return count;
            }
        }

        /**
         * @property Determine if this buyer bought any tickets.
         * @returns true if this buyer bought tickets and false otherwise.
         */
        public bool DidBuyAdultTickets
        {
            get
            {
                return AdultTicketCount > 0;
            }
        }

        /**
         * @method Determine if this buyer entered a full ticket order for the current year.
         * @param forYear - The year that we wish to check.
         * @returns true if this buyer purchased the maximum number of tickets for the year and false otherwise.
         */
        public bool HadFullAdultTicketOrder(string forYear)
        {
            return AdultTicketCount == _configuration.GetTicketCapForYear(forYear);
        }

        /**
         * @property Determine if this buyer sold at least one of their purchased tickets to another person.
         * @returns true if the buyer sold at least one of their tickets and false otherwise.
         */
        public bool DidBuyAdultTicketsForOthers
        {
            get
            {
                return SoldAdultTicketCount > 0;
            }
        }

        /**
         * @property Determine if this buyer sold all of their tickets to others.
         * @returns true if this buyer sold all of their originally purchased tickets to other people and false otherwise.
         */
        public bool SoldAllAdultTickets
        {
            get
            {
                return AdultTicketCount == SoldAdultTicketCount;
            }
        }
    }
}
