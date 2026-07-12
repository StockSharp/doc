# Janela de definições de ligação

[ConnectorWindow](xref:StockSharp.Xaml.ConnectorWindow) - uma janela especial para configurar adaptadores para ligar um conector.

![Janela de conexão da API GUI](../../../images/api_gui_connectorwindow.png)

Esta é a janela de definições de ligação. Na lista pendente (abre com o botão '+'), é necessário selecionar os adaptadores necessários e configurar as suas propriedades na janela de propriedades localizada à direita.

Esta janela deve ser chamada através do método de extensão [Extensions.Configure](xref:StockSharp.Xaml.Extensions.Configure(StockSharp.Algo.Connector,System.Windows.Window))**(**[StockSharp.Algo.Connector](xref:StockSharp.Algo.Connector) connector, [System.Windows.Window](xref:System.Windows.Window) owner **)**, ao qual são passados o [Connector](xref:StockSharp.Algo.Connector) e a janela principal. Se a configuração for bem-sucedida, o método de extensão [Extensions.Configure](xref:StockSharp.Xaml.Extensions.Configure(StockSharp.Algo.Connector,System.Windows.Window))**(**[StockSharp.Algo.Connector](xref:StockSharp.Algo.Connector) connector, [System.Windows.Window](xref:System.Windows.Window) owner **)** devolverá 'true'. Abaixo está o código para chamar a janela de definições de ligação do conector e guardar as definições num ficheiro.

```cs
		private void Setting_Click(object sender, RoutedEventArgs e)
		{
			if (_connector.Configure(this))
			{
				new JsonSerializer<SettingsStorage>().Serialize(_connector.Save(), _connectorFile);
			}
		}
	  				
```

> [!TIP]
> A correção da ligação pode ser verificada utilizando o botão **Verificar**.

O resultado desta janela será criar e adicionar adaptadores à lista de *adaptadores internos* da propriedade [Connector.Adapter](xref:StockSharp.Algo.Connector.Adapter).

Para mais informações sobre guardar e carregar definições do conector, consulte [Guardar e carregar definições](../connectors/save_and_load_settings.md).
