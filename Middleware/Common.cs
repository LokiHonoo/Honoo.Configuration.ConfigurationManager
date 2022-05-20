using Honoo.Configuration;
using System;

namespace Middleware
{
    public static class Common
    {
        public static Random Random = new Random();

        public static void SetLocalException()
        {
            ExceptionMessages.InvalidKey.Message = "无效的键。";
            ExceptionMessages.DuplicateKey.Message = "指定的键已存在。";
            ExceptionMessages.InvalidType.Message = "无效的类型。";
        }
    }
}