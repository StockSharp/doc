# ボリュームプロファイル

「ボリュームプロファイル」スクリプトは、選択した期間における価格水準ごとの取引高分布を分析するためのツールです。トレーダーやクオンツアナリストは、主な取引活動がどの価格水準に集中していたかを可視化し、調査できます。

![hydra_analytics_volume_profile](../../../../images/hydra_analytics_volume_profile.png)

## 機能の説明

このスクリプトは取引データを集計し、さまざまな価格水準で約定した出来高を表示するプロファイルを形成します。この情報はチャート上に表現でき、異なる価格ポイントにわたる取引の密度を示します。

## 実用上の意義

ボリュームプロファイルを分析することで、主要な需要ゾーンと供給ゾーンを特定し、次の用途に利用できます。

- 市場参加者から大きな関心が寄せられている銘柄のサポートレベルとレジスタンスレベルを特定する。
- 出来高分布の変化に基づいて、現在のトレンドの強さ、または弱まりつつある可能性を評価する。
- 累積流動性が最大となる水準を考慮して、市場へのエントリーポイントとエグジットポイントを計画する。

## 取引およびクオンツ分析での応用

- **取引**: ボリュームプロファイルは、出来高分析に基づく戦略の開発に利用でき、主な取引オペレーションがどこで行われているかを明確に把握できます。
- **クオンツ分析**: 出来高分布に関するデータは、ある水準に蓄積された出来高に基づいて価格変動の可能性を予測するクオンツモデルへの入力として使用できます。

## スクリプトの実装

「ボリュームプロファイル」スクリプトは、次の手順を実行します。

1. **データ収集**: スクリプトは指定された期間の取引データを集計します。
2. **プロファイル形成**: 収集されたデータに基づいて、各価格水準での取引活動を反映するボリュームプロファイルを形成します。
3. **可視化**: スクリプトの実行結果はチャートまたはヒストグラムとして可視化され、各バーは特定の価格水準とその取引高に対応します。

StockSharp プラットフォーム内で「ボリュームプロファイル」スクリプトを利用することで、包括的な市場分析、十分な根拠に基づく取引仮説の構築、そして取引判断の品質向上が可能になります。

## C# のスクリプトコード

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// 価格水準別の出来高分布を計算する分析スクリプトです。
	/// </summary>
	public class PriceVolumeScript : IAnalyticsScript
	{
		Task IAnalyticsScript.Run(ILogReceiver logs, IAnalyticsPanel panel, SecurityId[] securities, DateTime from, DateTime to, IStorageRegistry storage, IMarketDataDrive drive, StorageFormats format, DataType dataType, CancellationToken cancellationToken)
		{
			if (securities.Length == 0)
			{
				logs.LogWarning("No instruments.");
				return Task.CompletedTask;
			}

			// スクリプトは 1 つの銘柄のみ処理できます
			var security = securities.First();

			// ローソク足ストレージを取得
			var candleStorage = storage.GetCandleMessageStorage(security, dataType, drive, format);

			// 指定期間で利用可能な日付を取得
			var dates = candleStorage.GetDates(from, to).ToArray();

			if (dates.Length == 0)
			{
				logs.LogWarning("no data");
				return Task.CompletedTask;
			}

			// 中間価格でローソク足をグループ化
			var rows = candleStorage.Load(from, to)
				.GroupBy(c => c.LowPrice + c.GetLength() / 2)
				.ToDictionary(g => g.Key, g => g.Sum(c => c.TotalVolume));

			// チャートに描画
			panel.CreateChart<decimal, decimal>()
				.Append(security.ToStringId(), rows.Keys, rows.Values, DrawStyles.Histogram);

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

# 価格水準別の出来高分布を計算する分析スクリプトです。
class price_volume_script(IAnalyticsScript):
	def Run(
		self,
		logs,
		panel,
		securities,
		from_date,
		to_date,
		storage,
		drive,
		format,
		data_type,
		cancellation_token
	):
		# 銘柄がないか確認
		if not securities:
			logs.LogWarning("No instruments.")
			return Task.CompletedTask

		# スクリプトは 1 つの銘柄のみ処理できます
		security = securities[0]

		if data_type is None:
			logs.LogWarning(f"サポートされていないデータ型 {data_type}。")
			return Task.CompletedTask

		message_type = data_type.MessageType

		# ローソク足ストレージを取得
		candle_storage = get_candle_storage(storage, security, data_type, drive, format)

		# 指定期間で利用可能な日付を取得
		dates = get_dates(candle_storage, from_date, to_date)

		if len(dates) == 0:
			logs.LogWarning("no data")
			return Task.CompletedTask

		# ローソク足を中間価格でグループ化し、その出来高を合計
		candles = load_range(candle_storage, message_type, from_date, to_date)
		rows_dict = {}
		for candle in candles:
			# ローソク足の中間価格を計算
			key = candle.LowPrice + get_length(candle) / 2
			# 同じ価格水準の出来高を合計
			rows_dict[key] = rows_dict.get(key, 0) + candle.TotalVolume

		# チャートに描画
		chart = create_chart(panel, float, float)
		chart.Append(to_string_id(security), list(rows_dict.keys()), list(rows_dict.values()), DrawStyles.Histogram)

		return Task.CompletedTask

```
