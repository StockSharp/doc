# Gráficos JavaScript

[Gráficos de negociación JavaScript de StockSharp](https://github.com/StockSharp/Charts) es una biblioteca de gráficos independiente para navegadores. Contiene el motor canvas `sschart` sin dependencias en tiempo de ejecución y el conjunto de módulos gráficos utilizado por el terminal web de StockSharp. Hay una versión funcional disponible en la [demostración en línea](https://stocksharp.github.io/Charts/demo/).

![Gráfico de negociación JavaScript de StockSharp](../../../../images/javascript_charts.jpg)

A diferencia de los componentes para Windows de `StockSharp.Xaml.Charting`, esta biblioteca se ejecuta en el navegador y dibuja directamente en un `canvas` HTML. El motor se expone mediante el objeto global `SSChart` y también se puede importar desde `src/sschart.ts` en una compilación TypeScript.

## Funciones

- Series de velas, barras OHLC, líneas, áreas, histogramas, Renko, punto y figura, perfil de volumen, clústeres y cajas.
- Carga del histórico y actualizaciones en tiempo real mediante `setData` y `update`.
- Marcadores de operaciones, líneas de precio, cruceta, zoom, desplazamiento y cálculo automático del rango.
- Un motor de indicadores con aproximadamente 160 implementaciones de cálculo.
- Indicadores superpuestos, paneles de osciladores sincronizados y una leyenda controlada por la cruceta.
- Temas claro y oscuro, menú contextual, diálogo de indicadores y cambio del tipo de gráfico.

## Añadir un gráfico a una página

La compilación genera `dist/sschart.js`, que publica `window.SSChart`. Los valores de tiempo se pasan a la API como marcas de tiempo Unix en segundos.

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
  upColor: '#00c853',
  downColor: '#ff3d57',
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

Una llamada a `update` con la marca de tiempo actual sustituye el último punto. Una marca más reciente añade un punto.

## Conjunto completo de módulos del terminal

Los módulos de `src/chart` amplían el motor base con funciones del terminal:

- `IndicatorEngine`, renderizadores y ajustes de indicadores, además del catálogo de cálculos.
- Un selector de tipo para velas, barras, líneas, áreas, Heikin-Ashi, Renko, punto y figura, clústeres y cajas.
- Leyenda, paneles secundarios sincronizados, menú contextual y diálogo de selección de indicadores.
- Recálculo de los indicadores activos cuando cambian los datos en tiempo real.

Utilice `src/chart/app.ts` como ejemplo de integración del conjunto completo.

## Compilar desde el código fuente

Clone el repositorio y utilice los scripts npm incluidos:

```bash
git clone https://github.com/StockSharp/Charts.git
cd Charts
npm install
npm run build
npm test
npm run serve
```

El servidor de desarrollo sirve la demostración en `http://localhost:8791/demo/index.html`.

## Véase también

- [Repositorio de Charts](https://github.com/StockSharp/Charts)
- [Demostración en línea](https://stocksharp.github.io/Charts/demo/)
- [Componentes gráficos para Windows](../charts.md)
