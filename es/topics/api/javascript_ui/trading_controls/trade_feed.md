# Flujo de operaciones

`TradeFeedWidget` muestra las operaciones públicas del mercado y las ejecuciones de la cartera actual. El flujo del mercado puede alternarse entre una tabla y un gráfico de burbujas.

## Creación y flujo de datos

```ts
import {
  TradeFeedWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const flujo = TradeFeedWidget.create(
  document.querySelector<HTMLElement>('#trade-feed')!,
  {},
  { host },
);

flujo.setActiveSymbol('BTC@IMEX');

flujo.setTrades([{
  symbol: 'BTC@IMEX',
  side: 0,
  price: 68_420.5,
  quantity: 0.25,
  time: new Date().toISOString(),
}]);

flujo.addTrade({
  symbol: 'BTC@IMEX',
  side: 1,
  price: 68_419.9,
  quantity: 0.4,
  time: new Date().toISOString(),
});
```

`setTrades` sustituye el conjunto original, mientras que `addTrade` añade una ejecución en tiempo real. El control conserva como máximo 50 filas de la tabla y las 500 últimas marcas para el gráfico.

## Gráfico de burbujas

En el gráfico, el eje horizontal representa el tiempo; el vertical, el precio; el radio de la burbuja, el volumen; y el color, el lado de la operación. Cuando se combinan marcas adyacentes, la ayuda emergente muestra el VWAP, el volumen total y el número de operaciones. Una operación se considera grande si su volumen supera en más del doble la media móvil.

Los colores del lienzo proceden de `host.presentation.canvasPalette()`. El modo seleccionado se almacena en `host.preferences` con una clave común para toda la página.

## Instrumentos adicionales

Además del símbolo activo, el panel puede fijar otros instrumentos:

```ts
await flujo.addExtraSymbol('ETH@IMEX');
console.log(flujo.getExtraSymbols());
await flujo.removeExtraSymbol('ETH@IMEX');
```

Para ellos se crean suscripciones del nivel `MarketDataLevels.Tape` y, en el gráfico de burbujas, bandas independientes con sus propias escalas de precios. La lista de instrumentos adicionales se guarda en el estado de la instancia concreta y puede proporcionarse durante la creación como `{ extras: ['ETH@IMEX'] }`.

## Operaciones propias

La segunda pestaña muestra las ejecuciones de la cartera. La carga puede iniciarse explícitamente:

```ts
await flujo.loadMyTrades(host.trading.portfolioId(), 'BTC@IMEX');
```

El método llama a `host.trading.api.getExecutions`. El resto del flujo del mercado siempre se proporciona al control desde el exterior, de modo que varios paneles puedan utilizar una sola conexión.

## Métodos públicos

- `setActiveSymbol(symbol)`: establece el instrumento principal.
- `setTrades(...)`, `addTrade(...)`: sustituyen o amplían el flujo del mercado.
- `loadMyTrades(portfolioId, symbol)`: carga las ejecuciones propias.
- `addExtraSymbol`, `removeExtraSymbol`, `getExtraSymbols`: administran los instrumentos fijados.
- `dispose()`: elimina las suscripciones y libera los recursos.

## Véase también

- [Controles de negociación JavaScript](../trading_controls.md)
- [Historial de operaciones](trade_history.md)
- [Libro de órdenes](order_book.md)
