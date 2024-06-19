using System.Security.Cryptography;

namespace CovidLitSearch.Utilities;

public static class PasswordUtil
{
    public static string GenerateSalt()
    {
        var saltBytes = new byte[16];
        RandomNumberGenerator.Fill(saltBytes);
        return Convert.ToBase64String(saltBytes);
    }

    public static string Hash(string password, string salt)
    {
        var passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
        var saltBytes = Convert.FromBase64String(salt);

        var combinedBytes = new byte[passwordBytes.Length + saltBytes.Length];
        Buffer.BlockCopy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
        Buffer.BlockCopy(saltBytes, 0, combinedBytes, passwordBytes.Length, saltBytes.Length);
        var hashBytes = SHA256.HashData(combinedBytes);
        return Convert.ToBase64String(hashBytes);
    }

    public static bool Verify(string password, string salt, string hashedPassword)
    {
        var hashedInput = Hash(password, salt);
        return hashedInput == hashedPassword;
    }
}