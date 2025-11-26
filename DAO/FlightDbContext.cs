using System;
using System.Collections.Generic;
using DAO.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace DAO;

public partial class FlightDbContext : DbContext
{
    public FlightDbContext()
    {
    }

    public FlightDbContext(DbContextOptions<FlightDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Models.Account> Accounts { get; set; }

    public virtual DbSet<Models.Aircraft> Aircrafts { get; set; }

    public virtual DbSet<Models.Airline> Airlines { get; set; }

    public virtual DbSet<Models.Airport> Airports { get; set; }

    public virtual DbSet<Models.Baggage> Baggages { get; set; }

    public virtual DbSet<Models.BaggageHistory> BaggageHistories { get; set; }

    public virtual DbSet<Models.Booking> Bookings { get; set; }

    public virtual DbSet<Models.BookingPassenger> BookingPassengers { get; set; }

    public virtual DbSet<Models.CabinClass> CabinClasses { get; set; }

    public virtual DbSet<Models.FareRule> FareRules { get; set; }

    public virtual DbSet<Models.Flight> Flights { get; set; }

    public virtual DbSet<Models.FlightSeat> FlightSeats { get; set; }

    public virtual DbSet<Models.PassengerProfile> PassengerProfiles { get; set; }

    public virtual DbSet<Models.Payment> Payments { get; set; }

    public virtual DbSet<Models.Role> Roles { get; set; }

    public virtual DbSet<Models.Route> Routes { get; set; }

    public virtual DbSet<Models.Seat> Seats { get; set; }

    public virtual DbSet<Models.Ticket> Tickets { get; set; }

    public virtual DbSet<Models.TicketHistory> TicketHistories { get; set; }

    public virtual DbSet<Models.UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=flightticketmanagement;user=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.32-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Models.Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PRIMARY");

            entity.ToTable("accounts");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.Property(e => e.AccountId)
                .HasColumnType("int(11)")
                .HasColumnName("account_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
        });

