# TableSort

`TableSort<TRow>` é um controlador de ordenação autónomo utilizado internamente por [DataGrid](data_grid.md), mas que também pode ser ligado a uma tabela da aplicação. Guarda a coluna e o sentido selecionados, processa cliques em cabeçalhos com `data-sort` e devolve uma cópia ordenada da matriz de linhas.

## Marcação

Associe cada cabeçalho ordenável a uma chave do dicionário de funções de leitura:

```html
<table id="cotacoes">
  <thead>
    <tr>
      <th data-sort="symbol">Instrumento</th>
      <th data-sort="bid">Compra</th>
      <th data-sort="ask">Venda</th>
    </tr>
  </thead>
  <tbody></tbody>
</table>
```

## Criação do controlador

```ts
import { SortDirections, TableSort } from '@stocksharp/grids/table-sort';

interface Cotacao {
  symbol: string;
  bid: number | null;
  ask: number | null;
}

const table = document.querySelector<HTMLTableElement>('#cotacoes')!;
let cotacoes: Cotacao[] = [];

const sort = new TableSort<Cotacao>(
  table.tHead,
  {
    symbol: cotacao => cotacao.symbol,
    bid: cotacao => cotacao.bid,
    ask: cotacao => cotacao.ask,
  },
  render,
  { col: 'symbol', dir: SortDirections.Asc },
  new Intl.Collator('pt-PT', { numeric: true, sensitivity: 'base' }),
);

function render(): void {
  const body = table.tBodies[0];
  body.replaceChildren();

  for (const cotacao of sort.apply(cotacoes)) {
    const row = body.insertRow();
    row.insertCell().textContent = cotacao.symbol;
    row.insertCell().textContent = cotacao.bid?.toString() ?? '—';
    row.insertCell().textContent = cotacao.ask?.toString() ?? '—';
  }
}
```

O construtor recebe:

1. o elemento do cabeçalho ou `null`, caso não seja necessário processar cliques;
2. um dicionário de funções de leitura dos valores por chave de coluna;
3. a função `onChange`, que volta a apresentar as linhas;
4. a ordenação predefinida ou `null` para a ordem original;
5. um `Intl.Collator` previamente criado para comparar texto.

Se não estiver definida uma função de leitura para a chave selecionada, o controlador tenta ler a propriedade homónima da linha.

## Comportamento da ordenação

Um clique num cabeçalho novo ativa a ordenação ascendente. O clique seguinte muda-a para descendente e o terceiro restaura a ordem predefinida. Não existe um estado sem ordenação separado quando a tabela recebe `defaultSort`.

`apply(rows)` devolve sempre uma nova matriz e não altera a matriz da aplicação. `null`, `undefined` e uma cadeia vazia são colocados no fim nos dois sentidos.

A comparação é escolhida **pelos valores e não pelo tipo declarado da coluna**: se ambos os valores se converterem num número finito, são comparados numericamente e só nos restantes casos entra em ação o `Intl.Collator` fornecido. Por isso a cadeia `"42"` fica entre 41 e 43, e não onde o alfabeto a colocaria; uma cadeia como `"1e3"` também conta como número. Se uma coluna tiver de ser ordenada como texto com quaisquer valores, devolva dela um valor que não se converta em número.

O controlador atribui ao cabeçalho ativo a classe `sort-asc` ou `sort-desc`; as setas e o restante estilo visual destas classes são definidos pela aplicação.

## Controlo por código

```ts
sort.set('bid', SortDirections.Desc);

const explicitSort = sort.current();
// { col: 'bid', dir: 'desc' }

sort.set(null, null);
// Voltar a defaultSort.
```

`current()` devolve apenas a seleção explícita do utilizador. Enquanto estiver em vigor a ordem predefinida, o resultado é `null`, mesmo que as linhas estejam efetivamente ordenadas.

Se a aplicação voltar a criar os elementos `<th>` dentro do mesmo cabeçalho, chame `refreshHeader()` para reaplicar as classes de sentido. O processador de cliques está associado ao próprio elemento de cabeçalho fornecido e continuará a funcionar com os novos elementos descendentes.

## Consulte também

- [Tabelas JavaScript](../grids.md)
- [DataGrid](data_grid.md)
- [TableExport](table_export.md)
