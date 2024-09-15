using System;

namespace Honoo.Configuration
{
    /// <summary>
    /// 数组 byte[] 封装。
    /// </summary>
    public sealed class Binaries
    {
#if NETSTANDARD2_0
        private static readonly Binaries _empty = new Binaries(Array.Empty<byte>());
#else
        private static readonly Binaries _empty = new Binaries(new byte[0]);
#endif
        private readonly byte[] _bytes;
        private readonly string _hex;

        /// <summary>
        /// 空值。
        /// </summary>
        public static Binaries Empty => _empty;

        /// <summary>
        /// 获取数组值。
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:属性不应返回数组", Justification = "<挂起>")]
        public byte[] Bytes => (byte[])_bytes.Clone();

        /// <summary>
        /// 获取十六进制字符串。
        /// </summary>
        public string Hex => _hex;

        #region Construction

        /// <summary>
        /// 创建 Binaries 的新实例。
        /// </summary>
        /// <param name="bytes">数组类型。</param>
        public Binaries(byte[] bytes)
        {
            if (bytes is null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }
            _hex = BitConverter.ToString(bytes, 0).Replace("-", string.Empty);
            _bytes = (byte[])bytes.Clone();
        }

        /// <summary>
        /// 创建 Binaries 的新实例。
        /// </summary>
        /// <param name="hex">十六进制字符串类型。</param>
        public Binaries(string hex)
        {
            _hex = hex;
            _bytes = XValueHelper.Parse(hex);
        }

        #endregion Construction

        /// <summary>
        /// 方法已重写。返回实例的十六进制字符串。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _hex;
        }
    }
}