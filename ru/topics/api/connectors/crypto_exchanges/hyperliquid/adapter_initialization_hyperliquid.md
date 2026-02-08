# Инициализация адаптера Hyperliquid

Код ниже демонстрирует, как инициализировать [HyperliquidMessageAdapter](xref:StockSharp.Hyperliquid.HyperliquidMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new HyperliquidMessageAdapter(Connector.TransactionIdGenerator)
{
	WalletAddress = "<Ваш Wallet Address>",
	PrivateKey = "<Ваш Private Key>".To<SecureString>(),
	Section = HyperliquidSections.Derivatives,
	IsTestnet = false,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
