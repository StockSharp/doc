# Diagrama em JavaScript

O [StockSharp JS Diagram](https://github.com/StockSharp/JS-Diagram) é um componente de navegador independente e sem dependências que apresenta o esquema visual de estratégia do [Designer](../../designer.md) — o mesmo diagrama de blocos de elementos ligados — num `canvas` HTML. É publicado no npm como [@stocksharp/diagram](https://www.npmjs.com/package/@stocksharp/diagram) e alimenta os diagramas de estratégia só de leitura apresentados em todos os sites web da StockSharp.

Uma estratégia é descrita como um **esquema**: um conjunto de *nós* (elementos como uma fonte de velas, um indicador, uma condição ou uma ordem) interligados através de *portas* tipadas. O componente recebe esse esquema e uma *paleta* (o catálogo de tipos de elementos, respetivas portas e cores) e apresenta-o.

## Demonstração em direto

O diagrama abaixo utiliza o motor real em execução nesta página — um esqueleto mínimo de estratégia «fonte de dados → indicador → gráfico». Arraste o `canvas` para deslocar, utilize a roda do rato para ampliar ou reduzir e prima o botão de expansão para o abrir em ecrã inteiro.

```diagram-demo sma
```

Os três blocos representam uma fonte de **Velas** que alimenta um **Indicador** (uma média móvel simples); tanto as velas como a saída do indicador são apresentadas num elemento **Gráfico**. Este é o padrão completo mais pequeno no Designer: produzir dados, transformá-los e visualizá-los.

## Instalação

Instale o pacote a partir do npm:

```bash
npm install @stocksharp/diagram
```

Em seguida, importe os módulos ES — `import { renderScheme } from '@stocksharp/diagram/embed'` para o embed só de leitura, ou `import { StockSharpDiagram } from '@stocksharp/diagram'` para o [editor interativo](javascript_diagram/editor.md).

## Incorporando um diagrama

O componente expõe `renderScheme(host, paletteUrl, scheme)` a partir do ponto de entrada `@stocksharp/diagram/embed`. Forneça-lhe um elemento anfitrião, o URL de um JSON de paleta e um esquema construído a partir de `nodes` e `links`:

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

O `typeId` de cada nó deve existir na paleta; os tipos desconhecidos são apresentados como blocos de espaço reservado. As portas são referenciadas pela respetiva `key`, e uma ligação é válida quando o tipo da porta de origem é compatível com o tipo da porta de destino. `renderScheme` é só de leitura: o motor organiza o esquema, aplica o tema (segue a definição de modo claro/escuro da página) e permite ao visualizador deslocar, ampliar, reduzir e expandir, mas não editar o esquema.

O mesmo componente também funciona como um **editor** completo — arraste elementos de uma paleta, ligue portas, edite e elimine nós, anule ou refaça operações. Consulte [Editor interativo](javascript_diagram/editor.md) e [Eventos e API](javascript_diagram/events.md).

## Veja também

- [Editor interativo](javascript_diagram/editor.md)
- [Eventos e API](javascript_diagram/events.md)
- [Gráficos em JavaScript](charts/javascript_charts.md)
- [Designer](../../designer.md) — o editor visual de estratégias para desktop
- [Repositório do JS-Diagram](https://github.com/StockSharp/JS-Diagram)
