﻿using System;
using System.Collections.Generic;

namespace FlipsideTicketingModeler.DataProcessor
{
    /**
     * @brief This kind of object represents a potential future ticketing outcome as generated by the projector.
     */
    public class ProjectedFuture
    {
        /**
         * @brief Represents a ticketing outcome.  This object will contain two of these so that
         *      we have the opportunity to compare them.
         */
        public class Outcome
        {
            // The name for this outcome.
            public string Name;

            // The expected number of participants
            public int ExpectedTotalTicketBuyers = 0;

            // The number of tickets sold.
            public int TotalAdultTicketsSold = 0;

            // The number of tickets transferred to other participants.
            public int TotalAdultTicketsTransferred = 0;

            /**
             * @constructor
             * @param name - The name of this outcome.
             */
            public Outcome(string name)
            {
                Name = name;
            }

            /**
             * @method Build a percentage string for output given a comparison value
             * @param value - The comparison value to build a string for.
             * @returns An empty string if the value does not have data, or a string in the following format: -40% change.
             */
            private string GetComparisonStringForDeltaValue(float? value)
            {
                // Do nothing if we have no information.
                if (!value.HasValue || value.Value == 0f)
                {
                    return "";
                }

                // Otherwise, build a result string.
                return " (" + ((value.Value - 1f < 0f) ? "" : "+") + ((value.Value - 1f) * 100f).ToString("n2") + "% change)";
            }

            /**
             * @method Obtain a list of output strings to describe this outcome.
             * @param indentLevel - This number of tabs will be added before each of the output strings.
             * @param comparison - If this is specified, these particulars will also include the % change compared to the comparison Outcome.
             * @returns A list of strings, each of which describes a particular part of this outcome.
             */
            public List<string> GetOutcomeParticulars(int indentLevel = 0, Outcome comparison = null)
            {
                List<string> particulars = new List<string>();

                // Inject tabs at the beginning of each of these strings.
                string prefix = "";
                for (int i = 0; i < indentLevel; ++i)
                {
                    prefix += "\t";
                }

                // Compute comparisons.
                float? deltaBuyers = null;
                float? deltaAdultTicketsSold = null;
                float? deltaAdultTicketsTransferred = null;
                if (comparison != null)
                {
                    deltaBuyers = (float)(ExpectedTotalTicketBuyers) / (float)(comparison.ExpectedTotalTicketBuyers);
                    deltaAdultTicketsSold = (float)(TotalAdultTicketsSold) / (float)(comparison.TotalAdultTicketsSold);
                    deltaAdultTicketsTransferred = (float)(TotalAdultTicketsTransferred) / (float)(comparison.TotalAdultTicketsTransferred);
                }

                // Add each of our particulars to the output list.
                particulars.Add(prefix + "Total Buyers: " + ExpectedTotalTicketBuyers.ToString() + GetComparisonStringForDeltaValue(deltaBuyers));
                particulars.Add(prefix + "Total Adult Tickets Sold: " + TotalAdultTicketsSold.ToString() + GetComparisonStringForDeltaValue(deltaAdultTicketsSold));
                particulars.Add(prefix + "Total Adult Tickets Transferred: " + TotalAdultTicketsTransferred.ToString() + GetComparisonStringForDeltaValue(deltaAdultTicketsTransferred));

                return particulars;
            }
        }

        /**
         * @brief Tells us if we'll have a lottery, won't have one, or will have one if we will have one with a population increase
         */
        public enum LotteryStatusCode
        {
            // We will not have a lottery
            NoLottery,
            // We will have a lottery
            WillHaveLottery,
            // We will have a lottery if we have a limited population increase
            WillHaveLotteryWithPopulationIncrease
        }

        /**
         * @brief An object that will tell us whether we will have a potential lottery or not.
         */
        public class LotteryStatus
        {
            // Tells us whether we will have a lottery, won't have one, or will have one if the population increases enough.
            public LotteryStatusCode StatusCode = LotteryStatusCode.NoLottery;

            // If we need a percentage population increase to have a lottery, how much would we need to increase in order to hit it?
            public float PopulationUnitIncreaseForLottery = 0f;
        }

        // Our configuration data for this application.
        private Configuration.Configuration _configuration;

        // The predicted outcomes for this future.
        public Outcome IfApplied = new Outcome("If Applied");
        public Outcome IfNotApplied = new Outcome("If Not Applied");

        /**
         * @constructor
         * @param configuration - The configuration information for this application.
         */
        public ProjectedFuture(Configuration.Configuration configuration)
        {
            _configuration = configuration;
        }

        /**
         * @method Determine if we will have a lottery given a specific outcome.
         * @param outcome - The outcome that we wish to test
         * @returns An object that will tell us whether we will have a lottery, won't have one, or will have one given limited population growth
         */
        public LotteryStatus GetLotteryStatus(Outcome outcome)
        {
            LotteryStatus status = new LotteryStatus();

            // Will we have a lottery?
            if (outcome.TotalAdultTicketsSold > _configuration.projector.lotteryTicketCount)
            {
                status.StatusCode = LotteryStatusCode.WillHaveLottery;
                return status;
            }

            // We won't have an automatic lottery.  Will we potentially have a lottery?
            float neededPopulationIncreaseForLottery = (float)(_configuration.projector.lotteryTicketCount - outcome.TotalAdultTicketsSold) / (float)(outcome.TotalAdultTicketsSold);
            neededPopulationIncreaseForLottery *= 100f;

            // Is the needed growth outside of our maximum threshold?
            if (neededPopulationIncreaseForLottery > _configuration.projector.maximumExpectedPopulationGrowthPercentage)
            {
                // No lottery.
                status.StatusCode = LotteryStatusCode.NoLottery;
            }
            else
            {
                // Otherwise, we will have a lottery given a particular population growth.
                status.StatusCode = LotteryStatusCode.WillHaveLotteryWithPopulationIncrease;
                status.PopulationUnitIncreaseForLottery = neededPopulationIncreaseForLottery / 100f;
            }

            return status;
        }

        /**
         * @method Obtain a string that describes the lottery status for a particular outcome.
         * @param outcome - The outcome to obtain this description from.
         * @returns A string that describes whether this particular outcome will involve having a ticket lottery.
         */
        public string GetLotteryStatusSummary(Outcome outcome)
        {
            string result = "";

            // Figure out what the status of our lottery is.
            LotteryStatus status = GetLotteryStatus(outcome);

            // Write the string that indicates the result of this operation.
            switch (status.StatusCode)
            {
                case LotteryStatusCode.NoLottery:
                    result = "No lottery";
                    break;
                case LotteryStatusCode.WillHaveLottery:
                    result = "LOTTERY WILL OCCUR";
                    break;
                case LotteryStatusCode.WillHaveLotteryWithPopulationIncrease:
                    result = "Lottery WILL occur if the population increases by " + (status.PopulationUnitIncreaseForLottery * 100f).ToString("n2") + "%";
                    break;
                default:
                    Console.WriteLine("Unsupported lottery status code: " + status.StatusCode.ToString());
                    break;
            }

            return result;
        }
    }
}
