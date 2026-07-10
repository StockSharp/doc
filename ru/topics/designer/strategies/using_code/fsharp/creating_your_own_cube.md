# Создание собственного кубика

Аналогично созданию [кубика из схемы](../../using_visual_designer/composite_elements.md) можно создать свой кубик на основе F# кода. Такой кубик будет более функциональным, чем кубик из схемы.

Для создания кубика из кода, необходимо создать его в папке **Собственные кубики**:

![Designer_Source_Code_Elem_00](../../../../../images/designer_source_code_elem_00.png)

В нижеприведённом примере кубик наследуется от класса [DiagramExternalElement](xref:StockSharp.Diagram.DiagramExternalElement) и выглядит следующим образом:

```fsharp
/// <summary>
/// Пример элемента диаграммы показывает использование входных и выходных сокетов.
///
/// Подробнее:
/// https://doc.stocksharp.com/topics/designer/strategies/using_code/fsharp/creating_your_own_cube.html
/// </summary>
type EmptyDiagramElement() as this =
	inherit DiagramExternalElement()

	// Пример свойства, показывающий создание параметров
	let minValueParam =
		this.AddParam<int>("MinValue", 10)
			.SetBasic(true)  // сделать параметр видимым в базовом режиме
			.SetDisplay("Параметры", "Мин. значение", "Описание параметра минимального значения", 10)

	// Выходные сокеты — это события, помеченные атрибутом DiagramExternal
	let output1Event = new Event<Unit>()
	let output2Event = new Event<Unit>()

	[<CLIEvent>]
	[<DiagramExternal>]
	member this.Output1 = output1Event.Publish

	[<CLIEvent>]
	[<DiagramExternal>]
	member this.Output2 = output2Event.Publish

	// Раскомментируйте следующее свойство, если хотите, чтобы метод Process 
	// вызывался каждый раз при получении нового аргумента
	// (не нужно ждать получения всех входных аргументов).
	//
	// переопределить this.WaitAllInput
	//     with get () = false

	// Входные сокеты — это параметры методов, помеченные атрибутом DiagramExternal

	[<DiagramExternal>]
	member this.Process(candle: CandleMessage, diff: Unit) =
		let res = candle.ClosePrice + diff

		if diff >= minValueParam.Value then
			// Вызвать первое выходное событие
			output1Event.Trigger(res)
		else
			// Вызвать второе выходное событие
			output2Event.Trigger(res)

	override this.Start() =
		base.Start()
		// При необходимости добавить логику перед запуском

	override this.Stop() =
		base.Stop()
		// При необходимости добавить логику после остановки

	override this.Reset() =
		base.Reset()
		// При необходимости добавить логику сброса внутреннего состояния
```

В данном коде кубик имеет два входящих сокета и два исходящих. Входящие сокеты определяются путем применения атрибута [DiagramExternalAttribute](xref:StockSharp.Diagram.DiagramExternalAttribute) к методу:

```fsharp
[<DiagramExternal>]
member this.Process(candle: CandleMessage, diff: Unit) =
```

Исходящие сокеты определяются путем применения атрибута к событию. В примере кубика таких событий два:

```fsharp
// Выходные сокеты — это события, помеченные атрибутом DiagramExternal
let output1Event = new Event<Unit>()
let output2Event = new Event<Unit>()

[<CLIEvent>]
[<DiagramExternal>]
member this.Output1 = output1Event.Publish

[<CLIEvent>]
[<DiagramExternal>]
member this.Output2 = output2Event.Publish
```

Поэтому исходящих сокетов также будет два.

Дополнительно показано как сделать свойство у кубика:

```fsharp
// Пример свойства, показывающий создание параметров
let minValueParam =
	this.AddParam<int>("MinValue", 10)
		.SetBasic(true)  // сделать параметр видимым в базовом режиме
		.SetDisplay("Параметры", "Мин. значение", "Описание параметра минимального значения", 10)
```

При использовании класса [DiagramElementParam](xref:StockSharp.Diagram.DiagramElementParam`1) автоматически используется подход сохранения и восстановления настроек.

Свойство **MinValue** помечено как basic, и оно будет видно в режиме [Базовые свойства](../../using_visual_designer/diagram_panel.md).

Закомментированное свойство [WaitAllInput](xref:StockSharp.Diagram.DiagramExternalElement.WaitAllInput) отвечает за время вызова метода со входящими сокетами:

```fsharp
// переопределить this.WaitAllInput
//     with get () = false
```

Если раскомментировать свойство, то метод **Process** будет вызываться всегда, как только придёт хотя бы одно значение (в случае примера это или свеча, или числовое значение).

Чтобы добавить получившийся кубик на схему, необходимо в палитре в разделе **Собственные кубики** выбрать созданный кубик:

![Designer_Source_Code_Elem_01](../../../../../images/designer_source_code_elem_01.png)

> [!WARNING] 
> Кубики из F# кода невозможно использовать в стратегиях, созданных на F# коде. Их возможно использовать только в стратегиях, созданных [из кубиков](../../using_visual_designer.md).

## См. также

[Создание индикатора](create_own_indicator.md)
