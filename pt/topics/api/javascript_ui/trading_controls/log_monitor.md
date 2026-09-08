# Registo

`LogMonitorWidget` apresenta o registo de funcionamento: à esquerda a árvore de fontes, à direita a tabela de mensagens da fonte selecionada e de toda a sua subárvore. Cada mensagem é guardada uma única vez e transporta o identificador da fonte que a escreveu; o número de linhas guardadas é limitado.

![Registo com a árvore de fontes, o filtro de níveis e a tabela de mensagens](../../../../images/javascript_controls_log_monitor.png)

## Criação e atualização

```ts
import {
  LogLevels,
  LogMonitorWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const log = LogMonitorWidget.create(
  document.querySelector<HTMLElement>('#log')!,
  {},
  {
    host,
    maxMessages: 20_000,
  },
);

log.setSources([
  { id: 'connector', name: 'Connector' },
  { id: 'strategy-1', name: 'SMA', parentId: 'connector' },
]);

log.append([{
  id: 1,
  time: Date.now(),
  level: LogLevels.Warning,
  sourceId: 'strategy-1',
  message: 'Ordem rejeitada: fundos insuficientes',
}]);

log.select('connector');
```

Das dependências, só `host` é obrigatória: o registo escreve-se, não se age sobre ele, pelo que o controlo não necessita de processadores. As restantes são opcionais:

| Dependência | Predefinição | Finalidade |
|---|---|---|
| `maxMessages` | `5000` | Quantas mensagens guardar. As excedentes são descartadas a partir do início da lista. |
| `chrome` | `true` | Se deve desenhar o seu próprio cabeçalho com o botão de fecho. Um anfitrião que já titula e fecha o painel por si (por exemplo, através de um separador de doca) passa `false`. |
| `sources` | `true` | Se deve mostrar a árvore de fontes ao ser criado. Este é apenas o estado inicial: a árvore volta a partir do menu de contexto da tabela ou por uma chamada a `showSources`. |

`setSources` passa a lista de fontes na íntegra: uma fonte que desapareceu sai da árvore e a seleção volta a «todas as fontes». `append` acrescenta o que acabou de ser registado, `clear` esquece todas as mensagens e mantém a árvore de fontes no lugar.

A propriedade estática `LogMonitorWidget.TYPE` contém o identificador do controlo, `logMonitor`.

## Fontes e filtros

Uma fonte declara o seu progenitor (`parentId`) e não os seus descendentes, e pode surgir antes do progenitor. A árvore é montada com aquilo que já chegou: uma fonte com progenitor desconhecido passa a raiz e um ciclo é quebrado no primeiro nó. As linhas da árvore são planas e o aninhamento é indicado por avanço; a linha de topo seleciona todas as fontes de uma vez.

Atuam em simultâneo três filtros: o conjunto de níveis ativos, o texto de pesquisa sobre o corpo da mensagem (sem distinção entre maiúsculas e minúsculas) e a subárvore de fontes selecionada. Os níveis são alternados pelos botões da barra de ferramentas — `error`, `warning`, `info`, `debug`, `verbose` do objeto `LogLevels`; numa coluna estreita o nível é indicado pelas letras `E`, `W`, `I`, `D`, `V`. O método `visible` devolve o que restou depois de todos os filtros.

A tabela é composta pelas colunas de fonte, hora, nível e mensagem, está ordenada por hora ascendente e permite seleção múltipla, ordenação, ocultação de colunas, filtros e menu de contexto. Ao menu foi acrescentada a entrada que mostra a árvore de fontes. Um botão da barra de ferramentas exporta as linhas visíveis para XLSX; na exportação entram a hora formatada por `host.presentation.timeText` e o nome completo do nível, não a letra.

## O que faz o anfitrião

O controlo obtém do anfitrião as traduções (`host.t`), o formato da hora (`host.presentation.timeText`) e o tratamento do fecho do painel (`host.close`), além de se registar com `host.register` e de cancelar o registo em `dispose`. Não guarda definições próprias em `host.preferences` nem preserva o estado da instância: o segundo argumento de `create` é aceite por uniformidade, mas não é lido. A recolha das mensagens, a sua entrega e a reposição da posição do painel cabem ao anfitrião.

## Métodos públicos

- `setSources(sources)` — substituir a lista de fontes.
- `append(messages)` — acrescentar mensagens respeitando o limite `maxMessages`.
- `clear()` — limpar as mensagens.
- `select(sourceId)` — mostrar a subárvore de uma fonte; `null` mostra todas.
- `visible()` — as mensagens que restam depois de todos os filtros.
- `sourcesShown()` — se a árvore de fontes está visível.
- `showSources(on)` — mostrar ou ocultar a árvore; o filtro de fonte selecionado é preservado.
- `dispose()` — libertar os recursos.

## Consulte também

- [Controlos de negociação JavaScript](../trading_controls.md)
- [Estratégias](strategies.md)
- [Estatísticas](statistics.md)
