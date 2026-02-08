# Инициализация адаптера Aster

Код ниже демонстрирует, как инициализировать [AsterMessageAdapter](xref:StockSharp.Aster.AsterMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new AsterMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Ваш API Key>".To<SecureString>(),
	Secret = "<Ваш API Secret>".To<SecureString>(),
	Section = AsterSections.Derivatives,
	DerivativesProtocolMode = AsterDerivativesProtocolModes.Legacy,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
