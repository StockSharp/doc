# Libro de órdenes

`OrderBookWidget` muestra los niveles de compra y venta, el precio medio, el diferencial, el volumen acumulado y el sentimiento del mercado. Admite vistas diagonal y apilada, inversión de lados, una profundidad de 5 o 10 niveles y un gráfico de profundidad en lienzo.

## Creación

```ts
import {
  OrderBookWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const book = OrderBookWidget.create(
  document.querySelector<HTMLElement>('#order-book')!,
  { followsActive: true },
  {
    host,
    onPriceSelected: (price, side) =>
      console.log('rellenar', price, side),
    onPriceExecuted: (price, side) =>
      console.log('ejecutar', price, side),
    maxDepth: () => 10,
    pixelRatio: () => window.devicePixelRatio || 1,
  },
);

book.setSymbol('BTC@IMEX');
```

Un clic normal en un nivel llama a `onPriceSelected`, mientras que un clic con Ctrl o Cmd llama a `onPriceExecuted`. El lado numérico `0` significa compra y `1`, venta: al hacer clic en una oferta de venta se selecciona una compra y al hacerlo en una oferta de compra se selecciona una venta. El control transmite la intención al anfitrión, pero no envía la orden por sí mismo.

## Instantáneas y cambios del libro de órdenes

El primer cuadro debe ser una instantánea completa:

```ts
book.applyFrame({
  symbol: 'BTC@IMEX',
  sequence: 1,
  isSnapshot: true,
  bids: [
    { price: 68_420.4, quantity: 2.5 },
    { price: 68_420.3, quantity: 4.1 },
  ],
  asks: [
    { price: 68_420.5, quantity: 1.8 },
    { price: 68_420.6, quantity: 3.2 },
  ],
});
```

Los cuadros posteriores con `isSnapshot: false` se aplican como cambios. Una cantidad `0` elimina el nivel. `sequence` debe aumentar sin interrupciones; si se detecta un salto, el control llama a `host.trading.marketData.resubscribe(symbol, MarketDataLevels.Full)` para obtener una instantánea nueva. Los niveles no válidos y un libro cruzado se notifican mediante `host.log`.

## Vista y estado

```ts
book.setDepth(10);
book.setView('stacked');
book.setInvertSides(false);
book.setShowDepthChart(true);
```

`maxDepth()` permite que el anfitrión reduzca el número de niveles para una pantalla pequeña, mientras que `pixelRatio()` especifica la densidad del búfer del lienzo. Los colores del gráfico se obtienen mediante `host.presentation.canvasPalette()`.

En un panel que sigue el instrumento activo, la configuración de la vista se guarda en `host.preferences`. Una instancia fijada la almacena en el estado del panel. Durante la creación se pueden proporcionar `symbol`, `depth`, `view`, `invertSides`, `showDepthChart` y `followsActive`.

Las órdenes activas de `host.trading.marketData.getOrders()` se marcan junto a los niveles correspondientes.

## Métodos públicos

- `setSymbol`, `getSymbol`: administran el instrumento.
- `setDepth`, `getDepth`: establecen y devuelven la profundidad.
- `setView`, `setInvertSides`, `setShowDepthChart`: cambian la vista.
- `getBids`, `getAsks`: devuelven los niveles actuales.
- `isFollowsActive`: indica si el panel sigue el instrumento activo.
- `applyFrame`: aplica una instantánea o un cambio.
- `dispose`: libera los recursos.

## Véase también

- [Controles de negociación JavaScript](../trading_controls.md)
- [Lista de seguimiento](watchlist.md)
- [Entrada de órdenes](order_entry.md)
