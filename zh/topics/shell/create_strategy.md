# 创建策略

[Shell](../shell.md) 自带的策略位于 `Shell.Avalonia/Shell.Core/Strategies/`。自定义策略也添加到这里，需要完成三个步骤：策略类、清单中的条目，以及（如有需要）自定义面板。

## 策略类

策略是 StockSharp 中 `Strategy` 的普通派生类，其中没有任何 Shell 特有的内容：

```cs
public class MyStrategy : Strategy
{
	private readonly StrategyParam<int> _length;

	public MyStrategy()
	{
		Name = "我的策略";

		_length = Param(nameof(Length), 20)
			.SetGreaterThanZero()
			// 分组取自资源，使该参数与标准参数归入同一分组。
			.SetDisplay("长度", "移动平均线的长度", LocalizedStrings.Parameters)
			.SetOptimize(10, 50, 5);
	}

	public int Length
	{
		get => _length.Value;
		set => _length.Value = value;
	}

	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);

		// 数据订阅和交易逻辑
	}
}
```

入口点是 `OnStarted2(DateTime)` 而不是 `OnStarted`：调用该方法时，策略已经获得交易品种、投资组合和连接，因此可以直接在这里订阅数据。

通过 `Param(...)` 声明并设置了 `SetOptimize(...)` 的参数会自动出现在[优化](user_interface/optimization.md)选项卡上：要遍历的参数列表及遍历范围直接从策略本身读取，无需单独设置。

## 策略清单

要让策略出现在选择窗口中，需要将其添加到两个位置。

第一个位置是 `BuiltInStrategies` 中的工厂，它根据类型标识符创建实例：

```cs
public static Strategy TryCreate(string templateTypeId)
{
	...
	if (templateTypeId.Equals(MyTypeId, StringComparison.OrdinalIgnoreCase))
		return new MyStrategy();
	...
}
```

第二个位置是 `LegacyStrategyTemplates` 中的模板描述，列表中的策略名称以及参数集合（包括参数类型和默认值）都取自这里：

```cs
new StrategyTemplateDefinition(MyTypeId, "我的策略", StrategyTemplateKind.Generic, true,
[
	.. CommonParameters(),
	new("Length", "长度", StrategyParameterKind.Integer, "20", true, 1),
])
```

`CommonParameters()` 返回交易品种、投资组合和数量，每个策略都有这些参数；其余是策略自己的参数。描述字段依次为：键、标签、类型、默认值、是否必填、最小值和最大值。

`StrategyTemplateKind` 只区分外壳为其提供了专用面板的类型（`Sma`、`SmaIndex`、`StairsTrend`、`StairsCountertrend`、`Designer`）；没有专用面板的策略为 `Generic`。

用户在 **策略** 面板上设置的值会在 `StrategyMaterializer` 中应用到实例；新参数的解析也添加在那里。

策略设置由外壳负责保存和恢复，无需编写单独的保存代码。

## 自定义面板

如果标准参数面板已经足够，则无需执行任何操作，Shell 会自动显示该面板。当策略除参数外还有其他内容需要展示时，才需要自定义面板：如何设置和监控该面板，请参阅[创建策略面板](create_strategy_panel.md)。

## 推荐内容

[创建策略面板](create_strategy_panel.md)

[运行 Designer 策略](run_strategies_from_designer.md)
