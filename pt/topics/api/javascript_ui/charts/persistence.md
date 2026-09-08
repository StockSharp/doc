# Guardar a disposição

`ChartStatePersistence` reúne a disposição do gráfico — opções, painéis, escalas de preços, séries, indicadores e objetos gráficos — num único instantâneo JSON validado e repõe-na a partir dele. Os dados das barras não entram no instantâneo: o que fica guardado é a configuração, e as cotações vêm da sua própria origem de dados.

A camada não é dona nem do armazenamento nem da regra de nomes das chaves. Onde escrever (ficheiro, servidor, `localStorage`, IndexedDB) e como separar os instantâneos (por disposição, por instrumento, por utilizador) é decidido pela aplicação — através da implementação de `ChartStateStorage` e da função `key`.

## Criação e atualização

A importação faz-se a partir do ponto de entrada `@stocksharp/chart/persistence`:

```ts
import { createChart, CandlestickSeries } from '@stocksharp/chart';
import { DrawingController } from '@stocksharp/chart/drawings';
import {
  ChartStatePersistence,
  NativeChartLayoutAdapter,
  IndicatorEngineStateAdapter,
  type ChartStateStorage,
  type IndicatorEnginePersistenceApi,
} from '@stocksharp/chart/persistence';

declare const indicatorEngine: IndicatorEnginePersistenceApi;

const chart = createChart(document.querySelector<HTMLElement>('#chart')!, {});
chart.addSeries(CandlestickSeries, { id: 'price', upColor: '#26a69a', downColor: '#ef5350' });

const storage: ChartStateStorage = {
  load: key => localStorage.getItem(key),
  save: (key, value) => { localStorage.setItem(key, value); },
  remove: key => { localStorage.removeItem(key); },
};

const persistence = new ChartStatePersistence<{ layoutId: string; symbol: string }>({
  layout: new NativeChartLayoutAdapter({ chart, mainPaneId: 'main' }),
  indicators: new IndicatorEngineStateAdapter({ engine: indicatorEngine }),
  drawings: new DrawingController({ chart }),
  storage,
  key: ({ layoutId, symbol }) => `chart:${layoutId}:${symbol}`,
  pretty: true,
});

const context = { layoutId: 'desk', symbol: 'BTC@IMEX' };

await persistence.save(context);

const restored = await persistence.load(context);
if (restored !== null) {
  console.log(restored.state.panes.length);
  console.log(restored.drawings.skipped);   // objetos de tipo desconhecido foram ignorados, não perderam a reposição
}
```

O parâmetro de tipo `TContext` é aquilo que se passa a `save`, `load` e `remove`; a função `key` converte o contexto numa cadeia de chave e tem de devolver uma cadeia não vazia. `pretty: true` escreve o JSON com avanços.

O `DrawingController` deve ser a mesma instância que serve o desenho no gráfico — caso contrário será guardado um conjunto de objetos vazio.

## Adaptadores

`ChartStatePersistence` não conhece nem a API nativa do gráfico nem o motor de indicadores: trabalha através de dois adaptadores. As implementações prontas fazem parte do mesmo módulo mas, para uma arquitetura própria, podem passar-se outras — as interfaces `ChartStateLayoutAdapter` (`capture`, `restore`) e `ChartStateIndicatorAdapter` (`capture`, `clear`, `restore`) são públicas.

**`NativeChartLayoutAdapter`** captura e repõe a disposição do próprio gráfico: as opções do gráfico, os painéis com a sua ordem, altura, altura mínima e estado (`normal`, `minimized`, `maximized`), as definições das escalas de preços e ainda as séries com o seu tipo, painel, escala e opções de estilo. Opções do construtor:

- `chart` — a instância do gráfico (obrigatória).
- `mainPaneId` — o identificador do painel raiz que sobrevive à reposição; por predefinição `main`, caso contrário o primeiro painel.
- `createSeries(series, pane)` — criação própria da série em vez do registo de tipos, quando é preciso ligá-la a uma origem de dados.
- `includeSeries(series)` — filtro: uma série para a qual se devolveu `false` não é guardada nem removida na reposição.
- `onRemoveSeries(series)` — invocado para uma série alheia que a reposição ainda assim teve de desligar, porque o seu painel não faz parte da disposição carregada.
- `onUnknownSeries(series)` — o tipo da série não existe no registo.

