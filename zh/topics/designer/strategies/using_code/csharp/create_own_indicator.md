# 在 C# 中创建指标

有关在 [API](../../../../api.md) 中创建自定义指标的方法，请参阅[自定义指标](../../../../api/indicators/custom_indicator.md)。此类指标与 **Designer** 完全兼容。

要创建指标，请在 **策略图** 面板中选择 **指标** 文件夹，右键单击该文件夹，然后在上下文菜单中选择 **添加**：

![Designer_Source_Code_Indicator_00](../../../../../images/designer_source_code_indicator_00.png)

指标代码如下：

```cs
/// <summary>
/// 演示保存和加载参数的示例指标。
/// 
/// 将输入价格改变 +20% 或 -20%。
/// 
/// See more examples https://github.com/StockSharp/StockSharp/tree/master/Algo/Indicators
/// 
/// Doc https://doc.stocksharp.com/topics/Designer_Creating_indicator_from_source_code.html
/// </summary>
public class EmptyIndicator : BaseIndicator
{
	private int _change = 20;

	public int Change
	{
		get => _change;
		set
		{
			_change = value;
			Reset();
		}
	}

	private int _counter;
	// 已形成的指标收到所有必要输入，可用于交易
	private bool _isFormed;

	protected override bool CalcIsFormed() => _isFormed;

	public override void Reset()
	{
		base.Reset();

		_isFormed = default;
		_counter = default;
	}

	protected override IIndicatorValue OnProcess(IIndicatorValue input)
	{
		// 每第 10 次调用尝试返回空值
		if (RandomGen.GetInt(0, 10) == 0)
			return new DecimalIndicatorValue(this);

		if (_counter++ == 5)
		{
			// 例如，此指标需要 5 个输入值才会形成
			_isFormed = true;
		}

		var value = input.GetValue<decimal>();

		// 随机将当前值改变 +20% 或 -20%

		value += value * RandomGen.GetInt(-Change, Change) / 100.0m;

		return new DecimalIndicatorValue(this, value)
		{
			// final 值表示这是指定输入的最终值
			// 不再变化（例如，对于随最新价格变化的K线）
			IsFinal = RandomGen.GetBool()
		};
	}

	// 持久化属性，以便应用后续重启时保存

	public override void Load(SettingsStorage storage)
	{
		base.Load(storage);
		Change = storage.GetValue<int>(nameof(Change));
	}

	public override void Save(SettingsStorage storage)
	{
		base.Save(storage);
		storage.SetValue(nameof(Change), Change);
	}

	public override string ToString() => $"Change: {Change}";
}
```

该指标接收输入值，并根据 **变化** 参数对该值进行随机偏移。

有关指标方法的说明，请参阅[自定义指标](../../../../api/indicators/custom_indicator.md)。

要将创建的指标添加到策略图，请使用 [指标](../../using_visual_designer/elements/common/indicator.md) 模块，并在其中选择所需指标：

![Designer_Source_Code_Indicator_01](../../../../../images/designer_source_code_indicator_01.png)

属性面板会显示此前在指标代码中定义的 **变化** 参数。

> [!WARNING]
> 使用 C# 代码创建的指标不能用于同样使用 C# 代码创建的策略，只能用于通过[模块](../../using_visual_designer.md)创建的策略。
