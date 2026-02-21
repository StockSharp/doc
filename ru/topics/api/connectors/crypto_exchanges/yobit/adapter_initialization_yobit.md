> [!WARNING]
> Данная биржа прекратила работу (~2023 — фактически мертва). Коннектор более не функционирует. Документация сохранена для справки.

# Инициализация адаптера Yobit

Код ниже демонстрирует, как инициализировать [YobitMessageAdapter](xref:StockSharp.Yobit.YobitMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new YobitMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
