# ColumnSettings

`ColumnSettings` añade selección, reordenación y ocultación de columnas a una tabla HTML que ya ha generado el servidor u otro componente. A diferencia de [DataGrid](data_grid.md), el adaptador no construye el encabezado ni las filas y no administra los datos de la tabla.

## Requisitos del marcado

La tabla debe contener un elemento `<thead>` real. Las columnas administradas reciben atributos `data-col` únicos:

```html
<table id="operaciones">
  <thead>
    <tr>
      <th data-col="time">Hora</th>
      <th data-col="symbol">Instrumento</th>
      <th data-col="price">Precio</th>
      <th data-col="volume">Volumen</th>
      <th>Acciones</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>10:15:02</td>
      <td>SBER</td>
      <td>312,45</td>
      <td>10</td>
      <td><button type="button">Abrir</button></td>
    </tr>
  </tbody>
</table>
```

Una columna sin `data-col` se considera fija: el usuario no puede ocultarla ni moverla de su posición original. Al crear el adaptador, este marca las celdas correspondientes del cuerpo con las mismas claves. Las filas con un número diferente de celdas, por ejemplo una fila «No hay datos» con `colspan`, permanecen sin cambios.

## Conexión

La aplicación anfitriona proporciona tres elementos:

- una tabla con marcado generado en el servidor;
- un cuadro de diálogo al que el componente añade interruptores y botones de desplazamiento;
- un almacén con los métodos `read()` y `write()`.

```ts
import { ColumnSettings } from '@stocksharp/grids/column-settings';

const dialogElement = document.querySelector<HTMLElement>('#column-dialog')!;
const list = dialogElement.querySelector<HTMLElement>('#column-list')!;

const settings = new ColumnSettings({
  table: document.querySelector<HTMLTableElement>('#operaciones')!,
  dialog: {
    list,
    moveUpTitle: 'Mover hacia arriba',
    moveDownTitle: 'Mover hacia abajo',
    classes: {
      item: 'column-picker-item',
      toggle: 'column-picker-toggle',
      label: 'column-picker-label',
      move: 'column-picker-move',
      moveUpIcon: 'icon-arrow-up',
      moveDownIcon: 'icon-arrow-down',
    },
    open: () => { dialogElement.hidden = false; },
    close: () => { dialogElement.hidden = true; },
  },
  store: {
    read: () => {
      const value = localStorage.getItem('columnas-operaciones');
      return value ? JSON.parse(value) : null;
    },
    write: visible => {
      if (visible === null)
        localStorage.removeItem('columnas-operaciones');
      else
        localStorage.setItem('columnas-operaciones', JSON.stringify(visible));
    },
  },
});

document.querySelector('#open-columns')!
  .addEventListener('click', () => settings.openPicker());

document.querySelector('#apply-columns')!
  .addEventListener('click', () => settings.applyPicked());

document.querySelector('#reset-columns')!
  .addEventListener('click', () => settings.resetToDefault());
```

El componente solo rellena el elemento `list`. El título, los botones de confirmación y restablecimiento, la animación y la apertura y el cierre de la ventana modal pertenecen a la aplicación; por tanto, los controladores `applyPicked()` y `resetToDefault()` también deben asignarse en ella.

## Almacenar la disposición

`ColumnLayoutStore.read()` devuelve un conjunto de claves visibles en el orden requerido o `null` si se utiliza la disposición inicial. El constructor lee el valor inmediatamente y lo aplica antes de la primera interacción del usuario.

`write(visibleKeys)` recibe únicamente las columnas administradas que están visibles. El valor `null` indica que se ha seleccionado el orden original y que ninguna columna está oculta. Así, un almacén en la URL o en `localStorage` puede eliminar una entrada innecesaria en lugar de guardar todo el valor predeterminado.

Las claves del adaptador se normalizan a minúsculas. Las claves desconocidas y repetidas se descartan al aplicar la disposición.

## Métodos

- `defaultKeys()` devuelve el orden original de las columnas administradas;
- `apply(visibleKeys)` reordena y oculta inmediatamente las columnas, pero no guarda la disposición;
- `isDefault(visibleKeys)` comprueba si la disposición coincide con la original;
- `openPicker()` lee el DOM actual, construye la lista y abre el cuadro de diálogo;
- `applyPicked()` aplica la selección actual, la guarda y cierra el cuadro de diálogo;
- `resetToDefault()` restaura todas las columnas administradas, llama a `write(null)` y cierra el cuadro de diálogo.

## Diseño

`ColumnSettings` no incluye CSS y no depende de ninguna biblioteca de ventanas modales ni de ningún conjunto de iconos concreto. Mediante `ColumnPickerClasses`, la aplicación especifica las clases de la fila, la casilla, la etiqueta, los botones y los dos iconos. `moveUpTitle` y `moveDownTitle` deben localizarse antes de pasarlos al componente.

## Véase también

- [Cuadrículas JavaScript](../javascript_grids.md)
- [DataGrid](data_grid.md)
- [Repositorio de JS-Grids](https://github.com/StockSharp/JS-Grids)
