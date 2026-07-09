# Movimiento de orden

![Designer Moving applications 00](../../../../../../images/designer_moving_applications_00.png)

Este bloque se usa para modificar una orden de un instrumento.

### Sockets de entrada

Sockets de entrada

- **Activador** - señal que determina cuándo mover una orden.
- **Orden** - orden que se modificará.
- **Precio** - valor numérico del nuevo precio.
- **Volumen** - valor numérico del nuevo volumen.

### Sockets de salida

Sockets de salida

- **Orden** - orden modificada, que puede usarse para obtener sus transacciones mediante el elemento **Transacciones por orden** y para mostrarla en el gráfico usando el bloque **Panel de gráfico**.
- **Error** - error al mover la orden.
- **Operación** - operación de la orden colocada.

Parámetros

- **Precio cero** – un precio cero registra una orden de mercado.

## Véase también

[Cancelar orden](cancel.md)
