# Инициализация адаптера TradeOgre

Код ниже демонстрирует как инициализировать [TradeOgreMessageAdapter](xref:StockSharp.TradeOgre.TradeOgreMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
            Connector Connector = new Connector();				
            ...				
            var messageAdapter = new TradeOgreMessageAdapter(Connector.TransactionIdGenerator)
            {
                Key = "<Your API Key>".To<SecureString>(),
                Secret = "<Your API Secret>".To<SecureString>(),
            };
            Connector.Adapter.InnerAdapters.Add(messageAdapter);
            ...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
