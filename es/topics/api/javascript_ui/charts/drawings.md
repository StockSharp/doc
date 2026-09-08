# Herramientas de dibujo

`DrawingController` es la capa de dibujo manual del gráfico: líneas, figuras, niveles de Fibonacci y plantillas de posición. El controlador almacena las figuras como objetos JSON puros, las asocia a primitivos del lienzo, hace pasar cada cambio por la pila de deshacer del gráfico y se encarga de la construcción paso a paso con el ratón.

## Conexión

La capa se distribuye como punto de entrada aparte del paquete `@stocksharp/chart`:

```ts
import { BuiltInDrawingType, DrawingController } from '@stocksharp/chart/drawings';
```

Importar el punto de entrada registra de inmediato todos los tipos de dibujo integrados en el catálogo común `drawingDefinitionRegistry`.

## Creación y actualización

El controlador solo necesita el gráfico; la pila de comandos la toma de él de forma predeterminada (`chart.commandStack()`), por lo que deshacer y rehacer funcionan junto con el resto de acciones del gráfico:

```ts
import { CandlestickSeries, createChart } from '@stocksharp/chart';
import { BuiltInDrawingType, DrawingController } from '@stocksharp/chart/drawings';

const chart = createChart(document.getElementById('chart')!, { timeScale: { timeVisible: true } });
chart.addSeries(CandlestickSeries, {}).setData(candles);

const drawings = new DrawingController({ chart });

// Nivel horizontal: un punto, y el tiempo es tiempo Unix en segundos.
const level = drawings.create(
  BuiltInDrawingType.HorizontalLine,
  [{ time: 1_712_000_000, price: 68_000 }],
  { options: { color: '#f5c542', lineWidth: 2 } },
);

// Una línea de tendencia por dos puntos en un subpanel se indica mediante paneId.
drawings.create(
  BuiltInDrawingType.TrendLine,
  [
    { time: 1_712_000_000, price: 67_400 },
    { time: 1_712_600_000, price: 69_150 },
  ],
  { paneId: 'main' },
);

drawings.updateOptions(level.id, { lineWidth: 3 });
drawings.setLocked(level.id, true);

// La instantánea del conjunto llega ordenada por zOrder.
drawings.subscribe(items => console.log(items.length));

chart.commandStack().undo();
```

`create` rellena los campos que faltan: `paneId` es `main` de forma predeterminada, `visible` es `true`, `locked` es `false`, `zOrder` es una unidad mayor que el máximo actual, y las opciones se superponen a las `defaultOptions` del tipo. `add` inserta una instancia ya preparada por completo, `duplicate` copia una existente, y `remove` y `clear` eliminan. Cada una de estas llamadas deja en el historial exactamente un comando reversible.

`update` cambia cualquier combinación de campos (`points`, `options`, `paneId`, `visible`, `locked`, `zOrder`); `updateOptions`, `setVisible`, `setLocked` y `moveToPane` son formas abreviadas para los casos frecuentes. Antes de escribirla, la instancia se normaliza: los puntos y las opciones se comprueban en cuanto a compatibilidad con JSON y se congelan, el número de puntos se contrasta con el esquema del tipo, y el panel, con los paneles existentes del gráfico.

## Tipos integrados

Los identificadores están reunidos en `BuiltInDrawingType`; el valor de cadena es precisamente el campo `type` de la figura guardada.

| Constante | Valor | Puntos | Opciones |
|---|---|---|---|
| `HorizontalLine` | `horizontal-line` | 1 | `LineDrawingOptions` |
| `VerticalLine` | `vertical-line` | 1 | `LineDrawingOptions` |
| `TrendLine` | `trend-line` | 2 | `LineDrawingOptions` |
| `Ray` | `ray` | 2 | `LineDrawingOptions` |
| `Rectangle` | `rectangle` | 2 | `RectangleDrawingOptions` |
| `Text` | `text` | 1 | `TextDrawingOptions` |
| `Note` | `note` | 1 | `TextDrawingOptions` |
| `FibonacciRetracement` | `fibonacci-retracement` | 2 | `FibonacciDrawingOptions` |
| `Measure` | `measure` | 2 | `MeasureDrawingOptions` |
| `LongPosition` | `long-position` | 3 | `PositionDrawingOptions` |
| `ShortPosition` | `short-position` | 3 | `PositionDrawingOptions` |

Los conjuntos de opciones se diferencian por su propósito:

