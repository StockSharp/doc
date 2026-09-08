# Ferramentas de desenho

`DrawingController` é a camada de desenho manual sobre o gráfico: linhas, formas, níveis de Fibonacci e esboços de posição. O controlador guarda as figuras como objetos JSON puros, liga-as aos primitivos do `canvas`, faz passar cada alteração pela pilha de anulação do gráfico e assume a construção passo a passo com o rato.

## Ligação

A camada é distribuída num ponto de entrada próprio do pacote `@stocksharp/chart`:

```ts
import { BuiltInDrawingType, DrawingController } from '@stocksharp/chart/drawings';
```

Importar o ponto de entrada regista de imediato todos os tipos de desenho incorporados no catálogo comum `drawingDefinitionRegistry`.

## Criação e atualização

O controlador só precisa do gráfico; por predefinição, a pilha de comandos também vem dele (`chart.commandStack()`), pelo que anular e refazer funcionam em conjunto com as restantes ações no gráfico:

```ts
import { CandlestickSeries, createChart } from '@stocksharp/chart';
import { BuiltInDrawingType, DrawingController } from '@stocksharp/chart/drawings';

const chart = createChart(document.getElementById('chart')!, { timeScale: { timeVisible: true } });
chart.addSeries(CandlestickSeries, {}).setData(candles);

const drawings = new DrawingController({ chart });

// Nível horizontal: um ponto; o tempo é tempo Unix em segundos.
const level = drawings.create(
  BuiltInDrawingType.HorizontalLine,
  [{ time: 1_712_000_000, price: 68_000 }],
  { options: { color: '#f5c542', lineWidth: 2 } },
);

// Uma linha de tendência de dois pontos num subpainel indica-se através de paneId.
drawings.create(
  BuiltInDrawingType.TrendLine,
  [
    { time: 1_712_000_000, price: 67_400 },
    { time: 1_712_600_000, price: 69_150 },
  ],
  { paneId: 'main' },
);

drawings.updateOptions(level.id, { lineWidth: 3 });
drawings.setLocked(level.id, true);

// O instantâneo do conjunto chega ordenado por zOrder.
drawings.subscribe(items => console.log(items.length));

chart.commandStack().undo();
```

`create` preenche os campos em falta: `paneId` é por predefinição `main`, `visible` é `true`, `locked` é `false`, `zOrder` fica uma unidade acima do máximo atual e as opções são aplicadas sobre as `defaultOptions` do tipo. `add` insere uma instância já pronta na íntegra, `duplicate` copia uma existente, `remove` e `clear` eliminam. Cada uma destas chamadas coloca no histórico exatamente um comando anulável.

`update` altera qualquer combinação de campos (`points`, `options`, `paneId`, `visible`, `locked`, `zOrder`); `updateOptions`, `setVisible`, `setLocked` e `moveToPane` são formas abreviadas para os casos frequentes. Antes de ser escrita, a instância é normalizada: os pontos e as opções são verificados quanto à compatibilidade com JSON e congelados, o número de pontos é confrontado com o esquema do tipo e o painel com os painéis existentes no gráfico.

## Tipos incorporados

Os identificadores estão reunidos em `BuiltInDrawingType`; o valor de texto é o próprio campo `type` da figura guardada.

| Constante | Valor | Pontos | Opções |
|---|---|---|---|
| `HorizontalLine` | `horizontal-line` | 1 | `LineDrawingOptions` |
| `VerticalLine` | `vertical-line` | 1 | `LineDrawingOptions` |
| `TrendLine` | `trend-line` | 2 | `LineDrawingOptions` |
| `Ray` | `ray` | 2 | `LineDrawingOptions` |
| `Rectangle` | `rectangle` | 2 | `RectangleDrawingOptions` |
| `Text` | `text` | 1 | `TextDrawingOptions` |
| `Note` | `note` | 1 | `TextDrawingOptions` |
| `FibonacciRetracement` | `fibonacci-retracement` | 2 | `FibonacciDrawingOptions` |
| `Measure` | `measure` | 2 | `MeasureDrawingOptions` |
| `LongPosition` | `long-position` | 3 | `PositionDrawingOptions` |
| `ShortPosition` | `short-position` | 3 | `PositionDrawingOptions` |

Os conjuntos de opções distinguem-se pela finalidade:

