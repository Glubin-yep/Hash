using System.Security.Cryptography;
using System.Text;

public class MerkleTree
{
    private List<string> _transactions;
    private List<MerkleNode> _leaves;
    private List<MerkleNode> _allNodes;

    public MerkleTree(List<string> transactions)
    {
        _transactions = transactions;
        _leaves = new List<MerkleNode>();
        _allNodes = new List<MerkleNode>();

        // Convert transactions to leaf nodes
        foreach (var transaction in transactions)
        {
            MerkleNode leaf = new MerkleNode { HashValue = CalculateHash(transaction) };
            _leaves.Add(leaf);
            _allNodes.Add(leaf);
        }

        BuildTree();
    }

    private void BuildTree()
    {
        if (_allNodes.Count == 0)
            return;

        List<MerkleNode> currentLevel = new List<MerkleNode>(_allNodes);

        while (currentLevel.Count > 1)
        {
            List<MerkleNode> nextLevel = new List<MerkleNode>();

            for (int i = 0; i < currentLevel.Count; i += 2)
            {
                MerkleNode left = currentLevel[i];
                MerkleNode right = (i + 1 < currentLevel.Count) ? currentLevel[i + 1] : null;

                string combinedHash = (right != null) ?
                                      CalculateHash(left.HashValue + right.HashValue) :
                                      CalculateHash(left.HashValue);

                MerkleNode parent = new MerkleNode
                {
                    HashValue = combinedHash,
                    LeftChild = left,
                    RightChild = right
                };

                nextLevel.Add(parent);
                _allNodes.Add(parent);
            }

            currentLevel = nextLevel;
        }
    }

    public string GetRootHash()
    {
        return (_leaves.Count > 0) ? _leaves[0].HashValue : null;
    }

    public bool VerifyTransaction(string transaction)
    {
        string hash = CalculateHash(transaction);

        foreach (var leaf in _leaves)
        {
            if (leaf.HashValue == hash)
                return true;
        }

        return false;
    }

    public void PrintAllNodes()
    {
        int i = 0;
        foreach (var node in _allNodes)
        {
            Console.WriteLine($"Node #{i}: {node.HashValue}");
            i++;
        }
    }

    public List<string> GetProof(string transaction)
    {
        List<string> proof = new List<string>();

        // Find the leaf node corresponding to the transaction
        MerkleNode leafNode = _leaves.Find(node => node.HashValue == CalculateHash(transaction));

        if (leafNode == null)
        {
            Console.WriteLine("Transaction not found in the Merkle tree.");
            return proof;
        }

        // Traverse the tree upwards from the leaf node to the root
        MerkleNode currentNode = leafNode;
        while (currentNode != null)
        {
            proof.Add(currentNode.HashValue);
            currentNode = GetParent(currentNode);
        }

        return proof;
    }

    private MerkleNode GetParent(MerkleNode node)
    {
        foreach (var n in _allNodes)
        {
            if (n.LeftChild == node || n.RightChild == node)
            {
                return n;
            }
        }
        return null;
    }

    private string CalculateHash(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
