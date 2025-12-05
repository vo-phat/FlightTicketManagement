using System;
using System.Collections.Generic;

namespace DAO.Models;

public partial class PassengerProfile
{
    public int ProfileId { get; set; }

    public int AccountId { get; set; }

    public string FullName { get; set; } = null!;

    public DateOnly? DateOfBirth { get; set; }

    public string? PhoneNumber { get; set; }

    public string? PassportNumber { get; set; }

    public string? Nationality { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<BookingPassenger> BookingPassengers { get; set; } = new List<BookingPassenger>();
}
