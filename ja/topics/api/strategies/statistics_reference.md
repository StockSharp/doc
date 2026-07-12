# 統計リファレンス

[StatisticManager](xref:StockSharp.Algo.Statistics.StatisticManager) は、[IStatisticParameter](xref:StockSharp.Algo.Statistics.IStatisticParameter) インスタンスのコレクションを管理します。各パラメーターは、戦略実行中に特定の指標を追跡します。利用可能なすべてのパラメーターは [StatisticParameterRegistry](xref:StockSharp.Algo.Statistics.StatisticParameterRegistry) を使用して作成されます。

戦略統計の扱い方の一般的な概要については、[統計](statistics.md) セクションを参照してください。

## インターフェイス

統計システムは、インターフェイスの階層の上に構築されています。各インターフェイスは、パラメーターを計算するためのデータソースを定義します。

| インターフェイス | 説明 |
|-----------|-------------|
| [IStatisticParameter](xref:StockSharp.Algo.Statistics.IStatisticParameter) | 基本インターフェイス: プロパティ `Name`, `Type`, `Value`, `DisplayName`, `Description`, `Category`, `Order`; メソッド `Reset()` |
| [IPnLStatisticParameter](xref:StockSharp.Algo.Statistics.IPnLStatisticParameter) | 損益に基づくパラメーター: メソッド `Add(marketTime, pnl, commission)` |
| [ITradeStatisticParameter](xref:StockSharp.Algo.Statistics.ITradeStatisticParameter) | 取引に基づくパラメーター: メソッド `Add(PnLInfo)` |
| [IOrderStatisticParameter](xref:StockSharp.Algo.Statistics.IOrderStatisticParameter) | 注文に基づくパラメーター: メソッド `New(order)`, `Changed(order)`, `RegisterFailed(fail)`, `CancelFailed(fail)` |
| [IPositionStatisticParameter](xref:StockSharp.Algo.Statistics.IPositionStatisticParameter) | ポジションに基づくパラメーター: メソッド `Add(marketTime, position)` |
| [IRiskFreeRateStatisticParameter](xref:StockSharp.Algo.Statistics.IRiskFreeRateStatisticParameter) | リスクフリーレートを持つパラメーター: プロパティ `RiskFreeRate` |
| [IBeginValueStatisticParameter](xref:StockSharp.Algo.Statistics.IBeginValueStatisticParameter) | 初期値を持つパラメーター: プロパティ `BeginValue` |

## 損益 (P&L) パラメーター

