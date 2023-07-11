using System;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置组。
    /// </summary>
    public sealed class ConfigSectionGroup
    {
        private readonly XElement _content;
        private readonly ConfigSectionGroupSet _groups;
        private readonly ConfigSectionSet _sections;
        private XComment _comment = null;

        /// <summary>
        /// 获取配置组集合。
        /// </summary>
        public ConfigSectionGroupSet Groups => _groups;

        /// <summary>
        /// 获取配置容器集合。
        /// </summary>
        public ConfigSectionSet Sections => _sections;

        #region Construction

        internal ConfigSectionGroup(XElement declaration, XElement content, XComment comment)
        {
            _content = content;
            _comment = comment;
            _groups = new ConfigSectionGroupSet(declaration, content);
            _sections = new ConfigSectionSet(declaration, content);
        }

        #endregion Construction

        /// <summary>
        /// 获取注释。
        /// <br/>如果没有找到注释，返回 <see langword="null"/>。
        /// </summary>
        /// <param name="comment">配置容器的注释。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool GetComment(out string comment)
        {
            if (_comment != null)
            {
                comment = _comment.Value;
                return true;
            }
            comment = null;
            return false;
        }

        /// <summary>
        /// 删除注释。
        /// </summary>
        /// <exception cref="Exception"/>
        public void RemoveComment()
        {
            if (_comment != null)
            {
                _comment.Remove();
                _comment = null;
            }
        }

        /// <summary>
        /// 添加或更新或删除注释。
        /// </summary>
        /// <param name="comment">配置容器的注释。</param>
        /// <exception cref="Exception"/>
        public void SetComment(string comment)
        {
            if (comment == null)
            {
                RemoveComment();
            }
            else
            {
                if (_comment == null)
                {
                    _comment = new XComment(comment);
                    _content.AddBeforeSelf(_comment);
                }
                else
                {
                    _comment.Value = comment;
                }
            }
        }

        /// <summary>
        /// 方法已重写。返回节点的缩进 XML 文本。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _content.ToString();
        }
    }
}