using System;
using System.Collections.Generic;

namespace DAO.Models;

public partial class TicketHistory
{
    public int HistoryId { get; set; }

    public int TicketId { get; set; }

    public string? OldStatus { get; set; }

    public string? NewStatus { get; set; }

    public DateTime ChangedAt { get; set; }

    public virtual Ticket Ticket { get; set; } = null!;
}
