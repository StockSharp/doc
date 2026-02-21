> [!WARNING]
> Данная биржа прекратила работу (июль 2017 — конфискация). Коннектор более не функционирует. Документация сохранена для справки.

# Инициализация адаптера WEX (BTC\-e)

Код ниже демонстрирует, как инициализировать [BtceMessageAdapter](xref:StockSharp.Btce.BtceMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new BtceMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
