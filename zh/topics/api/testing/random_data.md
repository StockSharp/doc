# 随机数据

在随机数据上进行测试是一种特殊的测试。它并不旨在寻找最优参数。相反，这种测试可以通过将交易算法暴露于各种交易所场景来检测代码中的错误。

通常情况下，算法开发只使用一定的场景集。因此，当出现特殊情况时，算法可能反应不正确或抛出异常。例如，可能出现以下情况：

- 该策略适用于 [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage) K线，并期望在每次迭代中，总会有一根对应请求时间周期的 K线。当在某个周期内没有任何交易时，K线就不会生成。因此，如果没有适当的处理，将会抛出 [NullReferenceException](xref:System.NullReferenceException)，并导致策略停止运行。
- 该策略适用于非流动性交易品种，并使用 [IOrderBookMessage](xref:StockSharp.Messages.IOrderBookMessage)。该策略假设订单簿始终被填充。在某些时候，订单簿可能只填充了一半（例如，有买盘，但没有卖盘）。如果策略没有预料到这种情况，那么它要么会错误地注册订单，要么会抛出异常并导致策略停止。
- 该策略计算价格水平。代码的编写方式是策略会等待预先设定的水平被突破。如果水平被计算且设置不正确，那么它们将永远不会被突破，或者总是只有其中之一会被突破。结果，要么策略不会执行任何交易，要么这些交易会亏钱。

对于这些以及许多其他无法预先预测的交易所场景，[S#](../../api.md) 提供对随机数据的测试，由于其均匀的唯一性，可以在短时间内生成尽可能多的条件。

## 在随机数据上测试移动平均策略

1. SampleRandomEmulation 示例（*..Samples\/Testing\/SampleRandomEmulation*）几乎与 SampleHistoryTesting 示例相同（其描述可在[历史数据测试](historical_data.md)部分找到），因为它们都使用了统一的[HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector)类。但与[历史数据测试](historical_data.md)不同的是，在随机数据测试中，市场数据不会被加载，而是“即时生成”。因此，该示例中增加了两个随机数据生成器：一个用于订单簿，一个用于逐笔交易。在 SampleHistoryTesting 中只使用了一个生成器——用于订单簿，因为没有历史数据存储。

   ```cs
   _connector.MarketDataAdapter.SendInMessage(new GeneratorMessage
   {
       IsSubscribe = true,
       Generator = new RandomWalkTradeGenerator(new SecurityId { SecurityCode = security.Code })
       {
           Interval = TimeSpan.FromSeconds(1),
           MaxVolume = maxVolume,
           MaxPriceStepCount = 3,	
           GenerateOriginSide = true,
           MinVolume = minVolume,
           RandomArrayLength = 99,
       }
   });
   _connector.SubscribeMarketDepth(new TrendMarketDepthGenerator(_connector.GetSecurityId(security)) { GenerateDepthOnEachTrade = false });
   ```
2. 示例工作的结果如下： ![仿真测试示例](../../../images/sample_emulation_test.png)
