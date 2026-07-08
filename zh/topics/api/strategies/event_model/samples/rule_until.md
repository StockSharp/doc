# 直到规则

## 概览

`SimpleRulesUntilStrategy` 是一种策略，展示了在 StockSharp 中使用带有终止条件（`Until`）的规则的用法。它订阅交易和订单簿，然后建立一个规则，该规则会执行直到满足某个条件。

## 主要组件

```cs
// 主要组件
public class SimpleRulesUntilStrategy : Strategy
{
}
```

## OnStarted 方法

策略开始时调用：

- 创建对行情和订单簿的订阅
- 创建一个规则，当收到订单簿数据时执行，直到满足某个条件为止

```cs
// OnStarted 方法
protected override void OnStarted2(DateTime time)
{
	var tickSub = new Subscription(DataType.Ticks, Security);
	var mdSub = new Subscription(DataType.MarketDepth, Security);

	var i = 0;
	mdSub.WhenOrderBookReceived(this).Do(depth =>
	{
		i++;
		LogInfo($"The rule WhenOrderBookReceived BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
		LogInfo($"The rule WhenOrderBookReceived i={i}");
	})
	.Until(() => i >= 10)
	.Apply(this);

	// 发送市场数据订阅请求。
	Subscribe(tickSub);
	Subscribe(mdSub);

	base.OnStarted2(time);
}
```

## 逻辑

- 启动时，该策略会创建对报价和订单簿的订阅
- 创建了一个规则，每次接收到订单簿数据时都会触发
- 当规则触发时：
  - 计数器 `i` 已被增加
  - 有关最佳买价和卖价的信息已被添加到日志中
  - 计数器 `i` 的当前值已添加到日志中
- 该规则会执行，直到计数器 `i` 的值达到或超过 10
- 在条件满足后，规则会自动停止运行

## 特征

- 演示如何使用 `Until()` 方法来限制规则执行
- 使用订阅进行交易和订单簿
- 展示了如何使用 `LogInfo` 方法记录关于订单簿和对手方状态的信息的示例
- 说明如何根据特定条件限制规则执行的次数