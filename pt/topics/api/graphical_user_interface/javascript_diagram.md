# Diagrama em JavaScript

O [StockSharp JS Diagram](https://github.com/StockSharp/Diagram) é um componente de navegador independente e sem dependências que renderiza o esquema visual de estratégia do [Designer](../../designer.md) — o mesmo diagrama de blocos de elementos conectados — em um `canvas` HTML. Ele é publicado no npm como [@stocksharp/diagram](https://www.npmjs.com/package/@stocksharp/diagram) e alimenta os diagramas de estratégia somente leitura exibidos em todos os sites web da StockSharp.

Uma estratégia é descrita como um **esquema**: um conjunto de *nós* (elementos como uma fonte de candles, um indicador, uma condição ou uma ordem) interligados através de *portas* tipadas. O componente recebe esse esquema mais uma *paleta* (o catálogo de tipos de elementos, suas portas e cores) e o desenha.

## Demonstração ao vivo

O diagrama abaixo é o motor real em execução nesta página — um esqueleto mínimo de estratégia "fonte de dados → indicador → gráfico". Arraste o canvas para deslocar, use a roda do mouse para dar zoom e pressione o botão de expandir para abri-lo em tela cheia.

```diagram-demo sma
```

Os três blocos são uma fonte de **Candles** alimentando um **Indicator** (uma média móvel simples); tanto os candles quanto a saída do indicador são desenhados em um elemento **Chart**. Este é o menor padrão completo no Designer: produzir dados, transformá-los, visualizá-los.

## Instalação

Instale o pacote a partir do npm:

```bash
npm install @stocksharp/diagram
```

Em seguida, importe os módulos ES — `import { renderScheme } from '@stocksharp/diagram/embed'` para o embed somente leitura, ou `import { StockSharpDiagram } from '@stocksharp/diagram'` para o [editor interativo](javascript_diagram/editor.md).

## Incorporando um diagrama

O componente expõe `renderScheme(host, paletteUrl, scheme)` a partir do ponto de entrada `@stocksharp/diagram/embed`. Forneça a ele um elemento hospedeiro, a URL de um JSON de paleta e um esquema construído a partir de `nodes` e `links`:

```js
import { renderScheme } from '@stocksharp/diagram/embed';

const scheme = {
  nodes: [
    { id: 'candles', typeId: 'CandleElement',    name: 'Candles', x: 60,  y: 130 },
    { id: 'sma',     typeId: 'IndicatorElement', name: 'SMA',     x: 340, y: 60  },
    { id: 'chart',   typeId: 'ChartElement',     name: 'Chart',   x: 620, y: 130 },
  ],
  links: [
    { from: 'candles', fromPort: 'Output', to: 'sma',   toPort: 'Input' },
    { from: 'sma',     fromPort: 'Output', to: 'chart', toPort: 'Input' },
    { from: 'candles', fromPort: 'Output', to: 'chart', toPort: 'Input' },
  ],
};

renderScheme(document.getElementById('diagram'), '/data/designer-palette.json', scheme);
```

O `typeId` de cada nó deve existir na paleta; tipos desconhecidos são renderizados como blocos de espaço reservado. As portas são referenciadas por sua `key`, e um link é válido quando o tipo da porta de origem é compatível com o tipo da porta de destino. `renderScheme` é somente leitura: o motor faz o layout, aplica o tema (segue a configuração claro/escuro da página) e permite ao visualizador deslocar, dar zoom e expandir, mas não edita o esquema.

O mesmo componente também funciona como um **editor** completo — arraste elementos de uma paleta, conecte portas, edite e exclua nós, desfaça/refaça. Consulte [Editor interativo](javascript_diagram/editor.md) e [Eventos e API](javascript_diagram/events.md).

## Veja também

- [Editor interativo](javascript_diagram/editor.md)
- [Eventos e API](javascript_diagram/events.md)
- [Gráficos em JavaScript](charts/javascript_charts.md)
- [Designer](../../designer.md) — o editor visual de estratégias para desktop
- [Repositório do Diagram](https://github.com/StockSharp/Diagram)
