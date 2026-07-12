# Inicialização do adaptador Bitget

O código abaixo demonstra como inicializar o [BitgetMessageAdapter](xref:StockSharp.Bitget.BitgetMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new BitgetMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<A sua chave de API>".To<SecureString>(),
	Secret = "<O seu segredo de API>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
