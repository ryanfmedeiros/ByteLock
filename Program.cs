using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Secure File Vault (Console Version)");

        while (true)
        {
            Console.Write("Enter 'e' to encrypt, 'd' to decrypt, or 'q' to quit: ");
            string action = Console.ReadLine().ToLower();

            if (action == "q") break;

            if (action != "e" && action != "d")
            {
                Console.WriteLine("Invalid option.");
                continue;
            }

            Console.Write("Enter full file path: ");
            string inputPath = Console.ReadLine();

            if (!File.Exists(inputPath))
            {
                Console.WriteLine("File does not exist.");
                continue;
            }

            Console.Write("Enter password: ");
            string password = ReadPassword();

            try
            {
                byte[] fileBytes = File.ReadAllBytes(inputPath);

                if (action == "e")
                {
                    byte[] encrypted = CryptoHelper.Encrypt(fileBytes, password);

                    // Save encrypted file
                    string vaultDir = "vault";
                    Directory.CreateDirectory(vaultDir);

                    string outputFile = Path.Combine(vaultDir, Path.GetFileName(inputPath) + ".vault");
                    File.WriteAllBytes(outputFile, encrypted);

                    // Compute checksum of original file
                    string checksum = CryptoHelper.ComputeSHA256(fileBytes);

                    Console.WriteLine($"File encrypted successfully and saved to {outputFile}");
                    Console.WriteLine($"SHA256 checksum of original file: {checksum}");
                }
                else if (action == "d")
                {
                    byte[] decrypted = CryptoHelper.Decrypt(fileBytes, password);

                    string outputDir = "output";
                    Directory.CreateDirectory(outputDir);

                    string originalFileName = Path.GetFileNameWithoutExtension(inputPath); // remove .vault
                    string outputFile = Path.Combine(outputDir, originalFileName);

                    File.WriteAllBytes(outputFile, decrypted);

                    // Compute checksum of decrypted file
                    string checksum = CryptoHelper.ComputeSHA256(decrypted);

                    Console.WriteLine($"File decrypted successfully and saved to {outputFile}");
                    Console.WriteLine($"SHA256 checksum of decrypted file: {checksum}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during processing: " + ex.Message);
            }

            Console.WriteLine();
        }

        Console.WriteLine("Exiting Secure File Vault.");
    }

    // Read password without echoing input
    private static string ReadPassword()
    {
        string password = "";
        ConsoleKeyInfo info;

        do
        {
            info = Console.ReadKey(true);
            if (info.Key != ConsoleKey.Enter)
            {
                password += info.KeyChar;
                Console.Write("*");
            }
        } while (info.Key != ConsoleKey.Enter);

        Console.WriteLine();
        return password;
    }
}
