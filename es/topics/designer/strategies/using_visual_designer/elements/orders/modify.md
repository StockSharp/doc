# Movimiento de orden

![Designer Moving applications 00](../../../../../../images/designer_moving_applications_00.png)

Este bloque se usa para modificar una orden de un instrumento.

### Sockets de entrada

Sockets de entrada

- **Trigger** - señal que determina cuándo mover una orden.
- **Order** - orden que se modificará.
- **Price** - valor numérico del nuevo precio.
- **Volume** - valor numérico del nuevo volumen.

### Sockets de salida

Sockets de salida

- **Order** - orden modificada, que puede usarse para obtener sus transacciones mediante el elemento **Transactions by Order** y para mostrarla en el gráfico usando el bloque **Chart Panel**.
- **Error** - error al mover la orden.
- **Trade** - operación de la orden colocada.

Parámetros

- **Zero Price** – un precio cero registra una orden de mercado.

## Véase también

[Cancelar orden](cancel.md)
