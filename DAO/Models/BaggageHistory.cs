using System;
using System.Collections.Generic;

namespace DAO.Models;

public partial class BaggageHistory
{
    public int HistoryId { get; set; }

    public int BaggageId { get; set; }

    public string? OldStatus { get; set; }

    public string? NewStatus { get; set; }

    public DateTime ChangedAt { get; set; }

    public virtual Baggage Baggage { get; set; } = null!;
}
