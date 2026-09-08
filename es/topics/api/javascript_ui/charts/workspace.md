# Varios gráficos

`MultiChartWorkspace` dispone varios gráficos independientes en una cuadrícula dentro de un mismo contenedor, los enlaza por instrumento y marco temporal, y sincroniza el rango visible y el crosshair. La clase se distribuye en el punto de entrada `@stocksharp/chart/workspace` junto con el resto de controladores del espacio de trabajo: paneles, indicadores, plantillas, comparación de instrumentos y navegación por el historial.

El espacio de trabajo solo posee los gráficos de nivel superior. Los paneles de indicadores siguen siendo un asunto interno de su gráfico: no cuentan como celdas, no participan en la disposición y no se sincronizan.

## Creación y actualización

El contenedor y la fábrica de gráficos son obligatorios. La fábrica recibe `{ id, index, host }` y devuelve una celda: el propio gráfico, un controlador de datos opcional y una función de liberación opcional (de forma predeterminada se llama a `chart.remove()`):

```ts
import { CandlestickSeries, createChart } from '@stocksharp/chart';
import { ChartDataController, type IChartDataSource } from '@stocksharp/chart/data';
import { MultiChartWorkspace } from '@stocksharp/chart/workspace';

declare const dataSource: IChartDataSource;

const workspace = new MultiChartWorkspace({
  container: document.querySelector<HTMLElement>('#workspace')!,
  count: 4,
  columns: 2,                                   // null: cuadrícula automática, próxima a un cuadrado
  links: { symbol: true, resolution: false },   // instrumento común, marco temporal propio en cada celda
  sync: { range: true, crosshair: true },
  createChart: ({ host }) => {
    const chart = createChart(host, { timeScale: { timeVisible: true } });
    const series = chart.addSeries(CandlestickSeries, {
      upColor: '#26a69a',
      downColor: '#ef5350',
    });
    const data = new ChartDataController({ chart, series, dataSource, initialCount: 300 });

    return {
      chart,
      data,
      dispose: () => {
        data.dispose();
        chart.remove();
      },
    };
  },
});

const [first] = workspace.cells();
await workspace.setSelection(first.id, { symbol: 'BTC@IMEX', resolution: '1h' });

workspace.subscribe(snapshot => {
  console.log(snapshot.activeId, snapshot.columns, snapshot.rows, snapshot.errors.length);
});
```

`setCount` y `setColumns` cambian el tamaño de la cuadrícula por separado, y `setLayout({ count, columns })` lo hace en una sola operación. El contenedor se configura como cuadrícula CSS; los estilos originales se recuerdan y se restauran en `dispose`. Puede haber de 1 a 64 celdas; la última celda no se puede eliminar.

## Enlace y sincronización

`links` describe qué se traslada al resto de celdas al cambiar de instrumento: `symbol` y `resolution` se activan de forma independiente. Una celda cuya fábrica no devolvió `data` no participa en el enlace.

`sync` activa el traslado del rango visible (`range`) y de la posición del crosshair (`crosshair`). Tras aplicarlo, el rango se vuelve a leer del gráfico receptor: una celda con un historial más corto recorta la ventana solicitada, y en la instantánea se publica lo que realmente se muestra.

La celda activa se establece con `activate` y también de forma automática con `pointerdown` y `focusin` dentro de la celda. Cambiar `links` o `sync` reparte de inmediato al resto el estado actual de la celda activa.

Los errores de sincronización no interrumpen el trabajo de las demás celdas, sino que se acumulan en `snapshot.errors`, hasta los 32 últimos. Cada registro tiene `cellId`, `kind` (`WorkspaceSyncErrorKind`: `selection`, `range`, `crosshair`, `lifecycle`) y `error`. La lista se vacía llamando a `clearErrors`.

## Métodos públicos

