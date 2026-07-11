# Futuros contínuos

[ContinuousSecurityWindow](xref:StockSharp.Xaml.ContinuousSecurityWindow) - é um editor visual para criar instrumentos *contínuos* ([ContinuousSecurity](xref:StockSharp.Algo.ContinuousSecurity)). Consulte [Futuros contínuos](../../instruments/continuous_futures.md).

![Hydra futuro contínuo personalizado](../../../../images/hydragluingcscustom.png)

Este componente inclui:

- Campo de texto especial [SecurityIdTextBox](xref:StockSharp.Xaml.SecurityIdTextBox), que gera um instrumento *contínuo* com a introdução do Id - \[Code\]@\[Board\].
- O componente [SecurityJumpsEditor](xref:StockSharp.Xaml.SecurityJumpsEditor) é um DataGrid especial para trabalhar com instrumentos que fazem parte de um instrumento *contínuo*. Os instrumentos são encapsulados na classe [SecurityJump](xref:StockSharp.Xaml.SecurityJump), que tem duas propriedades: [SecurityJump.Security](xref:StockSharp.Xaml.SecurityJump.Security) e [SecurityJump.Date](xref:StockSharp.Xaml.SecurityJump.Date) (roll forward). Os instrumentos adicionados são guardados na lista [SecurityJumpsEditor.Jumps](xref:StockSharp.Xaml.SecurityJumpsEditor.Jumps). O componente tem a função [SecurityJumpsEditor.Validate](xref:StockSharp.Xaml.SecurityJumpsEditor.Validate) para verificar a correção dos instrumentos do componente.
- Botões para adicionar/remover instrumentos.
- O botão **Auto** permite criar automaticamente um instrumento *contínuo*.
- O botão **Ok** conclui a criação de um instrumento *contínuo*.

**Propriedades principais**

- [ContinuousSecurityWindow.Security](xref:StockSharp.Xaml.ContinuousSecurityWindow.Security) - instrumento contínuo
- [ContinuousSecurityWindow.SecurityStorage](xref:StockSharp.Xaml.ContinuousSecurityWindow.SecurityStorage) - fornecedor de informação sobre instrumentos.

Abaixo está um excerto de código com a sua utilização.

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

## Conteúdo recomendado

[Futuros contínuos](../../../hydra/instruments_and_boards/continuous_futures.md)
