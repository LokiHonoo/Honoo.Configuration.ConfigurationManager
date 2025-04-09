using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置管理器。提供对标准节点的有限读写支持。
    /// </summary>
    public sealed class ConfigurationManager : IDisposable
    {
        #region Members

        private static readonly XNamespace _assemblyBindingNamespace = "urn:schemas-microsoft-com:asm.v1";
        private static readonly XmlReaderSettings _readerSettings = new XmlReaderSettings() { IgnoreWhitespace = true };
        private static readonly XmlWriterSettings _writerSettings = new XmlWriterSettings() { Indent = true, Encoding = new UTF8Encoding(false) };
        private AppSettings _appSettings;
        private AssemblyBinding _assemblyBinding;
        private ConfigSections _configSections;
        private ConnectionStrings _connectionStrings;
        private bool _disposed;
        private XDocument _document;

        /// <summary>
        /// 映射到标准格式的 &lt;appSettings /&gt; 节点。
        /// </summary>
        public AppSettings AppSettings
        {
            get
            {
                if (!_disposed && _appSettings == null)
                {
                    _appSettings = new AppSettings(_document.Root);
                }
                return _appSettings;
            }
        }

        /// <summary>
        /// 映射到标准格式的 &lt;assemblyBinding /&gt; 节点。这是配置级的程序集绑定策略节点。
        /// </summary>
        public AssemblyBinding AssemblyBinding
        {
            get
            {
                if (!_disposed && _assemblyBinding == null)
                {
                    _assemblyBinding = new AssemblyBinding(_document.Root);
                }
                return _assemblyBinding;
            }
        }

        /// <summary>
        /// 映射到标准格式的 &lt;configSections /&gt; 节点。
        /// </summary>
        public ConfigSections ConfigSections
        {
            get
            {
                if (!_disposed && _configSections == null)
                {
                    _configSections = new ConfigSections(_document.Root);
                }
                return _configSections;
            }
        }

        /// <summary>
        /// 映射到标准格式的 &lt;connectionStrings /&gt; 节点。
        /// </summary>
        public ConnectionStrings ConnectionStrings
        {
            get
            {
                if (!_disposed && _connectionStrings == null)
                {
                    _connectionStrings = new ConnectionStrings(_document.Root);
                }
                return _connectionStrings;
            }
        }

        internal static XNamespace AssemblyBindingNamespace => _assemblyBindingNamespace;

        internal static XmlReaderSettings ReaderSettings => _readerSettings;

        #endregion Members

        #region Delegate

        /// <summary>
        /// 在 ConfigurationManager 实例内容改变时执行。
        /// </summary>
        /// <param name="manager">ConfigurationManager 实例。</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1711:标识符应采用正确的后缀", Justification = "<挂起>")]
        public delegate void ChangedEventHandler(ConfigurationManager manager);

        /// <summary>
        /// 在 ConfigurationManager 实例释放后执行。
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1711:标识符应采用正确的后缀", Justification = "<挂起>")]
        public delegate void DisposedEventHandler();

        /// <summary>
        /// 在 ConfigurationManager 实例正在释放时执行。
        /// </summary>
        /// <param name="manager">ConfigurationManager 实例。</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1711:标识符应采用正确的后缀", Justification = "<挂起>")]
        public delegate void DisposingEventHandler(ConfigurationManager manager);

        #endregion Delegate

        #region Event

        /// <summary>
        /// 在 ConfigurationManager 实例内容改变时执行。慎用，每个修改都会调用此事件。
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1003:使用泛型事件处理程序实例", Justification = "<挂起>")]
        public event ChangedEventHandler Changed;

        /// <summary>
        /// 在 ConfigurationManager 实例释放后执行。
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1003:使用泛型事件处理程序实例", Justification = "<挂起>")]
        public event DisposedEventHandler Disposed;

        /// <summary>
        /// 在 ConfigurationManager 实例准备释放时执行。
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
        /// 创建 ConfigurationManager 的新实例。
        /// </summary>
        public ConfigurationManager()
        {
            _document = new XDocument(new XDeclaration("1.0", "utf-8", string.Empty), new XElement("configuration"));
            _document.Changed += (s, e) => { OnChanged(); };
        }

        /// <summary>
        /// 创建 ConfigurationManager 的新实例。
        /// </summary>
        /// <param name="filePath">指定配置文件的路径。</param>
        /// <exception cref="Exception"/>
        public ConfigurationManager(string filePath)
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
        /// 创建 ConfigurationManager 的新实例。
        /// </summary>
        /// <param name="filePath">指定配置文件的路径。</param>
        /// <param name="createNewIfFileNotExists">如果文件不存在，创建无内容的 XConfigManager 实例。此时不会写入到文件路径。</param>
        /// <exception cref="Exception"/>
        public ConfigurationManager(string filePath, bool createNewIfFileNotExists)
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
                _document = new XDocument(new XDeclaration("1.0", "utf-8", string.Empty), new XElement("configuration"));
            }
            else
            {
                throw new FileNotFoundException($"The file \"{filePath}\" is not found.");
            }
            _document.Changed += (s, e) => { OnChanged(); };
        }

        /// <summary>
        /// 创建 ConfigurationManager 的新实例。
        /// </summary>
        /// <param name="stream">指定配置文件的流。</param>
        /// <exception cref="Exception"/>
        public ConfigurationManager(Stream stream)
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
        /// 创建 ConfigurationManager 的新实例。
        /// </summary>
        /// <param name="reader">指定配置文件的读取器。</param>
        /// <exception cref="Exception"/>
        public ConfigurationManager(XmlReader reader)
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
        /// 释放由 <see cref="ConfigurationManager"/> 使用的所有资源。
        /// </summary>
        ~ConfigurationManager()
        {
            Dispose(false);
        }

        /// <summary>
        /// 释放由 <see cref="ConfigurationManager"/> 使用的所有资源。
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
                    _appSettings = null;
                    _assemblyBinding = null;
                    _connectionStrings = null;
                    _configSections = null;
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
        /// 清除所有节点，没有被 <see cref="ConfigurationManager"/> 管理的节点和内容也会全部删除。
        /// </summary>
        /// <returns></returns>
        public void Clear()
        {
            _appSettings = null;
            _assemblyBinding = null;
            _connectionStrings = null;
            _configSections = null;
            _document.Root.RemoveAll();
        }

        /// <summary>
        /// 获取 XML 文档的副本。
        /// </summary>
        public XDocument CloneDocument()
        {
            return new XDocument(_document);
        }

        /// <summary>
        /// 方法已重写。返回根节点的缩进 XML 文本。不包括文档声明。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _document.ToString();
        }

        private static XDocument Coerce(XDocument document)
        {
            if (document.Root.Name != "configuration")
            {
                throw new FileLoadException("File is not a configuration file.");
            }
            return document;
        }

        private XDocument Clean()
        {
            XDocument result = new XDocument(_document);
            if (_appSettings != null && _appSettings.Properties.Count == 0)
            {
                result.Element("appSettings").Remove();
            }
            else { }
            if (_assemblyBinding != null && _assemblyBinding.Properties.Count == 0)
            {
                result.Element(_assemblyBindingNamespace + "assemblyBinding").Remove();
            }
            if (_connectionStrings != null && _connectionStrings.Properties.Count == 0)
            {
                result.Element("connectionStrings").Remove();
            }
            else
            {
            }
            if (_configSections != null && _configSections.Sections.Count == 0 && _configSections.Groups.Count == 0)
            {
                result.Element("configSections").Remove();
            }
            else
            {
            }
            return result;
        }
    }
}