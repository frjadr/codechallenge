using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerServiceReporting.ApplicationCore.DTOs
{
    /// <summary>
    /// PowerTradeService response DTO model.
    /// </summary>
    public class PowerTradeDTO
    {
        public DateTime Date { get; set; }

        public PowerPeriodDTO[] Periods { get; set; } = new PowerPeriodDTO[0];
    }

    public class PowerPeriodDTO
    {
        public int Period { get; set; }

        public double Volume { get; set; }

    }
}