このグループのすべてのパラメーターは、[IPnLStatisticParameter](xref:StockSharp.Algo.Statistics.IPnLStatisticParameter) インターフェイスを実装し、[BasePnLStatisticParameter](xref:StockSharp.Algo.Statistics.BasePnLStatisticParameter`1) から継承します。これらは戦略の P&L 値が更新されるたびにデータを受け取ります。

| クラス | 説明 | 値の型 |
|-------|-------------|------------|
| [NetProfitParameter](xref:StockSharp.Algo.Statistics.NetProfitParameter) | 期間全体の純利益。現在の P&L 値と等しく設定されます | `decimal` |
| [NetProfitPercentParameter](xref:StockSharp.Algo.Statistics.NetProfitPercentParameter) | パーセンテージで表した純利益。`BeginValue` (初期資本) の設定が必要です。式: `pnl * 100 / BeginValue` | `decimal` |
| [MaxProfitParameter](xref:StockSharp.Algo.Statistics.MaxProfitParameter) | 最大利益 (期間全体で最も高い P&L 値) | `decimal` |
| [MaxProfitPercentParameter](xref:StockSharp.Algo.Statistics.MaxProfitPercentParameter) | パーセンテージで表した最大利益。`BeginValue` が必要です。式: `MaxProfit * 100 / BeginValue` | `decimal` |
| [MaxProfitDateParameter](xref:StockSharp.Algo.Statistics.MaxProfitDateParameter) | 最大利益に到達した日付 | `DateTime` |
| [MaxDrawdownParameter](xref:StockSharp.Algo.Statistics.MaxDrawdownParameter) | 最大絶対ドローダウン。エクイティカーブのピークと谷の差 | `decimal` |
| [MaxDrawdownPercentParameter](xref:StockSharp.Algo.Statistics.MaxDrawdownPercentParameter) | パーセンテージで表した最大ドローダウン。式: `MaxDrawdown * 100 / MaxEquity` | `decimal` |
| [MaxDrawdownDateParameter](xref:StockSharp.Algo.Statistics.MaxDrawdownDateParameter) | 最大ドローダウンの日付 | `DateTime` |
| [MaxRelativeDrawdownParameter](xref:StockSharp.Algo.Statistics.MaxRelativeDrawdownParameter) | 最大相対ドローダウン。ドローダウンとピーク時エクイティ値の比率として計算されます | `decimal` |
| [ReturnParameter](xref:StockSharp.Algo.Statistics.ReturnParameter) | 期間全体の相対リターン。谷から現在値までの最大成長を相対値で表します | `decimal` |
| [CommissionParameter](xref:StockSharp.Algo.Statistics.CommissionParameter) | 支払済み手数料の合計。すべての手数料値を累積します | `decimal` |
| [AverageDrawdownParameter](xref:StockSharp.Algo.Statistics.AverageDrawdownParameter) | 平均ドローダウン。完了済みおよび現在のすべてのドローダウンの算術平均 | `decimal` |
| [RecoveryFactorParameter](xref:StockSharp.Algo.Statistics.RecoveryFactorParameter) | リカバリーファクター。式: `NetProfit / MaxDrawdown` | `decimal` |
| [SharpeRatioParameter](xref:StockSharp.Algo.Statistics.SharpeRatioParameter) | シャープレシオ。式: `(annualized return - risk-free rate) / annualized standard deviation` | `decimal` |
| [SortinoRatioParameter](xref:StockSharp.Algo.Statistics.SortinoRatioParameter) | ソルティノレシオ。Sharpe に似ていますが、下方偏差のみを考慮します | `decimal` |
| [CalmarRatioParameter](xref:StockSharp.Algo.Statistics.CalmarRatioParameter) | カルマーレシオ。式: `NetProfit / MaxDrawdown` | `decimal` |
| [SterlingRatioParameter](xref:StockSharp.Algo.Statistics.SterlingRatioParameter) | スターリングレシオ。式: `NetProfit / AverageDrawdown` | `decimal` |

### リスク係数

[SharpeRatioParameter](xref:StockSharp.Algo.Statistics.SharpeRatioParameter) と [SortinoRatioParameter](xref:StockSharp.Algo.Statistics.SortinoRatioParameter) は、基本クラス [RiskAdjustedRatioParameter](xref:StockSharp.Algo.Statistics.RiskAdjustedRatioParameter) から継承し、[IRiskFreeRateStatisticParameter](xref:StockSharp.Algo.Statistics.IRiskFreeRateStatisticParameter) インターフェイスを実装します。

これらは次の設定をサポートします。

- **RiskFreeRate** -- 年率のリスクフリーレート (例: `0.03m` = 3%)
- **Period** -- リターン計算期間 (既定値 `TimeSpan.FromDays(1)`)

[CalmarRatioParameter](xref:StockSharp.Algo.Statistics.CalmarRatioParameter) と [SterlingRatioParameter](xref:StockSharp.Algo.Statistics.SterlingRatioParameter) は他のパラメーター (`NetProfitParameter`, `MaxDrawdownParameter`, `AverageDrawdownParameter`) に依存し、[StatisticParameterRegistry](xref:StockSharp.Algo.Statistics.StatisticParameterRegistry) 経由で作成された場合に自動的にリンクされます。

## 取引パラメーター

このグループのすべてのパラメーターは [ITradeStatisticParameter](xref:StockSharp.Algo.Statistics.ITradeStatisticParameter) インターフェイスを実装します。実行された各取引について、[PnLInfo](xref:StockSharp.Algo.PnL.PnLInfo) オブジェクトを通じてデータを受け取ります。

| クラス | 説明 | 値の型 |
|-------|-------------|------------|
| [TradeCountParameter](xref:StockSharp.Algo.Statistics.TradeCountParameter) | 総取引数 (`ClosedVolume > 0` の取引のみがカウントされます) | `int` |
| [WinningTradesParameter](xref:StockSharp.Algo.Statistics.WinningTradesParameter) | 利益取引数 (`ClosedVolume > 0` かつ `PnL > 0`) | `int` |
| [LossingTradesParameter](xref:StockSharp.Algo.Statistics.LossingTradesParameter) | 損失取引数 (`ClosedVolume > 0` かつ `PnL < 0`) | `int` |
| [RoundtripCountParameter](xref:StockSharp.Algo.Statistics.RoundtripCountParameter) | 完了した往復取引の数 (`ClosedVolume > 0` の決済取引) | `int` |
| [AverageTradeProfitParameter](xref:StockSharp.Algo.Statistics.AverageTradeProfitParameter) | 1 取引あたりの平均利益。式: `SumPnL / Count` | `decimal` |
| [AverageWinTradeParameter](xref:StockSharp.Algo.Statistics.AverageWinTradeParameter) | 利益取引の平均利益。`PnL > 0` の取引のみが考慮されます | `decimal` |
| [AverageLossTradeParameter](xref:StockSharp.Algo.Statistics.AverageLossTradeParameter) | 損失取引の平均損失。`PnL < 0` の取引のみが考慮されます | `decimal` |
| [ProfitFactorParameter](xref:StockSharp.Algo.Statistics.ProfitFactorParameter) | プロフィットファクター。式: `GrossProfit / GrossLoss` | `decimal` |
| [ExpectancyParameter](xref:StockSharp.Algo.Statistics.ExpectancyParameter) | 数学的期待値。式: `P(win) * AvgWin + P(loss) * AvgLoss` | `decimal` |
| [PerMonthTradeParameter](xref:StockSharp.Algo.Statistics.PerMonthTradeParameter) | 月あたりの平均取引数 | `decimal` |
| [PerDayTradeParameter](xref:StockSharp.Algo.Statistics.PerDayTradeParameter) | 日あたりの平均取引数 | `decimal` |
| [GrossProfitParameter](xref:StockSharp.Algo.Statistics.GrossProfitParameter) | 総利益。すべての利益取引の P&L の合計 (`PnL > 0`) | `decimal` |
| [GrossLossParameter](xref:StockSharp.Algo.Statistics.GrossLossParameter) | 総損失。すべての損失取引の P&L の合計 (`PnL < 0`、値は負) | `decimal` |

## ポジションパラメーター

このグループのパラメーターは [IPositionStatisticParameter](xref:StockSharp.Algo.Statistics.IPositionStatisticParameter) インターフェイスを実装します。ポジションが変化するたびにデータを受け取ります。

| クラス | 説明 | 値の型 |
|-------|-------------|------------|
| [MaxLongPositionParameter](xref:StockSharp.Algo.Statistics.MaxLongPositionParameter) | 最大ロングポジション。最も高い正のポジション値 | `decimal` |
| [MaxShortPositionParameter](xref:StockSharp.Algo.Statistics.MaxShortPositionParameter) | 最大ショートポジション。負のポジション値の最大絶対値 | `decimal` |

## 注文パラメーター

このグループのすべてのパラメーターは [IOrderStatisticParameter](xref:StockSharp.Algo.Statistics.IOrderStatisticParameter) インターフェイスを実装し、[BaseOrderStatisticParameter](xref:StockSharp.Algo.Statistics.BaseOrderStatisticParameter`1) から継承します。注文登録、変更、エラーに関するデータを受け取ります。

