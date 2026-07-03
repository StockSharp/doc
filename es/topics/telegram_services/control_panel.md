# Panel de control

Servicio para gestionar estrategias y robots de trading mediante un bot de Telegram.

Para la configuración, complete previamente el [proceso de autorización del bot](authorization.md).

Después de esto, el bot está listo para usarse. A continuación, para que el bot empiece a ver sus estrategias, debe:

 - Al usar el programa [Designer](../designer.md), habilitar el modo Remote en el panel Cloud:

  ![DesignerRibbon.png](../../images/designerribbon.png)

  Todas las estrategias que se ejecutan en modo Live se transferirán automáticamente al bot de Telegram y podrá controlarlas desde su teléfono.

  En [StockSharpBot](https://t.me/StockSharpBot), seleccione el comando /apps para ver una lista de todos los programas:

  ![TelegramControlApps.png](../../images/telegramcontrolapps.png)

  Después de seleccionar el programa deseado, podrá ver las estrategias y sus controles:

  ![TelegramControlApp.png](../../images/telegramcontrolapp.png)

  ![TelegramControlStrategies.png](../../images/telegramcontrolstrategies.png)

  ![TelegramControlStrategy.png](../../images/telegramcontrolstrategy.png)

 - Al usar [Shell](../shell.md), vaya al panel Remote Manager y configure los ajustes de forma similar a [Designer](../designer.md).
 - Al usar [Hydra](../hydra.md), realice acciones similares a [Designer](../designer.md). La integración con [Hydra](../hydra.md) permite gestionar la descarga de datos de mercado y monitorizar estadísticas cuantitativas.

  ![TelegramHydra.png](../../images/telegramhydra.png)
  ![TelegramHydraStat.png](../../images/telegramhydrastat.png)

 - Al usar [S#](../api.md), puede integrarlo usando el código de [Shell](../shell.md). Gracias a que [S#](../api.md) es multiplataforma, sus robots pueden ejecutarse en cualquier sistema operativo.
