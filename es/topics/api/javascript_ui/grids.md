# Cuadrículas JavaScript

[StockSharp JS Grids](https://github.com/StockSharp/JS-Grids) es un conjunto de componentes para mostrar datos tabulares en el navegador. El paquete se publica en npm como [@stocksharp/grids](https://www.npmjs.com/package/@stocksharp/grids), no tiene dependencias de ejecución de terceros y puede utilizarse tanto con TypeScript como directamente en el navegador.

![Diario de negociación de StockSharp con filtros, filas seleccionadas y totales fijados](../../../images/javascript_grids_blotter.jpg)

Puede probar la versión lista para usar en la [demostración en línea](https://stocksharp.github.io/JS-Grids/demo/): los encabezados ordenan las filas, las columnas se arrastran y se ocultan, los filtros y la agrupación modifican la vista, y la exportación crea un archivo `.xlsx` real.

## Contenido del paquete

- [DataGrid](grids/data_grid.md) construye el encabezado y las filas a partir de una única descripción de columnas. Gestiona la ordenación, los filtros, la agrupación, la selección de filas, los totales fijados, el menú contextual, la persistencia del estado y la exportación.
- [ColumnSettings](grids/column_settings.md) se conecta a una tabla HTML ya renderizada por el servidor y permite al usuario cambiar el orden y la visibilidad de las columnas.
- [TableSort](grids/table_sort.md) proporciona un controlador de ordenación independiente para una tabla administrada por la aplicación.
- [TableExport](grids/table_export.md) genera un libro OOXML `.xlsx` sin bibliotecas de terceros.

`DataGrid` y `ColumnSettings` resuelven tareas diferentes. El primero crea por sí mismo el contenido de `<thead>` y `<tbody>` a partir de la declaración de columnas. El segundo no crea la tabla y está destinado exclusivamente al marcado de servidor existente con atributos `data-col`.

## Instalación

Instale el paquete desde npm:

```bash
npm install @stocksharp/grids
```

Todos los componentes principales están disponibles desde el punto de entrada común:

```ts
import {
  DataGrid,
  ColumnSettings,
  TableSort,
  TableExport,
} from '@stocksharp/grids';
```

Para reducir el código importado, se proporcionan puntos de entrada individuales:

```ts
import { DataGrid } from '@stocksharp/grids/data-grid';
import { ColumnSettings } from '@stocksharp/grids/column-settings';
import { TableSort } from '@stocksharp/grids/table-sort';
import { TableExport } from '@stocksharp/grids/table-export';
```

Si no utiliza un empaquetador, incluya el paquete precompilado para navegador. Sus objetos públicos se encuentran en `window.SSGrid`:

```html
<script src="https://cdn.jsdelivr.net/npm/@stocksharp/grids@1.1.0/dist/ssgrid.js"></script>
<script>
  const { DataGrid, TableExport } = window.SSGrid;
</script>
```

## Ejemplo rápido

Prepare una tabla normal con secciones de encabezado y datos:

```html
<table id="orders">
  <thead></thead>
  <tbody></tbody>
</table>
```

Describa las columnas una sola vez y pase las filas a la cuadrícula:

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
    { key: 'price', header: 'Precio', value: order => order.price, filter: 'number', exportable: true },
  ],
  defaultSort: { col: 'id', dir: SortDirections.Desc },
  rowKey: order => String(order.id),
  emptyText: 'No hay órdenes',
  locale: 'es-ES',
});

grid.setRows([
  { id: 101, symbol: 'SBER', price: 312.45 },
  { id: 102, symbol: 'GAZP', price: 164.18 },
]);
```

La biblioteca crea los elementos DOM, pero no impone ningún diseño. La hoja de estilos de la aplicación define los colores, las dimensiones, el resaltado de filas, el menú, el cuadro de diálogo de filtro y las clases auxiliares.

## Compilar desde el código fuente

El repositorio y la demostración local utilizan los comandos npm estándar:

```bash
git clone https://github.com/StockSharp/JS-Grids.git
cd JS-Grids
npm ci
npm test
npm run build
npm run serve
```

Después de iniciar el servidor, la demostración estará disponible en `http://localhost:8793/demo/`.

## Véase también

- [Repositorio de JS-Grids](https://github.com/StockSharp/JS-Grids)
- [Paquete @stocksharp/grids](https://www.npmjs.com/package/@stocksharp/grids)
- [Demostración en línea](https://stocksharp.github.io/JS-Grids/demo/)
- [Controles de negociación JavaScript](trading_controls.md)
- [Gráficos en JavaScript](charts.md)
- [Diagrama en JavaScript](diagram.md)
