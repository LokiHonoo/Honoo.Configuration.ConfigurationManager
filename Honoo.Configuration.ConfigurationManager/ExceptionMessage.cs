namespace Honoo.Configuration
{
    /// <summary>
    /// 错误消息。
    /// </summary>
    public class ExceptionMessage
    {
        /// <summary>
        /// 消息的本地化内容。
        /// </summary>
        public string Message { get; set; }

        #region Constructor

        internal ExceptionMessage(string message)
        {
            Message = message;
        }

        #endregion Constructor
    }
}