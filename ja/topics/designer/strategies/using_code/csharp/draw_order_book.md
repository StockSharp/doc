# 板情報の描画

コードから作成したストラテジーは、[板情報](../../../user_interface/components/order_book.md) パネルに、[板情報パネル](../../using_visual_designer/elements/market_depths/order_book_panel.md) キューブと同様にデータを描画できます。そのためには、次のコードを記述する必要があります。

1. [IOrderBookSource](xref:StockSharp.Algo.Strategies.IOrderBookSource) インターフェイスの派生を作成します。**Designer** はこれを使用してソースを識別します。この例では、インターフェイスの既定実装である [OrderBookSource](xref:StockSharp.Algo.Strategies.OrderBookSource) クラスを使用しています。

```cs
private static readonly OrderBookSource _bookSource = new OrderBookSource("SMA");
```

2. [OrderBookSources](xref:StockSharp.Algo.Strategies.Strategy.OrderBookSources) プロパティをオーバーライドします。

```cs
public override IEnumerable<IOrderBookSource> OrderBookSources
	=> new[] { _bookSource };
```

これにより、ストラテジーは外部コード (この場合は [板情報](../../../user_interface/components/order_book.md) パネル) に、利用可能な板情報のソースを示します。複数のソースが発生するのは、ストラテジーが複数の板情報 (異なる銘柄、または各種変更を加えた板情報、たとえば [間引き板情報](../../using_visual_designer/elements/market_depths/sparse_order_book.md) など) を扱う場合です。

3. ストラテジーコードに、板情報へのサブスクリプションの初期化を追加します。SmaStrategy の場合、[OnStarted](xref:StockSharp.Algo.Strategies.Strategy.OnStarted) メソッドの末尾に追加します。

```cs
var bookSubscription = new Subscription(DataType.MarketDepth, Security);
			
bookSubscription
	.WhenOrderBookReceived(this)
	.Do(book =>
	{
		// 板情報を描画します。
		DrawOrderBook(bookSubscription, _bookSource, book);
	})
	.Apply(this);
			
Subscribe(bookSubscription);
```

Do ハンドラー内では、板情報を描画用に送信する [DrawOrderBook](xref:StockSharp.Algo.Strategies.Strategy.DrawOrderBook(StockSharp.BusinessEntities.Subscription,StockSharp.Algo.Strategies.IOrderBookSource,StockSharp.Messages.IOrderBookMessage)) メソッドが呼び出されます。

4. [板情報](../../../user_interface/components/order_book.md) パネルを追加し、コードで作成したソースを選択します。

  ![Designer ソースコード板情報 00](../../../../../images/designer_source_code_orderbook_00.png)

5. テスト用にストラテジーを起動すると、板情報にデータが入力されます。

  ![Designer ソースコード板情報 01](../../../../../images/designer_source_code_orderbook_01.png)
