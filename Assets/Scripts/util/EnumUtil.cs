using System;

namespace Avatye.Pointhome.Util
{
    internal static class EnumUtil
    {
        internal static T? GetEnumType<T>(string type, bool ignoreCase) where T : struct, Enum
        {
            if (Enum.TryParse(type, ignoreCase, out T result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
