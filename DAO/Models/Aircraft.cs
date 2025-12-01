using System;
using System.Collections.Generic;

namespace DAO.Models;

public partial class Aircraft
{
    public int AircraftId { get; set; }

    public int AirlineId { get; set; }

    public string? Model { get; set; }

    public string? Manufacturer { get; set; }

    public int? Capacity { get; set; }

    public virtual Airline Airline { get; set; } = null!;

    public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();

    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
}
