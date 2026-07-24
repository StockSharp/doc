# Gráficos en JavaScript

[Gráficos de negociación JS de StockSharp](https://github.com/StockSharp/Charts) es una biblioteca de gráficos para navegador, autónoma y sin dependencias. Se publica en npm como [@stocksharp/chart](https://www.npmjs.com/package/@stocksharp/chart) e incluye el motor de canvas `sschart` que utiliza el terminal web de StockSharp. Hay una versión funcional disponible en la [demo en vivo](https://stocksharp.github.io/Charts/demo/).

![Gráfico de negociación en JavaScript de StockSharp](../../../../images/javascript_charts.jpg)

A diferencia de los componentes de Windows de `StockSharp.Xaml.Charting`, esta biblioteca se ejecuta en un navegador y dibuja directamente sobre un `canvas` HTML. El motor se expone a través del objeto global `SSChart` (desde `dist/sschart.js`) y también puede importarse como módulos ECMAScript desde el paquete npm (`import { createChart, CandlestickSeries } from '@stocksharp/chart'`).

## Demo en vivo

El gráfico de abajo es el motor real ejecutándose en esta página: velas con un histograma de volumen y una media móvil. Arrastre para desplazarse, use la rueda para hacer zoom y pulse el botón de expandir (arriba a la derecha) para abrirlo en pantalla completa.

```chart-demo overview
```

## Capacidades

- Un conjunto completo de series de precios: velas, barras OHLC, línea, área, histograma, banda, además de los tipos derivados Heikin-Ashi, Renko y Point & Figure.
- Estudios exactos de order-flow: footprint, perfil de volumen y TPO (perfil de mercado).
- Carga histórica y actualizaciones en tiempo real mediante `setData` y `update`.
- Marcadores de operaciones, líneas de precio, crosshair, zoom, desplazamiento y cálculo automático del rango.
- Un motor de indicadores con aproximadamente 160 implementaciones de cálculo.
- Indicadores superpuestos (overlay), paneles de osciladores sincronizados y una leyenda controlada por el crosshair.
- Temas claro y oscuro, un menú contextual, un diálogo de indicadores y cambio de tipo de gráfico.

## Instalación

Instale el paquete desde npm e importe los módulos ES:

```bash
npm install @stocksharp/chart
```

```js
import { createChart, CandlestickSeries } from '@stocksharp/chart';
```

O bien, sin un bundler, incluya el objeto global precompilado `SSChart` con una etiqueta `<script>` como se muestra a continuación.

## Añadir un gráfico a una página

La compilación produce `dist/sschart.js`, que publica `window.SSChart`. Los valores de tiempo pasados a la API son marcas de tiempo Unix en segundos.

```html
<div id="chart" style="width: 800px; height: 400px"></div>
<script src="dist/sschart.js"></script>
```

Cree un gráfico, añada una serie y cargue los datos:

```js
const chart = SSChart.createChart(document.getElementById('chart'), {
  timeScale: { timeVisible: true },
  crosshair: { mode: SSChart.CrosshairMode.Normal },
});

const candles = chart.addSeries(SSChart.CandlestickSeries, {
  upColor: '#26a69a',
  downColor: '#ef5350',
  borderVisible: false,
});

candles.setData([
  { time: 1704153600, open: 120, high: 122, low: 119, close: 121 },
  { time: 1704240000, open: 121, high: 124, low: 120, close: 123 },
]);

candles.update({
  time: 1704326400,
  open: 123,
  high: 123.5,
  low: 122.8,
  close: 123.2,
});

chart.timeScale().fitContent();
```

Llamar a `update` con la marca de tiempo actual reemplaza el último punto. Una marca de tiempo más reciente añade un punto.

## Modos de gráfico

Cada tipo de serie tiene su propio tema con una demo en vivo y el JavaScript que la configura:

- [Velas](javascript_charts/candlestick.md) — velas OHLC clásicas.
- [Barras OHLC](javascript_charts/bar.md) — ticks de apertura/cierre sobre una barra de rango vertical.
- [Línea](javascript_charts/line.md) — una única polilínea a través de los cierres.
- [Área](javascript_charts/area.md) — una línea con un relleno degradado.
- [Histograma](javascript_charts/histogram.md) — barras verticales, normalmente de volumen.
- [Banda](javascript_charts/band.md) — un canal superior/inferior (envolventes, Bollinger).
- [Velas Heikin-Ashi](javascript_charts/heikin_ashi.md) — velas suavizadas que filtran el ruido.
- [Renko](javascript_charts/renko.md) — ladrillos guiados por el precio, independientes del tiempo.
- [Punto y Figura](javascript_charts/point_figure.md) — columnas de X/O del movimiento del precio.
- [Footprint](javascript_charts/footprint.md) — volumen de bid × ask en cada precio dentro de cada barra.
- [Perfil de volumen](javascript_charts/volume_profile.md) — volumen por precio con POC y área de valor.
- [TPO (Perfil de mercado)](javascript_charts/tpo.md) — tiempo pasado en cada precio por sesión.

Más allá de los tipos de serie, el gráfico también dispone de un [motor de indicadores](javascript_charts/indicators.md) con unos 160 estudios y de [relleno histórico perezoso (lazy backfill)](javascript_charts/backfill.md) que carga barras más antiguas a medida que se desplaza.

Para el editor visual de estrategias renderizado por la misma pila web, consulte [Diagrama en JavaScript](../javascript_diagram.md).

## Pila completa del gráfico del terminal

Los módulos bajo `src/chart` amplían el motor base con funciones de terminal:

- `IndicatorEngine`, renderizadores de indicadores, ajustes y el catálogo de cálculos.
- Un conmutador de tipo de gráfico para velas, barras, líneas, áreas, Heikin-Ashi, Renko y Point & Figure.
- Una leyenda, paneles secundarios sincronizados, un menú contextual y un diálogo de selección de indicadores.
- Recálculo de los indicadores activos cuando cambian los datos en tiempo real.

Use `src/chart/app.ts` como ejemplo de integración de la pila completa.

## Compilar desde el código fuente

Clone el repositorio y utilice los scripts de npm incluidos:

```bash
git clone https://github.com/StockSharp/Charts.git
cd Charts
npm install
npm run build
npm test
npm run serve
```

El servidor de desarrollo sirve la demo en `http://localhost:8791/demo/index.html`.

## Véase también

- [Diagrama en JavaScript](../javascript_diagram.md)
- [Repositorio de Charts](https://github.com/StockSharp/Charts)
- [Demo en vivo](https://stocksharp.github.io/Charts/demo/)
- [Componentes de gráficos de Windows](../charts.md)
