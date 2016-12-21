using System;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace VissimSimulator
{
    public class CollectorWorker
    {
        /// <summary>
        /// This is the main method of the CollectorWork's business logic.
        /// It should at least do something like:
        /// 1. Persist the event into either file or DB
        /// 2. Do some aggregation that might be helpful for the research
        /// </summary>
        /// <param name="evt"></param>
        public void Process(CellularTowerEvent evt)
        {
            int locationId = int.Parse(evt.LocationId);
            int cellularTowerId = int.Parse(evt.CellularTowerId);
            string eventType = Convert.ToString(evt.Event.EventType);
            long eventTimeSpan = evt.CurrentTick;
            AddEvent(locationId, cellularTowerId, eventType, eventTimeSpan);
        }
        /// <summary>
        /// Insert output data into the SQL database table.
        /// </summary>
        /// <param name="locatioanId">The location id of the cell station.</param>
        /// <param name="cellularTowerId">The cell id of the cell station.</param>
        /// <param name="eventType">The type of the event.</param>
        /// <param name="eventTimestamp">The time of the event when it occurs.</param>
        static void AddEvent(int locationId, int cellularTowerId, string eventType, long eventTimeTick)
        {
            using (OracleConnection con = new OracleConnection())
            {
                con.ConnectionString = ConfigurationManager.AppSettings["SqlConnectionString"];
                con.Open();
                try
                {
                    using (OracleCommand command = new OracleCommand(
                        "INSERT INTO OUTPUT VALUES(@LocationId, @CellularTowerId, @EventType, @EventTimeSpan)", con))
                    {
                        command.Parameters.Add(new OracleParameter("LocationId", locationId));
                        command.Parameters.Add(new OracleParameter("CellularTowerId", cellularTowerId));
                        command.Parameters.Add(new OracleParameter("EventType", eventType));
                        command.Parameters.Add(new OracleParameter("EventTimeSpan", eventTimeTick));
                        command.ExecuteNonQuery();
                    }
                }
                catch
                {
                    Console.WriteLine("Count not insert.");
                }
            }
        }
    }
}