- `snapshot()`: el estado completo: número de celdas, columnas y filas, celda activa, `links`, `sync`, celdas y errores.
- `cells()`: instantáneas de las celdas: `id`, `index`, `active`, `selection`, `visibleRange`, `crosshairTime`.
- `chart(id)` / `host(id)`: el gráfico y el elemento DOM de la celda.
- `add(id?)`: añade una celda; sin argumento, el identificador se genera.
- `remove(id)`: elimina una celda.
- `setCount(count)`, `setColumns(columns)`, `setLayout(layout)`: cambian la cuadrícula.
- `activate(id)`: hace activa una celda.
- `setLinks(options)`, `setSync(options)`: conmutan el enlace y la sincronización.
- `setSelection(id, selection)`: fija el instrumento y el marco temporal de una celda y los reparte por los enlaces.
- `clearErrors()`: vacía los errores acumulados.
- `subscribe(listener)` / `unsubscribe(listener)`: suscripción a la instantánea de estado.
- `dispose()`: libera las celdas y restaura los estilos del contenedor.

## Otros controladores de la capa

- `PaneController`: gestión reversible (deshacer/rehacer) de los paneles del gráfico: `resizePair`, `reorder`, `moveSeries`, `setState`, `toggleMinimized`, `toggleMaximized`. Trabaja mediante la pila de comandos común del gráfico y no vuelve a crear el contenido de los paneles.
- `IndicatorController`: edición verificada de los indicadores sobre el motor de cálculo: `update`, `setParameters`, `setSource`, `moveToPane`, `setPriceScale`, `setVisible`, `setOutputStyle`. Cada cambio va a parar a la pila de comandos, y la instantánea contiene las definiciones de los parámetros, el estado del origen y los estilos de las salidas.
- `IndicatorCatalogController`: búsqueda en el catálogo de indicadores (`search` por texto, categoría e indicador de favorito) y favoritos guardados por el anfitrión: `loadFavorites`, `setFavorite`, `toggleFavorite`.
- `IndicatorTemplateController`: plantillas transferibles de ajustes de indicador: `create`, `replace`, `rename`, `remove`, `apply`, `load`. El método `apply` traslada los parámetros, el origen, la visibilidad y los estilos de las salidas, pero deja deliberadamente sin cambios el panel y la escala de precios del destino.
- `serializeIndicatorTemplates`, `deserializeIndicatorTemplates`, `normalizeIndicatorTemplateDocument`, `INDICATOR_TEMPLATE_SCHEMA_VERSION`: serialización y comprobación del documento versionado de plantillas.
- `CompareController`: superposición de varios instrumentos en un mismo gráfico: `add`, `remove`, `setPrimary`, `setColor`, `setVisible`, `reload`, `loadMoreBefore`, `legend`. El modo de normalización se establece con `setMode` (`CompareMode.Percentage` o `CompareMode.IndexedTo100`) y la forma de alinear el tiempo, con `setAlignment` (`CompareAlignment.Chart` o `CompareAlignment.PrimarySession`). Cada instrumento tiene su propio `ChartDataController` y su propia suscripción.
- `ChartNavigator`: navegación por el historial sin vínculo con el DOM: `setRange`, `selectPreset`, `goToDate`, `cancel`. El controlador carga por su cuenta las páginas de historial que faltan (de forma predeterminada, no más de 100 por operación) y publica un modelo de conjunto formado por un número limitado de muestras (600 de forma predeterminada). Los ajustes predefinidos `1D`, `5D`, `1M`, `3M`, `6M`, `YTD`, `1Y`, `5Y`, `All` los devuelve `defaultNavigatorPresets`; el resultado de una operación lo describe `NavigatorNavigationOutcome` (`applied`, `clamped`, `page-limit`, `empty`, `cancelled`) y la alineación de la fecha, `NavigatorDateAlignment`.

## Véase también

- [Gráficos en JavaScript](../charts.md)
- [Relleno de historial](backfill.md)
- [Indicadores](indicators.md)
