using System;
using System.Collections.Generic;

namespace DAO.Models;

public partial class Baggage
{
    public int BaggageId { get; set; }

    public int TicketId { get; set; }

    public int FlightId { get; set; }

    public string? BaggageTag { get; set; }

    public string BaggageType { get; set; } = null!;

    public decimal WeightKg { get; set; }

    public decimal AllowedWeightKg { get; set; }

    public decimal Fee { get; set; }

    public string Status { get; set; } = null!;

    public string? SpecialHandling { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<BaggageHistory> BaggageHistories { get; set; } = new List<BaggageHistory>();

    public virtual Flight Flight { get; set; } = null!;

    public virtual Ticket Ticket { get; set; } = null!;
}
