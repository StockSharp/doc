# Протокол SBE

Поддержка **SBE (Simple Binary Encoding)** предоставляет компактный бинарный транспорт для рыночных данных и торговых сообщений с низкой задержкой.

В StockSharp предусмотрены [SbeRecordSerializer](xref:StockSharp.Server.Sbe.SbeRecordSerializer) для кодирования и декодирования записей, [SbeServer](xref:StockSharp.Server.Sbe.SbeServer) для приема клиентских подключений и [StockSharpSBEMessageAdapter](xref:StockSharp.SBE.StockSharpSBEMessageAdapter) для подключения клиента.

Реализация поддерживает поиск инструментов, Level1, стаканы, тики, необязательные нативные свечи, данные по портфелям и позициям, а также операции с заявками. Идентификаторы и версии схемы на клиенте и сервере должны совпадать.

## См. также

[Настройка SBE](sbe_protocol/configuration_sbe.md)

[Инициализация адаптера SBE](sbe_protocol/adapter_initialization_sbe.md)

[Протокол FIX](../common/fix_protocol.md)

[Протокол FAST](../common/fast_protocol.md)
