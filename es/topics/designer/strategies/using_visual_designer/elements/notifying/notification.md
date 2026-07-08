# Notificación

![Designer Notice 00](../../../../../../images/designer_notice_00.png)

El cubo envía una notificación cuando llegan datos a su socket de entrada. El valor entrante se convierte en texto mediante `ToString`. Puede conectar una [Variable](../data_sources/variable.md) para enviar texto fijo, adjuntar flujos de operaciones o velas para ver sus detalles, o usar los cubos [Formato de cadena](string_format.md) y [Concatenación de cadenas](string_concat.md) para preparar un mensaje personalizado.

### Sockets de entrada

Sockets de entrada

- **Message** - datos que se enviarán. Se acepta cualquier valor y se convierte en cadena.

### Parámetros

Parámetros

- **Type** - tipo de mensaje (ventana emergente, e-mail, sms, etc.). Los tipos de notificaciones se describen en la sección [Configuración de notificaciones](../../../../../terminal/notifications.md).
- **Telegram** - canal usado para notificaciones de Telegram.
- **Header** - encabezado del mensaje.

## Contenido recomendado

[Formato de cadena](string_format.md)
[Concatenación de cadenas](string_concat.md)
[Configuración de notificaciones](../../../../../terminal/notifications.md)
