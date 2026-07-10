# 点差交易规则

## 概览

`SimpleTradeRulesStrategy` 是一种策略，它展示了在 StockSharp 中使用组合规则分析交易价格的方法。它订阅交易并创建一个在特定价格条件下触发的规则。

## 主要组件

```cs
// 主要组件
public class SimpleTradeRulesStrategy : Strategy
{
}
```

## OnStarted 方法

策略开始时调用：

- 创建对行情的订阅
- 创建一个用于分析交易价格的组合规则

```cs
// OnStarted 方法
protected override void OnStarted2(DateTime time)
{
	var sub = new Subscription(DataType.Ticks, Security);

	sub.WhenTickTradeReceived(this).Do(t =>
	{
		sub
			.WhenLastTradePriceMore(this, t.Price + 2)
			.Or(sub.WhenLastTradePriceLess(this, t.Price - 2))
			.Do(t =>
			{
				LogInfo($"规则 WhenLastTradePriceMore 或 WhenLastTradePriceLess tick={t}");
			})
			.Apply(this);
	})
	.Once() // 只调用此规则一次
	.Apply(this);

	// 发送市场数据订阅请求。
	Subscribe(sub);

	base.OnStarted2(time);
}
```

## 逻辑

- 当收到第一个行情时，会创建一个组合规则
- 它基于收到的价格波动：创建一个当价格变动±2时触发的规则
- 当最后一次交易的价格超过当前价格 +2 或低于当前价格 -2 时，该规则生效
- 当规则触发时，有关该勾选的信息会被添加到日志中
- 外部规则只触发一次（`Once()`）

## 特征

- 演示使用 `Or()` 创建组合规则
- 使用 `WhenLastTradePriceMore` 和 `WhenLastTradePriceLess` 进行价格分析
- 显示使用 `LogInfo` 方法记录交易信息的示例
- 说明如何使用 `Once()` 来限制规则触发
- 将 tick 参数传递给事件处理程序（与文档中的示例不同）