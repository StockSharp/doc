# 注文のルール

## 概要

`SimpleOrderRulesStrategy` は、StockSharp で注文に関連するイベントを処理するためのルールの使用を示すストラテジーです。取引をサブスクライブし、注文登録イベントを処理するためのルールを作成します。

## 主要コンポーネント

```cs
// 主要コンポーネント
public class SimpleOrderRulesStrategy : Strategy
{
}
```

## OnStarted メソッド

ストラテジー開始時に呼び出されます。

- ティックへのサブスクリプションを作成します
- 注文登録イベントを処理するための 2 組のルールを作成します

```cs
// OnStarted メソッド
protected override void OnStarted2(DateTime time)
{
	var sub = new Subscription(DataType.Ticks, Security);

	sub.WhenTickTradeReceived(this).Do(() =>
	{
		var order = CreateOrder(Sides.Buy, default, 1);

		var ruleReg = order.WhenRegistered(this);
		var ruleRegFailed = order.WhenRegisterFailed(this);

		ruleReg
			.Do(() => LogInfo("注文 №1 が登録されました"))
			.Once()
			.Apply(this)
			.Exclusive(ruleRegFailed);

		ruleRegFailed
			.Do(() => LogInfo("注文 №1 登録失敗"))
			.Once()
			.Apply(this)
			.Exclusive(ruleReg);

		RegisterOrder(order);
	}).Once().Apply(this);

	sub.WhenTickTradeReceived(this).Do(() =>
	{
		var order = CreateOrder(Sides.Buy, default, 10000000);

		var ruleReg = order.WhenRegistered(this);
		var ruleRegFailed = order.WhenRegisterFailed(this);

		ruleReg
			.Do(() => LogInfo("注文 №2 が登録されました"))
			.Once()
			.Apply(this)
			.Exclusive(ruleRegFailed);

		ruleRegFailed
			.Do(() => LogInfo("注文 №2 登録失敗"))
			.Once()
			.Apply(this)
			.Exclusive(ruleReg);

		RegisterOrder(order);
	}).Once().Apply(this);

	// マーケットデータをサブスクライブする要求を送信。
	Subscribe(sub);

	base.OnStarted2(time);
}
```

## ロジック

### 最初のルールセット

- ティックを受信したとき、1 単位の買い注文を作成します
- 注文は `CreateOrder` メソッドで作成され、方向、価格（default = 成行）、数量を指定します
- 登録成功と登録エラーを処理するルールを設定します
- ルールは相互排他的で、一度だけトリガーされます

### 2 番目のルールセット

- 次のティックを受信したとき、10,000,000 単位の買い注文を作成します
- 同様に、登録成功と登録エラーを処理するルールを設定します
- これらのルールも相互排他的で、一度だけトリガーされます

## 機能

- 注文登録イベントを処理するルールの作成を示します
- 相互排他的なルールの仕組み（`Exclusive`）を使用します
- `LogInfo` メソッドを使用して注文イベントに関する情報をログへ記録する例を示します
- `Once()` を使用してルールのトリガーを制限する方法を示します
- 異なるシナリオ（登録成功と登録エラー）を示すために、数量の異なる注文を作成します
