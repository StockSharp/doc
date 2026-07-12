# Configuración de pruebas históricas

El panel **Propiedades** está minimizado de forma predeterminada en el lado derecho de la pestaña de estrategia. Este panel es una tabla de propiedades de emulación o trading en vivo. Cuando selecciona una propiedad concreta, aparece una descripción detallada de esta propiedad en la parte inferior de la tabla. Todas las propiedades están agrupadas:

![Designer Propiedades de emulación 00](../../../../images/designer_properties_emulation_00.png)

**Configuración**

- **Datos de mercado** – almacenamiento de datos.
- **Formato de almacenamiento** – formato de almacenamiento.
- **Tipo de datos** – tipo de datos.
- **Marco temporal** – usar velas con el marco temporal especificado.
- **Volumen máximo de cotización en la profundidad generada** – volumen máximo de cotización en el libro de órdenes generado.
- **Intervalo** – intervalo de tiempo.
- **P/L no realizado** – intervalo de recálculo del beneficio no realizado.
- **Operaciones** – qué operaciones usar.
- **Profundidad de mercado** – qué libros de órdenes usar.
- **Registro de órdenes** – usar el registro de órdenes.
- **Número de estrategias** – número de estrategias probadas simultáneamente.
- **Nivel de registro** – nivel de registro.
- **Combinar al tocar** – durante la emulación, combinar órdenes cuando el precio de la operación toca el precio de la orden (es decir, es igual al precio de la orden).
- **Profundidad de mercado (vida útil)** – tiempo máximo durante el cual el libro de órdenes está en el emulador. Si durante este tiempo no hubo actualización, el libro de órdenes se elimina. Esta propiedad puede usarse para eliminar libros de órdenes antiguos cuando hay huecos en los datos.
- **Porcentaje de errores** – valor porcentual del error al registrar nuevas órdenes. El valor puede ser de 0 (no habrá errores) a 100.
- **Latencia** – valor mínimo de latencia de la orden registrada.
- **Nuevo registro** – si se admite el nuevo registro de órdenes como una sola operación.
- **Periodo de búfer** – enviar respuestas en un único paquete por intervalos. Se emulan la latencia de red y la operación en búfer del núcleo del exchange.
- **ID de orden** – número desde el que el emulador generará identificadores de órdenes.
- **ID de operación** – número desde el que el emulador generará identificadores de operaciones.
- **Transacción** – número desde el que el emulador generará identificadores de transacciones de órdenes.
- **Tamaño del spread** – tamaño del spread en pasos de precio. Se usa al especificar el spread para generar un libro de órdenes a partir de operaciones tick.
- **Profundidad del libro** – profundidad máxima del libro de órdenes que se generará a partir de ticks.
- **Número de pasos de volumen** – número de pasos de volumen por los que la orden es mayor que la operación tick. Se usa para pruebas con operaciones tick.
- **Intervalo de carteras** – intervalo de recálculo de cartera. Si el intervalo es cero, no se realiza recálculo.
- **Cambiar hora** – cambiar la hora de órdenes y operaciones a hora del exchange.
- **Zona horaria** – información sobre la zona horaria donde se encuentra el exchange.
- **Desplazamiento de precio** – desplazamiento de precio desde la última operación, que especifica los límites de precios máximo y mínimo para la siguiente sesión.
- **Añadir volumen extra** – añadir volumen extra al libro de órdenes al registrar órdenes de gran volumen.
- **[Comisiones](../commissions.md)** – comisión (corretaje, bolsa, etc.).

**Registro**

- **Nivel de registro** – nivel de registro para este elemento.

**Configuración**

- **[Gestión de riesgos](../risk_management.md)** – configuración de gestión de riesgos.

**Parámetros del diagrama**

- **Instrumento** - instrumento.
- **Cartera** - cartera.

Si no rellena los **Parámetros del diagrama**, durante la emulación se usará el instrumento del campo **Instrumento** de la pestaña **Emulación**, y como cartera se usará por defecto la cartera de prueba.

## Contenido recomendado

[Gráfico](chart.md)
