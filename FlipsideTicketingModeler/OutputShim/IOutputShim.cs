namespace FlipsideTicketingModeler.OutputShim
{
    /**
     * @brief An object implementing this interface is capable of producing output from this application.
     */
    public interface IOutputShim
    {
        /**
         * @method Inform this output shim that data processing is going to begin.
         * @params None
         * @returns None
         */
        void Start();

        /**
         * @method This method will be called whenever a projection scenario has been processed and a future has been determined.
         * @param scenario - The scenario that was processed.
         * @param future - The future outcome that was predicted.
         * @returns None
         */
        void OnProjectionScenarioCompleted(DataProcessor.ProjectionScenario scenario, DataProcessor.ProjectedFuture future);

        /**
         * @method Inform this output shim that data processing has ended.
         * @params None
         * @returns None
         */
        void End();
    }
}