| クラス | 説明 | 値の型 |
|-------|-------------|------------|
| [OrderCountParameter](xref:StockSharp.Algo.Statistics.OrderCountParameter) | 登録された注文の総数 | `int` |
| [OrderRegisterErrorCountParameter](xref:StockSharp.Algo.Statistics.OrderRegisterErrorCountParameter) | 注文登録エラーの数 | `int` |
| [OrderInsufficientFundErrorCountParameter](xref:StockSharp.Algo.Statistics.OrderInsufficientFundErrorCountParameter) | 「資金不足」エラーの数 (型 `InsufficientFundException`) | `int` |
| [OrderCancelErrorCountParameter](xref:StockSharp.Algo.Statistics.OrderCancelErrorCountParameter) | 注文キャンセルエラーの数 | `int` |

## レイテンシーパラメーター

レイテンシーパラメーターも [IOrderStatisticParameter](xref:StockSharp.Algo.Statistics.IOrderStatisticParameter) を実装しますが、注文処理の時間特性を追跡します。

| クラス | 説明 | 値の型 |
|-------|-------------|------------|
| [MaxLatencyRegistrationParameter](xref:StockSharp.Algo.Statistics.MaxLatencyRegistrationParameter) | 最大注文登録レイテンシー (`Order.LatencyRegistration` プロパティ) | `TimeSpan` |
| [MinLatencyRegistrationParameter](xref:StockSharp.Algo.Statistics.MinLatencyRegistrationParameter) | 最小注文登録レイテンシー | `TimeSpan` |
| [MaxLatencyCancellationParameter](xref:StockSharp.Algo.Statistics.MaxLatencyCancellationParameter) | 最大注文キャンセルレイテンシー (`Order.LatencyCancellation` プロパティ) | `TimeSpan` |
| [MinLatencyCancellationParameter](xref:StockSharp.Algo.Statistics.MinLatencyCancellationParameter) | 最小注文キャンセルレイテンシー | `TimeSpan` |

