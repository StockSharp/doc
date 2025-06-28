# Price Rounding

## Introduction

The [ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) method in StockSharp is an essential tool for correctly rounding prices according to market requirements. This ensures that submitted orders comply with exchange or broker rules.

## Purpose

The primary goal of [ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) is to round prices to allowable values, considering:
1. The instrument's price step ([Security.PriceStep](xref:StockSharp.BusinessEntities.Security.PriceStep))
2. The number of decimal places ([Security.Decimals](xref:StockSharp.BusinessEntities.Security.Decimals))

## Importance of Usage

Using [ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) is crucial for:
- Preventing order rejection by the exchange or broker due to incorrect prices
- Ensuring accuracy in calculations and trading operations
- Complying with the rules and restrictions of specific markets or instruments

## Operation Principle

1. If [Security.PriceStep](xref:StockSharp.BusinessEntities.Security.PriceStep) is set:
   - The price is rounded to the nearest value multiple of the price step.
2. If [Security.Decimals](xref:StockSharp.BusinessEntities.Security.Decimals) is set:
   - The price is rounded to the specified number of decimal places.
3. If both parameters are set:
   - More stringent rounding is applied (usually to the price step).

## Example Usage

```cs
// Create a Security object with specified parameters
var security = new Security
{
	PriceStep = 0.01m,  // Price step of 0.01
	Decimals = 2        // Two decimal places
};

// Examples of using ShrinkPrice

// Example 1: Rounding to the price step
decimal price1 = 10.234m;
decimal shrunkPrice1 = price1.ShrinkPrice(security);
Console.WriteLine($"Original price: {price1}, After ShrinkPrice: {shrunkPrice1}");
// Output: Original price: 10.234, After ShrinkPrice: 10.23

// Example 2: Rounding a price that already matches the step
decimal price2 = 10.22m;
decimal shrunkPrice2 = price2.ShrinkPrice(security);
Console.WriteLine($"Original price: {price2}, After ShrinkPrice: {shrunkPrice2}");
// Output: Original price: 10.22, After ShrinkPrice: 10.22

// Example 3: Rounding a price with more decimal places
decimal price3 = 10.2345678m;
decimal shrunkPrice3 = price3.ShrinkPrice(security);
Console.WriteLine($"Original price: {price3}, After ShrinkPrice: {shrunkPrice3}");
// Output: Original price: 10.2345678, After ShrinkPrice: 10.23

// Example 4: Using ShrinkPrice when creating an order
var order = new Order
{
	Security = security,
	Price = 10.237m.ShrinkPrice(security)  // Round the price before creating the order
};
Console.WriteLine($"Order price: {order.Price}");
// Output: Order price: 10.24
```

## Application

[ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) should be used before submitting any orders or performing calculations that require precise price compliance with market conditions.

## Conclusion

Proper use of [ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) helps avoid errors when placing orders and ensures that trading algorithms operate correctly according to market requirements.
