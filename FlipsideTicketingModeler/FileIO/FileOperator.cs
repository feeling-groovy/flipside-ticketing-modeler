using System;
using System.IO;
using Newtonsoft.Json;

namespace FlipsideTicketingModeler.FileIO
{
    /**
     * @brief This type of class can perform disk operations.
     */
    public class FileOperator<T>
    {
        // Our object to save and its fully qualified filepath.
        private static T _objectToSave = default(T);
        private static string _fullyQualifiedSaveFilename;

        /**
         * @method Load a file from disk and hand back a data object built from the file data.
         * @param relativeFileame - The file path relative to the program's folder that we wish to load.
         * @param logIfFileDoesNotExist - If this is true, an error message will be logged if the file can't be found.  Otherwise, no message will be logged.
         * @returns The loaded data object if we were successful or null otherwise.
         */
        public static T Load(string relativeFilename, bool logIfFileDoesNotExist = true)
        {
            T result = default(T);

            // Find the full filename for this file.
            string fileName = GetAbsoluteFilename(relativeFilename);

            // Try to stat the file.
            FileInfo file = new FileInfo(fileName);
            if (!file.Exists)
            {
                if (logIfFileDoesNotExist)
                {
                    Console.WriteLine("Unable to locate json file at " + fileName);
                }
                return default(T);
            }

            // Read file contents.
            try
            {
                TextReader fileStream = File.OpenText(fileName);
                JsonSerializer serializer = new JsonSerializer();
                result = (T)serializer.Deserialize(fileStream, typeof(T));
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not read file " + fileName + " due to exception " + e.Message);
                return default(T);
            }

            return result;
        }

        /**
         * @method This is the first half of the object saving workflow.  This will set an object that we will attempt to save.
         * @param objectToSave - The object that we will attempt to save to disk.
         * @param relativeFileame - The file path relative to the program's folder that we wish to save.
         * @returns None; the object to save will be set in this object.
         */
        public static void SetSaveableObject(T objectToSave, string relativeFilename)
        {
            _objectToSave = objectToSave;
            _fullyQualifiedSaveFilename = GetAbsoluteFilename(relativeFilename);
            
        }

        /**
         * @property Determine if we currently have an objecto to save.
         * @returns true if we have a pending save object and false otherwise.
         */
        public static bool IsSaving
        {
            get
            {
                return !_objectToSave.Equals(default(T));
            }
        }

        /**
         * @method This is the second half of the object saving workflow.  This will actually try to flush the object to disk.
         * @params None
         * @returns true if the object was successfully saved and false if it has not yet been saved.
         */
        public static bool TickSave()
        {
            // Do we have a thing to save?
            if (_objectToSave.Equals(default(T)))
            {
                // Nothing to save.
                return false;
            }

            try
            {
                // Try to stat the file.
                FileInfo file = new FileInfo(_fullyQualifiedSaveFilename);
                if (!file.Exists)
                {
                    File.Create(_fullyQualifiedSaveFilename);
                }

                // Write the file.
                {
                    // Create objects capable of serializing JSON, writing to our target file, and correctly writing JSON text.
                    JsonSerializer serializer = new JsonSerializer();
                    StreamWriter sw = new StreamWriter(_fullyQualifiedSaveFilename);
                    JsonWriter writer = new JsonTextWriter(sw);

                    // Make this file human-readable
                    writer.Formatting = Formatting.Indented;

                    // Serialize the target object.
                    serializer.Serialize(writer, _objectToSave, typeof(T));

                    // Write and close the file.
                    writer.Flush();
                    writer.AutoCompleteOnClose = true;
                    writer.Close();
                }
            }
            catch (Exception)
            {
                // We did not succeed.
                return false;
            }

            // We succeeded.  Clear our save object
            _objectToSave = default(T);
            _fullyQualifiedSaveFilename = "";
            return true;
        }

        /**
         * @method Obtain the fully qualified path to a relative filepath
         * @param relativeFilename - The filename relative to our application folder.
         * @returns The fully qualified path to the given file.
         */
        protected static string GetAbsoluteFilename(string relativeFilename)
        {
            return Environment.CurrentDirectory + "\\" + relativeFilename;
        }
    }
}
