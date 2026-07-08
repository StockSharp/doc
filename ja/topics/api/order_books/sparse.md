# スパース板情報

スパース板情報は、現在アクティブな注文がない価格レベルも含め、可能なすべての価格レベルを表示する板情報の表現です。この方法により、トレーダーは注文間の「ギャップ」、つまり買い注文または売り注文が存在しない価格レベルを視覚的に評価でき、潜在的なレジスタンスまたはサポートレベルに関する洞察を得られます。

## スパース板情報を使用する理由

スパース板情報を使用することには、いくつかの利点があります。

1. **ギャップの可視化:** 注文が欠けている価格レベルを特定しやすくなります。これは潜在的なエントリーポイントまたはエグジットポイントを示す場合があります。
2. **流動性分析:** 注文分布を明確に表現することで、異なる価格レベルにおける銘柄の流動性を評価しやすくなります。
3. **戦略的計画:** 板情報の構造を理解することで、流動性における潜在的な「空白」を考慮し、より正確な取引操作の計画が可能になります。

## スパース板情報の作成

グループ化された板情報を扱うには、まず [サブスクリプション](subscriptions.md) による受信を設定し、その後、拡張メソッド [Sparse](xref:StockSharp.Messages.Extensions.Sparse(StockSharp.Messages.IOrderBookMessage,System.Decimal,System.Nullable{System.Decimal},System.Int32)) を呼び出す必要があります。このメソッドは次のパラメーターを受け取ります。

- `priceRange` - レベルを展開する対象となる価格差。
- `priceStep` - 取引銘柄の価格ステップ。`priceRange` が価格レベルに関して `priceStep` より低い精度を持ち、取得した価格を銘柄の価格ステップに丸める必要がある場合に使用されます。

```cs
// orderBook は StockSharp から取得した IOrderBookMessage オブジェクトであると仮定します
var sparseDepth = orderBook.Sparse(priceRange, priceStep);

// これで sparseDepth には、注文が存在しない価格レベルも含め、
// 可能なすべての価格レベルを考慮した元の板情報の表現が含まれます。
```

この例では、[Sparse](xref:StockSharp.Messages.Extensions.Sparse(StockSharp.Messages.IOrderBookMessage,System.Decimal,System.Nullable{System.Decimal},System.Int32)) を使用してスパース板情報を作成しています。これにより、現在アクティブな注文がない価格レベルも含め、すべての価格レベルを表示できます。これは、サポートレベルまたはレジスタンスレベルとして機能する可能性のある「空」のレベルを分析する際に役立つ場合があります。
