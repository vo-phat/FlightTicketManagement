using System;
using System.Collections.Generic;

namespace DAO.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<PassengerProfile> PassengerProfiles { get; set; } = new List<PassengerProfile>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
