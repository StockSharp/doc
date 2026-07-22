# Inicialização do adaptador Chainlink Data Streams

O código abaixo demonstra como inicializar o [ChainlinkDataStreamsMessageAdapter](xref:StockSharp.ChainlinkDataStreams.ChainlinkDataStreamsMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ChainlinkDataStreamsMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Seu valor>".To<SecureString>(),
	Secret = "<Seu valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
