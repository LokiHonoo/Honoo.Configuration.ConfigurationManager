﻿//using System.Xml;
//using System.Xml.Linq;

//namespace Honoo.Configuration
//{
//    /// <summary>
//    /// 配置容器。
//    /// </summary>
//    public sealed class HonooSection
//    {
//        private readonly ConfigComment _comment;
//        private readonly XElement _content;
//        private readonly HonooDictionary _properties;

//        /// <summary>
//        /// 配置容器的注释。
//        /// </summary>
//        public ConfigComment Comment => _comment;

//        /// <summary>
//        /// 获取配置属性集合。
//        /// </summary>
//        public HonooDictionary Properties => _properties;

//        internal XElement Content => _content;

//        #region Construction

//        internal HonooSection(XElement root)
//        {
//            XName name = HonooSettingsManager.Namespace + "default";
//            _content = root.Element(name);
//            if (_content == null)
//            {
//                _content = new XElement(name);
//                _comment = new ConfigComment(null, _content);
//                root.AddFirst(_content);
//            }
//            else
//            {
//                XNode pre = _content.PreviousNode;
//                if (pre != null && pre.NodeType == XmlNodeType.Comment)
//                {
//                    _comment = new ConfigComment((XComment)pre, _content);
//                }
//                else
//                {
//                    _comment = new ConfigComment(null, _content);
//                }
//            }
//            _properties = new HonooDictionary(_content);
//        }

//        internal HonooSection(XElement content, XComment comment)
//        {
//            _content = content;
//            _comment = new ConfigComment(comment, content);
//            _properties = new HonooDictionary(_content);
//        }

//        #endregion Construction

//        /// <summary>
//        /// 方法已重写。返回节点的缩进 XML 文本。
//        /// </summary>
//        /// <returns></returns>
//        public override string ToString()
//        {
//            return _content.ToString();
//        }
//    }
//}