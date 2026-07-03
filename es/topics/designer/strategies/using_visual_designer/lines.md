# Líneas

La estrategia en el diseñador es un esquema de un conjunto de elementos y enlaces entre ellos, llamados conexiones. Cada conexión va desde el parámetro de salida de un cubo al parámetro de entrada de otro cubo. Normalmente, todas las líneas de conexión son grises, pero cuando apunta al cubo al que pertenecen, las líneas se pintan de negro.

![Designer Line 00](../../../../images/designer_line_00.png)

Cada conexión puede resaltarse apuntando sobre ella y haciendo clic con el botón izquierdo del ratón. La conexión seleccionada se marcará con círculos en los extremos de la línea, tomándolos puede redirigir la línea. Si pulsa el botón Del sobre la línea seleccionada, se eliminará.

Puede conectar entre sí parámetros de los mismos colores (los mismos tipos de datos), excepto los siguientes tipos de parámetros:

- El parámetro **negro** puede aceptar cualquier dato. Con mayor frecuencia, estos parámetros se usan para pasar señales para cualquier acción dentro del elemento. Por ejemplo, el elemento [Variable](elements/data_sources/variable.md) almacena algún valor y, cuando recibe una señal, pasa el valor a la salida.
- El parámetro **verde** puede aceptar distintos tipos de datos comparados. Por ejemplo, numéricos, valores de indicadores, cadenas, etc.

## Contenido recomendado

[Modelo de eventos](event_model.md)
