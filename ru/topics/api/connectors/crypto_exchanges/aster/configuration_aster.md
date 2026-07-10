# Настройки коннектора Aster

Для работы с коннектором необходимо сгенерировать **API-ключ** и **Секрет** в личном кабинете биржи и указать их в настройках подключения.

Основные настройки:

- **Ключ** и **Секрет**.
- **Секция**: `Spot` или `Derivatives`.
- **Режим деривативов**: `Legacy` или `V3 Agent`.
- адреса **Spot REST / Spot WS**.
- адреса **Derivatives REST / Derivatives WS**.
- режим **демонстрации**.

Официальная документация API:

- [Обзор Spot API](https://asterdex.github.io/aster-api-website/spot/spot-api-overview/)
- [API спотового аккаунта и торговли](https://asterdex.github.io/aster-api-website/spot/spot-account-and-trading-api/)
- [Спотовые рыночные данные WebSocket](https://asterdex.github.io/aster-api-website/spot/websocket-market-data/)
- [Информация спотового аккаунта WebSocket](https://asterdex.github.io/aster-api-website/spot/websocket-account-info/)
- [Общая информация Futures v3](https://asterdex.github.io/aster-api-website/futures-v3/general-info/)
- [Пользовательские потоки данных Futures](https://asterdex.github.io/aster-api-website/futures/user-data-streams/)
- [Конечные точки Aster Code](https://asterdex.github.io/aster-api-website/asterCode/endpoints/)

> [!TIP]
> Для деривативов у Aster есть две семьи протоколов. Перед запуском торговли выберите корректный **Режим деривативов**.
