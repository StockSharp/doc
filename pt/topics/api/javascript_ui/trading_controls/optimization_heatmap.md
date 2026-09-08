# Mapa de calor de otimização

`OptimizationHeatmapWidget` desenha uma métrica sobre dois parâmetros: os valores do primeiro parâmetro seguem na horizontal, os do segundo na vertical e, no cruzamento, fica a célula colorida pelo que esse conjunto de parâmetros produziu. O controlo não sabe nada sobre a otimização em si — o mapa de uma métrica sobre dois eixos é o mesmo, seja qual for a forma como os pares foram calculados —, pelo que os pares são fornecidos pelo anfitrião juntamente com os nomes dos eixos e da métrica.

![Mapa de calor de otimização sobre dois parâmetros](../../../../images/javascript_controls_optimization_heatmap.png)

## Criação e atualização

```ts
import {
  HeatDirections,
  OptimizationHeatmapWidget,
  type HeatCell,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const heatmap = OptimizationHeatmapWidget.create(
  document.querySelector<HTMLElement>('#heatmap')!,
  {},
  { host },
);

const cells: HeatCell[] = [
  { x: '10', y: '00:05:00', value: 12_400 },
  { x: '10', y: '00:15:00', value: 9_150 },
  { x: '20', y: '00:05:00', value: -1_800 },
  { x: '20', y: '00:15:00', value: 15_900 },
];

heatmap.update({
  xLabel: 'Length',
  yLabel: 'Timeframe',
  metricLabel: 'Net profit',
  betterWhen: HeatDirections.Higher,
  cells,
});
```

A única dependência é `host`; a interface `OptimizationHeatmapDeps` não tem outros campos. O mapa não aceita processadores: uma célula é a média das execuções de um par e não uma execução, pelo que não há nada para abrir com um clique. O segundo argumento de `create` é o estado guardado da instância; o controlo não o lê nem escreve nele.

`update` substitui o mapa por inteiro: um par que saiu do conjunto deixa de existir, e a célula que dele restasse falaria de uma execução que já não consta do relatório.

O argumento de `update` é um objeto `HeatmapData` com os campos:

| Campo | Finalidade |
|---|---|
| `xLabel` | Nome do eixo horizontal, rotulado por baixo do mapa. |
| `yLabel` | Nome do eixo vertical, rotulado por cima do mapa. |
| `metricLabel` | Nome da métrica, apresentado no cabeçalho do painel. |
| `betterWhen` | Em que sentido é melhor: `HeatDirections.Higher` (`'higher'`) ou `HeatDirections.Lower` (`'lower'`). O campo é obrigatório — sem ele um mapa de drawdowns pintaria o pior canto com a cor da vitória. |
| `cells` | As medições `HeatCell`: `x` e `y` são os valores dos eixos em texto, `value` é um número. |

O tipo `HeatmapData` está declarado no módulo do controlo e a exportação de raiz do pacote não o reexporta: para o tipar explicitamente, importe-o do subcaminho `@stocksharp/trading-controls/optimization-heatmap-widget`.

A propriedade estática `OptimizationHeatmapWidget.TYPE` é igual a `ControlTypes.OptimizationHeatmap` — ao identificador `optimizationHeatmap`.

## Dados e apresentação

Os eixos são discretos, pelo que os seus valores são passados como texto: `10`, `00:05:00` e `True` são posições equivalentes num eixo. A ordem dos valores é numérica se todos eles forem números (caso contrário `10` ficaria antes de `2` e a forma do mapa passaria a ser consequência da escrita dos números) e textual nos restantes casos; para cadeias de largura fixa, como as que o .NET usa para escrever intervalos de tempo, a ordem textual coincide com a cronológica. A primeira linha da grelha fica na base do mapa: isto é um gráfico e o eixo Y cresce para cima.

Várias execuções sobre o mesmo par são reduzidas a uma célula — a média, com o número de execuções; um registo com métrica não numérica é descartado em vez de estragar a célula inteira. A cor é calculada a partir de um valor de referência: se as medições atravessam o zero, a referência é o zero; caso contrário é o meio do intervalo, porque ancorar no zero quando o varrimento nunca lá chegou daria uma mancha uniforme sem contraste. A amplitude até à cor plena é igual dos dois lados, pelo que a mesma saturação em qualquer ponto significa o mesmo desvio da métrica. A direção `betterWhen` está refletida no sinal: o melhor resultado é sempre pintado com a cor de subida.

