# Inicialização do adaptador Goldman Sachs Marquee

O código abaixo demonstra como inicializar o [MarqueeMessageAdapter](xref:StockSharp.Marquee.MarqueeMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new MarqueeMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientId = "<Seu valor>",
	ClientSecret = "<Seu valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Ver também

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
