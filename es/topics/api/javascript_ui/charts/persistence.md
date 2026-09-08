# Guardado de la disposición

`ChartStatePersistence` reúne la disposición del gráfico —opciones, paneles, escalas de precio, series, indicadores y objetos gráficos— en una única instantánea JSON verificada y la restaura de vuelta. Los datos de las barras no entran en la instantánea: se guarda la configuración, y las cotizaciones llegan de su propia fuente.

La capa no posee ni el almacenamiento ni la regla de nomenclatura de las claves. Dónde escribir (archivo, backend, `localStorage`, IndexedDB) y cómo separar las instantáneas (por disposición, por instrumento, por usuario) lo decide la aplicación mediante una implementación de `ChartStateStorage` y la función `key`.

## Creación y actualización

La importación se hace desde el punto de entrada `@stocksharp/chart/persistence`:

```ts
import { createChart, CandlestickSeries } from '@stocksharp/chart';
import { DrawingController } from '@stocksharp/chart/drawings';
import {
  ChartStatePersistence,
  NativeChartLayoutAdapter,
  IndicatorEngineStateAdapter,
  type ChartStateStorage,
  type IndicatorEnginePersistenceApi,
} from '@stocksharp/chart/persistence';

declare const indicatorEngine: IndicatorEnginePersistenceApi;

const chart = createChart(document.querySelector<HTMLElement>('#chart')!, {});
chart.addSeries(CandlestickSeries, { id: 'price', upColor: '#26a69a', downColor: '#ef5350' });

const storage: ChartStateStorage = {
  load: key => localStorage.getItem(key),
  save: (key, value) => { localStorage.setItem(key, value); },
  remove: key => { localStorage.removeItem(key); },
};

const persistence = new ChartStatePersistence<{ layoutId: string; symbol: string }>({
  layout: new NativeChartLayoutAdapter({ chart, mainPaneId: 'main' }),
  indicators: new IndicatorEngineStateAdapter({ engine: indicatorEngine }),
  drawings: new DrawingController({ chart }),
  storage,
  key: ({ layoutId, symbol }) => `chart:${layoutId}:${symbol}`,
  pretty: true,
});

const context = { layoutId: 'desk', symbol: 'BTC@IMEX' };

await persistence.save(context);

const restored = await persistence.load(context);
if (restored !== null) {
  console.log(restored.state.panes.length);
  console.log(restored.drawings.skipped);   // los objetos de tipos desconocidos se omiten, no arruinan la restauración
}
```

El parámetro de tipo `TContext` es lo que se pasa a `save`, `load` y `remove`; la función `key` convierte el contexto en una cadena de clave y está obligada a devolver una cadena no vacía. `pretty: true` escribe el JSON con sangrado.

`DrawingController` debe ser la misma instancia que atiende el dibujo en el gráfico; de lo contrario se guardará un conjunto de objetos vacío.

## Adaptadores

`ChartStatePersistence` no conoce ni la API nativa del gráfico ni el motor de indicadores: trabaja mediante dos adaptadores. Las implementaciones ya preparadas forman parte del mismo módulo, pero con una arquitectura propia se pueden pasar las suyas: las interfaces `ChartStateLayoutAdapter` (`capture`, `restore`) y `ChartStateIndicatorAdapter` (`capture`, `clear`, `restore`) son abiertas.

**`NativeChartLayoutAdapter`** captura y devuelve la disposición del propio gráfico: las opciones del gráfico, los paneles con su orden, altura, altura mínima y estado (`normal`, `minimized`, `maximized`), los ajustes de las escalas de precio y también las series con su tipo, panel, escala y opciones de estilo. Opciones del constructor:

- `chart`: la instancia del gráfico (obligatoria).
- `mainPaneId`: identificador del panel raíz que sobrevive a la restauración; de forma predeterminada `main` y, si no, el primer panel.
- `createSeries(series, pane)`: creación propia de la serie en lugar del registro de tipos, cuando hay que conectar la serie a una fuente de datos.
- `includeSeries(series)`: filtro; una serie para la que se devuelve `false` no se guarda ni se elimina durante la restauración.
- `onRemoveSeries(series)`: se llama para una serie ajena que la restauración se ha visto obligada a desconectar porque su panel no forma parte de la disposición que se carga.
- `onUnknownSeries(series)`: el tipo de serie no está en el registro.