Os números não são impressos nas células: num varrimento de quarenta por quarenta os algarismos dentro da célula são ilegíveis, e a leitura da cor é justamente o objetivo do mapa. Um par que nunca foi executado não fica vazio, mas riscado a diagonal: um par por testar e um par com resultado zero são factos diferentes que uma escala com o zero no ponto neutro desenharia da mesma maneira. A melhor célula é contornada com a cor da grelha — a única cor sem direção da paleta.

Por cima do mapa é desenhada uma legenda com essas duas cores e três etiquetas: o limite inferior, o valor de referência e o limite superior. As etiquetas dos eixos são desbastadas quando os valores deixam de caber, mas o valor extremo do eixo é sempre rotulado. Os números são apresentados por `formatStatistic` — o mesmo formato do painel de estatísticas: arredondamento a duas casas decimais.

Ao passar o cursor sobre uma célula medida aparece uma dica com o valor dos dois eixos e da métrica. O número de execuções só é acrescentado quando foi feita a média de mais do que uma execução, e a marca `Best` apenas na melhor célula. Sobre uma célula riscada não há dica: já se vê que não foi testada. A dica encosta-se aos limites do `canvas` para não sair fora nas células das extremidades.

Enquanto não houver uma única medição, em vez do mapa é apresentada uma mensagem substituta com o texto da chave `NoOptimizationResults`.

## O que faz o controlo e o que fica a cargo do anfitrião

O controlo constrói a estrutura do painel, ajusta o `canvas` ao contentor através de `ResizeObserver` e cria um buffer em píxeis físicos segundo o `devicePixelRatio`, sem o qual o mapa seria desenhado com uma grelha de linhas de cabelo. As cores e o tipo de letra são pedidos a `host.presentation.canvasPalette()` em cada desenho: `up` e `down` são os dois lados da escala, `grid` é a grelha, os riscados, o contorno da melhor célula e as etiquetas, `font` é o tipo de letra do texto no `canvas`. Aqui o pacote não escolhe uma paleta própria, pelo que uma mudança de tema no anfitrião redesenha o mapa com as novas cores. A transparência é a única coisa de que o próprio mapa dispõe.

O botão de fecho do painel invoca `host.close()`, a instância regista-se com `host.register` e cancela o registo em `dispose`. Todo o texto visível é pedido através de `host.t`: `OptimizationHeatmap`, `OptimizationHeatmapChart`, `ClosePanel`, `NoOptimizationResults`, `Runs`, `Best`. O controlo não guarda definições próprias em `host.preferences` e não tem chaves.

O anfitrião fornece os dados: o mapa não lança o varrimento, não escolhe nem calcula a métrica e não adivinha a direção de «melhor» — desenha o que lhe for passado em `update`.

## Métodos públicos

- `update(data)` — mostrar o mapa completo.
- `dispose()` — desligar o observador de tamanho, chamar `host.unregister` e retirar o elemento raiz.

## Funções auxiliares

Toda a geometria do mapa está num módulo separado e é exportada pelo pacote — pode ser usada sem o controlo:

- `layoutHeatmap(input)` — a disposição do mapa: grelha, lacunas, etiquetas, legenda e escala; `null` se não houver medições.
- `hitHeatmap(layout, x, y)` — a célula sob um ponto ou `null`.
- `heatScale(buckets)` — o valor de referência, a amplitude e os limites do intervalo.
- `tintOf(value, scale, betterWhen)` — a saturação entre −1 e 1, onde o positivo é sempre melhor.
- `valueAt(tint, scale, betterWhen)` — a transformação inversa, para as etiquetas da legenda.
- `HeatDirections` — as direções `Higher` e `Lower`.

## Consulte também

- [Controlos de negociação JavaScript](../trading_controls.md)
- [Superfície de otimização](optimization_surface.md)
- [Estatísticas](statistics.md)
- [Curva de capital](equity.md)
