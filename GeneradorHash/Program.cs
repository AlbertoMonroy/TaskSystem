using System;
using System.Security.Cryptography;
using System.Text;

public static class PasswordHasher
{

    public static void Main(string[] args)
    {
        // Ejemplo de uso
        string password = "abc123";
        string hashedPassword = HashPassword(password);
        Console.WriteLine($"Hashed Password: {hashedPassword}");
        Console.ReadLine();
        //bool isMatch = VerifyPassword("mi_contraseña_segura", hashedPassword);
        //Console.WriteLine($"Password Match: {isMatch}");
    }

    public static string HashPassword(string password)
    {
        // Crear un salt aleatorio
        byte[] salt = new byte[16];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(salt);
        }

        // Generar el hash con PBKDF2
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000); // 10k iteraciones
        byte[] hash = pbkdf2.GetBytes(20); // Deriva una clave de 20 bytes

        // Combinar salt y hash
        byte[] hashBytes = new byte[36]; // 16 salt + 20 hash
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 20);

        // Convertir a string base64
        return Convert.ToBase64String(hashBytes);
    }

    public static bool VerifyPassword(string password, string storedHash)
    {
        byte[] hashBytes = Convert.FromBase64String(storedHash);

        // Extraer salt
        byte[] salt = new byte[16];
        Array.Copy(hashBytes, 0, salt, 0, 16);

        // Hash de entrada
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
        byte[] hash = pbkdf2.GetBytes(20);

        // Comparar byte a byte
        for (int i = 0; i < 20; i++)
        {
            if (hashBytes[i + 16] != hash[i])
                return false;
        }

        return true;
    }
}
