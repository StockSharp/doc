# 标的检索

![屏幕截图: 标的检索面板](../../../../images/gui_securitylookuppanel.png)

[SecurityLookupPanel](xref:StockSharp.Xaml.SecurityLookupPanel) - 用于检索标的的面板。在搜索框中输入代码或其片段，点击附加过滤按钮会打开 [Security](xref:StockSharp.BusinessEntities.Security) 编辑器，在其中设置类型、交易板块、货币和到期日。

面板本身不执行检索：点击检索按钮或按回车时，它会触发携带已填写过滤条件的 `Lookup` 事件。之后是向连接器发送请求还是在本地存储中查找，由应用程序决定。

下面是其使用的代码片段:

```xaml
<Window x:Class="Sample.LookupWindow"
	xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
	xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
	xmlns:xaml="http://schemas.stocksharp.com/xaml"
	Height="500" Width="400">
	<xaml:SecurityLookupPanel x:Name="LookupPanel" />
</Window>
```

```cs
// 把检索请求发送给连接器
LookupPanel.Lookup += filter =>
{
	// 过滤条件已经填写完毕
	_connector.Subscribe(new Subscription(filter.ToLookupMessage()));
};

// 把找到的标的显示在表格中
_connector.SecurityReceived += (subscription, security) =>
	this.GuiAsync(() => SecurityGrid.Securities.Add(security));
```

## 另请参阅

[服务面板](../service_panels.md)
