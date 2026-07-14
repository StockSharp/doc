# インジケーターを作成する

[API](../../../../api.md) で独自のインジケーターを作成する方法は、[カスタムインジケーター](../../../../api/indicators/custom_indicator.md) セクションで説明されています。このようなインジケーターは **Designer** と完全に互換性があります。

インジケーターを作成するには、**スキーム** パネルで **インジケーター** フォルダーを選択し、右クリックしてコンテキストメニューから **追加** を選択する必要があります。

![Designer ソースコードインジケーター 00](../../../../../images/designer_source_code_indicator_00.png)

インジケーターのコードは次のようになります。

```fsharp
/// <summary>
/// パラメーターを保存および読み込む方法を示すサンプルインジケーターです。
/// 入力価格を +20% または -20% 変更します。
///
/// その他の例:
/// https://github.com/StockSharp/StockSharp/tree/master/Algo/Indicators
///
/// ドキュメント:
/// https://doc.stocksharp.com/topics/designer/strategies/using_code/fsharp/create_own_indicator.html
/// </summary>
type EmptyIndicator() as this =
	inherit BaseIndicator()

	// 内部フィールド
	let mutable changeValue = 20
	let mutable counter = 0
	let mutable isFormedValue = false

	/// <summary>
	/// 入力価格を変更するために使用されるパーセンテージ値 (+/-)。
	/// </summary>
	member this.Change
		with get () = changeValue
		and set value =
			changeValue <- value
			this.Reset()

	/// <summary>
	/// インジケーターが形成済みかどうか (取引準備ができたかどうか) を定義します。
	/// </summary>
	override this.CalcIsFormed() = isFormedValue

	/// <summary>
	/// インジケーターを初期状態にリセットします。
	/// </summary>
	override this.Reset() =
		base.Reset()
		isFormedValue <- false
		counter <- 0

	/// <summary>
	/// 入力値を処理するメインロジックです。
	/// </summary>
	override this.OnProcess(input: IIndicatorValue) : IIndicatorValue =
		// 10 回に 1 回、"空" の値を返そうとします。
		if RandomGen.GetInt(0, 10) = 0 then
			// 空の値には時刻のみが含まれ、実データは含まれません。
			DecimalIndicatorValue(this, input.Time)
		else
			// 呼び出しごとにカウンターを増やします。
			counter <- counter + 1

			// 5 つの入力後、インジケーターは形成済みと見なされます。
			if counter = 5 then
				isFormedValue <- true

			let mutable value = input.ToDecimal()

			// +/- Change% の係数でランダムに変更します。
			let randomFactor = decimal (RandomGen.GetInt(-changeValue, changeValue)) / 100m
			value <- value + (value * randomFactor)

			// 最終的なインジケーター値を返します。
			let result = DecimalIndicatorValue(this, value, input.Time)
			// final としてマークするかどうかをランダムに決めます。
			result.IsFinal <- RandomGen.GetBool()
			result

	/// <summary>
	/// 指定された <see cref="SettingsStorage"/> からインジケーター設定を読み込みます。
	/// </summary>
	override this.Load(storage: SettingsStorage) =
		base.Load(storage)
		this.Change <- storage.GetValue<int>(nameof(this.Change))

	/// <summary>
	/// 指定された <see cref="SettingsStorage"/> にインジケーター設定を保存します。
	/// </summary>
	override this.Save(storage: SettingsStorage) =
		base.Save(storage)
		storage.SetValue(nameof(this.Change), this.Change)

	/// <summary>
	/// 現在の <see cref="Change"/> 値を含む文字列表現です。
	/// </summary>
	override this.ToString() =
		sprintf "変化: %d" this.Change

```

このインジケーターは入力値を受け取り、設定された **Change** パラメーター値に基づいて任意の偏差を加えます。

インジケーターメソッドの説明は、[カスタムインジケーター](../../../../api/indicators/custom_indicator.md) セクションで確認できます。

作成したインジケーターをダイアグラムに追加するには、[インジケーター](../../using_visual_designer/elements/common/indicator.md) キューブを使用し、その中で必要なインジケーターを指定する必要があります。

![Designer ソースコードインジケーター 01](../../../../../images/designer_source_code_indicator_01.png)

インジケーターコードで以前に設定した **Change** パラメーターは、プロパティパネルに表示されます。

> [!WARNING]
> F# コードのインジケーターは、F# コードで作成されたストラテジーでは使用できません。[キューブから](../../using_visual_designer.md)作成されたストラテジーでのみ使用できます。
