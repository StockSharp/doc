# Diagrama en JavaScript

[StockSharp JS Diagram](https://github.com/StockSharp/Diagram) es un componente de navegador independiente y sin dependencias que renderiza el esquema visual de estrategia de [Designer](../../designer.md) —el mismo diagrama de bloques de elementos conectados— en un `canvas` HTML. Se publica en npm como [@stocksharp/diagram](https://www.npmjs.com/package/@stocksharp/diagram) y da vida a los diagramas de estrategia de solo lectura que se muestran en los sitios web de StockSharp.

Una estrategia se describe como un **esquema**: un conjunto de *nodos* (elementos como una fuente de velas, un indicador, una condición o una orden) conectados entre sí mediante *puertos* tipados. El componente toma ese esquema más una *paleta* (el catálogo de tipos de elementos, sus puertos y colores) y lo dibuja.

## Demostración en vivo

El diagrama de abajo es el motor real ejecutándose en esta página: un esqueleto mínimo de estrategia «fuente de datos → indicador → gráfico». Arrastra el canvas para desplazarte, usa la rueda para hacer zoom y pulsa el botón de expandir para abrirlo a pantalla completa.

```diagram-demo sma
```

Los tres bloques son una fuente de **Candles** que alimenta un **Indicator** (una media móvil simple); tanto las velas como la salida del indicador se dibujan en un elemento **Chart**. Este es el patrón completo más pequeño de Designer: producir datos, transformarlos y visualizarlos.

## Instalación

Instala el paquete desde npm:

```bash
npm install @stocksharp/diagram
```

Luego importa los módulos ES: `import { renderScheme } from '@stocksharp/diagram/embed'` para la incrustación de solo lectura, o `import { StockSharpDiagram } from '@stocksharp/diagram'` para el [editor interactivo](javascript_diagram/editor.md).

## Incrustar un diagrama

El componente expone `renderScheme(host, paletteUrl, scheme)` desde el punto de entrada `@stocksharp/diagram/embed`. Proporciónale un elemento anfitrión, la URL de un JSON de paleta y un esquema construido a partir de `nodes` y `links`:

```js
import { renderScheme } from '@stocksharp/diagram/embed';

const scheme = {
  nodes: [
    { id: 'candles', typeId: 'CandleElement',    name: 'Candles', x: 60,  y: 130 },
    { id: 'sma',     typeId: 'IndicatorElement', name: 'SMA',     x: 340, y: 60  },
    { id: 'chart',   typeId: 'ChartElement',     name: 'Chart',   x: 620, y: 130 },
  ],
  links: [
    { from: 'candles', fromPort: 'Output', to: 'sma',   toPort: 'Input' },
    { from: 'sma',     fromPort: 'Output', to: 'chart', toPort: 'Input' },
    { from: 'candles', fromPort: 'Output', to: 'chart', toPort: 'Input' },
  ],
};

renderScheme(document.getElementById('diagram'), '/data/designer-palette.json', scheme);
```

El `typeId` de cada nodo debe existir en la paleta; los tipos desconocidos se renderizan como bloques de marcador de posición. Los puertos se referencian por su `key`, y un enlace es válido cuando el tipo del puerto de origen es compatible con el tipo del puerto de destino. `renderScheme` es de solo lectura: el motor distribuye, aplica el tema (sigue la configuración clara/oscura de la página) y permite al espectador desplazarse, hacer zoom y expandir, pero no edita el esquema.

El mismo componente también funciona como un **editor** completo: arrastra elementos desde una paleta, conecta puertos, edita y elimina nodos, deshaz/rehaz. Consulta [Editor interactivo](javascript_diagram/editor.md) y [Eventos y API](javascript_diagram/events.md).

## Véase también

- [Editor interactivo](javascript_diagram/editor.md)
- [Eventos y API](javascript_diagram/events.md)
- [Gráficos en JavaScript](charts/javascript_charts.md)
- [Designer](../../designer.md) — el editor visual de estrategias de escritorio
- [Repositorio del diagrama](https://github.com/StockSharp/Diagram)
