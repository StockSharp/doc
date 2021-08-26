# Инициализация адаптера Bibox

Код ниже демонстрирует как инициализировать [BiboxMessageAdapter](../api/StockSharp.Bibox.BiboxMessageAdapter.html) и передать его в [Connector](../api/StockSharp.Algo.Connector.html).

```cs
            Connector Connector = new Connector();				
            ...				
            var messageAdapter = new BiboxMessageAdapter(Connector.TransactionIdGenerator)
            {
                Key = "<Your API Key>".To<SecureString>(),
                Secret = "<Your API Secret>".To<SecureString>(),
            };
            Connector.Adapter.InnerAdapters.Add(messageAdapter);
            ...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
