# Аналитические скрипты

В [S\#](../api.md) реализована подсистема аналитических скриптов, позволяющая выполнять произвольный анализ маркет-данных с визуализацией результатов. Классы расположены в пространстве имён `StockSharp.Algo.Analytics`.

## IAnalyticsScript — основной интерфейс

Интерфейс [IAnalyticsScript](xref:StockSharp.Algo.Analytics.IAnalyticsScript) определяет единственный метод:

```cs
Task Run(
    ILogReceiver logs,
    IAnalyticsPanel panel,
    SecurityId[] securities,
    DateTime from,
    DateTime to,
    IStorageRegistry storage,
    IMarketDataDrive drive,
    StorageFormats format,
    DataType dataType,
    CancellationToken cancellationToken);
```

Параметры:

- **logs** — приёмник логов для вывода диагностических сообщений.
- **panel** — панель для отображения результатов анализа.
- **securities** — массив анализируемых инструментов.
- **from** / **to** — временной диапазон.
- **storage** — реестр хранилищ маркет-данных.
- **drive** — источник данных.
- **format** — формат хранения данных.
- **dataType** — тип анализируемых данных.
- **cancellationToken** — токен отмены.

## IAnalyticsPanel — панель результатов

Интерфейс [IAnalyticsPanel](xref:StockSharp.Algo.Analytics.IAnalyticsPanel) предоставляет методы для создания различных визуализаций:

- **CreateGrid(params string[] columns)** — создаёт таблицу [IAnalyticsGrid](xref:StockSharp.Algo.Analytics.IAnalyticsGrid) с указанными колонками.
- **CreateChart\<X, Y\>()** — создаёт двумерный график [IAnalyticsChart](xref:StockSharp.Algo.Analytics.IAnalyticsChart`2).
- **CreateChart\<X, Y, Z\>()** — создаёт трёхмерный график [IAnalyticsChart](xref:StockSharp.Algo.Analytics.IAnalyticsChart`3).
- **DrawHeatmap(string[] xTitles, string[] yTitles, double[,] data)** — рисует тепловую карту.
- **Draw3D(string[] xTitles, string[] yTitles, data, xTitle, yTitle, zTitle)** — рисует 3D-визуализацию.

## IAnalyticsChart — графики

Интерфейс [IAnalyticsChart](xref:StockSharp.Algo.Analytics.IAnalyticsChart`2) предоставляет метод для добавления серий данных:

```cs
void Append(string title, IEnumerable<X> xValues, IEnumerable<Y> yValues,
    DrawStyles style, Color? color = null);
```

Доступные стили отрисовки ([DrawStyles](xref:StockSharp.Algo.Analytics.DrawStyles)):

- **Line** — линейный график.
- **DashedLine** — пунктирная линия.
- **Histogram** — гистограмма.
- **Bubble** — пузырьковый график.

## IAnalyticsGrid — таблицы

Интерфейс [IAnalyticsGrid](xref:StockSharp.Algo.Analytics.IAnalyticsGrid) позволяет отображать табличные данные:

- **SetSort(string column, bool ascending)** — задать сортировку по колонке.
- **SetRow(params object[] values)** — добавить строку данных.

## Встроенные скрипты

В пакете `StockSharp.Algo.Analytics.CSharp` доступны готовые скрипты:

- **IndicatorScript** — расчёт и визуализация индикаторов на графике.
- **ChartDrawScript** — демонстрация построения различных типов графиков.
- **PriceVolumeScript** — анализ распределения объёмов по ценовым уровням.

## Пример: пользовательский аналитический скрипт

Ниже приведён пример скрипта, который загружает свечи для списка инструментов и отображает цены закрытия на линейном графике:

```cs
public class MyAnalyticsScript : IAnalyticsScript
{
    public async Task Run(ILogReceiver logs, IAnalyticsPanel panel,
        SecurityId[] securities, DateTime from, DateTime to,
        IStorageRegistry storage, IMarketDataDrive drive,
        StorageFormats format, DataType dataType,
        CancellationToken cancellationToken)
    {
        // создание двумерного графика
        var chart = panel.CreateChart<DateTime, decimal>();

        foreach (var secId in securities)
        {
            // получение хранилища свечей
            var candleStorage = storage.GetCandleMessageStorage(
                secId, dataType, drive, format);

            // загрузка данных за период
            var candles = await candleStorage
                .LoadAsync(from, to)
                .WithCancellation(cancellationToken)
                .ToArrayAsync(cancellationToken);

            if (candles.Length == 0)
            {
                logs.AddWarningLog($"Нет данных для {secId}");
                continue;
            }

            // добавление серии на график
            chart.Append(secId.ToString(),
                candles.Select(c => c.OpenTime.UtcDateTime),
                candles.Select(c => c.ClosePrice),
                DrawStyles.Line);

            logs.AddInfoLog($"{secId}: загружено {candles.Length} свечей");
        }
    }
}
```

## Пример: таблица с объёмами

```cs
public class VolumeTableScript : IAnalyticsScript
{
    public async Task Run(ILogReceiver logs, IAnalyticsPanel panel,
        SecurityId[] securities, DateTime from, DateTime to,
        IStorageRegistry storage, IMarketDataDrive drive,
        StorageFormats format, DataType dataType,
        CancellationToken cancellationToken)
    {
        var grid = panel.CreateGrid("Инструмент", "Всего свечей",
            "Суммарный объём", "Средний объём");
        grid.SetSort("Суммарный объём", false);

        foreach (var secId in securities)
        {
            var candleStorage = storage.GetCandleMessageStorage(
                secId, dataType, drive, format);

            var candles = await candleStorage
                .LoadAsync(from, to)
                .WithCancellation(cancellationToken)
                .ToArrayAsync(cancellationToken);

            if (candles.Length == 0)
                continue;

            var totalVolume = candles.Sum(c => c.TotalVolume);
            var avgVolume = totalVolume / candles.Length;

            grid.SetRow(secId.ToString(), candles.Length,
                totalVolume, avgVolume);
        }
    }
}
```

## См. также

[Индикаторы](indicators.md)

[Хранение данных](market_data_storage.md)
