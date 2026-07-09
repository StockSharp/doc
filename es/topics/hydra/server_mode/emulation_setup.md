# Configuración de emulación

En modo servidor, el programa permite habilitar el modo de emulación.

![hydra emulator start](../../../images/hydra_emulator_start.png)

En modo de emulación, el programa [Hydra](../../hydra.md) permite realizar las siguientes funciones:

- El programa permite configurar las claves para conectarse a la fuente y trabajar simultáneamente con una conexión en distintos programas ([Designer](../../designer.md), [Terminal](../../terminal.md)).
- Si la fuente de datos de mercado permite descargar datos históricos, estos pueden usarse simultáneamente para pruebas.
- Si la fuente permite recibir datos en tiempo real, el modo de emulación permite emular el modo de negociación. En este modo, los datos sobre las acciones del usuario (registro de órdenes, operaciones) se transfieren directamente a Hydra, mientras que las acciones se registran por separado para cada programa. Por ejemplo, al registrar una orden en Terminal, sus cambios serán visibles solo allí, y en Designer no se registrarán. Esto evita conflictos entre dos programas que funcionan sobre la misma conexión.
- ¡IMPORTANTE\! Las operaciones realizadas en modo de emulación, la negociación y las operaciones sobre ellas se emulan en tiempo real; cuando el modo está desactivado, las acciones se realizarán en negociación real.

Este modo se utiliza al [probar estrategias](../../shell/user_interface/emulation.md).

## Configuración de emulación.

![hydra emulator prop](../../../images/hydra_emulator_prop.png)

- **Match on touch** - al emular la coincidencia de operaciones, hacer coincidir órdenes cuando el precio de la operación sea igual al precio de la orden.
- **Order book (time in force)** - período máximo de vigencia del libro de órdenes en el emulador. Si el libro de órdenes no se ha actualizado durante el período especificado, su valor se borra. Se usa para eliminar datos antiguos del libro de órdenes si hay huecos en los datos.
- **Percentage of errors** - porcentaje de errores al registrar nuevas órdenes (de 0 a 100).
- **Latency** - latencia mínima de las órdenes registradas.
- **Re-registration** - indica si se admitirá la reinscripción de órdenes como una única transacción.
- **Buffering period** - parámetro responsable del período de envío de paquetes completos para emular la latencia de red y almacenar en búfer el trabajo del núcleo de la bolsa.
- **Order ID** - número con el que el emulador generará identificadores de órdenes.
- **Trade identifier** - número con el que el emulador generará identificadores de operaciones.
- **Transaction** - número con el que el emulador generará identificadores de transacciones de órdenes.
- **Spread size** - tamaño del spread en pasos de precio. Se usa para determinar el spread al generar el libro de órdenes a partir de operaciones tick.
- **Order book depth** - profundidad máxima del libro de órdenes generado por ticks.
- **Number of volume steps** - número de pasos de volumen por los que la orden es mayor que la operación tick. Se usa en pruebas sobre operaciones tick.
- **Portfolio interval** - intervalo para recalcular datos de carteras. Si el intervalo es 0, no se realiza recálculo.
- **Adjust time** - ajustar la hora de órdenes y operaciones a la hora de la bolsa.
- **Zona horaria** - información sobre la zona horaria donde se encuentra la bolsa.
- **Price shift** - desplazamiento de precio respecto a la última operación, que determina los límites de precios máximo y mínimo para la siguiente sesión.
- **Add additional volume** - añadir volumen adicional al libro de órdenes al registrar órdenes con gran volumen.
- **Trading session state** - comprobación del estado de negociación.
- **Money** - comprobar el saldo monetario.
- **Short** - posibilidad de abrir posiciones cortas.
- **Storage** - almacenamiento.
