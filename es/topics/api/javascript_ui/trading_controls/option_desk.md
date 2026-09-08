# Tablero de opciones

`OptionDeskWidget` muestra una serie de la cadena de opciones: a la izquierda las call, a la derecha, en espejo, las put, y entre ambas el strike y el valor intrínseco. El volumen, el interés abierto y las volatilidades no se presentan solo como números, sino también como barras, de modo que la cadena se lee por su forma y no únicamente por los valores.

![Tablero de opciones con los lados call y put alrededor de los strikes](../../../../images/javascript_controls_option_desk.png)

## Creación y actualización

```ts
import {
  OptionDeskWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const desk = OptionDeskWidget.create(
  document.querySelector<HTMLElement>('#option-desk')!,
  {},
  { host },
);

desk.update(
  [{
    strike: 68_000,
    call: {
      symbol: 'BTC-68000-C',
      bid: 1_240,
      ask: 1_265,
      last: 1_250,
      theoretical: 1_248,
      volume: 320,
      openInterest: 1_480,
      ivBid: 0.42,
      ivAsk: 0.44,
      ivLast: 0.43,
      historicalVolatility: 0.39,
    },
    put: {
      symbol: 'BTC-68000-P',
      bid: 820,
      ask: 845,
      volume: 210,
      openInterest: 960,
      ivLast: 0.47,
    },
  }],
  {
    assetPrice: 68_420,
    timeToExpiry: 0.08,
    riskFree: 0.05,
    dividend: 0,
  },
);
```

La única dependencia es `host`; el control no tiene dependencias opcionales. El segundo argumento de `create` es el estado del panel, que el control no utiliza.

`update(strikes, context)` sustituye toda la cadena. Ambos argumentos se transmiten juntos: la cadena y el precio del activo subyacente son una misma observación, y actualizarlos por separado mostraría griegas calculadas a partir de un precio que ya se ha movido. `context` es opcional y está vacío de forma predeterminada.

## Contexto de la cadena

`OptionChainContext` describe respecto a qué se valora la serie: `assetPrice` es el precio del activo subyacente, `timeToExpiry` el tiempo hasta el vencimiento en años, y `riskFree` y `dividend` son tasas en fracciones (`0.05` son cinco por ciento). Sin `assetPrice`, el tablero sigue mostrando las cotizaciones, pero el valor intrínseco es cero y las filas no se separan entre «dentro del dinero» y «fuera del dinero». Sin `assetPrice` o sin `timeToExpiry` las griegas no se calculan y la celda queda vacía, no a cero.

## Griegas

Las griegas llegan de una de estas dos maneras. Si el anfitrión las calcula por su cuenta, transmite un objeto `greeks` ya preparado en el lado del strike, y el tablero muestra lo que ha recibido. Si el anfitrión envía la volatilidad, las griegas se calculan sobre la marcha por Black-Scholes a partir del primer valor disponible en el orden `ivLast`, `ivBid`, `ivAsk`, `historicalVolatility`. Ninguna de las dos opciones es el recurso de reserva de la otra: son dos formas de proceder del anfitrión.

El número de decimales se elige según los datos: cuatro cifras significativas para el menor valor de la columna, pero nunca menos de dos ni más de ocho decimales. El cálculo es único para toda la columna y común a ambos lados, de modo que la delta no se convierte en `0.0000` y la gamma y su columna espejo se escriben igual.

## Columnas y presentación

El orden de las columnas se establece desde el strike hacia fuera: las volatilidades y las cotizaciones más cerca del centro, y las griegas en los extremos; el lado put es lo mismo en orden inverso. La ordenación es por strike de forma ascendente: la cadena se lee como una escalera.

De forma predeterminada están ocultas las columnas `callRho`, `callTheta`, `callHv`, `callTheor` y sus columnas espejo `putRho`, `putTheta`, `putHv`, `putTheor`; el menú contextual de la tabla las recupera.

Las barras se escalan de manera distinta. El volumen y el interés abierto, por separado en cada lado, porque las call y las put se negocian con tamaños diferentes. Las volatilidades, con una única escala para ambos lados a la vez, ya que de otro modo desaparecería el desequilibrio entre lados. La barra se dibuja con el ancho del elemento, sin canvas.

A la fila se le asigna la clase `option-itm-call` para los strikes por debajo del precio del activo y `option-itm-put` para los demás; si falta `assetPrice`, solo `option-row`. Las volatilidades se muestran como porcentajes con dos decimales y los precios con el formato de precio general del paquete.

El tablero no guarda ajustes: no accede a `host.preferences` ni a `host.cache`, y el conjunto de columnas y la ordenación viven en la instancia actual.

## Qué hace el anfitrión

Todo el texto visible se obtiene de `host.t`: el título del panel, los encabezados de columna, el texto de tabla vacía y las entradas del menú contextual. El botón de cierre llama a `host.close()`: el panel no se elimina a sí mismo. Al crearse, el control llama a `host.register(this)` y, en `dispose()`, a `host.unregister(this)`. Un botón de la barra lateral vuelca la cadena a XLSX.

El control no se suscribe a datos ni envía órdenes: la cadena y el contexto se los transmite el anfitrión mediante `update`.

## Métodos públicos

- `OptionDeskWidget.create(hostEl, state, deps)`: construye el panel y lo añade al contenedor.
- `update(strikes, context)`: sustituye la cadena y el contexto de valoración.
- `rows()`: devuelve las filas tal como las almacena el tablero, con las escalas de las barras y el valor intrínseco ya calculados.
- `dispose()`: libera los recursos y da de baja el registro en el anfitrión.
- `OptionDeskWidget.TYPE`: identificador del tipo de control, `ControlTypes.OptionDesk`.

## Funciones exportadas

La parte de cálculo está disponible por separado del panel:

- `scaleChain(strikes, context)`: en una sola pasada por la cadena calcula los máximos de las barras y el valor intrínseco de cada strike.
- `sideGreeks(row, which, context)`: griegas de un lado del strike, ya sean las transmitidas por el anfitrión o las calculadas a partir de su volatilidad; `null` cuando ninguna de las dos es posible.
- `greekPlaces(values)`: número de decimales para una columna de valores.
- `greekScales(rows, context)`: número de decimales de cada griega, medido a la vez sobre ambos lados de la cadena.

## Véase también

- [Controles de negociación JavaScript](../trading_controls.md)
- [Lista de seguimiento](watchlist.md)
- [Libro de órdenes](order_book.md)
- [Posiciones](positions.md)
