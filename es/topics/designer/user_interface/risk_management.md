# Gestión de riesgos

En los paneles [Configuración de backtesting](components/backtesting_settings.md) y [Configuración live](components/live_settings.md), puede establecer la configuración de control de riesgos.

En la ventana Riesgos, es necesario seleccionar una **Regla de riesgo**, configurar la condición de activación para la **Regla de riesgo** y la acción (cerrar posiciones, detener trading, cancelar órdenes) que se ejecutará cuando ocurra la condición de la **Regla de riesgo**.

Es posible usar varias reglas de riesgo del mismo tipo con distintas acciones. Por ejemplo, en la captura siguiente, si el volumen de la orden es 20, se ejecutan las acciones de cancelar órdenes y detener el trading.

![Designer Risk Rule](../../../images/designer_risk_rule.png)

### Lista de reglas de riesgo

Lista de reglas de riesgo

- **P/L** - regla de riesgo que monitorea el tamaño del beneficio/pérdida.
- **Posición** - regla de riesgo que monitorea el tamaño de la posición.
- **Posición (tiempo)** - regla de riesgo que monitorea la vida útil de una posición.
- **Comisión** - regla de riesgo que monitorea el tamaño de la comisión.
- **Deslizamiento** - regla de riesgo que monitorea el importe del slippage.
- **Precio de orden** - regla de riesgo que monitorea el precio de una orden.
- **Volumen de orden** - regla de riesgo que monitorea el volumen de una orden.
- **Orden (frecuencia)** - regla de riesgo que monitorea la frecuencia de colocación de órdenes.
- **Error al registrar/cancelar la orden** - regla de riesgo que monitorea el número de errores durante el registro/cancelación de órdenes.
- **Precio de operación** - regla de riesgo que monitorea el precio de una operación.
- **Operación (volumen)** - regla de riesgo que monitorea el volumen de una operación.
- **Operación (frecuencia)** - regla de riesgo que monitorea la frecuencia de realización de operaciones.
- **Error** - regla de riesgo que monitorea el número de cualquier error.
