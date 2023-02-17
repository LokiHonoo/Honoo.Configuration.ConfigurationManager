using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置管理器。
    /// </summary>
    public sealed class ConfigurationManager : IDisposable
    {
        private readonly XmlWriterSettings _writerSettings = new XmlWriterSettings() { Indent = true, Encoding = new UTF8Encoding(false) };
        private AppSettings _appSettings;
        private ConfigSections _configSections;
        private ConnectionStrings _connectionStrings;
        private bool _disposed;
        private XElement _root;

        /// <summary>
        /// 映射到标准格式的 "appSettings" 节点。
        /// </summary>
        public AppSettings AppSettings
        {
            get
            {
                if (_appSettings is null)
                {
                    _appSettings = new AppSettings(_root);
                }
                return _appSettings;
            }
        }

        /// <summary>
        /// 映射到标准格式的 "configSections" 节点。
        /// </summary>
        public ConfigSections ConfigSections
        {
            get
            {
                if (_configSections is null)
                {
                    _configSections = new ConfigSections(_root);
                }
                return _configSections;
            }
        }

        /// <summary>
        /// 映射到标准格式的 "connectionStrings" 节点。
        /// </summary>
        public ConnectionStrings ConnectionStrings
        {
            get
            {
                if (_connectionStrings is null)
                {
                    _connectionStrings = new ConnectionStrings(_root);
                }
                return _connectionStrings;
            }
        }

        #region Construction

        /// <summary>
        /// 创建 ConfigurationManager 的新实例。
        /// </summary>
        public ConfigurationManager()
        {
            _root = new XElement("configuration");
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
                throw new ArgumentException($"The invalid file path - {nameof(filePath)}.");
            }
            _root = XElement.Load(filePath);
        }

        /// <summary>
        /// 创建 ConfigurationManager 的新实例。
        /// </summary>
        /// <param name="stream">指定配置文件的流。</param>
        /// <param name="closeStream">读取完成后关闭流。</param>
        /// <exception cref="Exception"/>
        public ConfigurationManager(Stream stream, bool closeStream = true)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            _root = XElement.Load(stream);
            if (closeStream)
            {
                stream.Close();
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
                if (disposing)
                {
                    _root = null;
                }
                _disposed = true;
            }
        }

        #endregion Construction

        /// <summary>
        /// 确定指定的对象是否等于当前对象。
        /// </summary>
        /// <param name="obj">要与当前对象进行比较的对象。</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is ConfigurationManager other && _root.Equals(other._root);
        }

        /// <summary>
        /// 作为默认哈希函数。
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return _root.GetHashCode();
        }

        /// <summary>
        /// 保存到指定的字符串。
        /// </summary>
        /// <param name="xmlString">指定保存的配置文件内容的字符串。</param>
        public void Save(out string xmlString)
        {
            StringBuilder builder = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(builder, _writerSettings))
            {
                _root.Save(writer);
                xmlString = builder.ToString();
            }
        }

        /// <summary>
        /// 保存到指定的文件。
        /// </summary>
        /// <param name="filePath">指定配置文件的路径。</param>
        /// <exception cref="Exception"/>
        public void Save(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException($"The invalid file path - {nameof(filePath)}.");
            }
            using (XmlWriter writer = XmlWriter.Create(filePath, _writerSettings))
            {
                _root.Save(writer);
            }
        }

        /// <summary>
        /// 保存到指定的流。
        /// </summary>
        /// <param name="stream">指定配置文件的流。</param>
        /// <exception cref="Exception"/>
        public void Save(Stream stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            using (XmlWriter writer = XmlWriter.Create(stream, _writerSettings))
            {
                _root.Save(writer);
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
    }
}