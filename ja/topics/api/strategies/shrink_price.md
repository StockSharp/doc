# 価格の丸め

## はじめに

StockSharp の [ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) メソッドは、市場要件に従って価格を正しく丸めるための重要なツールです。これにより、送信される注文が取引所またはブローカーのルールに準拠することを保証できます。

## 目的

[ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) の主な目的は、次を考慮して価格を許容値に丸めることです。
1. 銘柄の価格刻み（[Security.PriceStep](xref:StockSharp.BusinessEntities.Security.PriceStep)）
2. 小数点以下の桁数（[Security.Decimals](xref:StockSharp.BusinessEntities.Security.Decimals)）

## 使用の重要性

[ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) の使用は、次の点で重要です。
- 不正な価格による取引所またはブローカーでの注文拒否を防ぐ
- 計算および取引操作の正確性を確保する
- 特定の市場または銘柄のルールと制約に準拠する

## 動作原理

1. [Security.PriceStep](xref:StockSharp.BusinessEntities.Security.PriceStep) が設定されている場合:
   - 価格は価格刻みの最も近い倍数に丸められます。
2. [Security.Decimals](xref:StockSharp.BusinessEntities.Security.Decimals) が設定されている場合:
   - 価格は指定された小数点以下桁数に丸められます。
3. 両方のパラメーターが設定されている場合:
   - より厳格な丸めが適用されます（通常は価格刻みに合わせます）。

## 使用例

```cs
// 指定されたパラメーターで Security オブジェクトを作成
var security = new Security
{
	PriceStep = 0.01m,  // 0.01 の価格刻み
	Decimals = 2        // 小数点以下 2 桁
};

// ShrinkPrice の使用例

// 例 1: 価格刻みに丸める
decimal price1 = 10.234m;
decimal shrunkPrice1 = price1.ShrinkPrice(security);
Console.WriteLine($"Original price: {price1}, After ShrinkPrice: {shrunkPrice1}");
// 出力: Original price: 10.234, After ShrinkPrice: 10.23

// 例 2: すでに刻みに一致している価格を丸める
decimal price2 = 10.22m;
decimal shrunkPrice2 = price2.ShrinkPrice(security);
Console.WriteLine($"Original price: {price2}, After ShrinkPrice: {shrunkPrice2}");
// 出力: Original price: 10.22, After ShrinkPrice: 10.22

// 例 3: 小数点以下桁数が多い価格を丸める
decimal price3 = 10.2345678m;
decimal shrunkPrice3 = price3.ShrinkPrice(security);
Console.WriteLine($"Original price: {price3}, After ShrinkPrice: {shrunkPrice3}");
// 出力: Original price: 10.2345678, After ShrinkPrice: 10.23

// 例 4: 注文作成時に ShrinkPrice を使用
var order = new Order
{
	Security = security,
	Price = 10.237m.ShrinkPrice(security)  // 注文作成前に価格を丸める
};
Console.WriteLine($"Order price: {order.Price}");
// 出力: Order price: 10.24
```

## 適用

[ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) は、注文を送信する前、または市場条件に対する正確な価格準拠が必要な計算を行う前に使用する必要があります。

## 結論

[ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) を適切に使用すると、注文発注時のエラーを回避し、取引アルゴリズムが市場要件に従って正しく動作することを保証できます。
