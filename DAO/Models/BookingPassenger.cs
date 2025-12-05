using System;
using System.Collections.Generic;

namespace DAO.Models;

public partial class BookingPassenger
{
    public int BookingPassengerId { get; set; }

    public int BookingId { get; set; }

    public int ProfileId { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual PassengerProfile Profile { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
