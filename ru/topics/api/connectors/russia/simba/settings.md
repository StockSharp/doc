# Настройки коннектора Simba

Коннектор использует протокол SBE и может работать в диалектах **Spectra** и **ASTS**. Выбор нужного диалекта осуществляется через свойство [DialectType](xref:StockSharp.Simba.SimbaMessageAdapter.DialectType).

```cs
var messageAdapter = new SimbaMessageAdapter(Connector.TransactionIdGenerator)
{
	DialectType = DialectTypes.Spectra, // или DialectTypes.Asts
};
```

Параметры подключения (адреса и порты UDP потоков) выдаются брокером.
