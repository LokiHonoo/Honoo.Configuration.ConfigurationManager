using System;
using System.Globalization;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置属性。
    /// </summary>
    public sealed class SingleTagProperty
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
        /// 创建 HonooProperty 的新实例。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        public SingleTagProperty(string value)
        {
            _content = new XAttribute("single_tag_property", value);
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        internal SingleTagProperty(XAttribute content)
        {
            _content = content;
            _value = content.Value;
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
        /// <returns></returns>
        /// <exception cref="Exception" />
        public byte[] GetBytesValue()
        {
            return XValueHelper.Parse(_value);
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
        /// 方法已重写。返回节点的缩进 XML 文本。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _content.ToString();
        }

        internal void ResetKey(string key)
        {
            _content = new XAttribute(key, _value);
        }
    }
}