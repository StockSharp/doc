# Подключение нескольких роботов

В зависимости от конкретного пользователя\/приложения сервер OEC может не поддерживать одновременное соединение нескольких приложений. В этом случае, если существуют другие соединения, они могут быть разорваны. Для обхода этого ограничения данная реализация [OpenECryMessageAdapter](xref:StockSharp.OpenECry.OpenECryMessageAdapter) поддерживает одновременную работу нескольких приложений через одно соединение с OEC сервером – [OECRemoting](https://gainfutures.com/gainfuturesapi).

Поддерживаются следующие режимы [OpenECryRemoting](xref:StockSharp.OpenECry.OpenECryRemoting):

- [None](xref:StockSharp.OpenECry.OpenECryRemoting.None) \- [OpenECryRemoting](xref:StockSharp.OpenECry.OpenECryRemoting) отключен. Приложение создает собственное соединение с сервером OEC. Приложение не может выступать как [Primary](xref:StockSharp.OpenECry.OpenECryRemoting.Primary) для других приложений.
- [Primary](xref:StockSharp.OpenECry.OpenECryRemoting.Primary) – приложение создает собственное соединение с сервером OEC.
- [Secondary](xref:StockSharp.OpenECry.OpenECryRemoting.Secondary) \- В момент инициализации выполняется поиск локальных приложений, работающих в режиме [Primary](xref:StockSharp.OpenECry.OpenECryRemoting.Primary). Если такие приложения найдены, используется их соединение с сервером OEC. В противном случае приложение переходит в режим [None](xref:StockSharp.OpenECry.OpenECryRemoting.None).

Для явного задания режима [OECRemoting](https://gainfutures.com/gainfuturesapi) необходимо сразу после создания объекта [OpenECryMessageAdapter](xref:StockSharp.OpenECry.OpenECryMessageAdapter) указать требуемый режим. Например, для указания режима [Secondary](xref:StockSharp.OpenECry.OpenECryRemoting.Secondary):

```cs
		MessageAdapterMessageAdapter.Remoting = OECRemoting.Secondary;
		
```

По умолчанию адаптер [OpenECryMessageAdapter](xref:StockSharp.OpenECry.OpenECryMessageAdapter) работает в режиме [OpenECryRemoting.None](xref:StockSharp.OpenECry.OpenECryRemoting.None).
