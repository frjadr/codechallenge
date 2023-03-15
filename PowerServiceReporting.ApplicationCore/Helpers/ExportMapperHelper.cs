using PowerServiceReporting.ApplicationCore.DTOs;
using Serilog;
using System.Numerics;
using System.Reflection;

namespace PowerServiceReporting.ApplicationCore.Helpers
{
    /// <summary>
    /// Helper class for data transformation and preparation for export.
    /// </summary>
    public static class ExportMapperHelper
    {
        /// <summary>
        /// Dictionary used for mapping Period with actual hour that is added to client DateTime.
        /// </summary>
        private static readonly Dictionary<int, int> PeriodHourMap = new Dictionary<int, int>
        {
            {0, 0}, {1, 23}, {2, 0}, {3, 1}, {4, 2}, {5, 3}, {6, 4}, {7, 5}, {8, 6}, {9, 7}, {10, 8}, {11, 9}, {12, 10},
            {13, 11}, {14, 12}, {15, 13}, {16, 14}, {17, 15}, {18, 16}, {19, 17}, {20, 18}, {21, 19}, {22, 20}, {23, 21}, {24, 22}
        };

        /// <summary>
        /// Handles local client date time period->hour addition and period hour extraction.
        /// </summary>
        /// <param name="powerTradeDate"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        private static (DateTime LocalClientDateTimeWithPeriod, string PeriodHour) MergeDateWithPeriod(DateTime powerTradeDate, int period)
        {
            var localClientDateTimeWithPeriod = period == 1 ? new DateTime(powerTradeDate.Year, powerTradeDate.Month, powerTradeDate.Day-1).AddHours(PeriodHourMap[period]) : new DateTime(powerTradeDate.Year, powerTradeDate.Month, powerTradeDate.Day).AddHours(PeriodHourMap[period]);
            var periodHour = (localClientDateTimeWithPeriod.Hour.ToString().Length == 1 ? ("0" + localClientDateTimeWithPeriod.Hour.ToString()) : localClientDateTimeWithPeriod.Hour.ToString()) + ":" + (localClientDateTimeWithPeriod.Minute == 0 ? "00" : localClientDateTimeWithPeriod.Minute.ToString());

            return (localClientDateTimeWithPeriod, periodHour);
        }

        /// <summary>
        /// Places 23:00 at start and removes it from end.
        /// </summary>
        /// <param name="powerTradeExportDTOs"></param>
        /// <returns></returns>
        private static List<PowerTradeExportDTO> MoveLastToFirstAndRemoveLast(this List<PowerTradeExportDTO> powerTradeExportDTOs)
        {
            if (powerTradeExportDTOs.Count > 0)
            {
                var last = powerTradeExportDTOs[powerTradeExportDTOs.Count - 1]; 
                powerTradeExportDTOs.RemoveAt(powerTradeExportDTOs.Count - 1);
                powerTradeExportDTOs.Insert(0, last); 
            }

            return powerTradeExportDTOs;
        }

        /// <summary>
        /// Filters, maps and aggregates PowerTrade data for csv frieldly format.
        /// </summary>
        /// <param name="powerTradeDTOs"></param>
        /// <param name="clientLocalTime"></param>
        /// <returns></returns>
        public static List<PowerTradeExportDTO> MapPowerTradesToPowerTradesExportAggregated(this List<PowerTradeDTO> powerTradeDTOs, DateTime clientLocalTime)
        {
            var powerTradeExportDTOs = new List<PowerTradeExportDTO>();
            try
            {
                
                powerTradeExportDTOs = powerTradeDTOs.SelectMany(powerTrade => // filters and remaps powertrade data
                {
                    return powerTrade.Periods.Select(period =>
                    {
                        var (mergeDate, periodHour) = MergeDateWithPeriod(powerTrade.Date, period.Period);
                        return new PowerTradeExportDTO
                        {
                           Period = periodHour,
                           Volume = period.Volume
                        };
                    });
                })
                .GroupBy(pt => pt.Period)
                .Select(g => new PowerTradeExportDTO  // returns aggregated volumes grouped and ordered by period 
                {
                    Period = g.Key,
                    Volume = g.Sum(pt => pt.Volume)
                })
                .OrderBy(pt => pt.Period)
                .ToList();

                powerTradeExportDTOs = MoveLastToFirstAndRemoveLast(powerTradeExportDTOs); // places 23:00 at start and removes it from end
            }
            catch (Exception ex)
            {
                Log.Error($"[{Assembly.GetEntryAssembly().GetName().Name}] => [{typeof(ExportMapperHelper).Name}.{ReflectionHelper.GetActualAsyncMethodName()}]" +
                    $" - failed at Client Local Time {clientLocalTime} with Exception:\n  -Message: {ex.Message}\n  -StackTrace: {ex.StackTrace}");
            }
            return powerTradeExportDTOs;
        }

        /// <summary>
        /// Filters and maps PowerTrade data for csv frieldly format.
        /// </summary>
        /// <param name="powerTradeDTOs"></param>
        /// <param name="clientLocalTime"></param>
        /// <returns></returns>
        public static List<PowerTradeExportDTO> MapPowerTradesToPowerTradesExportNonAggregated(this List<PowerTradeDTO> powerTradeDTOs, DateTime clientLocalTime)
        {
            var powerTradesExportDTOs = new List<PowerTradeExportDTO>();
            try
            {
                // filters and remaps powertrade data, orders them by local client time with period
                powerTradesExportDTOs = powerTradeDTOs.SelectMany(powerTrade =>
                {
                    return powerTrade.Periods.Select(period =>
                    {
                        var (localClientDateTimeWithPeriod, periodHour) = MergeDateWithPeriod(powerTrade.Date, period.Period);
                        return new PowerTradeExportDTO
                        {
                            LocalClientTimeWithPeriod = localClientDateTimeWithPeriod,
                            Period = periodHour,
                            Volume = period.Volume
                        };
                    });
                }).OrderBy(pt => pt.LocalClientTimeWithPeriod).ToList();
            }
            catch (Exception ex)
            {
                Log.Error($"[{Assembly.GetEntryAssembly().GetName().Name}] => [{typeof(ExportMapperHelper).Name}.{ReflectionHelper.GetActualAsyncMethodName()}]" +
                    $" - failed at Client Local Time {clientLocalTime} with Exception:\n  -Message: {ex.Message}\n  -StackTrace: {ex.StackTrace}");
            }
            return powerTradesExportDTOs;
        }
    }
}
