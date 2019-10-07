using RsaThreeDeeSecure.Extensions;

namespace RsaThreeDeeSecure.Jwe
{
    public partial class JweMessage
    {
        public class JweSignature
        {
            public byte[] Signature { get; }
            
            public JweSignature(string signatureB64)
            {
                Signature = signatureB64.DecodeBase64AsBytes();
            }
        }
    }


}