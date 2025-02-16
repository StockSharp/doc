# Creating Your Own Cube

Similar to creating a [cube from a diagram](../../using_visual_designer/composite_elements.md), you can create your own cube based on F# code. Such a cube will be more functional than a cube from a diagram.

To create a cube from code, it needs to be created in the **Own elements** folder:

![Designer_Source_Code_Elem_00](../../../../../images/designer_source_code_elem_00.png)

In the example provided below, the cube inherits from the [DiagramExternalElement](xref:StockSharp.Diagram.DiagramExternalElement) class, and looks as follows:

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

In this code, the cube has two incoming sockets and two outgoing sockets. Incoming sockets are defined by applying the [DiagramExternalAttribute](xref:StockSharp.Diagram.DiagramExternalAttribute) attribute to the method:

```fsharp
[<DiagramExternal>]
member this.Process(candle: CandleMessage, diff: Unit) =
```

Outgoing sockets are defined by applying the attribute to an event. In the example of the cube, there are two such events:


```fsharp
let output1Event = new Event<Unit>()
let output2Event = new Event<Unit>()

[<CLIEvent>]
[<DiagramExternal>]
member this.Output1 = output1Event.Publish

[<CLIEvent>]
[<DiagramExternal>]
member this.Output2 = output2Event.Publish
```

Therefore, there will also be two outgoing sockets.

Additionally, it shows how to make a property for the cube:

```fsharp
let minValueParam =
    this.AddParam<int>("MinValue", 10)
        .SetBasic(true)  // make the parameter visible in basic mode
        .SetDisplay("Parameters", "Min value", "Min value parameter description", 10)
```

Using the [DiagramElementParam](xref:StockSharp.Diagram.DiagramElementParam`1) class automatically employs the approach to save and restore settings.

The **MinValue** property is marked as basic, and it will be visible in the [Basic properties](../../using_visual_designer/diagram_panel.md) mode.

The commented [WaitAllInput](xref:StockSharp.Diagram.DiagramExternalElement.WaitAllInput) property is responsible for the timing of the method call with incoming sockets:

```fsharp
// override this.WaitAllInput 
//     with get () = false
```

If uncommented, the **Process** method will always be called as soon as at least one value arrives (in the example case, this is either a candle or a numerical value).

To add the resulting cube to the diagram, you need to select the created cube in the palette in the **Own elements** section:

![Designer_Source_Code_Elem_01](../../../../../images/designer_source_code_elem_01.png)

> [!WARNING] 
> Cubes from F# code cannot be used in strategies created in F# code. They can only be used in strategies created [from cubes](../../using_visual_designer.md).

## See Also

[Creating an Indicator](create_own_indicator.md)