# 日中出来高

"Intraday Volume" スクリプトは、単一の取引セッション内で証券の取引出来高が時間別にどのように分布するかを分析するためのツールです。StockSharp プラットフォーム内で使用するように設計されており、市場行動の詳細な研究と取引戦略の最適化を目指すトレーダーやクオンツアナリストを対象としています。

![hydra_analytics_intraday_volume](../../../../images/hydra_analytics_intraday_volume.png)

## 機能説明

このスクリプトは、選択された期間の取引操作に関するデータを収集し、グラフ形式で表示します。これにより、ユーザーは時間別の取引出来高の変化を可視化できます。これにより、1 日のうちどの時間帯で取引活動が増加または減少するかを評価できます。

## 実務上の意義

- **取引向け**: ピーク時間と非ピーク時間を理解することで、最も活発な市場期間を特定し、ポジションへのエントリーまたはエグジットのタイミングに関する判断に影響を与えます。
- **クオンツ分析向け**: クオンツアナリストは、日中出来高データを使用して、出来高指標に基づいて市場行動を予測する数学モデルやアルゴリズムを作成できます。

## 時間別分布

取引出来高の時間別分布は市場動向を明らかにし、主要な取引活動がある時間帯を強調します。これは、トレンドの変化、サポートおよびレジスタンス水準、ならびに流動性の増加または不足が生じる可能性のあるタイミングを示す場合があります。

## データの応用

"Intraday Volume" スクリプトは、より広範な市場分析システムに統合でき、次の用途に使用できるデータを提供します。

- **戦略の適応**: 市場活動水準に応じて取引アルゴリズムのパラメーターを調整します。
- **リスク評価**: 時間帯に応じた大きな価格変動の確率を計算します。

StockSharp 取引プラットフォーム内で "Intraday Volume" スクリプトを使用することで、トレーダーやアナリストは市場活動に関する具体的なデータに基づいて判断し、現在の取引条件に最適に合うよう戦略を適応させることができます。

## C# のスクリプトコード

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// 分析スクリプト。時間別の最大出来高の分布を計算します。
	/// </summary>
	public class TimeVolumeScript : IAnalyticsScript
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

			// 指定された期間で利用可能な日付を取得
			var dates = candleStorage.GetDates(from, to).ToArray();

			if (dates.Length == 0)
			{
				logs.LogWarning("no data");
				return Task.CompletedTask;
			}

			// 始値時刻 (時刻部分のみ) でローソク足をグループ化し、1 時間に切り詰める
			var rows = candleStorage.Load(from, to)
				.GroupBy(c => c.OpenTime.TimeOfDay.Truncate(TimeSpan.FromHours(1)))
				.ToDictionary(g => g.Key, g => g.Sum(c => c.TotalVolume));

			// 計算結果をグリッドに入れる
			var grid = panel.CreateGrid("Time", "Volume");

			foreach (var row in rows)
				grid.SetRow(row.Key, row.Value);

			// Volume 列で並べ替え (降順)
			grid.SetSort("Volume", false);

			return Task.CompletedTask;
		}
	}
}

```

## Python のスクリプトコード

```python
import clr

# .NET 参照を追加
clr.AddReference("StockSharp.Algo.Analytics")
clr.AddReference("StockSharp.Messages")
clr.AddReference("Ecng.Drawing")

from Ecng.Drawing import DrawStyles
from System import TimeSpan
from System.Threading.Tasks import Task
from StockSharp.Algo.Analytics import IAnalyticsScript
from storage_extensions import *
from candle_extensions import *
from chart_extensions import *
from indicator_extensions import *

# 分析スクリプト。時間別の最大出来高の分布を計算します。
class time_volume_script(IAnalyticsScript):
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
			logs.LogWarning(f"Unsupported data type {data_type}.")
			return Task.CompletedTask

		message_type = data_type.MessageType

		# ローソク足ストレージを取得
		candle_storage = get_candle_storage(storage, security, data_type, drive, format)

		# 指定された期間で利用可能な日付を取得
		dates = get_dates(candle_storage, from_date, to_date)

		if len(dates) == 0:
			logs.LogWarning("no data")
			return Task.CompletedTask

		# 始値時刻 (時間単位で切り詰め) でローソク足をグループ化し、出来高を合計
		candles = load_range(candle_storage, message_type, from_date, to_date)
		rows = {}
		for candle in candles:
			time_of_day = candle.OpenTime.TimeOfDay
			truncated = TimeSpan.FromHours(int(time_of_day.TotalHours))
			rows[truncated] = rows.get(truncated, 0) + candle.TotalVolume

		# 計算結果をグリッドに入れる
		grid = panel.CreateGrid("Time", "Volume")

		for key, value in rows.items():
			grid.SetRow(key, value)

		# Volume 列で降順に並べ替え
		grid.SetSort("Volume", False)

		return Task.CompletedTask

```
