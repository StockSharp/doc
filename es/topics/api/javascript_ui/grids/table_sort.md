# TableSort

`TableSort<TRow>` es un controlador de ordenación independiente que se utiliza dentro de [DataGrid](data_grid.md), pero que también puede conectarse a una tabla de la aplicación. Almacena la columna y la dirección seleccionadas, procesa los clics en encabezados con `data-sort` y devuelve una copia ordenada del conjunto de filas.

## Marcado

Asocie cada encabezado ordenable con una clave del diccionario de funciones de lectura:

```html
<table id="cotizaciones">
  <thead>
    <tr>
      <th data-sort="symbol">Instrumento</th>
      <th data-sort="bid">Compra</th>
      <th data-sort="ask">Venta</th>
    </tr>
  </thead>
  <tbody></tbody>
</table>
```

## Crear el controlador

```ts
import { SortDirections, TableSort } from '@stocksharp/grids/table-sort';

interface Cotizacion {
  symbol: string;
  bid: number | null;
  ask: number | null;
}

const table = document.querySelector<HTMLTableElement>('#cotizaciones')!;
let cotizaciones: Cotizacion[] = [];

const sort = new TableSort<Cotizacion>(
  table.tHead,
  {
    symbol: cotizacion => cotizacion.symbol,
    bid: cotizacion => cotizacion.bid,
    ask: cotizacion => cotizacion.ask,
  },
  render,
  { col: 'symbol', dir: SortDirections.Asc },
  new Intl.Collator('es-ES', { numeric: true, sensitivity: 'base' }),
);

function render(): void {
  const body = table.tBodies[0];
  body.replaceChildren();

  for (const cotizacion of sort.apply(cotizaciones)) {
    const row = body.insertRow();
    row.insertCell().textContent = cotizacion.symbol;
    row.insertCell().textContent = cotizacion.bid?.toString() ?? '—';
    row.insertCell().textContent = cotizacion.ask?.toString() ?? '—';
  }
}
```

El constructor acepta:

1. el elemento de encabezado o `null` si no es necesario procesar los clics;
2. un diccionario de funciones que leen los valores según la clave de la columna;
3. la función `onChange`, que volverá a renderizar las filas;
4. la ordenación predeterminada o `null` para conservar el orden original;
5. un `Intl.Collator` creado previamente para comparar texto.

Si no se proporciona una función de lectura para la clave seleccionada, el controlador intenta leer la propiedad del mismo nombre en la fila.

## Comportamiento de la ordenación

Al hacer clic en un encabezado nuevo se activa la ordenación ascendente. El siguiente clic la cambia a descendente y el tercero restaura el orden predeterminado. No existe un estado sin ordenación independiente cuando la tabla recibe un `defaultSort`.

`apply(rows)` siempre devuelve un conjunto nuevo y no modifica el conjunto de la aplicación. `null`, `undefined` y las cadenas vacías se sitúan al final en ambas direcciones.

La comparación se elige **por los valores y no por el tipo declarado de la columna**: si ambos valores se convierten en un número finito, se comparan numéricamente, y solo en los demás casos entra en juego el `Intl.Collator` proporcionado. Por eso la cadena `"42"` se colocará entre 41 y 43, y no donde la pondría el alfabeto; una cadena como `"1e3"` también se considera un número. Si una columna debe ordenarse como texto sean cuales sean los valores, devuelva desde ella un valor que no llegue a ser un número.

El controlador asigna al encabezado activo la clase `sort-asc` o `sort-desc`; la aplicación define las flechas y el resto del diseño visual de estas clases.

## Control mediante código

```ts
sort.set('bid', SortDirections.Desc);

const explicitSort = sort.current();
// { col: 'bid', dir: 'desc' }

sort.set(null, null);
// Volver a defaultSort.
```

`current()` devuelve únicamente la selección explícita del usuario. Mientras esté activo el orden predeterminado, el resultado será `null`, aunque las filas estén ordenadas realmente.

Si la aplicación ha vuelto a crear los elementos `<th>` dentro del mismo encabezado, llame a `refreshHeader()` para asignar de nuevo las clases de dirección. El controlador de clics se asigna al propio elemento de encabezado proporcionado y seguirá funcionando con los nuevos elementos secundarios.

## Véase también

- [Cuadrículas JavaScript](../grids.md)
- [DataGrid](data_grid.md)
- [TableExport](table_export.md)
