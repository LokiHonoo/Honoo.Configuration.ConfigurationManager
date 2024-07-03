using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置属性集合。
    /// </summary>
    public sealed class HonooPropertySet : IEnumerable<HonooProperty>
    {
        #region Properties

        private static readonly XNamespace _namespace = "https://github.com/LokiHonoo/Honoo.Configuration.ConfigurationManager/";
        private readonly XElement _container;
        private readonly Dictionary<string, HonooProperty> _properties = new Dictionary<string, HonooProperty>();

        /// <summary>
        /// 获取配置属性集合中包含的元素数。
        /// </summary>
        public int Count => _properties.Count;

        #endregion Properties

        #region Construction

        internal HonooPropertySet(XElement container)
        {
            _container = container;
            if (_container.HasElements)
            {
                IEnumerator<XNode> enumerator = _container.Nodes().GetEnumerator();
                XComment comment = null;
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.NodeType == XmlNodeType.Comment)
                    {
                        comment = (XComment)enumerator.Current;
                    }
                    else
                    {
                        if (enumerator.Current.NodeType == XmlNodeType.Element)
                        {
                            XElement content = (XElement)enumerator.Current;
                            if (content.Name == _namespace + "property")
                            {
                                HonooProperty property = new HonooProperty(content, comment);
                                _properties.Remove(property.Key);
                                _properties.Add(property.Key, property);
                            }
                        }
                        comment = null;
                    }
                }
            }
        }

        #endregion Construction

        #region Add

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="property">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty Add(HonooProperty property)
        {
            _properties.Add(property.Key, property);
            if (property.Comment != null)
            {
                _container.Add(property.Comment);
            }
            _container.Add(property.Content);
            return property;
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty Add(string key, string value)
        {
            return Add(new HonooProperty(key, value.ToString()));
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty Add(string key, bool value)
        {
            return Add(key, value.ToString());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty Add(string key, sbyte value)
        {
            return Add(key, value.ToString());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty Add(string key, byte value)
        {
            return Add(key, value.ToString());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty Add(string key, short value)
        {
            return Add(key, value.ToString());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty Add(string key, ushort value)
        {
            return Add(key, value.ToString());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty Add(string key, int value)
        {
            return Add(key, value.ToString());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty Add(string key, uint value)
        {
            return Add(key, value.ToString());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty Add(string key, long value)
        {
            return Add(key, value.ToString());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty Add(string key, ulong value)
        {
            return Add(key, value.ToString());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty Add(string key, float value)
        {
            return Add(key, value.ToString());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty Add(string key, double value)
        {
            return Add(key, value.ToString());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty Add(string key, decimal value)
        {
            return Add(key, value.ToString());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty Add(string key, char value)
        {
            return Add(key, value.ToString());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty Add(string key, byte[] value)
        {
            return Add(key, BitConverter.ToString(value).Replace("-", string.Empty));
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty Add<TEnum>(string key, TEnum value) where TEnum : Enum
        {
            return Add(key, value.ToString());
        }

        #endregion Add

        #region AddArray

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddArray(string key, string[] values)
        {
            return Add(new HonooProperty(key, values));
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddArray(string key, bool[] values)
        {
            List<string> result = new List<string>();
            foreach (bool value in values)
            {
                result.Add(value.ToString());
            }
            return AddArray(key, result.ToArray());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddArray(string key, sbyte[] values)
        {
            List<string> result = new List<string>();
            foreach (sbyte value in values)
            {
                result.Add(value.ToString());
            }
            return AddArray(key, result.ToArray());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddArray(string key, byte[] values)
        {
            List<string> result = new List<string>();
            foreach (byte value in values)
            {
                result.Add(value.ToString());
            }
            return AddArray(key, result.ToArray());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddArray(string key, short[] values)
        {
            List<string> result = new List<string>();
            foreach (short value in values)
            {
                result.Add(value.ToString());
            }
            return AddArray(key, result.ToArray());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddArray(string key, ushort[] values)
        {
            List<string> result = new List<string>();
            foreach (ushort value in values)
            {
                result.Add(value.ToString());
            }
            return AddArray(key, result.ToArray());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddArray(string key, int[] values)
        {
            List<string> result = new List<string>();
            foreach (int value in values)
            {
                result.Add(value.ToString());
            }
            return AddArray(key, result.ToArray());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddArray(string key, uint[] values)
        {
            List<string> result = new List<string>();
            foreach (uint value in values)
            {
                result.Add(value.ToString());
            }
            return AddArray(key, result.ToArray());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddArray(string key, long[] values)
        {
            List<string> result = new List<string>();
            foreach (long value in values)
            {
                result.Add(value.ToString());
            }
            return AddArray(key, result.ToArray());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddArray(string key, ulong[] values)
        {
            List<string> result = new List<string>();
            foreach (ulong value in values)
            {
                result.Add(value.ToString());
            }
            return AddArray(key, result.ToArray());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddArray(string key, float[] values)
        {
            List<string> result = new List<string>();
            foreach (float value in values)
            {
                result.Add(value.ToString());
            }
            return AddArray(key, result.ToArray());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddArray(string key, double[] values)
        {
            List<string> result = new List<string>();
            foreach (double value in values)
            {
                result.Add(value.ToString());
            }
            return AddArray(key, result.ToArray());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddArray(string key, decimal[] values)
        {
            List<string> result = new List<string>();
            foreach (decimal value in values)
            {
                result.Add(value.ToString());
            }
            return AddArray(key, result.ToArray());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddArray(string key, char[] values)
        {
            List<string> result = new List<string>();
            foreach (char value in values)
            {
                result.Add(value.ToString());
            }
            return AddArray(key, result.ToArray());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddArray(string key, byte[][] values)
        {
            List<string> result = new List<string>();
            foreach (byte[] value in values)
            {
                result.Add(BitConverter.ToString(value).Replace("-", string.Empty));
            }
            return AddArray(key, result.ToArray());
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddArray<TEnum>(string key, TEnum[] values) where TEnum : Enum
        {
            List<string> result = new List<string>();
            foreach (TEnum value in values)
            {
                result.Add(value.ToString());
            }
            return AddArray(key, result.ToArray());
        }

        #endregion AddArray

        #region AddOrUpdate

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="property">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdate(HonooProperty property)
        {
            Remove(property.Key);
            return Add(property);
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdate(string key, string value)
        {
            return AddOrUpdate(new HonooProperty(key, value));
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdate(string key, bool value)
        {
            return AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdate(string key, sbyte value)
        {
            return AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdate(string key, byte value)
        {
            return AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdate(string key, short value)
        {
            return AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdate(string key, ushort value)
        {
            return AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdate(string key, int value)
        {
            return AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdate(string key, uint value)
        {
            return AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdate(string key, long value)
        {
            return AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdate(string key, ulong value)
        {
            return AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdate(string key, float value)
        {
            return AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdate(string key, double value)
        {
            return AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdate(string key, decimal value)
        {
            return AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdate(string key, char value)
        {
            return AddOrUpdate(key, value.ToString());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdate(string key, byte[] value)
        {
            return AddOrUpdate(key, BitConverter.ToString(value).Replace("-", string.Empty));
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdate<TEnum>(string key, TEnum value) where TEnum : Enum
        {
            return AddOrUpdate(key, value.ToString());
        }

        #endregion AddOrUpdate

        #region AddOrUpdateArray

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdateArray(string key, string[] values)
        {
            return AddOrUpdate(new HonooProperty(key, values));
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdateArray(string key, bool[] values)
        {
            List<string> result = new List<string>();
            foreach (bool value in values)
            {
                result.Add(value.ToString());
            }
            return AddOrUpdateArray(key, result.ToArray());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdateArray(string key, sbyte[] values)
        {
            List<string> result = new List<string>();
            foreach (sbyte value in values)
            {
                result.Add(value.ToString());
            }
            return AddOrUpdateArray(key, result.ToArray());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdateArray(string key, byte[] values)
        {
            List<string> result = new List<string>();
            foreach (byte value in values)
            {
                result.Add(value.ToString());
            }
            return AddOrUpdateArray(key, result.ToArray());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdateArray(string key, short[] values)
        {
            List<string> result = new List<string>();
            foreach (short value in values)
            {
                result.Add(value.ToString());
            }
            return AddOrUpdateArray(key, result.ToArray());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdateArray(string key, ushort[] values)
        {
            List<string> result = new List<string>();
            foreach (ushort value in values)
            {
                result.Add(value.ToString());
            }
            return AddOrUpdateArray(key, result.ToArray());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdateArray(string key, int[] values)
        {
            List<string> result = new List<string>();
            foreach (int value in values)
            {
                result.Add(value.ToString());
            }
            return AddOrUpdateArray(key, result.ToArray());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdateArray(string key, uint[] values)
        {
            List<string> result = new List<string>();
            foreach (uint value in values)
            {
                result.Add(value.ToString());
            }
            return AddOrUpdateArray(key, result.ToArray());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdateArray(string key, long[] values)
        {
            List<string> result = new List<string>();
            foreach (long value in values)
            {
                result.Add(value.ToString());
            }
            return AddOrUpdateArray(key, result.ToArray());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdateArray(string key, ulong[] values)
        {
            List<string> result = new List<string>();
            foreach (ulong value in values)
            {
                result.Add(value.ToString());
            }
            return AddOrUpdateArray(key, result.ToArray());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdateArray(string key, float[] values)
        {
            List<string> result = new List<string>();
            foreach (float value in values)
            {
                result.Add(value.ToString());
            }
            return AddOrUpdateArray(key, result.ToArray());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdateArray(string key, double[] values)
        {
            List<string> result = new List<string>();
            foreach (double value in values)
            {
                result.Add(value.ToString());
            }
            return AddOrUpdateArray(key, result.ToArray());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdateArray(string key, decimal[] values)
        {
            List<string> result = new List<string>();
            foreach (decimal value in values)
            {
                result.Add(value.ToString());
            }
            return AddOrUpdateArray(key, result.ToArray());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdateArray(string key, char[] values)
        {
            List<string> result = new List<string>();
            foreach (char value in values)
            {
                result.Add(value.ToString());
            }
            return AddOrUpdateArray(key, result.ToArray());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdateArray(string key, byte[][] values)
        {
            List<string> result = new List<string>();
            foreach (byte[] value in values)
            {
                result.Add(BitConverter.ToString(value).Replace("-", string.Empty));
            }
            return AddOrUpdateArray(key, result.ToArray());
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty AddOrUpdateArray<TEnum>(string key, TEnum[] values) where TEnum : Enum
        {
            List<string> result = new List<string>();
            foreach (TEnum value in values)
            {
                result.Add(value.ToString());
            }
            return AddOrUpdateArray(key, result.ToArray());
        }

        #endregion AddOrUpdateArray

        #region TryGetValue

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="property">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out HonooProperty property)
        {
            return _properties.TryGetValue(key, out property);
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out string value)
        {
            if (TryGetValue(key, out HonooProperty val))
            {
                return val.TryGetValue(out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out bool value)
        {
            if (TryGetValue(key, out HonooProperty val))
            {
                return val.TryGetValue(out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out sbyte value)
        {
            if (TryGetValue(key, out HonooProperty val))
            {
                return val.TryGetValue(out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out byte value)
        {
            if (TryGetValue(key, out HonooProperty val))
            {
                return val.TryGetValue(out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out short value)
        {
            if (TryGetValue(key, out HonooProperty val))
            {
                return val.TryGetValue(out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out ushort value)
        {
            if (TryGetValue(key, out HonooProperty val))
            {
                return val.TryGetValue(out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out int value)
        {
            if (TryGetValue(key, out HonooProperty val))
            {
                return val.TryGetValue(out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out uint value)
        {
            if (TryGetValue(key, out HonooProperty val))
            {
                return val.TryGetValue(out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out long value)
        {
            if (TryGetValue(key, out HonooProperty val))
            {
                return val.TryGetValue(out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out ulong value)
        {
            if (TryGetValue(key, out HonooProperty val))
            {
                return val.TryGetValue(out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out float value)
        {
            if (TryGetValue(key, out HonooProperty val))
            {
                return val.TryGetValue(out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out double value)
        {
            if (TryGetValue(key, out HonooProperty val))
            {
                return val.TryGetValue(out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out decimal value)
        {
            if (TryGetValue(key, out HonooProperty val))
            {
                return val.TryGetValue(out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out char value)
        {
            if (TryGetValue(key, out HonooProperty val))
            {
                return val.TryGetValue(out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out byte[] value)
        {
            if (TryGetValue(key, out HonooProperty val))
            {
                return val.TryGetValue(out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue<TEnum>(string key, out TEnum value) where TEnum : struct
        {
            if (TryGetValue(key, out HonooProperty val))
            {
                return val.TryGetValue(out value);
            }
            value = default;
            return false;
        }

        #endregion TryGetValue

        #region TryGetArrayValue

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetArrayValue(string key, out string[] values)
        {
            if (TryGetValue(key, out HonooProperty val))
            {
                return val.TryGetArrayValue(out values);
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetArrayValue(string key, out bool[] values)
        {
            if (TryGetValue(key, out HonooProperty val))
            {
                return val.TryGetArrayValue(out values);
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetArrayValue(string key, out sbyte[] values)
        {
            if (TryGetValue(key, out HonooProperty val))
            {
                return val.TryGetArrayValue(out values);
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetArrayValue(string key, out byte[] values)
        {
            if (TryGetValue(key, out HonooProperty val))
            {
                return val.TryGetArrayValue(out values);
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetArrayValue(string key, out short[] values)
        {
            if (TryGetValue(key, out HonooProperty val))
            {
                return val.TryGetArrayValue(out values);
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetArrayValue(string key, out ushort[] values)
        {
            if (TryGetValue(key, out HonooProperty val))
            {
                return val.TryGetArrayValue(out values);
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetArrayValue(string key, out int[] values)
        {
            if (TryGetValue(key, out HonooProperty val))
            {
                return val.TryGetArrayValue(out values);
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetArrayValue(string key, out uint[] values)
        {
            if (TryGetValue(key, out HonooProperty val))
            {
                return val.TryGetArrayValue(out values);
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetArrayValue(string key, out long[] values)
        {
            if (TryGetValue(key, out HonooProperty val))
            {
                return val.TryGetArrayValue(out values);
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetArrayValue(string key, out ulong[] values)
        {
            if (TryGetValue(key, out HonooProperty val))
            {
                return val.TryGetArrayValue(out values);
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetArrayValue(string key, out float[] values)
        {
            if (TryGetValue(key, out HonooProperty val))
            {
                return val.TryGetArrayValue(out values);
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetArrayValue(string key, out double[] values)
        {
            if (TryGetValue(key, out HonooProperty val))
            {
                return val.TryGetArrayValue(out values);
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetArrayValue(string key, out decimal[] values)
        {
            if (TryGetValue(key, out HonooProperty val))
            {
                return val.TryGetArrayValue(out values);
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetArrayValue(string key, out char[] values)
        {
            if (TryGetValue(key, out HonooProperty val))
            {
                return val.TryGetArrayValue(out values);
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetArrayValue(string key, out byte[][] values)
        {
            if (TryGetValue(key, out HonooProperty val))
            {
                return val.TryGetArrayValue(out values);
            }
            values = default;
            return false;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="values">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetArrayValue<TEnum>(string key, out TEnum[] values) where TEnum : struct
        {
            if (TryGetValue(key, out HonooProperty val))
            {
                return val.TryGetArrayValue(out values);
            }
            values = default;
            return false;
        }

        #endregion TryGetArrayValue

        #region GetValue

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty GetValue(string key)
        {
            return TryGetValue(key, out HonooProperty value) ? value : null;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public string GetValue(string key, string defaultValue)
        {
            return TryGetValue(key, out string value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool GetValue(string key, bool defaultValue)
        {
            return TryGetValue(key, out bool value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public sbyte GetValue(string key, sbyte defaultValue)
        {
            return TryGetValue(key, out sbyte value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public byte GetValue(string key, byte defaultValue)
        {
            return TryGetValue(key, out byte value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public short GetValue(string key, short defaultValue)
        {
            return TryGetValue(key, out short value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ushort GetValue(string key, ushort defaultValue)
        {
            return TryGetValue(key, out ushort value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public int GetValue(string key, int defaultValue)
        {
            return TryGetValue(key, out int value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public uint GetValue(string key, uint defaultValue)
        {
            return TryGetValue(key, out uint value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public long GetValue(string key, long defaultValue)
        {
            return TryGetValue(key, out long value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ulong GetValue(string key, ulong defaultValue)
        {
            return TryGetValue(key, out ulong value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public float GetValue(string key, float defaultValue)
        {
            return TryGetValue(key, out float value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public double GetValue(string key, double defaultValue)
        {
            return TryGetValue(key, out double value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public decimal GetValue(string key, decimal defaultValue)
        {
            return TryGetValue(key, out decimal value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public char GetValue(string key, char defaultValue)
        {
            return TryGetValue(key, out char value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public byte[] GetValue(string key, byte[] defaultValue)
        {
            return TryGetValue(key, out byte[] value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public TEnum GetValue<TEnum>(string key, TEnum defaultValue) where TEnum : struct
        {
            return TryGetValue(key, out TEnum value) ? value : defaultValue;
        }

        #endregion GetValue

        #region GetArrayValue

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValues">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public string[] GetArrayValue(string key, string[] defaultValues)
        {
            return TryGetArrayValue(key, out string[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValues">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool[] GetArrayValue(string key, bool[] defaultValues)
        {
            return TryGetArrayValue(key, out bool[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValues">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public sbyte[] GetArrayValue(string key, sbyte[] defaultValues)
        {
            return TryGetArrayValue(key, out sbyte[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValues">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public byte[] GetArrayValue(string key, byte[] defaultValues)
        {
            return TryGetArrayValue(key, out byte[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValues">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public short[] GetArrayValue(string key, short[] defaultValues)
        {
            return TryGetArrayValue(key, out short[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValues">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ushort[] GetArrayValue(string key, ushort[] defaultValues)
        {
            return TryGetArrayValue(key, out ushort[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValues">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public int[] GetArrayValue(string key, int[] defaultValues)
        {
            return TryGetArrayValue(key, out int[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValues">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public uint[] GetArrayValue(string key, uint[] defaultValues)
        {
            return TryGetArrayValue(key, out uint[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValues">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public long[] GetArrayValue(string key, long[] defaultValues)
        {
            return TryGetArrayValue(key, out long[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValues">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ulong[] GetArrayValue(string key, ulong[] defaultValues)
        {
            return TryGetArrayValue(key, out ulong[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValues">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public float[] GetArrayValue(string key, float[] defaultValues)
        {
            return TryGetArrayValue(key, out float[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValues">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public double[] GetArrayValue(string key, double[] defaultValues)
        {
            return TryGetArrayValue(key, out double[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValues">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public decimal[] GetArrayValue(string key, decimal[] defaultValues)
        {
            return TryGetArrayValue(key, out decimal[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValues">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public char[] GetArrayValue(string key, char[] defaultValues)
        {
            return TryGetArrayValue(key, out char[] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValues">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public byte[][] GetArrayValue(string key, byte[][] defaultValues)
        {
            return TryGetArrayValue(key, out byte[][] values) ? values : defaultValues;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，仍返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValues">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public TEnum[] GetArrayValue<TEnum>(string key, TEnum[] defaultValues) where TEnum : struct
        {
            return TryGetArrayValue(key, out TEnum[] values) ? values : defaultValues;
        }

        #endregion GetArrayValue

        /// <summary>
        /// 从配置属性集合中移除所有配置属性。
        /// </summary>
        public void Clear()
        {
            _container.RemoveNodes();
            _properties.Clear();
        }

        /// <summary>
        /// 确定配置属性集合是否包含带有指定键的配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool Contains(string key)
        {
            return _properties.ContainsKey(key);
        }

        /// <summary>
        /// 返回循环访问集合的枚举数。
        /// </summary>
        /// <returns></returns>
        public IEnumerator<HonooProperty> GetEnumerator()
        {
            return _properties.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _properties.Values.GetEnumerator();
        }

        /// <summary>
        /// 从配置属性集合中移除带有指定键的配置属性。和指定键关联的配置属性的注释一并移除。
        /// <br/>如果该元素成功移除，返回 <see langword="true"/>。如果没有找到指定元素，则返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool Remove(string key)
        {
            if (_properties.TryGetValue(key, out HonooProperty value))
            {
                value.Comment?.Remove();
                value.Content.Remove();
                _properties.Remove(key);
                return true;
            }
            return false;
        }
    }
}