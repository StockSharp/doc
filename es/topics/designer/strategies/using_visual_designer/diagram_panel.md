# Diseñador de estrategias

El proceso principal de diseño de una estrategia y sus elementos componentes se realiza en el panel **Esquema**, combinando bloques y líneas de conexión. El panel Esquema consta de los paneles **Paleta**, **Designer** y **Propiedades**.

![Designer Designer schemes strategies and component elements 00](../../../../images/designer_designer_schemes_strategies_and_component_elements_00.png)

## Panel Palette

El panel **Paleta** contiene bloques a partir de los cuales se crean estrategias. Todos los elementos de la paleta están divididos en categorías, descritas en la sección [Descripción de bloques](elements.md). Para añadir un bloque al panel **Designer**, haga clic con el botón derecho en el bloque requerido y, sin soltar el botón, arrástrelo al panel **Designer**. Después, el elemento se seleccionará automáticamente y sus parámetros se mostrarán en la ventana para editar las propiedades del bloque.

## Panel Designer

El panel **Designer** es donde ocurre todo el proceso de creación de una estrategia mediante la combinación de bloques y conexiones (líneas). Representa visualmente el esquema de la estrategia. La información detallada sobre la creación de una estrategia se describe en la sección [Creación de un algoritmo a partir de bloques](first_strategy.md).

## Panel Properties

El panel **Propiedades** muestra los parámetros del bloque seleccionado en el panel **Designer**. Cuando se selecciona un bloque en el panel **Designer**, su marco se colorea de negro.

![Designer The Properties Panel 00](../../../../images/designer_properties_panel_00.png)

El panel **Propiedades** puede mostrarse en dos modos: *Configuración básica* y *Configuración avanzada*.

De forma predeterminada, al construir un esquema, las propiedades se muestran inicialmente en modo *basic settings*. Para cambiar al modo *advanced settings*, debe hacer clic en el título correspondiente.

En el modo *configuración básica*, solo se muestran las propiedades más necesarias del bloque. Por ejemplo, para el bloque [Velas](elements/data_sources/candles.md), se mostrará el marco temporal, la bandera para recibir solo velas formadas, la bandera de posibilidad de construir velas a partir de un marco temporal menor y la bandera de suscripción a velas por señal.

En el modo *advanced settings*, se mostrarán todas las propiedades del bloque disponibles para cambio y configuración.

![Designer The Properties Panel 00](../../../../images/designer_properties_panel_01.png)

Todos los bloques contienen un conjunto de propiedades predefinidas que se hacen visibles en el modo *advanced settings*:

- **Nombre** – nombre del elemento mostrado en el diseñador.
- **Nivel de registro** – nivel de registro para este elemento.
- **Parámetros** – mostrar los parámetros del elemento en elementos de nivel superior.
- **Conectores** – mostrar los sockets del elemento en elementos de nivel superior.

La información detallada sobre las propiedades de cada bloque se describe en la sección [Descripción de bloques](elements.md).

## Véase también

[Descripción de bloques](elements.md)
