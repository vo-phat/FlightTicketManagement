using System;

namespace DTO.FlightSeat
{
    public class FlightSeatDTO
    {
        public int FlightSeatId { get; set; }
        public int FlightId { get; set; }
        public int AircraftId { get; set; }
        public int SeatId { get; set; }
        public int ClassId { get; set; }
        public decimal BasePrice { get; set; }
        public string SeatStatus { get; set; }
        public string FlightName { get; set; }
        public string AircraftName { get; set; }
        public string SeatNumber { get; set; }
        public string ClassName { get; set; }

        public FlightSeatDTO()
        {
            SeatStatus = "available";
            FlightName = string.Empty;
            AircraftName = string.Empty;
            SeatNumber = string.Empty;
            ClassName = string.Empty;
        }

        // Constructor 5 tham số (basic)
        public FlightSeatDTO(int flightSeatId, int flightId, int seatId, decimal basePrice, string seatStatus)
        {
            FlightSeatId = flightSeatId;
            FlightId = flightId;
            SeatId = seatId;
            BasePrice = basePrice;
            SeatStatus = seatStatus;
            FlightName = string.Empty;
            AircraftName = string.Empty;
            SeatNumber = string.Empty;
            ClassName = string.Empty;
        }

        // Constructor 9 tham số (for GetByFlightId)
        public FlightSeatDTO(int flightSeatId, int flightId, int seatId, int classId, 
            decimal basePrice, string seatStatus, string flightName, string seatNumber, string className)
        {
            FlightSeatId = flightSeatId;
            FlightId = flightId;
            SeatId = seatId;
            ClassId = classId;
            BasePrice = basePrice;
            SeatStatus = seatStatus;
            FlightName = flightName;
            AircraftName = string.Empty;
            SeatNumber = seatNumber;
            ClassName = className;
        }

        // Constructor 11 tham số (for GetAllFlightSeats)
        public FlightSeatDTO(int flightSeatId, int flightId, int aircraftId, int seatId, int classId,
            decimal basePrice, string seatStatus, string flightName, string aircraftName, 
            string seatNumber, string className)
        {
            FlightSeatId = flightSeatId;
            FlightId = flightId;
            AircraftId = aircraftId;
            SeatId = seatId;
            ClassId = classId;
            BasePrice = basePrice;
            SeatStatus = seatStatus;
            FlightName = flightName;
            AircraftName = aircraftName;
            SeatNumber = seatNumber;
            ClassName = className;
        }
    }
}
