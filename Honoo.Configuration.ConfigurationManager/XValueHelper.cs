using System;
using System.Globalization;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    internal static class XValueHelper
    {
        internal static object GetDictionarySectionValue(XElement content)
        {
            string type = content.Attribute("type")?.Value;
            if (string.IsNullOrWhiteSpace(type))
            {
                type = "String";
            }
            string value = content.Attribute("value").Value;
            switch (type)
            {
                case "bool": case "Boolean": case "System.Boolean": return bool.Parse(value);
                case "sbyte": case "SByte": case "System.SByte": return sbyte.Parse(value, CultureInfo.InvariantCulture);
                case "byte": case "Byte": case "System.Byte": return byte.Parse(value, CultureInfo.InvariantCulture);
                case "short": case "Int16": case "System.Int16": return short.Parse(value, CultureInfo.InvariantCulture);
                case "ushort": case "UInt16": case "System.UInt16": return ushort.Parse(value, CultureInfo.InvariantCulture);
                case "int": case "Int32": case "System.Int32": return int.Parse(value, CultureInfo.InvariantCulture);
                case "uint": case "UInt32": case "System.UInt32": return uint.Parse(value, CultureInfo.InvariantCulture);
                case "long": case "Int64": case "System.Int64": return long.Parse(value, CultureInfo.InvariantCulture);
                case "ulong": case "UInt64": case "System.UInt64": return ulong.Parse(value, CultureInfo.InvariantCulture);
                case "float": case "Single": case "System.Single": return float.Parse(value, CultureInfo.InvariantCulture);
                case "double": case "Double": case "System.Double": return double.Parse(value, CultureInfo.InvariantCulture);
                case "decimal": case "Decimal": case "System.Decimal": return decimal.Parse(value, CultureInfo.InvariantCulture);
                case "char": case "Char": case "System.Char": return char.Parse(value);
                case "string": case "String": case "System.String": return value;
                case "byte[]": case "Byte[]": case "System.Byte[]": return GetHexBytes(value);
                default: throw new InvalidCastException();
            }
        }

        internal static void SetDictionarySectionValue(object value, XElement content)
        {
            switch (value)
            {
                case bool _:
                case sbyte _:
                case byte _:
                case short _:
                case ushort _:
                case int _:
                case uint _:
                case long _:
                case ulong _:
                case float _:
                case double _:
                case decimal _:
                case char _:
                    content.SetAttributeValue("value", value.ToString());
                    break;

                case string val:
                    content.SetAttributeValue("value", val);
                    break;

                case byte[] val:
                    content.SetAttributeValue("value", BitConverter.ToString(val).Replace("-", string.Empty));
                    break;

                default: throw new NotSupportedException();
            }
            content.SetAttributeValue("type", value.GetType().Name);
        }

        private static byte[] GetHexBytes(string hex)
        {
            if (string.IsNullOrWhiteSpace(hex))
            {
                throw new ArgumentException($"The invalid argument - {nameof(hex)}.");
            }
            byte[] result = new byte[hex.Length / 2];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return result;
        }
    }
}