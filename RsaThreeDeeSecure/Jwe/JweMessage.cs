using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Jose;
using Newtonsoft.Json;
using RsaThreeDeeSecure.Constants;
using RsaThreeDeeSecure.Exceptions;
using RsaThreeDeeSecure.Extensions;

namespace RsaThreeDeeSecure.Jwe
{
    public partial class JweMessage
    {
        private IJweCryptoPolicy CryptoPolicy { get; set; }

        private JwsHeader Header { get; set; }

        private JweEncryptedPayload Payload { get; set; }

        private JweSignature Signature { get; set; }
        
        private string EncryptedMessage { get; set; }
        
        public static JweMessage FromEncryptedString(string b64MessageToDecrypt, List<X509Certificate2> issuerEncryptionCerts, IJweCryptoPolicy cryptoPolicy)
        {
            var parts = b64MessageToDecrypt.SplitInToSections();

            var jwsHeader = JwsHeader.CreateJweHeaderFromEncryptedHeader(parts[0]);
            
            var verifiedPayload = JWT.Decode(
                b64MessageToDecrypt, jwsHeader.SigningPublicCert.GetRSAPublicKey());
            
            return new JweMessage
            {
                CryptoPolicy = cryptoPolicy,
                EncryptedMessage = b64MessageToDecrypt,
                Header = jwsHeader,
                Payload = JweEncryptedPayload.CreateFromEncryptedPayload(verifiedPayload, issuerEncryptionCerts),
                Signature = new JweSignature(parts[2])
            };
        }
        
        public static T FromEncryptedString<T>(string b64MessageToDecrypt, List<X509Certificate2> issuerEncryptionCerts, IJweCryptoPolicy cryptoPolicy)
        {
            var parts = b64MessageToDecrypt.SplitInToSections();

            var jwsHeader = JwsHeader.CreateJweHeaderFromEncryptedHeader(parts[0]);
            
            var verifiedPayload = JWT.Decode(
                b64MessageToDecrypt, jwsHeader.SigningPublicCert.GetRSAPublicKey());
            
            var message = new JweMessage
            {
                CryptoPolicy = cryptoPolicy,
                EncryptedMessage = b64MessageToDecrypt,
                Header = jwsHeader,
                Payload = JweEncryptedPayload.CreateFromEncryptedPayload(verifiedPayload, issuerEncryptionCerts),
                Signature = new JweSignature(parts[2])
            };
            
            if(!message.IsSignatureValidAndTrusted())
                throw new Rsa3dSecureException(RsaErrorCodes.VerifySignatureFailed, "Message Signature is not valid.");

            return message.GetDecryptedJsonObjectAs<T>();
        }
        
        public static string CreateFromClearText(
            string clearTextMessage, 
            X509Certificate2 issuerEncryptionCert,
            IList<X509Certificate2> signingCertificateChain)
        {
            var encryptedPayload =
                JweEncryptedPayload.CreateFromClearTextMessage(clearTextMessage, issuerEncryptionCert);

            return Sign(encryptedPayload.CipherTextAsBase64, signingCertificateChain);
        }
        
        public static string CreateFrom<T>(
            T objectToEncrypt, 
            X509Certificate2 issuerEncryptionCert,
            IList<X509Certificate2> signingCertificateChain)
            => CreateFromClearText(
                JsonConvert.SerializeObject(objectToEncrypt),
                issuerEncryptionCert,
                signingCertificateChain);

        public bool IsSignatureValidAndTrusted()
        {
            //Verify exp date. Should be less then 60s. The exp is unix time
            return (CryptoPolicy.IgnoreMessageExpiry || !Header.HasExpired()) && 
                   MessageSignatureIsValid() && 
                   Header.VerifyCertChain(CryptoPolicy);
        }
        
        private static string Sign(string payload, IList<X509Certificate2> signingCertificateChain)
        {
            var key = signingCertificateChain.FirstOrDefault().GetRSAPrivateKey();
            return JWT.Encode(
                payload, 
                key, 
                JwsAlgorithm.RS256, 
                new Dictionary<string, object>
                {
                    { JweHeaderConstants.Crit, new List<string>{ JweHeaderConstants.Exp } },
                    { JweHeaderConstants.Exp, SystemTime.Now.ToUniversalTime().AddSeconds(60).ToUnixTimeSeconds() },
                    { JweHeaderConstants.X5C, signingCertificateChain.Select(c => c.Export(X509ContentType.Cert)) }
                });
        }
        
        public T GetDecryptedJsonObjectAs<T>()
            => Payload.GetDecryptedJsonObjectAs<T>();
        
        public string GetClearTextMessage()
            => Payload.ClearTextMessage;

        private bool MessageSignatureIsValid()
        {
            if(!string.Equals(Header.Alg, JweHeaderConstants.AlgValueRs256))
                throw new Rsa3dSecureException(RsaErrorCodes.IssuerError, $"Unexpected Signing algorithm {Header.Alg}. Expected {JweHeaderConstants.AlgValueRs256}");
            
            var signingCertPublicKey = Header.SigningPublicCert.GetRSAPublicKey();
            
            var result = signingCertPublicKey.VerifyData(
                Encoding.UTF8.GetBytes(GetRawEncryptedMessageWithoutSignature()), 
                Signature.Signature, 
                HashAlgorithmName.SHA256, 
                RSASignaturePadding.Pkcs1);
            
            return result;
        }

        private string GetRawEncryptedMessageWithoutSignature()
        {
            var parts = EncryptedMessage.Split(JweConventionConstants.SectionSeperator);
            return $"{parts[0]}.{parts[1]}";
        }

    }
}