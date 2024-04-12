# Панель управления

Сервис управления торговыми стратегиями и роботами через Telegram бота.

Для настройки предварительно пройдите процесс [авторизации у бота](authorization.md).

После этого бот готов к использованию. Далее, чтобы бот начал видеть ваши стратегии, вам необходимо:

- В случае использования программы [Designer](../designer.md), включить на панели Облако режим **Удаленное**:

  ![DesignerRibbon.png](../../images/designerribbon.png)

  Все стратегии, которые запущены в режиме Live, автоматически будут переданы в телеграм бот, и вы сможете управлять ими с телефона.
  
  В боте [StockSharpBot](https://t.me/StockSharpBot) выбрав команду /apps можно увидеть список всех программ:

  ![TelegramControlApps.png](../../images/telegramcontrolapps.png)

  Выбран нужную программу можно увидеть стратегии и элементы управления ими:

  ![TelegramControlApp.png](../../images/telegramcontrolapp.png)

  ![TelegramControlStrategies.png](../../images/telegramcontrolstrategies.png)

  ![TelegramControlStrategy.png](../../images/telegramcontrolstrategy.png)

- В случае использования [Shell](../shell.md), вам необходимо перейти в панель **Remote Manager** и сделать настройки, аналогичные в [Designer](../designer.md).
- В случае использования [Hydra](../hydra.md), все действия делаются аналогично [Designer](../designer.md). Интеграция с [Hydra](../hydra.md) позволяет управлять скачиванием маркет-данных, отслеживать количественную статистику.

  ![TelegramHydra.png](../../images/telegramhydra.png)
  ![TelegramHydraStat.png](../../images/telegramhydrastat.png)

- В случае использования [S\#](../api.md), вы можете сделать интеграцию, используя код из [Shell](../shell.md). Благодаря тому, что [S\#](../api.md) является кросс-платформенной, ваши роботы могут быть запущены на любой операционной системе.