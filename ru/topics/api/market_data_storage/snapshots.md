# Система снапшотов

Снапшоты (snapshots) в StockSharp представляют собой механизм сохранения последнего актуального состояния маркет-данных. Вместо сканирования всей истории, снапшоты позволяют мгновенно получить текущее значение Level1, стакан, позицию или транзакцию.

## Назначение снапшотов

При работе с потоковыми данными часто необходимо знать последнее состояние инструмента -- текущую цену, стакан котировок, открытую позицию. Без снапшотов для получения этой информации пришлось бы загружать и обрабатывать всю историю. Система снапшотов решает эту задачу, сохраняя последнее состояние каждого объекта и обеспечивая доступ к нему за минимальное время.

## ISnapshotStorage -- интерфейс хранилища снапшотов

Интерфейс [ISnapshotStorage](xref:StockSharp.Algo.Storages.ISnapshotStorage) определяет базовый контракт для работы со снапшотами. Типизированная версия `ISnapshotStorage<TKey, TMessage>` предоставляет следующие методы:

- **Update(message)** -- сохранить или обновить снапшот. Если снапшот для данного ключа уже существует, он будет обновлен.
- **Get(key)** -- получить снапшот по ключу (например, по идентификатору инструмента).
- **GetAll(from, to)** -- получить все снапшоты за указанный диапазон дат.
- **Clear(key)** -- удалить снапшот для конкретного ключа.
- **ClearAll()** -- удалить все снапшоты.

## ISnapshotSerializer -- сериализация снапшотов

Интерфейс [ISnapshotSerializer](xref:StockSharp.Algo.Storages.ISnapshotSerializer`2) отвечает за преобразование снапшотов в бинарное представление и обратно:

- **DataType** -- информация о типе данных снапшота.
- **Version** -- версия формата сериализации.
- **Serialize(version, message)** -- сериализовать сообщение в массив байтов.
- **Deserialize(version, buffer)** -- десериализовать массив байтов обратно в сообщение.
- **GetKey(message)** -- извлечь ключ из сообщения.
- **Update(message, changes)** -- применить инкрементальные изменения к существующему снапшоту.

## SnapshotRegistry -- реестр снапшотов

Класс [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry) является центральным компонентом управления снапшотами. Он реализует интерфейс `ISnapshotRegistry` и координирует работу всех хранилищ снапшотов.

### Основные возможности

- **Управление хранилищами** -- предоставляет доступ к хранилищам снапшотов для различных типов данных.
- **Периодическая запись** -- изменения сбрасываются на диск каждые 10 секунд, что обеспечивает баланс между производительностью и надежностью.
- **Потокобезопасность** -- все операции безопасны для использования из нескольких потоков.

### Организация файлов

Файлы снапшотов хранятся по следующему пути:

```
{путь}/{yyyy_MM_dd}/{имя_сериализатора}.bin
```

## Встроенные сериализаторы

StockSharp включает четыре сериализатора для основных типов маркет-данных:

| Сериализатор | Тип сообщения | Назначение |
|---|---|---|
| [Level1BinarySnapshotSerializer](xref:StockSharp.Algo.Storages.Binary.Snapshot.Level1BinarySnapshotSerializer) | `Level1ChangeMessage` | Данные Level1 (цены, объемы, спреды) |
| [QuotesBinarySnapshotSerializer](xref:StockSharp.Algo.Storages.Binary.Snapshot.QuotesBinarySnapshotSerializer) | `QuoteChangeMessage` | Стакан котировок |
| [PositionBinarySnapshotSerializer](xref:StockSharp.Algo.Storages.Binary.Snapshot.PositionBinarySnapshotSerializer) | `PositionChangeMessage` | Позиции |
| [TransactionBinarySnapshotSerializer](xref:StockSharp.Algo.Storages.Binary.Snapshot.TransactionBinarySnapshotSerializer) | `ExecutionMessage` | Транзакции (заявки и сделки) |

Каждый сериализатор поддерживает версионирование формата, что обеспечивает обратную совместимость при обновлении StockSharp.

## Пример кода

### Создание реестра снапшотов

```cs
var snapshotRegistry = new SnapshotRegistry(Path.Combine(
    Directory.GetCurrentDirectory(), "Snapshots"));
```

### Работа со снапшотами Level1

```cs
// Получение хранилища снапшотов Level1
var level1Snapshots = snapshotRegistry.GetSnapshotStorage(
    DataType.Level1);

// Сохранение снапшота
var level1Msg = new Level1ChangeMessage
{
    SecurityId = "SBER@TQBR".ToSecurityId(),
    ServerTime = DateTimeOffset.Now,
};
level1Msg.TryAdd(Level1Fields.LastTradePrice, 260.5m);
level1Msg.TryAdd(Level1Fields.BestBidPrice, 260.4m);
level1Msg.TryAdd(Level1Fields.BestAskPrice, 260.6m);

level1Snapshots.Update(level1Msg);
```

### Получение снапшота

```cs
// Получение последнего снапшота для инструмента
var secId = "SBER@TQBR".ToSecurityId();
var snapshot = level1Snapshots.Get(secId);

if (snapshot != null)
{
    Console.WriteLine($"Последняя цена: {snapshot.Changes[Level1Fields.LastTradePrice]}");
}
```

## См. также

- [Работа с API](api.md)
- [Форматы хранения](formats.md)
- [Драйверы хранения](drives.md)
