# Inicialização do adaptador BitGo

O código abaixo demonstra como inicializar o [BitGoMessageAdapter](xref:StockSharp.BitGo.BitGoMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new BitGoMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Seu valor>".To<SecureString>(),
	Account = "<Seu valor>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
