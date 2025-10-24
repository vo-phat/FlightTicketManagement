using GUI.Features.Auth;
using GUI.Features.Test;
using GUI.MainApp;

namespace GUI {
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