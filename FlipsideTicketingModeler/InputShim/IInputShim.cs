namespace FlipsideTicketingModeler.InputShim
{
    /**
     * @brief An object implementing this interface is able to read data into our data processesor.
     */
    public interface IInputShim
    {
        /**
         * @method Set the database that this input shim will populate.
         * @param database - The database that this input shim will populate.
         * @returns None
         */
        void SetDatabase(Data.Database database);

        /**
         * @method Starts the input shim and let it populate its data in the database.
         * @params None
         * @returns None; the given database will be populated with the data from this shim.
         */
        void Run();
    }
}
