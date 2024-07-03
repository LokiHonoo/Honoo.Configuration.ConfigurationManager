using System;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 标签配置属性的基类。<see langword="&lt;add /&gt;"/>、<see langword="&lt;remove /&gt;"/>、<see langword="&lt;clear /&gt;"/> 从此类中继承。
    /// </summary>
    public abstract class TagProperty
    {
        /// <summary>
        /// 配置属性的节点。
        /// </summary>
        protected readonly XElement _content;

        /// <summary>
        /// 配置属性的注释节点。
        /// </summary>
        protected XComment _comment;

        private readonly TagPropertyKind _kind;

        /// <summary>
        /// 获取配置属性的类型。
        /// </summary>
        public TagPropertyKind Kind => _kind;

        internal XComment Comment => _comment;
        internal XElement Content => _content;

        #region Construction

        /// <summary>
        /// 创建 ConfigurationProperty 的新实例。
        /// </summary>
        /// <param name="kind">配置属性的类型。</param>
        /// <param name="content">配置属性的节点。</param>
        /// <param name="comment">配置属性的节点。</param>
        protected TagProperty(TagPropertyKind kind, XElement content, XComment comment)
        {
            _content = content;
            _comment = comment;
            _kind = kind;
        }

        #endregion Construction

        #region Comment

        /// <summary>
        /// 获取注释。如果没有找到注释，返回 <see langword="null"/>。
        /// </summary>
        /// <returns></returns>
        public string GetComment()
        {
            return TryGetComment(out string comment) ? comment : null;
        }

        /// <summary>
        /// 删除注释。如果注释成功删除，返回 <see langword="true"/>。如果没有找到注释节点，则返回 <see langword="false"/>。
        /// </summary>
        /// <returns></returns>
        public bool RemoveComment()
        {
            if (_comment != null)
            {
                _comment.Remove();
                _comment = null;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 添加或更新注释。
        /// </summary>
        /// <param name="comment">注释文本。</param>
        /// <exception cref="Exception"/>
        public void SetComment(string comment)
        {
            if (comment == null)
            {
                if (_comment != null)
                {
                    _comment.Remove();
                    _comment = null;
                }
            }
            else if (_comment == null)
            {
                _comment = new XComment(comment);
                _content.AddBeforeSelf(_comment);
            }
            else
            {
                _comment.Value = comment;
            }
        }

        /// <summary>
        /// 获取注释。如果没有找到注释，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="comment">注释文本。</param>
        /// <returns></returns>
        public bool TryGetComment(out string comment)
        {
            if (_comment != null)
            {
                comment = _comment.Value;
                return true;
            }
            comment = null;
            return false;
        }

        #endregion Comment

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