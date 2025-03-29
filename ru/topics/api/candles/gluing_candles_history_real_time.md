# Склеивание свечей, история + реал-тайм

Для склеивания исторических свечей с данными реального времени, необходимо инициализировать соответствующие хранилища: хранилище торговых объектов [CsvEntityRegistry](xref:StockSharp.Algo.Storages.Csv.CsvEntityRegistry), хранилище маркет-данных [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) и реестр хранилищ-снэпшотов [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry).

Рассмотрим пример из проекта Samples/Candles/CombineHistoryRealtime:

## Настройка хранилищ и коннектора

```cs
public partial class MainWindow
{
    private readonly Connector _connector;
    private const string _connectorFile = "ConnectorFile.json";
    
    // Путь к данным истории
    private readonly string _pathHistory = Paths.HistoryDataPath;
    
    private Subscription _subscription;
    private ChartCandleElement _candleElement;
    
    public MainWindow()
    {
        InitializeComponent();
        
        // Инициализируем хранилища
        var entityRegistry = new CsvEntityRegistry(_pathHistory);
        var storageRegistry = new StorageRegistry
        {
            DefaultDrive = new LocalMarketDataDrive(_pathHistory)
        };
        
        // Создаем коннектор с настроенными хранилищами
        _connector = new Connector(
            entityRegistry.Securities, 
            entityRegistry.PositionStorage, 
            new InMemoryExchangeInfoProvider(), 
            storageRegistry, 
            new SnapshotRegistry("SnapshotRegistry"));
        
        // Регистрируем провайдер адаптеров сообщений
        ConfigManager.RegisterService<IMessageAdapterProvider>(
            new InMemoryMessageAdapterProvider(_connector.Adapter.InnerAdapters));
        
        // Загружаем настройки коннектора, если файл существует
        if (File.Exists(_connectorFile))
        {
            _connector.Load(_connectorFile.Deserialize<SettingsStorage>());
        }
        
        // Устанавливаем тип данных свечей по умолчанию (5-минутки)
        CandleDataTypeEdit.DataType = TimeSpan.FromMinutes(5).TimeFrame();
    }
}
```

## Настройка подключения

```cs
// Метод для настройки параметров подключения
private void Setting_Click(object sender, RoutedEventArgs e)
{
    // Вызываем окно настройки коннектора
    if (_connector.Configure(this))
    {
        // Сохраняем настройки в файл
        _connector.Save().Serialize(_connectorFile);
    }
}

// Метод для подключения к торговой системе
private void Connect_Click(object sender, RoutedEventArgs e)
{
    // Устанавливаем коннектор как источник данных для выбора инструментов
    SecurityPicker.SecurityProvider = _connector;
    
    // Подписываемся на событие получения свечей
    _connector.CandleReceived += Connector_CandleReceived;
    
    // Подключаемся
    _connector.Connect();
}
```

## Обработка свечей и отображение на графике

```cs
// Обработчик события получения свечи
private void Connector_CandleReceived(Subscription subscription, ICandleMessage candle)
{
    // Отрисовываем свечу на графике
    Chart.Draw(_candleElement, candle);
}
```

## Создание подписки на свечи

```cs
// Метод, вызываемый при выборе инструмента
private void SecurityPicker_SecuritySelected(Security security)
{
    // Проверяем, выбран ли инструмент
    if (security == null) 
        return;
    
    // Отписываемся от предыдущей подписки, если она существует
    if (_subscription != null) 
        _connector.UnSubscribe(_subscription);
    
    // Создаем новую подписку на выбранный инструмент
    _subscription = new(CandleDataTypeEdit.DataType, security)
    {
        MarketData =
        {
            // Запрашиваем исторические данные за последние 720 дней
            From = DateTime.Today.AddDays(-720),
            
            // Режим работы: загрузка исторических данных и построение в реальном времени
            BuildMode = MarketDataBuildModes.LoadAndBuild,
        }
    };
    
    // Настраиваем график
    Chart.ClearAreas();
    
    // Создаем область графика и элемент для отображения свечей
    var area = new ChartArea();
    _candleElement = new ChartCandleElement();
    
    // Добавляем область и элемент на график
    Chart.AddArea(area);
    
    // Связываем элемент графика с подпиской для автоматической отрисовки
    Chart.AddElement(area, _candleElement, _subscription);
    
    // Запускаем подписку
    _connector.Subscribe(_subscription);
}
```

## Полный пример класса MainWindow

