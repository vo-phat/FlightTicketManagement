using System.Collections.Generic;

namespace DTO.Stats
{
    public class FlightStatsReportViewModel
    {
        public int TotalFlights { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalPassengers { get; set; }
        public decimal AverageOccupancyRate { get; set; }
        public List<FlightStatsViewModel> FlightDetails { get; set; } = new List<FlightStatsViewModel>();
    }
}
