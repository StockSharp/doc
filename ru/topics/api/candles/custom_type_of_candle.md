# Собственный тип свечей

[S\#](../../api.md) позволяет расширить возможности построения свечей, давая возможность работать с произвольными типами свечей. Это полезно в тех случаях, когда требуется работать со свечами, не поддерживаемыми в данный момент [S\#](../../api.md). Ниже описан процесс создания собственного типа свечей на примере Delta-свечей (свечи, формирующиеся на основе разницы между объемом покупок и продаж).

## Реализация Delta-свечей

1. Сначала необходимо создать свой тип сообщения свечи. Тип должен наследоваться от класса [CandleMessage](xref:StockSharp.Messages.CandleMessage):

   ```cs
   /// <summary>
   /// Свеча, формирующаяся на основе дельты объемов покупок и продаж.
   /// </summary>
   public class DeltaCandleMessage : CandleMessage
   {
       // Идентификатор типа сообщения берем из хелпера
       // чтобы использовать одно и то же значение и в RegisterCandleType
       
       /// <summary>
       /// Инициализировать новый экземпляр <see cref="DeltaCandleMessage"/>.
       /// </summary>
       public DeltaCandleMessage()
           : base(DeltaCandleHelper.DeltaCandleType)
       {
       }
       
       /// <summary>
       /// Пороговое значение дельты для формирования свечи.
       /// </summary>
       public decimal DeltaThreshold { get; set; }
       
       /// <summary>
       /// Текущее значение дельты.
       /// </summary>
       public decimal CurrentDelta { get; set; }
       
       /// <summary>
       /// Создать копию <see cref="DeltaCandleMessage"/>.
       /// </summary>
       /// <returns>Копия.</returns>
       public override Message Clone()
       {
           return CopyTo(new DeltaCandleMessage
           {
               DeltaThreshold = DeltaThreshold,
               CurrentDelta = CurrentDelta
           });
       }
       
       /// <summary>
       /// Параметр свечи.
       /// </summary>
       public override object Arg
       {
           get => DeltaThreshold;
           set => DeltaThreshold = (decimal)value;
       }
       
       /// <summary>
       /// Тип аргумента свечи.
       /// </summary>
       public override Type ArgType => typeof(decimal);
   }
   ```

2. Затем необходимо создать собственный тип данных в классе [DataType](xref:StockSharp.Messages.DataType):

   ```cs
   public static class DeltaCandleHelper
   {
       /// <summary>
       /// Определяем уникальный MessageType для дельта-свечей.
       /// </summary>
       public const MessageTypes DeltaCandleType = (MessageTypes)10001;
       
       /// <summary>
       /// <see cref="DeltaCandleMessage"/> тип данных.
       /// </summary>
       public static readonly DataType CandleDelta = 
           DataType.Create(typeof(DeltaCandleMessage)).Immutable();
       
       /// <summary>
       /// Создать тип данных для дельта-свечей.
       /// </summary>
       /// <param name="threshold">Пороговое значение дельты.</param>
       /// <returns>Тип данных.</returns>
       public static DataType Delta(this decimal threshold)
       {
           return DataType.Create(typeof(DeltaCandleMessage), threshold);
       }
       
       /// <summary>
       /// Регистрация типа дельта-свечей в системе.
       /// </summary>
       public static void RegisterDeltaCandleType()
       {
           // Регистрируем новый тип свечей в StockSharp
           Extensions.RegisterCandleType<decimal>(
               typeof(DeltaCandleMessage),      // Тип сообщения свечи
               DeltaCandleType,                // Тип сообщения
               "delta",                        // Имя файла для хранения
               str => str.To<decimal>(),       // Конвертер из строки в параметр
               arg => arg.ToString(),          // Конвертер из параметра в строку
               a => a > 0,                     // Валидатор параметра
               false                           // Можно ли получать такие свечи из источника (не только строить)
           );
       }
   }
   ```

3. Далее требуется создать построителя свечей для нового типа. Для этого создаем реализацию [CandleBuilder\<TCandleMessage\>](xref:StockSharp.Algo.Candles.Compression.CandleBuilder`1):

   ```cs
   /// <summary>
   /// Построитель свечей типа <see cref="DeltaCandleMessage"/>.
   /// </summary>
   public class DeltaCandleBuilder : CandleBuilder<DeltaCandleMessage>
   {
       /// <summary>
       /// Инициализирует новый экземпляр <see cref="DeltaCandleBuilder"/>.
       /// </summary>
       /// <param name="exchangeInfoProvider">Провайдер информации о биржах.</param>
       public DeltaCandleBuilder(IExchangeInfoProvider exchangeInfoProvider)
           : base(exchangeInfoProvider)
       {
       }
       
       /// <inheritdoc />
       protected override DeltaCandleMessage CreateCandle(ICandleBuilderSubscription subscription, ICandleBuilderValueTransform transform)
       {
           var time = transform.Time;
           
           return FirstInitCandle(subscription, new DeltaCandleMessage
           {
               DeltaThreshold = subscription.Message.GetArg<decimal>(),
               OpenTime = time,
               CloseTime = time,
               HighTime = time,
               LowTime = time,
               CurrentDelta = 0
           }, transform);
       }
       
       /// <inheritdoc />
       protected override bool IsCandleFinishedBeforeChange(ICandleBuilderSubscription subscription, DeltaCandleMessage candle, ICandleBuilderValueTransform transform)
       {
           // Свеча закрывается, когда абсолютное значение дельты превышает порог
           return Math.Abs(candle.CurrentDelta) >= candle.DeltaThreshold;
       }
       
       /// <inheritdoc />
       protected override void UpdateCandle(ICandleBuilderSubscription subscription, DeltaCandleMessage candle, ICandleBuilderValueTransform transform)
       {
           base.UpdateCandle(subscription, candle, transform);
           
           // Обновляем дельту в зависимости от стороны сделки
           if (transform.Side == Sides.Buy)
               candle.CurrentDelta += transform.Volume ?? 0;
           else if (transform.Side == Sides.Sell)
               candle.CurrentDelta -= transform.Volume ?? 0;
       }
   }
   ```

4. Затем необходимо зарегистрировать построитель свечей в [CandleBuilderProvider](xref:StockSharp.Algo.Candles.Compression.CandleBuilderProvider):

   ```cs
   private Connector _connector;
   ...
   // Регистрируем тип дельта-свечей в системе
   DeltaCandleHelper.RegisterDeltaCandleType();
   
   // Регистрируем построитель дельта-свечей
   _connector.Adapter.CandleBuilderProvider.Register(new DeltaCandleBuilder(_connector.ExchangeInfoProvider));
   ```

5. Создаем подписку на свечи типа `DeltaCandleMessage` и запрашиваем по ней данные:

   ```cs
   // Пороговое значение дельты
   decimal deltaThreshold = 1000m;
   
   // Создаем подписку на дельта-свечи
   var subscription = new Subscription(
       // Используем наш расширяющий метод для создания типа данных
       deltaThreshold.Delta(), 
       security)
   {
       MarketData =
       {
           // Указываем, что свечи будут строиться из тиков
           BuildMode = MarketDataBuildModes.Build,
           BuildFrom = DataType.Ticks
       }
   };
   
   // Подписываемся на событие получения свечей
   _connector.CandleReceived += (sub, candle) =>
   {
       if (sub != subscription)
           return;
       
       var deltaCandle = (DeltaCandleMessage)candle;
       
       // Обработка дельта-свечи
       Console.WriteLine($"Дельта-свеча {candle.OpenTime}: O:{candle.OpenPrice} H:{candle.HighPrice} " +
                        $"L:{candle.LowPrice} C:{candle.ClosePrice} V:{candle.TotalVolume} Delta:{deltaCandle.CurrentDelta}");
   };
   
   // Подписываемся на переход в онлайн-режим
   _connector.SubscriptionOnline += sub => 
   {
       if (sub == subscription)
           Console.WriteLine("Подписка на дельта-свечи перешла в онлайн режим");
   };
   
   // Запускаем подписку
   _connector.Subscribe(subscription);
   ```

## Использование дельта-свечей в торговых стратегиях

Пример простой стратегии, использующей дельта-свечи:

```cs
public class DeltaCandleStrategy : Strategy
{
    private readonly StrategyParam<decimal> _deltaThreshold;
    private readonly StrategyParam<decimal> _volume;
    private readonly StrategyParam<decimal> _signalDelta;

    private IChart _chart;
    private IChartCandleElement _chartCandleElement;
    private IChartIndicatorElement _deltaIndicatorElement;

    public decimal DeltaThreshold
    {
        get => _deltaThreshold.Value;
        set => _deltaThreshold.Value = value;
    }

    public decimal Volume
    {
        get => _volume.Value;
        set => _volume.Value = value;
    }

    public decimal SignalDelta
    {
        get => _signalDelta.Value;
        set => _signalDelta.Value = value;
    }

    public DeltaCandleStrategy()
    {
        // Параметры стратегии
        _deltaThreshold = Param(nameof(DeltaThreshold), 1000m)
            .SetDisplay("Пороговое значение дельты", "Значение дельты объема для формирования свечи", "Основные настройки")
            .SetValidator(new DecimalGreaterThanZeroAttribute())
            .SetCanOptimize(true)
            .SetOptimize(500m, 2000m, 100m);

        _volume = Param(nameof(Volume), 1m)
            .SetDisplay("Объем заявки", "Объем для торговых операций", "Основные настройки")
            .SetValidator(new DecimalGreaterThanZeroAttribute());

        _signalDelta = Param(nameof(SignalDelta), 500m)
            .SetDisplay("Минимальная дельта для сигнала", "Минимальное значение дельты для генерации сигнала", "Основные настройки")
            .SetValidator(new DecimalGreaterThanZeroAttribute())
            .SetCanOptimize(true);

        Name = "DeltaCandleStrategy";
    }

    protected override void OnStarted(DateTimeOffset time)
    {
        base.OnStarted(time);

        // Инициализация графика, если он доступен
        _chart = GetChart();
        if (_chart != null)
        {
            var area = _chart.AddArea();
            _chartCandleElement = area.AddCandles();
            _deltaIndicatorElement = area.AddIndicator();
            _deltaIndicatorElement.DrawStyle = DrawStyles.Histogram;
            _deltaIndicatorElement.Color = System.Drawing.Color.Purple;
        }

        // Создаем подписку на дельта-свечи
        var subscription = new Subscription(DeltaThreshold.Delta(), Security)
        {
            MarketData =
            {
                BuildMode = MarketDataBuildModes.Build,
                BuildFrom = DataType.Ticks
            }
        };

        // Создаем правило для обработки дельта-свечей
        this
            .WhenCandleReceived(subscription)
            .Do(ProcessDeltaCandle)
            .Apply(this);

        // Запускаем подписку
        Subscribe(subscription);
    }

    private void ProcessDeltaCandle(ICandleMessage candle)
    {
        // Отрисовка на графике, если он доступен
        if (_chart != null)
        {
            var deltaCandle = (DeltaCandleMessage)candle;
            
            var data = _chart.CreateData();
            data.Group(candle.OpenTime)
                .Add(_chartCandleElement, candle)
                .Add(_deltaIndicatorElement, deltaCandle.CurrentDelta);
                
            _chart.Draw(data);
        }

        // Обрабатываем только завершенные свечи
        if (candle.State != CandleStates.Finished)
            return;
            
        var deltaCandle = (DeltaCandleMessage)candle;
        
        // Проверяем, достаточно ли дельта для сигнала
        if (Math.Abs(deltaCandle.CurrentDelta) < SignalDelta)
        {
            this.AddInfoLog($"Дельта {deltaCandle.CurrentDelta} меньше порогового значения {SignalDelta}. Сигнал не генерируется.");
            return;
        }

        // Направление операции зависит от знака дельты
        var direction = deltaCandle.CurrentDelta > 0 ? Sides.Buy : Sides.Sell;
        
        this.AddInfoLog($"Дельта-свеча завершена. Дельта: {deltaCandle.CurrentDelta}. Направление: {direction}");
        
        // Для определения цены используем цену закрытия свечи
        var price = deltaCandle.ClosePrice;
        var volume = Volume;
        
        // Если у нас уже есть позиция в противоположном направлении, 
        // увеличиваем объем для закрытия существующей позиции
        if ((Position < 0 && direction == Sides.Buy) || 
            (Position > 0 && direction == Sides.Sell))
        {
            volume = Math.Max(volume, Math.Abs(Position) + volume);
        }
        
        // Регистрируем заявку
        RegisterOrder(this.CreateOrder(direction, price, volume));
    }
}
```

## Важные моменты при создании собственных типов свечей

1. **Уникальность MessageTypes** — убедитесь, что выбранный вами идентификатор `MessageTypes` не конфликтует с существующими типами в StockSharp. Рекомендуется использовать значения больше 10000 для пользовательских типов.

2. **Регистрация типа свечей** — регистрация через `Extensions.RegisterCandleType` необходима для правильной интеграции с графическими контролами StockSharp и хранилищем данных. Без регистрации тип свечей будет работать только в коде, но не будет доступен в пользовательском интерфейсе.

3. **Параметр свечи** — реализуйте свойство `ArgType`, которое возвращает тип аргумента свечи. Это используется для корректного отображения параметров в графическом интерфейсе.

4. **Файловая система** — параметр `fileName` в методе `RegisterCandleType` используется для сохранения свечей в файловой системе, когда вы используете хранилище данных StockSharp.

5. **Валидация параметров** — метод валидации параметров используется в StockSharp для проверки корректности значений перед созданием подписки.

Таким образом, мы создали полностью собственный тип свечей, который правильно интегрируется со всей экосистемой StockSharp (включая пользовательский интерфейс и хранилище данных) и может использоваться для построения торговых стратегий, основанных на анализе дельты объемов.