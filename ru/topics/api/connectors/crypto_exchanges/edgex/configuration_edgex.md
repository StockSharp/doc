# Настройки коннектора edgeX

Для работы с коннектором необходимо сгенерировать **API Key** и **Secret** в личном кабинете биржи и указать их в настройках подключения.

Основные настройки:

- **Key** и **Secret**.
- **Clearing account** и **Passphrase**.
- **Section**: `Spot` или `Derivatives`.
- **Enable spot**: включает секцию spot, если она поддерживается API.
- режим **Demo**.
- адреса **Spot REST / Derivatives REST**.
- адреса **Spot WS / Derivatives public WS / Derivatives private WS**.

Официальная документация API:

- [Authentication](https://edgex-1.gitbook.io/edgex-documentation/developer/api/authentication)
- [Order API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/private-api/order-api)
- [Account API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/private-api/account-api)
- [Private websocket stream](https://edgex-1.gitbook.io/edgex-documentation/developer/api/private-api/private-websocket-stream)
- [Funding API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/public-api/funding-api)
- [Meta-data API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/public-api/meta-data-api)
- [Quote API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/public-api/quote-api)

> [!TIP]
> В текущей реализации полностью поддержан `Derivatives`. `Spot` включайте только при подтвержденной поддержке в целевой API среде.
