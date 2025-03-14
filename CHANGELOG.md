# Honoo.Configuration.ConfigurationManager

<!-- @import "[TOC]" {cmd="toc" depthFrom=1 depthTo=6 orderedList=false} -->

<!-- code_chunk_output -->

- [Honoo.Configuration.ConfigurationManager](#honooconfigurationconfigurationmanager)
  - [CHANGELOG](#changelog)
    - [1.5.9](#159)
    - [1.5.7](#157)
    - [1.5.6](#156)
    - [1.5.4](#154)
    - [1.4.19-final](#1419-final)
    - [1.4.18](#1418)
    - [1.4.17](#1417)
    - [1.4.16](#1416)
    - [1.4.11](#1411)
    - [1.4.10](#1410)
    - [1.3.4](#134)
    - [1.3.2](#132)
    - [1.3.1](#131)
    - [1.2.5](#125)
    - [1.2.3](#123)
    - [1.2.0](#120)

<!-- /code_chunk_output -->

## CHANGELOG

### 1.5.9

**Features* 属性值转换 Byte[] 时可移除分隔符。

### 1.5.7

**Features* 由 XConfigManager 管理的 XProperty 类型可以增加自定义附加属性。

**Changed* 移除了部分方法，以使 IDE 中的智能提示指向更加明确。

**Changed* 更改加密选项。

### 1.5.6

**Fix* 修复 GetXmlString() 方法没有刷新写入器的 BUG。

### 1.5.4

**Features* 提供针对字符串的方法的重载。

**Features* 重写 XConfigManager。取消数组支持，更改为类型嵌套。支持 Dictionary、List 类型无限嵌套。

**Fix* 修复 BUG。

### 1.4.19-final

**Features* 添加方法。

### 1.4.18

**Fix* 修复 BUG。

### 1.4.17

**Features* 注释操作提取到单独的 XConfigComment 类中。

### 1.4.16

**Features* 新增 Binaries 类型用于封装 byte[]。避免过多的 [] 符号造成的视觉混乱。

### 1.4.11

**Features* XConfigManager 增加 2 维数组和 3 维数组支持。

### 1.4.10

**Refactored* 完全重构。现在能以标签方式处理混乱的 &lt;remove /&gt; &lt;clear /&gt; 标签。所有属性封装为类型并将注释（comment）的设置移动到属性封装中。

**Features* 提供 AppSettings.GetControlledProperties() 用于获取应用 file 属性以及 &lt;remove /&gt;、&lt;clear /&gt; 标签后的只读配置属性集合。

**Features* 提供 DictionarySection.GetControlledProperties() 用于获取应用 file 属性以及 &lt;remove /&gt;、&lt;clear /&gt; 标签后的只读配置属性集合。

**Features* 提供 NameValueSection.GetControlledProperties() 用于获取应用 &lt;remove /&gt;、&lt;clear /&gt; 标签后的只读配置属性集合。

**Features* 提供 AppSettingsManager.GetControlledProperties() 用于获取应用 file 属性以及 &lt;remove /&gt;、&lt;clear /&gt; 标签后的只读配置属性集合。

**Features* 提供 AppSettingsManager 用于读写 appSettings 节点的 file 属性指定的附加配置文件。

**Features* 提供一个额外的精简的配置属性文件，以字典类型保存，支持分组，支持加密，支持单一属性值和数组属性值。使用 XConfigManager 类读写此文件。

### 1.3.4

**Fixed* 修复了 AssemblyBinding 节点缺少命名空间的问题。

**Fixed* 修复了 Section 标签类型错误的问题。

**Changed* 移除了 IConfigSection 接口方式，更改为 ConfigSection 基类。

### 1.3.2

**Features* 提供了一个额外的加密方式加密整个配置文件。这和 ASP.NET 的默认加密方式无关，生成的加密配置文件仅可使用此项目工具读写。

**Features* 新增 &lt;assemblyBinding /&gt; 节点支持。

**Features* 读取 &lt;remove /&gt; &lt;clear /&gt; 标签时采用标签的默认行为。标签产生的删除行为会体现在配置文件中。例如 &lt;clear /&gt; 标签之前的 所有标签都会被移除。

### 1.3.1

**Changed* SectionGroup 和 Section 的注释（comment）设置从父级方法更改为自身方法。

**Changed* 移除了 ConfigSection 基类，更改为 IConfigSection 接口方式。

**Features* AppSettings、DictionarySection 读取同名键值不再抛出异常，使用最后读取的值。

**Features* NameValueSection 支持数组模式。

**Features* 属性值支持枚举类型。

**Features* 新增 GetValue(string, string) 方法，在取值时设置没有找到指定键时的默认值。

### 1.2.5

**Features* TextSection 以 xml 方式处理。现在 TextSection 支持解析 CDATA 区段。

### 1.2.3

**Changed* 移除了访问 ConnectionStrings 直接创建和访问实例的代码。提供 CreateInstance() 方法主动创建实例。如果没有引用相关的数据库程序集，在主动创建实例前不会抛出异常。

### 1.2.0

**Changed* 移除了原有的自动保存的代码，增加了事件，可在 OnChanged，OnDisposing 事件中实现自动保存。

**Features* 现在支持读写注释（comment）节点。
