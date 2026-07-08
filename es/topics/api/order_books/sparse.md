# Libro de órdenes disperso

Un libro de órdenes disperso es una presentación del libro de órdenes que muestra todos los niveles de precio posibles, incluidos aquellos sin órdenes activas en ese momento. Este enfoque permite a los traders evaluar visualmente los "huecos" entre órdenes, es decir, niveles de precio donde no hay órdenes de compra o venta, ofreciendo información sobre posibles niveles de resistencia o soporte.

## Por qué usar un libro de órdenes disperso

Usar un libro de órdenes disperso tiene varias ventajas:

1. **Visualización de huecos:** Es más fácil identificar niveles de precio sin órdenes, que pueden indicar posibles puntos de entrada o salida.
2. **Análisis de liquidez:** Una representación clara de la distribución de órdenes ayuda a evaluar la liquidez del instrumento en distintos niveles de precio.
3. **Planificación estratégica:** Comprender la estructura del libro de órdenes permite planificar operaciones de trading con mayor precisión, considerando posibles "vacíos" de liquidez.

## Creación de un libro de órdenes disperso

Para trabajar con un libro de órdenes agrupado, primero necesita configurar la recepción mediante [Suscripciones](subscriptions.md) y después llamar al método de extensión [Sparse](xref:StockSharp.Messages.Extensions.Sparse(StockSharp.Messages.IOrderBookMessage,System.Decimal,System.Nullable{System.Decimal},System.Int32)). El método toma los siguientes parámetros:

- `priceRange` - la diferencia de precio hasta la cual deben expandirse los niveles.
- `priceStep` - el paso de precio del instrumento de trading. Se usa en caso de que `priceRange` tenga menor precisión en niveles de precio que `priceStep` y necesite redondear los precios obtenidos al paso de precio del instrumento.

```cs
// Se asume que orderBook es un objeto IOrderBookMessage obtenido de StockSharp
var sparseDepth = orderBook.Sparse(priceRange, priceStep);

// Ahora, sparseDepth contiene una representación del libro de órdenes original,
// donde se consideran todos los niveles de precio posibles, incluidos aquellos sin órdenes.
```

En este ejemplo, [Sparse](xref:StockSharp.Messages.Extensions.Sparse(StockSharp.Messages.IOrderBookMessage,System.Decimal,System.Nullable{System.Decimal},System.Int32)) se usa para crear un libro de órdenes disperso, que permite mostrar todos los niveles de precio, incluso aquellos sin órdenes activas. Esto puede ser útil para analizar posibles niveles "vacíos" que pueden servir como niveles de soporte o resistencia.
