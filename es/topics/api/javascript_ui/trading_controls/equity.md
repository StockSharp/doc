# Curva de capital

`EquityWidget` dibuja el P&L acumulado de una ejecución como un gráfico en el tiempo. El panel está registrado con el identificador `equity` (`ControlTypes.Equity`) y la curva la construye el motor `@stocksharp/chart`, que se conecta como dependencia de pares (peer).

![Curva de capital según los resultados de la ejecución](../../../../images/javascript_controls_equity.png)

## Creación y actualización

```ts
import {
  EquityWidget,
  type PnlPoint,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const equity = EquityWidget.create(
  document.querySelector<HTMLElement>('#equity')!,
  {},
  { host },
);

const start = Date.now() - 3 * 60 * 60 * 1000;

const points: PnlPoint[] = [
  { time: start, value: 0 },
  { time: start + 45 * 60 * 1000, value: 320.5 },
  { time: start + 95 * 60 * 1000, value: -140.25 },
  { time: Date.now(), value: 1_180.75 },
];

equity.update(points);
```

La única dependencia de `EquityDeps` es `host`; el panel no tiene manejadores de acciones porque en la curva no hay nada que ejecutar. El segundo argumento de `create` —el estado guardado de la instancia— no lo utiliza el control: el panel no guarda ajustes propios ni en el estado ni en `host.preferences`.

`update` sustituye la ejecución por completo. El valor de la curva es acumulativo, por lo que un conjunto de puntos sin la muestra transmitida anteriormente se considera otra ejecución, no la continuación de la anterior.

## Datos y dibujado

El campo `time` de `PnlPoint` son milisegundos Unix; el motor cuenta el tiempo en segundos y la conversión la realiza el propio control. Los puntos se ordenan por tiempo, se descartan las muestras con `time` o `value` no numéricos y, de entre varios valores dentro del mismo segundo, se conserva el último: es el que indica dónde estaba realmente la ejecución en ese momento.

Mientras haya menos de dos puntos utilizables, el panel muestra el mensaje de la clave `NoEquity`, el gráfico no se crea y `chart()` devuelve `null`. El gráfico se construye en el primer dibujado y no en el constructor: el motor necesita un contenedor ya colocado en la página. De los cambios de tamaño del área se encarga un `ResizeObserver`, que llama a `resize` del motor.

Los colores, la fuente y el color de la cuadrícula se obtienen de `host.presentation.canvasPalette()`. La curva se colorea según el último valor de la ejecución: `up` cuando el valor no es menor que cero y `down` cuando es negativo; el relleno bajo la línea es del mismo color, atenuado al 28 % arriba y al 2 % abajo. El eje de tiempo muestra horas y segundos en la zona horaria del navegador y, si `Intl` no está disponible, en UTC.

## Título, ayuda emergente y botones

En el encabezado del panel se muestra el último valor de la ejecución con el formato de `formatPnl`, con signo y dos decimales; la clase de color la devuelve `host.presentation.pnlClass`. Sobre la curva se muestra el valor bajo el puntero: el instante expresado con palabras mediante `host.presentation.timeText` y, al lado, el propio valor. Cuando el puntero abandona el gráfico, la ayuda emergente se borra.

El botón del encabezado con la ayuda emergente `ResetView` llama a `resetZoom` y el botón de cierre, a `host.close()`. El panel obtiene el texto visible mediante las claves `Equity`, `ResetView`, `ClosePanel`, `PnLChart` y `NoEquity`.

El control se encarga por sí mismo de la forma de la curva, de su color y de la escala. Al anfitrión le quedan el idioma de los textos, la paleta, el formato del instante de tiempo y el origen de los propios puntos: el control no calcula el P&L ni solicita datos a nadie.

## Métodos públicos

- `EquityWidget.create(hostEl, state, deps)`: crea el panel y añade su elemento raíz al contenedor.
- `EquityWidget.TYPE`: el identificador `equity`.
- `update(points)`: muestra la ejecución completa.
- `resetZoom()`: devuelve la vista de toda la ejecución tras hacer zoom.
- `chart()`: la instancia de `IChartApi`, para que el anfitrión dibuje lo suyo: una línea de referencia, una marca de caída máxima. Devuelve `null` mientras no exista el gráfico.
- `dispose()`: desconecta el observador de tamaño, elimina el gráfico y da de baja el registro en el anfitrión.

La propiedad `rootEl` proporciona el elemento raíz del panel.

## Conectar el motor de gráficos

El paquete `@stocksharp/chart` se instala por separado:

```bash
npm install @stocksharp/chart
```

El paquete precompilado `sstradingcontrols.js` no incluye el motor, por lo que en una página sin empaquetador sus scripts se incluyen aparte:

```html
<script src="https://cdn.jsdelivr.net/npm/@stocksharp/indicators/dist/ssindicators.js"></script>
<script src="https://cdn.jsdelivr.net/npm/@stocksharp/chart/dist/sschart.js"></script>
```

## Véase también

- [Controles de negociación JavaScript](../trading_controls.md)
- [Posiciones](positions.md)
- [Historial de operaciones](trade_history.md)
- [Gráficos en JavaScript](../charts.md)
