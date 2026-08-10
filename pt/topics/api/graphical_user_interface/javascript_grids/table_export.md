# TableExport

`TableExport` cria no navegador um verdadeiro livro OOXML `.xlsx` e inicia imediatamente a sua transferência. A implementação não utiliza bibliotecas externas nem disfarça um ficheiro CSV com a extensão do Excel.

## Exportação direta

Forneça o nome base do ficheiro, o nome da folha, os cabeçalhos e uma matriz bidimensional de linhas:

```ts
import { TableExport } from '@stocksharp/grids/table-export';

TableExport.download(
  'portfolio-summary',
  'Resumo da carteira',
  ['Indicador', 'Valor'],
  [
    ['Disponibilidades', 125000.50],
    ['Posições abertas', 7],
    ['Lucro não realizado', 4380.25],
  ],
);
```

O navegador transfere um ficheiro com uma marca temporal no formato `<baseName>-YYYYMMDD-HHMMSS.xlsx`, por exemplo `portfolio-summary-20260810-143025.xlsx`.

Os números finitos são gravados como células numéricas e os restantes valores não vazios como cadeias incorporadas. `null`, `undefined` e uma cadeia vazia criam células vazias. Forneça as linhas na ordem pretendida: `TableExport` não ordena nem filtra os dados.

O nome da folha é automaticamente limpo dos caracteres proibidos pelo Excel `[]:*?/\`, limitado a 31 caracteres e substituído por `Sheet1` se nada restar depois da limpeza.

## Exportação a partir de DataGrid

[DataGrid](data_grid.md) prepara os dados a partir das declarações das colunas e chama o mesmo mecanismo:

```ts
const data = grid.exportData();
console.log(data.headers, data.rows);

grid.download('orders', 'Ordens');
```

O ficheiro inclui apenas as colunas visíveis com `exportable: true`. Por predefinição é utilizado o resultado de `value(row)` e, quando existe `exportValue(row)`, o resultado desta função. Isso permite, por exemplo, apresentar na célula um elemento DOM formatado, ordenar por um código numérico e exportar texto localizado.

As linhas são exportadas após a filtragem, a ordenação e o agrupamento, na ordem de apresentação. Os cabeçalhos dos grupos não são adicionados à folha, mas as linhas dos grupos recolhidos são preservadas. `renderLimit` não limita a exportação e as linhas fixas de `pinnedRows()` não são incluídas no livro.

`exportData()` não inicia qualquer transferência, pelo que é útil para pré-visualizar e testar o conteúdo. `download()` cria o ficheiro no cliente, adiciona temporariamente ao documento uma ligação com um `Blob` e liberta o URL do objeto depois de iniciar a transferência.

## Limitações

O componente cria um livro mínimo com uma folha. Não define fórmulas, estilos de células, larguras de colunas, várias folhas nem compressão ZIP. Se a aplicação necessitar destas funcionalidades, prepare a exportação com uma ferramenta especializada separada.

## Consulte também

- [Tabelas JavaScript](../javascript_grids.md)
- [DataGrid](data_grid.md)
- [TableSort](table_sort.md)
- [Pacote @stocksharp/grids](https://www.npmjs.com/package/@stocksharp/grids)
