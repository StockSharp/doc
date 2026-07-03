# Gestión de riesgos

En los paneles [Testing Properties](components/backtesting_settings.md) y [Live Trading Properties](components/live_settings.md), puede establecer la configuración de control de riesgos.

En la ventana Risks, es necesario seleccionar una **Risk Rule**, configurar la condición de activación para la **Risk Rule** y la acción (cerrar posiciones, detener trading, cancelar órdenes) que se ejecutará cuando ocurra la condición de la **Risk Rule**.

Es posible usar varias reglas de riesgo del mismo tipo con distintas acciones. Por ejemplo, en la captura siguiente, si el volumen de la orden es 20, se ejecutan las acciones de cancelar órdenes y detener el trading.

![Designer Risk Rule](../../../images/designer_risk_rule.png)

### Lista de reglas de riesgo

Lista de reglas de riesgo

- **P/L** - regla de riesgo que monitorea el tamaño del beneficio/pérdida.
- **Position** - regla de riesgo que monitorea el tamaño de la posición.
- **Position (Time)** - regla de riesgo que monitorea la vida útil de una posición.
- **Commission** - regla de riesgo que monitorea el tamaño de la comisión.
- **Slippage** - regla de riesgo que monitorea el importe del slippage.
- **Order Price** - regla de riesgo que monitorea el precio de una orden.
- **Order Volume** - regla de riesgo que monitorea el volumen de una orden.
- **Order (Frequency)** - regla de riesgo que monitorea la frecuencia de colocación de órdenes.
- **Error in Registration/Cancellation of Order** - regla de riesgo que monitorea el número de errores durante el registro/cancelación de órdenes.
- **Trade Price** - regla de riesgo que monitorea el precio de una operación.
- **Trade (Volume)** - regla de riesgo que monitorea el volumen de una operación.
- **Trade (Frequency)** - regla de riesgo que monitorea la frecuencia de realización de operaciones.
- **Error** - regla de riesgo que monitorea el número de cualquier error.
