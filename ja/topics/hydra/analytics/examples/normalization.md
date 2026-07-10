# 終値の正規化

"Closing Price Normalization" スクリプトは、金融商品の終値を標準化し、異なる資産を統一されたスケールで比較および分析できるように設計されています。これは、価格水準やボラティリティが異なる銘柄を比較する場合に特に有用です。

![hydra_analytics_normalize](../../../../images/hydra_analytics_normalize.png)

## スクリプト動作の説明

このスクリプトは、選択された正規化方法に従って終値データをスケーリングまたは変換することで適応させます。その結果、定量的な比較や複数銘柄分析に使用できる標準化された値のセットが得られます。

## 正規化の応用

- **価格スケールの統一**: 正規化は、さまざまな銘柄のデータを単一のスケールにそろえるのに役立ち、視覚的な比較と分析評価を簡素化します。
- **相関分析**: 標準化されたデータにより、資産間の相関関係を識別し、分散ポートフォリオを構築できます。
- **指数とモデルの作成**: 正規化された価格は、複合指数、価格モデル、その他の定量的研究の作成に使用されます。

## 正規化の方法論

正規化には、次の方法が含まれる場合があります。

1. **スケーリング**: 終値を 0 から 1 などの一定の値範囲に調整します。
2. **Z スコア**: 値が平均から何標準偏差離れているかを示す Z スコアを使用して終値を変換します。
3. **対数化**: データの分散を平滑化し、極端な値の影響を低減するために対数変換を適用します。

## スクリプトの実装

正規化プロセスには通常、次の手順が含まれます。

1. **正規化の選択**: 分析目的とデータ特性に基づいて正規化方法を決定します。
2. **データ処理**: 選択した正規化方法を各銘柄の終値に適用します。
3. **結果分析**: 正規化されたデータを、その後の分析や銘柄比較に使用します。

"Closing Price Normalization" スクリプトは、取引およびクオンツ分析のためのデータ準備において重要なツールであり、トレーダーやアナリストがさまざまな戦略や研究の中で金融資産をより正確に比較および評価できるようにします。

## C# のスクリプトコード

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// 分析スクリプト。証券の終値を正規化し、同じチャートに表示します。
	/// </summary>
	public class NormalizePriceScript : IAnalyticsScript
	{
		Task IAnalyticsScript.Run(ILogReceiver logs, IAnalyticsPanel panel, SecurityId[] securities, DateTime from, DateTime to, IStorageRegistry storage, IMarketDataDrive drive, StorageFormats format, DataType dataType, CancellationToken cancellationToken)
		{
			if (securities.Length == 0)
			{
				logs.LogWarning("No instruments.");
				return Task.CompletedTask;
			}

			var chart = panel.CreateChart<DateTimeOffset, decimal>();

			foreach (var security in securities)
			{
				// ユーザーがスクリプト実行をキャンセルした場合は計算を停止する
				if (cancellationToken.IsCancellationRequested)
					break;

				var series = new Dictionary<DateTimeOffset, decimal>();

				// ローソク足ストレージを取得
				var candleStorage = storage.GetCandleMessageStorage(security, dataType, drive, format);

				decimal? firstClose = null;

				foreach (var candle in candleStorage.Load(from, to))
				{
					firstClose ??= candle.ClosePrice;

					// 最初の終値で割って終値を正規化
					series[candle.OpenTime] = candle.ClosePrice / firstClose.Value;
				}

				// 系列をチャートに描画
				chart.Append(security.ToStringId(), series.Keys, series.Values);
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
from storage_extensions import *
from candle_extensions import *
from chart_extensions import *
from indicator_extensions import *

# 分析スクリプト。証券の終値を正規化し、同じチャートに表示します。
class normalize_price_script(IAnalyticsScript):
	def Run(self, logs, panel, securities, from_date, to_date, storage, drive, format, data_type, cancellation_token):
		if not securities:
			logs.LogWarning("No instruments.")
			return Task.CompletedTask

		chart = create_chart(panel, datetime, float)

		if data_type is None:
			logs.LogWarning(f"サポートされていないデータ型 {data_type}。")
			return Task.CompletedTask

		message_type = data_type.MessageType

		for security in securities:
			# ユーザーがスクリプト実行をキャンセルした場合は計算を停止する
			if cancellation_token.IsCancellationRequested:
				break

			series = {}

			# ローソク足ストレージを取得
			candle_storage = get_candle_storage(storage, security, data_type, drive, format)

			first_close = None

			for candle in load_range(candle_storage, message_type, from_date, to_date):
				if first_close is None:
					first_close = candle.ClosePrice

				# 最初の終値で割って終値を正規化
				series[candle.OpenTime] = candle.ClosePrice / first_close

			# 系列をチャートに描画
			chart.Append(to_string_id(security), list(series.keys()), list(series.values()))

		return Task.CompletedTask

```
