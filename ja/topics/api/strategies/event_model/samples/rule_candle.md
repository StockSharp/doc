# 単一ローソク足のルール

## 概要

`SimpleCandleRulesStrategy` は、StockSharp でローソク足に対するルールの使用を示すストラテジーです。ローソク足の出来高を追跡し、特定の条件が満たされたときに情報をログへ記録します。

## 主要コンポーネント

```cs
// 主要コンポーネント
public class SimpleCandleRulesStrategy : Strategy
{
}
```

## OnStarted メソッド

ストラテジー開始時に呼び出されます。

- 5分足へのサブスクリプションを初期化します
- ローソク足を処理するためのルールを設定します

```cs
// OnStarted メソッド
protected override void OnStarted2(DateTime time)
{
	var subscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security)
	{
		// その場で圧縮するモードよりも、すぐに使用できるローソク足の方がはるかに高速
		// オプティマイザーを高速化するため圧縮をオフにする（!!! ローソク足があることを確認してください）

		//MarketData =
		//{
		//    BuildMode = MarketDataBuildModes.Build,
		//    BuildFrom = DataType.Ticks,
		//}
	};
	Subscribe(subscription);

	var i = 0;
	var diff = "10%".ToUnit();

	this.WhenCandlesStarted(subscription)
		.Do((candle) =>
		{
			i++;

			this
				.WhenTotalVolumeMore(candle, diff)
				.Do((candle1) =>
				{
	LogInfo($"ルール WhenCandlesStarted と WhenTotalVolumeMore candle={candle1}");
	LogInfo($"ルール WhenCandlesStarted と WhenTotalVolumeMore i={i}");
				})
				.Once().Apply(this);

		}).Apply(this);

	base.OnStarted2(time);
}
```

## ロジック

- ストラテジーは 5分足をサブスクライブします
- 各ローソク足の形成が始まると、ルールが設定されます
- そのルールは、ローソク足の合計出来高が 10%（パーセンテージ値を使用）を超えたときにトリガーされます
- ルールがトリガーされると、ローソク足とカウンターに関する情報がログへ追加されます
- 最初のトリガー後、`Once()` メソッドによりルールは動作を停止します

## 機能

- `WhenCandlesStarted` ルールと `WhenTotalVolumeMore` ルールの使用を示します
- ローソク足サブスクリプションの仕組みを使用します
- `"10%".ToUnit()` によるパーセンテージ値の作成例を示します
- `LogInfo` メソッドを使用してストラテジー内で情報をログへ記録する例を示します
- ティックからローソク足を構築する設定用のコメントアウトされたコードを含みます
