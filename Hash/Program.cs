using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        // Create a list to store hashes of each frame
        List<string> hashValues = new List<string>();

        // Loop through each frame
        for (int i = 1; i <= 22; i++)
        {
            Bitmap frame = new Bitmap($"image{i}.jpg"); 

            string hash = CalculateFrameHash(frame);
            hashValues.Add(hash);

            frame.Dispose();
        }

        MerkleTree tree = new MerkleTree(hashValues);

        // Print root hash
        Console.WriteLine("Merkle root: " + tree.GetRootHash());

        // Print all nodes
        tree.PrintAllNodes();
    }

    static string CalculateFrameHash(Bitmap frame)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            frame.Save(ms, ImageFormat.Jpeg);
            byte[] frameBytes = ms.ToArray();

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(frameBytes);
                string hashSum = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return hashSum;
            }
        }
    }
}
