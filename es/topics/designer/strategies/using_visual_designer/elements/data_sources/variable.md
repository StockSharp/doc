# Variable

![Designer Variable 00](../../../../../../images/designer_variable_00.png)

El cubo se usa para almacenar valores y pasar el valor previamente almacenado más adelante por la cadena de elementos.

### Sockets de entrada

Sockets de entrada

- **Any data** – valor del tipo seleccionado, que se almacenará en lugar del valor predeterminado.
- **Trigger** – señal (cualquier valor excepto `False`) que determina el punto en el que debe pasar el valor almacenado por el socket de salida.

### Sockets de salida

Sockets de salida

- **Any data** – valor del tipo seleccionado de datos pasados.

### Parámetros

Parámetros

- **Data type** - tipo de datos almacenados dentro de la variable; el tipo de los parámetros de entrada y salida depende del tipo de dato seleccionado.
- **Value** - valor predeterminado almacenado en la variable. Este valor se usa si no se recibieron otros valores en la entrada del elemento.
- **Raise on start** - cuando la casilla está seleccionada, el valor se pasará al iniciar la estrategia.

Si se selecciona el tipo de dato **Instrument** o **Portfolio**, el valor predeterminado puede faltar. En este caso, si la bandera **Parameters** está establecida en las propiedades, al ejecutarse la estrategia estos datos se tomarán de las propiedades correspondientes de la estrategia.

## Contenido recomendado

[Indexador](../converters/indexer.md)
