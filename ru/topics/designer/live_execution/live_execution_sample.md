# Пример Live торговли

Для запуска примера в **Live торговлю** понадобится:

1. Тестовый терминал QUIK\-Junior, он предоставляется практически всеми брокерами на Российском рынке бесплатно, либо его можно получить на сайте производителя.

2. Настроить терминал QUIK\-Junior для работы с [Designer](../../designer.md). Как настроить терминал QUIK можно посмотреть в пункте [Настройка Quik Lua](../../api/connectors/russia/quik.md).

3. Настроить подключение к QUIK в [Designer](../../designer.md) и подключиться.

4. Скачать историю для необходимого инструмента. Для примера будет использоваться инструмент SBER@TQBR. В стратегии будут использоваться свечи с тайм\-фреймом в 5 сек., и история не понадобится, но для демонстрации возможности такой истории будет достаточно.

![Designer Example of Live trading 00](../../../images/designer_example_of_live_trading_00.png)

5. Настроить и запустить стратегию:

В примере со стратегией SMA будут использоваться следующие параметры:

- Инструмент SBER@TQBR
- Стандартное хранилище \\Documents\\StockSharp\\Designer\\Storage
- Формат хранилища CSV
- Тип данных которые будем брать из хранилища Ticks
- Свечи с тайм\-фреймом 5 секунд
- Объем 100
- Дней истории 2

![Designer Example of Live trading 01](../../../images/designer_example_of_live_trading_01.png)

После установки всех необходимых параметров идет запуск Live торговли стратегии, нажав кнопку ![Designer Panel Circuits 02](../../../images/designer_panel_circuits_02.png) Старт.

После нажатия кнопки ![Designer Panel Circuits 02](../../../images/designer_panel_circuits_02.png) Старт на графике начнет отображаться вся загруженная история за 2 дня:

![Designer Example of Live trading 02](../../../images/designer_example_of_live_trading_02.png)

Как прогрузится вся история из [Хранилище маркет\-данных](../market_data_storage.md) и Таблицы обезличенных сделок из терминала, стратегия начнет торговлю.

Ниже представлены графики из [Designer](../../designer.md) и из терминала QUIK за одинаковый период времени.

![Designer Example of Live trading 03](../../../images/designer_example_of_live_trading_03.png)

График из [Designer](../../designer.md):

![Designer Example of Live trading 04](../../../images/designer_example_of_live_trading_04.png)

График из QUIK:

## См. также

[Хранилище маркет\-данных](../market_data_storage.md)
