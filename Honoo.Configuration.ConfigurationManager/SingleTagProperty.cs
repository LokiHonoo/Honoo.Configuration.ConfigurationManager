using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置属性。
    /// </summary>
    public sealed class SingleTagProperty : IEquatable<SingleTagProperty>, IComparer<SingleTagProperty>, IComparable
    {
        private XAttribute _content;
        private string _value;

        /// <summary>
        /// 获取配置属性的值。
        /// </summary>
        public object Value => _value;

        internal XAttribute Content => _content;

        #region Construction

        /// <summary>
        /// 创建 SingleTagProperty 的新实例。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        public SingleTagProperty(string value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            _value = value.Trim();
        }

        internal SingleTagProperty(XAttribute attribute)
        {
            _content = attribute;
            _value = attribute.Value;
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
        /// <param name="removes">要移除的字符集合。</param>
        /// <returns></returns>
        /// <exception cref="Exception" />
        public byte[] GetBytesValue(params string[] removes)
        {
            return XValueHelper.Parse(_value, removes);
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

        #region SetValue

        /// <summary>
        /// 设置值。
        /// </summary>
        /// <param name="value">文本类型的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public SingleTagProperty SetValue(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            value = value.Trim();
            _content.Value = value;
            _value = value;
            return this;
        }

        #endregion SetValue

        /// <summary>
        /// 比较两个对象并返回一个值。该值指示一个对象是小于、等于还是大于另一个对象。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第二个对象。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public static int Compare(SingleTagProperty x, SingleTagProperty y)
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
        public static bool operator !=(SingleTagProperty left, SingleTagProperty right)
        {
            return !(left == right);
        }

        /// <summary>
        /// 比较。
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <(SingleTagProperty left, SingleTagProperty right)
        {
            return (Compare(left, right) < 0);
        }

        /// <summary>
        /// 比较。
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <=(SingleTagProperty left, SingleTagProperty right)
        {
            return (Compare(left, right) <= 0);
        }

        /// <summary>
        /// 比较。
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(SingleTagProperty left, SingleTagProperty right)
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
        public static bool operator >(SingleTagProperty left, SingleTagProperty right)
        {
            return (Compare(left, right) > 0);
        }

        /// <summary>
        /// 比较。
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >=(SingleTagProperty left, SingleTagProperty right)
        {
            return (Compare(left, right) >= 0);
        }

        /// <summary>
        /// 比较两个对象并返回一个值。该值指示一个对象是小于、等于还是大于另一个对象。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第二个对象。</param>
        /// <returns></returns>
        int IComparer<SingleTagProperty>.Compare(SingleTagProperty x, SingleTagProperty y)
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
            if (obj is SingleTagProperty other)
            {
                return CompareTo(other);
            }
            throw new ArgumentException($"{nameof(obj)} is not a SingleTagProperty.");
        }

        /// <summary>
        /// 将当前实例与另一个对象比较并返回一个值。该值指示当前实例在排序位置是小于、等于还是大于另一个对象。
        /// </summary>
        /// <param name="other">要比较的对象。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public int CompareTo(SingleTagProperty other)
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
        public bool Equals(SingleTagProperty other)
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
            return obj is SingleTagProperty other && Equals(other);
        }

        /// <summary>
        /// 方法已重写。获取字符串数据值的哈希代码。
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return -1073741820 + EqualityComparer<string>.Default.GetHashCode(_value);
        }

        /// <summary>
        /// 方法已重写。返回节点的缩进 XML 文本。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _content.ToString();
        }

        internal void CreateContent(string key)
        {
            _content = new XAttribute(key, _value);
        }

        internal void RemoveContent()
        {
            _content.Remove();
            _content = null;
        }
    }
}