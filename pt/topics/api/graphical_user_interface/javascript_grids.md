# Tabelas JavaScript

[StockSharp JS Grids](https://github.com/StockSharp/JS-Grids) é um conjunto de componentes para navegador destinados à apresentação de dados tabulares. O pacote está publicado no npm como [@stocksharp/grids](https://www.npmjs.com/package/@stocksharp/grids), não tem dependências externas em tempo de execução e pode ser utilizado tanto com TypeScript como diretamente no navegador.

![Registo de negociação StockSharp com filtros, linhas selecionadas e totais fixos](../../../images/javascript_grids_blotter.jpg)

Pode experimentar a versão funcional na [demonstração online](https://stocksharp.github.io/JS-Grids/demo/): os cabeçalhos ordenam as linhas, as colunas podem ser arrastadas e ocultadas, os filtros e o agrupamento alteram a apresentação e a exportação cria um verdadeiro ficheiro `.xlsx`.

## Conteúdo do pacote

- [DataGrid](javascript_grids/data_grid.md) constrói o cabeçalho e as linhas a partir de uma descrição unificada das colunas. É responsável pela ordenação, filtros, agrupamento, seleção de linhas, totais fixos, menu de contexto, persistência do estado e exportação.
- [ColumnSettings](javascript_grids/column_settings.md) liga-se a uma tabela HTML já apresentada pelo servidor e permite ao utilizador alterar a ordem e a visibilidade das colunas.
- [TableSort](javascript_grids/table_sort.md) fornece um controlador de ordenação autónomo para uma tabela gerida pela aplicação.
- [TableExport](javascript_grids/table_export.md) cria um livro OOXML `.xlsx` sem bibliotecas externas.

`DataGrid` e `ColumnSettings` resolvem problemas diferentes. O primeiro cria o conteúdo de `<thead>` e `<tbody>` a partir da declaração das colunas. O segundo não cria a tabela e destina-se apenas a marcação existente, gerada pelo servidor, com atributos `data-col`.

## Instalação

Instale o pacote a partir do npm:

```bash
npm install @stocksharp/grids
```

Todos os componentes principais estão disponíveis no ponto de entrada comum:

```ts
import {
  DataGrid,
  ColumnSettings,
  TableSort,
  TableExport,
} from '@stocksharp/grids';
```

Para reduzir o código importado, existem pontos de entrada separados:

```ts
import { DataGrid } from '@stocksharp/grids/data-grid';
import { ColumnSettings } from '@stocksharp/grids/column-settings';
import { TableSort } from '@stocksharp/grids/table-sort';
import { TableExport } from '@stocksharp/grids/table-export';
```

Sem um empacotador, ligue o pacote pronto para navegador. Os respetivos objetos públicos encontram-se em `window.SSGrid`:

```html
<script src="https://cdn.jsdelivr.net/npm/@stocksharp/grids@1.1.0/dist/ssgrid.js"></script>
<script>
  const { DataGrid, TableExport } = window.SSGrid;
</script>
```

## Exemplo rápido

Prepare uma tabela comum com secções de cabeçalho e dados:

```html
<table id="orders">
  <thead></thead>
  <tbody></tbody>
</table>
```

Descreva as colunas uma vez e forneça as linhas à tabela:

```ts
import { DataGrid, SortDirections } from '@stocksharp/grids';

interface Order {
  id: number;
  symbol: string;
  price: number;
}

const table = document.querySelector<HTMLTableElement>('#orders')!;
const grid = new DataGrid<Order>({
  head: table.tHead!,
  body: table.tBodies[0],
  columns: [
    { key: 'id', header: 'N.º', value: order => order.id, filter: 'number', exportable: true },
    { key: 'symbol', header: 'Instrumento', value: order => order.symbol, filter: 'text', exportable: true },
    { key: 'price', header: 'Preço', value: order => order.price, filter: 'number', exportable: true },
  ],
  defaultSort: { col: 'id', dir: SortDirections.Desc },
  rowKey: order => String(order.id),
  emptyText: 'Sem ordens',
  locale: 'pt-PT',
});

grid.setRows([
  { id: 101, symbol: 'SBER', price: 312.45 },
  { id: 102, symbol: 'GAZP', price: 164.18 },
]);
```

A biblioteca cria elementos DOM, mas não impõe um estilo. As cores, dimensões, realce das linhas, menu, caixa de diálogo do filtro e classes auxiliares são definidos pela folha de estilos da aplicação.

## Compilação a partir do código-fonte

O repositório e a demonstração local utilizam os comandos npm habituais:

```bash
git clone https://github.com/StockSharp/JS-Grids.git
cd JS-Grids
npm ci
npm test
npm run build
npm run serve
```

Depois do arranque, a demonstração fica disponível em `http://localhost:8793/demo/`.

## Consulte também

- [Repositório JS-Grids](https://github.com/StockSharp/JS-Grids)
- [Pacote @stocksharp/grids](https://www.npmjs.com/package/@stocksharp/grids)
- [Demonstração online](https://stocksharp.github.io/JS-Grids/demo/)
- [Controlos de negociação JavaScript](javascript_trading_controls.md)
- [Gráficos JavaScript](charts/javascript_charts.md)
- [Diagrama JavaScript](javascript_diagram.md)