Uma série com a opção `persist: false` é excluída do instantâneo tal como uma série recusada pelo filtro `includeSeries`.

**`IndicatorEngineStateAdapter`** guarda a configuração dos indicadores — tipo, parâmetros, estilos de desenho, ligação ao painel e à escala, visibilidade e origem — mas não os valores calculados: depois da reposição são calculados de novo. Opções do construtor:

- `engine` — o motor de indicadores que implementa `IndicatorEnginePersistenceApi` (`getIndicators`, `removeAll`, `add`, `setVisible`).
- `resolveTargetPaneId(indicator)` — correspondência entre o painel guardado e o painel do anfitrião, quando os identificadores diferem.
- `onUnknownIndicator(indicator)` — o motor não conseguiu criar um indicador desse tipo.
- `onUnknownStyle(indicator, styleId)` — nos estilos surgiu um identificador que o indicador não tem.

Um indicador calculado sobre a saída de outro indicador é reposto depois dele: o adaptador ordena por si a cadeia de origens e assinala um erro se a referência apontar para um indicador inexistente ou se o grafo tiver um ciclo.

## Formato do estado e migrações

O instantâneo é descrito pelo tipo `ChartStateV1` com os campos `schemaVersion`, `chartOptions`, `panes`, `series`, `indicators`, `drawings`; a versão atual do esquema é a constante `CHART_STATE_SCHEMA_VERSION` (igual a 1).

- `serializeChartState(state, { pretty })` — validar e converter o estado numa cadeia JSON.
- `deserializeChartState(value, { migrations })` — analisar a cadeia (ou aceitar um objeto já pronto), aplicar as migrações até à versão atual e validar o resultado.
- `normalizeChartStateV1(value)` — validação e congelamento do estado: chaves estranhas, identificadores duplicados, referências a painéis inexistentes e uma disposição sem um único painel são rejeitadas.
- `normalizePersistedObject(value, path, { omitUndefined })` — cópia profunda de JSON arbitrário para um objeto imutável; ciclos, valores não numéricos, aninhamento excessivo e as chaves `__proto__`, `prototype`, `constructor` são proibidos.

Os instantâneos antigos são elevados por migrações passo a passo. O registo comum `chartStateMigrations` já contém a passagem da versão 0 para a versão 1, e os passos próprios registam-se assim:

```ts
import {
  ChartStateMigrationRegistry,
  deserializeChartState,
} from '@stocksharp/chart/persistence';

const migrations = new ChartStateMigrationRegistry();
migrations.register(1, state => ({ ...state, schemaVersion: 2 }));

const state = deserializeChartState(json, { migrations });
```

Cada migração faz avançar o estado exatamente uma versão e é obrigada a definir o novo valor de `schemaVersion`. Um instantâneo cuja versão seja superior à suportada não é aceite para carregamento.

## Métodos públicos

- `snapshot()` — reunir o estado atual do gráfico em `ChartStateV1`, sem tocar no armazenamento.
- `restore(state)` — aplicar o estado ao gráfico; devolve `{ state, drawings }`, em que `drawings` contém as listas `restored` e `skipped`.
- `save(context)` — capturar o instantâneo, serializá-lo e escrevê-lo no armazenamento com a chave calculada; devolve o estado guardado.
- `load(context)` — ler o registo pela chave, aplicar as migrações e repô-lo; `null` se o registo não existir.
- `remove(context)` — eliminar o registo do armazenamento.

A ordem de reposição é fixa: primeiro os indicadores são libertados, depois é reposta a disposição dos painéis e das séries, depois os indicadores e, por fim, os objetos gráficos.

O módulo exporta também os tipos que descrevem o instantâneo e os adaptadores: `ChartStateLayoutSnapshot`, `ChartStateRestoreResult`, `ChartStatePersistenceOptions`, `PersistedPane`, `PersistedPriceScale`, `PersistedSeries`, `PersistedIndicator`, `PersistedDrawing`, `PersistedChartOptions`, `PersistedSeriesOptions`, `PersistedIndicatorParameters`, `PersistedIndicatorStyles`, `PersistedObject`, `PersistedJsonValue`, `PersistableIndicatorEntry`, `RawChartState`, `ChartStateMigration`, `MaybePromise`.

## Veja também

- [Gráficos em JavaScript](../charts.md)
- [Indicadores](indicators.md)
- [Preenchimento de histórico](backfill.md)
