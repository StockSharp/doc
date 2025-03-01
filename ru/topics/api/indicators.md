# Индикаторы

[S\#](../api.md) стандартно предоставляет более 140 индикаторов технического анализа. Это позволяет не создавать с нуля нужные индикаторы, а использовать уже готовые. Кроме того можно создавать собственные индикаторы, взяв за основу существующие, как показано в разделе [Собственный индикатор](indicators/custom_indicator.md). Все базовые классы для работы с индикаторами, а также сами индикаторы находятся в пространстве имен [StockSharp.Algo.Indicators](xref:StockSharp.Algo.Indicators). 

## Подключение индикатора в робот

1. В самом начале нужно создать индикатор. Индикатор создается как и обычный .NET объект:

   ```cs
   var longSma = new SimpleMovingAverage { Length = 80 };
   var shortSma = new SimpleMovingAverage { Length = 30 };
   
   // Рекомендуется добавить индикаторы в коллекцию стратегии
   Indicators.Add(longSma);
   Indicators.Add(shortSma);
   ```

2. Далее, необходимо обрабатывать маркет-данные для индикаторов. Наиболее эффективный способ - это использование результата метода [Process](xref:StockSharp.Algo.Indicators.IIndicator.Process(StockSharp.Algo.Indicators.IIndicatorValue)):

   ```cs
   private void ProcessCandle(ICandleMessage candle)
   {
       // Обрабатываем свечу индикаторами и сразу сохраняем результат
       var longValue = longSma.Process(candle);
       var shortValue = shortSma.Process(candle);
       
       // Используем результаты для принятия торговых решений
       if (shortValue.GetValue<decimal>() > longValue.GetValue<decimal>())
       {
           // Сигнал на покупку
           BuyAtMarket();
       }
   }
   ```

   Индикатор принимает на вход [IIndicatorValue](xref:StockSharp.Algo.Indicators.IIndicatorValue). Некоторые из индикаторов оперируют простым числом, как, например, [SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage). Другим требуются полностью свеча, как, например, [MedianPrice](xref:StockSharp.Algo.Indicators.MedianPrice). Поэтому входящие значения необходимо приводить или к [DecimalIndicatorValue](xref:StockSharp.Algo.Indicators.DecimalIndicatorValue) или к [CandleIndicatorValue](xref:StockSharp.Algo.Indicators.CandleIndicatorValue). Результирующее значение индикатора работает по тем же правилам, что и входящее значение. 

3. Результирующее и входящее значение индикатора имеют свойство [IIndicatorValue.IsFinal](xref:StockSharp.Algo.Indicators.IIndicatorValue.IsFinal), которое говорит о том, что значение является окончательным и индикатор не будет изменяться в данной точке времени. Например, индикатор [SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage) формируется по цене закрытия свечи, но в текущий момент времени окончательная цена закрытия свечи неизвестна и меняется. В таком случае результирующее значение [IIndicatorValue.IsFinal](xref:StockSharp.Algo.Indicators.IIndicatorValue.IsFinal) будет false. Eсли в индикатор передать законченную свечу, то входящее и результирующее значения [IIndicatorValue.IsFinal](xref:StockSharp.Algo.Indicators.IIndicatorValue.IsFinal) будут true.

4. **Рекомендуемый подход**: непосредственно использовать значения, полученные в результате вызова метода [Process](xref:StockSharp.Algo.Indicators.IIndicator.Process(StockSharp.Algo.Indicators.IIndicatorValue)), вместо последующего обращения к [GetCurrentValue](xref:StockSharp.Algo.Indicators.IndicatorHelper.GetCurrentValue(StockSharp.Algo.Indicators.IIndicator)): 

   ```cs
   // Пример стратегии с двумя скользящими средними
   private void ProcessCandle(ICandleMessage candle)
   {
       // Обрабатываем свечу индикаторами и сразу сохраняем результаты
       var longValue = _longSma.Process(candle);
       var shortValue = _shortSma.Process(candle);
       
       // Рисуем на графике
       DrawCandlesAndIndicators(candle, longValue, shortValue);
       
       if (!IsFormedAndOnlineAndAllowTrading()) 
           return;
           
       // Используем полученные значения для сравнения
       var isShortLessCurrent = shortValue.GetValue<decimal>() < longValue.GetValue<decimal>();
       var isShortLessPrev = _shortSma.GetValue(1) < _longSma.GetValue(1);
       
       // Проверяем, произошло ли пересечение
       if (isShortLessCurrent == isShortLessPrev) 
           return;
       
       var volume = Volume + Math.Abs(Position);
       
       // Торговые действия на основе сигнала
       if (isShortLessCurrent)
           SellMarket(volume);
       else
           BuyMarket(volume);
   }
   ```

   Такой подход имеет следующие преимущества:
   - Соответствует стриминговой модели обработки данных (получили → обработали → использовали результат)
   - Более производительный, так как избегает повторного обращения к контейнеру накопленных значений
   - Устраняет возможные рассинхронизации между вызовом Process и последующим GetCurrentValue

5. Не рекомендуемый подход (менее эффективный):

   ```cs
   // Неоптимальный подход
   foreach (var candle in candles)
   {
       // Обработали свечу, но игнорируем возвращаемое значение
       _longSma.Process(candle);
       _shortSma.Process(candle);
   }
   
   // Позже пытаемся получить значения через GetCurrentValue()
   var isShortLessThenLong = _shortSma.GetCurrentValue() < _longSma.GetCurrentValue();
   ```
   
   При таком подходе происходит дополнительное обращение к контейнеру исторических значений индикатора, что вносит задержки и нарушает стриминговую модель обработки данных.

6. У всех индикаторов есть свойство [BaseIndicator.IsFormed](xref:StockSharp.Algo.Indicators.BaseIndicator.IsFormed), которое говорит о том готов ли индикатор к использованию. Например, индикатор [SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage) имеет период, и пока индикатор не обработает количество свечей, равное периоду индикатора, индикатор будет считаться не готовым к использованию. И свойство [BaseIndicator.IsFormed](xref:StockSharp.Algo.Indicators.BaseIndicator.IsFormed) будет false.

## Пример полной стратегии на скользящих средних

Ниже приведен пример стратегии, которая правильно использует индикаторы, обрабатывая свечи и используя результаты метода Process:

```cs
public class SmaStrategy : Strategy
{
    private readonly StrategyParam<DataType> _series;
    private readonly StrategyParam<int> _longSmaLength;
    private readonly StrategyParam<int> _shortSmaLength;

    private SimpleMovingAverage _longSma;
    private SimpleMovingAverage _shortSma;

    private IChartIndicatorElement _longSmaIndicatorElement;
    private IChartIndicatorElement _shortSmaIndicatorElement;
    private IChartCandleElement _chartCandleElement;
    private IChartTradeElement _tradesElem;
    private IChart _chart;

    public SmaStrategy()
    {
        base.Name = "SMA strategy";

        // Инициализация параметров стратегии
        _longSmaLength = Param(nameof(LongSmaLength), 80);
        _shortSmaLength = Param(nameof(ShortSmaLength), 30);
        _series = Param(nameof(Series), DataType.TimeFrame(TimeSpan.FromMinutes(15)));
    }

    protected override void OnStarted(DateTimeOffset time)
    {
        base.OnStarted(time);

        // Создание индикаторов
        _shortSma = new SimpleMovingAverage { Length = _shortSmaLength.Value };
        _longSma = new SimpleMovingAverage { Length = _longSmaLength.Value };

        // Добавление индикаторов в коллекцию стратегии
        Indicators.Add(_shortSma);
        Indicators.Add(_longSma);

        // Инициализация графика
        _chart = GetChart();
        if (_chart != null)
        {
            InitChart();
        }
        
        // Подписка на свечи
        var subscription = new Subscription(_series.Value, Security);

        Connector
            .WhenCandlesFinished(subscription)
            .Do(ProcessCandle)
            .Apply(this);

        Connector.Subscribe(subscription);
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        // Обработка свечи индикаторами и сохранение результатов
        var longValue = _longSma.Process(candle);
        var shortValue = _shortSma.Process(candle);
        
        // Отрисовка на графике
        DrawCandlesAndIndicators(candle, longValue, shortValue);
        
        // Проверка условий для торговли
        if (!IsFormedAndOnlineAndAllowTrading()) 
            return;

        // Сравнение текущих и предыдущих значений индикаторов
        var isShortLessCurrent = shortValue.GetValue<decimal>() < longValue.GetValue<decimal>();
        var isShortLessPrev = _shortSma.GetValue(1) < _longSma.GetValue(1);

        // Проверка на пересечение
        if (isShortLessCurrent == isShortLessPrev) 
            return;

        var volume = Volume + Math.Abs(Position);

        // Торговые действия на основе сигнала
        if (isShortLessCurrent)
            SellMarket(volume);
        else
            BuyMarket(volume);
    }

    private void DrawCandlesAndIndicators(ICandleMessage candle, IIndicatorValue longSma, IIndicatorValue shortSma)
    {
        if (_chart == null) return;
        var data = _chart.CreateData();
        data.Group(candle.OpenTime)
            .Add(_chartCandleElement, candle)
            .Add(_longSmaIndicatorElement, longSma)
            .Add(_shortSmaIndicatorElement, shortSma);
        _chart.Draw(data);
    }

    // Другие методы инициализации графика опущены для краткости
}
```

Этот пример демонстрирует правильный подход к работе с индикаторами в стриминговой модели StockSharp.
