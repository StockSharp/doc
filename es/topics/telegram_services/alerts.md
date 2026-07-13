# Alertas

Servicio para enviar mensajes desde aplicaciones (como [Designer](../designer.md) o su propio programa personalizado) tanto a canales o grupos privados como públicos en Telegram.

Para configurar:

1. Complete el [proceso de autorización del bot](authorization.md).

2. Cree un canal o grupo (privado o público).

   ![Creación de un canal de Telegram](../../images/telegramchannelcreating.png)
   ![Selección del tipo de canal en Telegram](../../images/telegramchanneltype.png)

3. Añada el bot [StockSharpBot](https://t.me/StockSharpBot)

   ![Agregar el bot a Telegram](../../images/telegramaddbot.png)

4. Hágalo administrador.

   ![Configurar el bot como administrador en Telegram](../../images/telegrammakeadmin.png)

5. Permisos necesarios para el funcionamiento correcto.

   ![Permisos para el bot de Telegram](../../images/telegrambotpermissions.png)

6. Escriba la palabra especial **activate** en el canal o grupo.

   ![Activación del canal en Telegram](../../images/telegramchannelactivate.png)

7. Si todo va bien, recibirá una respuesta.

   ![Confirmación de activación del canal en Telegram](../../images/telegramchannelactivated.png)

El canal que ha creado ya está disponible para sus estrategias y robots de negociación:

  - Al usar [Designer](../designer.md), haga clic en la lista de canales en el panel superior:

  ![Captura de Alertas](../../images/designerribbonchannels.png)

  En la ventana que aparece, verá listas de todos los canales y grupos donde ha activado el bot:

  ![Lista de canales en Telegram](../../images/telegramlistchannels.png)

  Al pulsar el botón con el icono de Telegram se enviará un mensaje de prueba. Si se recibe, significa que todo está configurado correctamente.

  ![Envío de un mensaje de prueba desde Designer](../../images/designertestmessage.png)

  *En la tarifa gratuita se añade una línea mencionando el sitio StockSharp. En las tarifas de pago, esta línea se elimina.*

Si tiene varios canales de salida en Telegram y desea dirigir distintas estrategias a canales separados, puede especificar los canales para cada estrategia en la configuración:

![Configuración remota en Designer](../../images/designerremotesettings.png)

- En otros programas, la configuración se realiza de forma similar a [Designer](../designer.md). Por ejemplo, en el programa [Hydra](../hydra.md), puede configurar el registro de errores de descarga de datos de mercado si [Hydra](../hydra.md) se encuentra en un servidor y necesita recibir rápidamente información sobre una conexión no funcional.
- En el caso de [Shell](../shell.md) o [S#](../api.md), puede ver el código que integra sus estrategias con el servicio Telegram.
