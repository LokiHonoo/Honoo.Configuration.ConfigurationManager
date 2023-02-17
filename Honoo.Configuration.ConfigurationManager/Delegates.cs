namespace Honoo.Configuration
{
    /// <summary>
    /// 在内容改变时执行。
    /// </summary>
    /// <param name="manager">ConfigurationManager 实例。</param>
    public delegate void OnChangedEventHandler(ConfigurationManager manager);

    /// <summary>
    /// 在 ConfigurationManager 实例正在释放时执行。
    /// </summary>
    /// <param name="manager">ConfigurationManager 实例。</param>
    public delegate void OnDisposingEventHandler(ConfigurationManager manager);
}