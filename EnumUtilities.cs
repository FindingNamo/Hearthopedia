using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using Hearthopedia.Filters;
using System.Reflection;

namespace Hearthopedia
{
    /// <summary>
    /// In a C# world this class doesn't make too much sense, and we should use extension methods on the Enum class.
    /// as in, with exention methods you cuold say
    /// 
    /// CardType.Warrior.GetName();
    /// 
    /// rather than what we have now
    /// EnumUtilities.GetName<CardType>(CardType.Warrior)
    /// 
    /// Next time!
    /// </summary>
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

            foreach (TEnum enumVal in EnumUtilities.GetEnums<TEnum>())
            {
                EnumerableOption<TEnum> option = new EnumerableOption<TEnum>
                {
                    Name = EnumUtilities.GetFriendlyName<TEnum>(enumVal),
                    EnumValue = enumVal,
                    Value = true,
                };
                optionsList.Add(option);
            }

            // Sort the list?
            optionsList.Sort((x, y) => x.Name.CompareTo(y.Name));
            return optionsList;
        }

        /// <summary>
        /// Get the description attribute of an enum.
        /// </summary>
        public static string GetDescription<TEnum>(TEnum enumVal) where TEnum : struct
        {
            Type enumType = enumVal.GetType();
            string enumName = Enum.GetName(enumType, enumVal);

            if (enumName != null)
            {
                FieldInfo field = enumType.GetField(enumName);
                if (field != null)
                {
                    DescriptionAttribute descAttr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (descAttr != null)
                        return descAttr.Description;
                }
            }
            return null;
        }

        /// <summary>
        /// Get the friendly name attribute from the enum passed in.
        /// Returns the Name of the enum if one isn't specified.
        /// </summary>
        public static string GetFriendlyName<TEnum>(TEnum enumVal) where TEnum : struct
        {
            Type enumType = enumVal.GetType();
            string enumName = Enum.GetName(enumType, enumVal);

            if (enumName != null)
            {
                FieldInfo field = enumType.GetField(enumName);
                if (field != null)
                {
                    FriendlyNameAttribute descAttr = Attribute.GetCustomAttribute(field, typeof(FriendlyNameAttribute)) as FriendlyNameAttribute;
                    if (descAttr != null)
                        return descAttr.FriendlyName;
                }
            }

            return GetName<TEnum>(enumVal);
        }
    }
}
