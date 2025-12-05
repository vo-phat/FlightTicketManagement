using System;
using System.Collections.Generic;

namespace DAO.Models;

public partial class Ticket
{
    public int TicketId { get; set; }

    public int TicketPassengerId { get; set; }

    public int FlightSeatId { get; set; }

    public string? TicketNumber { get; set; }

    public DateTime IssueDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Baggage> Baggages { get; set; } = new List<Baggage>();

    public virtual FlightSeat FlightSeat { get; set; } = null!;

    public virtual ICollection<TicketHistory> TicketHistories { get; set; } = new List<TicketHistory>();

    public virtual BookingPassenger TicketPassenger { get; set; } = null!;
}
