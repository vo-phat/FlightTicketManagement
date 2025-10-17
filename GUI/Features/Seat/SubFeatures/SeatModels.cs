namespace FlightTicketManagement.GUI.Features.Seat.SubFeatures {
    public enum SeatStatus { Available, Booked, Blocked }

    public class SeatVM {
        public int Row;
        public string Col = "A";
        public string Cabin = "Economy";
        public string Class = "Economy";
        public SeatStatus Status = SeatStatus.Available;
        public decimal BasePrice = 0m;
        public string FareCode = "";
        public string? PNR;
        public string? Note;
        public override string ToString() => $"{Row}{Col}";
    }
}
