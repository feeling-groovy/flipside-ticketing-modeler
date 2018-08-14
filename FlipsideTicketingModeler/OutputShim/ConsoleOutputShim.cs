using System;

namespace FlipsideTicketingModeler.OutputShim
{
    /**
     * @brief Logs output data to this application's console.
     */
    public class ConsoleOutputShim : IOutputShim
    {
        // IOutputShim

        public void Start()
        {
            Console.WriteLine("Starting data processing.");
        }

        public void OnProjectionScenarioCompleted(DataProcessor.ProjectionScenario scenario, DataProcessor.ProjectedFuture future)
        {
            Console.WriteLine("Finished processing future scenario: " + scenario.ToString());
            Console.WriteLine("Outcome:");
            Console.WriteLine("");

            Console.WriteLine("\t" + future.IfNotApplied.Name + ":");
            foreach (string particular in future.IfNotApplied.GetOutcomeParticulars(2))
            {
                Console.WriteLine(particular);
            }
            Console.WriteLine("\t" + future.GetLotteryStatusSummary(future.IfNotApplied));
            Console.WriteLine("");

            Console.WriteLine("\t" + future.IfApplied.Name + ":");
            foreach (string particular in future.IfApplied.GetOutcomeParticulars(2, future.IfNotApplied))
            {
                Console.WriteLine(particular);
            }
            Console.WriteLine("\t" + future.GetLotteryStatusSummary(future.IfApplied));
            Console.WriteLine("");
        }

        public void End()
        {
            Console.WriteLine("Finished data processing.");
        }
    }
}
