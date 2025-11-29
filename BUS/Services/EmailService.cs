using System.Net;
using System.Net.Mail;

namespace BUS.Services {
    public static class EmailService {
        // TODO: sửa lại theo SMTP thật của bạn
        private const string SMTP_HOST = "smtp.gmail.com";
        private const int SMTP_PORT = 587;
        private const string SMTP_USER = "vootanphat@gmail.com";
        private const string SMTP_PASS = "iilk deea dtds pycc"; // App Password, không dùng mật khẩu thường

        public static void SendOtp(string toEmail, string otpCode) {
            var msg = new MailMessage();
            msg.From = new MailAddress(SMTP_USER, "Flight Ticket Management");
            msg.To.Add(new MailAddress(toEmail));
            msg.Subject = "Mã xác thực đặt lại mật khẩu";
            msg.Body = $"Mã xác thực của bạn là: {otpCode}\nMã có hiệu lực trong 10 phút.";

            using (var client = new SmtpClient(SMTP_HOST, SMTP_PORT)) {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(SMTP_USER, SMTP_PASS);
                client.Send(msg);
            }
        }
    }
}
