# Creating an Indicator in C#

Creating your own indicator in [API](../../../../api.md) is described in the section [Custom Indicator](../../../../api/indicators/custom_indicator.md). Such indicators are fully compatible with **Designer**.

To create an indicator, on the **Scheme** panel you need to select the **Indicators** folder, right-click and in the context menu select **Add**:

![Designer_Source_Code_Indicator_00](../../../../../images/designer_source_code_indicator_00.png)

The indicator code will look like this:

```cs
/// <summary>
/// Sample indicator demonstrating to save and load parameters.
/// 
/// Changes input price on +20% or -20%.
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
	// formed indicator received all necessary inputs for be available for trading
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
		// every 10th call try return empty value
		if (RandomGen.GetInt(0, 10) == 0)
			return new DecimalIndicatorValue(this);

		if (_counter++ == 5)
		{
			// for example, our indicator needs 5 inputs for become formed
			_isFormed = true;
		}

		var value = input.GetValue<decimal>();

		// random change on +20% or -20% current value

		value += value * RandomGen.GetInt(-Change, Change) / 100.0m;

		return new DecimalIndicatorValue(this, value)
		{
			// final value means that this value for the specified input
			// is not changed anymore (for example, for candles that changes with last price)
			IsFinal = RandomGen.GetBool()
		};
	}

	// persist our properties to save for further the app restarts

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

This indicator receives an incoming value and makes an arbitrary deviation on the set parameter **Change** value.

The description of the indicator methods is available in the section [Custom Indicator](../../../../api/indicators/custom_indicator.md).

To add the created indicator to the diagram, you need to use the [Indicator](../../using_visual_designer/elements/common/indicator.md) cube, and in it, specify the necessary indicator:

![Designer_Source_Code_Indicator_01](../../../../../images/designer_source_code_indicator_01.png)

The **Change** parameter, previously set in the indicator code, is shown in the properties panel.

> [!WARNING] 
> Indicators from C# code cannot be used in strategies created in C# code. They can only be used in strategies created [from cubes](../../using_visual_designer.md).
