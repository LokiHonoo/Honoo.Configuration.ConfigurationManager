using System;
using System.IO;
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
        #region Members

        private static readonly XNamespace _namespace = "https://github.com/LokiHonoo/Honoo.Configuration.ConfigurationManager/";
        private static readonly XmlReaderSettings _readerSettings = new XmlReaderSettings() { IgnoreWhitespace = true };
        private static readonly XmlWriterSettings _writerSettings = new XmlWriterSettings() { Indent = true, Encoding = new UTF8Encoding(false) };
        private XDictionary _default;
        private bool _disposed;
        private XDocument _document;
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
                    _default = GetDefault();
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
                    _sections = new XSectionSet(_document.Root);
                }
                return _sections;
            }
        }

        internal static XNamespace Namespace => _namespace;

        #endregion Members

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
            _document = new XDocument(new XDeclaration("1.0", "utf-8", string.Empty), new XElement(_namespace + "config"));
            _document.Changed += (s, e) => { OnChanged(); };
        }

        /// <summary>
        /// 创建 XConfigManager 的新实例。
        /// </summary>
        /// <param name="filePath">指定配置文件的路径。</param>
        /// <exception cref="Exception"/>
        public XConfigManager(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException($"The invalid argument - {nameof(filePath)}.");
            }
            using (XmlReader reader = XmlReader.Create(filePath, _readerSettings))
            {
                _document = XDocument.Load(reader);
                _document = Coerce(_document);
                _document.Changed += (s, e) => { OnChanged(); };
            }
        }

        /// <summary>
        /// 创建 XConfigManager 的新实例。
        /// </summary>
        /// <param name="filePath">指定配置文件的路径。</param>
        /// <param name="createNewIfFileNotExists">如果文件不存在，创建无内容的 XConfigManager 实例。此时不会写入到文件路径。</param>
        /// <exception cref="Exception"/>
        public XConfigManager(string filePath, bool createNewIfFileNotExists)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException($"The invalid argument - {nameof(filePath)}.");
            }
            if (File.Exists(filePath))
            {
                using (XmlReader reader = XmlReader.Create(filePath, _readerSettings))
                {
                    _document = XDocument.Load(reader);
                    _document = Coerce(_document);
                }
            }
            else if (createNewIfFileNotExists)
            {
                _document = new XDocument(new XDeclaration("1.0", "utf-8", string.Empty), new XElement(_namespace + "config"));
            }
            else
            {
                throw new FileNotFoundException($"The file \"{filePath}\" is not found.");
            }
            _document.Changed += (s, e) => { OnChanged(); };
        }

        /// <summary>
        /// 创建 XConfigManager 的新实例。
        /// </summary>
        /// <param name="stream">指定配置文件的流。</param>
        /// <exception cref="Exception"/>
        public XConfigManager(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            using (XmlReader reader = XmlReader.Create(stream, _readerSettings))
            {
                _document = XDocument.Load(reader);
                _document = Coerce(_document);
                _document.Changed += (s, e) => { OnChanged(); };
            }
        }

        /// <summary>
        /// 创建 XConfigManager 的新实例。
        /// </summary>
        /// <param name="reader">指定配置文件的读取器。</param>
        /// <exception cref="Exception"/>
        public XConfigManager(XmlReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }
            using (XmlReader readerF = XmlReader.Create(reader, _readerSettings))
            {
                _document = XDocument.Load(readerF);
                _document = Coerce(_document);
                _document.Changed += (s, e) => { OnChanged(); };
            }
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
                    _document = null;
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
        /// <exception cref="Exception"/>
        public void Save(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException($"The invalid argument - {nameof(filePath)}.");
            }
            XDocument document = Clean();
            using (XmlWriter writer = XmlWriter.Create(filePath, _writerSettings))
            {
                document.WriteTo(writer);
                writer.Flush();
            }
        }

        /// <summary>
        /// 格式化为缩进 XML 文档并保存到指定的流。
        /// </summary>
        /// <param name="stream">指定配置文件的流。</param>
        /// <exception cref="Exception"/>
        public void Save(Stream stream)
        {
            XDocument document = Clean();
            using (XmlWriter writer = XmlWriter.Create(stream, _writerSettings))
            {
                document.WriteTo(writer);
                writer.Flush();
            }
        }

        /// <summary>
        /// 保存到指定的写入器。
        /// </summary>
        /// <param name="writer">指定配置文件的写入器。</param>
        /// <exception cref="Exception"/>
        public void Save(XmlWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }
            XDocument document = Clean();
            document.WriteTo(writer);
            writer.Flush();
        }

        #endregion Save

        /// <summary>
        /// 清除所有节点，没有被 <see cref="XConfigManager"/> 管理的节点和内容也会全部删除。
        /// </summary>
        /// <returns></returns>
        public void Clear()
        {
            _default = null;
            _sections = null;
            _document.Root.RemoveAll();
        }

        /// <summary>
        /// 获取 XML 文档的副本。
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1024:在适用处使用属性", Justification = "<挂起>")]
        public XDocument GetDocumentClone()
        {
            return new XDocument(_document);
        }

        /// <summary>
        /// 方法已重写。返回 XML 文档的缩进 XML 文本。不包括文档声明。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _document.ToString();
        }

        private static XDocument Coerce(XDocument document)
        {
            if (document.Root.Name != _namespace + "config")
            {
                throw new FileLoadException("File is not a config(https://github.com/LokiHonoo/Honoo.Configuration.ConfigurationManager/) file.");
            }
            return document;
        }

        private XDocument Clean()
        {
            XDocument result = new XDocument(_document);
            if (_default != null && _default.Properties.Count == 0 && !_default.Comment.HasValue)
            {
                result.Element("default").Remove();
            }
            return result;
        }

        private XDictionary GetDefault()
        {
            XComment comment = null;
            XElement content = _document.Root.Element(_namespace + "default");
            if (content == null)
            {
                content = new XElement(_namespace + "default");
                _document.Root.AddFirst(content);
            }
            else
            {
                XNode pre = content.PreviousNode;
                if (pre != null && pre.NodeType == XmlNodeType.Comment)
                {
                    comment = (XComment)pre;
                }
            }
            return new XDictionary(content, comment, ProtectionHelper.QueryProtected(content));
        }
    }
}