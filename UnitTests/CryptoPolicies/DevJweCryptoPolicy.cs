using System.Security.Cryptography.X509Certificates;
using RsaThreeDeeSecure.Jwe;

namespace UnitTests
{
    public class DevJweCryptoPolicy : IJweCryptoPolicy
    {
        public X509Chain GetX509TrustChain()
            => new X509Chain
            {
                ChainPolicy =
                {
                    RevocationMode = X509RevocationMode.NoCheck,
                    RevocationFlag = X509RevocationFlag.ExcludeRoot,
                    VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority |
                                        X509VerificationFlags.IgnoreInvalidBasicConstraints
                }
            };

        public bool IgnoreMessageExpiry => true;
    }
}