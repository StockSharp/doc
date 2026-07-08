# 板情報

## 概要

板情報（マーケットデプスとも呼ばれます）は、特定の証券について現在の買い注文と売り注文を価格レベル別に整理した情報です。StockSharp では、板情報は需要と供給に関するデータを提供し、リアルタイムの市場分析を可能にします。

## 構造

[板情報](xref:StockSharp.Messages.IOrderBookMessage) には、2 つの注文リストが含まれます。

- 価格の降順で並べられた買い注文 - [Bids](xref:StockSharp.Messages.IOrderBookMessage.Bids)。
- 価格の昇順で並べられた売り注文 - [Asks](xref:StockSharp.Messages.IOrderBookMessage.Asks)。

各注文には価格と数量が含まれます。

## 使用方法

板情報データは次の用途に使用されます。

- 注文数量が最大となる価格レベルを特定すること。これは潜在的なサポートまたはレジスタンスレベルを示す場合があります。
- 証券の市場流動性を評価すること。
- 板情報の変化の分析に基づいて取引戦略を開発すること。

## データ取得

StockSharp では、板情報データへのサブスクライブと更新の受信は、対応する [API メソッド](order_books/subscriptions.md) を通じて行います。
