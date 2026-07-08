# グループ化された板情報

[スパース板情報](sparse.md) に加えて、グループ化された板情報を使用すると便利な場合があります。これは、分析を簡素化し、需要と供給の全体的な傾向を特定するために、注文をより広い価格範囲で集約するものです。

グループ化された板情報の利点:

- **分析の簡素化:** 注文データを集約することで、市場全体の状況を把握しやすくなります。
- **トレンドの特定:** 注文の大部分が集中している主要な価格レベルを特定しやすくなります。

## グループ化された板情報の実装:

グループ化された板情報を扱うには、まず [サブスクリプション](subscriptions.md) による受信を設定し、その後、拡張メソッド [Group](xref:StockSharp.Messages.Extensions.Group(StockSharp.Messages.IOrderBookMessage,System.Decimal)) を呼び出す必要があります。

```cs
// 価格集約ステップ（例: 価格の 0.5 単位）で板情報データをグループ化
var groupedDepth = orderBook.Group(0.5);

// groupedDepth には、指定された集約ステップの価格レベルで
// 注文がグループ化された板情報が含まれます。
```

[Group](xref:StockSharp.Messages.Extensions.Group(StockSharp.Messages.IOrderBookMessage,System.Decimal)) メソッドを使用すると、板内の注文をより大きな価格レベルで集約できます。これにより、市場の視覚的分析が簡素化され、個々の価格変化をすべて分析する必要なく、需要と供給の主要なレベルを特定しやすくなります。
