# Mapa de calor de optimización

`OptimizationHeatmapWidget` dibuja una métrica en función de dos parámetros: los valores del primero se disponen en horizontal y los del segundo en vertical, y en la intersección hay una celda coloreada según lo que dio ese juego de parámetros. El control no sabe nada sobre la optimización en sí —el mapa de una métrica sobre dos ejes es el mismo, sea como sea que se hayan calculado los pares—, por lo que el anfitrión transmite los pares junto con los nombres de los ejes y de la métrica.

![Mapa de calor de optimización sobre dos parámetros](../../../../images/javascript_controls_optimization_heatmap.png)

## Creación y actualización

```ts
import {
  HeatDirections,
  OptimizationHeatmapWidget,
  type HeatCell,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const heatmap = OptimizationHeatmapWidget.create(
  document.querySelector<HTMLElement>('#heatmap')!,
  {},
  { host },
);

const cells: HeatCell[] = [
  { x: '10', y: '00:05:00', value: 12_400 },
  { x: '10', y: '00:15:00', value: 9_150 },
  { x: '20', y: '00:05:00', value: -1_800 },
  { x: '20', y: '00:15:00', value: 15_900 },
];

heatmap.update({
  xLabel: 'Length',
  yLabel: 'Timeframe',
  metricLabel: 'Net profit',
  betterWhen: HeatDirections.Higher,
  cells,
});
```

La única dependencia es `host`; la interfaz `OptimizationHeatmapDeps` no contiene otros campos. El mapa no acepta manejadores: una celda es la media de las ejecuciones de un par, no una ejecución, así que al hacer clic no hay nada que abrir. El segundo argumento de `create` es el estado guardado de la instancia; el control no lo lee ni escribe nada en él.

`update` sustituye el mapa por completo: un par que sale del conjunto deja de existir, y la celda que quedara de él informaría de una ejecución que ya no está en el informe.

El argumento de `update` es un objeto `HeatmapData` con los campos:

| Campo | Propósito |
|---|---|
| `xLabel` | Nombre del eje horizontal, se rotula debajo del mapa. |
| `yLabel` | Nombre del eje vertical, se rotula encima del mapa. |
| `metricLabel` | Nombre de la métrica, se muestra en el título del panel. |
| `betterWhen` | Hacia dónde es mejor: `HeatDirections.Higher` (`'higher'`) o `HeatDirections.Lower` (`'lower'`). El campo es obligatorio: sin él, un mapa de caídas máximas pintaría el peor rincón con el color del triunfo. |
| `cells` | Mediciones `HeatCell`: `x` e `y` son los valores de los ejes como cadenas y `value` es un número. |

El tipo `HeatmapData` está declarado en el módulo del control y la exportación raíz del paquete no lo reexporta: si necesita tipado explícito, impórtelo desde el subpath `@stocksharp/trading-controls/optimization-heatmap-widget`.

La propiedad estática `OptimizationHeatmapWidget.TYPE` es igual a `ControlTypes.OptimizationHeatmap`, es decir, al identificador `optimizationHeatmap`.

## Datos y presentación

Los ejes son discretos, por lo que sus valores se transmiten como cadenas: `10`, `00:05:00` y `True` son posiciones equivalentes en el eje. El orden de los valores es numérico si todos ellos son números (de lo contrario, `10` iría antes que `2` y la forma del mapa sería consecuencia de cómo se escriben los números) y textual en los demás casos; para cadenas de ancho fijo, como las que .NET usa para los intervalos de tiempo, el orden textual coincide con el cronológico. La primera fila de la cuadrícula queda en la base del mapa: es un gráfico, y el eje Y crece hacia arriba.

Varias ejecuciones sobre un mismo par se agrupan en una sola celda —la media junto con el número de ejecuciones—; un registro con una métrica no numérica se descarta en lugar de estropear toda la celda. El color se calcula respecto a un valor de referencia: si las mediciones cruzan el cero, la referencia es el cero; en caso contrario, es el punto medio del rango, porque anclarse a un cero al que la búsqueda no llegó daría una mancha uniforme sin contraste. La amplitud hasta el color pleno es igual a ambos lados, de modo que una misma saturación en cualquier punto significa una misma desviación de la métrica. La dirección `betterWhen` se tiene en cuenta en el signo: el mejor resultado siempre se pinta con el color de subida.

