using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerServiceReporting.ApplicationCore.Helpers
{
    public static class LocalClientTimeHelper
    {

        /// <summary>
        /// Gets clients Local DateTime by timezoneId.
        /// </summary>
        /// <param name="timeZoneId"></param>
        /// <returns></returns>
        public static DateTime LocalClientTime(this string timeZoneId)
        {
            Console.WriteLine($"Server time: {DateTime.Now}");

            // Get the current time in server time zone
            DateTime serverUTCTime = DateTime.UtcNow;
            Console.WriteLine($"Server UTC time: {serverUTCTime}");

            // Define the client time zone
            TimeZoneInfo clientTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            Console.WriteLine($"Client Zone: {clientTimeZone}");

            // Convert the server time to client local time
            DateTime clientTime = TimeZoneInfo.ConvertTimeFromUtc(serverUTCTime, clientTimeZone);
            Console.WriteLine($"Client local time: {clientTime}");

            return clientTime;
        }
    }
}
