# Создание собственного кубика

Аналогично созданию [кубика из схемы](../../using_visual_designer/composite_elements.md) можно создать свой кубик на основе C# кода. Такой кубик будет более функциональным, чем кубик из схемы.

Для создания кубика из кода, необходимо создать его в папке **Собственные кубики**:

![Designer_Source_Code_Elem_00](../../../../../images/designer_source_code_elem_00.png)

В ниже приведенном примере кубик наследуется от класса [DiagramExternalElement](xref:StockSharp.Diagram.DiagramExternalElement), и выглядит следующим образом:

```cs
/// <summary>
/// Sample diagram element demonstrates input and output sockets usage.
/// 
/// https://doc.stocksharp.com/topics/Designer_Combine_Source_code_and_standard_elements.html
/// </summary>
public class EmptyDiagramElement : DiagramExternalElement
{
	private readonly DiagramElementParam<int> _minValue;

	public EmptyDiagramElement()
	{
		// example property to show how to make parameters
	
		_minValue = AddParam("MinValue", 10)
			.SetBasic(true) // make parameter visible in basic mode
			.SetDisplay("Parameters", "Min value", "Min value parameter description", 10);
	}

	// output sockets are events marked with DiagramExternal attribute

	[DiagramExternal]
	public event Action<Unit> Output1;

	[DiagramExternal]
	public event Action<Unit> Output2;

	// uncomment to get Process method called every time when new arg received
	// (no need wait when all input args received)
	//public override bool WaitAllInput => false;

	// input sockets are method parameters marked with DiagramExternal attribute

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

		// add logic before start
	}

	public override void Stop()
	{
		base.Stop();

		// add logic after stop
	}

	public override void Reset()
	{
		base.Reset();

		// add logic for reset internal state
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

Поэтому исходящих сокета будет также два.

Дополнительно показано как сделать свойство у кубика:

```cs
_minValue = AddParam("MinValue", 10)
	.SetBasic(true) // make parameter visible in basic mode
	.SetDisplay("Parameters", "Min value", "Min value parameter description", 10);
```

При использовании класса [DiagramElementParam](xref:StockSharp.Diagram.DiagramElementParam`1) автоматически используется подход сохранения и восстановления настроек.

Свойство **MinValue** помечено как basic, и оно будет видно в режиме [Базовые свойства](../../using_visual_designer/diagram_panel.md).

Закомментированное свойство [WaitAllInput](xref:StockSharp.Diagram.DiagramExternalElement.WaitAllInput) отвечает за время вызова метода со входящими сокетами:

```cs
//public override bool WaitAllInput => false;
```

Если раскомментировать свойство, то метод **Process** будет вызыватся всегда, как только придет хотя бы одно значение (в случае примера, это или свеча или числовое значение).

Чтобы добавить получившийся кубик на схему, необходимо в палитре в разделе **Собственные кубики** выбрать созданный кубик:

![Designer_Source_Code_Elem_01](../../../../../images/designer_source_code_elem_01.png)

> [!WARNING] 
> Кубики из C# кода невозможно использовать в стратегиях, созданных на C# коде. Их возможно использовать только в стратегиях, созданных [из кубиков](../../using_visual_designer.md).

## См. также

[Создание индикатора](create_own_indicator.md)