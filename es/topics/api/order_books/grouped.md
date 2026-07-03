# Libro de órdenes agrupado

Además del [libro de órdenes disperso](sparse.md), puede ser útil usar un libro de órdenes agrupado, donde las órdenes se agregan en rangos de precio más amplios para simplificar el análisis e identificar tendencias generales de demanda y oferta.

Ventajas de un libro de órdenes agrupado:

- **Análisis simplificado:** La agregación de datos de órdenes simplifica la percepción del panorama general del mercado.
- **Identificación de tendencias:** Es más fácil identificar niveles de precio clave donde se concentra la mayoría de las órdenes.

## Implementación de un libro de órdenes agrupado:

Para trabajar con un libro de órdenes agrupado, primero es necesario configurar la recepción mediante [suscripciones](subscriptions.md) y después llamar al método de extensión [Group](xref:StockSharp.Messages.Extensions.Group(StockSharp.Messages.IOrderBookMessage,System.Decimal)):

```cs
// Agrupar datos del libro de órdenes con un paso de agregación de precio, por ejemplo, 0.5 unidades de precio
var groupedDepth = orderBook.Group(0.5);

// groupedDepth ahora contiene un libro de órdenes en el que las órdenes están agrupadas
// por niveles de precio con el paso de agregación especificado.
```

El método [Group](xref:StockSharp.Messages.Extensions.Group(StockSharp.Messages.IOrderBookMessage,System.Decimal)) permite agregar órdenes en el libro sobre niveles de precio más grandes, simplificando el análisis visual del mercado y ayudando a identificar los principales niveles de demanda y oferta sin necesidad de analizar cada cambio individual de precio.
