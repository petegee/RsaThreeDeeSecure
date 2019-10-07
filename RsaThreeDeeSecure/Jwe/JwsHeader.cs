using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using RsaThreeDeeSecure.Constants;
using RsaThreeDeeSecure.Extensions;

namespace RsaThreeDeeSecure.Jwe
{
    public partial class JweMessage
    {
        public class JwsHeader
        {
            public static JwsHeader CreateJweHeaderFromEncryptedHeader(string b64Header)
            {
                var jwsHeader = JsonConvert.DeserializeObject<JwsHeader>(b64Header.DecodeBase64());
                
                jwsHeader.Certificates = jwsHeader.Base64Certs
                    .Select(b64 => new X509Certificate2(b64.DecodeBase64AsBytes()))
                    .ToList();

                jwsHeader.SigningPublicCert = jwsHeader.Certificates.FirstOrDefault();
                
                return jwsHeader;
            }
            
            [JsonProperty(JweHeaderConstants.Alg)]
            public string Alg { get; set; }
            
            [JsonProperty(JweHeaderConstants.Crit)]
            public List<string> Crit { get; set; }
            
            [JsonProperty(JweHeaderConstants.Exp)]
            public long Expiry { get; set; }

            [JsonProperty(JweHeaderConstants.X5C)]
            public List<string> Base64Certs { get; set; }
            
            [JsonIgnore]
            public List<X509Certificate2> Certificates { get; set; }
            
            [JsonIgnore]
            public X509Certificate2 SigningPublicCert { get; private set; }

            public bool HasExpired()
            {
                var utcNow = SystemTime.Now.ToUniversalTime();
                
                var messageExpiresAt = DateTimeOffset.FromUnixTimeSeconds(Expiry);

                return messageExpiresAt - utcNow < TimeSpan.Zero;
            }
            
            public bool VerifyCertChain(IJweCryptoPolicy policy)
            {
                var chain = policy.GetX509TrustChain();

                Certificates.Where(c => c.SerialNumber != SigningPublicCert.SerialNumber)
                    .ToList()
                    .ForEach(c => chain.ChainPolicy.ExtraStore.Add(c));
                
                if (!chain.Build(SigningPublicCert))
                    return false;
                
                // Make sure we have the same number of elements.
                if (chain.ChainElements.Count != chain.ChainPolicy.ExtraStore.Count + 1)
                    return false;

                // Make sure all the thumbprints of the CAs match up.
                // The first one should be 'primaryCert', leading up to the root CA.
                for (var i = 1; i < chain.ChainElements.Count; i++)
                {
                    if (chain.ChainElements[i].Certificate.Thumbprint != chain.ChainPolicy.ExtraStore[i - 1].Thumbprint)
                        return false;
                }
                
                return true;
            }
        } 
    }


}