using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置容器集合。
    /// </summary>
    public sealed class ConfigSectionSet : IEnumerable<KeyValuePair<string, ConfigSection>>
    {
        #region Members

        private static readonly Dictionary<Type, ConfigSectionKind> _kind = new Dictionary<Type, ConfigSectionKind>()
        {
            { typeof(TextSection), ConfigSectionKind.TextSection },
            { typeof(SingleTagSection), ConfigSectionKind.SingleTagSection },
            { typeof(NameValueSection), ConfigSectionKind.NameValueSection },
            { typeof(DictionarySection), ConfigSectionKind.DictionarySection }
        };

        private readonly XElement _contentContainer;
        private readonly XElement _declarationContainer;
        private readonly Dictionary<string, ConfigSection> _sections = new Dictionary<string, ConfigSection>();

        /// <summary>
        /// 获取配置容器集合中包含的元素数。
        /// </summary>
        public int Count => _sections.Count;

        /// <summary>
        /// 获取配置容器集合的键的集合。
        /// </summary>
        public Dictionary<string, ConfigSection>.KeyCollection Keys => _sections.Keys;

        /// <summary>
        /// 获取配置容器集合的值的集合。
        /// </summary>
        public Dictionary<string, ConfigSection>.ValueCollection Values => _sections.Values;

        /// <summary>
        /// 获取与指定名称关联的配置容器。
        /// </summary>
        /// <param name="name">配置容器的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ConfigSection this[string name] => GetValue(name);

        #endregion Members

        #region Construction

        internal ConfigSectionSet(XElement declarationContainer, XElement contentContainer)
        {
            _declarationContainer = declarationContainer;
            _contentContainer = contentContainer;
            if (_declarationContainer.HasElements)
            {
                foreach (XElement declaration in _declarationContainer.Elements("section"))
                {
                    string name = declaration.Attribute("name").Value;
                    string type = declaration.Attribute("type").Value;
                    XElement content = _contentContainer.Element(name);
                    if (content != null)
                    {
                        if (content.Attribute("configProtectionProvider") != null)
                        {
                            throw new CryptographicException("Encryped configuration sections are not supported.");
                        }
                        XComment comment = null;
                        XNode pre = content.PreviousNode;
                        if (pre != null && pre.NodeType == XmlNodeType.Comment)
                        {
                            comment = (XComment)pre;
                        }
                        ConfigSection section;
                        switch (type)
                        {
                            case "DictionarySectionHandler":
                            case "System.Configuration.DictionarySectionHandler":
                            case "System.Configuration.DictionarySectionHandler, System.Configuration":
                                section = new DictionarySection(declaration, content, comment);
                                break;

                            case "NameValueSectionHandler":
                            case "System.Configuration.NameValueSectionHandler":
                            case "System.Configuration.NameValueSectionHandler, System.Configuration":
                                section = new NameValueSection(declaration, content, comment);
                                break;

                            case "SingleTagSectionHandler":
                            case "System.Configuration.SingleTagSectionHandler":
                            case "System.Configuration.SingleTagSectionHandler, System.Configuration":
                                section = new SingleTagSection(declaration, content, comment);
                                break;

                            case "TextSectionHandler":
                            case "Honoo.Configuration.TextSectionHandler":
                            case "Honoo.Configuration.TextSectionHandler, Honoo.Configuration":
                            default:
                                section = new TextSection(declaration, content, comment);
                                break;
                        }
                        _sections.Add(name, section);
                    }
                }
            }
        }

        #endregion Construction

        #region Add

        /// <summary>
        /// 添加一个配置容器。
        /// </summary>
        /// <typeparam name="T">添加配置容器时使用的类型。</typeparam>
        /// <param name="name">配置容器的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public T Add<T>(string name) where T : ConfigSection
        {
            XElement declaration = new XElement("section");
            declaration.SetAttributeValue("name", name);
            XElement content = new XElement(name);
            ConfigSection section;
            switch (_kind[typeof(T)])
            {
                case ConfigSectionKind.TextSection:
                    declaration.SetAttributeValue("type", "Honoo.Configuration.TextSectionHandler");
                    section = new TextSection(declaration, content, null);
                    break;

                case ConfigSectionKind.SingleTagSection:
                    declaration.SetAttributeValue("type", "System.Configuration.SingleTagSectionHandler");
                    section = new SingleTagSection(declaration, content, null);
                    break;

                case ConfigSectionKind.NameValueSection:
                    declaration.SetAttributeValue("type", "System.Configuration.NameValueSectionHandler");
                    section = new NameValueSection(declaration, content, null);
                    break;

                case ConfigSectionKind.DictionarySection:
                    declaration.SetAttributeValue("type", "System.Configuration.DictionarySectionHandler");
                    section = new DictionarySection(declaration, content, null);
                    break;

                default: throw new ArgumentException($"The invalid argument - {nameof(T)}.");
            }
            _sections.Add(name, section);
            _declarationContainer.Add(declaration);
            _contentContainer.Add(content);
            return section as T;
        }

        #endregion Add

        #region GetOrAdd

        /// <summary>
        /// 获取与指定名称关联的配置容器的值。如果不存在，添加一个 <typeparamref name="T"/> 类型的配置容器并返回值。
        /// <br/>如果配置容器存在但不是指定的类型，则抛出 <see cref="InvalidCastException"/>。
        /// </summary>
        /// <typeparam name="T">添加配置容器时使用的类型。</typeparam>
        /// <param name="name">配置容器的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public T GetOrAdd<T>(string name) where T : ConfigSection
        {
            if (_sections.TryGetValue(name, out ConfigSection value))
            {
                if (value is T val)
                {
                    return val;
                }
                else
                {
                    throw new InvalidCastException($"The section exists but is not of the specified type - section type:{value.GetType()}.");
                }
            }
            else
            {
                return Add<T>(name);
            }
        }

        #endregion GetOrAdd

        #region TryGetValue

        /// <summary>
        /// 获取与指定名称关联的配置容器的值。
        /// <br/>如果没有找到指定名称，返回 <see langword="false"/>。如果找到了指定名称但指定的类型不符，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <typeparam name="T">指定配置属性类型。</typeparam>
        /// <param name="name">配置容器的名称。</param>
        /// <param name="value">配置容器的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue<T>(string name, out T value) where T : ConfigSection
        {
            if (_sections.TryGetValue(name, out ConfigSection val))
            {
                if (val is T va)
                {
                    value = va;
                    return true;
                }
            }
            value = null;
            return false;
        }

        #endregion TryGetValue

        #region GetValue

        /// <summary>
        /// 获取与指定名称关联的配置容器的值。
        /// </summary>
        /// <param name="name">配置容器的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ConfigSection GetValue(string name)
        {
            return TryGetValue(name, out ConfigSection value) ? value : null;
        }

        #endregion GetValue

        /// <summary>
        /// 从配置容器集合中移除所有配置容器。
        /// </summary>
        public void Clear()
        {
            foreach (string name in _sections.Keys)
            {
                Remove(name);
            }
        }

        /// <summary>
        /// 确定配置容器集合是否包含带有指定名称的配置容器。
        /// </summary>
        /// <param name="name">配置容器的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool ContainsName(string name)
        {
            return _sections.ContainsKey(name);
        }

        /// <summary>
        /// 返回循环访问集合的枚举数。
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, ConfigSection>> GetEnumerator()
        {
            return _sections.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _sections.GetEnumerator();
        }

        /// <summary>
        /// 从配置容器集合中移除带有指定名称的配置容器。和指定名称关联的配置容器的注释一并移除。
        /// <br/>如果该元素成功移除，返回 <see langword="true"/>。如果没有找到指定名称，则返回 <see langword="false"/>。
        /// </summary>
        /// <param name="name">配置容器的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool Remove(string name)
        {
            if (_sections.TryGetValue(name, out ConfigSection value))
            {
                value.Comment.Remove();
                value.Declaration.Remove();
                value.Content.Remove();
                _sections.Remove(name);
                return true;
            }
            return false;
        }
    }
}