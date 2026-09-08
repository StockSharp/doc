# Estadísticas

`StatisticsWidget` es una tabla con los parámetros estadísticos de la estrategia: beneficio, caída máxima, número de operaciones, latencias. El panel es de solo lectura: una fila por parámetro, con las filas agrupadas según el área a la que pertenece cada parámetro.

![Panel de estadísticas de la ejecución con los indicadores agrupados](../../../../images/javascript_controls_statistics.png)

## Creación y actualización

```ts
import {
  StatisticsWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const statistics = StatisticsWidget.create(
  document.querySelector<HTMLElement>('#statistics')!,
  {},
  { host },
);

statistics.update([
  {
    key: 'NetProfit',
    category: 'pnl',
    categoryText: 'Beneficio',
    order: 1,
    name: 'Beneficio neto',
    description: 'Resultado de la ejecución',
    value: 11_055.75,
  },
  {
    key: 'MaxProfitDate',
    category: 'pnl',
    categoryText: 'Beneficio',
    order: 2,
    name: 'Fecha del máximo',
    value: '2024-03-26T07:30:00Z',
  },
  {
    key: 'NetProfit',
    category: 'pnl',
    categoryText: 'Operaciones',
    order: 100,
    name: 'Número de operaciones',
    value: 1_340,
  },
]);
```

El conjunto de dependencias `StatisticsDeps` consta de un único campo obligatorio, `host`. El control no tiene manejadores de acciones: las estadísticas las genera la estrategia, y en el panel no hay nada que cancelar, recargar ni editar. El segundo argumento de `create` es el estado guardado del panel; el control no lo utiliza.

`update` sustituye por completo el conjunto de filas. La estrategia publica sus parámetros como una única tabla, por lo que una fila que desaparece del conjunto se considera que ha dejado de existir, no que ha dejado de cambiar. La identidad de una fila la determina el campo `key`.

## Filas y orden

Una fila se describe con el tipo `StatisticRow`:

| Campo | Propósito |
|---|---|
| `key` | Identificador estable del parámetro. |
| `category` | Clave del grupo, independiente del idioma. |
| `categoryText` | Título del grupo que se muestra. Si falta, se utiliza `category`. |
| `order` | Posición del parámetro en el registro. |
| `name` | Nombre localizado del parámetro. |
| `description` | Explicación localizada, se muestra como ayuda emergente en la celda del nombre. |
| `value` | Número, fecha o cadena. `null` significa que el parámetro aún no se ha medido. |

La versión de escritorio de la tabla obtiene las filas por reflexión sobre los parámetros de la estrategia; en el navegador no existe ese mecanismo, por lo que las filas llegan del anfitrión ya preparadas, con el nombre y la descripción traducidos.

La agrupación se realiza por `category` y no por `categoryText`: agrupar por el título traducido reconstruiría la tabla al cambiar de idioma. El orden de los grupos lo determina el menor `order` entre sus parámetros, por lo que el beneficio va por encima de la caída máxima y esta por encima de los contadores de operaciones. Ordenar por nombre o por valor separaría parámetros que se leen juntos.

Hay dos columnas visibles: `Name` y `Value`. Las columnas `category` y `order` están declaradas, pero ocultas: sirven para agrupar y ordenar, y no le dicen nada al lector. La exportación incluye únicamente las dos columnas visibles.

## Formato de los valores

El texto de la celda de valor lo genera la función exportada `formatStatistic(value)`:

```ts
import { formatStatistic } from '@stocksharp/trading-controls';

formatStatistic(11_055.756); // '11055.76'
formatStatistic(1_340);      // '1340'
formatStatistic('2024-03-26T07:30:00Z'); // '2024-03-26'
formatStatistic(null);       // ''
```

Los números se redondean a dos decimales y se muestran sin ceros a la derecha. De la fecha solo se muestra el día: los parámetros de este tipo describen la ejecución completa, y la hora sería ruido. Una cadena se reconoce como fecha únicamente si empieza por `AAAA-MM-DD`; en caso contrario sigue siendo texto. Un valor ausente deja la celda vacía en lugar de un cero, que se leería como un resultado medido.

La ordenación se realiza sobre el valor original, de modo que un número se ordena como número y no como cadena.

## Qué hace el control y qué hace el anfitrión

El control obtiene del anfitrión todo el texto visible mediante `host.t`, incluidos los encabezados de columna, el título del panel, el texto de tabla vacía y las entradas del menú contextual de la tabla. El botón de cierre llama a `host.close` y la exportación vuelca la tabla a XLSX. Al crearse, la instancia se registra mediante `host.register` y, en `dispose`, se da de baja con `host.unregister`.

El control no guarda ajustes propios en `host.preferences`. Tampoco solicita datos: las filas las suministra el anfitrión mediante `update`.

El identificador del tipo de panel está disponible como `StatisticsWidget.TYPE` y es igual a `ControlTypes.Statistics`.

## Métodos públicos

- `StatisticsWidget.create(hostEl, state, deps)`: crea el panel en el contenedor indicado.
- `update(rows)`: sustituye todo el conjunto de filas de estadísticas.
- `dispose()`: libera los recursos.

El panel también admite ordenación, selección de varias filas, menú contextual y exportación a XLSX.

## Véase también

- [Controles de negociación JavaScript](../trading_controls.md)
- [Posiciones](positions.md)
- [Historial de operaciones](trade_history.md)
- [Órdenes activas](active_orders.md)
