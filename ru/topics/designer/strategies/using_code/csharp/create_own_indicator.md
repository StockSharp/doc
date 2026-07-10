# Создание индикатора

Создание собственного индикатора в [API](../../../../api.md) описано в пункте [Собственный индикатор](../../../../api/indicators/custom_indicator.md). Такие индикаторы полностью совместимы с **Дизайнер**.

Чтобы создать индикатор, на панели **Схемы** необходимо выбрать папку **Индикаторы**, нажать правую кнопку мыши и в контекстном меню выбрать **Добавить**:

![Designer_Source_Code_Indicator_00](../../../../../images/designer_source_code_indicator_00.png)

Код индикатора будет выглядеть так:

```cs
/// <summary>
/// Пример индикатора, показывающий сохранение и загрузку параметров.
/// 
/// Изменяет входную цену на +20% или -20%.
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
	// сформированный индикатор получил все нужные входные данные и готов для торговли
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
		// каждый 10-й вызов пытается вернуть пустое значение
		if (RandomGen.GetInt(0, 10) == 0)
			return new DecimalIndicatorValue(this);

		if (_counter++ == 5)
		{
	// например, нашему индикатору нужно 5 входных значений, чтобы стать сформированным
			_isFormed = true;
		}

		var value = input.GetValue<decimal>();

		// случайно изменить текущее значение на +20% или -20%

		value += value * RandomGen.GetInt(-Change, Change) / 100.0m;

		return new DecimalIndicatorValue(this, value)
		{
			// финальное значение означает, что это значение для указанного входа
	// больше не изменяется (например, для свечей, изменяющихся по последней цене)
			IsFinal = RandomGen.GetBool()
		};
	}

	// сохранить наши свойства для последующих перезапусков приложения

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

Данный индикатор получает входящее значение и делает произвольное отклонение на значение, заданное параметром **Change**.

Описание методов индикатора доступно в разделе [Собственный индикатор](../../../../api/indicators/custom_indicator.md).

Чтобы добавить созданный индикатор на схему, необходимо использовать кубик [Индикатор](../../using_visual_designer/elements/common/indicator.md), и уже в нем задать необходимый индикатор:

![Designer_Source_Code_Indicator_01](../../../../../images/designer_source_code_indicator_01.png)

Параметр **Change**, ранее заданный в коде индикатора, показан в панели свойств.

> [!WARNING] 
> Индикаторы из C# кода невозможно использовать в стратегиях, созданных на C# коде. Их возможно использовать только в стратегиях, созданных [из кубиков](../../using_visual_designer.md).
