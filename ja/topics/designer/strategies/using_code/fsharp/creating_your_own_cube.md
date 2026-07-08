# 独自のキューブを作成する

[ダイアグラムからキューブ](../../using_visual_designer/composite_elements.md)を作成する場合と同様に、F# コードに基づいて独自のキューブを作成できます。このようなキューブは、ダイアグラムから作成したキューブよりも高機能になります。

コードからキューブを作成するには、**Own elements** フォルダー内に作成する必要があります。

![Designer_Source_Code_Elem_00](../../../../../images/designer_source_code_elem_00.png)

以下の例では、キューブは [DiagramExternalElement](xref:StockSharp.Diagram.DiagramExternalElement) クラスを継承し、次のようになります。

```fsharp
/// <summary>
/// 入力ソケットと出力ソケットの使用方法を示すサンプルダイアグラム要素です。
///
/// 詳細:
/// https://doc.stocksharp.com/topics/designer/strategies/using_code/fsharp/creating_your_own_cube.html
/// </summary>
type EmptyDiagramElement() as this =
	inherit DiagramExternalElement()

	// パラメーターの作成方法を示すプロパティの例です。
	let minValueParam =
		this.AddParam<int>("MinValue", 10)
			.SetBasic(true)  // パラメーターを basic モードで表示します。
			.SetDisplay("Parameters", "Min value", "Min value parameter description", 10)

	// 出力ソケットは DiagramExternal 属性でマークされたイベントです。
	let output1Event = new Event<Unit>()
	let output2Event = new Event<Unit>()

	[<CLIEvent>]
	[<DiagramExternal>]
	member this.Output1 = output1Event.Publish

	[<CLIEvent>]
	[<DiagramExternal>]
	member this.Output2 = output2Event.Publish

	// 新しい引数を受信するたびに Process メソッドを呼び出したい場合は、
	// 次のプロパティをコメント解除します。
	// (すべての入力引数を受信するまで待つ必要はありません)。
	//
	// override this.WaitAllInput 
	//     with get () = false

	// 入力ソケットは DiagramExternal 属性でマークされたメソッドパラメーターです。

	[<DiagramExternal>]
	member this.Process(candle: CandleMessage, diff: Unit) =
		let res = candle.ClosePrice + diff

		if diff >= minValueParam.Value then
			// 最初の出力イベントをトリガーします。
			output1Event.Trigger(res)
		else
			// 2 番目の出力イベントをトリガーします。
			output2Event.Trigger(res)

	override this.Start() =
		base.Start()
		// 必要に応じて開始前のロジックを追加します。

	override this.Stop() =
		base.Stop()
		// 必要に応じて停止後のロジックを追加します。

	override this.Reset() =
		base.Reset()
		// 必要に応じて内部状態をリセットするロジックを追加します。
```

このコードでは、キューブに 2 つの入力ソケットと 2 つの出力ソケットがあります。入力ソケットは、メソッドに [DiagramExternalAttribute](xref:StockSharp.Diagram.DiagramExternalAttribute) 属性を適用することで定義されます。

```fsharp
[<DiagramExternal>]
member this.Process(candle: CandleMessage, diff: Unit) =
```

出力ソケットは、イベントに属性を適用することで定義されます。キューブの例では、このようなイベントが 2 つあります。


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

したがって、出力ソケットも 2 つになります。

さらに、キューブのプロパティを作成する方法を示しています。

```fsharp
let minValueParam =
	this.AddParam<int>("MinValue", 10)
		.SetBasic(true)  // パラメーターを basic モードで表示します。
		.SetDisplay("Parameters", "Min value", "Min value parameter description", 10)
```

[DiagramElementParam](xref:StockSharp.Diagram.DiagramElementParam`1) クラスを使用すると、設定を保存および復元する方法が自動的に利用されます。

**MinValue** プロパティは basic としてマークされており、[Basic properties](../../using_visual_designer/diagram_panel.md) モードで表示されます。

コメントアウトされている [WaitAllInput](xref:StockSharp.Diagram.DiagramExternalElement.WaitAllInput) プロパティは、入力ソケットを持つメソッド呼び出しのタイミングを担います。

```fsharp
// override this.WaitAllInput 
//     with get () = false
```

コメント解除すると、少なくとも 1 つの値が到着した時点で、**Process** メソッドが常に呼び出されます (この例では、ローソク足または数値のいずれかです)。

作成されたキューブをダイアグラムに追加するには、パレットの **Own elements** セクションで作成したキューブを選択する必要があります。

![Designer_Source_Code_Elem_01](../../../../../images/designer_source_code_elem_01.png)

> [!WARNING] 
> F# コードのキューブは、F# コードで作成されたストラテジーでは使用できません。[キューブから](../../using_visual_designer.md)作成されたストラテジーでのみ使用できます。

## 関連項目

[インジケーターの作成](create_own_indicator.md)
