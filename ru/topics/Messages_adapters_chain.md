# Вспомогательные адаптеры

Для упрощения создания собственных подключений, в пакете [S\#](StockSharpAbout.md) входят ряд специальных адаптеров:

| Адаптер
                                                                                                    | Описание
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     |
| ------------------------------------------------------------------------------------------------------------ | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| [HeartbeatMessageAdapter](../api/StockSharp.Algo.HeartbeatMessageAdapter.html)
                             | Адаптер, отслеживающий сообщения [ConnectMessage](../api/StockSharp.Messages.ConnectMessage.html) и [DisconnectMessage](../api/StockSharp.Messages.DisconnectMessage.html) в случае разрыва соединения и последующих попыток его переустановить. Дополнительно, шлет в подключение [TimeMessage](../api/StockSharp.Messages.TimeMessage.html) для имитации ping сообщений, если значение [IMessageAdapter.HeartbeatInterval](../api/StockSharp.Messages.IMessageAdapter.HeartbeatInterval.html) установлено.
 |
| [Level1DepthBuilderAdapter](../api/StockSharp.Algo.Level1DepthBuilderAdapter.html)
                         | Адаптер, собирающий [QuoteChangeMessage](../api/StockSharp.Messages.QuoteChangeMessage.html) из [Level1ChangeMessage](../api/StockSharp.Messages.Level1ChangeMessage.html) сообщений, если при подписке [MarketDataMessage.BuildMode](../api/StockSharp.Messages.MarketDataMessage.BuildMode.html) было установлено и сообщение level1 содержит информацию о лучшей покупке или продаже.
                                                                                                                     |
| [Level1ExtendBuilderAdapter](../api/StockSharp.Algo.Level1ExtendBuilderAdapter.html)
                       | Адаптер, собирающий [Level1ChangeMessage](../api/StockSharp.Messages.Level1ChangeMessage.html) сообщения из [QuoteChangeMessage](../api/StockSharp.Messages.QuoteChangeMessage.html), тиковых сделок и свечей.
                                                                                                                                                                                                                                                                                               |
| [LookupTrackingMessageAdapter](../api/StockSharp.Algo.LookupTrackingMessageAdapter.html)
                   | Адаптер, отслеживающий подписки вида [SecurityLookupMessage](../api/StockSharp.Messages.SecurityLookupMessage.html), для которые подключение не шлет результирующее сообщение [SubscriptionFinishedMessage](../api/StockSharp.Messages.SubscriptionFinishedMessage.html). В этом случае через некий тайм\-аут данный адаптер самостоятельно формирует результирующее сообщение.
                                                                                                                              |
| [OrderBookIncrementMessageAdapter](../api/StockSharp.Algo.OrderBookIncrementMessageAdapter.html)
           | Адаптер, собирающий из инкрементальных сообщений целый стакан. Подробнее [Стаканы (инкрементальные и обычные)](Messages_adapters_books.md).
                                                                                                                                                                                                                                                                                                                                                                  |
| [OrderBookSortMessageAdapter](../api/StockSharp.Algo.OrderBookSortMessageAdapter.html)
                     | Адаптер, автоматически сортирующий в стакане покупки и продажи в случае, если [QuoteChangeMessage.IsSorted](../api/StockSharp.Messages.QuoteChangeMessage.IsSorted.html) установлено как **false**.
                                                                                                                                                                                                                                                                                                          |
| [OrderBookTruncateMessageAdapter](../api/StockSharp.Algo.OrderBookTruncateMessageAdapter.html)
             | Адаптер, автоматически обрезающий глубину стакана, если при подписке [MarketDataMessage.MaxDepth](../api/StockSharp.Messages.MarketDataMessage.MaxDepth.html) было установлено.
                                                                                                                                                                                                                                                                                                                              |
| [OrderLogMessageAdapter](../api/StockSharp.Algo.OrderLogMessageAdapter.html)
                               | Адаптер, автоматически создающий стакан из лога заявок, если при подписке [MarketDataMessage.BuildMode](../api/StockSharp.Messages.MarketDataMessage.BuildMode.html) было установлено. Подробнее [Лог заявок](Messages_adapters_orderlog.md).
                                                                                                                                                                                                                                                                |
| [PartialDownloadMessageAdapter](../api/StockSharp.Algo.PartialDownloadMessageAdapter.html)
                 | Адаптер, автоматически разбивающий запрос истории на несколько под\-запросов с интервалами. Подробнее [Исторические данные](Messages_adapters_history.md).
                                                                                                                                                                                                                                                                                                                                                   |
| [SecurityMappingMessageAdapter](../api/StockSharp.Algo.SecurityMappingMessageAdapter.html)
                 | Адаптер, автоматически заменяющий идентификаторы инструментов, если они заданы в хранилище [ISecurityMappingStorage](../api/StockSharp.Algo.Storages.ISecurityMappingStorage.html).
                                                                                                                                                                                                                                                                                                                          |
| [SecurityNativeIdMessageAdapter](../api/StockSharp.Algo.SecurityNativeIdMessageAdapter.html)
               | Адаптер, автоматически заменяющий идентификаторы инструментов, если если адаптер работает с системными кодами инструментов [IMessageAdapter.IsNativeIdentifiers](../api/StockSharp.Messages.IMessageAdapter.IsNativeIdentifiers.html).
                                                                                                                                                                                                                                                                       |
| [SubscriptionOnlineMessageAdapter](../api/StockSharp.Algo.SubscriptionOnlineMessageAdapter.html)
           | Адаптер, отслеживающий подписки и предотвращающий отправку дубликатов online\-подписок далее в подключение. Дублирующие подписки сохраняются и добавляются в исходящие сообщения, наследующиеся от [ISubscriptionIdMessage](../api/StockSharp.Messages.ISubscriptionIdMessage.html) через свойство [ISubscriptionIdMessage.SubscriptionIds](../api/StockSharp.Messages.ISubscriptionIdMessage.SubscriptionIds.html).
                                                                                         |
| [SubscriptionMessageAdapter](../api/StockSharp.Algo.SubscriptionMessageAdapter.html)
                       | Адаптер, отслеживающий подписки. В отличие от [SubscriptionOnlineMessageAdapter](../api/StockSharp.Algo.SubscriptionOnlineMessageAdapter.html), адаптер перенаправляет далее дублирующиеся подписки и работает не только с online подписками, а так же и с историей.
                                                                                                                                                                                                                                         |
| [TransactionOrderingMessageAdapter](../api/StockSharp.Algo.TransactionOrderingMessageAdapter.html)
         | Адаптер, отслеживающий транзакционные сообщения (заявки или сделки), и сортирующий их на случай, если информация о сделки приходит ранее, чем пришла информация о заявке, по которой прошла сделка.
                                                                                                                                                                                                                                                                                                          |
| [StorageMessageAdapter](../api/StockSharp.Algo.Storages.StorageMessageAdapter.html)
                        | Адаптер, отслеживающий исторические подписки, и пытающийся загрузить данные из внутреннего хранилища. В случае отсутствия необходимых данных, подписка перенаправляется далее.
                                                                                                                                                                                                                                                                                                                               |
| [StorageMetaInfoMessageAdapter](../api/StockSharp.Algo.Storages.StorageMetaInfoMessageAdapter.html)
        | Адаптер, пытающийся загрузить мета\-данные ([SecurityMessage](../api/StockSharp.Messages.SecurityMessage.html), [BoardMessage](../api/StockSharp.Messages.BoardMessage.html), [PositionChangeMessage](../api/StockSharp.Messages.PositionChangeMessage.html)) из внутреннего хранилища.
                                                                                                                                                                                                                      |
| [CandleBuilderMessageAdapter](../api/StockSharp.Algo.Candles.Compression.CandleBuilderMessageAdapter.html)
 | Адаптер, склеивающий, строящий (из тиков или других доступных данных) и загружающий свечи.
                                                                                                                                                                                                                                                                                                                                                                                                                   |
