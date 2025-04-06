using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// AppSettings 附加配置管理器。提供对 appSettings 的 "file" 属性指定的附加文件的有限读写支持。
    /// </summary>
    public sealed class AppSettingsManager : IDisposable
    {
        #region Members

        private static readonly XmlReaderSettings _readerSettings = new XmlReaderSettings() { IgnoreWhitespace = true };
        private static readonly XmlWriterSettings _writerSettings = new XmlWriterSettings() { Indent = true, Encoding = new UTF8Encoding(false) };
        private bool _disposed;
        private XDocument _document;
        private DictionaryPropertySet _properties;

        /// <summary>
        /// 获取配置属性集合。
        /// </summary>
        public DictionaryPropertySet Properties => _properties;

        #endregion Members

        #region Delegate

        /// <summary>
        /// 在 AppSettingsManager 实例内容改变时执行。
        /// </summary>
        /// <param name="manager">AppSettingsManager 实例。</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1711:标识符应采用正确的后缀", Justification = "<挂起>")]
        public delegate void ChangedEventHandler(AppSettingsManager manager);

        /// <summary>
        /// 在 AppSettingsManager 实例释放后执行。
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1711:标识符应采用正确的后缀", Justification = "<挂起>")]
        public delegate void DisposedEventHandler();

        /// <summary>
        /// 在 AppSettingsManager 实例正在释放时执行。
        /// </summary>
        /// <param name="manager">AppSettingsManager 实例。</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1711:标识符应采用正确的后缀", Justification = "<挂起>")]
        public delegate void DisposingEventHandler(AppSettingsManager manager);

        #endregion Delegate

        #region Event

        /// <summary>
        /// 在 AppSettingsManager 实例内容改变时执行。
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1003:使用泛型事件处理程序实例", Justification = "<挂起>")]
        public event ChangedEventHandler Changed;

        /// <summary>
        /// 在 AppSettingsManager 实例释放后执行。
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1003:使用泛型事件处理程序实例", Justification = "<挂起>")]
        public event DisposedEventHandler Disposed;

        /// <summary>
        /// 在 AppSettingsManager 实例准备释放时执行。
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
        /// 创建 AppSettingsManager 的新实例。
        /// </summary>
        public AppSettingsManager()
        {
            _document = new XDocument(new XDeclaration("1.0", "utf-8", string.Empty), new XElement("appSettings"));
            _properties = GetPropertySet(_document);
            _document.Changed += (s, e) => { OnChanged(); };
        }

        /// <summary>
        /// 创建 AppSettingsManager 的新实例。
        /// </summary>
        /// <param name="filePath">指定配置文件的路径。</param>
        /// <exception cref="Exception"/>
        public AppSettingsManager(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException($"The invalid argument - {nameof(filePath)}.");
            }
            using (XmlReader reader = XmlReader.Create(filePath, _readerSettings))
            {
                _document = XDocument.Load(reader);
                _document = Coerce(_document);
                _properties = GetPropertySet(_document);
                _document.Changed += (s, e) => { OnChanged(); };
            }
        }

        /// <summary>
        /// 创建 AppSettingsManager 的新实例。
        /// </summary>
        /// <param name="filePath">指定配置文件的路径。</param>
        /// <param name="createNewIfFileNotExists">如果文件不存在，创建无内容的 XConfigManager 实例。此时不会写入到文件路径。</param>
        /// <exception cref="Exception"/>
        public AppSettingsManager(string filePath, bool createNewIfFileNotExists)
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
                    _properties = GetPropertySet(_document);
                }
            }
            else if (createNewIfFileNotExists)
            {
                _document = new XDocument(new XDeclaration("1.0", "utf-8", string.Empty), new XElement("appSettings"));
                _properties = GetPropertySet(_document);
            }
            else
            {
                throw new FileNotFoundException($"The file \"{filePath}\" is not found.");
            }
            _document.Changed += (s, e) => { OnChanged(); };
        }

        /// <summary>
        /// 创建 AppSettingsManager 的新实例。
        /// </summary>
        /// <param name="stream">指定配置文件的流。</param>
        /// <exception cref="Exception"/>
        public AppSettingsManager(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            using (XmlReader reader = XmlReader.Create(stream, _readerSettings))
            {
                _document = XDocument.Load(reader);
                _document = Coerce(_document);
                _properties = GetPropertySet(_document);
                _document.Changed += (s, e) => { OnChanged(); };
            }
        }

        /// <summary>
        /// 创建 AppSettingsManager 的新实例。
        /// </summary>
        /// <param name="reader">指定配置文件的读取器。</param>
        /// <exception cref="Exception"/>
        public AppSettingsManager(XmlReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }
            using (XmlReader readerF = XmlReader.Create(reader, _readerSettings))
            {
                _document = XDocument.Load(readerF);
                _document = Coerce(_document);
                _properties = GetPropertySet(_document);
                _document.Changed += (s, e) => { OnChanged(); };
            }
        }

        /// <summary>
        /// 释放由 <see cref="AppSettingsManager"/> 使用的所有资源。
        /// </summary>
        ~AppSettingsManager()
        {
            Dispose(false);
        }

        /// <summary>
        /// 释放由 <see cref="AppSettingsManager"/> 使用的所有资源。
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
                    _properties = null;
                    _document = null;
                }
                _disposed = true;
                OnDisposed();
            }
        }

        #endregion Construction

        #region File

        /// <summary>
        /// 获取 "file" 属性的值。
        /// </summary>
        /// <returns></returns>
        public string GetFileAttribute()
        {
            return TryGetFileAttribute(out string file) ? file : null;
        }

        /// <summary>
        /// 设置 "file" 属性的值、添加或删除 "file" 属性。
        /// </summary>
        /// <param name="value">"file" 属性的值。"file" 特性指向一个根节点为 &lt;appSettings&gt; 的配置文件。</param>
        /// <returns></returns>
        public void SetFileAttribute(string value)
        {
            _document.Root.SetAttributeValue("file", value);
        }

        /// <summary>
        /// 获取 "file" 属性的值。
        /// <br/>如果没有找到指定属性，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">"file" 属性的值。</param>
        /// <returns></returns>
        public bool TryGetFileAttribute(out string value)
        {
            if (_document.Root.Attribute("file") is XAttribute attribute)
            {
                value = attribute.Value;
                return true;
            }
            value = null;
            return false;
        }

        #endregion File

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
            using (XmlWriter writer = XmlWriter.Create(filePath, _writerSettings))
            {
                _document.WriteTo(writer);
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
            using (XmlWriter writer = XmlWriter.Create(stream, _writerSettings))
            {
                _document.WriteTo(writer);
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
            _document.WriteTo(writer);
            writer.Flush();
        }

        #endregion Save

        /// <summary>
        /// 清除所有节点，没有被 <see cref="AppSettingsManager"/> 管理的节点和内容也会全部删除。
        /// </summary>
        /// <returns></returns>
        public void Clear()
        {
            _document.Root.RemoveAll();
            _properties = GetPropertySet(_document);
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
        /// 获取应用 file 属性以及 &lt;remove /&gt;、&lt;clear /&gt; 标签后的只读配置属性集合。当配置文件修改时应重新获取。
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1024:在适用处使用属性", Justification = "<挂起>")]
        public DictionaryPropertySetControlled GetPropertySetControlled()
        {
            return new DictionaryPropertySetControlled(_document.Root);
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
            if (document.Root.Name != "appSettings")
            {
                throw new FileLoadException("File is not a appSettings extra file.");
            }
            return document;
        }

        private static DictionaryPropertySet GetPropertySet(XDocument document)
        {
            if (document.Root.Name.LocalName != "appSettings")
            {
                throw new FileLoadException("File is not a appSettings extra file.");
            }
            if (document.Root.Attribute("configProtectionProvider") != null)
            {
                throw new CryptographicException("Encryped configuration sections are not supported.");
            }
            return new DictionaryPropertySet(document.Root);
        }
    }
}