using FlightTicketManagement.GUI.Features.Auth;
using FlightTicketManagement.GUI.Features.Test;
using FlightTicketManagement.GUI.Features.MainApp;

namespace FlightTicketManagement.GUI.Features.MainApp {
    internal static class Program {
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //using (var login = new LoginForm()) {
            //    if (login.ShowDialog() == DialogResult.OK) {
            //        Application.Run(new MainForm());
            //    }
            //}

            Application.Run(new MainForm());
        }
    }
}
