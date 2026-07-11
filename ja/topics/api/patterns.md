# パターン

**パターン** (英語: pattern - モデル、サンプル) は、テクニカル分析において、価格データ、出来高、またはインジケーターの安定して繰り返し現れる組み合わせを指します。パターン分析は、テクニカル分析の公理の 1 つである「歴史は繰り返す」に基づいています。つまり、繰り返し現れるデータの組み合わせは類似した結果につながると考えられています。

パターンは、テクニカル分析の「**テンプレート**」または「**図形**」とも呼ばれます。

パターンは慣例的に次のように分類されます。

- 不確定 (現在のトレンドの継続と変化の両方につながる可能性があります)。
- 現在のトレンドの継続パターン。
- 既存トレンドの反転パターン。

## パターンの使用

### Designer で

[Designer](../designer.md) には、取引戦略で使用できる組み込みのプリセット ローソク足パターンがあります。パターンは [インジケーター](../designer/strategies/using_visual_designer/elements/common/indicator.md) キューブから呼び出し、その後、対応する値を選択します。パターン自体は、右側のウィンドウにあるドロップダウン リストから選択します。

![IndicatorPatternCommon](../../images/indicatorpatterncommon00.png)

既存のパターンを編集したり、独自のカスタム パターンを追加したりすることもできます。これを行うには、![Designer edit button](../../images/designer_creating_repository_of_historical_data_01.png) ボタンをクリックします。すると、パターン編集ウィンドウが表示されます。

![パターン のスクリーンショット](../../images/indicatorpatterncommon01.png)

独自のパターンを作成するには、ウィンドウ上部の ![Designer プラスボタン](../../images/designer_panel_circuits_01_button.png) ボタンをクリックします。![Designer 削除ボタン](../../images/designer_delete_button.png) ボタンをクリックすると、パターンが削除されます。

### Terminal で

[Terminal](../terminal.md) では、パターンは他のインジケーターと同様にチャートへ追加されます。これを行うには、チャートを右クリックし、使用可能なインジケーターのリストから適切なインジケーターを選択するだけです。

### StockSharp API で

[S#](../api.md) を使用する場合 (または Designer で [コードから戦略](../designer/strategies/using_code.md) を作成する場合)、パターンの操作は他のインジケーターと同じように行います。使用例:

```cs
// パターン インジケーターを作成します
var patternIndicator = new CandlePatternIndicator
{
	// 目的のパターンを設定します
	Pattern = new ExpressionCandlePattern("自分のパターン", new[]
	{
		new CandleExpressionCondition(Paths.FileSystem, "C > O"), // 現在のローソク足は上昇しています
		new CandleExpressionCondition(Paths.FileSystem, "pC < pO") // 前のローソク足は下降しています
	})
};

// インジケーターをコレクションに追加します
Indicators.Add(patternIndicator);

// ローソク足を処理します
var result = patternIndicator.Process(candle);

// 結果を確認します
if (result.GetValue<bool>())
{
	// パターンが検出されたため、必要な処理を実行します
}
```

## パターン記述形式

パターンを編集する際、各行は個別のローソク足を表します。最上部の行が現在のローソク足であり、それに応じて 2 行目は 1 本前のローソク足、3 行目以降は 2 本以上前のローソク足です。

エディターは次のパラメーターを使用します。
- O - 始値、
- H - 高値、
- L - 安値、
- C - 終値、
- V - 出来高、
- OI - 未決済建玉、
- B - ローソク足の実体、
- LEN - ローソク足の長さ (高値から安値まで)、
- BS - ローソク足の下ヒゲ、
- TS - ローソク足の上ヒゲ。

パラメーターでは、目的の値を参照するために次のインデックス (参照) を使用できます。たとえば、終値の場合:
- C: 現在のローソク足の終値、
- C1: 現在のローソク足の次の 1 本目のローソク足の終値、
- C2: 現在のローソク足の次の 2 本目のローソク足の終値、
- pC: 前のローソク足の終値、
- pC1: 前のローソク足のさらに前のローソク足の終値、
すべての参照は現在のパターンの範囲内になければなりません。たとえば、3 Black Crows パターンの範囲は現在のローソク足と 2 本前までのローソク足で構成されるため、3 本前のローソク足を参照することはできません。

パラメーターの相関を追加で検証するには、論理 AND を表す式 && を使用します。

パターンを記述する際には、次の関数も使用できます: abs, acos, asin, atan, ceiling, cos, exp, floor, log, log10, max, min, pow, round, sign, sin, sqrt, tan, truncate。関数の使用方法の詳細は、[数式](../designer/strategies/using_visual_designer/elements/common/formula.md) キューブの説明で解説されています。

コードで [ExpressionCandlePattern](xref:StockSharp.Algo.Candles.Patterns.ExpressionCandlePattern) を使用する場合、数式は上記と同じルールで作成され、同じ変数を使用します。

## 標準パターン

既存のパターンに基づいてパターンをすばやく作成するには、パターン エディター ウィンドウの下部にあるセクションを使用できます。ウィンドウ下部の ![Designer プラスボタン](../../images/designer_panel_circuits_01_button.png) ボタンをクリックすると、反対側のドロップダウン リストで選択したパターンのロジックが編集ウィンドウに追加されます。ウィンドウ下部の ![Designer 削除ボタン](../../images/designer_delete_button.png) ボタンは、編集ウィンドウ内の選択された行を削除します。

## 高度な機能

- [ComplexCandlePattern](xref:StockSharp.Algo.Candles.Patterns.ComplexCandlePattern) - 複数のパターンを 1 つの複合パターンに組み合わせ、より複雑な分析を行えるようにします。
- [ICandlePatternProvider](xref:StockSharp.Algo.Candles.Patterns.ICandlePatternProvider) - カスタム パターンの読み込みと保存を可能にするパターン プロバイダー インターフェイス。
