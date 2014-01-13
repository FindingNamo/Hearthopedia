using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearthopedia
{
    public static class EnumUtilities
    {
        /// <summary>
        /// Gets the string name of the enum passed in.
        /// </summary>
        public static string GetName<TEnum>(TEnum enumObject) where TEnum : struct
        {
            //TODO: Try Catch around here?
            // What if we had an invalid enumObject come in (we cast it?)
            return Enum.GetName(typeof(TEnum), enumObject);
        }

        /// <summary>
        /// Tries to parse the string as an enum.
        /// </summary>
        public static TEnum GetEnumValueFromEnumName<TEnum>(string enumName) where TEnum : struct
        {
            TEnum result;
            if (Enum.TryParse<TEnum>(enumName, out result))
                return result;

            // This shoudln't happen?
            throw new ArgumentException();
        }

        /// <summary>
        /// Get a list of all the enums names
        /// </summary>
        public static List<string> GetEnumNames<TEnum>() where TEnum : struct
        {
            return new List<string>(Enum.GetNames(typeof(TEnum)));
        }
    }
}
