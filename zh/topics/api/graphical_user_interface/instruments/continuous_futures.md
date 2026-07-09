# 连续期货

[ContinuousSecurityWindow](xref:StockSharp.Xaml.ContinuousSecurityWindow) - 是一个用于创建*连续* ([ContinuousSecurity](xref:StockSharp.Algo.ContinuousSecurity)) 工具的可视化编辑器。请参见 [连续期货](../../instruments/continuous_futures.md)。

![HydraGluingCSCustom](../../../../images/hydragluingcscustom.png)

该组件包括：

- 特殊 [SecurityIdTextBox](xref:StockSharp.Xaml.SecurityIdTextBox) 文本字段，它通过输入 Id \- \[Code\]@\[Board\] 生成一个*连续*的工具。
- [SecurityJumpsEditor](xref:StockSharp.Xaml.SecurityJumpsEditor) 组件是一个用于处理属于*连续*工具的工具的特殊 DataGrid。工具被封装在 [SecurityJump](xref:StockSharp.Xaml.SecurityJump) 类中，该类具有两个属性：[SecurityJump.Security](xref:StockSharp.Xaml.SecurityJump.Security) 和 [SecurityJump.Date](xref:StockSharp.Xaml.SecurityJump.Date)（向前滚动）。添加的工具存储在 [SecurityJumpsEditor.Jumps](xref:StockSharp.Xaml.SecurityJumpsEditor.Jumps) 列表中。该组件具有 [SecurityJumpsEditor.Validate](xref:StockSharp.Xaml.SecurityJumpsEditor.Validate) 函数，用于检查组件工具的正确性。
- 用于添加/删除交易品种的按钮。
- **自动** 按钮允许你自动创建一个*连续*交易品种。
- **确定**按钮完成*连续*交易品种的创建。

**主要属性**

- [ContinuousSecurityWindow.Security](xref:StockSharp.Xaml.ContinuousSecurityWindow.Security) – 连续交易品种
- [ContinuousSecurityWindow.SecurityStorage](xref:StockSharp.Xaml.ContinuousSecurityWindow.SecurityStorage) – 提供关于交易品种信息的供应商。

下面是含有使用示例的代码片段。

```cs
private void CreateContinuousSecurity_OnClick(object sender, RoutedEventArgs e)
{
	_continuousSecurityWindow = new ContinuousSecurityWindow
	{
		SecurityStorage = _entityRegistry.Securities,
		Security = new ContinuousSecurity { Board = ExchangeBoard.Associated }
	};
	if (!_continuousSecurityWindow.ShowModal(this))
		return;
	_continuousSecurity = _continuousSecurityWindow.Security;
	ContinuousSecurity.Content = _continuousSecurity.Id;
	var first = _continuousSecurity.InnerSecurities.First();
	var gluingSecurity = new Security
	{
		Id = _continuousSecurity.Id,
		Code = _continuousSecurity.Code,
		Board = ExchangeBoard.Associated,
		Type = _continuousSecurity.Type,
		VolumeStep = first.VolumeStep,
		PriceStep = first.PriceStep,
		ExtensionInfo = new Dictionary<object, object> { { "GluingSecurity", true } }
	};
	if (_entityRegistry.Securities.ReadById(gluingSecurity.Id) == null)
	{
		_entityRegistry.Securities.Save(gluingSecurity);
	}
}
```

## 推荐内容

[连续期货](../../../hydra/instruments_and_boards/continuous_futures.md)
