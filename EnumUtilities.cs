using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hearthopedia.Filters;

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

        /// <summary>
        /// Gets an enumeration of all the enums
        /// </summary>
        public static List<TEnum> GetEnums<TEnum>() where TEnum : struct
        {
            return new List<TEnum>((TEnum[])Enum.GetValues(typeof(TEnum)));
        }

        /// <summary>
        /// Creates a list of the "Options" for this enumeration.
        /// </summary>
        public static List<EnumerableOption<TEnum>> AssembleEnumerableOptionList<TEnum>() where TEnum : struct
        {
            List<EnumerableOption<TEnum>> optionsList = new List<EnumerableOption<TEnum>>();

            foreach (TEnum cardClass in EnumUtilities.GetEnums<TEnum>())
            {
                EnumerableOption<TEnum> option = new EnumerableOption<TEnum>
                {
                    Name = EnumUtilities.GetName<TEnum>(cardClass),
                    EnumValue = cardClass,

                    //TODO: Make this true?
                    Value = false,
                };
                optionsList.Add(option);
            }

            // Sort the list?
            optionsList.Sort((x, y) => x.Name.CompareTo(y.Name));
            return optionsList;
        }
    }
}
