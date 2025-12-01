using System;
using System.Collections.Generic;

namespace DAO.Models;

public partial class Flight
{
    public int FlightId { get; set; }

    public string FlightNumber { get; set; } = null!;

    public int AircraftId { get; set; }

    public int RouteId { get; set; }

    public DateTime? DepartureTime { get; set; }

    public DateTime? ArrivalTime { get; set; }

    public string Status { get; set; } = null!;

    public virtual Aircraft Aircraft { get; set; } = null!;

    public virtual ICollection<Baggage> Baggages { get; set; } = new List<Baggage>();

    public virtual ICollection<FlightSeat> FlightSeats { get; set; } = new List<FlightSeat>();

    public virtual Route Route { get; set; } = null!;
}
