# Inicialização do adaptador Binance History

O código abaixo demonstra como inicializar o [BinanceHistoryMessageAdapter](xref:StockSharp.BinanceHistory.BinanceHistoryMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new BinanceHistoryMessageAdapter(Connector.TransactionIdGenerator)
{
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
