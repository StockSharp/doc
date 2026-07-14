# Gráficos JavaScript

[Gráficos de negociação JavaScript do StockSharp](https://github.com/StockSharp/Charts) é uma biblioteca de gráficos autónoma para navegadores. Contém o motor canvas `sschart` sem dependências de execução e o conjunto de módulos de gráficos usado pelo terminal web StockSharp. Está disponível uma versão funcional na [demonstração online](https://stocksharp.github.io/Charts/demo/).

![Gráfico de negociação JavaScript do StockSharp](../../../../images/javascript_charts.jpg)

Ao contrário dos componentes Windows de `StockSharp.Xaml.Charting`, esta biblioteca é executada no navegador e desenha diretamente num `canvas` HTML. O motor é exposto através do objeto global `SSChart` e também pode ser importado de `src/sschart.ts` numa compilação TypeScript.

## Funcionalidades

- Séries de velas, barras OHLC, linhas, áreas, histogramas, Renko, ponto e figura, perfil de volume, clusters e caixas.
- Carregamento de histórico e atualizações em tempo real com `setData` e `update`.
- Marcadores de negócios, linhas de preço, cursor em cruz, zoom, deslocamento e cálculo automático do intervalo.
- Um motor de indicadores com aproximadamente 160 implementações de cálculo.
- Indicadores sobrepostos, painéis de osciladores sincronizados e uma legenda controlada pelo cursor em cruz.
- Temas claro e escuro, menu de contexto, diálogo de indicadores e mudança do tipo de gráfico.

## Adicionar um gráfico a uma página

A compilação gera `dist/sschart.js`, que publica `window.SSChart`. Os valores de tempo são passados à API como marcas de tempo Unix em segundos.

```html
<div id="chart" style="width: 800px; height: 400px"></div>
<script src="dist/sschart.js"></script>
```

Crie um gráfico, adicione uma série e carregue os dados:

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

Uma chamada a `update` com a marca de tempo atual substitui o último ponto. Uma marca mais recente adiciona um ponto.

## Conjunto completo de módulos do terminal

Os módulos em `src/chart` ampliam o motor base com funcionalidades do terminal:

- `IndicatorEngine`, renderizadores e definições de indicadores, além do catálogo de cálculos.
- Um seletor de tipo para velas, barras, linhas, áreas, Heikin-Ashi, Renko, ponto e figura, clusters e caixas.
- Legenda, painéis secundários sincronizados, menu de contexto e diálogo de seleção de indicadores.
- Recálculo dos indicadores ativos quando os dados em tempo real mudam.

Use `src/chart/app.ts` como exemplo de integração do conjunto completo.

## Compilar a partir do código-fonte

Clone o repositório e use os scripts npm incluídos:

```bash
git clone https://github.com/StockSharp/Charts.git
cd Charts
npm install
npm run build
npm test
npm run serve
```

O servidor de desenvolvimento disponibiliza a demonstração em `http://localhost:8791/demo/index.html`.

## Ver também

- [Repositório Charts](https://github.com/StockSharp/Charts)
- [Demonstração online](https://stocksharp.github.io/Charts/demo/)
- [Componentes de gráficos para Windows](../charts.md)
