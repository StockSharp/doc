# Estratégias

`StrategiesWidget` apresenta a lista das estratégias em execução: uma linha por estratégia com o seu estado, modo de negociação, posição, contadores de ordens e negócios, lucro e botões de comando. O identificador do controlo é `strategies` (`ControlTypes.Strategies`), também disponível através da propriedade estática `StrategiesWidget.TYPE`.

![Lista de estratégias com estado, posição, PnL e curva de rentabilidade](../../../../images/javascript_controls_strategies.png)

## Criação e atualização

```ts
import {
  StrategiesWidget,
  StrategyStates,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

let strategies!: StrategiesWidget;

strategies = StrategiesWidget.create(
  document.querySelector<HTMLElement>('#strategies')!,
  {},
  {
    host,
    tradingModes: ['Disabled', 'CancelOrders', 'ReducePosition', 'Full'],
    start: id => console.log('iniciar', id),
    stop: id => console.log('parar', id),
    closePosition: id => console.log('fechar', id),
    openStrategy: id => console.log('abrir', id),
    riskRules: id => console.log('risco', id),
    setTradingMode: (id, mode) => console.log('modo', id, mode),
  },
);

strategies.update([{
  id: 'sma-1',
  name: 'SMA crossover',
  state: StrategyStates.Started,
  online: true,
  tradingMode: 'Full',
  portfolio: 'Demo',
  security: 'BTC@IMEX',
  position: 0.25,
  ordersCount: 12,
  tradesCount: 8,
  pnlChange: 105,
  realized: 20,
  unrealized: 85,
  pnl: [
    { time: 1, value: 0 },
    { time: 2, value: 60 },
    { time: 3, value: 105 },
  ],
}]);
```

`update` recebe o conjunto completo de linhas e substitui a tabela por ele: uma estratégia que não conste da lista fornecida é dada como eliminada e a sua linha desaparece. O controlo não tem atualização em fluxo de uma única linha — o novo estado chega como lista inteira.

O segundo argumento de `create` é o estado guardado da instância. O controlo não o lê nem guarda seja o que for: não escreve chaves em `host.preferences` nem chama `host.persistState`.

O objeto `StrategyStates` exporta os estados `Stopped`, `Starting`, `Started` e `Stopping`.

## Dependências

Só o anfitrião é obrigatório. A integridade do anfitrião é verificada na criação pela função `assertHost`, pelo que um `TradingHost` incompleto origina uma exceção com o nome do membro em falta e não um botão que não funciona.

| Dependência | Obrigatoriedade | Comportamento predefinido |
|---|---|---|
| `host` | obrigatória | — |
| `start(id)` | opcional | O botão de arranque não é criado. |
| `stop(id)` | opcional | O botão de paragem não é criado. |
| `closePosition(id)` | opcional | Na coluna da posição fica apenas o número. |
| `openStrategy(id)` | opcional | O botão de abertura da estratégia não é criado. |
| `riskRules(id)` | opcional | O botão das regras de risco não é criado. |
| `setTradingMode(id, mode)` | opcional | O modo de negociação é apresentado como texto. |
| `tradingModes` | opcional | Lista vazia; a lista pendente de modos não é criada. |

Este conjunto permite montar um painel apenas de leitura: se não for fornecida nenhuma função de ação, a tabela mostra os dados e não apresenta um único botão.

As cadeias de `tradingModes` passam por `host.t`, ou seja, servem de chaves de tradução. Pertencem ao anfitrião e não ao pacote, pelo que a sua ausência em `translation-keys.json` é normal e cabe ao anfitrião traduzi-las.

## Estados e ações

A célula de estado é composta por um ponto e uma palavra: o ponto lê-se numa vista rápida da lista, a palavra distingue `Starting` de `Started`. Se a linha tiver o campo `error` preenchido, o texto do erro entra na dica tanto do ponto como da palavra, e uma estratégia parada por falha é assinalada com a palavra «Erro» e não «Parada».

Os botões da linha são criados apenas para as funções fornecidas pelo anfitrião e só ficam ativos onde o estado o permite:

- arrancar — apenas numa estratégia no estado `Stopped`;
- parar — apenas numa estratégia no estado `Started`;
- fechar a posição — apenas numa estratégia em funcionamento com posição diferente de zero;
- regras de risco e abertura da estratégia — sempre.

A lista pendente do modo de negociação só está ativa numa estratégia parada: o modo determina com que definições a estratégia será arrancada e não serve de alavanca durante a negociação. A alteração do modo chama `setTradingMode`; o controlo não altera o valor na linha por si e espera pelo `update` seguinte.

## Colunas e apresentação

A tabela apresenta o estado, as ações, o indicador de ligação, o modo de negociação, o nome, a carteira, o instrumento, a posição, o número de ordens e de negócios, a variação do lucro, o gráfico do lucro, o lucro realizado e não realizado e o erro. O indicador de ligação é global: a estratégia só é considerada em linha se estiver formada e ligada, e quem o decide é o fornecedor de dados.

As classes de cor da posição, da variação do lucro e das duas grandezas de lucro são devolvidas por `host.presentation.pnlClass`. A variação do lucro é ainda assinalada com uma seta de direção; quando a variação é nula não há seta.

A coluna do gráfico desenha a curva de lucro acumulado a partir dos pontos `pnl` num campo de 140 × 26 píxeis CSS. O `canvas` é criado tendo em conta o `devicePixelRatio`, pelo que a linha se mantém nítida em ecrãs de alta densidade. As cores vêm de `host.presentation.canvasPalette()` e a curva é colorida pelo resultado da execução: uma estratégia que chegou ao pico e devolveu tudo é apresentada como deficitária. Sem pontos `pnl` a célula fica vazia.

A ordenação predefinida é por nome, ascendente: a lista lê-se de cima para baixo à procura de uma estratégia concreta, e trocar as linhas de sítio ao sabor do lucro atrapalha essa leitura. O painel também permite selecionar várias linhas, abrir o menu de contexto, filtrar e exportar para XLSX.

## O que fica a cargo do anfitrião

O controlo não arranca nem para estratégias, não envia ordens e não fecha posições — invoca as funções fornecidas e espera por uma nova lista de linhas.

O valor de `pnlChange` é aceite tal como vem: o ponto de referência é escolhido pelo fornecedor de dados. Assim, uma estratégia reiniciada não continua a contar a variação desde antes do reinício.

O botão de fecho do painel invoca `host.close()` e a exportação grava o ficheiro `strategies`. A instância regista-se no anfitrião ao ser criada e cancela o registo em `dispose`.

## Métodos públicos

- `StrategiesWidget.create(hostEl, state, deps)` — construir o painel e adicioná-lo ao contentor.
- `update(rows)` — substituir toda a lista de estratégias.
- `dispose()` — cancelar o registo, remover a tabela e libertar os recursos.

## Consulte também

- [Controlos de negociação JavaScript](../trading_controls.md)
- [Posições](positions.md)
- [Ordens ativas](active_orders.md)
- [Histórico de negócios](trade_history.md)
