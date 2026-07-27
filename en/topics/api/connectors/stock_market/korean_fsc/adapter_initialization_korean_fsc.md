# Adapter initialization: Korean FSC

The following code initializes [KoreanFscMessageAdapter](xref:StockSharp.KoreanFsc.KoreanFscMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new KoreanFscMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_korean_fsc.md) page.

## See also

[Connector configuration](configuration_korean_fsc.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
