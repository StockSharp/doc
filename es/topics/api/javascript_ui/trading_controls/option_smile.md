# Sonrisa de volatilidad

`OptionSmileWidget` dibuja la sonrisa de volatilidad de una serie de opciones: la volatilidad implícita de las call y de las put por strike, dos líneas en una misma escala. El gráfico lo construye el motor del paquete `@stocksharp/chart`, declarado como dependencia de pares (peer).

![Sonrisa de volatilidad por los strikes de la cadena de opciones](../../../../images/javascript_controls_option_smile.png)

## Creación y actualización

```ts
import {
  OptionSmileWidget,
  type OptionStrike,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const smile = OptionSmileWidget.create(
  document.querySelector<HTMLElement>('#smile')!,
  {},
  { host },
);

const chain: OptionStrike[] = [
  { strike: 67_000, call: { ivLast: 0.52 }, put: { ivBid: 0.55, ivAsk: 0.57 } },
  { strike: 67_500, call: { ivLast: 0.48 }, put: { ivLast: 0.50 } },
  { strike: 68_000, call: { ivBid: 0.45, ivAsk: 0.47 }, put: { ivLast: 0.47 } },
  { strike: 69_000, call: { ivLast: 0.49 }, put: {} },
];

smile.update(chain, { assetPrice: 68_120 });
```

La única dependencia es `host`; la interfaz `OptionSmileDeps` no contiene otros campos. El segundo argumento de `create` es el estado guardado de la instancia: la sonrisa no lo lee ni escribe nada en él.

`update` sustituye toda la cadena por completo. El segundo parámetro, `context`, es opcional y de forma predeterminada es un objeto vacío; se transmite con el mismo tipo `OptionChainContext` que en el tablero de opciones, pero la sonrisa solo necesita de él el precio del activo subyacente, `assetPrice`.

La propiedad estática `OptionSmileWidget.TYPE` es igual a `ControlTypes.OptionSmile`, es decir, al identificador `optionSmile`.

## Datos y presentación

Los strikes se ordenan de forma ascendente y se descartan los registros con un `strike` no numérico. La volatilidad de un lado se toma de `ivLast` y, si no ha habido operaciones, como la media de `ivBid` y `ivAsk`; solo se tienen en cuenta valores finitos y positivos. Si no hay ninguno de ellos, el punto no se dibuja: el strike permanece en el eje y la línea se interrumpe, de modo que se ve el strike cotizado por un solo lado. En el gráfico los valores se muestran en porcentaje.

El eje de strikes funciona en modo `ordinal`: los pasos son uniformes según el listado, no según la distancia entre los números, por lo que el salto entre 67_500 y 68_000 no se convierte en un hueco. Las etiquetas del eje y la marca del crosshair las genera un mismo formateador de precio.

El precio del activo subyacente no se dibuja como línea —en un eje ordinal no hay un escalón para él—, sino que se muestra como texto en la leyenda junto a las claves `Call` y `Put`. Al pasar el puntero sobre el gráfico aparece una línea con el strike y los valores de ambos lados en él: la sonrisa se lee por la distancia entre las curvas, así que se muestran las dos.

Mientras no haya ningún strike cotizado, en lugar del gráfico se muestra un marcador de posición con el texto de la clave `NoOptions`.

## Qué hace el control y qué queda a cargo del anfitrión

El control crea el gráfico por sí mismo con los primeros datos, obtiene los colores, la fuente y el color de la cuadrícula de `host.presentation.canvasPalette()` (`up` es call y `down` es put), vigila el tamaño del contenedor mediante `ResizeObserver` y ajusta el lienzo, y atiende el botón de restablecer el zoom y el botón de cierre del panel, que llama a `host.close()`. Todo el texto visible se solicita mediante `host.t`: `OptionSmile`, `ResetView`, `ClosePanel`, `ImpliedVolatility`, `Call`, `Put`, `OptionChain`, `NoOptions`, `Underlying`.

El anfitrión suministra los datos: la sonrisa no crea suscripciones de mercado, no calcula la volatilidad y no distingue una serie de otra; dibuja lo que se le pasa en `update`. El control no guarda ajustes propios en `host.preferences` y no tiene claves.

## Métodos públicos

- `update(strikes, context)`: muestra la cadena y el contexto en que se tomó.
- `resetZoom()`: devuelve la vista de toda la cadena tras hacer zoom.
- `chart()`: devuelve el objeto del gráfico (`IChartApi`) o `null` si el gráfico aún no se ha creado; lo necesita el anfitrión que añade una segunda serie o un marcador al mismo lienzo.
- `dispose()`: desconecta el observador de tamaño, elimina el gráfico, llama a `host.unregister` y retira el elemento raíz.

## Funciones auxiliares

El paquete exporta también las funciones sobre las que se apoya el dibujado: pueden utilizarse por separado.

- `sideVolatility(side)`: la volatilidad de un lado, o `null` si se desconoce.
- `sortedChain(strikes)`: la cadena en el orden de dibujado, por strike ascendente y sin registros incorrectos.
- `toSmileSeries(chain, put)`: un lado en forma de conjunto de puntos para el gráfico.

## Véase también

- [Controles de negociación JavaScript](../trading_controls.md)
- [Tablero de opciones](option_desk.md)
- [Curva de capital](equity.md)
- [Libro de órdenes](order_book.md)
