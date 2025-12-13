using System;
using System.Collections.Generic;

namespace DAO.Models;

public partial class FlightSeat
{
    public int FlightSeatId { get; set; }

    public int FlightId { get; set; }

    public int SeatId { get; set; }

    public decimal BasePrice { get; set; }

    public string SeatStatus { get; set; } = null!;

    public virtual Flight Flight { get; set; } = null!;

    public virtual Seat Seat { get; set; } = null!;

    public virtual Ticket? Ticket { get; set; }
}
