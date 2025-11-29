using System;
using System.Security.Cryptography;
using System.Text;

namespace BUS.Security {
    public static class PasswordHasher {
        public static string Hash(string password) {
            if (password == null) throw new ArgumentNullException(nameof(password));

            using (var sha = SHA256.Create()) {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }

        public static bool Verify(string password, string hashedPassword) {
            if (hashedPassword == null) return false;
            return Hash(password) == hashedPassword;
        }
    }
}
