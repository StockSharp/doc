# Estatísticas

`StatisticsWidget` é uma tabela com os parâmetros estatísticos da estratégia: lucro, drawdown, número de negócios, latências. O painel é apenas de leitura: uma linha por parâmetro, com as linhas agrupadas pela área a que o parâmetro pertence.

![Painel de estatísticas da execução com os indicadores agrupados](../../../../images/javascript_controls_statistics.png)

## Criação e atualização

```ts
import {
  StatisticsWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const statistics = StatisticsWidget.create(
  document.querySelector<HTMLElement>('#statistics')!,
  {},
  { host },
);

statistics.update([
  {
    key: 'NetProfit',
    category: 'pnl',
    categoryText: 'Lucro',
    order: 1,
    name: 'Lucro líquido',
    description: 'Resultado da execução',
    value: 11_055.75,
  },
  {
    key: 'MaxProfitDate',
    category: 'pnl',
    categoryText: 'Lucro',
    order: 2,
    name: 'Data do máximo',
    value: '2024-03-26T07:30:00Z',
  },
  {
    key: 'NetProfit',
    category: 'pnl',
    categoryText: 'Negócios',
    order: 100,
    name: 'Número de negócios',
    value: 1_340,
  },
]);
```

O conjunto de dependências `StatisticsDeps` é composto por um único campo obrigatório, `host`. O controlo não tem processadores de ações: as estatísticas são produzidas pela estratégia e no painel não há nada para cancelar, recarregar ou editar. O segundo argumento de `create` é o estado guardado do painel; o controlo não o utiliza.

`update` substitui todo o conjunto de linhas. A estratégia publica os seus parâmetros como uma única tabela, pelo que uma linha que desapareceu do conjunto é considerada extinta e não apenas parada. A identidade da linha é determinada pelo campo `key`.

## Linhas e ordem

A linha é descrita pelo tipo `StatisticRow`:

| Campo | Finalidade |
|---|---|
| `key` | Identificador estável do parâmetro. |
| `category` | Chave do grupo, independente do idioma. |
| `categoryText` | Título do grupo para apresentação. Na sua ausência é utilizado `category`. |
| `order` | Posição do parâmetro no registo. |
| `name` | Nome localizado do parâmetro. |
| `description` | Explicação localizada, apresentada como dica sobre a célula do nome. |
| `value` | Número, data ou cadeia de texto. `null` — o parâmetro ainda não foi medido. |

A versão para ambiente de trabalho desta tabela obtém as linhas por reflexão sobre os parâmetros da estratégia; no navegador não existe tal mecanismo, pelo que as linhas chegam já prontas do anfitrião — com o nome e a descrição traduzidos.

O agrupamento é feito por `category` e não por `categoryText`: agrupar pelo título traduzido reconstruiria a tabela a cada mudança de idioma. A ordem dos grupos é dada pelo menor `order` dos seus parâmetros, pelo que o lucro fica acima do drawdown e este acima dos contadores de negócios. Ordenar por nome ou por valor separaria parâmetros que se leem em conjunto.

Existem duas colunas visíveis: `Name` e `Value`. As colunas `category` e `order` estão declaradas, mas ocultas — servem para agrupar e ordenar e nada dizem ao leitor. Na exportação entram apenas as duas colunas visíveis.

## Formatação dos valores

O texto da célula de valor é produzido pela função exportada `formatStatistic(value)`:

```ts
import { formatStatistic } from '@stocksharp/trading-controls';

formatStatistic(11_055.756); // '11055.76'
formatStatistic(1_340);      // '1340'
formatStatistic('2024-03-26T07:30:00Z'); // '2024-03-26'
formatStatistic(null);       // ''
```

O número é arredondado a duas casas decimais e apresentado sem zeros à direita. A data mostra apenas o dia: parâmetros deste tipo descrevem a execução inteira, e a hora seria ruído. Uma cadeia de texto só é reconhecida como data quando começa por `AAAA-MM-DD`; caso contrário permanece texto. Um valor ausente dá uma célula vazia e não um zero, que se leria como um resultado medido.

A ordenação é feita pelo valor original, pelo que um número é ordenado como número e não como texto.

## O que faz o controlo e o que faz o anfitrião

O controlo obtém do anfitrião todo o texto visível através de `host.t`, incluindo os títulos das colunas, o título do painel, a mensagem de tabela vazia e as entradas do menu de contexto da tabela. O botão de fecho invoca `host.close` e a exportação grava a tabela em XLSX. Ao ser criada, a instância regista-se com `host.register` e, em `dispose`, é removida com `host.unregister`.

O controlo não guarda definições próprias em `host.preferences`. Também não pede dados: as linhas são fornecidas pelo anfitrião através de `update`.

O identificador do tipo de painel está disponível como `StatisticsWidget.TYPE` e é igual a `ControlTypes.Statistics`.

## Métodos públicos

- `StatisticsWidget.create(hostEl, state, deps)` — criar o painel no contentor indicado.
- `update(rows)` — substituir todo o conjunto de linhas estatísticas.
- `dispose()` — libertar os recursos.

O painel também permite ordenar, selecionar várias linhas, abrir o menu de contexto e exportar para XLSX.

## Consulte também

- [Controlos de negociação JavaScript](../trading_controls.md)
- [Posições](positions.md)
- [Histórico de negócios](trade_history.md)
- [Ordens ativas](active_orders.md)
