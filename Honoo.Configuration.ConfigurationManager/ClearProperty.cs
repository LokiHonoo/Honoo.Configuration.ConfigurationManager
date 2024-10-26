using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置属性。
    /// </summary>
    public sealed class ClearProperty : TagProperty
    {
        #region Construction

        /// <summary>
        /// 创建 ClearProperty 的新实例。
        /// </summary>
        public ClearProperty() : base(TagPropertyKind.ClearProperty, new XElement("clear"), null)
        {
        }

        internal ClearProperty(XElement content, XComment comment) : base(TagPropertyKind.ClearProperty, content, comment)
        {
        }

        #endregion Construction
    }
}