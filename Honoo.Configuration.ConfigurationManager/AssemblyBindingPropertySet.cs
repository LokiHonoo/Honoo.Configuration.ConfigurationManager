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
    public class AssemblyBindingPropertySet : IEnumerable<string>
    {
        #region Class

        /// <summary>
        /// 代表此配置属性集合的值的集合。
        /// </summary>
        public sealed class ValueCollection : IEnumerable<string>
        {
            #region Properties

            private readonly List<string> _properties;

            /// <summary>
            /// 获取配置属性集合的值的元素数。
            /// </summary>
            public int Count => _properties.Count;

            #endregion Properties

            internal ValueCollection(List<string> properties)
            {
                _properties = properties;
            }

            /// <summary>
            /// 从指定数组索引开始将值元素复制到到指定数组。
            /// </summary>
            /// <param name="array">要复制到的目标数组。</param>
            /// <param name="arrayIndex">目标数组中从零开始的索引，从此处开始复制。</param>
            public void CopyTo(string[] array, int arrayIndex)
            {
                _properties.CopyTo(array, arrayIndex);
            }

            /// <summary>
            /// 返回循环访问集合的枚举数。
            /// </summary>
            /// <returns></returns>
            public IEnumerator<string> GetEnumerator()
            {
                return _properties.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _properties.GetEnumerator();
            }
        }

        #endregion Class

        #region Properties

        private readonly List<XComment> _comments = new List<XComment>();
        private readonly List<XElement> _contents = new List<XElement>();
        private readonly List<string> _properties = new List<string>();
        private readonly XElement _superior;
        private readonly ValueCollection _valueExhibits;

        /// <summary>
        /// 获取配置属性集合中包含的元素数。
        /// </summary>
        public int Count => _properties.Count;

        /// <summary>
        /// 获取配置属性集合的值的集合。
        /// </summary>
        public ValueCollection Values => _valueExhibits;

        /// <summary>
        /// 获取或设置指定索引处的配置属性的值。
        /// <br/>取值时如果没有找到指定索引，返回 <see langword="null"/>。使用 <see cref="GetValue(int, string)"/> 设置没有找到指定索引时的默认值。
        /// </summary>
        /// <param name="index">配置属性在集合中的索引。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public string this[int index]
        {
            get => TryGetValue(index, out string href) ? href : null;
            set { Add(value); }
        }

        #endregion Properties

        #region Construction

        internal AssemblyBindingPropertySet(XElement superior)
        {
            _superior = superior;
            if (superior.HasElements)
            {
                IEnumerator<XNode> enumerator = superior.Nodes().GetEnumerator();
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
                            if (content.Name == "linkedConfiguration")
                            {
                                string value = content.Attribute("href").Value;
                                _properties.Add(value);
                                _contents.Add(content);
                                _comments.Add(comment);
                            }
                        }
                        comment = null;
                    }
                }
            }
            _valueExhibits = new ValueCollection(_properties);
        }

        #endregion Construction

        #region Add

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="href">要包含的配置文件的 URL。 href 属性支持的唯一格式是 file://。 支持本地文件和 UNC 文件。</param>
        /// <exception cref="Exception"/>
        public void Add(string href)
        {
            if (href == null)
            {
                throw new ArgumentException($"The invalid argument - {nameof(href)}.");
            }
            XElement content = new XElement("linkedConfiguration");
            content.SetAttributeValue("href", href);
            _properties.Add(href);
            _contents.Add(content);
            _comments.Add(null);
            _superior.Add(content);
        }

        #endregion Add

        #region GetValue

        /// <summary>
        /// 获取与指定索引处的配置属性的值。
        /// <br/>如果没有找到指定索引，返回 <paramref name="defaultHref"/>。
        /// </summary>
        /// <param name="index">配置属性的索引。</param>
        /// <param name="defaultHref">没有找到指定索引时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public string GetValue(int index, string defaultHref)
        {
            return TryGetValue(index, out string href) ? href : defaultHref;
        }

        #endregion GetValue

        #region TrySetValue

        /// <summary>
        /// 修改指定索引处的配置属性的值。
        /// <br/>如果没有找到指定索引，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="index">配置属性的索引。</param>
        /// <param name="href">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TrySetValue(int index, string href)
        {
            if (index < _properties.Count)
            {
                _properties[index] = href;
                return true;
            }
            return false;
        }

        #endregion TrySetValue

        #region TryGetValue

        /// <summary>
        /// 获取指定索引处的配置属性的值。
        /// <br/>如果没有找到指定索引，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="index">配置属性的索引。</param>
        /// <param name="href">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(int index, out string href)
        {
            if (index < _properties.Count)
            {
                href = _properties[index];
                return true;
            }
            href = default;
            return false;
        }

        #endregion TryGetValue

        #region Comment

        /// <summary>
        /// 删除一个与指定索引处配置属性关联的的注释。
        /// </summary>
        /// <param name="index">配置属性的索引。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool RemoveComment(int index)
        {
            if (index < _comments.Count)
            {
                XComment comment_ = _comments[index];
                if (comment_ != null)
                {
                    comment_.Remove();
                    _comments[index] = null;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取与指定索引处配置属性关联的注释。
        /// <br/>如果没有找到指定索引，返回 <see langword="false"/>。如果找到了指定索引但没有注释节点，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="index">配置属性的索引。</param>
        /// <param name="comment">配置属性的注释。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetComment(int index, out string comment)
        {
            if (index < _comments.Count)
            {
                XComment comment_ = _comments[index];
                if (comment_ != null)
                {
                    comment = comment_.Value;
                    return true;
                }
            }
            comment = null;
            return false;
        }

        /// <summary>
        /// 添加或更新或删除一个与指定索引处的配置属性关联的注释。
        /// </summary>
        /// <param name="index">配置属性的索引。</param>
        /// <param name="comment">配置属性的注释。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TrySetComment(int index, string comment)
        {
            if (comment == null)
            {
                RemoveComment(index);
                return true;
            }
            else
            {
                if (index < _comments.Count)
                {
                    XComment comment_ = _comments[index];
                    if (comment_ == null)
                    {
                        comment_ = new XComment(comment);
                        _contents[index].AddBeforeSelf(comment_);
                        _comments[index] = comment_;
                    }
                    else
                    {
                        comment_.Value = comment;
                    }
                    return true;
                }
            }
            return false;
        }

        #endregion Comment

        /// <summary>
        /// 从配置属性集合中移除所有配置属性。
        /// </summary>
        public void Clear()
        {
            _properties.Clear();
            _contents.Clear();
            _comments.Clear();
            _superior.RemoveNodes();
        }

        /// <summary>
        /// 返回循环访问集合的枚举数。
        /// </summary>
        /// <returns></returns>
        public IEnumerator<string> GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        /// <summary>
        /// 从配置属性集合中移除指定索引处的配置属性。和配置属性相关的注释一并移除。
        /// <br/>如果该元素成功移除，返回 <see langword="true"/>。如果没有找到指定索引，则返回 <see langword="false"/>。
        /// </summary>
        /// <param name="index">配置属性的索引。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool Remove(int index)
        {
            if (index < _comments.Count)
            {
                _properties.RemoveAt(index);
                _contents[index].Remove();
                _contents.RemoveAt(index);
                _comments[index]?.Remove();
                _comments.RemoveAt(index);
                return true;
            }
            return false;
        }
    }
}