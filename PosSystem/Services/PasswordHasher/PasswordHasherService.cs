using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace PosSystem.Services.PasswordHasher;

public static class PasswordHasherService
{
    public static string HashPassword(string password, string salt,int iterations = 1000000, int numBytesRequested = 32)
    {
        ValidateHashParam(password, salt);
        byte[] saltBytes = Convert.FromBase64String(salt);
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password!,
            salt: saltBytes,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: iterations,
            numBytesRequested: numBytesRequested));

        return hashed;
    }

    public static bool PasswordValidation(string oldPassword,string newPassword,string salt)
    {
        string newHashedPassword=HashPassword(newPassword, salt);
        if (newHashedPassword != oldPassword)
            return false;

        return true;
    }
    private static void ValidateHashParam(string password,string salt)
    {
        if (string.IsNullOrEmpty(password)) 
            throw new ArgumentNullException("Password is invalid");
        if(string .IsNullOrEmpty(salt))
            throw new ArgumentNullException("Salt is invalid");
    }
    private static string GenerateSalt(int size = 16)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(size);
        return Convert.ToBase64String(salt);
    }
}