## 使用方法

### 戦略統計へのアクセス

```cs
var strategy = new MyStrategy();

// 実行後に統計へアクセス
foreach (var param in strategy.StatisticManager.Parameters)
{
    Console.WriteLine($"{param.DisplayName}: {param.Value}");
}
```

### 特定のパラメーターの取得

```cs
// 純利益値を取得
var netProfit = strategy.StatisticManager.Parameters
    .OfType<NetProfitParameter>()
    .First();

Console.WriteLine($"純利益: {netProfit.Value}");
```

### 係数用のリスクフリーレートの設定

Sharpe と Sortino の各レシオを正しく計算するには、リスクフリーレートを設定する必要があります。

```cs
// すべての係数に 3% のリスクフリーレートを設定
foreach (var param in strategy.StatisticManager.Parameters
    .OfType<IRiskFreeRateStatisticParameter>())
{
    param.RiskFreeRate = 0.03m;
}
```

### パーセンテージパラメーター用の初期資本の設定

[NetProfitPercentParameter](xref:StockSharp.Algo.Statistics.NetProfitPercentParameter) と [MaxProfitPercentParameter](xref:StockSharp.Algo.Statistics.MaxProfitPercentParameter) パラメーターでは、初期資本値の設定が必要です。

```cs
// パーセンテージ計算用の初期資本を設定
foreach (var param in strategy.StatisticManager.Parameters
    .OfType<IBeginValueStatisticParameter>())
{
    param.BeginValue = 1_000_000m; // 1,000,000
}
```

### 統計のリセット

```cs
// すべての統計パラメーターをリセット
strategy.StatisticManager.Reset();
```

### 状態の保存と読み込み

すべてのパラメーターは、`IPersistable` インターフェイスによるシリアル化をサポートします。

```cs
// 保存
var storage = new SettingsStorage();
strategy.StatisticManager.Save(storage);

// 読み込み
strategy.StatisticManager.Load(storage);
```

## 関連項目

- [戦略の統計](statistics.md)
- [統計グラフィカルコンポーネント](../graphical_user_interface/strategies/statistics.md)
