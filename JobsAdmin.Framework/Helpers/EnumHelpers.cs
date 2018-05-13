using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsAdmin.Framework.Helpers
{
    public static class EnumHelpers
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static IEnumerable<KeyValuePair<int, string>> GetAsNameValue(Type enumType)
        {
            return (from Enum item in Enum.GetValues(enumType)
                    select new KeyValuePair<int, string>(Convert.ToInt32(item), item.GetDescription()))
                    .ToList();
        }

        public static int AsInteger(this Enum value)
        {
            return (int)value.GetType().GetField(value.ToString()).GetValue(value);
        }
    }
}