        modelBuilder.Entity<Models.Aircraft>(entity =>
        {
            entity.HasKey(e => e.AircraftId).HasName("PRIMARY");

            entity.ToTable("aircrafts");

            entity.HasIndex(e => e.AirlineId, "idx_aircraft_airline");

            entity.Property(e => e.AircraftId)
                .HasColumnType("int(11)")
                .HasColumnName("aircraft_id");
            entity.Property(e => e.AirlineId)
                .HasColumnType("int(11)")
                .HasColumnName("airline_id");
            entity.Property(e => e.Capacity)
                .HasColumnType("int(11)")
                .HasColumnName("capacity");
            entity.Property(e => e.Manufacturer)
                .HasMaxLength(100)
                .HasColumnName("manufacturer");
            entity.Property(e => e.Model)
                .HasMaxLength(100)
                .HasColumnName("model");

            entity.HasOne(d => d.Airline).WithMany(p => p.Aircraft)
                .HasForeignKey(d => d.AirlineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_aircraft_airline");
        });

        modelBuilder.Entity<Models.Airline>(entity =>
        {
            entity.HasKey(e => e.AirlineId).HasName("PRIMARY");

            entity.ToTable("airlines");

            entity.HasIndex(e => e.AirlineCode, "airline_code").IsUnique();

            entity.Property(e => e.AirlineId)
                .HasColumnType("int(11)")
                .HasColumnName("airline_id");
            entity.Property(e => e.AirlineCode)
                .HasMaxLength(10)
                .HasColumnName("airline_code");
            entity.Property(e => e.AirlineName)
                .HasMaxLength(100)
                .HasColumnName("airline_name");
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .HasColumnName("country");
        });

        modelBuilder.Entity<Models.Airport>(entity =>
        {
            entity.HasKey(e => e.AirportId).HasName("PRIMARY");

            entity.ToTable("airports");

            entity.HasIndex(e => e.AirportCode, "airport_code").IsUnique();

            entity.Property(e => e.AirportId)
                .HasColumnType("int(11)")
                .HasColumnName("airport_id");
            entity.Property(e => e.AirportCode)
                .HasMaxLength(10)
                .HasColumnName("airport_code");
            entity.Property(e => e.AirportName)
                .HasMaxLength(100)
                .HasColumnName("airport_name");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .HasColumnName("country");
        });

        modelBuilder.Entity<Models.Baggage>(entity =>
        {
            entity.HasKey(e => e.BaggageId).HasName("PRIMARY");

            entity.ToTable("baggage");

            entity.HasIndex(e => e.BaggageTag, "baggage_tag").IsUnique();

            entity.HasIndex(e => e.FlightId, "idx_baggage_flight");

            entity.HasIndex(e => e.Status, "idx_baggage_status");

            entity.HasIndex(e => e.TicketId, "idx_baggage_ticket");

            entity.Property(e => e.BaggageId)
                .HasColumnType("int(11)")
                .HasColumnName("baggage_id");
            entity.Property(e => e.AllowedWeightKg)
                .HasPrecision(5, 2)
                .HasColumnName("allowed_weight_kg");
            entity.Property(e => e.BaggageTag)
                .HasMaxLength(20)
                .HasColumnName("baggage_tag");
            entity.Property(e => e.BaggageType)
                .HasDefaultValueSql("'CHECKED'")
                .HasColumnType("enum('CHECKED','CARRY_ON','SPECIAL')")
                .HasColumnName("baggage_type");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Fee)
                .HasPrecision(12, 2)
                .HasColumnName("fee");
            entity.Property(e => e.FlightId)
                .HasColumnType("int(11)")
                .HasColumnName("flight_id");
            entity.Property(e => e.SpecialHandling)
                .HasMaxLength(100)
                .HasColumnName("special_handling");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'CREATED'")
                .HasColumnType("enum('CREATED','CHECKED_IN','LOADED','IN_TRANSIT','CLAIMED','LOST')")
                .HasColumnName("status");
            entity.Property(e => e.TicketId)
                .HasColumnType("int(11)")
                .HasColumnName("ticket_id");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.WeightKg)
                .HasPrecision(5, 2)
                .HasColumnName("weight_kg");

            entity.HasOne(d => d.Flight).WithMany(p => p.Baggages)
                .HasForeignKey(d => d.FlightId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_baggage_flight");

            entity.HasOne(d => d.Ticket).WithMany(p => p.Baggages)
                .HasForeignKey(d => d.TicketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_baggage_ticket");
        });

        modelBuilder.Entity<Models.BaggageHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PRIMARY");

            entity.ToTable("baggage_history");

            entity.HasIndex(e => e.BaggageId, "idx_bh_baggage");

            entity.Property(e => e.HistoryId)
                .HasColumnType("int(11)")
                .HasColumnName("history_id");
            entity.Property(e => e.BaggageId)
                .HasColumnType("int(11)")
                .HasColumnName("baggage_id");
            entity.Property(e => e.ChangedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("changed_at");
            entity.Property(e => e.NewStatus)
                .HasColumnType("enum('CREATED','CHECKED_IN','LOADED','IN_TRANSIT','CLAIMED','LOST')")
                .HasColumnName("new_status");
            entity.Property(e => e.OldStatus)
                .HasColumnType("enum('CREATED','CHECKED_IN','LOADED','IN_TRANSIT','CLAIMED','LOST')")
                .HasColumnName("old_status");

            entity.HasOne(d => d.Baggage).WithMany(p => p.BaggageHistories)
                .HasForeignKey(d => d.BaggageId)
                .HasConstraintName("fk_bh_baggage");
        });

        modelBuilder.Entity<Models.Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PRIMARY");

            entity.ToTable("bookings");

            entity.HasIndex(e => e.AccountId, "idx_booking_account");

            entity.HasIndex(e => e.Status, "idx_booking_status");

            entity.Property(e => e.BookingId)
                .HasColumnType("int(11)")
                .HasColumnName("booking_id");
            entity.Property(e => e.AccountId)
                .HasColumnType("int(11)")
                .HasColumnName("account_id");
            entity.Property(e => e.BookingDate)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("booking_date");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'PENDING'")
                .HasColumnType("enum('PENDING','CONFIRMED','CANCELLED','REFUNDED')")
                .HasColumnName("status");
            entity.Property(e => e.TotalAmount)
                .HasPrecision(12, 2)
                .HasColumnName("total_amount");