- `LineDrawingOptions` — `color`, `lineWidth` (no intervalo (0, 20]), `lineStyle` (0…4).
- `RectangleDrawingOptions` — o mesmo mais `fillColor` para o preenchimento.
- `TextDrawingOptions` — `text` (até 10 000 caracteres, as mudanças de linha contam), `color`, `backgroundColor`, `borderColor`, `borderWidth`, `fontSize`, `fontFamily`, `padding`. `Note` distingue-se de `Text` apenas nos valores predefinidos: fundo, contorno e margens maiores.
- `FibonacciDrawingOptions` — `levels` (de 2 a 32 valores no intervalo [-5, 5]; os duplicados são removidos e a lista é ordenada), `labelsVisible`, `extendRight`, além de `color`, `lineWidth`, `lineStyle`, `fillColor`, `fontSize`.
- `MeasureDrawingOptions` — `color`, `lineWidth`, `fillColor`, `labelColor`, `labelBackgroundColor`, `fontSize`. A etiqueta mostra a variação de preço, a percentagem e a duração do intervalo selecionado.
- `PositionDrawingOptions` — `entryColor`, `targetColor`, `stopColor`, `targetFillColor`, `stopFillColor`, `textColor`, `lineWidth`, `fontSize` e `quantity`. Os três pontos são dados por ordem: entrada, objetivo, stop; a partir deles são calculados o lucro, o risco e o rácio R:R apresentados nas etiquetas.

Um valor de opção que não passe a verificação de tipo origina uma exceção — não é possível guardar uma figura com uma largura de linha inválida ou uma cor vazia.

## Construção com o rato

A introdução passo a passo é conduzida pelo próprio controlador: coloca o gráfico em modo de desenho e subscreve os cliques e o cursor em cruz.

```ts
drawings.subscribeCreation(state => {
  if (state === null) return;                    // construção concluída ou cancelada
  console.log(state.name, state.points.length, state.minimumPoints, state.maximumPoints);
});

drawings.beginCreation(BuiltInDrawingType.Rectangle, { options: { color: '#26a69a' } });

// Cancelamento por Esc enquanto a figura não tiver o número de pontos necessário.
document.addEventListener('keydown', event => {
  if (event.key === 'Escape') drawings.cancelCreation();
});
```

Cada clique acrescenta um ponto, passado pelo íman; o movimento do cursor atualiza o rascunho, que é desenhado pelo mesmo primitivo da figura final mas não entra no histórico. O painel fica fixado no primeiro clique e os cliques nos outros painéis são ignorados. Assim que forem reunidos tantos pontos quantos o máximo do tipo permite, a construção termina sozinha e é criada uma figura normal. `finishCreation` fecha a construção antes do tempo e devolve `null` se houver menos pontos do que o mínimo; `cancelCreation` descarta o rascunho; `creation` devolve o instantâneo atual `DrawingCreationSnapshot`.

## Ajuste às barras

O íman puxa o ponto para os valores das séries do painel atual — o cálculo é feito em coordenadas de ecrã, pela distância vertical até ao candidato.

```ts
import { DrawingMagnetMode } from '@stocksharp/chart/drawings';

const drawings = new DrawingController({
  chart,
  magnet: { mode: DrawingMagnetMode.Weak, maxDistance: 12 },
});

drawings.applyMagnetOptions({ mode: DrawingMagnetMode.Strong });
console.log(drawings.magnetOptions());
```

`DrawingMagnetMode.None` desliga o ajuste, `Weak` (o modo predefinido) só atrai dentro de `maxDistance` — por predefinição 10 píxeis CSS — e `Strong` atrai sempre para o valor mais próximo. Alterar as definições durante a construção recalcula de imediato o ponto de pré-visualização.

## Guardar e repor

`DrawingInstance` não contém deliberadamente objetos de tempo de execução, pelo que o conjunto de desenhos é serializado tal como está:

```ts
import type { DrawingInstance } from '@stocksharp/chart/drawings';

const saved = JSON.stringify(drawings.drawings());

const result = drawings.replaceAll(JSON.parse(saved) as DrawingInstance[], { unknownType: 'skip' });
console.log(result.restored.length, result.skipped);
```

`replaceAll` substitui todo o conjunto: primeiro são verificadas todas as instâncias de entrada (identificadores repetidos são um erro), depois as figuras antigas são retiradas do gráfico e as novas acrescentadas. Se pelo menos uma figura não entrar, o estado anterior é reposto. Um `type` desconhecido, com a política `skip` (a predefinida), vai para `skipped` com a razão `unknown-type`; com `error`, interrompe a reposição. A reposição limpa o histórico de comandos, pelo que não pode ser chamada dentro de uma transação.

