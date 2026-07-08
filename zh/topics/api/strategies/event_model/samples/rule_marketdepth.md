# 订单簿和交易规则

## 概览

`SimpleRulesStrategy` 是一种策略，展示了在 StockSharp 中创建和应用规则的各种方法。它订阅交易和订单簿，然后建立各种处理接收数据的规则。

## 主要组件

```cs
// 主要组件
public class SimpleRulesStrategy : Strategy
{
}
```

## OnStarted 方法

策略开始时调用：

- 创建对交易和订单簿的订阅
- 演示创建和应用规则的各种方法

```cs
// OnStarted 方法
protected override void OnStarted2(DateTime time)
{
	var tickSub = new Subscription(DataType.Ticks, Security);
	var mdSub = new Subscription(DataType.MarketDepth, Security);

	// -----------------------创建规则。方法 №1-----------------------------------
	mdSub.WhenOrderBookReceived(this).Do((depth) =>
	{
		LogInfo($"The rule WhenOrderBookReceived №1 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
	}).Once().Apply(this);

	// -----------------------创建规则。方法 №2-----------------------------------
	var whenMarketDepthChanged = mdSub.WhenOrderBookReceived(this);

	whenMarketDepthChanged.Do((depth) =>
	{
		LogInfo($"The rule WhenOrderBookReceived №2 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
	}).Once().Apply(this);

	// ----------------------规则中的规则-----------------------------------
	mdSub.WhenOrderBookReceived(this).Do((depth) =>
	{
		LogInfo($"The rule WhenOrderBookReceived №3 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");

		// ----------------------不是 Once 规则-----------------------------------
		mdSub.WhenOrderBookReceived(this).Do((depth1) =>
		{
			LogInfo($"The rule WhenOrderBookReceived №4 BestBid={depth1.GetBestBid()}, BestAsk={depth1.GetBestAsk()}");
		}).Apply(this);
	}).Once().Apply(this);

	// 发送市场数据订阅请求。
	Subscribe(tickSub);
	Subscribe(mdSub);

	base.OnStarted2(time);
}
```

## 逻辑

### 方法 #1：创建规则

- 创建一个在收到订单簿时触发的规则
- 记录最佳买价和卖价
- 该规则仅触发一次（`Once()`）

### 方法 #2：创建规则

- 演示创建规则的另一种方法
- 在功能上与方法#1相同

### 规则中的规则

- 创建一个在收到订单簿时触发的规则
- 在这个规则内部，创建了另一个规则
- 外部规则触发一次，内部规则——每次接收到订单簿时触发

## 特征

- 演示在 StockSharp 中创建和应用规则的各种方法
- 使用订阅进行交易和订单簿
- 展示在策略中使用 `LogInfo` 方法记录信息的示例
- 说明如何使用 `Once()` 来限制规则触发