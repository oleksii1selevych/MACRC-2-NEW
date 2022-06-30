using System.Security.Cryptography;

namespace Marc2.Helpers
{
    public static class PasswordHelper
    {
        private static int SaltSize = 16;
        private static int KeySize = 32;
        private static int Iterations = 1000;
        private static int PasswordLength = 32;

        public static string Hash(string password)
        {
            using (var algorithm = new Rfc2898DeriveBytes(
                password,
                SaltSize,
                Iterations,
                HashAlgorithmName.SHA256
                ))
            {
                var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
                var salt = Convert.ToBase64String(algorithm.Salt);

                return String.Format("{0}.{1}.{2}", Iterations, salt, key);
            }
        }

        public static bool CheckPassword(string passwordHash, string password)
        {
            var parts = passwordHash.Split('.', 3);

            if (parts.Length != 3)
                return false;

            var iterations = Convert.ToInt32(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);

            using (var algorithm = new Rfc2898DeriveBytes(
                    password,
                    salt,
                    iterations,
                    HashAlgorithmName.SHA256))
            {
                var keyToCheck = algorithm.GetBytes(KeySize);

                var verified = keyToCheck.SequenceEqual(key);

                return verified;
            }
        }
    }
}
