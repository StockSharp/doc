# インジケーター

"Indicator" スクリプトは、StockSharp プラットフォーム内でテクニカル分析インジケーターを扱う方法を示すことを目的としています。ユーザーは履歴データを読み込み、さまざまなインジケーターを適用し、その結果をチャートに表示できます。このアプローチは、市場トレンドの分析と、情報に基づく取引判断に役立ちます。

![hydra_analytics_indicator](../../../../images/hydra_analytics_indicator.png)

## 機能

このスクリプトは次の機能を提供します。

- **履歴データの読み込み**: 関心のある金融商品を選択し、指定された期間の履歴データを読み込みます。
- **インジケーターの適用**: 読み込んだデータに 1 つ以上のテクニカル分析インジケーターを適用します。
- **可視化**: データと分析結果をインジケーターとともにチャート上に表示し、市場動向を明確に把握できるようにします。

## インジケーターの例

このスクリプトは、次を含むがこれらに限定されない幅広いインジケーターを扱えます。

- **移動平均 (MA)**: 一定期間の平均価格を表し、トレンドの識別に役立ちます。
- **相対力指数 (RSI)**: 価格変化の大きさと速度を評価し、買われ過ぎまたは売られ過ぎの状態を識別するのに役立ちます。
- **ボリンジャーバンド (BB)**: 移動平均と標準偏差に基づいて、価格の範囲とボラティリティを示します。

## 取引と分析での応用

このスクリプトを通じてテクニカル分析インジケーターを使用すると、次が可能になります。

- **トレンドの識別**: エントリー戦略とエグジット戦略を計画するために、市場の動きの方向を検出します。
- **反転ポイントの識別**: 市場トレンドの方向が変化する可能性のあるタイミングを判断します。
- **ボラティリティの分析**: 価格の不安定性の水準を評価し、戦略を市場状況に適応させます。

## スクリプトでの実装

スクリプトを使用するには、次の手順を実行する必要があります。

1. **銘柄と期間の選択**: 分析対象の証券と時間枠を決定します。
2. **インジケーターの適用**: データに適用するインジケーターを選択し、パラメーターを設定します。
3. **結果の表示**: 分析のため、履歴データとインジケーターをチャート上に可視化します。

"Indicator" スクリプトは、金融市場を詳細に分析するための強力なツールを提供し、トレーダーやアナリストがこれらのインジケーターを使用して効果的な取引戦略を開発できるようにします。

## C# のスクリプトコード

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// 分析スクリプト。ROC インジケーターを使用します。
	/// </summary>
	public class IndicatorScript : IAnalyticsScript
	{
		Task IAnalyticsScript.Run(ILogReceiver logs, IAnalyticsPanel panel, SecurityId[] securities, DateTime from, DateTime to, IStorageRegistry storage, IMarketDataDrive drive, StorageFormats format, DataType dataType, CancellationToken cancellationToken)
		{
			if (securities.Length == 0)
			{
				logs.LogWarning("No instruments.");
				return Task.CompletedTask;
			}

			// ローソク足とインジケーター系列用に 2 つのペインを作成
			var candleChart = panel.CreateChart<DateTimeOffset, decimal>();
			var indicatorChart = panel.CreateChart<DateTimeOffset, decimal>();

			foreach (var security in securities)
			{
				// ユーザーがスクリプト実行をキャンセルした場合は計算を停止する
				if (cancellationToken.IsCancellationRequested)
					break;

				var candlesSeries = new Dictionary<DateTimeOffset, decimal>();
				var indicatorSeries = new Dictionary<DateTimeOffset, decimal>();

				// ROC を作成
				var roc = new RateOfChange();

				// ローソク足ストレージを取得
				var candleStorage = storage.GetCandleMessageStorage(security, dataType, drive, format);

				foreach (var candle in candleStorage.Load(from, to))
				{
					// 系列を埋める
					candlesSeries[candle.OpenTime] = candle.ClosePrice;
					indicatorSeries[candle.OpenTime] = roc.Process(candle).ToDecimal();
				}

				// 系列をチャートに描画
				candleChart.Append($"{security} (close)", candlesSeries.Keys, candlesSeries.Values);
				indicatorChart.Append($"{security} (ROC)", indicatorSeries.Keys, indicatorSeries.Values);
			}

			return Task.CompletedTask;
		}
	}
}

```

## Python のスクリプトコード

```python
import clr

# .NET 参照を追加
clr.AddReference("StockSharp.Messages")
clr.AddReference("StockSharp.Algo.Analytics")
clr.AddReference("Ecng.Drawing")

from Ecng.Drawing import DrawStyles
from System.Threading.Tasks import Task
from StockSharp.Algo.Analytics import IAnalyticsScript
from StockSharp.Algo.Indicators import ROC
from storage_extensions import *
from candle_extensions import *
from chart_extensions import *
from indicator_extensions import *

# 分析スクリプト。ROC インジケーターを使用します。
class indicator_script(IAnalyticsScript):
	def Run(self, logs, panel, securities, from_date, to_date, storage, drive, format, data_type, cancellation_token):
		if not securities:
			logs.LogWarning("No instruments.")
			return Task.CompletedTask

		# ローソク足とインジケーター系列用に 2 つのペインを作成
		candle_chart = create_chart(panel, datetime, float)
		indicator_chart = create_chart(panel, datetime, float)

		if data_type is None:
			logs.LogWarning(f"サポートされていないデータ型 {data_type}。")
			return Task.CompletedTask

		message_type = data_type.MessageType

		for security in securities:
			# ユーザーがスクリプト実行をキャンセルした場合は計算を停止する
			if cancellation_token.IsCancellationRequested:
				break

			candles_series = {}
			indicator_series = {}

			# ROC を作成
			roc = ROC()

			# ローソク足ストレージを取得
			candle_storage = get_candle_storage(storage, security, data_type, drive, format)

			for candle in load_range(candle_storage, message_type, from_date, to_date):
				# 系列を埋める
				candles_series[candle.OpenTime] = candle.ClosePrice
				indicator_series[candle.OpenTime] = to_decimal(process_candle(roc, candle))

			# 系列をチャートに描画
			candle_chart.Append(
				f"{security} (close)",
				list(candles_series.keys()),
				list(candles_series.values())
			)
			indicator_chart.Append(
				f"{security} (ROC)",
				list(indicator_series.keys()),
				list(indicator_series.values())
			)

		return Task.CompletedTask

```
