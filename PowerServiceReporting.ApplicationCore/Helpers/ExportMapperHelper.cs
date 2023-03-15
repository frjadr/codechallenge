using PowerServiceReporting.ApplicationCore.DTOs;
using Serilog;
using System.Reflection;

namespace PowerServiceReporting.ApplicationCore.Helpers
{
    public static class ExportMapperHelper
    {
        private static readonly Dictionary<int, int> PeriodHourMap = new Dictionary<int, int>
        {
            {0, 0}, {1, -13}, {2, 0}, {3, 1}, {4, 2}, {5, 3}, {6, 4}, {7, 5}, {8, 6}, {9, 7}, {10, 8}, {11, 9}, {12, 10},
            {13, 11}, {14, 12}, {15, 13}, {16, 14}, {17, 15}, {18, 16}, {19, 17}, {20, 18}, {21, 19}, {22, 20}, {23, 21}, {24, 22},
        };

        public static (DateTime MergeDate, string PeriodHour) MergeDateWithPeriod(DateTime powerTradeDate, int period)
        {
            var mergeDate = new DateTime(powerTradeDate.Year, powerTradeDate.Month, powerTradeDate.Day).AddHours(PeriodHourMap[period]);
            var periodHour = (mergeDate.Hour.ToString().Length == 1 ? ("0" + mergeDate.Hour.ToString()) : mergeDate.Hour.ToString()) + ":" + (mergeDate.Minute == 0 ? "00" : mergeDate.Minute.ToString());

            return (mergeDate, periodHour);
        }

        public static List<PowerTradeExportDTO> MapPowerTradesToPowerTradesExportAggregated(this List<PowerTradeDTO> powerTrades, DateTime clientLocalTime)
        {
            var powerTradesExport = new List<PowerTradeExportDTO>();
            try
            {
                powerTradesExport = powerTrades.SelectMany(powerTrade =>
                {
                    return powerTrade.Periods.Select(period =>
                    {
                        var (mergeDate, periodHour) = MergeDateWithPeriod(powerTrade.Date, period.Period);
                        return new PowerTradeExportDTO
                        {
                            LocalClientTimeOriginal = powerTrade.Date,
                            LocalClientTimeWithPeriod = mergeDate,
                            Period = periodHour,
                            Volume = period.Volume
                        };
                    });
                }).OrderBy(pt => pt.LocalClientTimeWithPeriod).Where(pt => pt.LocalClientTimeOriginal.Date == pt.LocalClientTimeWithPeriod.Date).ToList();

                powerTradesExport = powerTradesExport.GroupBy(pt => new { pt.Period })
                .Select(group =>
                {
                    var first = group.First();
                    return new PowerTradeExportDTO
                    {
                        Period = first.Period,
                        Volume = group.Sum(pt => pt.Volume)
                    };
                })
                .OrderBy(pt => pt.Period)
                .ToList();
            }
            catch (Exception ex)
            {
                Log.Error($"[{Assembly.GetEntryAssembly().GetName().Name}] => [{typeof(ExportMapperHelper).Name}.{ReflectionHelper.GetActualAsyncMethodName()}]" +
                    $" - failed at Client Local Time {clientLocalTime} with Exception:\n  -Message: {ex.Message}\n  -StackTrace: {ex.StackTrace}");
            }
            return powerTradesExport;
        }

        public static List<PowerTradeExportDTO> MapPowerTradesToPowerTradesExportNonAggregated(this List<PowerTradeDTO> powerTrades, DateTime clientLocalTime)
        {
            var powerTradesExport = new List<PowerTradeExportDTO>();
            try
            {
                powerTradesExport = powerTrades.SelectMany(powerTrade =>
                {
                    return powerTrade.Periods.Select(period =>
                    {
                        var (mergeDate, periodHour) = MergeDateWithPeriod(powerTrade.Date, period.Period);
                        return new PowerTradeExportDTO
                        {
                            LocalClientTimeOriginal = powerTrade.Date,
                            LocalClientTimeWithPeriod = mergeDate,
                            Period = periodHour,
                            Volume = period.Volume
                        };
                    });
                }).OrderBy(pt => pt.LocalClientTimeWithPeriod).Where(pt => pt.LocalClientTimeOriginal.Date == pt.LocalClientTimeWithPeriod.Date).ToList();
            }
            catch (Exception ex)
            {
                Log.Error($"[{Assembly.GetEntryAssembly().GetName().Name}] => [{typeof(ExportMapperHelper).Name}.{ReflectionHelper.GetActualAsyncMethodName()}]" +
                    $" - failed at Client Local Time {clientLocalTime} with Exception:\n  -Message: {ex.Message}\n  -StackTrace: {ex.StackTrace}");
            }
            return powerTradesExport;
        }
    }
}
