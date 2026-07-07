# Seletor

O componente [SecurityPicker](xref:StockSharp.Xaml.SecurityPicker) foi concebido para procurar e selecionar instrumentos. Suporta seleção única e múltipla. O componente permite filtrar a lista de instrumentos pelo respetivo tipo. Este componente também pode ser utilizado para apresentar informação financeira (campos Level1), como mostrado na secção [SecurityGrid](table.md).

![GUI SecurityPicker2](../../../../images/gui_securitypicker2.png)

[SecurityPicker](xref:StockSharp.Xaml.SecurityPicker) é composto por:

1. Um campo de texto para introduzir o código (ou Id) do instrumento. Ao introduzir texto, a lista é filtrada pela subcadeia introduzida.
2. A caixa combinada especial [SecurityTypeComboBox](xref:StockSharp.Xaml.SecurityTypeComboBox) para filtrar instrumentos pelo respetivo tipo.
3. A tabela [SecurityGrid](xref:StockSharp.Xaml.SecurityGrid) para apresentar a lista de instrumentos.

**Propriedades principais**

- [SecurityPicker.SelectionMode](xref:StockSharp.Xaml.SecurityPicker.SelectionMode) - modo de seleção de instrumentos: único, múltiplo.
- [SecurityPicker.ShowCommonStatColumns](xref:StockSharp.Xaml.SecurityPicker.ShowCommonStatColumns) - para apresentar as colunas principais.
- [SecurityPicker.ShowCommonOptionColumns](xref:StockSharp.Xaml.SecurityPicker.ShowCommonOptionColumns) - para apresentar as colunas principais para opções.
- [SecurityPicker.Title](xref:StockSharp.Xaml.SecurityPicker.Title) - o título apresentado na parte superior do componente.
- [SecurityPicker.Securities](xref:StockSharp.Xaml.SecurityPicker.Securities) - a lista de instrumentos.
- [SecurityPicker.SelectedSecurity](xref:StockSharp.Xaml.SecurityPicker.SelectedSecurity) - o instrumento selecionado.
- [SecurityPicker.SelectedSecurities](xref:StockSharp.Xaml.SecurityPicker.SelectedSecurities) - a lista de instrumentos selecionados.
- [SecurityPicker.FilteredSecurities](xref:StockSharp.Xaml.SecurityPicker.FilteredSecurities) - a lista de instrumentos filtrados.
- [SecurityPicker.ExcludeSecurities](xref:StockSharp.Xaml.SecurityPicker.ExcludeSecurities) - a lista de instrumentos ocultos.
- [SecurityPicker.SelectedType](xref:StockSharp.Xaml.SecurityPicker.SelectedType) - o tipo de instrumento selecionado.
- [SecurityPicker.SecurityProvider](xref:StockSharp.Xaml.SecurityPicker.SecurityProvider) - o fornecedor de informação sobre instrumentos.
- [SecurityPicker.MarketDataProvider](xref:StockSharp.Xaml.SecurityPicker.MarketDataProvider) - o fornecedor de dados de mercado.

Abaixo está um excerto de código com a sua utilização, retirado do exemplo *Samples\/InteractiveBrokers\/SampleIB*.

```xaml
<Window x:Class="Sample.SecuritiesWindow"
	xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
	xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
	xmlns:loc="clr-namespace:StockSharp.Localization;assembly=StockSharp.Localization"
	xmlns:xaml="http://schemas.stocksharp.com/xaml"
	Title="{x:Static loc:LocalizedStrings.Securities}" Height="415" Width="1081">
	<Grid>
		<Grid.RowDefinitions>
			<RowDefinition Height="*" />
			<RowDefinition Height="Auto" />
		</Grid.RowDefinitions>
		<xaml:SecurityPicker x:Name="SecurityPicker" x:FieldModifier="public" SecuritySelected="SecurityPicker_OnSecuritySelected" ShowCommonStatColumns="True" />
	</Grid>
</Window>
	  	
```
```cs
private void ConnectClick(object sender, RoutedEventArgs e)
{
	......................................
	_connector.SecurityReceived += (sub, security) => _securitiesWindow.SecurityPicker.Securities.Add(security);
	_securitiesWindow.SecurityPicker.MarketDataProvider = _connector;
	......................................
}
private void SecurityPicker_OnSecuritySelected(Security security)
{
	NewStopOrder.IsEnabled = NewOrder.IsEnabled =
	Level1.IsEnabled = Depth.IsEnabled = security != null;
}
```
