using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        List<string> transactions = GenerateTransactions(38);

        MerkleTree tree = new MerkleTree(transactions);

        // Print root hash
        Console.WriteLine("Merkle root: " + tree.GetRootHash());

        // Print all nodes
        tree.PrintAllNodes();

        // Verify transaction
        string transactionToVerify = transactions[1]; 
        Console.WriteLine($"Transaction '{transactionToVerify}' is present: {tree.VerifyTransaction(transactionToVerify)}");

        // Get proof for transaction
        List<string> proof = tree.GetProof(transactionToVerify);    

        Console.WriteLine($"Minimum number of hashes required for proof: {proof.Count}");
    }

    static List<string> GenerateTransactions(int count)
    {
        List<string> transactions = new List<string>();

        Random random = new Random();
        for (int i = 0; i < count; i++)
        {
            byte[] buffer = new byte[16];
            random.NextBytes(buffer);
            transactions.Add(BitConverter.ToString(buffer).Replace("-", ""));
        }

        return transactions;
    }
}
