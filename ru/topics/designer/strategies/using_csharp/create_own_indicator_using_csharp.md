# Создание индикатора на C\#

Создание собственного индикатора в [API](../../../api.md) описано в пункте [Собственный индикатор](../../../api/indicators/custom_indicator.md). Такие индикаторы полностью совместимы с **Дизайнер**.

Чтобы создать индикатор, на панели **Схемы** необходимо выбрать папку **Индикаторы**, нажать правую кнопку мыши и в контекстном меню выбрать **Добавить**:

![Designer_Source_Code_Indicator_00](../../../../images/designer_source_code_indicator_00.png)

Код индикатора будет выглядет так:

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

Данный индикатор получает входящее значение и делает у него произвольное отклонение на заданный параметре **Change** значение.

Описание методов индикатора доступно в разделе [Собственный индикатор](../../../api/indicators/custom_indicator.md).

Чтобы добавить созданный индикатор на схему, необходимо использовать кубик [Индикатор](../using_visual_designer/elements/common/indicator.md), и уже в нем задать необходимый индикатор:

![Designer_Source_Code_Indicator_01](../../../../images/designer_source_code_indicator_01.png)

Параметр **Change**, ранее заданный в коде индикатора, показан в панели свойств.

> [!WARNING] 
> Индикаторы из C# кода невозможно использовать в стратегиях, созданных на C# коде. Их возможно использовать только в стратегиях, созданных [из кубиков](../using_visual_designer.md).