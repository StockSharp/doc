# Simulador

[Designer](../../designer.md) permite ejecutar las estrategias creadas en modo **Simulation**. Para configurar la **Simulation**, se deben realizar las siguientes acciones:

1. Al hacer clic en la flecha junto al botón **Connect** ![Designer The quick access toolbar 00](../../../images/designer_quick_access_toolbar_00.png), aparece el botón **Emulator settings**:

![Designer The connection settings 00](../../../images/designer_connection_settings_00.png)

2. Al hacer clic en el botón **Emulator settings**, se abre la ventana **Emulator settings**:

![Designer Properties emulation 00](../../../images/designer_properties_emulation_00.png)

1. **Simulator**

- **Use emulator** – usar el emulador.
- **Instruments** – instrumentos.

2. **Settings**

- **Combine on touch** - durante la emulación, combinar operaciones cuando el precio de la operación toca el precio de la orden (es decir, es igual al precio de la orden).
- **Market depth (lifetime)** - tiempo máximo durante el cual el libro de órdenes está en el emulador. Si durante este tiempo no hubo actualización, el libro de órdenes se borra. Esta propiedad puede usarse para eliminar libros de órdenes antiguos cuando hay huecos en los datos.
- **Errors percentage** - valor porcentual del error al registrar nuevas órdenes. El valor puede ir de 0 (sin errores) a 100.
- **Latency** - valor mínimo de retraso para órdenes registradas.
- **Reregistering** - indica si se admite el nuevo registro de órdenes en forma de una única operación.
- **Buffering period** - enviar respuestas por lotes en un único paquete. Se emulan el retraso de red y el trabajo en búfer del núcleo del exchange.
- **Order ID** - número a partir del cual el emulador generará identificadores de órdenes.
- **Trade ID** - número a partir del cual el emulador generará identificadores de operaciones.
- **Transaction** - número a partir del cual el emulador generará identificadores de transacciones de órdenes.
- **Spread size** - tamaño del spread en incrementos de precio. Se usa al determinar el spread para generar el libro de órdenes a partir de operaciones tick.
- **Depth of book** - profundidad máxima del libro de órdenes que se generará a partir de ticks.
- **Number of volume steps** - número de pasos de volumen por los que la orden es mayor que la operación tick. Se usa al probar operaciones tick.
- **Portfolios interval** - intervalo de recálculo de cartera. Si el intervalo es igual a cero, no se realiza el recálculo.
- **Change time** - cambiar la hora de órdenes y operaciones por la hora del exchange.
- **Time zone** - información sobre la zona horaria del exchange.
- **Price shift** - desplazamiento de precio desde la última operación, que determina los límites de precios máximo y mínimo para la siguiente sesión.
- **Add extra volume** - añadir volumen extra al libro de órdenes al registrar órdenes de gran volumen.

## Contenido recomendado

[Gráfico](../user_interface/components/chart.md)
