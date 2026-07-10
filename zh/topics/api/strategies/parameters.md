# 策略参数

对于策略配置和优化，StockSharp 提供了一个特殊类 [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1)。策略参数允许您在不更改代码的情况下修改交易算法设置，这在测试模式和实盘交易模式之间切换时尤其方便。此外，这些参数还用于优化过程中，以自动迭代数值并找到最佳策略设置。

与常规 C# 属性不同，使用此类创建的参数会自动显示在可视化设置中（例如，在 Designer 中），并且可以用于策略优化。

## 创建策略参数

参数在策略构造函数中使用 [Strategy.Param](xref:StockSharp.Algo.Strategies.Strategy.Param``1(System.String,``0)) 方法创建：

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
							.SetGreaterThanZero()
							.SetDisplay("Long SMA length", string.Empty, "基本设置");
	}
}
```

在此示例中，创建了一个参数 `LongSmaLength`，初始值为 80，设置了验证器以确保该值大于零，并为用户界面配置了显示设置。

## 参数配置方法

[StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) 类提供了多种参数配置方法：

### 设置显示

[StrategyParam\<T\>.SetDisplay](xref:StockSharp.Algo.Strategies.StrategyParam`1.SetDisplay(System.String,System.String,System.String)) 方法用于设置参数的显示名称、描述和类别：

```cs
_longSmaLength = Param(nameof(LongSmaLength), 80)
					.SetDisplay("Long SMA length", "Period of the long moving average", "基本设置");
```

### 设置验证器

[StrategyParam\<T\>.SetValidator](xref:Ecng.ComponentModel.Extensions.SetValidator``1(``0,System.ComponentModel.DataAnnotations.ValidationAttribute)) 方法设置一个验证器来检查参数值。StockSharp 提供了一系列预定义的验证器，可用于最常见的任务：

```cs
// 检查数值是否大于零
_longSmaLength = Param(nameof(LongSmaLength), 80)
					.SetValidator(new IntGreaterThanZeroAttribute());

// 检查数值是否非负
_volume = Param(nameof(Volume), 1)
			.SetValidator(new DecimalNotNegativeAttribute());

// 检查取值范围
_percentage = Param(nameof(Percentage), 50)
				.SetValidator(new RangeAttribute(0, 100));

// 检查必填值
_security = Param<Security>(nameof(Security))
				.SetValidator(new RequiredAttribute());
```

为了方便，[StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) 为最常见的验证器内置了方法：

```cs
// 检查数值是否大于零
_longSmaLength = Param(nameof(LongSmaLength), 80).SetGreaterThanZero();

// 检查数值是否非负
_volume = Param(nameof(Volume), 1).SetNotNegative();

// 检查该值是否为 NULL 或非负
_interval = Param<TimeSpan?>(nameof(Interval)).SetNullOrNotNegative();

// 设置取值范围
_percentage = Param(nameof(Percentage), 50).SetRange(0, 100);
```

如果内置验证器不够用，你可以通过继承 [ValidationAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.validationattribute) 来创建自己的验证器：

```cs
public class EvenNumberAttribute : ValidationAttribute
{
	public EvenNumberAttribute()
		: base("Value must be an even number.")
	{
	}

	public override bool IsValid(object value)
	{
		if (value is int intValue)
			return intValue % 2 == 0;
		
		return false;
	}
}

// 使用自定义验证器
_barCount = Param(nameof(BarCount), 10)
				.SetValidator(new EvenNumberAttribute());
```

### 设置隐藏

[StrategyParam\<T\>.SetHidden](xref:StockSharp.Algo.Strategies.StrategyParam`1.SetHidden(System.Boolean)) 方法在属性编辑器中隐藏参数：

```cs
_systemParam = Param(nameof(SystemParam), "value")
				.SetHidden(true);
```

### 设置基础

[StrategyParam\<T\>.SetBasic](xref:StockSharp.Algo.Strategies.StrategyParam`1.SetBasic(System.Boolean)) 方法将参数标记为基本参数，这会影响其在用户界面中的显示。基本参数会在简化属性编辑器模式下显示：

