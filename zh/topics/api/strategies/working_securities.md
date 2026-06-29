# 证券运作

## 描述

基类 `Strategy` 中的 `GetWorkingSecurities()` 方法用于获取策略在操作中使用的工具和数据类型列表。当使用 [Designer](../../designer.md) 时，该方法起着重要作用。

## 目的

该方法的主要目的是向 Designer 提供有关策略工作所需的工具和数据类型的信息。这使得 Designer 能够：

1. 在开始测试之前，检查存储中必要历史数据的可用性
2. 在可用时自动加载所需数据
3. 在启动策略时正确设置订阅

## 实施

在基类 `Strategy` 中，该方法返回一个空集合。为了与 Designer 正确配合，建议在您的策略中重写它：

```cs
public override IEnumerable<(Security sec, DataType dt)> GetWorkingSecurities()
{
	// Return a list of pairs (instrument, data type) used by the strategy
	return new[] 
	{ 
		(Security, CandleType),
		// Other instrument-data type pairs if the strategy uses multiple
	};
}
```

## 重写方法的重要性

如果在你的策略中未重写 `GetWorkingSecurities()` 方法：

- Designer 将无法自动检查所需的数据
- 如果存储中缺少所需的历史数据，Designer将不会发出警告
- 该策略可以启动进行测试，但不会显示任何结果
- 用户将不会收到关于结果缺失原因的任何信息

## 使用示例

```cs
public class MySmaStrategy : Strategy
{
	private readonly StrategyParam<DataType> _candleType;
	
	public DataType CandleType
	{
		get => _candleType.Value;
		set => _candleType.Value = value;
	}
	
	public MySmaStrategy()
	{
		_candleType = Param(nameof(CandleType), DataType.TimeFrame(TimeSpan.FromMinutes(1)));
	}
	
	// Override the method for correct work with the Designer
	public override IEnumerable<(Security sec, DataType dt)> GetWorkingSecurities()
	{
		return new[] { (Security, CandleType) };
	}
	
	// The rest of the strategy code...
}
```

## 结论

虽然 `GetWorkingSecurities()` 方法对于实现策略的基本功能并非强制性的，但强烈建议重写它，以便与 StockSharp Designer 正确配合工作。这有助于避免因缺少必要的历史数据而导致策略在测试时启动但未显示任何结果的情况。
