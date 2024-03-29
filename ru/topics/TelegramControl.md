# Панель управления

Сервис управления торговыми стратегиями и роботами через Telegram бота.

Для настройки предварительно пройдите процесс [авторизации у бота](TelegramAuth.md).

После этого бот готов к использованию. Далее, чтобы бот начал видеть ваши стратегии, вам необходимо:

- В случае использования программы [Designer](Designer.md), включить на панели Облако режим **Удаленное**:

  ![DesignerRibbon.png](../images/DesignerRibbon.png)

  Все стратегии, которые запущены в режиме Live, автоматически будут переданы в телеграм бот, и вы сможете управлять ими с телефона.
  
  В боте [StockSharpBot](https://t.me/StockSharpBot) выбрав команду /apps можно увидеть список всех программ:

  ![TelegramControlApps.png](../images/TelegramControlApps.png)

  Выбран нужную программу можно увидеть стратегии и элементы управления ими:

  ![TelegramControlApp.png](../images/TelegramControlApp.png)

  ![TelegramControlStrategies.png](../images/TelegramControlStrategies.png)

  ![TelegramControlStrategy.png](../images/TelegramControlStrategy.png)

- В случае использования [Shell](Shell.md), вам необходимо перейти в панель **Remote Manager** и сделать настройки, аналогичные в [Designer](Designer.md).
- В случае использования [Hydra](Hydra.md), все действия делаются аналогично [Designer](Designer.md). Интеграция с [Hydra](Hydra.md) позволяет управлять скачиванием маркет-данных, отслеживать количественную статистику.

  ![TelegramHydra.png](../images/TelegramHydra.png)
  ![TelegramHydraStat.png](../images/TelegramHydraStat.png)

- В случае использования [S\#](StockSharpAbout.md), вы можете сделать интеграцию, используя код из [Shell](Shell.md). Благодаря тому, что [S\#](StockSharpAbout.md) является кросс-платформенной, ваши роботы могут быть запущены на любой операционной системе.