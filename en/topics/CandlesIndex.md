# Candle creation by the instruments basket

To create [ContinuousSecurity](../api/StockSharp.Algo.ContinuousSecurity.html), [WeightedIndexSecurity](../api/StockSharp.Algo.WeightedIndexSecurity.html) or [ExpressionIndexSecurity](../api/StockSharp.Algo.Expressions.ExpressionIndexSecurity.html) candles, the same procedure is used as in the creation of the [Security](../api/StockSharp.BusinessEntities.Security.html) candles.

For example, create 1 min candles for spread APM5 \- ESM5:

```cs
private Connector \_connector;
private Security \_instr1;
private Security \_instr2;
private WeightedIndexSecurity \_indexInstr;
private const string \_secCode1 \= "APM5";
private const string \_secCode2 \= "ESM5";
private CandleSeries \_indexSeries;
readonly TimeSpan \_timeFrame \= TimeSpan.FromMinutes(1);
private ChartArea \_area;
private ChartCandleElement \_candlesElem;
...
if (\_connector.Configure(this))
			{
				new XmlSerializer\<SettingsStorage\>().Serialize(\_connector.Save(), \_connectorFile);
			}
			
...
\_area \= new ChartArea();
\_chart.Areas.Add(\_area);
\_candlesElem \= new ChartCandleElement();
\_area.Candles.Add(\_candlesElem);
...
\_connector.CandleSeriesProcessing +\= Connector\_CandleSeriesProcessing;
....
ConfigManager.RegisterService\<ISecurityProvider\>(\_connector);
ConfigManager.RegisterService\<ICompilerService\>(new RoslynCompilerService());
...
\_indexInstr \= new WeightedIndexSecurity() { Board \= ExchangeBoard.Nyse, Id \= "IndexInstr" };
\_indexInstr.Weights.Add(\_instr1, 1);
\_indexInstr.Weights.Add(\_instr2, \-1);
\_indexSeries \=
	new CandleSeries(typeof(TimeFrameCandle), \_indexInstr, \_timeFrame)
	{
		BuildCandlesMode \= MarketDataBuildModes.Build,
		BuildCandlesFrom \= MarketDataTypes.Trades,
	};
...
\_connector.SubscribeCandles(\_indexSeries, DateTime.Today.Subtract(TimeSpan.FromDays(30)), DateTime.Now);			
		
```

After that, the [Connector.CandleSeriesProcessing](../api/StockSharp.Algo.Connector.CandleSeriesProcessing.html) \- DrawCandles event handler will receive the candles, which can be displayed on the chart:

```cs
private void Connector\_CandleSeriesProcessing(CandleSeries candleSeries, Candle candle)
{
    if (candleSeries \=\= \_indexSeries) 
       {
          var chartData \= new ChartDrawData();
          chartData.Group(candle.OpenTime).Add(\_candleElement, candle);
          \_chart.Draw(chartData);
       }
}
		
```

## Recommended content

[Continuous futures](SecurityContinuous.md)

[Index](SecurityIndex.md)
