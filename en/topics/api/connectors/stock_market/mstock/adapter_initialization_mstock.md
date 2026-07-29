# Adapter initialization: m.Stock

The following code initializes [MStockMessageAdapter](xref:StockSharp.MStock.MStockMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new MStockMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Your API key>".To<SecureString>(),
	ClientCode = "<Your client code>",
	Password = "<Your password>".To<SecureString>(),
	Otp = "<Current OTP>".To<SecureString>(),
	UseTotp = true,
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the access values and any other required properties described on the [Connector configuration](configuration_mstock.md) page.

## See also

[Connector configuration](configuration_mstock.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
