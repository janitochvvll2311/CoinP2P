using System.Security.Cryptography;

namespace CoinP2P.Helpers;

public static class CryptoHelper
{

    public static byte[] Encrypt(this byte[] data, byte[] pubkey)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(pubkey, out int readed);
        var edata = rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA256);
        return edata;
    }

    public static byte[] Decrypt(this byte[] data, byte[] privkey)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(privkey, out int readed);
        var ddata = rsa.Decrypt(data, RSAEncryptionPadding.OaepSHA256);
        return ddata;
    }

    public static byte[] SignData(this byte[] data, byte[] privkey)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(privkey, out int readed);
        var signature = rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        return signature;
    }

    public static bool VerifyData(this byte[] data, byte[] signature, byte[] pubkey)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(pubkey, out int readed);
        var valid = rsa.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        return valid;
    }

    public static byte[] ComputeHash(this byte[] data)
    {
        var hash = SHA256.HashData(data);
        return hash;
    }

    public static byte[] SignHash(this byte[] hash, byte[] privkey)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(privkey, out int readed);
        var signature = rsa.SignHash(hash, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        return signature;
    }

    public static bool VerifyHash(this byte[] hash, byte[] signature, byte[] pubkey)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(pubkey, out int readed);
        var valid = rsa.VerifyHash(hash, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        return valid;
    }

    public static byte[] ComputeHash(this IEnumerable<byte[]> hashes)
    {
        var hashlist = new List<byte[]>(hashes);
        while (hashlist.Count != 1)
        {
            var hashlock = hashlist.ToArray();
            hashlist.Clear();
            var m = hashlock.Length / 2 + hashlock.Length % 2;
            for (var i = 0; i < m; i++)
            {
                var h1 = hashlock[i];
                var h2 = hashlock[hashlock.Length - 1 - i];
                if (h1 == h2)
                {
                    hashlist.Add(h1);
                }
                else
                {
                    var nhash = h1.Concat(h2).ToArray().ComputeHash();
                    hashlist.Add(nhash);
                }
            }
        }
        var thash = hashlist.First();
        return thash;
    }

}