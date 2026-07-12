# Cancelación de orden

![Captura de Cancelación de orden](../../../../../../images/designer_cancellations_00.png)

Este bloque se usa para cancelar una orden de un instrumento.

### Conectores de entrada

Conectores de entrada

- **Activador** – evento que activa la cancelación de la orden.
- **Orden** – señal usada para determinar el momento en que una orden debe cancelarse.

### Conectores de salida

Conectores de salida

- **Orden** – orden cancelada, que puede usarse para recuperar sus transacciones mediante el elemento **Transacciones**, así como para mostrarla en el gráfico usando el bloque **Panel de gráfico**.
- **Error** – error al cancelar la orden (por ejemplo, la orden ya se ejecutó o se canceló anteriormente).

## Véase también

[Cancelación masiva de órdenes](mass_cancel.md)