```cs
_longSmaLength = Param(nameof(LongSmaLength), 80)
					.SetBasic(true);
```

![策略 参数 基础 高级](../../../images/strategy_parameters_basic_advanced.png)

### 设置为只读

[StrategyParam\<T\>.SetReadOnly](xref:StockSharp.Algo.Strategies.StrategyParam`1.SetReadOnly(System.Boolean)) 方法使参数只读：

```cs
_calculatedParam = Param(nameof(CalculatedParam), 0)
					.SetReadOnly(true);
```

### SetCanOptimize 和 SetOptimize

方法 [StrategyParam\<T\>.SetCanOptimize](xref:StockSharp.Algo.Strategies.StrategyParam`1.SetCanOptimize(System.Boolean)) 和 [StrategyParam\<T\>.SetOptimize](xref:StockSharp.Algo.Strategies.StrategyParam`1.SetOptimize(`0,`0,`0)) 指定参数是否可以用于优化，并设置优化的取值范围：

```cs
_longSmaLength = Param(nameof(LongSmaLength), 80)
					.SetCanOptimize(true)
					.SetOptimize(10, 200, 10);
```

在上面的例子中，该参数将在从10到200的范围内以10为步长进行优化。

## 在策略中使用参数

策略参数的使用方式就像常规属性一样：

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	_shortSma = new SimpleMovingAverage { Length = ShortSmaLength };
	_longSma = new SimpleMovingAverage { Length = LongSmaLength };
	
	// ...
}
```

## 保存和加载参数

参数值会自动在基类 [Strategy](xref:StockSharp.Algo.Strategies.Strategy) 中保存和加载。当重写 [Strategy.Save](xref:StockSharp.Algo.Strategies.Strategy.Save(Ecng.Serialization.SettingsStorage)) 和 [Strategy.Load](xref:StockSharp.Algo.Strategies.Strategy.Load(Ecng.Serialization.SettingsStorage)) 方法时，必须调用基类方法：

```cs
public override void Save(SettingsStorage settings)
{
	base.Save(settings);
	
	// 额外的保存逻辑...
}

public override void Load(SettingsStorage settings)
{
	base.Load(settings);
	
	// 额外的加载逻辑...
}
```

## 示例：具有多个参数的策略

下面是一个包含多个参数的策略示例：

```cs
public class SmaStrategy : Strategy
{
	private readonly StrategyParam<DataType> _series;
	private readonly StrategyParam<int> _longSmaLength;
	private readonly StrategyParam<int> _shortSmaLength;

	public DataType Series
	{
		get => _series.Value;
		set => _series.Value = value;
	}

	public int LongSmaLength
	{
		get => _longSmaLength.Value;
		set => _longSmaLength.Value = value;
	}

	public int ShortSmaLength
	{
		get => _shortSmaLength.Value;
		set => _shortSmaLength.Value = value;
	}

	public SmaStrategy()
	{
		base.Name = "SMA strategy";

		Param("TypeId", GetType().GetTypeName(false)).SetHidden();
		_longSmaLength = Param(nameof(LongSmaLength), 80)
							.SetGreaterThanZero()
							.SetDisplay("Long SMA length", string.Empty, "基本设置")
							.SetCanOptimize(true)
							.SetOptimize(20, 200, 10);
		
		_shortSmaLength = Param(nameof(ShortSmaLength), 30)
							.SetGreaterThanZero()
							.SetDisplay("Short SMA length", string.Empty, "基本设置")
							.SetCanOptimize(true)
							.SetOptimize(5, 50, 5);
		
		_series = Param(nameof(Series), DataType.TimeFrame(TimeSpan.FromMinutes(15)))
					.SetDisplay("Series", string.Empty, "基本设置");
	}

	// ...
}
```

在此示例中，我们创建了一个基于两条移动平均线交叉的策略，并具有三个可配置参数：
- `Series` - 数据类型和时间范围
- `LongSmaLength` - 长期移动平均线的周期
- `ShortSmaLength` - 短期移动平均线的周期

对于这两个数值参数，我们配置了具有指定范围的优化功能。

## 另请参阅

[保存和加载设置](settings_saving_and_loading.md)
