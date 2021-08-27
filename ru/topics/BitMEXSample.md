# Инициализация адаптера BitMEX

Код ниже демонстрирует как инициализировать [BitmexMessageAdapter](xref:StockSharp.Bitmex.BitmexMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
            Connector Connector = new Connector();				
            ...				
            var messageAdapter = new BitmexMessageAdapter(Connector.TransactionIdGenerator)
            {
                Key = "<Your API Key>".To<SecureString>(),
                Secret = "<Your API Secret>".To<SecureString>(),
            };
            Connector.Adapter.InnerAdapters.Add(messageAdapter);
            ...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
