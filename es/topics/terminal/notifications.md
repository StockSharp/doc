# Notificaciones

[Terminal](../terminal.md) informa de los eventos importantes con ventanas emergentes en la esquina de la pantalla: conexión establecida o interrumpida, operación propia realizada, orden que no se ha podido cancelar. Un error de conexión se muestra en una ventana de advertencia.

Terminal también puede informar de otras maneras sobre los eventos de las órdenes y de la conexión. Se eligen en la ventana [Configuraciones](settings/common_settings.md) (**Configuraciones → Configuraciones**), en el grupo **Notificación**, para cada evento por separado:

- **Cambio de orden**: la orden ha cambiado;
- **Emparejamiento**: la orden se ha ejecutado;
- **Error**: la orden no se ha aceptado;
- **Perdido** y **Restaurado**: la conexión se ha perdido y se ha restablecido.

Para cada evento se puede elegir cómo informar de él:

- **Sonido** - se reproducirá una señal sonora.
- **Ventana emergente** - aparecerá una pequeña ventana con un mensaje en la esquina de la pantalla.
- **Archivo de registro** - el mensaje se escribirá en el registro, que se ve en el panel [Registros](user_interface/logs.md).
- **Telegram** - el mensaje se enviará al canal de Telegram que se indica en el mismo grupo de configuración. Los canales se toman de la cuenta de StockSharp, por lo que para este método hay que iniciar sesión en el sitio web.

Si no se elige ningún método, no se informa del evento.

## Véase también

[Configuración general](settings/common_settings.md)

[Menú principal](user_interface/ribbon.md)
