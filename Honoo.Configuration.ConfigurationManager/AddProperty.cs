using System;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置属性。
    /// </summary>
    public sealed class AddProperty : TagProperty
    {
        private readonly string _key;
        private readonly string _value;

        /// <summary>
        /// 获取配置属性的键。
        /// </summary>
        public string Key => _key;

        /// <summary>
        /// 获取配置属性的值。
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1721:属性名不应与 get 方法匹配", Justification = "<挂起>")]
        public string Value => _value;

        #region Construction

        /// <summary>
        /// 创建 AddProperty 的新实例。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        public AddProperty(string key, string value) : base(TagPropertyKind.AddProperty, GetElement(key, value), null)
        {
            _key = key ?? throw new ArgumentNullException(nameof(key));
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        internal AddProperty(XElement content, XComment comment) : base(TagPropertyKind.AddProperty, content, comment)
        {
            _key = content.Attribute("key").Value;
            _value = content.Attribute("value").Value;
        }

        #endregion Construction

        #region TryGetValue

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue(out string value)
        {
            value = _value;
            return false;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue(out bool value)
        {
            return bool.TryParse(_value, out value);
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue(out sbyte value)
        {
            return sbyte.TryParse(_value, out value);
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue(out byte value)
        {
            return byte.TryParse(_value, out value);
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue(out short value)
        {
            return short.TryParse(_value, out value);
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue(out ushort value)
        {
            return ushort.TryParse(_value, out value);
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue(out int value)
        {
            return int.TryParse(_value, out value);
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue(out uint value)
        {
            return uint.TryParse(_value, out value);
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue(out long value)
        {
            return long.TryParse(_value, out value);
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue(out ulong value)
        {
            return ulong.TryParse(_value, out value);
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue(out float value)
        {
            return float.TryParse(_value, out value);
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue(out double value)
        {
            return double.TryParse(_value, out value);
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue(out decimal value)
        {
            return decimal.TryParse(_value, out value);
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue(out char value)
        {
            return char.TryParse(_value, out value);
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue(out Binaries value)
        {
            if (XValueHelper.TryParse(_value, out byte[] val))
            {
                value = new Binaries(val);
                return true;
            }
            value = null;
            return false;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue<TEnum>(out TEnum value) where TEnum : struct
        {
            if (typeof(TEnum).BaseType.FullName == "System.Enum")
            {
                return Enum.TryParse(_value, false, out value);
            }
            value = default;
            return false;
        }

        #endregion TryGetValue

        #region GetValue

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="defaultValue">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public string GetValue(string defaultValue)
        {
            return TryGetValue(out string value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="defaultValue">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool GetValue(bool defaultValue)
        {
            return TryGetValue(out bool value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="defaultValue">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public sbyte GetValue(sbyte defaultValue)
        {
            return TryGetValue(out sbyte value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="defaultValue">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public byte GetValue(byte defaultValue)
        {
            return TryGetValue(out byte value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="defaultValue">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public short GetValue(short defaultValue)
        {
            return TryGetValue(out short value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="defaultValue">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ushort GetValue(ushort defaultValue)
        {
            return TryGetValue(out ushort value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="defaultValue">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public int GetValue(int defaultValue)
        {
            return TryGetValue(out int value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="defaultValue">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public uint GetValue(uint defaultValue)
        {
            return TryGetValue(out uint value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="defaultValue">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public long GetValue(long defaultValue)
        {
            return TryGetValue(out long value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="defaultValue">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ulong GetValue(ulong defaultValue)
        {
            return TryGetValue(out ulong value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="defaultValue">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public float GetValue(float defaultValue)
        {
            return TryGetValue(out float value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="defaultValue">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public double GetValue(double defaultValue)
        {
            return TryGetValue(out double value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="defaultValue">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public decimal GetValue(decimal defaultValue)
        {
            return TryGetValue(out decimal value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="defaultValue">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public char GetValue(char defaultValue)
        {
            return TryGetValue(out char value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="defaultValue">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public Binaries GetValue(Binaries defaultValue)
        {
            return TryGetValue(out Binaries value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取指定类型的配置属性的值。如果无法转换为指定的类型，则返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="defaultValue">无法转换为指定的类型时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public TEnum GetValue<TEnum>(TEnum defaultValue) where TEnum : struct
        {
            return TryGetValue(out TEnum value) ? value : defaultValue;
        }

        #endregion GetValue

        private static XElement GetElement(string key, string value)
        {
            XElement element = new XElement("add");
            element.SetAttributeValue("key", key);
            element.SetAttributeValue("value", value);
            return element;
        }
    }
}