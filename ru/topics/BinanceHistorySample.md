# Инициализация адаптера Binance History

Код ниже демонстрирует как инициализировать [BinanceHistoryMessageAdapter](xref:StockSharp.BinanceHistory.BinanceHistoryMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
            Connector Connector = new Connector();				
            ...				
            var messageAdapter = new BinanceHistoryMessageAdapter(Connector.TransactionIdGenerator);
            Connector.Adapter.InnerAdapters.Add(messageAdapter);
            ...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
