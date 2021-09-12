# Непрерывный фьючерс

[ContinuousSecurityWindow](xref:StockSharp.Xaml.ContinuousSecurityWindow) \- визуальный редактор для создания *непрерывных* ([ContinuousSecurity](xref:StockSharp.Algo.ContinuousSecurity)) инструментов. См. [Непрерывный фьючерс](SecurityContinuous.md). 

![HydraGluingCSCustom](../images/HydraGluingCSCustom.png)

Этот компонент включает: 

- Специальное текстовое поле [SecurityIdTextBox](xref:StockSharp.Xaml.SecurityIdTextBox) \- генерирует *непрерывный* инструмент при помощи введенного Id \- \[Code\]@\[Board\]. 
- Компонент [SecurityJumpsEditor](xref:StockSharp.Xaml.SecurityJumpsEditor) \- специальный DataGrid для работы с инструментами, входящими в состав *непрерывного* инструмента. Составляющие инструменты "обертываются" в класс [SecurityJump](xref:StockSharp.Xaml.SecurityJump), который имеет два свойства: [SecurityJump.Security](xref:StockSharp.Xaml.SecurityJump.Security) и [SecurityJump.Date](xref:StockSharp.Xaml.SecurityJump.Date) (дата роллирования). Добавленные инструменты хранятся в списке [SecurityJumpsEditor.Jumps](xref:StockSharp.Xaml.SecurityJumpsEditor.Jumps). Компонент имеет функцию [SecurityJumpsEditor.Validate](xref:StockSharp.Xaml.SecurityJumpsEditor.Validate) для проверки корректности составляющих инструментов. 
- Кнопки добавления\/удаления инструментов. 
- Кнопка **Auto** \- позволяет автоматически создавать *непрерывный* инструмент. 
- Кнопка **Ok** \- завершение создания *непрерывного* инструмента. 

**Основные свойства**

- [ContinuousSecurityWindow.Security](xref:StockSharp.Xaml.ContinuousSecurityWindow.Security) \- непрерывный инструмент.
- [ContinuousSecurityWindow.SecurityStorage](xref:StockSharp.Xaml.ContinuousSecurityWindow.SecurityStorage) \- хранилище информации об инструментах. 

Ниже приведен фрагмент кода с его использованием. 

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

## См. также

[Склеивание данных](HydraGluingData.md)
