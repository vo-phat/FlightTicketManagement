using GUI.Features.Auth;
using GUI.Features.Test;
using GUI.MainApp;

namespace GUI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new LoginForm());


            //Application.Run(new MainForm());
            //Application.Run(new Form1());
        }
    }
}