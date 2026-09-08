# Estrategias

`StrategiesWidget` muestra la lista de estrategias en ejecución: una fila por estrategia con su estado, modo de negociación, posición, contadores de órdenes y operaciones, beneficio y botones de control. El identificador del control es `strategies` (`ControlTypes.Strategies`), accesible también mediante la propiedad estática `StrategiesWidget.TYPE`.

![Lista de estrategias con estado, posición, PnL y curva de rentabilidad](../../../../images/javascript_controls_strategies.png)

## Creación y actualización

```ts
import {
  StrategiesWidget,
  StrategyStates,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

let strategies!: StrategiesWidget;

strategies = StrategiesWidget.create(
  document.querySelector<HTMLElement>('#strategies')!,
  {},
  {
    host,
    tradingModes: ['Disabled', 'CancelOrders', 'ReducePosition', 'Full'],
    start: id => console.log('start', id),
    stop: id => console.log('stop', id),
    closePosition: id => console.log('flatten', id),
    openStrategy: id => console.log('open', id),
    riskRules: id => console.log('risk', id),
    setTradingMode: (id, mode) => console.log('mode', id, mode),
  },
);

strategies.update([{
  id: 'sma-1',
  name: 'SMA crossover',
  state: StrategyStates.Started,
  online: true,
  tradingMode: 'Full',
  portfolio: 'Demo',
  security: 'BTC@IMEX',
  position: 0.25,
  ordersCount: 12,
  tradesCount: 8,
  pnlChange: 105,
  realized: 20,
  unrealized: 85,
  pnl: [
    { time: 1, value: 0 },
    { time: 2, value: 60 },
    { time: 3, value: 105 },
  ],
}]);
```

`update` recibe el conjunto completo de filas y sustituye con él la tabla: una estrategia que no aparece en la lista transmitida se considera eliminada y su fila desaparece. El control no dispone de actualización en flujo de una sola fila: el nuevo estado llega como lista completa.

El segundo argumento de `create` es el estado guardado de la instancia. El control no lo lee ni guarda nada por su cuenta: ni claves en `host.preferences` ni llamadas a `host.persistState`.

El objeto `StrategyStates` exporta los estados `Stopped`, `Starting`, `Started` y `Stopping`.

## Dependencias

Solo el anfitrión es obligatorio. La integridad del anfitrión se comprueba al crear el control con la función `assertHost`, de modo que un `TradingHost` incompleto provoca una excepción con el nombre del miembro que falta, y no un botón que no funciona.

| Dependencia | Obligatoriedad | Comportamiento predeterminado |
|---|---|---|
| `host` | obligatoria | — |
| `start(id)` | opcional | No se crea el botón de inicio. |
| `stop(id)` | opcional | No se crea el botón de parada. |
| `closePosition(id)` | opcional | En la columna de posición solo queda el número. |
| `openStrategy(id)` | opcional | No se crea el botón para ir a la estrategia. |
| `riskRules(id)` | opcional | No se crea el botón de reglas de riesgo. |
| `setTradingMode(id, mode)` | opcional | El modo de negociación se muestra como texto. |
| `tradingModes` | opcional | Lista vacía: no se crea la lista desplegable de modos. |

Este conjunto permite montar un panel de solo lectura: si no se pasa ninguna función de acción, la tabla muestra los datos y no presenta ningún botón.

Las cadenas de `tradingModes` pasan por `host.t`, es decir, sirven como claves de traducción. Pertenecen al anfitrión y no al paquete, por lo que su ausencia en `translation-keys.json` es normal y quien las traduce es el anfitrión.

## Estados y acciones

La celda de estado consta de un punto y una palabra: el punto se lee al recorrer rápidamente la lista y la palabra distingue `Starting` de `Started`. Si la fila tiene relleno el campo `error`, el texto del error aparece en la ayuda emergente tanto del punto como de la palabra, y una estrategia detenida por un fallo se marca con la palabra «Error» en lugar de «Detenida».

Los botones de la fila se crean únicamente para las funciones que ha pasado el anfitrión y se habilitan solo donde el estado lo permite:

- iniciar: solo en una estrategia en estado `Stopped`;
- detener: solo en una estrategia en estado `Started`;
- cerrar la posición: solo en una estrategia en marcha con posición distinta de cero;
- reglas de riesgo e ir a la estrategia: siempre.

La lista desplegable del modo de negociación solo está activa en una estrategia detenida: el modo determina con qué se pondrá en marcha la estrategia y no es una palanca para usar durante la negociación. Cambiar el modo llama a `setTradingMode`; el control no modifica por sí mismo el valor de la fila y espera al siguiente `update`.

## Columnas y presentación

La tabla muestra el estado, las acciones, el indicador de conexión, el modo de negociación, el nombre, la cartera, el instrumento, la posición, el número de órdenes y de operaciones, la variación del beneficio, el gráfico de beneficio, el beneficio realizado y no realizado, y el error. El indicador de conexión es global: una estrategia se considera en línea solo si está formada y conectada, y eso lo decide el proveedor de datos.

Las clases de color de la posición, de la variación del beneficio y de ambas magnitudes de beneficio las devuelve `host.presentation.pnlClass`. La variación del beneficio se marca además con una flecha de dirección; si la variación es cero, no hay flecha.

La columna del gráfico dibuja la curva del beneficio acumulado a partir de los puntos `pnl` en un área de 140 × 26 píxeles CSS. El lienzo se crea teniendo en cuenta `devicePixelRatio`, por lo que la línea se mantiene nítida en pantallas de alta densidad. Los colores provienen de `host.presentation.canvasPalette()` y la curva se colorea según el resultado final de la ejecución: una estrategia que alcanzó un máximo y lo devolvió todo se muestra como perdedora. Sin puntos `pnl`, la celda queda vacía.

La ordenación predeterminada es por nombre de forma ascendente: la lista se lee de arriba abajo buscando una estrategia concreta, y que las filas se reordenen siguiendo al beneficio estorba esa lectura. El panel también admite selección de varias filas, menú contextual, filtros y exportación a XLSX.

## Qué queda a cargo del anfitrión

El control no inicia ni detiene estrategias, no envía órdenes ni cierra posiciones: llama a las funciones que se le han pasado y espera una nueva lista de filas.

El valor `pnlChange` el control lo acepta tal cual: el punto de referencia lo elige el proveedor de datos. Así, una estrategia reiniciada no sigue calculando la variación desde antes del reinicio.

El botón de cierre del panel llama a `host.close()` y la exportación vuelca el archivo `strategies`. La instancia se registra en el anfitrión al crearse y se da de baja en `dispose`.

## Métodos públicos

- `StrategiesWidget.create(hostEl, state, deps)`: construye el panel y lo añade al contenedor.
- `update(rows)`: sustituye toda la lista de estrategias.
- `dispose()`: da de baja el registro, elimina la tabla y libera los recursos.

## Véase también

- [Controles de negociación JavaScript](../trading_controls.md)
- [Posiciones](positions.md)
- [Órdenes activas](active_orders.md)
- [Historial de operaciones](trade_history.md)
