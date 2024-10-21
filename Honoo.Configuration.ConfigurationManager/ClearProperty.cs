using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置属性。
    /// </summary>
    public sealed class ClearProperty : TagProperty, IEquatable<ClearProperty>, IComparer<ClearProperty>, IComparable
    {
        #region Construction

        /// <summary>
        /// 创建 ClearProperty 的新实例。
        /// </summary>
        public ClearProperty() : base(TagPropertyKind.ClearProperty, new XElement("clear"), null)
        {
        }

        internal ClearProperty(XElement content, XComment comment) : base(TagPropertyKind.ClearProperty, content, comment)
        {
        }

        #endregion Construction

        /// <summary>
        /// 比较两个对象并返回一个值。该值指示一个对象是小于、等于还是大于另一个对象。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第二个对象。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public static int Compare(ClearProperty x, ClearProperty y)
        {
            if (x is null)
            {
                throw new ArgumentNullException(nameof(x));
            }
            if (y is null)
            {
                throw new ArgumentNullException(nameof(y));
            }
            return 0;
        }

        /// <summary>
        /// 比较。
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(ClearProperty left, ClearProperty right)
        {
            return !(left == right);
        }

        /// <summary>
        /// 比较。
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <(ClearProperty left, ClearProperty right)
        {
            return (Compare(left, right) < 0);
        }

        /// <summary>
        /// 比较。
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <=(ClearProperty left, ClearProperty right)
        {
            return (Compare(left, right) <= 0);
        }

        /// <summary>
        /// 比较。
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(ClearProperty left, ClearProperty right)
        {
            if (left is null)
            {
                return right is null;
            }
            return left.Equals(right);
        }

        /// <summary>
        /// 比较。
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >(ClearProperty left, ClearProperty right)
        {
            return (Compare(left, right) > 0);
        }

        /// <summary>
        /// 比较。
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >=(ClearProperty left, ClearProperty right)
        {
            return (Compare(left, right) >= 0);
        }

        /// <summary>
        /// 比较两个对象并返回一个值。该值指示一个对象是小于、等于还是大于另一个对象。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第二个对象。</param>
        /// <returns></returns>
        int IComparer<ClearProperty>.Compare(ClearProperty x, ClearProperty y)
        {
            return 0;
        }

        /// <summary>
        /// 将当前实例与另一个对象比较并返回一个值。该值指示当前实例在排序位置是小于、等于还是大于另一个对象。
        /// </summary>
        /// <param name="obj">要比较的对象。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public int CompareTo(object obj)
        {
            if (obj is ClearProperty other)
            {
                return CompareTo(other);
            }
            throw new ArgumentException($"{nameof(obj)} is not a ClearProperty.");
        }

        /// <summary>
        /// 将当前实例与另一个对象比较并返回一个值。该值指示当前实例在排序位置是小于、等于还是大于另一个对象。
        /// </summary>
        /// <param name="other">要比较的对象。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
        public int CompareTo(ClearProperty other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }
            return 0;
        }

        /// <summary>
        /// 确定此实例和指定的对象具有相同的值。
        /// </summary>
        /// <param name="other">比较的对象。</param>
        /// <returns></returns>
        public bool Equals(ClearProperty other)
        {
            return true;
        }

        /// <summary>
        /// 确定指定的对象是否等于当前对象。
        /// </summary>
        /// <param name="obj">比较的对象。</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is ClearProperty;
        }

        /// <summary>
        /// 方法已重写。获取字符串数据值的哈希代码。
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return -374766917;
        }
    }
}