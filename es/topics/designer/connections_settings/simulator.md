# Simulador

[Designer](../../designer.md) permite ejecutar las estrategias creadas en modo **Simulación**. Para configurar la **Simulación**, se deben realizar las siguientes acciones:

1. Al hacer clic en la flecha junto al botón **Conectar** ![Designer The quick access toolbar 00](../../../images/designer_quick_access_toolbar_00.png), aparece el botón **Configuración del emulador**:

![Designer The connection settings 00](../../../images/designer_connection_settings_00.png)

2. Al hacer clic en el botón **Configuración del emulador**, se abre la ventana **Configuración del emulador**:

![Designer Properties emulation 00](../../../images/designer_properties_emulation_00.png)

1. **Simulador**

- **Usar emulador** – usar el emulador.
- **Instrumentos** – instrumentos.

2. **Configuración**

- **Combinar al tocar** - durante la emulación, combinar operaciones cuando el precio de la operación toca el precio de la orden (es decir, es igual al precio de la orden).
- **Profundidad de mercado (vida útil)** - tiempo máximo durante el cual el libro de órdenes está en el emulador. Si durante este tiempo no hubo actualización, el libro de órdenes se borra. Esta propiedad puede usarse para eliminar libros de órdenes antiguos cuando hay huecos en los datos.
- **Porcentaje de errores** - valor porcentual del error al registrar nuevas órdenes. El valor puede ir de 0 (sin errores) a 100.
- **Latencia** - valor mínimo de retraso para órdenes registradas.
- **Nuevo registro** - indica si se admite el nuevo registro de órdenes en forma de una única operación.
- **Periodo de búfer** - enviar respuestas por lotes en un único paquete. Se emulan el retraso de red y el trabajo en búfer del núcleo del exchange.
- **ID de orden** - número a partir del cual el emulador generará identificadores de órdenes.
- **ID de operación** - número a partir del cual el emulador generará identificadores de operaciones.
- **Transacción** - número a partir del cual el emulador generará identificadores de transacciones de órdenes.
- **Tamaño del spread** - tamaño del spread en incrementos de precio. Se usa al determinar el spread para generar el libro de órdenes a partir de operaciones tick.
- **Profundidad del libro** - profundidad máxima del libro de órdenes que se generará a partir de ticks.
- **Número de pasos de volumen** - número de pasos de volumen por los que la orden es mayor que la operación tick. Se usa al probar operaciones tick.
- **Intervalo de carteras** - intervalo de recálculo de cartera. Si el intervalo es igual a cero, no se realiza el recálculo.
- **Cambiar hora** - cambiar la hora de órdenes y operaciones por la hora del exchange.
- **Zona horaria** - información sobre la zona horaria del exchange.
- **Desplazamiento de precio** - desplazamiento de precio desde la última operación, que determina los límites de precios máximo y mínimo para la siguiente sesión.
- **Añadir volumen extra** - añadir volumen extra al libro de órdenes al registrar órdenes de gran volumen.

## Contenido recomendado

[Gráfico](../user_interface/components/chart.md)
