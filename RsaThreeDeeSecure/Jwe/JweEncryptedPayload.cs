using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Jose;
using Newtonsoft.Json;
using RsaThreeDeeSecure.Constants;
using RsaThreeDeeSecure.Exceptions;
using RsaThreeDeeSecure.Extensions;

namespace RsaThreeDeeSecure.Jwe
{
    public partial class JweMessage
    {
        public class JweEncryptedPayload
        {
            public static JweEncryptedPayload CreateFromEncryptedPayload(string payloadB64, List<X509Certificate2> issuerEncryptionCerts)
            {
                var splitPayload = payloadB64.SplitInToSections();
                var header = JweHeader.CreateFromEncryptedHeader(splitPayload[0]);
                var encryptionPrivateKey = FindPrivateKeyForEncryptionCert(issuerEncryptionCerts, header.Certs.FirstOrDefault());
                
                return new JweEncryptedPayload
                {
                    Header = JweHeader.CreateFromEncryptedHeader(splitPayload[0]),
                    AuthTag = splitPayload[4],
                    ClearTextMessage = Decode(payloadB64, encryptionPrivateKey)
                };
            }

            private static RSA FindPrivateKeyForEncryptionCert(IEnumerable<X509Certificate2> issuerEncryptionCerts, string headerEncryptionPublicCertBase64)
            {
                var matchingCertificate = issuerEncryptionCerts
                    .FirstOrDefault(c => 
                        Convert.ToBase64String(c.Export(X509ContentType.Cert)) == headerEncryptionPublicCertBase64);
                
                if (matchingCertificate == null)
                {
                    throw new Rsa3dSecureException(
                        RsaErrorCodes.DecryptionFailed, 
                        "No matching private encryption key found for encryption public sent in JWE protected header X5C property");             
                }
                
                var privateEncryptionKey = matchingCertificate.GetRSAPrivateKey();
                if (privateEncryptionKey == null)
                {
                    throw new Rsa3dSecureException(
                        RsaErrorCodes.DecryptionFailed, 
                        "The located issuer encryption certificate did not have an associated private key.");             
                }

                return privateEncryptionKey;
            }

            public static JweEncryptedPayload CreateFromClearTextMessage(string clearTextMessage, X509Certificate2 issuerEncryptionCert)
            {
                var encryptedPayload = JWT.Encode(
                    clearTextMessage,
                    issuerEncryptionCert.GetRSAPublicKey(),
                    JweAlgorithm.RSA_OAEP,
                    JweEncryption.A128CBC_HS256, 
                    extraHeaders: new Dictionary<string, object>
                    {
                        { JweHeaderConstants.X5C, new List<byte[]> { issuerEncryptionCert.Export(X509ContentType.Cert)} }
                    });

                return new JweEncryptedPayload
                {
                    CipherTextAsBase64 = encryptedPayload,
                    ClearTextMessage = clearTextMessage
                };
            }

            public T GetDecryptedJsonObjectAs<T>()
                => JsonConvert.DeserializeObject<T>(ClearTextMessage);
            
            public JweHeader Header { get; set; }
            
            public string CipherTextAsBase64 { get; set; }

            public string AuthTag { get; set; }

            public string ClearTextMessage { get; set; }
            
            private static string Decode(string payload, RSA key)
                => JWT.Decode(payload, key, JweAlgorithm.RSA_OAEP, JweEncryption.A128CBC_HS256);

            public class JweHeader
            {
                public static JweHeader CreateFromEncryptedHeader(string headerB64)
                {
                    var headerString = headerB64.DecodeBase64();
                    var protectedHeader = JsonConvert.DeserializeObject<JweHeader>(headerString);

                    return protectedHeader;
                }

                [JsonProperty(JweHeaderConstants.Alg)]
                public string Alg { get; set; }
            
                [JsonProperty(JweHeaderConstants.Enc)]
                public string Encoding { get; set; }
            
                [JsonProperty(JweHeaderConstants.X5C)]
                public List<string> Certs { get; set; }
            }

        }
    }


}