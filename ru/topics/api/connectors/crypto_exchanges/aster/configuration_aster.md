# Настройки коннектора Aster

Для работы с коннектором необходимо сгенерировать **API Key** и **Secret** в личном кабинете биржи и указать их в настройках подключения.

Основные настройки:

- **Key** и **Secret**.
- **Section**: `Spot` или `Derivatives`.
- **Derivatives mode**: `Legacy` или `V3 Agent`.
- адреса **Spot REST / Spot WS**.
- адреса **Derivatives REST / Derivatives WS**.
- режим **Demo**.

Официальная документация API:

- [Spot API overview](https://asterdex.github.io/aster-api-website/spot/spot-api-overview/)
- [Spot account and trading API](https://asterdex.github.io/aster-api-website/spot/spot-account-and-trading-api/)
- [Spot websocket market data](https://asterdex.github.io/aster-api-website/spot/websocket-market-data/)
- [Spot websocket account info](https://asterdex.github.io/aster-api-website/spot/websocket-account-info/)
- [Futures v3 general info](https://asterdex.github.io/aster-api-website/futures-v3/general-info/)
- [Futures user data streams](https://asterdex.github.io/aster-api-website/futures/user-data-streams/)
- [Aster Code endpoints](https://asterdex.github.io/aster-api-website/asterCode/endpoints/)

> [!TIP]
> Для деривативов у Aster есть две семьи протоколов. Перед запуском торговли выберите корректный **Derivatives mode**.
