# Registro de orden

![Designer Apertura de posición 00](../../../../../../images/designer_position_opening_00.png)

El componente "Registro de orden" se usa para colocar órdenes de trading para un instrumento seleccionado.

## Conectores de entrada

- **Instrumento** – instrumento seleccionado para la orden.
- **Precio** – especifica el precio de una orden limitada.
- **Activador** – señal de activación para la orden; acepta cualquier valor excepto `False`.
- **Volumen** – cantidad de instrumentos para la orden.
- **Cartera** – cartera dentro de la cual se colocará la orden.

## Conectores de salida

- **Orden** – información sobre la orden colocada.
- **Error** – información sobre cualquier error durante el registro de la orden.
- **Transacción** – información sobre la transacción realizada sobre la orden.
- **Cancelación** – señal de que la orden fue cancelada.
- **Ejecutado** – señal de que la orden fue completamente ejecutada.
- **Completado** – señal que combina eventos de error, cancelación o ejecución completa de la orden.

## Parámetros

- **Dirección** – determina si la orden es de compra o venta.
- **Orden de mercado** – indica si la orden es de mercado.
- **Precio cero** – si el precio se establece en cero, la orden se registra como orden de mercado.
- **Vigencia** – duración durante la cual una orden limitada permanece activa.

## Configuración de orden condicional

**Orden condicional** – orden con condiciones adicionales que determinan el momento de colocación en el sistema de trading según la situación actual del mercado.

![Captura de Registro de orden](../../../../../../images/designer_conditional_application.png)

- **Conexión** – conexión donde se colocará la orden.
- **Tipo de orden stop** – tipo de orden stop.
- **Resultado** – resultado de la orden stop ejecutada.
- **Identificador de instrumento** – identificador del instrumento para órdenes stop con condiciones relacionadas con otro instrumento.
- **Condición de precio stop** – condición de precio stop. Se usa para órdenes como "precio stop para otro instrumento".
- **Precio stop** – precio stop que establece la condición para activar la orden stop.
- **Precio stop-limit** – similar al precio stop, pero se usa solo para órdenes de tipo "toma de beneficios y stop-limit".
- **Stop-limit a precio de mercado** – indica si la orden "Stop-Limit" se ejecuta al precio de mercado.
- **Intervalo de comprobación de condición** – intervalo de tiempo para comprobar las condiciones de la orden solo dentro del período especificado (si es null, no hay comprobaciones). Se usa para los tipos "toma de beneficios y stop-limit" y "toma de beneficios y stop-limit por orden".
- **Identificador de ejecución de orden condicional** – identificador de la orden condicional basada en ejecución.
- **Dirección de orden condicional por ejecución** – dirección de la orden condicional basada en ejecución.
- **Activación en ejecución parcial** – se tiene en cuenta la ejecución parcial de la orden. Una orden "por ejecución" se activará tras la ejecución parcial de la orden de condición.
- **Volumen ejecutado** – toma el volumen ejecutado de la orden como cantidad para colocar la orden stop. La cantidad de valores en una orden "por ejecución" se toma como el volumen ejecutado de la orden de condición.
- **Precio de la orden vinculada** – precio de la orden limitada vinculada.
- **Retirada en ejecución parcial** – indica la retirada de la orden stop tras la ejecución parcial de la orden limitada vinculada.
- **Desplazamiento desde el máximo** – cantidad de desplazamiento desde el precio máximo (mínimo) de la última transacción.
- **Spread de protección** – tamaño del spread protector.
- **Take-profit a precio de mercado** – indica si la orden "Take-Profit" se ejecuta al precio de mercado.

## Nota

Trabajar con órdenes es un método de bajo nivel para gestionar posiciones. Para una gestión de nivel superior, se recomienda usar el componente "Modificar posición", descrito en [Modificar posición](../positions/modify.md).

## Véase también

[Modificar posición](../positions/modify.md)
