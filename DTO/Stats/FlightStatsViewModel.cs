namespace DTO.Stats
{
    public class FlightStatsViewModel
    {
        public int FlightId { get; set; }
        public string FlightCode { get; set; }
        public string Route { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
        public int TotalSeats { get; set; }
        public int BookedSeats { get; set; }
        public decimal OccupancyRate { get; set; }
        public int TotalPassengers { get; set; }
        public decimal Revenue { get; set; }
    }
}
