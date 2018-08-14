namespace FlipsideTicketingModeler.DataProcessor
{
    /**
     * @brief Defines a scenario that the projector can use to predict the future.
     */
    public class ProjectionScenario
    {
        // What is this scenario's display name?
        public string DisplayName = "Scenario";

        // What should the ticket cap be for this scenario?
        public int? TicketCap = null;

        /**
         * @constructor
         * @param configurationScenario - The scenario as defined in the configuration file to build this object from.
         */
        public ProjectionScenario(Configuration.Configuration.ConfigurationProjectorScenario configurationScenario)
        {
            // Copy name
            DisplayName = configurationScenario.name ?? "Scenario";

            // Copy parameters
            if (configurationScenario.changeTicketMax)
            {
                TicketCap = configurationScenario.newTicketMax;
            }
        }

        public override string ToString()
        {
            return DisplayName + " (" + (TicketCap.HasValue ? ("Ticket Cap: " + TicketCap.Value.ToString()) : "") + ")";
        }
    }
}
