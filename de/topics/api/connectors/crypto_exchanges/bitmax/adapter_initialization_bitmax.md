> [!NOTE]
> BitMax wurde in AscendEX umbenannt. Diese Dokumentation bleibt zu historischen Referenzzwecken erhalten.

# Adapterinitialisierung BitMax

Der folgende Code zeigt, wie man den [BitMaxMessageAdapter](xref:StockSharp.BitMax.BitMaxMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergibt.

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new BitMaxMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ihr API-Schlüssel>".To<SecureString>(),
				Secret = "<Ihr API-Geheimnis>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
