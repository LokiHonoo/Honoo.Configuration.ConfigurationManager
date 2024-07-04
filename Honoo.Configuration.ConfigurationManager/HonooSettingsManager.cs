using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 简单配置属性管理器。提供一个精简的配置属性文件，以字典类型保存，支持加密，支持单一属性值和数组属性值。
    /// </summary>
    public sealed class HonooSettingsManager : IDisposable
    {
        #region Properties

        private static readonly XNamespace _namespace = "https://github.com/LokiHonoo/Honoo.Configuration.ConfigurationManager/";
        private static readonly XmlWriterSettings _writerSettings = new XmlWriterSettings() { Indent = true, Encoding = new UTF8Encoding(false) };
        private bool _disposed;
        private HonooPropertySet _properties;
        private XElement _root;

        /// <summary>
        /// 获取配置属性集合。
        /// </summary>
        public HonooPropertySet Properties => _properties;

        #endregion Properties

        #region Delegate

        /// <summary>
        /// 在 HonooSettingsManager 实例内容改变时执行。
        /// </summary>
        /// <param name="manager">HonooSettingsManager 实例。</param>
        public delegate void OnChangedEventHandler(HonooSettingsManager manager);

        /// <summary>
        /// 在 HonooSettingsManager 实例释放后执行。
        /// </summary>
        public delegate void OnDisposedEventHandler();

        /// <summary>
        /// 在 HonooSettingsManager 实例正在释放时执行。
        /// </summary>
        /// <param name="manager">HonooSettingsManager 实例。</param>
        public delegate void OnDisposingEventHandler(HonooSettingsManager manager);

        #endregion Delegate

        #region Event

        /// <summary>
        /// 在 HonooSettingsManager 实例内容改变时执行。
        /// </summary>
        public event OnChangedEventHandler OnChanged;

        /// <summary>
        /// 在 HonooSettingsManager 实例释放后执行。
        /// </summary>
        public event OnDisposedEventHandler OnDisposed;

        /// <summary>
        /// 在 HonooSettingsManager 实例准备释放时执行。
        /// </summary>
        public event OnDisposingEventHandler OnDisposing;

        #endregion Event

        #region Construction

        /// <summary>
        /// 创建 HonooSettingsManager 的新实例。
        /// </summary>
        public HonooSettingsManager()
        {
            _root = new XElement(_namespace + "settings");
            _root.Changed += OnContentChanged;
            _properties = GetPropertySet(_root);
        }

        /// <summary>
        /// 创建 HonooSettingsManager 的新实例。
        /// </summary>
        /// <param name="filePath">指定配置文件的路径。</param>
        /// <param name="protectionAlgorithm">
        /// 指定一个非对称加密算法，用于读取加密配置文件。
        /// <br/>这和 ASP.NET 的默认加密方式无关，生成的加密配置文件仅可使用此项目工具读写。
        /// <br/>算法必须拥有私钥。
        /// </param>
        /// <exception cref="Exception"/>
        public HonooSettingsManager(string filePath, RSACryptoServiceProvider protectionAlgorithm = null)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException($"The invalid argument - {nameof(filePath)}.");
            }
            _root = XElement.Load(filePath);
            _root = Coerce(_root, protectionAlgorithm);
            _root.Changed += OnContentChanged;
            _properties = GetPropertySet(_root);
        }

        /// <summary>
        /// 创建 HonooSettingsManager 的新实例。
        /// </summary>
        /// <param name="stream">指定配置文件的流。</param>
        /// <param name="closeStream">读取完成后关闭流。</param>
        /// <param name="protectionAlgorithm">
        /// 指定一个非对称加密算法，用于读取加密配置文件。
        /// <br/>这和 ASP.NET 的默认加密方式无关，生成的加密配置文件仅可使用此项目工具读写。
        /// <br/>算法必须拥有私钥。
        /// </param>
        /// <exception cref="Exception"/>
        public HonooSettingsManager(Stream stream, bool closeStream = true, RSACryptoServiceProvider protectionAlgorithm = null)
        {
            _root = XElement.Load(stream);
            if (closeStream)
            {
                stream.Close();
            }
            _root = Coerce(_root, protectionAlgorithm);
            _root.Changed += OnContentChanged;
            _properties = GetPropertySet(_root);
        }

        /// <summary>
        /// 创建 HonooSettingsManager 的新实例。
        /// </summary>
        /// <param name="reader">指定配置文件的读取器。</param>
        /// <param name="closeReader">读取完成后关闭读取器。</param>
        /// <param name="protectionAlgorithm">
        /// 指定一个非对称加密算法，用于读取加密配置文件。
        /// <br/>这和 ASP.NET 的默认加密方式无关，生成的加密配置文件仅可使用此项目工具读写。
        /// <br/>算法必须拥有私钥。
        /// </param>
        /// <exception cref="Exception"/>
        public HonooSettingsManager(XmlReader reader, bool closeReader = true, RSACryptoServiceProvider protectionAlgorithm = null)
        {
            _root = XElement.Load(reader);
            if (closeReader)
            {
                reader.Close();
            }
            _root = Coerce(_root, protectionAlgorithm);
            _root.Changed += OnContentChanged;
        }

        /// <summary>
        /// 释放由 <see cref="HonooSettingsManager"/> 使用的所有资源。
        /// </summary>
        ~HonooSettingsManager()
        {
            Dispose(false);
        }

        /// <summary>
        /// 释放由 <see cref="HonooSettingsManager"/> 使用的所有资源。
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
                    _properties = null;
                    _root = null;
                }
                _disposed = true;
                OnDisposed?.Invoke();
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
        public void Save(string filePath, RSACryptoServiceProvider protectionAlgorithm = null)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException($"The invalid argument - {nameof(filePath)}.");
            }
            XElement root = _root;
            if (protectionAlgorithm != null)
            {
                root = ProtectionHelper.Encrypt(root, protectionAlgorithm);
            }
            using (XmlWriter writer = XmlWriter.Create(filePath, _writerSettings))
            {
                root.WriteTo(writer);
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
            XElement root = _root;
            if (protectionAlgorithm != null)
            {
                root = ProtectionHelper.Encrypt(root, protectionAlgorithm);
            }
            using (XmlWriter writer = XmlWriter.Create(stream, _writerSettings))
            {
                root.WriteTo(writer);
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
        public void Save(XmlWriter writer, RSACryptoServiceProvider protectionAlgorithm = null)
        {
            XElement root = _root;
            if (protectionAlgorithm != null)
            {
                root = ProtectionHelper.Encrypt(root, protectionAlgorithm);
            }
            root.WriteTo(writer);
        }

        #endregion Save

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
            XElement root = _root;
            if (protectionAlgorithm != null)
            {
                root = ProtectionHelper.Encrypt(root, protectionAlgorithm);
            }
            StringBuilder builder = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(builder, _writerSettings))
            {
                root.WriteTo(writer);
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

        private static XElement Coerce(XElement root, RSACryptoServiceProvider protectionAlgorithm)
        {
            if (root.Name != _namespace + "settings")
            {
                throw new Exception("File is not a settings(https://github.com/LokiHonoo/Honoo.Configuration.ConfigurationManager/) file.");
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

        private static HonooPropertySet GetPropertySet(XElement root)
        {
            if (root.Name != _namespace + "settings")
            {
                throw new Exception("File is not a settings(https://github.com/LokiHonoo/Honoo.Configuration.ConfigurationManager/) file.");
            }
            return new HonooPropertySet(root);
        }

        private void OnContentChanged(object sender, XObjectChangeEventArgs e)
        {
            OnChanged?.Invoke(this);
        }
    }
}