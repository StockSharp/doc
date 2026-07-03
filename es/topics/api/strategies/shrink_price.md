# Redondeo de precios

## Introducción

El método [ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) en StockSharp es una herramienta esencial para redondear correctamente los precios según los requisitos del mercado. Esto garantiza que las órdenes enviadas cumplan las reglas del exchange o broker.

## Propósito

El objetivo principal de [ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) es redondear precios a valores permitidos, considerando:
1. El paso de precio del instrumento ([Security.PriceStep](xref:StockSharp.BusinessEntities.Security.PriceStep))
2. El número de decimales ([Security.Decimals](xref:StockSharp.BusinessEntities.Security.Decimals))

## Importancia del uso

Usar [ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) es crucial para:
- Evitar el rechazo de órdenes por parte del exchange o broker debido a precios incorrectos
- Garantizar precisión en cálculos y operaciones de trading
- Cumplir las reglas y restricciones de mercados o instrumentos específicos

## Principio de funcionamiento

1. Si [Security.PriceStep](xref:StockSharp.BusinessEntities.Security.PriceStep) está establecido:
   - El precio se redondea al valor más cercano múltiplo del paso de precio.
2. Si [Security.Decimals](xref:StockSharp.BusinessEntities.Security.Decimals) está establecido:
   - El precio se redondea al número especificado de decimales.
3. Si ambos parámetros están establecidos:
   - Se aplica el redondeo más estricto (normalmente al paso de precio).

## Ejemplo de uso

```cs
// Crear un objeto Security con parámetros especificados
var security = new Security
{
	PriceStep = 0.01m,  // Paso de precio de 0.01
	Decimals = 2        // Dos decimales
};

// Ejemplos de uso de ShrinkPrice

// Ejemplo 1: redondeo al paso de precio
decimal price1 = 10.234m;
decimal shrunkPrice1 = price1.ShrinkPrice(security);
Console.WriteLine($"Original price: {price1}, After ShrinkPrice: {shrunkPrice1}");
// Output: Original price: 10.234, After ShrinkPrice: 10.23

// Ejemplo 2: redondeo de un precio que ya coincide con el paso
decimal price2 = 10.22m;
decimal shrunkPrice2 = price2.ShrinkPrice(security);
Console.WriteLine($"Original price: {price2}, After ShrinkPrice: {shrunkPrice2}");
// Output: Original price: 10.22, After ShrinkPrice: 10.22

// Ejemplo 3: redondeo de un precio con más decimales
decimal price3 = 10.2345678m;
decimal shrunkPrice3 = price3.ShrinkPrice(security);
Console.WriteLine($"Original price: {price3}, After ShrinkPrice: {shrunkPrice3}");
// Output: Original price: 10.2345678, After ShrinkPrice: 10.23

// Ejemplo 4: uso de ShrinkPrice al crear una orden
var order = new Order
{
	Security = security,
	Price = 10.237m.ShrinkPrice(security)  // Redondear el precio antes de crear la orden
};
Console.WriteLine($"Order price: {order.Price}");
// Output: Order price: 10.24
```

## Aplicación

[ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) debe usarse antes de enviar cualquier orden o realizar cálculos que requieran cumplimiento preciso del precio con las condiciones de mercado.

## Conclusión

El uso correcto de [ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) ayuda a evitar errores al colocar órdenes y garantiza que los algoritmos de trading funcionen correctamente según los requisitos del mercado.