Los números no se imprimen en las celdas: en una búsqueda de cuarenta por cuarenta las cifras de una celda son ilegibles, y leer el color es precisamente el sentido del mapa. Un par que nadie ha ejecutado no queda vacío, sino tachado con una diagonal: un par no probado y un par con resultado cero son hechos distintos que una escala con el cero en el punto neutro dibujaría igual. La mejor celda se rodea con un marco del color de la cuadrícula, el único color sin dirección de la paleta.

Sobre el mapa se dibuja una leyenda con esos mismos dos colores y tres rótulos: el borde inferior, el valor de referencia y el borde superior. Los rótulos de los ejes se aclaran si los valores dejan de caber, pero el valor extremo del eje siempre se rotula. Los números se muestran mediante `formatStatistic`, el mismo formato que en el panel de estadísticas: redondeo a dos decimales.

Al pasar el puntero sobre una celda medida aparece una ayuda emergente con el valor de ambos ejes y el de la métrica. El número de ejecuciones se añade solo cuando se ha promediado más de una, y la marca `Best` solo en la mejor celda. Sobre una celda tachada no hay ayuda emergente: ya se ve que no se ha probado. La ayuda emergente se ajusta a los bordes del lienzo para no salirse por el borde en las celdas extremas.

Mientras no haya ninguna medición, en lugar del mapa se muestra un marcador de posición con el texto de la clave `NoOptimizationResults`.

## Qué hace el control y qué queda a cargo del anfitrión

El control construye por sí mismo el marcado del panel, ajusta el lienzo al contenedor mediante `ResizeObserver` y crea un búfer en píxeles físicos según `devicePixelRatio`, ya que de otro modo el mapa se dibujaría con una cuadrícula de líneas capilares. Los colores y la fuente se los pide a `host.presentation.canvasPalette()` en cada dibujado: `up` y `down` son los dos lados de la escala, `grid` es la cuadrícula, los tachados, el marco de la mejor celda y los rótulos, y `font` es la fuente del texto del lienzo. Aquí el paquete no elige una paleta propia, por lo que un cambio de tema en el anfitrión vuelve a dibujar el mapa con los colores nuevos. La transparencia es lo único de lo que dispone el propio mapa.

El botón de cierre del panel llama a `host.close()`, la instancia se registra con `host.register` y se da de baja en `dispose`. Todo el texto visible se solicita mediante `host.t`: `OptimizationHeatmap`, `OptimizationHeatmapChart`, `ClosePanel`, `NoOptimizationResults`, `Runs`, `Best`. El control no guarda ajustes propios en `host.preferences` y no tiene claves.

El anfitrión suministra los datos: el mapa no lanza la búsqueda, no elige ni calcula la métrica y no adivina la dirección de «mejor»; dibuja lo que se le pasa en `update`.

## Métodos públicos

- `update(data)`: muestra el mapa completo.
- `dispose()`: desconecta el observador de tamaño, llama a `host.unregister` y retira el elemento raíz.

## Funciones auxiliares

Toda la geometría del mapa está en un módulo aparte y el paquete la exporta: puede utilizarse sin el control.

- `layoutHeatmap(input)`: el trazado del mapa —cuadrícula, huecos, rótulos, leyenda y escala—; `null` si no hay mediciones.
- `hitHeatmap(layout, x, y)`: la celda bajo un punto, o `null`.
- `heatScale(buckets)`: el valor de referencia, la amplitud y los límites del rango.
- `tintOf(value, scale, betterWhen)`: la saturación de −1 a 1, donde lo positivo siempre es mejor.
- `valueAt(tint, scale, betterWhen)`: la transformación inversa, para los rótulos de la leyenda.
- `HeatDirections`: las direcciones `Higher` y `Lower`.

## Véase también

- [Controles de negociación JavaScript](../trading_controls.md)
- [Superficie de optimización](optimization_surface.md)
- [Estadísticas](statistics.md)
- [Curva de capital](equity.md)
