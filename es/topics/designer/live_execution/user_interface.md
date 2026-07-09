# Interfaz

Después de añadir una estrategia a la carpeta **En vivo**, al hacer doble clic en la estrategia añadida se abrirá una pestaña titulada "Live [Nombre de la estrategia]". Al navegar a esta pestaña, la pestaña **En vivo** se abrirá automáticamente en la **Cinta**. En la pestaña **En vivo**, puede especificar el instrumento y la cartera con los que trabajará la estrategia. Al pulsar el botón **Iniciar**, se inicia el trading en vivo para la estrategia; al pulsar el botón **Detener**, se detiene.

![Designer Interface Live trade 00](../../../images/designer_interface_live_trade_00.png)

La pestaña de la estrategia contiene el Diseñador de estrategias para esquemas y elementos componentes, similar al descrito en [Diseñador de estrategias](../strategies/using_visual_designer/diagram_panel.md). Además, la pestaña incluye el panel [Propiedades de trading en vivo](../user_interface/components/live_settings.md), que de forma predeterminada está contraído y fijado al lado derecho de la pestaña.

Añadir una estrategia a **En vivo** implica copiarla desde el código original (en el caso de usar [esquemas](../strategies/using_visual_designer.md) o [código](../strategies/using_code.md)). Por lo tanto, los cambios en el algoritmo dentro de la copia **En vivo** no afectan al original. Al iniciar la estrategia, si hay una discrepancia entre **En vivo** y el original, se mostrará una advertencia:

![Designer Interface Live trade 01](../../../images/designer_interface_live_trade_01.png)

- **Sí** significa aplicar los cambios del original a la copia **en vivo**.
- **No** significa ignorar la diferencia e iniciar la copia **en vivo** sin aplicar cambios.
- **Cancelar** significa no iniciar nada.

Los cambios en la copia **En vivo** deben ser mínimos, orientados a pruebas y a su posterior transferencia al original. De lo contrario, existe el riesgo de perder cambios si la copia **En vivo** se actualiza a la versión del original.

## Véase también

[Configuración de conexión](../connections_settings.md)
