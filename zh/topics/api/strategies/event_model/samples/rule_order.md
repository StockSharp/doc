# 订单规则

## 概览

`SimpleOrderRulesStrategy` 是一种策略，它展示了在 StockSharp 中处理与订单相关的事件的规则的使用。它订阅交易并创建用于处理订单注册事件的规则。

## 主要组件

```cs
// 主要组件
public class SimpleOrderRulesStrategy : Strategy
{
}
```

## OnStarted 方法

策略开始时调用：

- 创建对行情的订阅
- 为处理订单注册事件创建两套规则

```cs
// OnStarted 方法
protected override void OnStarted2(DateTime time)
{
	var sub = new Subscription(DataType.Ticks, Security);

	sub.WhenTickTradeReceived(this).Do(() =>
	{
		var order = CreateOrder(Sides.Buy, default, 1);

		var ruleReg = order.WhenRegistered(this);
		var ruleRegFailed = order.WhenRegisterFailed(this);

		ruleReg
			.Do(() => LogInfo("订单 №1 已注册"))
			.Once()
			.Apply(this)
			.Exclusive(ruleRegFailed);

		ruleRegFailed
			.Do(() => LogInfo("Order №1 RegisterFailed"))
			.Once()
			.Apply(this)
			.Exclusive(ruleReg);

		RegisterOrder(order);
	}).Once().Apply(this);

	sub.WhenTickTradeReceived(this).Do(() =>
	{
		var order = CreateOrder(Sides.Buy, default, 10000000);

		var ruleReg = order.WhenRegistered(this);
		var ruleRegFailed = order.WhenRegisterFailed(this);

		ruleReg
			.Do(() => LogInfo("订单 №2 已注册"))
			.Once()
			.Apply(this)
			.Exclusive(ruleRegFailed);

		ruleRegFailed
			.Do(() => LogInfo("Order №2 RegisterFailed"))
			.Once()
			.Apply(this)
			.Exclusive(ruleReg);

		RegisterOrder(order);
	}).Once().Apply(this);

	// 发送市场数据订阅请求。
	Subscribe(sub);

	base.OnStarted2(time);
}
```

## 逻辑

### 第一套规则

- 当收到一个行情更新时，创建一个购买1个单位的订单
- 该订单是使用 `CreateOrder` 方法创建的，指定方向、价格（默认 = 市价）和数量
- 为处理成功注册和注册错误设定规则
- 这些规则是互斥的，并且只触发一次

### 第二套规则

- 当收到下一个报价时，创建一个购买10,000,000单位的订单
- 同样为处理成功注册和注册错误设定规则
- 这些规则也是互斥的，并且只触发一次

## 特征

- 演示创建用于处理订单注册事件的规则
- 使用互斥规则机制（`Exclusive`）
- 展示了使用 `LogInfo` 方法记录关于订单事件的信息的示例
- 说明如何使用 `Once()` 来限制规则触发
- 创建不同数量的订单以演示各种场景（成功注册和注册错误）