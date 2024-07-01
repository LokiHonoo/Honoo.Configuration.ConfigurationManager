using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置管理器。提供对标准节点 appSettings、connectionStrings、configSections 的有限读写支持。
    /// </summary>
    public sealed class ConfigurationManager : IDisposable
    {
        #region Properties

        private static readonly XmlWriterSettings _writerSettings = new XmlWriterSettings() { Indent = true, Encoding = new UTF8Encoding(false) };
        private AppSettings _appSettings;
        private AssemblyBinding _assemblyBinding;
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
                if (_appSettings == null)
                {
                    _appSettings = new AppSettings(_root);
                }
                return _appSettings;
            }
        }

        /// <summary>
        /// 映射到标准格式的 "assemblyBinding" 节点。这是配置级的程序集绑定策略节点。
        /// </summary>
        public AssemblyBinding AssemblyBinding
        {
            get
            {
                if (_assemblyBinding == null)
                {
                    _assemblyBinding = new AssemblyBinding(_root);
                }
                return _assemblyBinding;
            }
        }

        /// <summary>
        /// 映射到标准格式的 "configSections" 节点。
        /// </summary>
        public ConfigSections ConfigSections
        {
            get
            {
                if (_configSections == null)
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
                if (_connectionStrings == null)
                {
                    _connectionStrings = new ConnectionStrings(_root);
                }
                return _connectionStrings;
            }
        }

        #endregion Properties

        #region Delegate

        /// <summary>
        /// 在 ConfigurationManager 实例内容改变时执行。
        /// </summary>
        /// <param name="manager">ConfigurationManager 实例。</param>
        public delegate void OnChangedEventHandler(ConfigurationManager manager);

        /// <summary>
        /// 在 ConfigurationManager 实例释放后执行。
        /// </summary>
        public delegate void OnDisposedEventHandler();

        /// <summary>
        /// 在 ConfigurationManager 实例正在释放时执行。
        /// </summary>
        /// <param name="manager">ConfigurationManager 实例。</param>
        public delegate void OnDisposingEventHandler(ConfigurationManager manager);

        #endregion Delegate

        #region Event

        /// <summary>
        /// 在 ConfigurationManager 实例内容改变时执行。
        /// </summary>
        public event OnChangedEventHandler OnChanged;

        /// <summary>
        /// 在 ConfigurationManager 实例释放后执行。
        /// </summary>
        public event OnDisposedEventHandler OnDisposed;

        /// <summary>
        /// 在 ConfigurationManager 实例准备释放时执行。
        /// </summary>
        public event OnDisposingEventHandler OnDisposing;

        #endregion Event

        #region Construction

        /// <summary>
        /// 创建 ConfigurationManager 的新实例。
        /// </summary>
        public ConfigurationManager()
        {
            _root = new XElement("configuration");
            _root.Changed += OnContentChanged;
        }

        /// <summary>
        /// 创建 ConfigurationManager 的新实例。
        /// </summary>
        /// <param name="filePath">指定配置文件的路径。如果配置文件不存在，创建新的空白配置（保存前不会创建磁盘文件）。</param>
        /// <param name="protectionAlgorithm">
        /// 指定一个非对称加密算法，用于读取加密配置文件。
        /// <br/>这和 ASP.NET 的默认加密方式无关，生成的加密配置文件仅可使用此项目工具读写。
        /// <br/>算法必须拥有私钥。
        /// </param>
        /// <exception cref="Exception"/>
        public ConfigurationManager(string filePath, RSACryptoServiceProvider protectionAlgorithm = null)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException($"The invalid argument - {nameof(filePath)}.");
            }
            if (File.Exists(filePath))
            {
                _root = XElement.Load(filePath);
            }
            else
            {
                _root = new XElement("configuration");
            }
            _root = Coerce(_root, protectionAlgorithm);
            _root.Changed += OnContentChanged;
        }

        /// <summary>
        /// 创建 ConfigurationManager 的新实例。
        /// </summary>
        /// <param name="stream">指定配置文件的流。</param>
        /// <param name="closeStream">读取完成后关闭流。</param>
        /// <param name="protectionAlgorithm">
        /// 指定一个非对称加密算法，用于读取加密配置文件。
        /// <br/>这和 ASP.NET 的默认加密方式无关，生成的加密配置文件仅可使用此项目工具读写。
        /// <br/>算法必须拥有私钥。
        /// </param>
        /// <exception cref="Exception"/>
        public ConfigurationManager(Stream stream, bool closeStream = true, RSACryptoServiceProvider protectionAlgorithm = null)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            _root = XElement.Load(stream);
            if (closeStream)
            {
                stream.Close();
            }
            _root = Coerce(_root, protectionAlgorithm);
            _root.Changed += OnContentChanged;
        }

        /// <summary>
        /// 创建 ConfigurationManager 的新实例。
        /// </summary>
        /// <param name="reader">指定配置文件的读取器。</param>
        /// <param name="closeReader">读取完成后关闭读取器。</param>
        /// <param name="protectionAlgorithm">
        /// 指定一个非对称加密算法，用于读取加密配置文件。
        /// <br/>这和 ASP.NET 的默认加密方式无关，生成的加密配置文件仅可使用此项目工具读写。
        /// <br/>算法必须拥有私钥。
        /// </param>
        /// <exception cref="Exception"/>
        public ConfigurationManager(XmlReader reader, bool closeReader = true, RSACryptoServiceProvider protectionAlgorithm = null)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }
            _root = XElement.Load(reader);
            if (closeReader)
            {
                reader.Close();
            }
            _root = Coerce(_root, protectionAlgorithm);
            _root.Changed += OnContentChanged;
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
                OnDisposing?.Invoke(this);
                if (disposing)
                {
                    _appSettings = null;
                    _configSections = null;
                    _connectionStrings = null;
                    _root = null;
                }
                _disposed = true;
                OnDisposed?.Invoke();
            }
        }

        #endregion Construction

        /// <summary>
        /// 返回配置文件的缩进 XML 文档文本。
        /// </summary>
        /// <param name="protectionAlgorithm">
        /// 指定一个非对称加密算法，用于保存加密配置文件。
        /// <br/>这和 ASP.NET 的默认加密方式无关，生成的加密配置文件仅可使用此项目工具读写。
        /// <br/>算法可以是公钥或私钥。
        /// </param>
        /// <exception cref="Exception"/>
        public string GetXmlString(RSACryptoServiceProvider protectionAlgorithm = null)
        {
            XElement root = Clean(_root);
            if (protectionAlgorithm != null)
            {
                root = ProtectionHelper.Encrypt(root, protectionAlgorithm);
            }
            StringBuilder builder = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(builder, _writerSettings))
            {
                root.Save(writer);
                return builder.ToString();
            }
        }

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
        public void Save(string filePath, RSACryptoServiceProvider protectionAlgorithm = null)
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
                root.Save(writer);
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
        public void Save(Stream stream, RSACryptoServiceProvider protectionAlgorithm = null)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            XElement root = Clean(_root);
            if (protectionAlgorithm != null)
            {
                root = ProtectionHelper.Encrypt(root, protectionAlgorithm);
            }
            using (XmlWriter writer = XmlWriter.Create(stream, _writerSettings))
            {
                root.Save(writer);
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
        public void Save(TextWriter writer, RSACryptoServiceProvider protectionAlgorithm = null)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }
            XElement root = Clean(_root);
            if (protectionAlgorithm != null)
            {
                root = ProtectionHelper.Encrypt(root, protectionAlgorithm);
            }
            root.Save(writer);
        }

        /// <summary>
        /// 方法已重写。返回根节点的缩进 XML 文本。不包括文档声明。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _root.ToString();
        }

        private XElement Clean(XElement root)
        {
            XNamespace ns = "urn:schemas-microsoft-com:asm.v1";
            XElement result = XElement.Parse(root.ToString());
            if (_appSettings != null && _appSettings.Properties.Count == 0)
            {
                XElement element = result.Element("appSettings");
                if (!element.HasElements)
                {
                    element.Remove();
                }
            }
            if (_assemblyBinding != null && _assemblyBinding.Properties.Count == 0)
            {
                result.Element(ns + "assemblyBinding").Remove();
            }
            if (_connectionStrings != null && _connectionStrings.Properties.Count == 0)
            {
                result.Element("connectionStrings").Remove();
            }
            if (_configSections != null && _configSections.Sections.Count == 0 && _configSections.Groups.Count == 0)
            {
                result.Element("configSections").Remove();
            }
            return result;
        }

        private XElement Coerce(XElement root, RSACryptoServiceProvider protectionAlgorithm)
        {
            if (root.Name != "configuration")
            {
                throw new ArgumentException($"Not a configuration file.");
            }
            if (protectionAlgorithm != null)
            {
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
            }
            return root;
        }

        private void OnContentChanged(object sender, XObjectChangeEventArgs e)
        {
            OnChanged?.Invoke(this);
        }
    }
}