            entity.HasOne(d => d.Account).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_booking_account");
        });

        modelBuilder.Entity<Models.BookingPassenger>(entity =>
        {
            entity.HasKey(e => e.BookingPassengerId).HasName("PRIMARY");

            entity.ToTable("booking_passengers");

            entity.HasIndex(e => e.ProfileId, "fk_bp_profile");

            entity.HasIndex(e => new { e.BookingId, e.ProfileId }, "ux_bp_booking_profile").IsUnique();

            entity.Property(e => e.BookingPassengerId)
                .HasColumnType("int(11)")
                .HasColumnName("booking_passenger_id");
            entity.Property(e => e.BookingId)
                .HasColumnType("int(11)")
                .HasColumnName("booking_id");
            entity.Property(e => e.ProfileId)
                .HasColumnType("int(11)")
                .HasColumnName("profile_id");

            entity.HasOne(d => d.Booking).WithMany(p => p.BookingPassengers)
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("fk_bp_booking");

            entity.HasOne(d => d.Profile).WithMany(p => p.BookingPassengers)
                .HasForeignKey(d => d.ProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bp_profile");
        });

        modelBuilder.Entity<Models.CabinClass>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PRIMARY");

            entity.ToTable("cabin_classes");

            entity.HasIndex(e => e.ClassName, "ux_cabin_class_name").IsUnique();

            entity.Property(e => e.ClassId)
                .HasColumnType("int(11)")
                .HasColumnName("class_id");
            entity.Property(e => e.ClassName)
                .HasMaxLength(50)
                .HasColumnName("class_name");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
        });

        modelBuilder.Entity<Models.FareRule>(entity =>
        {
            entity.HasKey(e => e.RuleId).HasName("PRIMARY");

            entity.ToTable("fare_rules");

            entity.HasIndex(e => e.ClassId, "fk_fare_class");

            entity.HasIndex(e => new { e.RouteId, e.ClassId }, "idx_fare_route_class");

            entity.Property(e => e.RuleId)
                .HasColumnType("int(11)")
                .HasColumnName("rule_id");
            entity.Property(e => e.ClassId)
                .HasColumnType("int(11)")
                .HasColumnName("class_id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.EffectiveDate).HasColumnName("effective_date");
            entity.Property(e => e.ExpiryDate).HasColumnName("expiry_date");
            entity.Property(e => e.FareType)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Standard'")
                .HasColumnName("fare_type");
            entity.Property(e => e.Price)
                .HasPrecision(12, 2)
                .HasColumnName("price");
            entity.Property(e => e.RouteId)
                .HasColumnType("int(11)")
                .HasColumnName("route_id");
            entity.Property(e => e.Season)
                .HasDefaultValueSql("'NORMAL'")
                .HasColumnType("enum('PEAK','OFFPEAK','NORMAL')")
                .HasColumnName("season");

            entity.HasOne(d => d.Class).WithMany(p => p.FareRules)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_fare_class");

            entity.HasOne(d => d.Route).WithMany(p => p.FareRules)
                .HasForeignKey(d => d.RouteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_fare_route");
        });

        modelBuilder.Entity<Models.Flight>(entity =>
        {
            entity.HasKey(e => e.FlightId).HasName("PRIMARY");

            entity.ToTable("flights");

            entity.HasIndex(e => e.AircraftId, "idx_flight_aircraft");

            entity.HasIndex(e => e.RouteId, "idx_flight_route");

            entity.HasIndex(e => new { e.FlightNumber, e.DepartureTime }, "ux_flight_number_time").IsUnique();

            entity.Property(e => e.FlightId)
                .HasColumnType("int(11)")
                .HasColumnName("flight_id");
            entity.Property(e => e.AircraftId)
                .HasColumnType("int(11)")
                .HasColumnName("aircraft_id");
            entity.Property(e => e.ArrivalTime)
                .HasColumnType("datetime")
                .HasColumnName("arrival_time");
            entity.Property(e => e.DepartureTime)
                .HasColumnType("datetime")
                .HasColumnName("departure_time");
            entity.Property(e => e.FlightNumber)
                .HasMaxLength(20)
                .HasColumnName("flight_number");
            entity.Property(e => e.RouteId)
                .HasColumnType("int(11)")
                .HasColumnName("route_id");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'SCHEDULED'")
                .HasColumnType("enum('SCHEDULED','DELAYED','CANCELLED','COMPLETED')")
                .HasColumnName("status");

            entity.HasOne(d => d.Aircraft).WithMany(p => p.Flights)
                .HasForeignKey(d => d.AircraftId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_flight_aircraft");

            entity.HasOne(d => d.Route).WithMany(p => p.Flights)
                .HasForeignKey(d => d.RouteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_flight_route");
        });

        modelBuilder.Entity<Models.FlightSeat>(entity =>
        {
            entity.HasKey(e => e.FlightSeatId).HasName("PRIMARY");

            entity.ToTable("flight_seats");

            entity.HasIndex(e => e.SeatId, "fk_fseat_seat");

            entity.HasIndex(e => e.SeatStatus, "idx_fseat_status");

            entity.HasIndex(e => new { e.FlightId, e.SeatId }, "ux_fseat_flight_seat").IsUnique();

            entity.Property(e => e.FlightSeatId)
                .HasColumnType("int(11)")
                .HasColumnName("flight_seat_id");
            entity.Property(e => e.BasePrice)
                .HasPrecision(12, 2)
                .HasColumnName("base_price");
            entity.Property(e => e.FlightId)
                .HasColumnType("int(11)")
                .HasColumnName("flight_id");
            entity.Property(e => e.SeatId)
                .HasColumnType("int(11)")
                .HasColumnName("seat_id");
            entity.Property(e => e.SeatStatus)
                .HasDefaultValueSql("'AVAILABLE'")
                .HasColumnType("enum('AVAILABLE','BOOKED','BLOCKED')")
                .HasColumnName("seat_status");

            entity.HasOne(d => d.Flight).WithMany(p => p.FlightSeats)
                .HasForeignKey(d => d.FlightId)
                .HasConstraintName("fk_fseat_flight");

            entity.HasOne(d => d.Seat).WithMany(p => p.FlightSeats)
                .HasForeignKey(d => d.SeatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_fseat_seat");
        });

        modelBuilder.Entity<Models.PassengerProfile>(entity =>
        {
            entity.HasKey(e => e.ProfileId).HasName("PRIMARY");

            entity.ToTable("passenger_profiles");

            entity.HasIndex(e => e.AccountId, "idx_profile_account");

            entity.HasIndex(e => e.PassportNumber, "idx_profile_passport");

            entity.Property(e => e.ProfileId)
                .HasColumnType("int(11)")
                .HasColumnName("profile_id");
            entity.Property(e => e.AccountId)
                .HasColumnType("int(11)")
                .HasColumnName("account_id");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("full_name");
            entity.Property(e => e.Nationality)
                .HasMaxLength(50)
                .HasColumnName("nationality");
            entity.Property(e => e.PassportNumber)
                .HasMaxLength(50)
                .HasColumnName("passport_number");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(255)
                .HasColumnName("phone_number");

            entity.HasOne(d => d.Account).WithMany(p => p.PassengerProfiles)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_profile_account");
        });

        modelBuilder.Entity<Models.Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PRIMARY");

            entity.ToTable("payments");

            entity.HasIndex(e => e.BookingId, "idx_payment_booking");

            entity.HasIndex(e => e.Status, "idx_payment_status");

            entity.Property(e => e.PaymentId)
                .HasColumnType("int(11)")
                .HasColumnName("payment_id");
            entity.Property(e => e.Amount)
                .HasPrecision(12, 2)
                .HasColumnName("amount");
            entity.Property(e => e.BookingId)
                .HasColumnType("int(11)")
                .HasColumnName("booking_id");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("payment_date");
            entity.Property(e => e.PaymentMethod)
                .HasColumnType("enum('CREDIT_CARD','BANK_TRANSFER','E_WALLET','CASH')")
                .HasColumnName("payment_method");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'PENDING'")
                .HasColumnType("enum('SUCCESS','FAILED','PENDING')")
                .HasColumnName("status");

            entity.HasOne(d => d.Booking).WithMany(p => p.Payments)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_payment_booking");
        });

        modelBuilder.Entity<Models.Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.HasIndex(e => e.RoleName, "role_name").IsUnique();

            entity.Property(e => e.RoleId)
                .HasColumnType("int(11)")
                .HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Models.Route>(entity =>
        {
            entity.HasKey(e => e.RouteId).HasName("PRIMARY");

            entity.ToTable("routes");

            entity.HasIndex(e => e.ArrivalPlaceId, "idx_routes_arr");

            entity.HasIndex(e => e.DeparturePlaceId, "idx_routes_dep");

            entity.Property(e => e.RouteId)
                .HasColumnType("int(11)")
                .HasColumnName("route_id");
            entity.Property(e => e.ArrivalPlaceId)
                .HasColumnType("int(11)")
                .HasColumnName("arrival_place_id");
            entity.Property(e => e.DeparturePlaceId)
                .HasColumnType("int(11)")
                .HasColumnName("departure_place_id");
            entity.Property(e => e.DistanceKm)
                .HasColumnType("int(11)")
                .HasColumnName("distance_km");
            entity.Property(e => e.DurationMinutes)
                .HasColumnType("int(11)")
                .HasColumnName("duration_minutes");

            entity.HasOne(d => d.ArrivalPlace).WithMany(p => p.RouteArrivalPlaces)
                .HasForeignKey(d => d.ArrivalPlaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_routes_arr");

            entity.HasOne(d => d.DeparturePlace).WithMany(p => p.RouteDeparturePlaces)
                .HasForeignKey(d => d.DeparturePlaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_routes_dep");
        });

        modelBuilder.Entity<Models.Seat>(entity =>
        {
            entity.HasKey(e => e.SeatId).HasName("PRIMARY");

            entity.ToTable("seats");

            entity.HasIndex(e => e.ClassId, "idx_seat_class");

            entity.HasIndex(e => new { e.AircraftId, e.SeatNumber }, "ux_seat_aircraft_number").IsUnique();

            entity.Property(e => e.SeatId)
                .HasColumnType("int(11)")
                .HasColumnName("seat_id");
            entity.Property(e => e.AircraftId)
                .HasColumnType("int(11)")
                .HasColumnName("aircraft_id");
            entity.Property(e => e.ClassId)
                .HasColumnType("int(11)")
                .HasColumnName("class_id");
            entity.Property(e => e.SeatNumber)
                .HasMaxLength(10)
                .HasColumnName("seat_number");

            entity.HasOne(d => d.Aircraft).WithMany(p => p.Seats)
                .HasForeignKey(d => d.AircraftId)
                .HasConstraintName("fk_seat_aircraft");

            entity.HasOne(d => d.Class).WithMany(p => p.Seats)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_seat_class");
        });

        modelBuilder.Entity<Models.Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PRIMARY");

            entity.ToTable("tickets");

            entity.HasIndex(e => e.TicketPassengerId, "fk_ticket_bp");

            entity.HasIndex(e => e.Status, "idx_ticket_status");

            entity.HasIndex(e => e.TicketNumber, "ticket_number").IsUnique();

            entity.HasIndex(e => e.FlightSeatId, "ux_ticket_fseat").IsUnique();

            entity.Property(e => e.TicketId)
                .HasColumnType("int(11)")
                .HasColumnName("ticket_id");
            entity.Property(e => e.FlightSeatId)
                .HasColumnType("int(11)")
                .HasColumnName("flight_seat_id");
            entity.Property(e => e.IssueDate)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("issue_date");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'BOOKED'")
                .HasColumnType("enum('BOOKED','CONFIRMED','CHECKED_IN','BOARDED','CANCELLED','REFUNDED')")
                .HasColumnName("status");
            entity.Property(e => e.TicketNumber)
                .HasMaxLength(50)
                .HasColumnName("ticket_number");
            entity.Property(e => e.TicketPassengerId)
                .HasColumnType("int(11)")
                .HasColumnName("ticket_passenger_id");

            entity.HasOne(d => d.FlightSeat).WithOne(p => p.Ticket)
                .HasForeignKey<Ticket>(d => d.FlightSeatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ticket_fseat");

            entity.HasOne(d => d.TicketPassenger).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.TicketPassengerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ticket_bp");
        });

        modelBuilder.Entity<Models.TicketHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PRIMARY");

            entity.ToTable("ticket_history");

            entity.HasIndex(e => e.TicketId, "idx_th_ticket");

            entity.Property(e => e.HistoryId)
                .HasColumnType("int(11)")
                .HasColumnName("history_id");
            entity.Property(e => e.ChangedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("changed_at");
            entity.Property(e => e.NewStatus)
                .HasColumnType("enum('BOOKED','CONFIRMED','CHECKED_IN','BOARDED','CANCELLED','REFUNDED')")
                .HasColumnName("new_status");
            entity.Property(e => e.OldStatus)
                .HasColumnType("enum('BOOKED','CONFIRMED','CHECKED_IN','BOARDED','CANCELLED','REFUNDED')")
                .HasColumnName("old_status");
            entity.Property(e => e.TicketId)
                .HasColumnType("int(11)")
                .HasColumnName("ticket_id");

            entity.HasOne(d => d.Ticket).WithMany(p => p.TicketHistories)
                .HasForeignKey(d => d.TicketId)
                .HasConstraintName("fk_th_ticket");
        });

        modelBuilder.Entity<Models.UserRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleId).HasName("PRIMARY");

            entity.ToTable("user_role");

            entity.HasIndex(e => e.AccountId, "idx_userrole_account");

            entity.HasIndex(e => e.RoleId, "idx_userrole_role");

            entity.Property(e => e.UserRoleId)
                .HasColumnType("int(11)")
                .HasColumnName("user_role_id");
            entity.Property(e => e.AccountId)
                .HasColumnType("int(11)")
                .HasColumnName("account_id");
            entity.Property(e => e.RoleId)
                .HasColumnType("int(11)")
                .HasColumnName("role_id");

            entity.HasOne(d => d.Account).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("fk_userrole_account");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_userrole_role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
