> [!NOTE]
> BitMax ha sido renombrado a AscendEX. Esta documentación se conserva como referencia histórica.

# Inicialización del adaptador BitMax

El código siguiente muestra cómo inicializar el [BitMaxMessageAdapter](xref:StockSharp.BitMax.BitMaxMessageAdapter) y enviarlo al [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new BitMaxMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Su clave API>".To<SecureString>(),
				Secret = "<Su secreto API>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
