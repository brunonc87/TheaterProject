using System;
using System.ComponentModel;
using System.Reflection;

namespace Theater.Domain.Sections
{
    public enum AnimationType
    {
        [Description("2D")]
        D2 = 0,
        [Description("3D")]
        D3 = 1
    }

    public static class ExtensionMethods
    {

        public static string GetName(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }
    }
}
