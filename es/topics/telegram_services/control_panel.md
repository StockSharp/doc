# Panel de control

Servicio para gestionar estrategias y robots de negociación mediante un bot de Telegram.

Para la configuración, complete previamente el [proceso de autorización del bot](authorization.md).

Después de esto, el bot está listo para usarse. A continuación, para que el bot empiece a ver sus estrategias, debe:

 - Al usar el programa [Designer](../designer.md), habilitar el modo remoto en el panel Nube:

  ![Panel de control 01](../../images/designerribbon.png)

  Todas las estrategias que se ejecutan en modo en vivo se transferirán automáticamente al bot de Telegram y podrá controlarlas desde su teléfono.

  En [StockSharpBot](https://t.me/StockSharpBot), seleccione el comando /apps para ver una lista de todos los programas:

  ![Panel de control 02](../../images/telegramcontrolapps.png)

  Después de seleccionar el programa deseado, podrá ver las estrategias y sus controles:

  ![Panel de control 03](../../images/telegramcontrolapp.png)

  ![Panel de control 04](../../images/telegramcontrolstrategies.png)

  ![Panel de control 05](../../images/telegramcontrolstrategy.png)

 - Al usar [Shell](../shell.md), active en la barra de comandos de la ventana principal el interruptor **Remotamente** y permita el control remoto en la configuración de las estrategias necesarias. Se describe con detalle en la sección [Control remoto](../shell/remote_control.md).
 - Al usar [Hydra](../hydra.md), realice acciones similares a [Designer](../designer.md). La integración con [Hydra](../hydra.md) permite gestionar la descarga de datos de mercado y monitorizar estadísticas cuantitativas.

  ![Panel de control 06](../../images/telegramhydra.png)
  ![Panel de control 07](../../images/telegramhydrastat.png)

 - Al usar [S#](../api.md), puede integrarlo usando el código de [Shell](../shell.md). Gracias a que [S#](../api.md) es multiplataforma, sus robots pueden ejecutarse en cualquier sistema operativo.
