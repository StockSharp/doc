# Создание собственного кубика

Аналогично созданию [кубика из схемы](../../using_visual_designer/composite_elements.md) можно создать свой кубик на основе F# кода. Такой кубик будет более функциональным, чем кубик из схемы.

Для создания кубика из кода, необходимо создать его в папке **Собственные кубики**:

![Designer_Source_Code_Elem_00](../../../../../images/designer_source_code_elem_00.png)

В ниже приведенном примере кубик наследуется от класса [DiagramExternalElement](xref:StockSharp.Diagram.DiagramExternalElement), и выглядит следующим образом:

```fsharp
/// <summary>
/// Sample diagram element demonstrating input and output sockets usage.
///
/// See more details:
/// https://doc.stocksharp.com/topics/designer/strategies/using_code/fsharp/creating_your_own_cube.html
/// </summary>
type EmptyDiagramElement() as this =
    inherit DiagramExternalElement()

    // Example property showing how to create parameters
    let minValueParam =
        this.AddParam<int>("MinValue", 10)
            .SetBasic(true)  // make the parameter visible in basic mode
            .SetDisplay("Parameters", "Min value", "Min value parameter description", 10)

    // Output sockets are events marked with DiagramExternal attribute
    let output1Event = new Event<Unit>()
    let output2Event = new Event<Unit>()

    [<CLIEvent>]
    [<DiagramExternal>]
    member this.Output1 = output1Event.Publish

    [<CLIEvent>]
    [<DiagramExternal>]
    member this.Output2 = output2Event.Publish

    // Uncomment the following property if you want the Process method 
    // to be called every time when a new argument is received
    // (no need to wait for all input args to be received).
    //
    // override this.WaitAllInput 
    //     with get () = false

    // Input sockets are method parameters marked with DiagramExternal attribute

    [<DiagramExternal>]
    member this.Process(candle: CandleMessage, diff: Unit) =
        let res = candle.ClosePrice + diff

        if diff >= minValueParam.Value then
            // Trigger the first output event
            output1Event.Trigger(res)
        else
            // Trigger the second output event
            output2Event.Trigger(res)

    override this.Start() =
        base.Start()
        // Add logic before start if needed

    override this.Stop() =
        base.Stop()
        // Add logic after stop if needed

    override this.Reset() =
        base.Reset()
        // Add logic for resetting internal state if needed
```

В данном коде кубик имеет два входящих сокета и два исходящих. Входящие сокеты определяются путем применения атрибута [DiagramExternalAttribute](xref:StockSharp.Diagram.DiagramExternalAttribute) к методу:

```fsharp
[<DiagramExternal>]
member this.Process(candle: CandleMessage, diff: Unit) =
```

Исходящие сокеты определяются путем применения атрибута к событию. В примере кубика таких событий два:

```fsharp
// Output sockets are events marked with DiagramExternal attribute
let output1Event = new Event<Unit>()
let output2Event = new Event<Unit>()

[<CLIEvent>]
[<DiagramExternal>]
member this.Output1 = output1Event.Publish

[<CLIEvent>]
[<DiagramExternal>]
member this.Output2 = output2Event.Publish
```

Поэтому исходящих сокета будет также два.

Дополнительно показано как сделать свойство у кубика:

```fsharp
// Example property showing how to create parameters
let minValueParam =
    this.AddParam<int>("MinValue", 10)
        .SetBasic(true)  // make the parameter visible in basic mode
        .SetDisplay("Parameters", "Min value", "Min value parameter description", 10)
```

При использовании класса [DiagramElementParam](xref:StockSharp.Diagram.DiagramElementParam`1) автоматически используется подход сохранения и восстановления настроек.

Свойство **MinValue** помечено как basic, и оно будет видно в режиме [Базовые свойства](../../using_visual_designer/diagram_panel.md).

Закомментированное свойство [WaitAllInput](xref:StockSharp.Diagram.DiagramExternalElement.WaitAllInput) отвечает за время вызова метода со входящими сокетами:

```fsharp
// override this.WaitAllInput 
//     with get () = false
```

Если раскомментировать свойство, то метод **Process** будет вызыватся всегда, как только придет хотя бы одно значение (в случае примера, это или свеча или числовое значение).

Чтобы добавить получившийся кубик на схему, необходимо в палитре в разделе **Собственные кубики** выбрать созданный кубик:

![Designer_Source_Code_Elem_01](../../../../../images/designer_source_code_elem_01.png)

> [!WARNING] 
> Кубики из F# кода невозможно использовать в стратегиях, созданных на F# коде. Их возможно использовать только в стратегиях, созданных [из кубиков](../../using_visual_designer.md).

## См. также

[Создание индикатора](create_own_indicator.md)