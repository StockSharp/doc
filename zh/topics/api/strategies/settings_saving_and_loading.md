# 保存和加载设置

在 StockSharp 中，保存和加载策略设置的机制是通过 [Strategy.Save](xref:StockSharp.Algo.Strategies.Strategy.Save(Ecng.Serialization.SettingsStorage)) 和 [Strategy.Load](xref:StockSharp.Algo.Strategies.Strategy.Load(Ecng.Serialization.SettingsStorage)) 方法实现的。

## 自动参数处理

在大多数情况下，**没有必要**重写 `Save` 和 `Load` 方法，因为基类 [Strategy](xref:StockSharp.Algo.Strategies.Strategy) 会自动保存和加载使用 [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) 创建的策略参数。

推荐的方法是使用在 [策略参数](parameters.md) 部分中详细描述的策略参数机制。采用这种方法，所有参数都会被自动保存和加载：

```cs
public class SmaStrategy : Strategy
{
	private readonly StrategyParam<int> _longSmaLength;

	public int LongSmaLength
	{
		get => _longSmaLength.Value;
		set => _longSmaLength.Value = value;
	}

	public SmaStrategy()
	{
		_longSmaLength = Param(nameof(LongSmaLength), 80)
							.SetDisplay("长期 SMA 周期", string.Empty, "基本设置");
	}
}
```

## 为特殊情况覆盖

只有在特殊情况下，当您需要保存或加载不属于标准策略参数集合的数据时，才需要重写 `Save` 和 `Load` 方法。例如，用于保存内部状态、非标准数据结构或缓存值。

如果您重写这些方法，**必须调用基类方法**：

```cs
public override void Save(SettingsStorage settings)
{
	// 先调用基类方法保存标准参数
	base.Save(settings);
	
	// 然后添加你的特定保存逻辑
	settings.SetValue("CustomState", _customState);
}
	
public override void Load(SettingsStorage settings)
{
	// 先调用基类方法加载标准参数
	base.Load(settings);
	
	// 然后添加你的特定加载逻辑
	if (settings.Contains("CustomState"))
		_customState = settings.GetValue<string>("CustomState");
}
```

## 从文件保存和加载

要将设置保存到文件或从文件加载，您可以使用 StockSharp 中实现的序列化和反序列化：

```cs
// 将设置保存到文件
var settingsStorage = new SettingsStorage();
strategy.Save(settingsStorage);
new JsonSerializer<SettingsStorage>().Serialize(settingsStorage, "strategy.json");

// 从文件加载设置
var newStrategy = new SmaStrategy();
if (File.Exists("strategy.json"))
{
	var loadedSettings = new JsonSerializer<SettingsStorage>().Deserialize("strategy.json");
	newStrategy.Load(loadedSettings);
}
```

## 推荐

1. 在可能的情况下，对于所有可配置的策略参数，请使用 [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1)。
2. 仅当需要保存/加载非标准数据时才重写 `Save` 和 `Load` 方法。
3. 在重写时始终调用基方法 `base.Save()` 和 `base.Load()`。
4. 使用 StockSharp 的标准序列化工具将设置保存到文件或从文件加载。

## 另请参阅

[策略参数](parameters.md)
