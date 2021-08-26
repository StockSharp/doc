# Подключение нескольких роботов

В зависимости от конкретного пользователя\/приложения сервер OEC может не поддерживать одновременное соединение нескольких приложений. В этом случае, если существуют другие соединения, они могут быть разорваны. Для обхода этого ограничения данная реализация [OpenECryMessageAdapter](../api/StockSharp.OpenECry.OpenECryMessageAdapter.html) поддерживает одновременную работу нескольких приложений через одно соединение с OEC сервером – [OECRemoting](https://gainfutures.com/gainfuturesapi).

Поддерживаются следующие режимы [OpenECryRemoting](../api/StockSharp.OpenECry.OpenECryRemoting.html):

- [None](../api/StockSharp.OpenECry.OpenECryRemoting.None.html) \- [OpenECryRemoting](../api/StockSharp.OpenECry.OpenECryRemoting.html) отключен. Приложение создает собственное соединение с сервером OEC. Приложение не может выступать как [Primary](../api/StockSharp.OpenECry.OpenECryRemoting.Primary.html) для других приложений.
- [Primary](../api/StockSharp.OpenECry.OpenECryRemoting.Primary.html) – приложение создает собственное соединение с сервером OEC.
- [Secondary](../api/StockSharp.OpenECry.OpenECryRemoting.Secondary.html) \- В момент инициализации выполняется поиск локальных приложений, работающих в режиме [Primary](../api/StockSharp.OpenECry.OpenECryRemoting.Primary.html). Если такие приложения найдены, используется их соединение с сервером OEC. В противном случае приложение переходит в режим [None](../api/StockSharp.OpenECry.OpenECryRemoting.None.html).

Для явного задания режима [OECRemoting](https://gainfutures.com/gainfuturesapi) необходимо сразу после создания объекта [OpenECryMessageAdapter](../api/StockSharp.OpenECry.OpenECryMessageAdapter.html) указать требуемый режим. Например, для указания режима [Secondary](../api/StockSharp.OpenECry.OpenECryRemoting.Secondary.html):

```cs
		MessageAdapterMessageAdapter.Remoting \= OECRemoting.Secondary;
		
```

По умолчанию адаптер [OpenECryMessageAdapter](../api/StockSharp.OpenECry.OpenECryMessageAdapter.html) работает в режиме [OpenECryRemoting.None](../api/StockSharp.OpenECry.OpenECryRemoting.None.html).