```cs
namespace StockSharp.Samples.Candles.CombineHistoryRealtime;

using System;
using System.Windows;
using System.IO;

using Ecng.Common;
using Ecng.Serialization;
using Ecng.Configuration;

using StockSharp.Configuration;
using StockSharp.Algo;
using StockSharp.Algo.Storages;
using StockSharp.Algo.Storages.Csv;
using StockSharp.BusinessEntities;
using StockSharp.Xaml;
using StockSharp.Messages;
using StockSharp.Xaml.Charting;
using StockSharp.Charting;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private readonly Connector _connector;
    private const string _connectorFile = "ConnectorFile.json";

    private readonly string _pathHistory = Paths.HistoryDataPath;

    private Subscription _subscription;
    private ChartCandleElement _candleElement;
    
    public MainWindow()
    {
        InitializeComponent();
        var entityRegistry = new CsvEntityRegistry(_pathHistory);
        var storageRegistry = new StorageRegistry
        {
            DefaultDrive = new LocalMarketDataDrive(_pathHistory)
        };
        _connector = new Connector(
            entityRegistry.Securities, 
            entityRegistry.PositionStorage, 
            new InMemoryExchangeInfoProvider(), 
            storageRegistry, 
            new SnapshotRegistry("SnapshotRegistry"));

        // registering all connectors
        ConfigManager.RegisterService<IMessageAdapterProvider>(
            new InMemoryMessageAdapterProvider(_connector.Adapter.InnerAdapters));

        if (File.Exists(_connectorFile))
        {
            _connector.Load(_connectorFile.Deserialize<SettingsStorage>());
        }

        CandleDataTypeEdit.DataType = TimeSpan.FromMinutes(5).TimeFrame();
    }

    private void Setting_Click(object sender, RoutedEventArgs e)
    {
        if (_connector.Configure(this))
        {
            _connector.Save().Serialize(_connectorFile);
        }
    }

    private void Connect_Click(object sender, RoutedEventArgs e)
    {
        SecurityPicker.SecurityProvider = _connector;
        _connector.CandleReceived += Connector_CandleReceived;
        _connector.Connect();
    }

    private void Connector_CandleReceived(Subscription subscription, ICandleMessage candle)
    {
        Chart.Draw(_candleElement, candle);
    }

    private void SecurityPicker_SecuritySelected(Security security)
    {
        if (security == null) return;
        if (_subscription != null) _connector.UnSubscribe(_subscription);

        _subscription = new(CandleDataTypeEdit.DataType, security)
        {
            MarketData =
            {
                From = DateTime.Today.AddDays(-720),
                BuildMode = MarketDataBuildModes.LoadAndBuild,
            }
        };

        //-----------------Chart--------------------------------
        Chart.ClearAreas();

        var area = new ChartArea();
        _candleElement = new ChartCandleElement();

        Chart.AddArea(area);
        Chart.AddElement(area, _candleElement, _subscription);

        _connector.Subscribe(_subscription);
    }
}
```

## Особенности примера

1. **Создание хранилищ**:
   - Используется [CsvEntityRegistry](xref:StockSharp.Algo.Storages.Csv.CsvEntityRegistry) для хранения сущностей
   - Настраивается [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) с указанием пути к хранилищу
   - Создается [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry) для работы со снэпшотами

2. **Создание подписки**:
   - Используется класс [Subscription](xref:StockSharp.BusinessEntities.Subscription)
   - В параметрах MarketData указывается From для определения начальной даты загрузки истории
   - Устанавливается режим BuildMode = MarketDataBuildModes.LoadAndBuild для автоматического склеивания истории и реал-тайма

3. **Отображение на графике**:
   - Используется метод Chart.AddElement для связывания элемента графика с подпиской
   - График автоматически обновляется при получении новых свечей

4. **Обработка событий**:
   - Подписка на событие CandleReceived для обработки получаемых свечей
   - Отписка от предыдущей подписки при изменении выбранного инструмента

## Расширенные возможности

Данный пример можно расширить, добавив следующие функции:

### Отслеживание перехода в режим реального времени

```cs
// Подписка на событие перехода в режим реального времени
_connector.SubscriptionOnline += OnSubscriptionOnline;

// Обработчик события
private void OnSubscriptionOnline(Subscription subscription)
{
    if (subscription == _subscription)
    {
        this.GuiAsync(() => StatusLabel.Content = "Онлайн режим");
    }
}
```

### Настройка периода загрузки истории

```cs
// Установка периода загрузки истории
private void SetHistoryPeriod(int days)
{
    if (_subscription != null)
    {
        _connector.UnSubscribe(_subscription);
        
        _subscription.MarketData.From = DateTime.Today.AddDays(-days);
        
        _connector.Subscribe(_subscription);
    }
}
```

### Сохранение полученных данных

```cs
// Метод для сохранения полученных данных
private void SaveReceivedData()
{
    if (_connector.StorageAdapter != null)
    {
        // Принудительно сохраняем кэшированные данные на диск
        _connector.StorageAdapter.Flush();
    }
}
```

### Дополнительная обработка свечей

```cs
// Расширенная обработка свечей с выводом информации
private void ExtendedCandleProcessing(Subscription subscription, ICandleMessage candle)
{
    // Отрисовываем свечу на графике
    Chart.Draw(_candleElement, candle);
    
    // Выводим информацию о свече в логи
    this.GuiAsync(() => 
    {
        var status = subscription.State == SubscriptionStates.Online ? "Реал-тайм" : "История";
        LogControl.LogMessage($"{status}: {candle.OpenTime} - O:{candle.OpenPrice} H:{candle.HighPrice} L:{candle.LowPrice} C:{candle.ClosePrice}");
    });
}
```