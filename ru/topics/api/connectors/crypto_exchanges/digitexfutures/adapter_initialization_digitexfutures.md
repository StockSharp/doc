> [!WARNING]
> Данная биржа прекратила работу (~2022 — закрытие). Коннектор более не функционирует. Документация сохранена для справки.

# Инициализация адаптера DigitexFutures

Код ниже демонстрирует, как инициализировать [DigitexFuturesMessageAdapter](xref:StockSharp.DigitexFutures.DigitexFuturesMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new DigitexFuturesMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
