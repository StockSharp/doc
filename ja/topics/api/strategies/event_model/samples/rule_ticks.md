# ティック取引のルール

## 概要

`SimpleTradeRulesStrategy` は、StockSharp で取引価格を分析するための結合ルールの使用を示すストラテジーです。取引をサブスクライブし、特定の価格条件でトリガーされるルールを作成します。

## 主要コンポーネント

```cs
// 主要コンポーネント
public class SimpleTradeRulesStrategy : Strategy
{
}
```

## OnStarted メソッド

ストラテジー開始時に呼び出されます。

- ティックへのサブスクリプションを作成します
- 取引価格を分析するための結合ルールを作成します

```cs
// OnStarted メソッド
protected override void OnStarted2(DateTime time)
{
	var sub = new Subscription(DataType.Ticks, Security);

	sub.WhenTickTradeReceived(this).Do(t =>
	{
		sub
			.WhenLastTradePriceMore(this, t.Price + 2)
			.Or(sub.WhenLastTradePriceLess(this, t.Price - 2))
			.Do(t =>
			{
				LogInfo($"The rule WhenLastTradePriceMore Or WhenLastTradePriceLess tick={t}");
			})
			.Apply(this);
	})
	.Once() // このルールは一度だけ呼び出す
	.Apply(this);

	// マーケットデータをサブスクライブする要求を送信。
	Subscribe(sub);

	base.OnStarted2(time);
}
```

## ロジック

- 最初のティックを受信すると、結合ルールが作成されます
- これは受信したティックの価格を基準にしており、価格が +/- 2 変化したときにトリガーされるルールを作成します
- ルールは、直近約定価格が現在値 + 2 より高くなるか、現在値 - 2 より低くなったときにトリガーされます
- ルールがトリガーされると、ティックに関する情報がログへ追加されます
- 外側のルールは一度だけトリガーされます（`Once()`）

## 機能

- `Or()` を使用した結合ルールの作成を示します
- 価格分析に `WhenLastTradePriceMore` と `WhenLastTradePriceLess` を使用します
- `LogInfo` メソッドを使用して取引に関する情報をログへ記録する例を示します
- `Once()` を使用してルールのトリガーを制限する方法を示します
- イベントハンドラーにティックパラメーターを渡します（ドキュメント内の例とは異なります）
