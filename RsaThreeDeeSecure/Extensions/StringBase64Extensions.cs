using System;
using System.Text;
using RsaThreeDeeSecure.Constants;

namespace RsaThreeDeeSecure.Extensions
{
    public static class StringBase64Extensions
    {
        public static string DecodeBase64(this string s)
        {
            var bytes = DecodeBase64AsBytes(s);
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }

        public static string SanitiseBase64(this string s)
            => s.Replace('-', '+')
                .Replace('_', '/')
                .PadRight(4 * ((s.Length + 3) / 4), '=');
        
        public static byte[] DecodeBase64AsBytes(this string s)
            => Convert.FromBase64String(SanitiseBase64(s));

        public static string[] SplitInToSections(this string s)
            => s.Split(JweConventionConstants.SectionSeperator);
    }
}