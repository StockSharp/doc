# Настройки тестирования

Некоторые настройки [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector).

- [MarketTimeChangedInterval](xref:StockSharp.Algo.Testing.HistoryEmulationConnector.MarketTimeChangedInterval) \- интервал прихода события о смене времени. Если используются генераторы сделок, сделки будут генерироваться с этой периодичностью. По\-умолчанию равно 1 минуте.
- [MarketEmulatorSettings.Latency](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.Latency) \- Минимальное значение задержки выставляемых заявок. По\-умолчанию равно TimeSpan.Zero, что означает мгновенное принятие биржей выставляемых заявок. 
- [MarketEmulatorSettings.MatchOnTouch](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.MatchOnTouch) \- удовлетворять заявки, если цена “коснулась” уровня (допущение иногда слишком “оптимистично” и для реалистичного тестирования следует выключить режим). Если режим выключен, то лимитные заявки будут удовлетворяться, если цена “прошла сквозь них” хотя бы на 1 шаг. Опция работает во всех режимах кроме ордер лога. По\-умолчанию выключено.

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

- Интервал обновления стакана [MarketDataGenerator.Interval](xref:StockSharp.Algo.Testing.MarketDataGenerator.Interval). Обновление не может быть чаще, чем приходят тиковые сделки (т.к. стаканы генерируются перед каждой сделкой):

  ```cs
  mdGenerator.Interval = TimeSpan.FromSeconds(1);
  				
  ```
- Глубина стаканов [MarketDepthGenerator.MaxBidsDepth](xref:StockSharp.Algo.Testing.MarketDepthGenerator.MaxBidsDepth) и [MarketDepthGenerator.MaxAsksDepth](xref:StockSharp.Algo.Testing.MarketDepthGenerator.MaxAsksDepth). Чем больше \- тем медленнее тестирование:

  ```cs
  mdGenerator.MaxAsksDepth = 1; 
  mdGenerator.MaxBidsDepth = 1;
  				
  ```
- Объемы у [MarketDepth.BestBid](xref:StockSharp.BusinessEntities.MarketDepth.BestBid) и [MarketDepth.BestAsk](xref:StockSharp.BusinessEntities.MarketDepth.BestAsk) берутся из объема сделки, по которой идет генерация. Опция [MarketDepthGenerator.UseTradeVolume](xref:StockSharp.Algo.Testing.MarketDepthGenerator.UseTradeVolume) устанавливает реалистичный объем уровня в стакане:

  ```cs
  mdGenerator.UseTradeVolume = true;
  				
  ```
- Объем уровня [MarketDataGenerator.MinVolume](xref:StockSharp.Algo.Testing.MarketDataGenerator.MinVolume) и [MarketDataGenerator.MaxVolume](xref:StockSharp.Algo.Testing.MarketDataGenerator.MaxVolume):

  ```cs
  mdGenerator.MinVolume = 1;
  mdGenerator.MaxVolume = 1;
  				
  ```
- Минимальный генерируемый спред равен [Security.PriceStep](xref:StockSharp.BusinessEntities.Security.PriceStep). Не следует генерировать спред между [MarketDepth.BestBid](xref:StockSharp.BusinessEntities.MarketDepth.BestBid) и [MarketDepth.BestAsk](xref:StockSharp.BusinessEntities.MarketDepth.BestAsk) больше чем 5 [Security.PriceStep](xref:StockSharp.BusinessEntities.Security.PriceStep) (чтобы при генерации из свечей не получалось слишком широкого спреда):

  ```cs
  mdGenerator.MinSpreadStepCount = 1;
  mdGenerator.MaxSpreadStepCount = 5;
  				
  ```