Una serie con la opción `persist: false` queda excluida de la instantánea igual que una rechazada por el filtro `includeSeries`.

**`IndicatorEngineStateAdapter`** guarda la configuración de los indicadores —tipo, parámetros, estilos de dibujo, vínculo con el panel y la escala, visibilidad y origen—, pero no los valores calculados: tras la restauración se vuelven a calcular. Opciones del constructor:

- `engine`: el motor de indicadores que implementa `IndicatorEnginePersistenceApi` (`getIndicators`, `removeAll`, `add`, `setVisible`).
- `resolveTargetPaneId(indicator)`: correspondencia entre el panel guardado y el panel del anfitrión, cuando los identificadores difieren.
- `onUnknownIndicator(indicator)`: el motor no ha podido crear un indicador de ese tipo.
- `onUnknownStyle(indicator, styleId)`: en los estilos ha aparecido un identificador que el indicador no tiene.

Un indicador que se calcula sobre la salida de otro indicador se restaura después de este: el adaptador ordena por sí mismo la cadena de orígenes e informa de un error si la referencia lleva a un indicador ausente o si el grafo tiene ciclos.

## Formato del estado y migraciones

La instantánea se describe con el tipo `ChartStateV1`, con los campos `schemaVersion`, `chartOptions`, `panes`, `series`, `indicators`, `drawings`; la versión actual del esquema es la constante `CHART_STATE_SCHEMA_VERSION` (igual a 1).

- `serializeChartState(state, { pretty })`: comprueba el estado y lo convierte en una cadena JSON.
- `deserializeChartState(value, { migrations })`: analiza la cadena (o acepta un objeto ya preparado), ejecuta las migraciones hasta la versión actual y comprueba el resultado.
- `normalizeChartStateV1(value)`: comprobación y congelación del estado; se rechazan las claves ajenas, los identificadores duplicados, las referencias a paneles inexistentes y una disposición sin ningún panel.
- `normalizePersistedObject(value, path, { omitUndefined })`: copia profunda de un JSON arbitrario en un objeto inmutable; se prohíben los ciclos, los valores no numéricos, un anidamiento excesivo y las claves `__proto__`, `prototype`, `constructor`.

Las instantáneas antiguas se elevan mediante migraciones paso a paso. El registro común `chartStateMigrations` ya contiene el paso de la versión 0 a la versión 1, y los pasos propios se registran así:

```ts
import {
  ChartStateMigrationRegistry,
  deserializeChartState,
} from '@stocksharp/chart/persistence';

const migrations = new ChartStateMigrationRegistry();
migrations.register(1, state => ({ ...state, schemaVersion: 2 }));

const state = deserializeChartState(json, { migrations });
```

Cada migración lleva el estado exactamente una versión hacia delante y está obligada a fijar el nuevo valor de `schemaVersion`. Una instantánea cuya versión sea superior a la admitida no se acepta para su carga.

## Métodos públicos

- `snapshot()`: reúne el estado actual del gráfico en un `ChartStateV1`, sin acceder al almacenamiento.
- `restore(state)`: aplica el estado al gráfico; devuelve `{ state, drawings }`, donde `drawings` contiene las listas `restored` y `skipped`.
- `save(context)`: toma la instantánea, la serializa y la escribe en el almacenamiento con la clave calculada; devuelve el estado guardado.
- `load(context)`: lee el registro por su clave, ejecuta las migraciones y lo restaura; `null` si el registro no existe.
- `remove(context)`: elimina el registro del almacenamiento.

El orden de restauración es fijo: primero se liberan los indicadores, después se restaura la disposición de paneles y series, luego los indicadores y, en último lugar, los objetos gráficos.

El módulo exporta también los tipos que describen la instantánea y los adaptadores: `ChartStateLayoutSnapshot`, `ChartStateRestoreResult`, `ChartStatePersistenceOptions`, `PersistedPane`, `PersistedPriceScale`, `PersistedSeries`, `PersistedIndicator`, `PersistedDrawing`, `PersistedChartOptions`, `PersistedSeriesOptions`, `PersistedIndicatorParameters`, `PersistedIndicatorStyles`, `PersistedObject`, `PersistedJsonValue`, `PersistableIndicatorEntry`, `RawChartState`, `ChartStateMigration`, `MaybePromise`.

## Véase también

- [Gráficos en JavaScript](../charts.md)
- [Indicadores](indicators.md)
- [Relleno de historial](backfill.md)
