# 高度な最適化

## 概要

StockSharp は、ストラテジー最適化プロセスを細かく調整するための高度なツールを提供します。このセクションでは、[GeneticSettings](xref:StockSharp.Algo.Strategies.Optimization.GeneticSettings) による遺伝的アルゴリズム設定、[FitnessFormulaProvider](xref:StockSharp.Algo.Strategies.Optimization.FitnessFormulaProvider) によるカスタム適応度式のコンパイル、[OptimizationProgressTracker](xref:StockSharp.Algo.Strategies.Optimization.OptimizationProgressTracker) による進捗監視という高度なコンポーネントについて説明します。

## GeneticSettings

[GeneticSettings](xref:StockSharp.Algo.Strategies.Optimization.GeneticSettings) クラスは、遺伝的最適化アルゴリズムのすべての側面を管理します。このクラスは `StockSharp.Algo.Strategies.Optimization` 名前空間にあります。

### 母集団と世代のパラメーター

| プロパティ | 型 | デフォルト | 説明 |
|----------|------|---------|-------------|
| `Population` | int | 8 | 初期母集団サイズ |
| `PopulationMax` | int | 16 | 最大母集団サイズ |
| `GenerationsMax` | int | 20 | 最大世代数 |
| `GenerationsStagnation` | int | 5 | 停滞時に停止 (改善のない世代数) |

### 確率パラメーター

| プロパティ | 型 | デフォルト | 説明 |
|----------|------|---------|-------------|
| `MutationProbability` | decimal | 0.1 | 突然変異確率 (0-1) |
| `CrossoverProbability` | decimal | 0.8 | 交叉確率 (0-1) |

### 遺伝的オペレーター

| プロパティ | デフォルト | 説明 |
|----------|---------|-------------|
| `Reinsertion` | ElitistReinsertion | 世代置換戦略 |
| `Mutation` | UniformMutation | 突然変異オペレーター |
| `Crossover` | OnePointCrossover | 交叉オペレーター |
| `Selection` | TournamentSelection | 選択オペレーター |

## FitnessFormulaProvider

[FitnessFormulaProvider](xref:StockSharp.Algo.Strategies.Optimization.FitnessFormulaProvider) クラスは、最適化中にストラテジーを評価するために使用される C# の文字列表現を `Func<Strategy, decimal>` 関数へコンパイルします。

### Compile メソッド

```cs
var fitnessProvider = new FitnessFormulaProvider();
Func<Strategy, decimal> fitness = fitnessProvider.Compile("PnL / (MaxDD + 1)");
```

### 使用可能な変数

適応度関数の式では、ストラテジー統計指標に対応する次の変数を使用できます。

| 変数 | 説明 |
|----------|-------------|
| PnL | 純利益 |
| WinTrades | 勝ち取引 |
| LosTrades | 負け取引 |
| TCount | 取引総数 |
| RTrip | 往復取引 |
| AvgTPnL | 1 取引あたりの平均利益 |
| AvgWTrades | 平均勝ち取引 |
| AvgLTrades | 平均負け取引 |
| MaxLong | 最大ロングポジション |
| MaxShort | 最大ショートポジション |
| MaxPnL | 最大利益 |
| MaxDD | 最大ドローダウン |
| MaxRelDD | 最大相対ドローダウン |
| Ret | リターン |
| Recovery | リカバリーファクター |
| MaxLatReg | 最大登録レイテンシ |
| MaxLatCan | 最大キャンセルレイテンシ |
| MinLatReg | 最小登録レイテンシ |
| MinLatCan | 最小キャンセルレイテンシ |
| OrdCount | 注文数 |
| OrdRegErrCount | 登録エラー |
| OrdCancelErrCount | キャンセルエラー |
| OrdFundErrCount | 資金不足エラー |

変数は任意の数式で組み合わせることができます。例: `"PnL - MaxDD * 2"`、`"Recovery * WinTrades"`、`"PnL / (MaxDD + 1)"`。

## OptimizationProgressTracker

[OptimizationProgressTracker](xref:StockSharp.Algo.Strategies.Optimization.OptimizationProgressTracker) クラスは、最適化プロセスを便利に監視する機能を提供します。

### プロパティ

| プロパティ | 型 | 説明 |
|----------|------|-------------|
| `TotalIterations` | int | 総イテレーション数 |
| `CompletedIterations` | int | 完了したイテレーション数 |
| `TotalProgress` | double | 全体の進捗 (0-100) |
| `StartedAt` | DateTimeOffset | 最適化開始時刻 |
| `Elapsed` | TimeSpan | 経過時間 |
| `Remaining` | TimeSpan | 推定残り時間 |

### メソッド

| メソッド | 説明 |
|--------|-------------|
| `IterationCompleted()` | 1 回のイテレーションを完了としてマークする |
| `Reset(totalIterations)` | 新しい実行のためにトラッカーをリセットする |

## オプティマイザークラス

StockSharp は、`BaseOptimizer` を継承する 2 つのオプティマイザーを提供します。

- **[BruteForceOptimizer](xref:StockSharp.Algo.Strategies.Optimization.BruteForceOptimizer)** -- すべてのパラメーター組み合わせを総当たりで列挙します。小さなパラメーター空間に適しています。
- **[GeneticOptimizer](xref:StockSharp.Algo.Strategies.Optimization.GeneticOptimizer)** -- 遺伝的アルゴリズムです。多数のパラメーターがある場合に効率的で、最適解へ自動的に収束します。

## 使用例

```cs
var geneticSettings = new GeneticSettings
{
	Population = 16,
	PopulationMax = 32,
	GenerationsMax = 50,
	GenerationsStagnation = 10,
	MutationProbability = 0.15m,
	CrossoverProbability = 0.75m,
};

// カスタム適応度関数の式
var fitnessProvider = new FitnessFormulaProvider();
var fitness = fitnessProvider.Compile("PnL / (MaxDD + 1)");

// 遺伝的オプティマイザーを作成
var optimizer = new GeneticOptimizer(
	new CollectionSecurityProvider(new[] { security }),
	new CollectionPortfolioProvider(new[] { portfolio }),
	storageRegistry,
	Paths.FileSystem);

optimizer.Settings.Population = geneticSettings.Population;
optimizer.Settings.PopulationMax = geneticSettings.PopulationMax;
optimizer.Settings.GenerationsMax = geneticSettings.GenerationsMax;
optimizer.Settings.GenerationsStagnation = geneticSettings.GenerationsStagnation;
optimizer.Settings.MutationProbability = geneticSettings.MutationProbability;
optimizer.Settings.CrossoverProbability = geneticSettings.CrossoverProbability;

// 進捗監視
var tracker = new OptimizationProgressTracker();
tracker.Reset(totalIterations: 100);

optimizer.SingleProgressChanged += (strategy, parameters, progress) =>
{
	if (progress == 100)
	{
		tracker.IterationCompleted();
		Console.WriteLine($"進捗: {tracker.TotalProgress:F1}%, " +
			$"残り: {tracker.Remaining:hh\\:mm\\:ss}");
	}
};
```

## 関連項目

- [ストラテジー最適化](../testing/optimization.md)
- [ストラテジー統計](statistics.md)
- [Designer での遺伝的最適化](../../designer/optimization/genetic.md)
