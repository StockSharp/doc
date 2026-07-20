# Inicialização do adaptador Cboe DataShop / LiveVol

O código abaixo demonstra como inicializar o [CboeDataShopMessageAdapter](xref:StockSharp.CboeDataShop.CboeDataShopMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new CboeDataShopMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Seu valor>",
	Password = "<Seu valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Ver também

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
