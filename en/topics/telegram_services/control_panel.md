# Control Panel

A service for managing trading strategies and robots via a Telegram bot.

For setup, go through the [bot authorization process](authorization.md) beforehand.

After that, the bot is ready for use. Next, for the bot to start seeing your strategies, you need to:

 - When using the [Designer](../designer.md) program, enable Remote mode in the Cloud panel:

  ![DesignerRibbon.png](../../images/designerribbon.png)

  All strategies that run in Live mode will automatically be transferred to the Telegram bot, and you will be able to control them from your phone.

  In [StockSharpBot](https://t.me/StockSharpBot) select the /apps command to see a list of all programs:

  ![TelegramControlApps.png](../../images/telegramcontrolapps.png)

  Once the desired program is selected, you can see the strategies and their controls:

  ![TelegramControlApp.png](../../images/telegramcontrolapp.png)

  ![TelegramControlStrategies.png](../../images/telegramcontrolstrategies.png)

  ![TelegramControlStrategy.png](../../images/telegramcontrolstrategy.png)

 - When using [Shell](../shell.md), go to the Remote Manager panel and configure settings similar to [Designer](../designer.md).
 - When using [Hydra](../hydra.md), perform actions similar to [Designer](../designer.md). Integration with [Hydra](../hydra.md) allows you to manage the downloading of market data and monitor quantitative statistics.

  ![TelegramHydra.png](../../images/telegramhydra.png)
  ![TelegramHydraStat.png](../../images/telegramhydrastat.png)

 - When using [S#](../api.md), you can integrate using the code from [Shell](../shell.md). Thanks to [S#](../api.md) being cross-platform, your robots can run on any operating system.
