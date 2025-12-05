using System;
using System.Collections.Generic;

namespace DAO.Models;

public partial class CabinClass
{
    public int ClassId { get; set; }

    public string ClassName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<FareRule> FareRules { get; set; } = new List<FareRule>();

    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
}
