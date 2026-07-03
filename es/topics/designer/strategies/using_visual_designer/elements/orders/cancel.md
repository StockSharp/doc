# Cancelación de orden

![Designer Cancellations 00](../../../../../../images/designer_cancellations_00.png)

Este bloque se usa para cancelar una orden de un instrumento.

### Sockets de entrada

Sockets de entrada

- **Trigger** – evento que activa la cancelación de la orden.
- **Order** – señal usada para determinar el momento en que una orden debe cancelarse.

### Sockets de salida

Sockets de salida

- **Order** – orden cancelada, que puede usarse para recuperar sus transacciones mediante el elemento **Transactions**, así como para mostrarla en el gráfico usando el bloque **Chart Panel**.
- **Error** – error al cancelar la orden (por ejemplo, la orden ya se ejecutó o se canceló anteriormente).

## Véase también

[Cancelación masiva de órdenes](mass_cancel.md)
