# TableExport

`TableExport` crea en el navegador un libro OOXML `.xlsx` real e inicia inmediatamente su descarga. La implementación no utiliza bibliotecas de terceros ni disfraza un archivo CSV con una extensión de Excel.

## Exportación directa

Proporcione el nombre base del archivo, el nombre de la hoja, los encabezados y un conjunto bidimensional de filas:

```ts
import { TableExport } from '@stocksharp/grids/table-export';

TableExport.download(
  'portfolio-summary',
  'Resumen de cartera',
  ['Indicador', 'Valor'],
  [
    ['Efectivo', 125000.50],
    ['Posiciones abiertas', 7],
    ['Beneficio no realizado', 4380.25],
  ],
);
```

El navegador descargará un archivo con una marca de tiempo en el formato `<baseName>-YYYYMMDD-HHMMSS.xlsx`, por ejemplo, `portfolio-summary-20260810-143025.xlsx`.

Los números finitos se escriben como celdas numéricas y los demás valores no vacíos como cadenas incorporadas. `null`, `undefined` y una cadena vacía crean celdas vacías. Proporcione las filas en el orden requerido: `TableExport` no ordena ni filtra los datos.

El nombre de la hoja elimina automáticamente los caracteres no permitidos por Excel `[]:*?/\`, se limita a 31 caracteres y se sustituye por `Sheet1` si no queda nada después de la limpieza.

## Exportar desde DataGrid

[DataGrid](data_grid.md) prepara los datos a partir de las declaraciones de columnas e invoca el mismo mecanismo:

```ts
const data = grid.exportData();
console.log(data.headers, data.rows);

grid.download('orders', 'Órdenes');
```

El archivo solo contiene las columnas visibles con `exportable: true`. De forma predeterminada se utiliza el resultado de `value(row)` y, si existe `exportValue(row)`, se utiliza el resultado de esta función. Esto permite, por ejemplo, mostrar en la celda un elemento DOM con formato, ordenar por un código numérico y exportar un texto localizado.

Las filas se exportan después de aplicar el filtrado, la ordenación y la agrupación, en el orden de la vista. Los encabezados de grupo no se añaden a la hoja, pero se conservan las filas de los grupos contraídos. `renderLimit` no recorta la exportación, y las filas fijadas de `pinnedRows()` no se incluyen en el libro.

`exportData()` no descarga nada, por lo que resulta útil para obtener una vista previa y comprobar el contenido. `download()` crea el archivo en el cliente, añade temporalmente al documento un enlace con un `Blob` y libera la URL del objeto después de iniciar la descarga.

## Limitaciones

El componente genera un libro mínimo con una sola hoja. No admite fórmulas, estilos de celda, anchos de columna, varias hojas ni compresión ZIP. Si la aplicación necesita estas funciones, prepare la exportación mediante una herramienta especializada independiente.

## Véase también

- [Cuadrículas JavaScript](../grids.md)
- [DataGrid](data_grid.md)
- [TableSort](table_sort.md)
- [Paquete @stocksharp/grids](https://www.npmjs.com/package/@stocksharp/grids)
