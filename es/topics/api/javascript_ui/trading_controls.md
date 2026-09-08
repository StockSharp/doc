# Controles de negociación JavaScript

[Controles de negociación JavaScript de StockSharp](https://github.com/StockSharp/JS-TradingControls) es un conjunto de paneles para terminales de negociación que se ejecutan en el navegador. El paquete se publica en npm como [`@stocksharp/trading-controls`](https://www.npmjs.com/package/@stocksharp/trading-controls), y todos los controles pueden verse en la [demostración en línea](https://stocksharp.github.io/JS-TradingControls/demo/).

![Pantalla de negociación con flujo de operaciones, libro de órdenes, lista de instrumentos, entrada de órdenes y tablas](../../../images/javascript_trading_controls.jpg)

La captura también muestra un gráfico de velas del paquete independiente `@stocksharp/chart`. `@stocksharp/trading-controls` incluye quince controles independientes:

| Control | Clase | Identificador |
|---|---|---|
| [Órdenes activas](trading_controls/active_orders.md) | `ActiveOrdersWidget` | `activeOrders` |
| [Posiciones](trading_controls/positions.md) | `PositionsWidget` | `positions` |
| [Historial de operaciones](trading_controls/trade_history.md) | `TradeHistoryWidget` | `tradeHistory` |
| [Lista de seguimiento](trading_controls/watchlist.md) | `WatchlistWidget` | `watchlist` |
| [Entrada de órdenes](trading_controls/order_entry.md) | `OrderEntryWidget` | `orderEntry` |
| [Libro de órdenes](trading_controls/order_book.md) | `OrderBookWidget` | `orderbook` |
| [Flujo de operaciones](trading_controls/trade_feed.md) | `TradeFeedWidget` | `tradefeed` |
| [Estadísticas](trading_controls/statistics.md) | `StatisticsWidget` | `statistics` |
| [Registro](trading_controls/log_monitor.md) | `LogMonitorWidget` | `logMonitor` |
| [Estrategias](trading_controls/strategies.md) | `StrategiesWidget` | `strategies` |
| [Tablero de opciones](trading_controls/option_desk.md) | `OptionDeskWidget` | `optionDesk` |
| [Sonrisa de volatilidad](trading_controls/option_smile.md) | `OptionSmileWidget` | `optionSmile` |
| [Curva de capital](trading_controls/equity.md) | `EquityWidget` | `equity` |
| [Mapa de calor de optimización](trading_controls/optimization_heatmap.md) | `OptimizationHeatmapWidget` | `optimizationHeatmap` |
| [Superficie de optimización](trading_controls/optimization_surface.md) | `SurfaceWidget` | `optimizationSurface` |

Los valores de los identificadores están disponibles mediante el objeto exportado `ControlTypes`. Tenga en cuenta que en la superficie de optimización el nombre de la clase no coincide con el identificador: la clase se llama `SurfaceWidget` y el identificador es `optimizationSurface`.

## Instalación

```bash
npm install @stocksharp/trading-controls
```

Las tablas las dibujan los controles mediante [@stocksharp/grids](grids.md), que llega automáticamente como dependencia normal. En cambio, [@stocksharp/chart](charts.md) está declarado como **dependencia de pares (peer)**: npm no lo instalará y hay que instalarlo uno mismo si se utiliza la curva de capital o la sonrisa de volatilidad, ya que ambas están construidas sobre el motor de gráficos.

```bash
npm install @stocksharp/chart
```

Además de la importación raíz, el paquete declara subpaths: uno por cada control (`@stocksharp/trading-controls/watchlist`, etc.), módulos auxiliares (`/trading-host`, `/control-types`, `/formatters`, `/dom`, `/trading-data`) y una familia paralela `/source/*` con el código fuente en TypeScript, para quienes compilan los controles con su propio empaquetador junto con el resto del código.

Los estilos principales son obligatorios. Puede incluir también la paleta clara y oscura predefinida o sustituirla por sus propias variables CSS `--t-*`:

```ts
import '@stocksharp/trading-controls/styles.css';
import '@stocksharp/trading-controls/theme.css'; // Opcional: tema predefinido.
```

Los controles utilizan clases de [Bootstrap Icons](https://icons.getbootstrap.com/), pero no incluyen las fuentes ni los archivos SVG. La página anfitriona debe incluir los iconos por separado.

Para las páginas sin empaquetador se proporciona el archivo `dist/sstradingcontrols.js`, que crea el objeto global `window.SSTradingControls`.

## Esquema general de creación

Cada control se crea mediante el método estático `create`. Este método valida el anfitrión, construye su propio DOM y añade el elemento raíz al contenedor proporcionado:

```ts
import {
  PositionsWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const positions = PositionsWidget.create(
  document.querySelector<HTMLElement>('#positions')!,
  {},
  {
    host,
    closePosition: (portfolioId, instrumentId, symbol) =>
      console.log('cerrar', portfolioId, instrumentId, symbol),
    reversePosition: (portfolioId, instrumentId, symbol) =>
      console.log('invertir', portfolioId, instrumentId, symbol),
    refreshPositions: () => console.log('actualizar'),
  },
);

positions.update([]);
```

El segundo argumento es el estado guardado de la instancia. El conjunto de dependencias del tercer argumento varía según el control: por ejemplo, el panel de posiciones recibe controladores para cerrar e invertir posiciones, mientras que el libro de órdenes recibe controladores para seleccionar y ejecutar un precio.

## Contrato TradingHost

Los controles no acceden directamente a un traductor global, al almacén de configuración, a la conexión de negociación ni al administrador de ventanas. Toda interacción externa pasa por un único objeto `TradingHost`.

| Miembro del anfitrión | Propósito |
|---|---|
| `isPrimary` | Indica la instancia principal del control en la página. |
| `t(key, ...args)` | Traduce el texto visible e inserta los argumentos. |
| `presentation` | Formatea el lado, el tipo y el estado de la orden, las clases de beneficios y la paleta del lienzo. |
| `preferences`, `cache` | Almacenan la configuración persistente y los datos temporales. |
| `trading.api` | Busca instrumentos y carga ejecuciones. |
| `trading.marketData` | Administra las suscripciones y proporciona las órdenes activas. |
| `trading.portfolioId()` | Devuelve la cartera actual. |
| `trading.pickInstrument(...)` | Abre el selector de instrumentos. |
| `ticker` | Recibe los instrumentos visibles y sus cotizaciones. |
| `allow(action)` | Comprueba el permiso para realizar una acción. |
| `close`, `spawn`, `persistState`, `saveLayout` | Administran el ciclo de vida y el estado del panel. |
| `register`, `unregister`, `broadcast` | Registran instancias y distribuyen cambios entre ellas. |
| `log(message)` | Recibe mensajes de diagnóstico. |

Todos los miembros son obligatorios. `assertHost` valida las funciones anidadas antes de renderizar el control e indica la ruta exacta que falta. Si la aplicación no necesita alguna función, puede proporcionar implementaciones vacías razonables para los comandos obligatorios, por ejemplo, `log: console.warn` o un `saveLayout` vacío.

## Localización y diseño

Los controles obtienen todo el texto visible exclusivamente mediante `host.t`. La lista completa y actualizada de 235 claves se distribuye en `@stocksharp/trading-controls/translation-keys.json`. No es un conjunto, sino un objeto `{ $comment, count, keys }`: las claves en sí están en el campo `keys`. Una clave desconocida se muestra al usuario tal cual, por lo que el anfitrión debe definir las traducciones de toda la lista.

El archivo `styles.css` contiene las reglas, pero obtiene los colores, las fuentes y las dimensiones de las variables CSS `--t-*`. Si no se utiliza el archivo `theme.css` predefinido, la aplicación debe definir estas variables. Los colores del lienzo para el libro de órdenes y el flujo de burbujas se obtienen mediante `host.presentation.canvasPalette()`.

## Liberar recursos

Llame a `dispose()` cuando elimine el panel. Este método retira los controladores, desconecta los observadores y las suscripciones específicos del control cuando existen y, finalmente, llama a `host.unregister`.

```ts
positions.dispose();
```

## Compilar desde el código fuente

```bash
git clone https://github.com/StockSharp/JS-TradingControls.git
cd JS-TradingControls
npm install
npm test
npm run build
```

## Véase también

- [Cuadrículas JavaScript](grids.md)
- [Gráficos en JavaScript](charts.md)
- [Repositorio de JS-TradingControls](https://github.com/StockSharp/JS-TradingControls)
- [Demostración en línea](https://stocksharp.github.io/JS-TradingControls/demo/)
