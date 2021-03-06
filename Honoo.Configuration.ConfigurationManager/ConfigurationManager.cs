using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置管理器。
    /// </summary>
    public sealed class ConfigurationManager : IDisposable, ISavable
    {
        private readonly string _filePath;
        private readonly XmlWriterSettings _writerSettings = new XmlWriterSettings() { Indent = true, Encoding = new UTF8Encoding(false) };
        private AppSettings _appSettings;
        private bool _autoSave;
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
                    _appSettings = new AppSettings(_root, this);
                }
                return _appSettings;
            }
        }

        /// <summary>
        /// 获取或设置自动保存选项。如果在创建 <see cref="ConfigurationManager"/> 实例时没有指定文件路径，此选项无效。默认值是 false。
        /// </summary>
        public bool AutoSave { get => _autoSave; set => _autoSave = value; }

        /// <summary>
        /// 映射到标准格式的 "configSections" 节点。
        /// </summary>
        public ConfigSections ConfigSections
        {
            get
            {
                if (_configSections is null)
                {
                    _configSections = new ConfigSections(_root, this);
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
                    _connectionStrings = new ConnectionStrings(_root, this);
                }
                return _connectionStrings;
            }
        }

        /// <summary>
        /// 获取映射的配置文件的路径。
        /// </summary>
        public string FilePath => _filePath;

        #region Constructor

        /// <summary>
        /// 创建 ConfigurationManager 的新实例。
        /// </summary>
        public ConfigurationManager()
        {
            _root = new XElement("configuration");
            _filePath = string.Empty;
        }

        /// <summary>
        /// 创建 ConfigurationManager 的新实例。
        /// </summary>
        /// <param name="stream">指定配置文件的流。</param>
        public ConfigurationManager(Stream stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            if (stream.Length > 0)
            {
                stream.Seek(0, SeekOrigin.Begin);
                _root = XElement.Load(stream);
            }
            else
            {
                _root = new XElement("configuration");
            }
            _filePath = string.Empty;
        }

        /// <summary>
        /// 创建 ConfigurationManager 的新实例。
        /// </summary>
        /// <param name="filePath">指定配置文件的路径。</param>
        public ConfigurationManager(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }
            FileInfo file = new FileInfo(filePath);
            if (file.Exists && file.Length > 0)
            {
                _root = XElement.Load(filePath);
            }
            else
            {
                _root = new XElement("configuration");
            }
            _filePath = filePath.Trim();
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

        #endregion Constructor

        /// <summary>
        /// 创建映射到默认配置文件的 <see cref="ConfigurationManager"/> 实例。文件路径形如 Directory\program.exe.config。
        /// </summary>
        public static ConfigurationManager CreateAppConfigManager()
        {
            return new ConfigurationManager(Assembly.GetEntryAssembly().Location + ".config");
        }

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
        /// 保存到创建 <see cref="ConfigurationManager"/> 实例时指定的文件。
        /// </summary>
        public void Save()
        {
            if (!string.IsNullOrWhiteSpace(_filePath))
            {
                using (XmlWriter writer = XmlWriter.Create(_filePath, _writerSettings))
                {
                    _root.Save(writer);
                }
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
            stream.SetLength(0);
            using (XmlWriter writer = XmlWriter.Create(stream, _writerSettings))
            {
                _root.Save(writer);
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
                throw new ArgumentNullException(nameof(filePath));
            }
            using (XmlWriter writer = XmlWriter.Create(filePath, _writerSettings))
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