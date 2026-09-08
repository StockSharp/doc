# Interfaz del gráfico

`createChartUi` monta alrededor del motor una interfaz lista para usar: títulos de panel, leyenda con crosshair, menú contextual, menú de tipo de gráfico y diálogo de indicadores. El motor solo dibuja; todo lo que lo rodea vive en un punto de entrada aparte, `@stocksharp/chart/ui`, de modo que una página con un único sparkline no paga por ello.

![La leyenda, un panel de indicador y los menús del gráfico alrededor del motor](../../../../images/javascript_charts_ui.png)

## Conexión

Esta capa se distribuye como subpath del paquete y necesita su propia hoja de estilos:

```ts
import { createChartUi, standaloneHost } from '@stocksharp/chart/ui';
import '@stocksharp/chart/ui.css';
```

Sin empaquetador, incluya el paquete para navegador `dist/sschartui.js`, que publica el objeto global `SSChartUI`. Hay que cargarlo **después** de `dist/sschart.js`: la capa lee el motor del objeto global que publica ese archivo y no lleva una segunda copia de él.

## Creación y actualización

La capa necesita el elemento en el que se ha creado el gráfico, el anfitrión de la página, un origen de precio para el menú contextual y la lista de tipos de gráfico para el menú de la leyenda:

```ts
import { createChart, CandlestickSeries } from '@stocksharp/chart';
import {
  ChartType,
  createChartUi,
  localChartUiStorage,
  standaloneHost,
  type ChartUi,
} from '@stocksharp/chart/ui';
import '@stocksharp/chart/ui.css';

declare const candles: { time: number; open: number; high: number; low: number; close: number; volume?: number }[];

const container = document.querySelector<HTMLElement>('#chart')!;

const chart = createChart(container, { timeScale: { timeVisible: true } });
const series = chart.addSeries(CandlestickSeries, { upColor: '#26a69a', downColor: '#ef5350' });
series.setData(candles);

const ui: ChartUi = createChartUi(chart, {
  container,
  host: standaloneHost,
  priceSource: series,
  chartTypes: [
    { value: ChartType.Candle, label: 'Candles', icon: 'bi bi-bar-chart-fill' },
    { value: ChartType.Line, label: 'Line', icon: 'bi bi-graph-up' },
  ],
  storage: localChartUiStorage('sschart'),
});

ui.setCandles(candles);
```

`setCandles` se vuelve a llamar cada vez que cambia la ventana de velas: un instrumento nuevo, un tipo de gráfico conmutado, una página de historial recién llegada. Una sola llamada actualiza tanto el motor de indicadores como la leyenda.

Opciones de `createChartUi`:

| Opción | Propósito |
|---|---|
| `container` | Elemento en el que se ha creado el gráfico; a su alrededor se construyen los paneles. |
| `host` | Traducción, formato y mensajes. |
| `priceSource` | Píxel → precio para el menú contextual. |
| `chartTypes` | Entradas del menú de tipo de gráfico en el orden en que se muestran; una lista vacía no dibuja el menú. |
| `storage` | Dónde se guardan los indicadores favoritos y las plantillas. De forma predeterminada, en memoria. |
| `dialogRoot` | Marcado propio para el diálogo de indicadores. Sin él, el marcado se construye y se añade al `body`. |
| `modal` | Implementación propia de la apertura y el cierre del diálogo. |
| `provideItems` | Entradas de la página en el menú contextual, por encima de las de la propia capa. |

## Qué devuelve createChartUi

El resultado son esos mismos objetos, ya enlazados entre sí; se puede acceder directamente a cualquiera de ellos.

| Campo | Qué es |
|---|---|
| `engine` | `IndicatorEngine`: cálculo y ciclo de vida de los indicadores. |
| `renderer` | `IndicatorRenderer`: las series con las que se dibujan las salidas de los indicadores. |
| `paneManager` | `ChartPaneManager`: títulos de los paneles, sus menús y su restauración. |
| `legend` | `ChartLegend`: la fila OHLCV y los valores de los indicadores bajo el crosshair. |
| `dialog` | `IndicatorDialog`: catálogo, búsqueda, parámetros e indicadores activos. |
| `menu` | `ChartContextMenu`: el menú del botón derecho. |
| `indicators` | `IndicatorController`: edición reversible de los indicadores ya añadidos. |
| `templates` | `IndicatorTemplateController`: plantillas de ajustes de indicador transferibles. |

## Métodos públicos

- `setCandles(candles)`: entrega la ventana de velas actual al motor de indicadores y a la leyenda.
- `showIndicators()`: abre el diálogo de indicadores.
- `dispose()`: retira el menú, el diálogo, la leyenda y los paneles; el marcado del diálogo creado por la capa se elimina.

## Anfitrión de la página

Ningún módulo de la capa accede a objetos globales: las palabras, los números y los mensajes llegan de `ChartUiHost`, un objeto con los campos `translate`, `formatters` y `notify`.

```ts
import { consoleNotify, createTranslate, defaultChartFormatters, type ChartUiHost } from '@stocksharp/chart/ui';

const host: ChartUiHost = {
  translate: createTranslate({ 'Indicators': 'Indicadores', 'Add indicator…': 'Añadir indicador…' }),
  formatters: { ...defaultChartFormatters, price: value => value.toFixed(2) },
  notify: consoleNotify,
};
```

El diccionario es plano y está indexado por el texto original en inglés: una clave sin respuesta se devuelve a sí misma, es decir, una cadena inglesa legible y no un marcador de traducción ausente. La sustitución es posicional: `{0}`, `{1}`.

