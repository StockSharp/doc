> [!WARNING]
> Данная биржа прекратила работу (май 2023 — закрытие). Коннектор более не функционирует. Документация сохранена для справки.

# Инициализация адаптера Hotbit

Код ниже демонстрирует, как инициализировать [HotbitMessageAdapter](xref:StockSharp.Hotbit.HotbitMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new HotbitMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
