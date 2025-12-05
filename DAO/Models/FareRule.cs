using System;
using System.Collections.Generic;

namespace DAO.Models;

public partial class FareRule
{
    public int RuleId { get; set; }

    public int RouteId { get; set; }

    public int ClassId { get; set; }

    public string FareType { get; set; } = null!;

    public string Season { get; set; } = null!;

    public DateOnly? EffectiveDate { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public virtual CabinClass Class { get; set; } = null!;

    public virtual Route Route { get; set; } = null!;
}
