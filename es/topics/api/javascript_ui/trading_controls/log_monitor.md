# Registro

`LogMonitorWidget` muestra el registro de actividad: a la izquierda, el árbol de fuentes; a la derecha, la tabla de mensajes de la fuente seleccionada y de todo su subárbol. Cada mensaje se almacena una sola vez y lleva el identificador de la fuente que lo escribió, y el número de filas conservadas está limitado.

![Registro con el árbol de fuentes, el filtro de niveles y la tabla de mensajes](../../../../images/javascript_controls_log_monitor.png)

## Creación y actualización

```ts
import {
  LogLevels,
  LogMonitorWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const log = LogMonitorWidget.create(
  document.querySelector<HTMLElement>('#log')!,
  {},
  {
    host,
    maxMessages: 20_000,
  },
);

log.setSources([
  { id: 'connector', name: 'Connector' },
  { id: 'strategy-1', name: 'SMA', parentId: 'connector' },
]);

log.append([{
  id: 1,
  time: Date.now(),
  level: LogLevels.Warning,
  sourceId: 'strategy-1',
  message: 'Orden rechazada: fondos insuficientes',
}]);

log.select('connector');
```

De las dependencias solo es obligatoria `host`: el registro se escribe, no se actúa sobre él, por lo que el control no necesita manejadores. Las demás son opcionales:

| Dependencia | Valor predeterminado | Propósito |
|---|---|---|
| `maxMessages` | `5000` | Cuántos mensajes conservar. Los sobrantes se descartan por el principio de la lista. |
| `chrome` | `true` | Si debe dibujar su propio encabezado con el botón de cierre. Un anfitrión que titula y cierra el panel por su cuenta (por ejemplo, mediante la pestaña de un panel acoplado) pasa `false`. |
| `sources` | `true` | Si debe mostrar el árbol de fuentes al crearse. Es solo el estado inicial: el árbol vuelve desde el menú contextual de la tabla o llamando a `showSources`. |

`setSources` transmite la lista completa de fuentes: una fuente que desaparece se elimina del árbol y la selección se restablece a «todas las fuentes». `append` añade lo que se acaba de registrar y `clear` olvida todos los mensajes, dejando el árbol de fuentes en su sitio.

La propiedad estática `LogMonitorWidget.TYPE` contiene el identificador del control, `logMonitor`.

## Fuentes y filtros

Una fuente declara su padre (`parentId`), no sus hijos, y puede aparecer antes que su padre. El árbol se construye con lo que ya ha llegado: una fuente con un padre desconocido se convierte en raíz y un ciclo se rompe en el primer nodo. Las filas del árbol son planas y el anidamiento se indica con la sangría; la fila superior selecciona todas las fuentes a la vez.

Actúan simultáneamente tres filtros: el conjunto de niveles activados, la cadena de búsqueda por el texto del mensaje (sin distinguir mayúsculas) y el subárbol de fuentes seleccionado. Los niveles se conmutan con los botones de la barra de herramientas — `error`, `warning`, `info`, `debug`, `verbose` del objeto `LogLevels`—; en la columna estrecha el nivel se muestra con las letras `E`, `W`, `I`, `D`, `V`. El método `visible` devuelve lo que queda tras aplicar todos los filtros.

La tabla consta de las columnas de fuente, hora, nivel y mensaje, está ordenada por hora de forma ascendente y admite selección múltiple, ordenación, ocultación de columnas, filtros y menú contextual. En el menú se ha añadido una entrada para mostrar el árbol de fuentes. Un botón de la barra de herramientas vuelca las filas visibles a XLSX; en el volcado figuran la hora formateada con `host.presentation.timeText` y el nombre completo del nivel, no la letra.

## Qué hace el anfitrión

El control obtiene del anfitrión las traducciones (`host.t`), el formato de la hora (`host.presentation.timeText`) y el tratamiento del cierre del panel (`host.close`); además, se registra con `host.register` y se da de baja en `dispose`. No guarda ajustes propios en `host.preferences` ni conserva el estado de la instancia: el segundo argumento de `create` se acepta por uniformidad, pero no se lee. La recogida de los mensajes, su entrega y la restauración de la posición del panel corren a cargo del anfitrión.

## Métodos públicos

- `setSources(sources)`: sustituye la lista de fuentes.
- `append(messages)`: añade mensajes respetando el límite `maxMessages`.
- `clear()`: borra los mensajes.
- `select(sourceId)`: muestra el subárbol de una fuente; `null` muestra todas.
- `visible()`: los mensajes que quedan tras aplicar todos los filtros.
- `sourcesShown()`: indica si el árbol de fuentes está visible.
- `showSources(on)`: muestra u oculta el árbol; el filtro de fuente seleccionado se conserva.
- `dispose()`: libera los recursos.

## Véase también

- [Controles de negociación JavaScript](../trading_controls.md)
- [Estrategias](strategies.md)
- [Estadísticas](statistics.md)
