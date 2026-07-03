# Depuración

Durante el proceso de prueba de una estrategia, a menudo es necesario comprobar qué datos llegan a la entrada de un cubo concreto o se pasan por su salida. Para ello, [Designer](../../designer.md) incluye el **Debugger**.

![Designer Debug 00](../../../images/designer_debug_00.png)

Los siguientes botones se encuentran en el grupo **Debugger** de la cinta **Emulation**:

- ![Designer Debug 01](../../../images/designer_debug_01.png)**Add breakpoint** – añade un punto de interrupción en el elemento seleccionado. Los elementos a los que se ha añadido un punto de interrupción se resaltan con un borde rojo.
- ![Designer Debug 02](../../../images/designer_debug_02.png)**Delete breakpoint** – elimina un punto de interrupción.
- ![Designer Debug 03](../../../images/designer_debug_03.png)**Next element** – cuando se activa el punto de interrupción, pasa al siguiente elemento del esquema.
- **Step to out** – cuando se activa el punto de interrupción, pasa a la salida del elemento actual; se usa para comprobar los valores pasados en la salida del elemento.
- ![Designer Debug 04](../../../images/designer_debug_04.png)**Step in** – cuando se activa el punto de interrupción, entra en el elemento compuesto. Abre automáticamente el esquema del elemento compuesto y se detiene en el elemento al que se transmiten los datos primero.
- ![Designer Debug 05](../../../images/designer_debug_05.png)**Step out** – cuando se activa el punto de interrupción y se está dentro del elemento compuesto, sale un nivel hacia arriba, donde se usa el elemento compuesto abierto.
- ![Designer Debug 06](../../../images/designer_debug_06.png)**Continue** – continúa hasta que se active el siguiente punto de interrupción.

## Contenido recomendado

[Puntos de interrupción](debugging/break_points.md)
