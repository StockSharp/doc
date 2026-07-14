# 独自のキューブを作成する

[ダイアグラムからキューブ](../../using_visual_designer/composite_elements.md)を作成する場合と同様に、C# コードに基づいて独自のキューブを作成できます。このようなキューブは、ダイアグラムから作成したキューブよりも高機能になります。

コードからキューブを作成するには、**独自要素** フォルダー内に作成する必要があります。

![Designer ソースコード要素 00](../../../../../images/designer_source_code_elem_00.png)

以下の例では、キューブは [DiagramExternalElement](xref:StockSharp.Diagram.DiagramExternalElement) クラスを継承し、次のようになります。

```cs
/// <summary>
/// 入力ソケットと出力ソケットの使用方法を示すサンプルダイアグラム要素です。
///
/// https://doc.stocksharp.com/topics/Designer_Combine_Source_code_and_standard_elements.html
/// </summary>
public class EmptyDiagramElement : DiagramExternalElement
{
	private readonly DiagramElementParam<int> _minValue;

	public EmptyDiagramElement()
	{
		// パラメーターの作成方法を示すプロパティの例です。

		_minValue = AddParam("MinValue", 10)
			.SetBasic(true) // パラメーターを basic モードで表示します。
			.SetDisplay("パラメーター", "最小値", "最小値パラメーターの説明", 10);
	}

	// 出力ソケットは DiagramExternal 属性でマークされたイベントです。

	[DiagramExternal]
	public event Action<Unit> Output1;

	[DiagramExternal]
	public event Action<Unit> Output2;

	// 入力ソケットは DiagramExternal 属性でマークされたメソッドパラメーターです。

	// 新しい引数を受信するたびに Process メソッドを呼び出すにはコメント解除します。
	// (すべての入力引数を受信するまで待つ必要はありません)
	//public override bool WaitAllInput => false;

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

		// 開始前のロジックを追加します。
	}

	public override void Stop()
	{
		base.Stop();

		// 停止後のロジックを追加します。
	}

	public override void Reset()
	{
		base.Reset();

		// 内部状態をリセットするロジックを追加します。
	}
}
```

このコードでは、キューブに 2 つの入力ソケットと 2 つの出力ソケットがあります。入力ソケットは、メソッドに [DiagramExternalAttribute](xref:StockSharp.Diagram.DiagramExternalAttribute) 属性を適用することで定義されます。

```cs
[DiagramExternal]
public void Process(CandleMessage candle, Unit diff)
```

出力ソケットは、イベントに属性を適用することで定義されます。キューブの例では、このようなイベントが 2 つあります。


```cs
[DiagramExternal]
public event Action<Unit> Output1;

[DiagramExternal]
public event Action<Unit> Output2;
```

したがって、出力ソケットも 2 つになります。

さらに、キューブのプロパティを作成する方法を示しています。

```cs
_minValue = AddParam("MinValue", 10)
	.SetBasic(true) // パラメーターを basic モードで表示します。
	.SetDisplay("パラメーター", "最小値", "最小値パラメーターの説明", 10);
```

[DiagramElementParam](xref:StockSharp.Diagram.DiagramElementParam`1) クラスを使用すると、設定を保存および復元する方法が自動的に利用されます。

**MinValue** プロパティは基本プロパティとしてマークされており、[基本プロパティ](../../using_visual_designer/diagram_panel.md) モードで表示されます。

コメントアウトされている [WaitAllInput](xref:StockSharp.Diagram.DiagramExternalElement.WaitAllInput) プロパティは、入力ソケットを持つメソッド呼び出しのタイミングを担います。

```cs
//public override bool WaitAllInput => false;
```

コメント解除すると、少なくとも 1 つの値が到着した時点で、**Process** メソッドが常に呼び出されます (この例では、ローソク足または数値のいずれかです)。

作成されたキューブをダイアグラムに追加するには、パレットの **独自要素** セクションで作成したキューブを選択する必要があります。

![Designer ソースコード要素 01](../../../../../images/designer_source_code_elem_01.png)

> [!WARNING]
> C# コードのキューブは、C# コードで作成されたストラテジーでは使用できません。[キューブから](../../using_visual_designer.md)作成されたストラテジーでのみ使用できます。

## 関連項目

[インジケーターの作成](create_own_indicator.md)
