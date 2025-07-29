using System;
using System.IO;
using System.Security.Cryptography;

public static class CryptoHelper
{
    private const int KeySize = 256;
    private const int BlockSize = 128;
    private const int SaltSize = 16;
    private const int IvSize = 16;
    private const int Iterations = 100_000;

    public static byte[] Encrypt(byte[] data, string password)
    {
        byte[] salt = GenerateRandomBytes(SaltSize);
        byte[] iv = GenerateRandomBytes(IvSize);

        byte[] key = DeriveKey(password, salt);

        using var aes = Aes.Create();
        aes.KeySize = KeySize;
        aes.BlockSize = BlockSize;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.Key = key;
        aes.IV = iv;

        using var encryptor = aes.CreateEncryptor();
        byte[] cipherText = encryptor.TransformFinalBlock(data, 0, data.Length);

        using var ms = new MemoryStream();
        ms.Write(salt, 0, salt.Length);
        ms.Write(iv, 0, iv.Length);
        ms.Write(cipherText, 0, cipherText.Length);

        return ms.ToArray();
    }

    public static byte[] Decrypt(byte[] encryptedData, string password)
    {
        byte[] salt = new byte[SaltSize];
        byte[] iv = new byte[IvSize];

        Array.Copy(encryptedData, 0, salt, 0, SaltSize);
        Array.Copy(encryptedData, SaltSize, iv, 0, IvSize);

        byte[] key = DeriveKey(password, salt);

        int cipherStartIndex = SaltSize + IvSize;
        int cipherLength = encryptedData.Length - cipherStartIndex;

        using var aes = Aes.Create();
        aes.KeySize = KeySize;
        aes.BlockSize = BlockSize;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.Key = key;
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor();
        return decryptor.TransformFinalBlock(encryptedData, cipherStartIndex, cipherLength);
    }

    public static string ComputeSHA256(byte[] data)
    {
        using var sha256 = SHA256.Create();
        byte[] hash = sha256.ComputeHash(data);
        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }

    private static byte[] GenerateRandomBytes(int size)
    {
        byte[] bytes = new byte[size];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return bytes;
    }

    private static byte[] DeriveKey(string password, byte[] salt)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        return pbkdf2.GetBytes(KeySize / 8);
    }
}
