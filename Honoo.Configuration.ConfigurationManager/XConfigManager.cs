using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 简单配置属性管理器。提供一个精简的配置属性文件，支持加密，支持字典/列表类型无限嵌套。
    /// </summary>
    public sealed class XConfigManager : IDisposable
    {
        #region Properties

        private static readonly XNamespace _namespace = "https://github.com/LokiHonoo/Honoo.Configuration.ConfigurationManager/";
        private static readonly XmlWriterSettings _writerSettings = new XmlWriterSettings() { Indent = true, Encoding = new UTF8Encoding(false) };
        private XDictionary _default;
        private bool _disposed;
        private XElement _root;
        private XSectionSet _sections;

        /// <summary>
        /// 映射到 &lt;default /&gt; 配置容器节点。
        /// </summary>
        public XDictionary Default
        {
            get
            {
                if (!_disposed && _default == null)
                {
                    GetDefault(out XElement content, out XComment comment);
                    _default = new XDictionary(content, comment);
                }
                return _default;
            }
        }

        /// <summary>
        /// 获取配置容器集合。
        /// </summary>
        public XSectionSet Sections
        {
            get
            {
                if (!_disposed && _sections == null)
                {
                    _sections = new XSectionSet(_root);
                }
                return _sections;
            }
        }

        internal static XNamespace Namespace => _namespace;

        #endregion Properties

        #region Delegate

        /// <summary>
        /// 在 XConfigManager 实例内容改变时执行。
        /// </summary>
        /// <param name="manager">XConfigManager 实例。</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1711:标识符应采用正确的后缀", Justification = "<挂起>")]
        public delegate void ChangedEventHandler(XConfigManager manager);

        /// <summary>
        /// 在 XConfigManager 实例释放后执行。
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1711:标识符应采用正确的后缀", Justification = "<挂起>")]
        public delegate void DisposedEventHandler();

        /// <summary>
        /// 在 XConfigManager 实例正在释放时执行。
        /// </summary>
        /// <param name="manager">XConfigManager 实例。</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1711:标识符应采用正确的后缀", Justification = "<挂起>")]
        public delegate void DisposingEventHandler(XConfigManager manager);

        #endregion Delegate

        #region Event

        /// <summary>
        /// 在 XConfigManager 实例内容改变时执行。
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1003:使用泛型事件处理程序实例", Justification = "<挂起>")]
        public event ChangedEventHandler Changed;

        /// <summary>
        /// 在 XConfigManager 实例释放后执行。
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1003:使用泛型事件处理程序实例", Justification = "<挂起>")]
        public event DisposedEventHandler Disposed;

        /// <summary>
        /// 在 XConfigManager 实例准备释放时执行。
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1003:使用泛型事件处理程序实例", Justification = "<挂起>")]
        public event DisposingEventHandler Disposing;

        private void OnChanged()
        {
            Changed?.Invoke(this);
        }

        private void OnDisposed()
        {
            Disposed?.Invoke();
        }

        private void OnDisposing()
        {
            Disposing?.Invoke(this);
        }

        #endregion Event

        #region Construction

        /// <summary>
        /// 创建 XConfigManager 的新实例。
        /// </summary>
        public XConfigManager()
        {
            _root = new XElement(_namespace + "config");
            _root.Changed += (s, e) => { OnChanged(); };
        }

        /// <summary>
        /// 创建 XConfigManager 的新实例。
        /// </summary>
        /// <param name="filePath">指定配置文件的路径。</param>
        /// <param name="protectionAlgorithm">
        /// 指定一个非对称加密算法，用于读取加密配置文件。
        /// <br/>这和 ASP.NET 的默认加密方式无关，生成的加密配置文件仅可使用此项目工具读写。
        /// <br/>算法必须拥有私钥。
        /// </param>
        /// <exception cref="Exception"/>
        public XConfigManager(string filePath, RSA protectionAlgorithm = null)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException($"The invalid argument - {nameof(filePath)}.");
            }
            _root = XElement.Load(filePath);
            _root = Coerce(_root, protectionAlgorithm);
            _root.Changed += (s, e) => { OnChanged(); };
        }

        /// <summary>
        /// 创建 XConfigManager 的新实例。
        /// </summary>
        /// <param name="stream">指定配置文件的流。</param>
        /// <param name="protectionAlgorithm">
        /// 指定一个非对称加密算法，用于读取加密配置文件。
        /// <br/>这和 ASP.NET 的默认加密方式无关，生成的加密配置文件仅可使用此项目工具读写。
        /// <br/>算法必须拥有私钥。
        /// </param>
        /// <exception cref="Exception"/>
        public XConfigManager(Stream stream, RSA protectionAlgorithm = null)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            _root = XElement.Load(stream);
            _root = Coerce(_root, protectionAlgorithm);
            _root.Changed += (s, e) => { OnChanged(); };
        }

        /// <summary>
        /// 创建 XConfigManager 的新实例。
        /// </summary>
        /// <param name="reader">指定配置文件的读取器。</param>
        /// <param name="protectionAlgorithm">
        /// 指定一个非对称加密算法，用于读取加密配置文件。
        /// <br/>这和 ASP.NET 的默认加密方式无关，生成的加密配置文件仅可使用此项目工具读写。
        /// <br/>算法必须拥有私钥。
        /// </param>
        /// <exception cref="Exception"/>
        public XConfigManager(XmlReader reader, RSA protectionAlgorithm = null)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }
            _root = XElement.Load(reader);
            _root = Coerce(_root, protectionAlgorithm);
            _root.Changed += (s, e) => { OnChanged(); };
        }

        /// <summary>
        /// 释放由 <see cref="XConfigManager"/> 使用的所有资源。
        /// </summary>
        ~XConfigManager()
        {
            Dispose(false);
        }

        /// <summary>
        /// 释放由 <see cref="XConfigManager"/> 使用的所有资源。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                OnDisposing();
                if (disposing)
                {
                    _default = null;
                    _sections = null;
                    _root = null;
                }
                _disposed = true;
                OnDisposed();
            }
        }

        #endregion Construction

        #region Save

        /// <summary>
        /// 格式化为缩进 XML 文档并保存到指定的文件。如果文件已存在，覆盖旧文件。
        /// </summary>
        /// <param name="filePath">指定配置文件的路径。</param>
        /// <param name="protectionAlgorithm">
        /// 指定一个非对称加密算法，用于保存加密配置文件。
        /// <br/>这和 ASP.NET 的默认加密方式无关，生成的加密配置文件仅可使用此项目工具读写。
        /// <br/>算法可以是公钥或私钥。
        /// </param>
        /// <exception cref="Exception"/>
        public void Save(string filePath, RSA protectionAlgorithm = null)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException($"The invalid argument - {nameof(filePath)}.");
            }
            XElement root = Clean(_root);
            if (protectionAlgorithm != null)
            {
                root = ProtectionHelper.Encrypt(root, protectionAlgorithm);
            }
            using (XmlWriter writer = XmlWriter.Create(filePath, _writerSettings))
            {
                root.WriteTo(writer);
                writer.Flush();
            }
        }

        /// <summary>
        /// 格式化为缩进 XML 文档并保存到指定的流。
        /// </summary>
        /// <param name="stream">指定配置文件的流。</param>
        /// <param name="protectionAlgorithm">
        /// 指定一个非对称加密算法，用于保存加密配置文件。
        /// <br/>这和 ASP.NET 的默认加密方式无关，生成的加密配置文件仅可使用此项目工具读写。
        /// <br/>算法可以是公钥或私钥。
        /// </param>
        /// <exception cref="Exception"/>
        public void Save(Stream stream, RSA protectionAlgorithm = null)
        {
            XElement root = Clean(_root);
            if (protectionAlgorithm != null)
            {
                root = ProtectionHelper.Encrypt(root, protectionAlgorithm);
            }
            using (XmlWriter writer = XmlWriter.Create(stream, _writerSettings))
            {
                root.WriteTo(writer);
                writer.Flush();
            }
        }

        /// <summary>
        /// 保存到指定的读取器。
        /// </summary>
        /// <param name="writer">指定配置文件的读取器。</param>
        /// <param name="protectionAlgorithm">
        /// 指定一个非对称加密算法，用于保存加密配置文件。
        /// <br/>这和 ASP.NET 的默认加密方式无关，生成的加密配置文件仅可使用此项目工具读写。
        /// <br/>算法可以是公钥或私钥。
        /// </param>
        /// <exception cref="Exception"/>
        public void Save(XmlWriter writer, RSA protectionAlgorithm = null)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }
            XElement root = Clean(_root);
            if (protectionAlgorithm != null)
            {
                root = ProtectionHelper.Encrypt(root, protectionAlgorithm);
            }
            root.WriteTo(writer);
            writer.Flush();
        }

        #endregion Save

        /// <summary>
        /// 清除所有节点，没有被 <see cref="XConfigManager"/> 管理的节点和内容也会全部删除。
        /// </summary>
        /// <returns></returns>
        public void Clear()
        {
            _root.RemoveAll();
        }

        /// <summary>
        /// 返回配置文件的缩进 XML 文档文本。
        /// </summary>
        /// <param name="protectionAlgorithm">
        /// 指定一个非对称加密算法，用于保存加密配置文件。
        /// <br/>这和 ASP.NET 的默认加密方式无关，生成的加密配置文件仅可使用此项目工具读写。
        /// <br/>算法可以是公钥或私钥。
        /// </param>
        /// <exception cref="Exception"/>
        public string GetXmlString(RSA protectionAlgorithm = null)
        {
            XElement root = Clean(_root);
            if (protectionAlgorithm != null)
            {
                root = ProtectionHelper.Encrypt(root, protectionAlgorithm);
            }
            StringBuilder builder = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(builder, _writerSettings))
            {
                root.WriteTo(writer);
                writer.Flush();
                return builder.ToString();
            }
        }

        /// <summary>
        /// 方法已重写。返回根节点的缩进 XML 文本。不包括文档声明。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _root.ToString();
        }

        private static XElement Coerce(XElement root, RSA protectionAlgorithm)
        {
            if (root.Name != _namespace + "config")
            {
                throw new FileLoadException("File is not a config(https://github.com/LokiHonoo/Honoo.Configuration.ConfigurationManager/) file.");
            }
            if (root.Attribute("protected") is XAttribute attribute)
            {
                if (bool.TryParse(attribute.Value, out bool isProtected))
                {
                    if (isProtected)
                    {
                        root = ProtectionHelper.Decrypt(root, protectionAlgorithm);
                    }
                }
                else
                {
                    throw new CryptographicException($"Attribute \"protected\" is not a boolean value.");
                }
            }
            return root;
        }

        private XElement Clean(XElement root)
        {
            XElement result = XElement.Parse(root.ToString());
            if (_default != null && _default.Properties.Count == 0 && !_default.Comment.HasValue)
            {
                result.Element("default").Remove();
            }
            if (_sections != null && _sections.Count > 0)
            {
                foreach (XElement section in result.Elements("section"))
                {
                    XComment comment = null;
                    XNode pre = section.PreviousNode;
                    if (pre != null && pre.NodeType == XmlNodeType.Comment)
                    {
                        comment = (XComment)pre;
                    }
                    if (!section.HasElements && comment == null)
                    {
                        section.Remove();
                    }
                }
            }
            return result;
        }

        private void GetDefault(out XElement content, out XComment comment)
        {
            comment = null;
            content = _root.Element(_namespace + "default");
            if (content == null)
            {
                content = new XElement(_namespace + "default");
                _root.AddFirst(content);
            }
            else
            {
                XNode pre = content.PreviousNode;
                if (pre != null && pre.NodeType == XmlNodeType.Comment)
                {
                    comment = (XComment)pre;
                }
            }
        }
    }
}