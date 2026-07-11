# 戦略の統計

## 概要

StockSharp プラットフォームは、取引戦略の統計分析のための包括的なシステムを提供します。これにより、トレーダーは有効性の評価、パラメーターの最適化、十分な情報に基づく意思決定を行えます。統計システムは、注文、取引、ポジション、損益指標など、取引のさまざまな側面からデータを収集して処理します。

## 目的と利点

取引戦略における統計分析は、いくつかの重要な機能を果たします。

1. **パフォーマンス測定**: 純利益、最大ドローダウン、リカバリーファクターなどの指標を使用した、戦略の成功度の定量的評価。

2. **リスク管理**: 最大ドローダウン率やポジションサイズ統計などの指標を通じた、戦略のリスクプロファイルの把握。

3. **最適化**: さまざまなパラメーターセット間で統計指標を比較することによる、最適な戦略パラメーターの発見。

4. **取引品質分析**: 取引分布、利益取引と損失取引の比率、1 取引あたりの平均利益の分析。

5. **運用指標**: レイテンシー統計や注文エラー率などの運用指標を追跡し、執行上の問題を特定します。

## 利用可能な統計指標

StockSharp の [IStatisticManager](xref:StockSharp.Algo.Statistics.IStatisticManager) インターフェイスは、複数のカテゴリに整理された多数の統計パラメーターへのアクセスを提供します。

### 損益統計

- 純利益
- 純利益 (%)
- 最大利益
- 最大ドローダウン
- 最大ドローダウン (%)
- 最大相対ドローダウン
- リカバリーファクター

### 取引統計

- 利益取引数
- 損失取引数
- 総取引数
- 1 取引あたりの平均利益
- 平均利益取引
- 平均損失取引
- 月/日あたりの取引数

### ポジション統計

- 最大ロングポジション
- 最大ショートポジション

### 注文統計

- 注文数
- 注文エラー数
- 最大/最小登録遅延
- 最大/最小キャンセル遅延

## Strategy クラスとの統合

[Strategy](xref:StockSharp.Algo.Strategies.Strategy) クラスは、実行中に統計を自動的に収集して計算します。統計マネージャーは `StatisticManager` プロパティを通じて利用でき、このプロパティは [IStatisticManager](xref:StockSharp.Algo.Statistics.IStatisticManager) インターフェイスを実装しています。

主要な統計値は、Strategy クラスのプロパティとしても直接表現されています。

- `PnL`: 損益値
- `Commission`: 支払済み手数料の合計
- `Slippage`: 合計スリッページ
- `Latency`: 注文操作の平均レイテンシー

## 可視化

StockSharp は、戦略統計を可視化するための `StatisticParameterGrid` という特別なグラフィカルコンポーネントを提供しています。これは `StockSharp.Xaml` 名前空間で利用できます。このグリッドは、すべての統計パラメーターを使いやすい形式で表示します。

グラフィカルコンポーネントの詳細については、[統計](../graphical_user_interface/strategies/statistics.md) のドキュメントを参照してください。

## 使用例

コード内で戦略統計を扱う例を次に示します。

```csharp
// 戦略を作成
var strategy = new SmaStrategy
{
	// 戦略パラメーターを設定
	Security = security,
	Portfolio = portfolio,
	Volume = 1,
	// SMA パラメーターを設定
	LongSma = 200,
	ShortSma = 50,
};

// 可視化のために戦略をチャートへ接続
var chart = new ChartPanel();
strategy.SetChart(chart);

// 統計マネージャーへアクセス
var statisticManager = strategy.StatisticManager;

// ユーザーインターフェイスに戦略統計を表示
// XAML に 'StatisticsGrid' として定義された StatisticParameterGrid があると仮定
StatisticsGrid.Parameters.Clear();
StatisticsGrid.Parameters.AddRange(statisticManager.Parameters);

// 戦略を開始
strategy.Start();

// 統計の変化に反応する必要がある場合
strategy.PnLChanged += () =>
{
	Console.WriteLine($"現在のPnL: {strategy.PnL}");
	
	// 個別の統計パラメーターにもアクセスできます
	var netProfit = statisticManager.Parameters
		.OfType<NetProfitParameter>()
		.FirstOrDefault();
		
	if (netProfit != null)
	{
		Console.WriteLine($"Net Profit: {netProfit.Value}");
	}
};

// ポジション統計を追跡する場合
strategy.PositionChanged += () =>
{
	Console.WriteLine($"現在ポジション: {strategy.Position}");
};
```

## カスタム統計

適切なインターフェイスを実装することで、独自の統計パラメーターを作成することもできます。

- [IStatisticParameter](xref:StockSharp.Algo.Statistics.IStatisticParameter): すべての統計パラメーターの基本インターフェイス
- [IPnLStatisticParameter](xref:StockSharp.Algo.Statistics.IPnLStatisticParameter): 損益に関連するパラメーター用
- [ITradeStatisticParameter](xref:StockSharp.Algo.Statistics.ITradeStatisticParameter): 取引に関連するパラメーター用
- [IPositionStatisticParameter](xref:StockSharp.Algo.Statistics.IPositionStatisticParameter): ポジションに関連するパラメーター用
- [IOrderStatisticParameter](xref:StockSharp.Algo.Statistics.IOrderStatisticParameter): 注文に関連するパラメーター用

カスタム統計パラメーターの簡単な例を次に示します。

```csharp
[Display(
	ResourceType = typeof(LocalizedStrings),
	Name = "自分のカスタムインジケーター",
	Description = "カスタムインジケーターの説明",
	GroupName = "カスタムパラメーター",
	Order = 1000
)]
public class MyCustomParameter : BasePnLStatisticParameter<decimal>
{
	public MyCustomParameter()
		: base(StatisticParameterTypes.Custom)
	{
	}

	public override void Add(DateTimeOffset marketTime, decimal pnl, decimal? commission)
	{
		// カスタム計算ロジック
		Value = /* カスタム計算 */;
	}
}

// 次に、それを戦略の StatisticManager に追加します
strategy.StatisticManager.Parameters.Add(new MyCustomParameter());
```

## まとめ

StockSharp の統計分析システムは、トレーダーが取引戦略を評価し最適化するための強力なツールを提供します。これらの統計を使用することで、戦略のパフォーマンスに関する有益な洞察を得て、改善すべき領域を特定し、データに基づく意思決定によって取引結果を向上させることができます。
