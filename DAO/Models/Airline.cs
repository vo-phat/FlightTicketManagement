using System;
using System.Collections.Generic;

namespace DAO.Models;

public partial class Airline
{
    public int AirlineId { get; set; }

    public string AirlineCode { get; set; } = null!;

    public string AirlineName { get; set; } = null!;

    public string? Country { get; set; }

    public virtual ICollection<Aircraft> Aircraft { get; set; } = new List<Aircraft>();
}
