using System.Security.Cryptography;

namespace CoinP2P.Models.Crypto;

public struct KeyPair
{
    public byte[] Private { get; set; }
    public byte[] Public { get; set; }
    public void Generate()
    {
        using var rsa = RSA.Create();
        var privkey = rsa.ExportRSAPrivateKey();
        var pubkey = rsa.ExportRSAPublicKey();
        Private = privkey;
        Public = pubkey;
    }
}