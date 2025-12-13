using System;
using System.Collections.Generic;

namespace DAO.Models;

public partial class Route
{
    public int RouteId { get; set; }

    public int DeparturePlaceId { get; set; }

    public int ArrivalPlaceId { get; set; }

    public int? DistanceKm { get; set; }

    public int? DurationMinutes { get; set; }

    public virtual Airport ArrivalPlace { get; set; } = null!;

    public virtual Airport DeparturePlace { get; set; } = null!;

    public virtual ICollection<FareRule> FareRules { get; set; } = new List<FareRule>();

    public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();
}
