using System;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 注释。
    /// </summary>
    public sealed class ConfigComment
    {
        private readonly XElement _leader;
        private XComment _comment;

        /// <summary>
        /// 获取一个值，指示是否有注释节点。
        /// </summary>
        public bool HasValue => _comment != null;

        internal XComment Comment => _comment;

        #region Construction

        /// <summary>
        /// 创建 ConfigComment 的新实例。
        /// </summary>
        /// <param name="comment">注释节点。</param>
        /// <param name="leader">内容节点。</param>
        internal ConfigComment(XComment comment, XElement leader)
        {
            _comment = comment;
            _leader = leader;
        }

        #endregion Construction

        /// <summary>
        /// 获取注释。如果没有注释节点，返回 <see langword="null"/>。
        /// </summary>
        /// <returns></returns>
        public string GetValue()
        {
            return TryGetValue(out string comment) ? comment : null;
        }

        /// <summary>
        /// 删除注释。如果注释成功删除，返回 <see langword="true"/>。如果没有找到注释节点，则返回 <see langword="false"/>。
        /// </summary>
        /// <returns></returns>
        public bool Remove()
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
        /// <param name="value">注释文本。</param>
        /// <exception cref="Exception"/>
        public void SetValue(string value)
        {
            if (value == null)
            {
                if (_comment != null)
                {
                    _comment.Remove();
                    _comment = null;
                }
            }
            else if (_comment == null)
            {
                _comment = new XComment(value);
                _leader.AddBeforeSelf(_comment);
            }
            else
            {
                _comment.Value = value;
            }
        }

        /// <summary>
        /// 获取注释。如果没有注释节点，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">注释文本。</param>
        /// <returns></returns>
        public bool TryGetValue(out string value)
        {
            if (_comment != null)
            {
                value = _comment.Value;
                return true;
            }
            value = null;
            return false;
        }
    }
}