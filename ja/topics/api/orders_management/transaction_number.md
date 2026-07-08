# トランザクション番号

注文を扱う際の主要な識別子は [Order.TransactionId](xref:StockSharp.BusinessEntities.Order.TransactionId) であり、[Order.Id](xref:StockSharp.BusinessEntities.Order.Id) ではありません。これは [Order.Id](xref:StockSharp.BusinessEntities.Order.Id) が取引所によって生成されるためです。このため、[Connector.RegisterOrder](xref:StockSharp.Algo.Connector.RegisterOrder(StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) order **)** メソッドの実行直後は、しばらくの間 [Order.Id](xref:StockSharp.BusinessEntities.Order.Id) が初期化されていない場合があります。したがって、トランザクションを送信した直後に、取引プログラムが [Order.TransactionId](xref:StockSharp.BusinessEntities.Order.TransactionId) を生成します。

[Order.TransactionId](xref:StockSharp.BusinessEntities.Order.TransactionId) は [IdGenerator](https://github.com/StockSharp/Ecng/blob/master/Common/IdGenerator.cs) クラスによって自動的に生成されます。これは抽象クラスであり、2 つの標準実装があります。

- [IncrementalIdGenerator](https://github.com/StockSharp/Ecng/blob/master/Common/IdGenerator.cs#L28) - 既定でインストールされます。番号を 1 ずつ増加させます。初期値は [IncrementalIdGenerator.Current](https://github.com/StockSharp/Ecng/blob/master/Common/IdGenerator.cs#L42) プロパティを通じて設定され、既定では、その値は一日の開始からのミリ秒数に等しくなります。
- [MillisecondIdGenerator](https://github.com/StockSharp/Ecng/blob/master/Common/IdGenerator.cs#L93)。ジェネレーターが作成された時点からのミリ秒数に等しいトランザクション番号を生成します。
