using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置属性。
    /// </summary>
    public sealed class AddProperty : ConfigProperty, IEquatable<AddProperty>, IComparer<AddProperty>, IComparable
    {
        private string _value;

        /// <summary>
        /// 获取配置属性的值。
        /// </summary>
        public string Value => _value;

        #region Construction

        /// <summary>
        /// 创建 AddProperty 的新实例。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        public AddProperty(string value) : base(ConfigPropertyType.AddProperty, GetElement(value), null)
        {
            _value = base.Content.Attribute("value").Value;
        }

        internal AddProperty(XElement content, XComment comment) : base(ConfigPropertyType.AddProperty, content, comment)
        {
            _value = base.Content.Attribute("value").Value;
        }

        #endregion Construction

        #region GetValue

        /// <summary>
        /// 获取转换为 <see cref="bool"/> 格式的数据值。
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool GetBooleanValue()
        {
            return bool.Parse(_value);
        }

        /// <summary>
        /// 获取转换为 <see cref="byte"/>[] 格式的数据值。
        /// </summary>
        /// <param name="sourceFormat">指定要转换为 <see cref="byte"/>[] 类型的字符串的源格式。</param>
        /// <param name="removes">移除指定的字符后再转换。</param>
        /// <returns></returns>
        /// <exception cref="Exception" />
        public byte[] GetBytesValue(XStringFormat sourceFormat, params string[] removes)
        {
            switch (sourceFormat)
            {
                case XStringFormat.Binary: return XValueHelper.BinaryToBytes(_value, removes);
                case XStringFormat.Hex: default: return XValueHelper.HexToBytes(_value, removes);
                case XStringFormat.Base64: return Convert.FromBase64String(_value);
            }
        }

        /// <summary>
        /// 获取转换为 <see cref="byte"/> 格式的数据值。
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public byte GetByteValue()
        {
            return byte.Parse(_value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 获取转换为 <see cref="char"/> 格式的数据值。
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public char GetCharValue()
        {
            return char.Parse(_value);
        }

        /// <summary>
        /// 获取转换为 <see cref="DateTime"/> 格式的数据值。
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public DateTime GetDateTimeValue()
        {
            return DateTime.Parse(_value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 获取转换为 <see cref="decimal"/> 格式的数据值。
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public decimal GetDecimalValue()
        {
            return decimal.Parse(_value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 获取转换为 <see cref="double"/> 格式的数据值。
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public double GetDoubleValue()
        {
            return double.Parse(_value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 获取转换为 <see cref="Enum"/> 格式的数据值。
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public TEnum GetEnumValue<TEnum>() where TEnum : Enum
        {
            return (TEnum)Enum.Parse(typeof(TEnum), _value, true);
        }

        /// <summary>
        /// 获取转换为 <see cref="short"/> 格式的数据值。
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public short GetInt16Value()
        {
            return short.Parse(_value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 获取转换为 <see cref="int"/> 格式的数据值。
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public int GetInt32Value()
        {
            return int.Parse(_value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 获取转换为 <see cref="long"/> 格式的数据值。
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public long GetInt64Value()
        {
            return long.Parse(_value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 获取转换为 <see cref="sbyte"/> 格式的数据值。
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public sbyte GetSByteValue()
        {
            return sbyte.Parse(_value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 获取转换为 <see cref="float"/> 格式的数据值。
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public float GetSingleValue()
        {
            return float.Parse(_value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 获取字符串数据值。
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1024:在适用处使用属性", Justification = "<挂起>")]
        public string GetStringValue()
        {
            return _value;
        }

        /// <summary>
        /// 获取转换为 <see cref="ushort"/> 格式的数据值。
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ushort GetUInt16Value()
        {
            return ushort.Parse(_value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 获取转换为 <see cref="uint"/> 格式的数据值。
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public uint GetUInt32Value()
        {
            return uint.Parse(_value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 获取转换为 <see cref="ulong"/> 格式的数据值。
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ulong GetUInt64Value()
        {
            return ulong.Parse(_value, CultureInfo.InvariantCulture);
        }

        #endregion GetValue

        /// <summary>
        /// 比较两个对象并返回一个值。该值指示一个对象是小于、等于还是大于另一个对象。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第二个对象。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public static int Compare(AddProperty x, AddProperty y)
        {
            if (x is null)
            {
                throw new ArgumentNullException(nameof(x));
            }
            if (y is null)
            {
                throw new ArgumentNullException(nameof(y));
            }
            return string.Compare(x._value, y._value, StringComparison.Ordinal);
        }

        /// <summary>
        /// 比较。
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(AddProperty left, AddProperty right)
        {
            return !(left == right);
        }

        /// <summary>
        /// 比较。
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <(AddProperty left, AddProperty right)
        {
            return (Compare(left, right) < 0);
        }

        /// <summary>
        /// 比较。
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <=(AddProperty left, AddProperty right)
        {
            return (Compare(left, right) <= 0);
        }

        /// <summary>
        /// 比较。
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(AddProperty left, AddProperty right)
        {
            if (left is null)
            {
                return right is null;
            }
            return left.Equals(right);
        }

        /// <summary>
        /// 比较。
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >(AddProperty left, AddProperty right)
        {
            return (Compare(left, right) > 0);
        }

        /// <summary>
        /// 比较。
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >=(AddProperty left, AddProperty right)
        {
            return (Compare(left, right) >= 0);
        }

        /// <summary>
        /// 比较两个对象并返回一个值。该值指示一个对象是小于、等于还是大于另一个对象。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第二个对象。</param>
        /// <returns></returns>
        int IComparer<AddProperty>.Compare(AddProperty x, AddProperty y)
        {
            return string.Compare(x._value, y._value, StringComparison.Ordinal);
        }

        /// <summary>
        /// 将当前实例与另一个对象比较并返回一个值。该值指示当前实例在排序位置是小于、等于还是大于另一个对象。
        /// </summary>
        /// <param name="obj">要比较的对象。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public int CompareTo(object obj)
        {
            if (obj is AddProperty other)
            {
                return CompareTo(other);
            }
            throw new ArgumentException($"{nameof(obj)} is not a AddProperty.");
        }

        /// <summary>
        /// 将当前实例与另一个对象比较并返回一个值。该值指示当前实例在排序位置是小于、等于还是大于另一个对象。
        /// </summary>
        /// <param name="other">要比较的对象。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public int CompareTo(AddProperty other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }
            return string.Compare(_value, other._value, StringComparison.Ordinal);
        }

        /// <summary>
        /// 确定此实例和指定的对象具有相同的值。
        /// </summary>
        /// <param name="other">比较的对象。</param>
        /// <returns></returns>
        public bool Equals(AddProperty other)
        {
            return other != null && GetHashCode() == other.GetHashCode();
        }

        /// <summary>
        /// 确定指定的对象是否等于当前对象。
        /// </summary>
        /// <param name="obj">比较的对象。</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is AddProperty other && Equals(other);
        }

        /// <summary>
        /// 方法已重写。获取字符串数据值的哈希代码。
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return -689372227 + EqualityComparer<string>.Default.GetHashCode(_value);
        }

        private static XElement GetElement(string value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            XElement element = new XElement("add");
            element.SetAttributeValue("key", "add_property");
            element.SetAttributeValue("value", value.Trim());
            return element;
        }
    }
}