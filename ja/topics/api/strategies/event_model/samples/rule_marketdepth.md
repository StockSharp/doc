# オーダーブックと取引のルール

## 概要

`SimpleRulesStrategy` は、StockSharp でルールを作成して適用するさまざまな方法を示すストラテジーです。取引とオーダーブックをサブスクライブし、受信したデータを処理するための各種ルールを設定します。

## 主要コンポーネント

```cs
// 主要コンポーネント
public class SimpleRulesStrategy : Strategy
{
}
```

## OnStarted メソッド

ストラテジー開始時に呼び出されます。

- 取引とオーダーブックへのサブスクリプションを作成します
- ルールを作成して適用するさまざまな方法を示します

```cs
// OnStarted メソッド
protected override void OnStarted2(DateTime time)
{
	var tickSub = new Subscription(DataType.Ticks, Security);
	var mdSub = new Subscription(DataType.MarketDepth, Security);

	//-----------------------ルールを作成。方法 №1-----------------------------------
	mdSub.WhenOrderBookReceived(this).Do((depth) =>
	{
		LogInfo($"ルール WhenOrderBookReceived №1 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
	}).Once().Apply(this);

	//-----------------------ルールを作成。方法 №2-----------------------------------
	var whenMarketDepthChanged = mdSub.WhenOrderBookReceived(this);

	whenMarketDepthChanged.Do((depth) =>
	{
		LogInfo($"ルール WhenOrderBookReceived №2 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
	}).Once().Apply(this);

	//----------------------ルール内のルール-----------------------------------
	mdSub.WhenOrderBookReceived(this).Do((depth) =>
	{
		LogInfo($"ルール WhenOrderBookReceived №3 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");

		//----------------------Once ルールではない-----------------------------------
		mdSub.WhenOrderBookReceived(this).Do((depth1) =>
		{
			LogInfo($"ルール WhenOrderBookReceived №4 BestBid={depth1.GetBestBid()}, BestAsk={depth1.GetBestAsk()}");
		}).Apply(this);
	}).Once().Apply(this);

	// マーケットデータをサブスクライブする要求を送信。
	Subscribe(tickSub);
	Subscribe(mdSub);

	base.OnStarted2(time);
}
```

## ロジック

### 方法 #1: ルールの作成

- オーダーブックを受信したときにトリガーされるルールを作成します
- 最良買気配と最良売気配をログへ記録します
- ルールは一度だけトリガーされます（`Once()`）

### 方法 #2: ルールの作成

- ルールを作成する別の方法を示します
- 機能的には方法 #1 と同じです

### ルール内のルール

- オーダーブックを受信したときにトリガーされるルールを作成します
- このルール内で、別のルールを作成します
- 外側のルールは一度だけトリガーされ、内側のルールはオーダーブックを受信するたびにトリガーされます

## 機能

- StockSharp でルールを作成して適用するさまざまな方法を示します
- 取引とオーダーブックへのサブスクリプションを使用します
- `LogInfo` メソッドを使用してストラテジー内で情報をログへ記録する例を示します
- `Once()` を使用してルールのトリガーを制限する方法を示します
