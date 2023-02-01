using Honoo.Configuration;
using System;

namespace Middleware
{
    internal static class Common
    {
        public static Random Random { get; } = new Random();

        public static void SetLocalException()
        {
            Localization.InvalidKey = "无效的键。";
            Localization.DuplicateKey = "指定的键已存在。";
            Localization.InvalidType = "无效的类型。";
        }
    }
}