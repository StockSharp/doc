# 创建自定义模块

与[通过策略图创建模块](../../using_visual_designer/composite_elements.md)类似，也可以使用 F# 代码创建自定义模块。代码模块的功能比策略图模块更灵活。

要通过代码创建模块，请在 **Own elements** 文件夹中创建该模块：

![Designer_Source_Code_Elem_00](../../../../../images/designer_source_code_elem_00.png)

下面的示例模块继承自 [DiagramExternalElement](xref:StockSharp.Diagram.DiagramExternalElement) 类，代码如下：

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

在这段代码中，模块包含两个输入端口和两个输出端口。对方法应用 [DiagramExternalAttribute](xref:StockSharp.Diagram.DiagramExternalAttribute) 特性，即可定义输入端口：

```fsharp
[<DiagramExternal>]
member this.Process(candle: CandleMessage, diff: Unit) =
```

对事件应用该特性即可定义输出端口。示例模块包含以下两个事件：


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

因此，模块也会包含两个输出端口。

示例还演示了如何为模块创建属性：

```fsharp
let minValueParam =
	this.AddParam<int>("MinValue", 10)
		.SetBasic(true)  // make the parameter visible in basic mode
		.SetDisplay("Parameters", "Min value", "Min value parameter description", 10)
```

使用 [DiagramElementParam](xref:StockSharp.Diagram.DiagramElementParam`1) 类后，系统会自动处理设置的保存和恢复。

**MinValue** 属性被标记为基本属性，因此会显示在[基本属性](../../using_visual_designer/diagram_panel.md)模式中。

已注释的 [WaitAllInput](xref:StockSharp.Diagram.DiagramExternalElement.WaitAllInput) 属性控制何时根据输入端口的数据调用方法：

```fsharp
// override this.WaitAllInput 
//     with get () = false
```

取消注释后，只要至少有一个值到达，**Process** 方法就会立即被调用。在本例中，该值可以是蜡烛或数值。

要将创建的模块添加到策略图，请在组件面板的 **Own elements** 部分选择该模块：

![Designer_Source_Code_Elem_01](../../../../../images/designer_source_code_elem_01.png)

> [!WARNING]
> 使用 F# 代码创建的模块不能用于同样使用 F# 代码创建的策略，只能用于通过[模块](../../using_visual_designer.md)创建的策略。

## 另请参阅

[创建指标](create_own_indicator.md)
