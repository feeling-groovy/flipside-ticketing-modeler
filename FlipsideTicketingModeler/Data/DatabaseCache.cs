using System;
using System.IO;

namespace FlipsideTicketingModeler.Data
{
    /**
     * @brief Writes an on-disk file cache of this application's database to remove the necessity of
     *      connecting to the flipside ticket database each time the application is run.
     */
    public class DatabaseCache : FileIO.FileOperator<Database>
    {
        // The name of this cache.
        private string _name = "";

        /**
         * @constructor
         * @param configuration - Contains configuration data for this application.
         * @param name - The name of this cache.  This name will be used to identify cache files on disk.
         */
        public DatabaseCache(Configuration.Configuration configuration, string name)
        {
            _name = name;
        }

        /**
         * @property Obtain the name for this cache.
         * @returns The name of this cache.
         */
        public string Name
        {
            get
            {
                return _name;
            }
        }

        /**
         * @method Save a database object to a local disk cache.
         * @param database - The file to save to disk.
         * @returns None; this will cause files to be written to disk.
         */
        public void Save(Database database)
        {
            SetSaveableObject(database, GetCacheFilename());
            while (!TickSave())
            {
                // Wait for success.
            }
        }

        /**
         * @method Load a database from a disk cache.
         * @params None
         * @returns The loaded database object or null if the cache does not exist.
         */
        public Database Load()
        {
            return Load(GetCacheFilename(), false);
        }

        /**
         * @method Invalidate the data cache on disk for this database.
         * @params Nons
         * @returns None; the files represeting this database cache will be deleted.
         */
        public void Invalidate()
        {
            FileInfo file = new FileInfo(GetAbsoluteFilename(GetCacheFilename()));
            // If this file doesn't exist, do nothing.
            if (!file.Exists)
            {
                // The cache is already invalid.
                return;
            }

            // Otherwise, try to delete the file.
            try
            {
                file.Delete();
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not invalidate cache " + Name + " due to exception " + e.Message);
            }
        }

        /**
         * @method Obtain the cache filename that we will save our database information to.
         * @params None
         * @returns The name of the file relative to our application directory that we will use as a cache for database data.
         */
        private string GetCacheFilename()
        {
            return "cache_" + Name + ".json";
        }
    }
}
