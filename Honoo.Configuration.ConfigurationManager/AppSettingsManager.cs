﻿using System;
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

        private static readonly XmlWriterSettings _writerSettings = new XmlWriterSettings() { Indent = true, Encoding = new UTF8Encoding(false) };
        private bool _disposed;
        private DictionaryPropertySet _properties;
        private XElement _root;

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
            _root = new XElement("appSettings");
            _properties = GetPropertySet(_root);
            _root.Changed += (s, e) => { OnChanged(); };
        }

        /// <summary>
        /// 创建 AppSettingsManager 的新实例。
        /// </summary>
        /// <param name="filePath">指定配置文件的路径。</param>
        /// <param name="protectionAlgorithm">
        /// 指定一个非对称加密算法，用于读取加密配置文件。
        /// <br/>这和 ASP.NET 的默认加密方式无关，生成的加密配置文件仅可使用此项目工具读写。
        /// <br/>算法必须拥有私钥。
        /// </param>
        /// <exception cref="Exception"/>
        public AppSettingsManager(string filePath, RSA protectionAlgorithm = null)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException($"The invalid argument - {nameof(filePath)}.");
            }
            _root = XElement.Load(filePath);
            _root = Coerce(_root, protectionAlgorithm);
            _properties = GetPropertySet(_root);
            _root.Changed += (s, e) => { OnChanged(); };
        }

        /// <summary>
        /// 创建 AppSettingsManager 的新实例。
        /// </summary>
        /// <param name="filePath">指定配置文件的路径。</param>
        /// <param name="createNewIfFileNotExists">如果文件不存在，创建无内容的 XConfigManager 实例。此时不会写入到文件路径。</param>
        /// <param name="protectionAlgorithm">
        /// 指定一个非对称加密算法，用于读取加密配置文件。
        /// <br/>这和 ASP.NET 的默认加密方式无关，生成的加密配置文件仅可使用此项目工具读写。
        /// <br/>算法必须拥有私钥。
        /// </param>
        /// <exception cref="Exception"/>
        public AppSettingsManager(string filePath, bool createNewIfFileNotExists, RSA protectionAlgorithm = null)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException($"The invalid argument - {nameof(filePath)}.");
            }
            if (File.Exists(filePath))
            {
                _root = XElement.Load(filePath);
                _root = Coerce(_root, protectionAlgorithm);
                _properties = GetPropertySet(_root);
            }
            else if (createNewIfFileNotExists)
            {
                _root = new XElement("appSettings");
                _properties = GetPropertySet(_root);
            }
            else
            {
                throw new FileNotFoundException($"The file \"{filePath}\" is not found.");
            }
            _root.Changed += (s, e) => { OnChanged(); };
        }

        /// <summary>
        /// 创建 AppSettingsManager 的新实例。
        /// </summary>
        /// <param name="stream">指定配置文件的流。</param>
        /// <param name="closeStream">读取完成后关闭流。</param>
        /// <param name="protectionAlgorithm">
        /// 指定一个非对称加密算法，用于读取加密配置文件。
        /// <br/>这和 ASP.NET 的默认加密方式无关，生成的加密配置文件仅可使用此项目工具读写。
        /// <br/>算法必须拥有私钥。
        /// </param>
        /// <exception cref="Exception"/>
        public AppSettingsManager(Stream stream, bool closeStream = true, RSA protectionAlgorithm = null)
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
            _properties = GetPropertySet(_root);
            _root.Changed += (s, e) => { OnChanged(); };
        }

        /// <summary>
        /// 创建 AppSettingsManager 的新实例。
        /// </summary>
        /// <param name="reader">指定配置文件的读取器。</param>
        /// <param name="closeReader">读取完成后关闭读取器。</param>
        /// <param name="protectionAlgorithm">
        /// 指定一个非对称加密算法，用于读取加密配置文件。
        /// <br/>这和 ASP.NET 的默认加密方式无关，生成的加密配置文件仅可使用此项目工具读写。
        /// <br/>算法必须拥有私钥。
        /// </param>
        /// <exception cref="Exception"/>
        public AppSettingsManager(XmlReader reader, bool closeReader = true, RSA protectionAlgorithm = null)
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
            _properties = GetPropertySet(_root);
            _root.Changed += (s, e) => { OnChanged(); };
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
                    _root = null;
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
            _root.SetAttributeValue("file", value);
        }

        /// <summary>
        /// 获取 "file" 属性的值。
        /// <br/>如果没有找到指定属性，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">"file" 属性的值。</param>
        /// <returns></returns>
        public bool TryGetFileAttribute(out string value)
        {
            if (_root.Attribute("file") is XAttribute attribute)
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
            XElement root = _root;
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
            XElement root = _root;
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
        /// 保存到指定的写入器。
        /// </summary>
        /// <param name="writer">指定配置文件的写入器。</param>
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
            XElement root = _root;
            if (protectionAlgorithm != null)
            {
                root = ProtectionHelper.Encrypt(root, protectionAlgorithm);
            }
            root.WriteTo(writer);
            writer.Flush();
        }

        #endregion Save

        /// <summary>
        /// 清除所有节点，没有被 <see cref="AppSettingsManager"/> 管理的节点和内容也会全部删除。
        /// </summary>
        /// <returns></returns>
        public void Clear()
        {
            _root.RemoveAll();
            _properties = GetPropertySet(_root);
        }

        /// <summary>
        /// 获取应用 file 属性以及 &lt;remove /&gt;、&lt;clear /&gt; 标签后的只读配置属性集合。当配置文件修改时应重新获取。
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1024:在适用处使用属性", Justification = "<挂起>")]
        public DictionaryPropertySetControlled GetPropertySetControlled()
        {
            return new DictionaryPropertySetControlled(_root);
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
            if (root.Name != "appSettings")
            {
                throw new FileLoadException("File is not a appSettings extra file.");
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

        private static DictionaryPropertySet GetPropertySet(XElement root)
        {
            if (root.Name.LocalName != "appSettings")
            {
                throw new FileLoadException("File is not a appSettings extra file.");
            }
            if (root.Attribute("configProtectionProvider") != null)
            {
                throw new CryptographicException("Encryped configuration sections are not supported.");
            }
            return new DictionaryPropertySet(root);
        }
    }
}