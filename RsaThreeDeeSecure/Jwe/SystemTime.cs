using System;
using System.Threading;

namespace RsaThreeDeeSecure.Jwe
{
    public static class SystemTime
    {
        private static readonly Func<DateTimeOffset> InternalNow = () => DateTimeOffset.Now;

        private static readonly AsyncLocal<Func<DateTimeOffset>> Override = new AsyncLocal<Func<DateTimeOffset>>();

        public static DateTimeOffset Now => (Override.Value ?? InternalNow)();

        public static void Set(Func<DateTimeOffset> func)
        {
            Override.Value = func;
        }

        public static void Reset()
        {
            Override.Value = null;
        }
    }
}
