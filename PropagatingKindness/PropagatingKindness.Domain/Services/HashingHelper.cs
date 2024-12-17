using System.Security.Cryptography;
using System.Text;

namespace PropagatingKindness.Domain.Services;

public static class HashingHelper
{
    public const string SALT_SEPARATOR = "|||";

    public static string CalculateHashWithSalt(string password)
    {
        var salt = GetRandomSalt();
        var hash = CalculateHash(salt + password);
        return $"{salt}{SALT_SEPARATOR}{hash}";
    }

    public static bool ValidateHashWithSalt(string saltedHash, string password)
    {
        if (!saltedHash.Contains(SALT_SEPARATOR))
            throw new Exception("ERROR: Salted hash is missing the salt separator.");

        var parts = saltedHash.Split(SALT_SEPARATOR);
        var hash = CalculateHash(parts[0] + password);
        return parts[1].Equals(hash);
    }

    private static string CalculateHash(string password)
    {
        var data = Encoding.UTF8.GetBytes(password);
        byte[] hashedData = SHA512.HashData(data);
        return Convert.ToBase64String(hashedData);
    }

    private static string GetRandomSalt() => Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5);
}
