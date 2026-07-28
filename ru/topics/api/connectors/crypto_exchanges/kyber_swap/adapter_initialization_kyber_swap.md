# Инициализация адаптера: KyberSwap

Следующий код создаёт [KyberSwapMessageAdapter](xref:StockSharp.KyberSwap.KyberSwapMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new KyberSwapMessageAdapter(connector.TransactionIdGenerator)
{
	ClientId = "<Идентификатор клиента>",
	WalletAddress = "<Адрес вашего кошелька>",
	PrivateKey = "<Ваш закрытый ключ>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_kyber_swap.md).

## См. также

[Настройки коннектора](configuration_kyber_swap.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