## Tipos de desenho próprios

O catálogo de tipos é extensível. Basta descrever a definição e devolver a ligação ao primitivo — o invólucro pronto, com seleção, marcadores e arrastamento, é dado por `createInteractiveDrawingBinding`:

```ts
import { createInteractiveDrawingBinding, registerDrawing } from '@stocksharp/chart/drawings';

registerDrawing({
  type: 'price-band',
  name: 'Price Band',
  points: { min: 2, max: 2 },
  defaultOptions: { color: '#4a9eff' },
  normalizeOptions: options => Object.freeze({ color: String(options.color).trim() }),
  create: (instance, events) => createInteractiveDrawingBinding(instance, events, {
    draw(context) {
      const [first, second] = context.points;
      if (second === undefined) return;
      context.context.strokeStyle = String(context.instance.options.color);
      context.context.strokeRect(
        context.plot.x, Math.min(first.y, second.y),
        context.plot.width, Math.abs(second.y - first.y),
      );
    },
    hitTest(point, context) {
      const [first, second] = context.points;
      if (second === undefined) return null;
      return point.y >= Math.min(first.y, second.y) && point.y <= Math.max(first.y, second.y)
        ? { cursor: 'move' }
        : null;
    },
  }),
});

drawings.create('price-band', [
  { time: 1_712_000_000, price: 67_800 },
  { time: 1_712_600_000, price: 68_900 },
]);
```

`draw` recebe os pontos de ecrã, o retângulo da área de desenho, o tema, o fator de escala e o indicador de seleção; `hitTest` responde se o cursor caiu dentro do corpo da figura. Os opcionais `autoscaleInfo` e `handleColor` definem a participação no ajuste automático de escala e a cor dos marcadores. `normalizeOptions` é invocada antes de cada escrita no modelo — é o único sítio onde vale a pena validar os valores das opções.

O arrastamento do corpo ou de um ponto isolado passa pelos eventos `preview` (estados intermédios, não escritos no histórico), `commit` (um único comando «Edit drawing») e `cancel` (regresso ao estado anterior ao gesto). Uma figura bloqueada (`locked`) não se arrasta nem mostra marcadores.

O catálogo também pode ser gerido diretamente: `unregisterDrawing(type)`, `getDrawingDefinition(type)`, `getDrawingTypes()`, e `DrawingDefinitionRegistry` permite criar um catálogo à parte e passá-lo ao controlador no parâmetro `registry`.

## Métodos públicos

`DrawingController`:

- `drawings()`, `get(id)`, `has(id)` — leitura do conjunto atual.
- `create(type, points, options?)`, `add(instance)`, `duplicate(id, duplicateId?)` — adição de figuras.
- `update(id, patch)`, `updateOptions(id, patch)`, `setVisible(id, visible)`, `setLocked(id, locked)`, `moveToPane(id, paneId)` — alteração.
- `remove(id)`, `clear()` — eliminação.
- `beginCreation(type, options?)`, `finishCreation()`, `cancelCreation()`, `creation()` — construção com o rato.
- `magnetOptions()`, `applyMagnetOptions(patch)` — ajuste às barras.
- `replaceAll(instances, options?)` — reposição de um conjunto guardado.
- `subscribe(listener)` / `unsubscribe(listener)`, `subscribeCreation(listener)` / `unsubscribeCreation(listener)` — subscrições.
- `dispose()` — libertar os recursos.

O construtor recebe `chart` (obrigatório) e ainda `registry`, `commandStack`, `idFactory` e `magnet`.

O ponto de entrada exporta também as restantes partes da camada: `DrawingMagnet` para calcular o ajuste por conta própria, `InteractiveDrawingPrimitive` juntamente com `createInteractiveDrawingBinding`, as funções de validação `normalizeDrawingInstance` e `normalizeDrawingOptions`, os conjuntos prontos de definições `builtInLineDrawingDefinitions`, `builtInShapeDrawingDefinitions`, `builtInAnalysisDrawingDefinitions`, `builtInPositionDrawingDefinitions` e as funções que lhes correspondem — `registerBuiltInLineDrawings`, `registerBuiltInShapeDrawings`, `registerBuiltInAnalysisDrawings`, `registerBuiltInPositionDrawings` — para registo num catálogo próprio.

## Veja também

- [Gráficos em JavaScript](../charts.md)
- [Candlestick](candlestick.md)
- [Indicadores](indicators.md)
- [Preenchimento de histórico](backfill.md)
