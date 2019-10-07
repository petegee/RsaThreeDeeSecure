using System.Security.Cryptography.X509Certificates;

namespace RsaThreeDeeSecure.Jwe
{
    public interface IJweCryptoPolicy
    {
        X509Chain GetX509TrustChain();

        bool IgnoreMessageExpiry { get; }
    }
}