- `LineDrawingOptions`: `color`, `lineWidth` (en el rango (0, 20]), `lineStyle` (0…4).
- `RectangleDrawingOptions`: lo mismo más `fillColor` para el relleno.
- `TextDrawingOptions`: `text` (hasta 10 000 caracteres, con los saltos de línea incluidos), `color`, `backgroundColor`, `borderColor`, `borderWidth`, `fontSize`, `fontFamily`, `padding`. `Note` se diferencia de `Text` únicamente en los valores predeterminados: fondo, borde y márgenes mayores.
- `FibonacciDrawingOptions`: `levels` (de 2 a 32 valores en el rango [-5, 5]; los duplicados se eliminan y la lista se ordena), `labelsVisible`, `extendRight`, y también `color`, `lineWidth`, `lineStyle`, `fillColor`, `fontSize`.
- `MeasureDrawingOptions`: `color`, `lineWidth`, `fillColor`, `labelColor`, `labelBackgroundColor`, `fontSize`. El rótulo muestra la variación del precio, el porcentaje y la duración del intervalo seleccionado.
- `PositionDrawingOptions`: `entryColor`, `targetColor`, `stopColor`, `targetFillColor`, `stopFillColor`, `textColor`, `lineWidth`, `fontSize` y `quantity`. Los tres puntos se indican en orden: entrada, objetivo y stop; a partir de ellos se calculan el beneficio, el riesgo y la relación R:R de los rótulos.

Un valor de opción que no supera la comprobación de tipo provoca una excepción: no se puede guardar una figura con un ancho de línea incorrecto o con un color vacío.

## Construcción con el ratón

La entrada paso a paso la lleva el propio controlador: pone el gráfico en modo de dibujo y se suscribe a los clics y al crosshair.

```ts
drawings.subscribeCreation(state => {
  if (state === null) return;                    // construcción terminada o cancelada
  console.log(state.name, state.points.length, state.minimumPoints, state.maximumPoints);
});

drawings.beginCreation(BuiltInDrawingType.Rectangle, { options: { color: '#26a69a' } });

// Cancelación con Esc mientras la figura no haya reunido el número de puntos necesario.
document.addEventListener('keydown', event => {
  if (event.key === 'Escape') drawings.cancelCreation();
});
```

Cada clic añade un punto, pasado por el imán; el movimiento del cursor actualiza el borrador, que se dibuja con el mismo primitivo que la figura terminada, pero no llega al historial. El panel queda fijado con el primer clic y los clics en otros paneles se ignoran. En cuanto se reúnen tantos puntos como permite el máximo del tipo, la construcción termina sola y se crea una figura normal. `finishCreation` cierra la construcción anticipadamente y devuelve `null` si hay menos puntos que el mínimo; `cancelCreation` descarta el borrador; `creation` entrega la instantánea actual, `DrawingCreationSnapshot`.

## Ajuste a las barras

El imán atrae el punto hacia los valores de las series del panel actual: el cálculo se realiza en coordenadas de pantalla, según la distancia vertical hasta el candidato.

```ts
import { DrawingMagnetMode } from '@stocksharp/chart/drawings';

const drawings = new DrawingController({
  chart,
  magnet: { mode: DrawingMagnetMode.Weak, maxDistance: 12 },
});

drawings.applyMagnetOptions({ mode: DrawingMagnetMode.Strong });
console.log(drawings.magnetOptions());
```

`DrawingMagnetMode.None` desactiva el ajuste, `Weak` (el modo predeterminado) atrae solo dentro de `maxDistance` —10 píxeles CSS de forma predeterminada— y `Strong` atrae siempre al valor más cercano. Cambiar los ajustes durante la construcción recalcula de inmediato el punto de la vista previa.

## Guardar y restaurar

`DrawingInstance` no contiene deliberadamente objetos de tiempo de ejecución, por lo que el conjunto de dibujos se serializa tal cual:

```ts
import type { DrawingInstance } from '@stocksharp/chart/drawings';

const saved = JSON.stringify(drawings.drawings());

const result = drawings.replaceAll(JSON.parse(saved) as DrawingInstance[], { unknownType: 'skip' });
console.log(result.restored.length, result.skipped);
```

`replaceAll` sustituye todo el conjunto por completo: primero se comprueban todas las instancias de entrada (los identificadores repetidos son un error), después se retiran del gráfico las figuras antiguas y se añaden las nuevas. Si al menos una figura no encaja, se restaura el estado anterior. Un `type` desconocido, con la política `skip` (la predeterminada), va a parar a `skipped` con el motivo `unknown-type`; con `error`, interrumpe la restauración. La restauración vacía el historial de comandos, por lo que no se puede invocar dentro de una transacción.

