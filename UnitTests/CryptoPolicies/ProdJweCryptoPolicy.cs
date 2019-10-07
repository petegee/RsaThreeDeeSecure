using System.Security.Cryptography.X509Certificates;
using RsaThreeDeeSecure.Jwe;

namespace UnitTests
{
    public class ProdJweCryptoPolicy : IJweCryptoPolicy
    {
        public X509Chain GetX509TrustChain()
            => new X509Chain
            {
                ChainPolicy =
                {
                    RevocationMode = X509RevocationMode.Offline,
                    RevocationFlag = X509RevocationFlag.EntireChain,
                    VerificationFlags = X509VerificationFlags.NoFlag
                }
            };

        public bool IgnoreMessageExpiry => false;
    }
}