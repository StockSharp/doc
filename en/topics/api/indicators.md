# Indicators

[S\#](../api.md) provides more than 140 standard technical analysis indicators. This allows you to use ready-made indicators rather than creating them from scratch. You can also create your own indicators based on existing ones, as shown in the [Custom Indicator](indicators/custom_indicator.md) section. All base classes for working with indicators, as well as the indicators themselves, are located in the [StockSharp.Algo.Indicators](xref:StockSharp.Algo.Indicators) namespace.

## Integrating Indicators into a Trading Algorithm

1. First, you need to create an indicator. An indicator is created like a regular .NET object:

   ```cs
   var longSma = new SimpleMovingAverage { Length = 80 };
   var shortSma = new SimpleMovingAverage { Length = 30 };
   
   // It's recommended to add indicators to the strategy collection
   Indicators.Add(longSma);
   Indicators.Add(shortSma);
   ```

2. Next, you need to process market data for the indicators. The most efficient approach is to use the result returned by the [Process](xref:StockSharp.Algo.Indicators.IIndicator.Process(StockSharp.Algo.Indicators.IIndicatorValue)) method:

   ```cs
   private void ProcessCandle(ICandleMessage candle)
   {
       // Process the candle with indicators and immediately save the results
       var longValue = longSma.Process(candle);
       var shortValue = shortSma.Process(candle);
       
       // Use the results for trading decisions
       if (shortValue.GetValue<decimal>() > longValue.GetValue<decimal>())
       {
           // Buy signal
           BuyAtMarket();
       }
   }
   ```

   An indicator accepts [IIndicatorValue](xref:StockSharp.Algo.Indicators.IIndicatorValue) as input. Some indicators operate with a simple number, such as [SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage). Others require a complete candle, such as [MedianPrice](xref:StockSharp.Algo.Indicators.MedianPrice). Therefore, input values need to be cast either to [DecimalIndicatorValue](xref:StockSharp.Algo.Indicators.DecimalIndicatorValue) or to [CandleIndicatorValue](xref:StockSharp.Algo.Indicators.CandleIndicatorValue). The indicator's resulting value follows the same rules as the input value.

3. Both the resulting and input values of the indicator have the [IIndicatorValue.IsFinal](xref:StockSharp.Algo.Indicators.IIndicatorValue.IsFinal) property, which indicates that the value is final and the indicator will not change at this point in time. For example, the [SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage) indicator is formed based on the candle's closing price, but at the current moment, the final closing price is unknown and changing. In this case, the resulting value of [IIndicatorValue.IsFinal](xref:StockSharp.Algo.Indicators.IIndicatorValue.IsFinal) will be false. If you pass a completed candle to the indicator, both the input and resulting values of [IIndicatorValue.IsFinal](xref:StockSharp.Algo.Indicators.IIndicatorValue.IsFinal) will be true.

4. **Recommended approach**: directly use the values obtained from calling the [Process](xref:StockSharp.Algo.Indicators.IIndicator.Process(StockSharp.Algo.Indicators.IIndicatorValue)) method, instead of subsequently calling [GetCurrentValue](xref:StockSharp.Algo.Indicators.IndicatorHelper.GetCurrentValue(StockSharp.Algo.Indicators.IIndicator)):

   ```cs
   // Example of a strategy with two moving averages
   private void ProcessCandle(ICandleMessage candle)
   {
       // Process the candle with indicators and immediately save the results
       var longValue = _longSma.Process(candle);
       var shortValue = _shortSma.Process(candle);
       
       // Draw on the chart
       DrawCandlesAndIndicators(candle, longValue, shortValue);
       
       if (!IsFormedAndOnlineAndAllowTrading()) 
           return;
           
       // Use the obtained values for comparison
       var isShortLessCurrent = shortValue.GetValue<decimal>() < longValue.GetValue<decimal>();
       var isShortLessPrev = _shortSma.GetValue(1) < _longSma.GetValue(1);
       
       // Check if a crossover occurred
       if (isShortLessCurrent == isShortLessPrev) 
           return;
       
       var volume = Volume + Math.Abs(Position);
       
       // Trading actions based on the signal
       if (isShortLessCurrent)
           SellMarket(volume);
       else
           BuyMarket(volume);
   }
   ```

   This approach has the following advantages:
   - It aligns with the streaming data processing model (receive → process → use the result)
   - It's more efficient as it avoids repeatedly accessing the container of accumulated values
   - It eliminates potential synchronization issues between calling Process and subsequent GetCurrentValue calls

5. Not recommended approach (less efficient):

   ```cs
   // Suboptimal approach
   foreach (var candle in candles)
   {
       // Process the candle but ignore the returned value
       _longSma.Process(candle);
       _shortSma.Process(candle);
   }
   
   // Later try to get values via GetCurrentValue()
   var isShortLessThenLong = _shortSma.GetCurrentValue() < _longSma.GetCurrentValue();
   ```
   
   With this approach, there's an additional access to the container of historical indicator values, which introduces delays and disrupts the streaming model of data processing.

6. All indicators have the [BaseIndicator.IsFormed](xref:StockSharp.Algo.Indicators.BaseIndicator.IsFormed) property, which indicates whether the indicator is ready for use. For example, the [SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage) indicator has a period, and until the indicator processes a number of candles equal to the indicator's period, the indicator will be considered not ready for use. And the [BaseIndicator.IsFormed](xref:StockSharp.Algo.Indicators.BaseIndicator.IsFormed) property will be false.

## Example of a Complete Moving Average Strategy

Below is an example of a strategy that correctly uses indicators, processing candles and utilizing the results of the Process method:

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

        // Initialize strategy parameters
        _longSmaLength = Param(nameof(LongSmaLength), 80);
        _shortSmaLength = Param(nameof(ShortSmaLength), 30);
        _series = Param(nameof(Series), DataType.TimeFrame(TimeSpan.FromMinutes(15)));
    }

    protected override void OnStarted(DateTimeOffset time)
    {
        base.OnStarted(time);

        // Create indicators
        _shortSma = new SimpleMovingAverage { Length = _shortSmaLength.Value };
        _longSma = new SimpleMovingAverage { Length = _longSmaLength.Value };

        // Add indicators to the strategy collection
        Indicators.Add(_shortSma);
        Indicators.Add(_longSma);

        // Initialize chart
        _chart = GetChart();
        if (_chart != null)
        {
            InitChart();
        }
        
        // Subscribe to candles
        var subscription = new Subscription(_series.Value, Security);

        Connector
            .WhenCandlesFinished(subscription)
            .Do(ProcessCandle)
            .Apply(this);

        Connector.Subscribe(subscription);
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        // Process the candle with indicators and save the results
        var longValue = _longSma.Process(candle);
        var shortValue = _shortSma.Process(candle);
        
        // Draw on the chart
        DrawCandlesAndIndicators(candle, longValue, shortValue);
        
        // Check conditions for trading
        if (!IsFormedAndOnlineAndAllowTrading()) 
            return;

        // Compare current and previous indicator values
        var isShortLessCurrent = shortValue.GetValue<decimal>() < longValue.GetValue<decimal>();
        var isShortLessPrev = _shortSma.GetValue(1) < _longSma.GetValue(1);

        // Check for crossover
        if (isShortLessCurrent == isShortLessPrev) 
            return;

        var volume = Volume + Math.Abs(Position);

        // Trading actions based on the signal
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

    // Other chart initialization methods omitted for brevity
}
```

This example demonstrates the correct approach to working with indicators in the StockSharp streaming model.
