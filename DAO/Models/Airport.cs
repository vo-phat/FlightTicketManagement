using System;
using System.Collections.Generic;

namespace DAO.Models;

public partial class Airport
{
    public int AirportId { get; set; }

    public string AirportCode { get; set; } = null!;

    public string AirportName { get; set; } = null!;

    public string? City { get; set; }

    public string? Country { get; set; }

    public virtual ICollection<Route> RouteArrivalPlaces { get; set; } = new List<Route>();

    public virtual ICollection<Route> RouteDeparturePlaces { get; set; } = new List<Route>();
}
