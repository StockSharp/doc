# Настройки коннектора: FXOpen TickTrader

Создайте токен Web API FXOpen и укажите параметры подключения.

- `WebApiId` — идентификатор токена Web API.
- `Key` — ключ Web API.
- `Secret` — секрет Web API.
- `OneTimePassword` — необязательный одноразовый пароль, если требуется двухфакторная аутентификация.
- `IsDemo` — выбор демо-контура. Значение по умолчанию: `false`.
- `Address` — конечная точка REST. Адрес для реального счёта по умолчанию: `https://ttlivewebapi.fxopen.net`.
- `FeedAddress` — конечная точка WebSocket рыночных данных. Адрес для реального счёта по умолчанию: `wss://marginalttlivewebapi.fxopen.net/feed`.
- `TradeAddress` — конечная точка WebSocket торговых операций. Адрес для реального счёта по умолчанию: `wss://marginalttlivewebapi.fxopen.net/trade`.

При включении `IsDemo` выбираются официальные адреса демонстрационной среды TickTrader, если адреса не были изменены вручную. Идентификатор, ключ и секрет обязательны для подписок WebSocket и всех закрытых операций.

## См. также

[Официальная документация FXOpen API](https://ticktrader.fxopen.com/api)

[TickTrader Web REST API](https://ttlivewebapi.fxopen.net/api/doc/index?apiaddress=ttlivewebapi.fxopen.net&apiport=443)
