using System;
using System.Collections.Generic;

namespace DAO.Models;

public partial class Seat
{
    public int SeatId { get; set; }

    public int AircraftId { get; set; }

    public string SeatNumber { get; set; } = null!;

    public int ClassId { get; set; }

    public virtual Aircraft Aircraft { get; set; } = null!;

    public virtual CabinClass Class { get; set; } = null!;

    public virtual ICollection<FlightSeat> FlightSeats { get; set; } = new List<FlightSeat>();
}
