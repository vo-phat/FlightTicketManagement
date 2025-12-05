namespace GUI.Features.Setting {
    public static class DefaultRolePermissions {
        public static readonly string[] User = {
            Perm.Home_View,
            Perm.Flights_Read,
            Perm.Tickets_CreateSearch,
            Perm.Tickets_Mine,
            Perm.Tickets_History,
            Perm.Baggage_Checkin,
            Perm.Baggage_Track,
            Perm.Baggage_Report,
            Perm.Notifications_Read,
            Perm.Customers_Profiles
        };

        public static readonly string[] Staff = {
            Perm.Home_View,
            Perm.Flights_Read,
            Perm.Tickets_CreateSearch,
            Perm.Tickets_Mine,
            Perm.Tickets_Operate,
            Perm.Tickets_History,
            Perm.Baggage_Checkin,
            Perm.Baggage_Track,
            Perm.Baggage_Report,
            Perm.Customers_Profiles,
            Perm.Payments_Pos,
            Perm.Notifications_Read,
            Perm.Reports_View
        };

        public static readonly string[] Admin = {
            // Staff
            Perm.Home_View,
            Perm.Flights_Read,
            Perm.Tickets_CreateSearch,
            Perm.Tickets_Mine,
            Perm.Tickets_Operate,
            Perm.Tickets_History,
            Perm.Baggage_Checkin,
            Perm.Baggage_Track,
            Perm.Baggage_Report,
            Perm.Customers_Profiles,
            Perm.Payments_Pos,
            Perm.Notifications_Read,
            Perm.Reports_View,

            // Master data & system
            Perm.FareRules_Manage,
            Perm.Catalogs_Airlines,
            Perm.Catalogs_Aircrafts,
            Perm.Catalogs_Airports,
            Perm.Catalogs_Routes,
            Perm.Catalogs_CabinClasses,
            Perm.Catalogs_Seats,
            Perm.Accounts_Manage,
            Perm.System_Roles
        };
    }
}
