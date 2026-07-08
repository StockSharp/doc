# Configuración de backtesting

El panel **Properties** está minimizado de forma predeterminada en el lado derecho de la pestaña de estrategia. Este panel es una tabla de propiedades de emulación o Live trade. Cuando selecciona una propiedad concreta, aparece una descripción detallada de esta propiedad en la parte inferior de la tabla. Todas las propiedades están agrupadas:

![Designer Properties emulation 00](../../../../images/designer_properties_emulation_00.png)

**Settings**

- **Market data** – almacenamiento de datos.
- **Storage format** – formato de almacenamiento.
- **Data type** – tipo de datos.
- **Time frame** – usar velas con el marco temporal especificado.
- **Maximum quote volume in generated depth** – volumen máximo de cotización en el libro de órdenes generado.
- **Interval** – intervalo de tiempo.
- **Unrealized P\/L** – intervalo de recálculo del beneficio no realizado.
- **Trades** – qué operaciones usar.
- **Marked depth** – qué libros de órdenes usar.
- **Order log** – usar el registro de órdenes.
- **Number of strategies** – número de estrategias probadas simultáneamente.
- **Logging level** – nivel de registro.
- **Combine on touch** – durante la emulación, combinar órdenes cuando el precio de la operación toca el precio de la orden (es decir, es igual al precio de la orden).
- **Marked depth (lifetime)** – tiempo máximo durante el cual el libro de órdenes está en el emulador. Si durante este tiempo no hubo actualización, el libro de órdenes se elimina. Esta propiedad puede usarse para eliminar libros de órdenes antiguos cuando hay huecos en los datos.
- **Errors percentage** – valor porcentual del error al registrar nuevas órdenes. El valor puede ser de 0 (no habrá errores) a 100.
- **Latency** – valor mínimo de latencia de la orden registrada.
- **Reregistering** – si se admite el nuevo registro de órdenes como una sola operación.
- **Buffering period** – enviar respuestas en un único paquete por intervalos. Se emulan la latencia de red y la operación en búfer del núcleo del exchange.
- **Order ID** – número desde el que el emulador generará identificadores de órdenes.
- **Trade ID** – número desde el que el emulador generará identificadores de operaciones.
- **Transaction** – número desde el que el emulador generará identificadores de transacciones de órdenes.
- **Spread size** – tamaño del spread en pasos de precio. Se usa al especificar el spread para generar un libro de órdenes a partir de operaciones tick.
- **Depth of book** – profundidad máxima del libro de órdenes que se generará a partir de ticks.
- **Number of volume steps** – número de pasos de volumen por los que la orden es mayor que la operación tick. Se usa para pruebas con operaciones tick.
- **Portfolios interval** – intervalo de recálculo de cartera. Si el intervalo es cero, no se realiza recálculo.
- **Change time** – cambiar la hora de órdenes y operaciones a hora del exchange.
- **Time zone** – información sobre la zona horaria donde se encuentra el exchange.
- **Price shift** – desplazamiento de precio desde la última operación, que especifica los límites de precios máximo y mínimo para la siguiente sesión.
- **Add extra volume** – añadir volumen extra al libro de órdenes al registrar órdenes de gran volumen.
- **[Comisiones](../commissions.md)** – comisión (brokerage, exchange, etc.).

**Logging**

- **Logging level** – nivel de registro para este elemento.

**Setting**

- **[Gestión de riesgos](../risk_management.md)** – configuración de gestión de riesgos.

**Diagram parameters**

- **Security** - instrumento.
- **Portfolio** - cartera.

Si no rellena los **Diagram parameters**, durante la emulación se usará el instrumento del campo **Instrument** de la pestaña **Emulation**, y como cartera se usará por defecto la cartera de prueba.

## Contenido recomendado

[Gráfico](chart.md)
