# 单支K线的规则

## 概览

`SimpleCandleRulesStrategy` 是一个策略，展示了在 StockSharp 中使用K线规则的方法。它跟踪K线的成交量，并在满足特定条件时记录信息。

## 主要组件

```cs
// 主要组件
public class SimpleCandleRulesStrategy : Strategy
{
}
```

## OnStarted 方法

策略开始时调用：

- 初始化对5分钟K线的订阅
- 建立处理K线的规则

```cs
// OnStarted 方法
protected override void OnStarted2(DateTime time)
{
	var subscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security)
	{
		// 现成 K线比即时压缩模式快得多
	// 关闭压缩以加速优化器（!!! 请确保有K线）

		//MarketData =
		//{
		//    BuildMode = MarketDataBuildModes.Build,
		//    BuildFrom = DataType.Ticks,
		//}
	};
	Subscribe(subscription);

	var i = 0;
	var diff = "10%".ToUnit();

	this.WhenCandlesStarted(subscription)
		.Do((candle) =>
		{
			i++;

			this
				.WhenTotalVolumeMore(candle, diff)
				.Do((candle1) =>
				{
	LogInfo($"规则 WhenCandlesStarted 和 WhenTotalVolumeMore K线={candle1}");
	LogInfo($"规则 WhenCandlesStarted 和 WhenTotalVolumeMore i={i}");
				})
				.Once().Apply(this);

		}).Apply(this);

	base.OnStarted2(time);
}
```

## 逻辑

- 该策略使用5分钟K线
- 当每根K线开始形成时，一条规则就被建立
- 当K线的总成交量超过10%（使用百分比值）时，该规则会触发
- 当规则被触发时，关于K线和计数器的信息会被添加到日志中
- 在第一次触发后，规则由于 `Once()` 方法而停止工作

## 特征

- 演示 `WhenCandlesStarted` 和 `WhenTotalVolumeMore` 规则的使用
- 使用K线订阅机制
- 显示了通过 `"10%".ToUnit()` 创建百分比值的示例
- 展示在策略中使用 `LogInfo` 方法记录信息的示例
- 包含用于从逐笔数据构建K线的注释代码
