# Инициализация адаптера Paradex

Код ниже демонстрирует, как инициализировать [ParadexMessageAdapter](xref:StockSharp.Paradex.ParadexMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ParadexMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Ваш API Key>".To<SecureString>(),
	Secret = "<Ваш API Secret>".To<SecureString>(),
	StarknetAccount = "<Ваш Starknet Account>",
	StarknetPrivateKey = "<Ваш Starknet Private Key>".To<SecureString>(),
	Section = ParadexSections.Derivatives,
	AuthPath = "/v1/auth",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
