# Lista de seguimiento

`WatchlistWidget` muestra instrumentos y cotizaciones en tiempo real. El control admite búsqueda, favoritos, categorías, selección del instrumento activo y suscripciones exclusivas a los símbolos que están realmente visibles.

## Creación

```ts
import {
  WatchlistWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

let watchlist!: WatchlistWidget;

watchlist = WatchlistWidget.create(
  document.querySelector<HTMLElement>('#watchlist')!,
  {},
  {
    host,
    onSelect: symbol => {
      watchlist.setCurrentSymbol(symbol);
      console.log('seleccionado', symbol);
    },
  },
);
```

Durante la creación se ejecuta automáticamente `init()`, que carga la lista mediante `host.trading.api.searchInstruments('')`. Para cada registro se utilizan los datos del símbolo, el nombre, la bolsa y la categoría.

El flujo de precios se proporciona desde el exterior:

```ts
watchlist.onPriceUpdate('BTC@IMEX', 68_420.5);
```

El método actualiza únicamente las celdas correspondientes del precio y el porcentaje, manteniendo la animación del cambio. `setCurrentSymbol(symbol)` resalta el instrumento actual.

## Búsqueda, categorías y suscripciones

La búsqueda comprueba el símbolo, el nombre y la bolsa. Se crean pestañas para todos los instrumentos, los favoritos y las categorías detectadas. Los símbolos favoritos se guardan en `host.preferences`.

El control suscribe los primeros 30 instrumentos visibles al nivel `MarketDataLevels.Quotes`. En la pantalla se renderizan como máximo 300 filas, pero el filtrado y la exportación utilizan todo el conjunto encontrado. Solo la instancia con `host.isPrimary === true` publica las cotizaciones visibles mediante `host.ticker`.

El cambio porcentual se calcula desde el primer precio recibido del día actual en UTC. Estos precios base forman parte de la caché, por lo que se guardan en `host.cache` y no en la configuración del usuario.

## Métodos públicos

- `init()`: carga los instrumentos y prepara las suscripciones; `create` lo llama automáticamente.
- `setCurrentSymbol(symbol)`: marca el instrumento activo.
- `onPriceUpdate(symbol, price)`: aplica el precio nuevo.
- `dispose()`: elimina las suscripciones y libera los recursos.

## Véase también

- [Controles de negociación JavaScript](../javascript_trading_controls.md)
- [Libro de órdenes](order_book.md)
- [Entrada de órdenes](order_entry.md)
