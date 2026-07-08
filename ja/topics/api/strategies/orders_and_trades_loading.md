# 注文と約定の読み込み

ストラテジーを開始するとき、以前に執行された注文や約定を読み込む必要がある場合があります (たとえば、取引セッション中にロボットが再起動された場合や、注文と約定が翌日に持ち越される場合)。これを行うには、次の手順が必要です。

1. 以前に保存された注文のトランザクション ID を読み込む (たとえばファイルから)。
2. 将来のセッション用に新しいトランザクション ID を記録するため、`OrderReceived` イベントを購読する。
3. 以前に発注された注文をストラテジーへ関連付けるため、`CanAttach` メソッドをオーバーライドする。
4. 注文がストラテジーへアタッチされると、その注文で執行されたすべての約定が自動的に読み込まれる。

次の例は、すべての約定をストラテジーへ読み込む方法を示しています。

## 以前に執行された注文と約定をストラテジーへ読み込む

1. ストラテジー開始時に保存済みのトランザクション番号を読み込み、新しい番号を保存するために `OrderReceived` を購読します。

```cs
private HashSet<long> _transactions;

protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	_transactions = File.Exists($"orders_{Name}.txt")
			? File.ReadAllLines($"orders_{Name}.txt").Select(l => l.To<long>()).ToHashSet()
			: new HashSet<long>();

	OrderReceived += order =>
	{
			File.AppendAllLines($"orders_{Name}.txt", new[] { order.TransactionId.ToString() });
			_transactions.Add(order.TransactionId);
	};
}
```

2. 再起動後にストラテジーが自分の注文を認識できるように、`CanAttach` をオーバーライドします。

```cs
protected override bool CanAttach(Order order)
{
	return _transactions.Contains(order.TransactionId);
}
```

3. 注文がストラテジーへ読み込まれると、その注文で執行されたすべての約定も自動的に読み込まれます。
