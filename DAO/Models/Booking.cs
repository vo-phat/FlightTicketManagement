using System;
using System.Collections.Generic;

namespace DAO.Models;

public partial class Booking
{
    public int BookingId { get; set; }

    public int AccountId { get; set; }

    public DateTime BookingDate { get; set; }

    public string Status { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<BookingPassenger> BookingPassengers { get; set; } = new List<BookingPassenger>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
