using GUI.Features.Auth;
using GUI.Features.Test;
using GUI.MainApp;
using DAO.Database;

namespace GUI {
    internal static class Program {
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new LoginForm());
        }

    }
}