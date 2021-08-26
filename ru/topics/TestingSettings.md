# Настройки тестирования

Некоторые настройки [HistoryEmulationConnector](../api/StockSharp.Algo.Testing.HistoryEmulationConnector.html).

- [EmulationSettings.MarketTimeChangedInterval](../api/StockSharp.Algo.Strategies.Testing.EmulationSettings.MarketTimeChangedInterval.html) \- интервал прихода события о смене времени. Если используются генераторы сделок, сделки будут генерироваться с этой периодичностью. По\-умолчанию равно 1 минуте.
- [MarketEmulatorSettings.Latency](../api/StockSharp.Algo.Testing.MarketEmulatorSettings.Latency.html) \- Минимальное значение задержки выставляемых заявок. По\-умолчанию равно TimeSpan.Zero, что означает мгновенное принятие биржей выставляемых заявок. 
- [MarketEmulatorSettings.MatchOnTouch](../api/StockSharp.Algo.Testing.MarketEmulatorSettings.MatchOnTouch.html) \- удовлетворять заявки, если цена “коснулась” уровня (допущение иногда слишком “оптимистично” и для реалистичного тестирования следует выключить режим). Если режим выключен, то лимитные заявки будут удовлетворяться, если цена “прошла сквозь них” хотя бы на 1 шаг. Опция работает во всех режимах кроме ордер лога. По\-умолчанию выключено.

Даже если стратегия тестируется на свечах, нужно подписываться на тиковые сделки:

```cs
		_connector.SubscribeTrades(security);
		
```

Если для стратегии нужны стаканы, нужно подписываться на стаканы:

```cs
		_connector.SubscribeMarketDepth(security);
		
```

Если стаканов нет, то для проверки работоспособности стратегий, нуждающихся в стаканах, можно включить генерацию:

```cs
var mdGenerator = new TrendMarketDepthGenerator(connector.GetSecurityId(security));
_connector.MarketDataAdapter.SendInMessage(new GeneratorMessage
{
    IsSubscribe = true,
    Generator = mdGeneratorб
});
		
```

- Интервал обновления стакана [MarketDataGenerator.Interval](../api/StockSharp.Algo.Testing.MarketDataGenerator.Interval.html). Обновление не может быть чаще, чем приходят тиковые сделки (т.к. стаканы генерируются перед каждой сделкой):

  ```cs
  mdGenerator.Interval = TimeSpan.FromSeconds(1);
  				
  ```
- Глубина стаканов [MarketDepthGenerator.MaxBidsDepth](../api/StockSharp.Algo.Testing.MarketDepthGenerator.MaxBidsDepth.html) и [MarketDepthGenerator.MaxAsksDepth](../api/StockSharp.Algo.Testing.MarketDepthGenerator.MaxAsksDepth.html). Чем больше \- тем медленнее тестирование:

  ```cs
  mdGenerator.MaxAsksDepth = 1; 
  mdGenerator.MaxBidsDepth = 1;
  				
  ```
- Объемы у [MarketDepth.BestBid](../api/StockSharp.BusinessEntities.MarketDepth.BestBid.html) и [MarketDepth.BestAsk](../api/StockSharp.BusinessEntities.MarketDepth.BestAsk.html) берутся из объема сделки, по которой идет генерация. Опция [MarketDepthGenerator.UseTradeVolume](../api/StockSharp.Algo.Testing.MarketDepthGenerator.UseTradeVolume.html) устанавливает реалистичный объем уровня в стакане:

  ```cs
  mdGenerator.UseTradeVolume = true;
  				
  ```
- Объем уровня [MarketDataGenerator.MinVolume](../api/StockSharp.Algo.Testing.MarketDataGenerator.MinVolume.html) и [MarketDataGenerator.MaxVolume](../api/StockSharp.Algo.Testing.MarketDataGenerator.MaxVolume.html):

  ```cs
  mdGenerator.MinVolume = 1;
  mdGenerator.MaxVolume = 1;
  				
  ```
- Минимальный генерируемый спред равен [Security.PriceStep](../api/StockSharp.BusinessEntities.Security.PriceStep.html). Не следует генерировать спред между [MarketDepth.BestBid](../api/StockSharp.BusinessEntities.MarketDepth.BestBid.html) и [MarketDepth.BestAsk](../api/StockSharp.BusinessEntities.MarketDepth.BestAsk.html) больше чем 5 [Security.PriceStep](../api/StockSharp.BusinessEntities.Security.PriceStep.html) (чтобы при генерации из свечей не получалось слишком широкого спреда):

  ```cs
  mdGenerator.MinSpreadStepCount = 1;
  mdGenerator.MaxSpreadStepCount = 5;
  				
  ```
