namespace RsaThreeDeeSecure.Extensions
{
    public static class StringPemCertExtensions
    {
        public static string StripPemHeadersAndWhitespaceFromKey(this string fullPemCert)
        {
            return fullPemCert.Replace("-----BEGIN PRIVATE KEY-----", "")
                .Replace("-----END PRIVATE KEY-----", "")
                .Replace("\r", "")
                .Replace("\n", "");
        }
        
        public static string StripPemHeadersAndWhitespaceFromCert(this string fullPemCert)
        {
            return fullPemCert.Replace("-----BEGIN CERTIFICATE-----", "")
                .Replace("-----END CERTIFICATE-----", "")
                .Replace("\r", "")
                .Replace("\n", "");
        }
        
        public static byte[] PemCertToByteArray(this string fullPemCert)
        {
            return fullPemCert.StripPemHeadersAndWhitespaceFromCert().DecodeBase64AsBytes();
        }
        
        public static byte[] PemKeyToByteArray(this string fullPemCert)
        {
            return fullPemCert.StripPemHeadersAndWhitespaceFromKey().DecodeBase64AsBytes();
        }
    }
}