## Tipos de dibujo propios

El catálogo de tipos es extensible. Basta con describir la definición y devolver una asociación a un primitivo: la envoltura lista para usar, con selección, marcadores y arrastre, la proporciona `createInteractiveDrawingBinding`:

```ts
import { createInteractiveDrawingBinding, registerDrawing } from '@stocksharp/chart/drawings';

registerDrawing({
  type: 'price-band',
  name: 'Price Band',
  points: { min: 2, max: 2 },
  defaultOptions: { color: '#4a9eff' },
  normalizeOptions: options => Object.freeze({ color: String(options.color).trim() }),
  create: (instance, events) => createInteractiveDrawingBinding(instance, events, {
    draw(context) {
      const [first, second] = context.points;
      if (second === undefined) return;
      context.context.strokeStyle = String(context.instance.options.color);
      context.context.strokeRect(
        context.plot.x, Math.min(first.y, second.y),
        context.plot.width, Math.abs(second.y - first.y),
      );
    },
    hitTest(point, context) {
      const [first, second] = context.points;
      if (second === undefined) return null;
      return point.y >= Math.min(first.y, second.y) && point.y <= Math.max(first.y, second.y)
        ? { cursor: 'move' }
        : null;
    },
  }),
});

drawings.create('price-band', [
  { time: 1_712_000_000, price: 67_800 },
  { time: 1_712_600_000, price: 68_900 },
]);
```

`draw` recibe los puntos en pantalla, el rectángulo del área de dibujo, el tema, el factor de escala y el indicador de selección; `hitTest` responde a si el cursor ha caído dentro del cuerpo de la figura. Las opcionales `autoscaleInfo` y `handleColor` determinan la participación en el escalado automático y el color de los marcadores. `normalizeOptions` se llama antes de cada escritura en el modelo: es el único sitio donde conviene comprobar los valores de las opciones.

Arrastrar el cuerpo o un punto concreto pasa por los eventos `preview` (estados intermedios, no se escriben en el historial), `commit` (un único comando «Edit drawing») y `cancel` (vuelta al estado anterior al gesto). Una figura bloqueada (`locked`) no se arrastra ni muestra marcadores.

El catálogo también se puede gobernar directamente: `unregisterDrawing(type)`, `getDrawingDefinition(type)`, `getDrawingTypes()`, y `DrawingDefinitionRegistry` permite crear un catálogo aparte y pasarlo al controlador con el parámetro `registry`.

## Métodos públicos

`DrawingController`:

- `drawings()`, `get(id)`, `has(id)`: lectura del conjunto actual.
- `create(type, points, options?)`, `add(instance)`, `duplicate(id, duplicateId?)`: adición de figuras.
- `update(id, patch)`, `updateOptions(id, patch)`, `setVisible(id, visible)`, `setLocked(id, locked)`, `moveToPane(id, paneId)`: modificación.
- `remove(id)`, `clear()`: eliminación.
- `beginCreation(type, options?)`, `finishCreation()`, `cancelCreation()`, `creation()`: construcción con el ratón.
- `magnetOptions()`, `applyMagnetOptions(patch)`: ajuste a las barras.
- `replaceAll(instances, options?)`: restauración del conjunto guardado.
- `subscribe(listener)` / `unsubscribe(listener)`, `subscribeCreation(listener)` / `unsubscribeCreation(listener)`: suscripciones.
- `dispose()`: libera los recursos.

El constructor acepta `chart` (obligatorio) y, además, `registry`, `commandStack`, `idFactory` y `magnet`.

El punto de entrada exporta también el resto de partes de la capa: `DrawingMagnet` para calcular el ajuste por su cuenta, `InteractiveDrawingPrimitive` junto con `createInteractiveDrawingBinding`, las funciones de comprobación `normalizeDrawingInstance` y `normalizeDrawingOptions`, los conjuntos de definiciones ya preparados `builtInLineDrawingDefinitions`, `builtInShapeDrawingDefinitions`, `builtInAnalysisDrawingDefinitions`, `builtInPositionDrawingDefinitions` y sus funciones asociadas `registerBuiltInLineDrawings`, `registerBuiltInShapeDrawings`, `registerBuiltInAnalysisDrawings`, `registerBuiltInPositionDrawings` para registrarlos en un catálogo propio.

## Véase también

- [Gráficos en JavaScript](../charts.md)
- [Velas](candlestick.md)
- [Indicadores](indicators.md)
- [Relleno de historial](backfill.md)
