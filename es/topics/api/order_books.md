# Libro de órdenes

## Descripción

El libro de órdenes (también conocido como market depth) es información sobre las órdenes actuales de compra y venta de un instrumento específico, organizada por niveles de precio. En StockSharp, el libro de órdenes proporciona datos sobre demanda y oferta, lo que permite análisis de mercado en tiempo real.

## Estructura

El [Libro de órdenes](xref:StockSharp.Messages.IOrderBookMessage) contiene dos listas de órdenes:

- Órdenes de compra, ordenadas por precio descendente - [Bids](xref:StockSharp.Messages.IOrderBookMessage.Bids).
- Órdenes de venta, ordenadas por precio ascendente - [Asks](xref:StockSharp.Messages.IOrderBookMessage.Asks).

Cada orden incluye un precio y volumen.

## Uso

Los datos del libro de órdenes se usan para:

- Identificar niveles de precio con volúmenes máximos de órdenes, que pueden indicar posibles niveles de soporte o resistencia.
- Evaluar la liquidez de mercado de un instrumento.
- Desarrollar estrategias de trading basadas en el análisis de cambios en el libro de órdenes.

## Recuperación de datos

En StockSharp, la suscripción a datos del libro de órdenes y la recepción de actualizaciones se realiza mediante los [métodos API](order_books/subscriptions.md) correspondientes.
