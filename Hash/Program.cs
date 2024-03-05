using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main()
    {
        // Розміри вхідних даних для тестування
        int[] inputSizes = { 2_000_000 }; // Розміри даних в байтах

        // Хеш-функції для тестування
        HashAlgorithm[] algorithms = {
            new SHA1CryptoServiceProvider(),
            new SHA256CryptoServiceProvider(),
            new SHA384CryptoServiceProvider(),
            new SHA512CryptoServiceProvider()
        };

        // Вимірювання часу обчислення хеш-значень
        foreach (var algorithm in algorithms)
        {
            Console.WriteLine($"Testing {algorithm.GetType().Name}:");
            foreach (var size in inputSizes)
            {
                Console.WriteLine($"Input size: {size} bytes");
                string input = GenerateInput(size);
                Stopwatch sw = Stopwatch.StartNew();
                string hash = ComputeHash(input, algorithm);
                sw.Stop();
                Console.WriteLine($"Hash: {hash}");
                Console.WriteLine($"Time elapsed: {sw.ElapsedMilliseconds} ms");
                Console.WriteLine();
            }
        }
    }

    static string GenerateInput(int size)
    {
        StringBuilder sb = new StringBuilder();
        Random random = new Random();
        for (int i = 0; i < size; i++)
        {
            sb.Append((char)random.Next(32, 127)); // Генерація випадкових символів ASCII
        }
        return sb.ToString();
    }

    static string ComputeHash(string input, HashAlgorithm algorithm)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        byte[] hashBytes = algorithm.ComputeHash(inputBytes);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }
}
