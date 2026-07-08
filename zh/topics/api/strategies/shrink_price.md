# 价格四舍五入

## 介绍

StockSharp 中的 [ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) 方法是根据市场要求正确四舍五入价格的基本工具。这确保了提交的订单符合交易所或经纪商的规则。

## 目的

[ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) 的主要目标是将价格四舍五入到允许的值，考虑到：
1. 该工具的价格步长 ([Security.PriceStep](xref:StockSharp.BusinessEntities.Security.PriceStep))
2. 小数位数 ([Security.Decimals](xref:StockSharp.BusinessEntities.Security.Decimals))

## 使用的重要性

使用 [ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) 对于以下方面至关重要：
- 防止因价格错误而被交易所或经纪商拒绝订单
- 确保计算和交易操作的准确性
- 遵守特定市场或工具的规则和限制

## 操作原理

1. 如果设置了 [Security.PriceStep](xref:StockSharp.BusinessEntities.Security.PriceStep) ：
   - 价格会四舍五入到价格步长的最接近倍数。
2. 如果设置了 [Security.Decimals](xref:StockSharp.BusinessEntities.Security.Decimals) ：
   - 价格四舍五入到指定的小数位数。
3. 如果两个参数都已设置：
   - 应用更严格的四舍五入（通常针对价格步长）。

## 示例用法

```cs
// 使用指定参数创建 Security 对象
var security = new Security
{
	PriceStep = 0.01m,  // Price step of 0.01
	Decimals = 2        // Two decimal places
};

// ShrinkPrice 使用示例

// 示例 1：按价格步长取整
decimal price1 = 10.234m;
decimal shrunkPrice1 = price1.ShrinkPrice(security);
Console.WriteLine($"Original price: {price1}, After ShrinkPrice: {shrunkPrice1}");
// 输出：原始价格：10.234，ShrinkPrice 后：10.23

// 示例 2：对已符合步长的价格取整
decimal price2 = 10.22m;
decimal shrunkPrice2 = price2.ShrinkPrice(security);
Console.WriteLine($"Original price: {price2}, After ShrinkPrice: {shrunkPrice2}");
// 输出：原始价格：10.22，ShrinkPrice 后：10.22

// 示例 3：对小数位更多的价格取整
decimal price3 = 10.2345678m;
decimal shrunkPrice3 = price3.ShrinkPrice(security);
Console.WriteLine($"Original price: {price3}, After ShrinkPrice: {shrunkPrice3}");
// 输出：原始价格：10.2345678，ShrinkPrice 后：10.23

// 示例 4：创建订单时使用 ShrinkPrice
var order = new Order
{
	Security = security,
	Price = 10.237m.ShrinkPrice(security)  // Round the price before creating the order
};
Console.WriteLine($"Order price: {order.Price}");
// 输出：订单价格：10.24
```

## 应用

[ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) 应在提交任何订单或执行需要与市场条件严格价格一致的计算之前使用。

## 结论

正确使用 [ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) 有助于在下单时避免错误，并确保交易算法根据市场要求正确运行。
