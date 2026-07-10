# Настройки коннектора edgeX

Для работы с коннектором необходимо сгенерировать **API-ключ** и **Секрет** в личном кабинете биржи и указать их в настройках подключения.

Основные настройки:

- **Ключ** и **Секрет**.
- **Клиринговый аккаунт** и **Парольная фраза**.
- **Секция**: `Spot` или `Derivatives`.
- **Включить спот**: включает секцию spot, если она поддерживается API.
- режим **демонстрации**.
- адреса **Spot REST / Derivatives REST**.
- адреса **Spot WS / Derivatives public WS / Derivatives private WS**.

Официальная документация API:

- [Аутентификация](https://edgex-1.gitbook.io/edgex-documentation/developer/api/authentication)
- [API заявок](https://edgex-1.gitbook.io/edgex-documentation/developer/api/private-api/order-api)
- [API аккаунта](https://edgex-1.gitbook.io/edgex-documentation/developer/api/private-api/account-api)
- [Приватный поток WebSocket](https://edgex-1.gitbook.io/edgex-documentation/developer/api/private-api/private-websocket-stream)
- [API финансирования](https://edgex-1.gitbook.io/edgex-documentation/developer/api/public-api/funding-api)
- [API метаданных](https://edgex-1.gitbook.io/edgex-documentation/developer/api/public-api/meta-data-api)
- [API котировок](https://edgex-1.gitbook.io/edgex-documentation/developer/api/public-api/quote-api)

> [!TIP]
> В текущей реализации полностью поддержан `Derivatives`. `Spot` включайте только при подтвержденной поддержке в целевой API среде.
