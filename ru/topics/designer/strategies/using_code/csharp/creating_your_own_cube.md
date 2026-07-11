# Создание собственного кубика

Аналогично созданию [кубика из схемы](../../using_visual_designer/composite_elements.md) можно создать свой кубик на основе C# кода. Такой кубик будет более функциональным, чем кубик из схемы.

Для создания кубика из кода, необходимо создать его в папке **Собственные кубики**:

![Designer элемент с исходным кодом 00](../../../../../images/designer_source_code_elem_00.png)

В приведённом ниже примере кубик наследуется от класса [DiagramExternalElement](xref:StockSharp.Diagram.DiagramExternalElement) и выглядит следующим образом:

```cs
/// <summary>
/// Пример элемента диаграммы показывает использование входных и выходных сокетов.
/// 
/// https://doc.stocksharp.com/topics/Designer_Combine_Source_code_and_standard_elements.html
/// </summary>
public class EmptyDiagramElement : DiagramExternalElement
{
	private readonly DiagramElementParam<int> _minValue;

	public EmptyDiagramElement()
	{
		// пример свойства, показывающий создание параметров
	
		_minValue = AddParam("MinValue", 10)
			.SetBasic(true) // сделать параметр видимым в базовом режиме
			.SetDisplay("Параметры", "Мин. значение", "Описание параметра минимального значения", 10);
	}

	// выходные сокеты — это события, помеченные атрибутом DiagramExternal

	[DiagramExternal]
	public event Action<Unit> Output1;

	[DiagramExternal]
	public event Action<Unit> Output2;

	// раскомментируйте, чтобы метод Process вызывался при каждом новом аргументе
	// (не нужно ждать получения всех входных аргументов)
	//public override bool WaitAllInput => false;

	// входные сокеты — это параметры методов, помеченные атрибутом DiagramExternal

	[DiagramExternal]
	public void Process(CandleMessage candle, Unit diff)
	{
		var res = candle.ClosePrice + diff;

		if (diff >= _minValue.Value)
			Output1?.Invoke(res);
		else
			Output2?.Invoke(res);
	}

	public override void Start()
	{
		base.Start();

		// добавить логику перед запуском
	}

	public override void Stop()
	{
		base.Stop();

		// добавить логику после остановки
	}

	public override void Reset()
	{
		base.Reset();

		// добавить логику сброса внутреннего состояния
	}
}
```

В данном коде кубик имеет два входящих сокета и два исходящих. Входящие сокеты определяются путем применения атрибута [DiagramExternalAttribute](xref:StockSharp.Diagram.DiagramExternalAttribute) к методу:

```cs
[DiagramExternal]
public void Process(CandleMessage candle, Unit diff)
```

Исходящие сокеты определяются путем применения атрибута к событию. В примере кубика таких событий два:

```cs
[DiagramExternal]
public event Action<Unit> Output1;

[DiagramExternal]
public event Action<Unit> Output2;
```

Поэтому исходящих сокетов будет также два.

Дополнительно показано как сделать свойство у кубика:

```cs
_minValue = AddParam("MinValue", 10)
	.SetBasic(true) // сделать параметр видимым в базовом режиме
	.SetDisplay("Параметры", "Мин. значение", "Описание параметра минимального значения", 10);
```

При использовании класса [DiagramElementParam](xref:StockSharp.Diagram.DiagramElementParam`1) автоматически используется подход сохранения и восстановления настроек.

Свойство **MinValue** помечено как basic, и оно будет видно в режиме [Базовые свойства](../../using_visual_designer/diagram_panel.md).

Закомментированное свойство [WaitAllInput](xref:StockSharp.Diagram.DiagramExternalElement.WaitAllInput) отвечает за время вызова метода со входящими сокетами:

```cs
//public override bool WaitAllInput => false;
```

Если раскомментировать свойство, то метод **Process** будет вызываться всегда, как только придёт хотя бы одно значение (в случае примера это или свеча, или числовое значение).

Чтобы добавить получившийся кубик на схему, необходимо в палитре в разделе **Собственные кубики** выбрать созданный кубик:

![Designer элемент с исходным кодом 01](../../../../../images/designer_source_code_elem_01.png)

> [!WARNING] 
> Кубики из C# кода невозможно использовать в стратегиях, созданных на C# коде. Их возможно использовать только в стратегиях, созданных [из кубиков](../../using_visual_designer.md).

## См. также

[Создание индикатора](create_own_indicator.md)
