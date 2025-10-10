using FlightTicketManagement.GUI.Features.Auth;
using FlightTicketManagement.GUI.Features.Test;

namespace FlightTicketManagement {
    internal static class Program {
        [STAThread]
        static void Main() {
            ApplicationConfiguration.Initialize();

            //Application.Run(new TestControl());

            Application.Run(new LoginForm());
        }
    }
}
