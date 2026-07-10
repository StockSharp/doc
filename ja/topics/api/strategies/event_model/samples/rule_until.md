# Until ルール

## 概要

`SimpleRulesUntilStrategy` は、StockSharp で終了条件（`Until`）付きルールの使用を示すストラテジーです。取引とオーダーブックをサブスクライブし、特定の条件が満たされるまで実行されるルールを設定します。

## 主要コンポーネント

```cs
// 主要コンポーネント
public class SimpleRulesUntilStrategy : Strategy
{
}
```

## OnStarted メソッド

ストラテジー開始時に呼び出されます。

- ティックとオーダーブックへのサブスクリプションを作成します
- 特定の条件が満たされるまで、オーダーブックデータを受信したときに実行されるルールを作成します

```cs
// OnStarted メソッド
protected override void OnStarted2(DateTime time)
{
	var tickSub = new Subscription(DataType.Ticks, Security);
	var mdSub = new Subscription(DataType.MarketDepth, Security);

	var i = 0;
	mdSub.WhenOrderBookReceived(this).Do(depth =>
	{
		i++;
		LogInfo($"ルール WhenOrderBookReceived BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
		LogInfo($"ルール WhenOrderBookReceived i={i}");
	})
	.Until(() => i >= 10)
	.Apply(this);

	// マーケットデータをサブスクライブする要求を送信。
	Subscribe(tickSub);
	Subscribe(mdSub);

	base.OnStarted2(time);
}
```

## ロジック

- 起動時、ストラテジーはティックとオーダーブックへのサブスクリプションを作成します
- オーダーブックデータを受信するたびにトリガーされるルールが作成されます
- ルールがトリガーされたとき:
  - カウンター `i` がインクリメントされます
  - 最良買気配と最良売気配の価格に関する情報がログへ追加されます
  - カウンター `i` の現在値がログへ追加されます
- ルールは、カウンター `i` の値が 10 以上になるまで実行されます
- 条件が満たされると、ルールは自動的に動作を停止します

## 機能

- `Until()` メソッドを使用してルールの実行を制限する方法を示します
- 取引とオーダーブックへのサブスクリプションを使用します
- `LogInfo` メソッドを使用してオーダーブックとカウンター状態に関する情報をログへ記録する例を示します
- 特定の条件に基づいてルールの実行回数を制限する方法を示します
