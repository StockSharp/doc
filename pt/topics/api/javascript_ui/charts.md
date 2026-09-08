# Gráficos em JavaScript

Os [Gráficos de negociação JS da StockSharp](https://github.com/StockSharp/JS-Charts) constituem uma biblioteca de gráficos para navegador, autónoma e sem dependências. É publicada no npm como [@stocksharp/chart](https://www.npmjs.com/package/@stocksharp/chart) e fornece o motor de `canvas` `sschart` utilizado pelo terminal web da StockSharp. Está disponível uma versão funcional na [demonstração online](https://stocksharp.github.io/JS-Charts/demo/).

![Gráfico de negociação JavaScript do StockSharp](../../../images/javascript_charts.jpg)

Ao contrário dos componentes Windows de `StockSharp.Xaml.Charting`, esta biblioteca é executada num navegador e desenha diretamente num `canvas` HTML. O motor é exposto através do objeto global `SSChart` (de `dist/sschart.js`) e também pode ser importado como módulos ECMAScript a partir do pacote npm (`import { createChart, CandlestickSeries } from '@stocksharp/chart'`).

## Demonstração online

O gráfico abaixo utiliza o motor real em execução nesta página — velas com um histograma de volume e uma média móvel. Arraste para deslocar, utilize a roda do rato para ampliar ou reduzir e prima o botão de expansão (no canto superior direito) para o abrir em ecrã inteiro.

```chart-demo overview
```

## Funcionalidades

- Um conjunto completo de séries de preço: velas, barras OHLC, linha, área, histograma, banda, além dos tipos derivados Heikin-Ashi, Renko e Point & Figure.
- Estudos exatos de order-flow: footprint, perfil de volume e TPO (market profile).
- Carregamento de histórico e atualizações em tempo real com `setData` e `update`.
- Marcadores de negócios, linhas de preço, cursor em cruz, zoom, deslocamento e cálculo automático do intervalo.
- Um motor de indicadores com aproximadamente 160 implementações de cálculo.
- Indicadores sobrepostos, painéis de osciladores sincronizados e uma legenda controlada pelo cursor em cruz.
- Temas claro e escuro, menu de contexto, diálogo de indicadores e mudança do tipo de gráfico.

## Instalação

Instale o pacote a partir do npm e importe os módulos ES:

```bash
npm install @stocksharp/chart
```

```js
import { createChart, CandlestickSeries } from '@stocksharp/chart';
```

Ou, sem um bundler, insira o global pré-compilado `SSChart` com uma tag `<script>`, conforme mostrado abaixo.

## Adicionar um gráfico a uma página

A compilação gera `dist/sschart.js`, que publica `window.SSChart`. Os valores de tempo passados à API são marcas de tempo Unix em segundos.

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

Uma chamada a `update` com a marca de tempo atual substitui o último ponto. Uma marca mais recente adiciona um ponto.

## Modos de gráfico

Cada tipo de série tem o seu próprio tópico com uma demonstração online e o JavaScript que a configura:

- [Candlestick](charts/candlestick.md) — velas OHLC clássicas.
- [Barras OHLC](charts/bar.md) — marcas de abertura/fecho numa barra vertical de intervalo.
- [Linha](charts/line.md) — uma única polilinha através dos fechos.
- [Área](charts/area.md) — uma linha com preenchimento em gradiente.
- [Histograma](charts/histogram.md) — barras verticais, tipicamente de volume.
- [Banda](charts/band.md) — um canal superior/inferior (envelopes, Bollinger).
- [Velas Heikin-Ashi](charts/heikin_ashi.md) — velas suavizadas que filtram o ruído.
- [Renko](charts/renko.md) — tijolos determinados pelo preço, independentes do tempo.
- [Ponto e Figura](charts/point_figure.md) — colunas de X/O do movimento de preço.
- [Footprint](charts/footprint.md) — volume de bid × ask em cada preço dentro de cada barra.
- [Perfil de volume](charts/volume_profile.md) — volume por preço com POC e área de valor.
- [TPO (Perfil de mercado)](charts/tpo.md) — tempo gasto em cada preço por sessão.

Além dos tipos de série, o gráfico também tem um [motor de indicadores](charts/indicators.md) com cerca de 160 estudos e [preenchimento de histórico a pedido](charts/backfill.md), que carrega barras mais antigas à medida que o utilizador desloca o gráfico.

Para conhecer o editor visual de estratégias apresentado pela mesma plataforma web, consulte [Diagrama em JavaScript](diagram.md).

## Camadas sobre o motor

Tudo o que foi descrito acima é o motor base, o ponto de entrada `@stocksharp/chart`. O restante está distribuído por pontos de entrada publicados em separado: importe o de que precisa, não é necessário copiar código-fonte.

| Ponto de entrada | O que dá |
|---|---|
| [Interface do gráfico](charts/ui.md) — `@stocksharp/chart/ui` | Interface pronta a usar: legenda, menu de contexto, seletor de tipo de gráfico, gestor de painéis, diálogo de indicadores. Exige a folha de estilos `@stocksharp/chart/ui.css`. |
| `@stocksharp/chart/indicators` | `IndicatorEngine`, renderizadores e definições de indicadores. Os cálculos propriamente ditos estão no pacote [@stocksharp/indicators](charts/indicators.md). |
| [Negociação a partir do gráfico](charts/trading.md) — `@stocksharp/chart/trading` | Camada de negociação: linhas de ordens, posição com o resultado atual, ordens de proteção e cotações. Não conhece a corretora — comunica a intenção e o anfitrião executa-a. |
| [Ferramentas de desenho](charts/drawings.md) — `@stocksharp/chart/drawings` | Ferramentas de desenho, ajuste às barras e registo de figuras próprias. |
| [Vários gráficos](charts/workspace.md) — `@stocksharp/chart/workspace` | Vários gráficos sincronizados, comparação de instrumentos, navegador de intervalo, modelos de indicadores. |
| [Guardar a disposição](charts/persistence.md) — `@stocksharp/chart/persistence` | Guardar e repor a disposição, com versionamento e migrações. |
| [Tempo e sessões de negociação](charts/time.md) — `@stocksharp/chart/time` | Calendário de negociação, sessões e fusos horários, contagem decrescente até ao fecho da barra. |
| `@stocksharp/chart/orderflow` | Análise por clusters: footprint e perfil de volume. |

Sem um empacotador, as mesmas funcionalidades estão disponíveis no pacote para navegador: `dist/sschartui.js` publica o objeto global `SSChartUI`.

## Compilar a partir do código-fonte

Clone o repositório e use os scripts npm incluídos:

```bash
git clone https://github.com/StockSharp/JS-Charts.git
cd JS-Charts
npm install
npm run build
npm test
npm run serve
```

O servidor de desenvolvimento disponibiliza a demonstração em `http://localhost:8791/demo/index.html`.

## Ver também

- [Diagrama em JavaScript](diagram.md)
- [Repositório JS-Charts](https://github.com/StockSharp/JS-Charts)
- [Demonstração online](https://stocksharp.github.io/JS-Charts/demo/)
- [Componentes de gráficos para Windows](../graphical_user_interface/charts.md)
