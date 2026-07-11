# Ventana de configuración de conexión

[ConnectorWindow](xref:StockSharp.Xaml.ConnectorWindow) - una ventana especial para configurar adaptadores para conectar un conector. 

![Ventana de conexión de API GUI](../../../images/api_gui_connectorwindow.png)

Esta es la ventana de configuración de conexión. En la lista desplegable (se abre con el botón '+'), debe seleccionar los adaptadores necesarios y configurar sus propiedades en la ventana de propiedades situada a la derecha. 

Esta ventana debe llamarse mediante el método de extensión [Extensions.Configure](xref:StockSharp.Xaml.Extensions.Configure(StockSharp.Algo.Connector,System.Windows.Window))**(**conector [StockSharp.Algo.Connector](xref:StockSharp.Algo.Connector), propietario [System.Windows.Window](xref:System.Windows.Window) **)**, al que se pasan el [Connector](xref:StockSharp.Algo.Connector) y la ventana principal. Si la configuración se realiza correctamente, el método de extensión [Extensions.Configure](xref:StockSharp.Xaml.Extensions.Configure(StockSharp.Algo.Connector,System.Windows.Window))**(**conector [StockSharp.Algo.Connector](xref:StockSharp.Algo.Connector), propietario [System.Windows.Window](xref:System.Windows.Window) **)** devolverá 'true'. A continuación se muestra el código para llamar a la ventana de configuración de conexión del conector y guardar la configuración en un archivo. 

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
> La corrección de la conexión se puede comprobar mediante el botón **Comprobar**.

El resultado de esta ventana será crear y agregar adaptadores a la lista de *adaptadores internos* de la propiedad [Connector.Adapter](xref:StockSharp.Algo.Connector.Adapter). 

Para obtener más información sobre cómo guardar y cargar la configuración del conector, consulte [Guardar y cargar la configuración](../connectors/save_and_load_settings.md).
