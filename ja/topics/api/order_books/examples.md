# 板情報の例

## 最良価格の取得

板情報から最良価格を取得するには、買い注文（[Bids](xref:StockSharp.Messages.IOrderBookMessage.Bids)）と売り注文（[Asks](xref:StockSharp.Messages.IOrderBookMessage.Asks)）のリストの先頭要素に注目することが重要です。これらは取引で利用可能な最も有利な価格を表します。

```cs
var bestBid = orderBook.Bids.FirstOrDefault();
var bestAsk = orderBook.Asks.FirstOrDefault();

if (bestBid != null)
{
	Console.WriteLine($"Best buy price: {bestBid.Price}");
}

if (bestAsk != null)
{
	Console.WriteLine($"Best sell price: {bestAsk.Price}");
}
```

または、用意されている拡張メソッド [GetBestBid](xref:StockSharp.Messages.Extensions.GetBestBid(StockSharp.Messages.IOrderBookMessage)) と [GetBestAsk](xref:StockSharp.Messages.Extensions.GetBestAsk(StockSharp.Messages.IOrderBookMessage)) を使用します。

```cs
var bestBid = orderBook.GetBestBid();
var bestAsk = orderBook.GetBestAsk();

if (bestBid != null)
{
	Console.WriteLine($"Best buy price: {bestBid.Price}, volume: {bestBid.Volume}");
}
else
{
	Console.WriteLine("No best buy orders.");
}

if (bestAsk != null)
{
	Console.WriteLine($"Best sell price: {bestAsk.Price}, volume: {bestAsk.Volume}");
}
else
{
	Console.WriteLine("No best sell orders.");
}
```

## 板の深さの分析

板の深さを分析するには、[Bids](xref:StockSharp.Messages.IOrderBookMessage.Bids) および [Asks](xref:StockSharp.Messages.IOrderBookMessage.Asks) リストの項目を、リストの先頭から順に反復処理します。これにより、異なる価格レベルにおける注文分布の概要を把握でき、潜在的なサポートレベルとレジスタンスレベルの特定に役立ちます。

```cs
foreach (var bid in orderBook.Bids)
{
	Console.WriteLine($"Buy price: {bid.Price}, volume: {bid.Volume}");
}

foreach (var ask in orderBook.Asks)
{
	Console.WriteLine($"Sell price: {ask.Price}, volume: {ask.Volume}");
}
```

## 板情報内の数量の検索

板情報内で大きな数量を検索するアルゴリズムは、大口注文が蓄積しているレベルの特定に役立ちます。これは大口参加者の関心を示す可能性があり、取引判断を行う際の追加シグナルとして機能します。

アルゴリズム:

1. 重要とみなす数量しきい値を決定します。
2. [Bids](xref:StockSharp.Messages.IOrderBookMessage.Bids) および [Asks](xref:StockSharp.Messages.IOrderBookMessage.Asks) リスト内の注文を反復処理し、各注文の数量をしきい値と比較します。
3. しきい値を超える数量の注文が見つかった価格レベルを記録します。

```cs
double significantVolumeThreshold = 10000; // しきい値の例

Console.WriteLine("Significant volumes in the order book:");

foreach (var bid in orderBook.Bids)
{
	if (bid.Volume >= significantVolumeThreshold)
	{
		Console.WriteLine($"Buy: Price {bid.Price}, volume {bid.Volume}");
	}
}

foreach (var ask in orderBook.Asks)
{
	if (ask.Volume >= significantVolumeThreshold)
	{
		Console.WriteLine($"Sell: Price {ask.Price}, volume {ask.Volume}");
	}
}
```

このアルゴリズムは、大きな数量を持つレベルを強調するのに役立ちます。これらのレベルは、市場価格の変動において重要な役割を果たす可能性があります。
