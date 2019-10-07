using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Jose;
using Newtonsoft.Json;
using RsaThreeDeeSecure.Constants;
using RsaThreeDeeSecure.Extensions;

namespace RsaThreeDeeSecure.Jwe
{
    public partial class JweMessage
    {
        public class JweEncryptedPayload
        {
            public static JweEncryptedPayload CreateFromEncryptedPayload(string payloadB64, RSA encryptionCert)
            {
                var splitPayload = payloadB64.SplitInToSections();
                return new JweEncryptedPayload
                {
                    Header = JweHeader.CreateFromEncryptedHeader(splitPayload[0]),
                    AuthTag = splitPayload[4],
                    ClearTextMessage = Decode(payloadB64, encryptionCert)
                };
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
                    protectedHeader.EncryptionPublicCert = 
                        System.Text.Encoding.UTF8.GetBytes(
                            protectedHeader.Certs.FirstOrDefault() 
                                ?? throw new ApplicationException("No encryption public cert was found. "));
                    
                    return protectedHeader;
                }

                [JsonProperty(JweHeaderConstants.Alg)]
                public string Alg { get; set; }
            
                [JsonProperty(JweHeaderConstants.Enc)]
                public string Encoding { get; set; }
            
                [JsonProperty(JweHeaderConstants.X5C)]
                public List<string> Certs { get; set; }

                [JsonIgnore]
                public byte[] EncryptionPublicCert { get; set; }
            }

        }
    }


}