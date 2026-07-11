# Variable

![Captura de Variable](../../../../../../images/designer_variable_00.png)

El cubo se usa para almacenar valores y pasar el valor previamente almacenado más adelante por la cadena de elementos.

### Sockets de entrada

Sockets de entrada

- **Cualquier dato** – valor del tipo seleccionado, que se almacenará en lugar del valor predeterminado.
- **Activador** – señal (cualquier valor excepto `False`) que determina el punto en el que debe pasar el valor almacenado por el socket de salida.

### Sockets de salida

Sockets de salida

- **Cualquier dato** – valor del tipo seleccionado de datos pasados.

### Parámetros

Parámetros

- **Tipo de datos** - tipo de datos almacenados dentro de la variable; el tipo de los parámetros de entrada y salida depende del tipo de dato seleccionado.
- **Valor** - valor predeterminado almacenado en la variable. Este valor se usa si no se recibieron otros valores en la entrada del elemento.
- **Activar al iniciar** - cuando la casilla está seleccionada, el valor se pasará al iniciar la estrategia.

Si se selecciona el tipo de dato **Instrumento** o **Cartera**, el valor predeterminado puede faltar. En este caso, si la bandera **Parámetros** está establecida en las propiedades, al ejecutarse la estrategia estos datos se tomarán de las propiedades correspondientes de la estrategia.

## Contenido recomendado

[Indexador](../converters/indexer.md)