- `standaloneHost`: un anfitrión que se ocupa de todo por su cuenta: texto en inglés, formato según la magnitud del número y mensajes a la consola.
- `identityTranslate`: traducción para una página con un solo idioma; la sustitución de marcadores de posición se conserva.
- `defaultChartFormatters`: `price`, `volume` y `time` (segundos Unix, formato `YYYY-MM-DD HH:MM`).
- `consoleNotify`: envío de mensajes a la consola del navegador; niveles `success`, `info`, `warning`, `error`.
- `createPlainModalController(root)`: apertura y cierre del diálogo para una página sin biblioteca propia de ventanas modales: fondo atenuado y cierre con `Escape`. Un clic fuera de la ventana no cierra el diálogo.

## Almacenamiento

Los indicadores favoritos y las plantillas se guardan mediante `ChartUiStorage`, dos funciones: `load(key)` y `save(key, value)`.

- `inMemoryChartUiStorage`: el valor predeterminado; los datos viven lo mismo que la página.
- `localChartUiStorage(prefix)`: una envoltura sobre `localStorage` con prefijo, para que dos gráficos de una misma página no se sobrescriban mutuamente los favoritos.

## Tipo de gráfico

`ChartTypeSwitcher` redibuja una misma ventana de barras como velas, barras, línea, área, Heikin-Ashi, Renko o Point & Figure. Cambiar de tipo significa otro renderizador, por lo que la serie se crea de nuevo y su instancia anterior deja de ser válida:

```ts
import {
  ChartType,
  ChartTypeSwitcher,
  allChartTypes,
  defaultChartTypePalette,
  parseChartType,
} from '@stocksharp/chart/ui';

const switcher = new ChartTypeSwitcher({
  chart,
  series,
  initialType: ChartType.Candle,
  availableTypes: allChartTypes,
  palette: defaultChartTypePalette,
  host: standaloneHost,
});
switcher.setRawCandles(candles);

switcher.onSeriesChanged(next => ui.menu.setPriceSource(next));

ui.legend.onChartTypeChange = value => {
  const type = parseChartType(value);
  if (type === null) return;

  switcher.switchType(type);
  ui.setCandles(switcher.getIndicatorCandles());
};
```

`parseChartType` lee el tipo de una cadena que la página guardó por su cuenta —una disposición guardada o el atributo de un botón— y devuelve `null` si ese tipo no existe. `getIndicatorCandles` entrega las barras sobre las que, tras la conmutación, se calculan los indicadores: Renko y Point & Figure reconstruyen las barras originales en las suyas propias, e `isDerivedChartType(type)` responde a si eso ha ocurrido; esas barras tampoco tienen volumen. Los demás métodos son `getCurrentSeries`, `getCurrentType`, `getAvailableTypes` y `updatePrice`.

## Menú contextual

`ChartContextMenu` no añade entradas propias: las aporta la página mediante `provideItems`, devolviendo grupos de entradas; entre grupos se dibuja un separador y los grupos vacíos no cuestan nada. Una entrada se describe con la clave `key`, el texto `label`, los opcionales `icon`, `tone` y `disabled`, y el método `invoke`. El tono se indica con los valores de `ChartContextMenuTone`: `Neutral`, `Positive`, `Negative`.

```ts
import { ChartContextMenuTone, createChartUi, standaloneHost } from '@stocksharp/chart/ui';

const ui = createChartUi(chart, {
  container,
  host: standaloneHost,
  priceSource: series,
  chartTypes: [],
  provideItems: context => [[
    {
      key: 'buy',
      label: `Comprar a ${context.priceText}`,
      icon: 'bi bi-arrow-up-circle',
      tone: ChartContextMenuTone.Positive,
      invoke: () => placeOrder('buy', context.price),
    },
  ]],
});
```

A esas entradas `createChartUi` añade su propio grupo: `Add indicator…` y `Add pane…`, ambas a través de `host.translate`. `ChartContextMenuMode` distingue el menú sobre el gráfico de precios (`Chart`, hay un precio bajo el cursor) del menú sobre el título de un subpanel (`Pane`, no hay precio). Los métodos son `init`, `setPriceSource`, `openAt`, `close` y `dispose`.

## Otras exportaciones

- `ChartLegend` y `fullscreenMenuLayer`: la leyenda y la capa en la que se abre su menú flotante (el elemento en pantalla completa si existe y, si no, `body`). Métodos de la leyenda: `init`, `setRawCandles`, `setChartType`, `setIndicatorEngine`, `refresh`, `dispose`; retrollamadas `onEditIndicator` y `onChartTypeChange`.
- `ChartPaneManager`: una envoltura sobre los paneles integrados del motor: `init`, `addPane`, `removePane`, `restorePane`, `getChart`, `getPanes`, `getPaneByMeasure`, `setPaneTitle`, `getValuesElement`, `legendLayer`, `resize`, `dispose`.
- `IndicatorDialog` y `createIndicatorCatalogController`: el diálogo de indicadores y el modelo de su catálogo. Métodos del diálogo: `show`, `showForPane`, `showEdit`, `hide`, `dispose`.
- `IndicatorEngine`, `IndicatorRenderer`, `IndicatorSettings`: la mecánica de los indicadores que gobierna el diálogo. Se publica tanto desde aquí como desde `@stocksharp/chart/indicators`.
- Los tipos `LegendBar`, `LegendChartType`, `LegendChart`, `LegendPaneHost`, `LegendIndicatorEngine`, `IndicatorPaneChart`, `IndicatorPaneHost`, `ChartContextMenuProvider`, `PriceCoordinateSource`, `ChartTypePalette`, `ModalController`: contratos estructurales; una página que dispone los paneles o calcula los indicadores por su cuenta los implementa con sus propios objetos.

## Véase también

- [Gráficos en JavaScript](../charts.md)
- [Indicadores](indicators.md)
- [Relleno de historial](backfill.md)
- [Velas](candlestick.md)
